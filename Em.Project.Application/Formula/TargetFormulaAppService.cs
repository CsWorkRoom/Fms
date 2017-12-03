using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 公式管理
    /// </summary>
    public class TargetFormulaAppService : EasymanAppServiceBase, ITargetFormulaAppService
    {
        #region 初始化

        private readonly IRepository<TargetFormula,long> _TargetFormulaCase;
        /// <summary>
        /// 构造函数注入TargetFormula仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public TargetFormulaAppService(IRepository<TargetFormula, long> TargetFormulaCase)
        {
            _TargetFormulaCase = TargetFormulaCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个公式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TargetFormulaModel GetTargetFormula(long id)
        {
            var entObj= _TargetFormulaCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<TargetFormulaModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增公式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TargetFormulaModel InsertOrUpdateTargetFormula(TargetFormulaModel input)
        {
            if(_TargetFormulaCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            var entObj =input.MapTo<TargetFormula>();
            //var entObj= AutoMapper.Mapper.Map<TargetFormula>(input);
            var resObj= _TargetFormulaCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<TargetFormulaModel>();
            }
        }

        /// <summary>
        /// 删除一条公式
        /// </summary>
        /// <param name="input"></param>
        public void DeleteTargetFormula(EntityDto<long> input)
        {
            try
            {
                _TargetFormulaCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }
        /// <summary>
        /// 获取公式json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetTargetFormulaTreeJson()
        {
            var objList= _TargetFormulaCase.GetAllList();
            if(objList!=null&& objList.Count>0)
            {
                return objList.Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    open = false,
                    iconSkin = "menu"
                }).ToList();
            }
            return null;
        }
        /// <summary>
        /// 获取所有标识List
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> TargetFormulaList()
        {
            var objList = _TargetFormulaCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();
            }
            return null;
        }
        #endregion
    }
}
