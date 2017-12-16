using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Common;
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
    /// 指标管理
    /// </summary>
    public class TargetAppService : EasymanAppServiceBase, ITargetAppService
    {
        #region 初始化

        private readonly IRepository<Target,long> _TargetCase;
        /// <summary>
        /// 构造函数注入Target仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public TargetAppService(IRepository<Target, long> TargetCase)
        {
            _TargetCase = TargetCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个指标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TargetModel GetTarget(long id)
        {
            var entObj= _TargetCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<TargetModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增指标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TargetModel InsertOrUpdateTarget(TargetModel input)
        {
            if(_TargetCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<Target>();
            var entObj = _TargetCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new Target();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<Target>(input);
            var resObj= _TargetCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<TargetModel>();
            }
        }

        /// <summary>
        /// 删除一条指标
        /// </summary>
        /// <param name="input"></param>
        public void DeleteTarget(EntityDto<long> input)
        {
            try
            {
                _TargetCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取指标json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetTargetTreeJson()
        {
            var objList= _TargetCase.GetAllList();
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
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> TargetList()
        {
            var objList = _TargetCase.GetAllList();
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

        public List<TargetModel> AllTargetList()
        {
            var objList = _TargetCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.MapTo<List<TargetModel>>();
            }
            return null;
        }
        #endregion
    }
}
