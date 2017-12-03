using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 指标目标
    /// </summary>
    public class TargetValueAppService : EasymanAppServiceBase, ITargetValueAppService
    {
        #region 初始化

        private readonly IRepository<TargetValue,long> _TargetValueCase;
        private readonly IRepository<Target,long> _TargetCase;
        /// <summary>
        /// 构造函数注入仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public TargetValueAppService(IRepository<Target, long> TargetCase,
            IRepository<TargetValue, long> TargetValueCase)
        {
            _TargetCase = TargetCase;
            _TargetValueCase = TargetValueCase;
        }
        #endregion

        #region
        /// <summary>
        /// 根据指标标识和组织获取对应的指标目标列表
        /// </summary>
        /// <param name="targetTagId">指标标识</param>
        /// <param name="districtId">组织 1=默认获取市设定的区县指标,其他=区县设定的客户经理指标值</param>
        /// <returns></returns>
        public List<TargetValueModel> GetTargetValueList(long targetTagId, long districtId)
        {
            #region 基础数据准备
            //查询当前配置有效指标列表Target
            var targetList = _TargetCase.GetAllList(p => p.TargetTagId == targetTagId && p.IsUse);
            //获取当前已配置的目标值列表TargetValueModel
            var targetValueList = _TargetValueCase.GetAllList(p => p.Target.TargetTagId == targetTagId && p.Target.IsUse
             && p.DistrictId == districtId && p.IsUse).Select(p =>
             {
                 var tv = new TargetValueModel
                 {
                     Id = p.Id,
                     TargetTypeName = p.Target.TargetType.Name,
                     TargetName = p.Target.Name,
                     ChooseType = p.Target.ChooseType,
                     TargetWeight = p.Target.Weight,
                     TargetRemark = p.Target.Remark,
                     TargetId = p.TargetId,
                     DistrictId = p.DistrictId,
                     TValue = p.TValue,
                     ScoreWeight = p.ScoreWeight ?? (districtId == 1 ? 1 : 0.5),
                     IsUse = p.IsUse
                 };
                 return tv;
             }).ToList();
            #endregion

            #region 把以上数据组装成一个新的指标值列表 targetValueList（已配置+未配置）
            //待添加的列表Target
            var addList = targetList.Where(p => !targetValueList.Select(x => x.TargetId).Contains(p.Id)).ToList();
            foreach (var ad in addList)
            {
                var targetVal = new TargetValueModel
                {
                    Id = 0,
                    ChooseType = ad.ChooseType,
                    IsUse = true,
                    DistrictId = districtId,
                    TargetId = ad.Id,
                    TargetName = ad.Name,
                    TargetRemark = ad.Remark,
                    TargetTypeName = ad.TargetType.Name,
                    TargetWeight = ad.Weight,
                    ScoreWeight = (districtId == 1 ? 1 : 0.5),
                    TValue = 0
                };
                targetValueList.Add(targetVal);
            }
            #endregion

            return targetValueList;
        }

        /// <summary>
        /// 更新保存设定的指标
        /// </summary>
        /// <param name="targetVals">targetVals的指标集合json</param>
        /// <returns></returns>
        [UnitOfWork]
        public void SaveTargetValueList(string targetVals)
        {
            List<TargetValueModel> targetValueList = JSON.EncodeToEntity<List<TargetValueModel>>(targetVals);
            if (targetValueList != null && targetValueList.Count > 0)
            {
                var targetVlist = targetValueList.MapTo<List<TargetValue>>();
                foreach (var targetVal in targetVlist)
                {
                    var tValue = _TargetValueCase.FirstOrDefault(p => p.TargetId == targetVal.Id && p.DistrictId == targetVal.Id);
                    if (tValue == null)
                    {
                        //targetVal.IsUse = true;
                        _TargetValueCase.InsertOrUpdateAsync(targetVal);
                    }
                    else
                    {
                        tValue.TValue = targetVal.TValue;
                        tValue.IsUse = targetVal.IsUse;
                        _TargetValueCase.UpdateAsync(tValue);
                    }
                }
                //CurrentUnitOfWork.SaveChanges();
            }
        }
        #endregion

    }
}
