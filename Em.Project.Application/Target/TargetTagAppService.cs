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
    /// 指标标识管理
    /// </summary>
    public class TargetTagAppService : EasymanAppServiceBase, ITargetTagAppService
    {
        #region 初始化

        private readonly IRepository<TargetTag,long> _TargetTagCase;
        /// <summary>
        /// 构造函数注入TargetTag仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public TargetTagAppService(IRepository<TargetTag, long> TargetTagCase)
        {
            _TargetTagCase = TargetTagCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个指标标识
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TargetTagModel GetTargetTag(long id)
        {
            var entObj= _TargetTagCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<TargetTagModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增指标标识
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TargetTagModel InsertOrUpdateTargetTag(TargetTagModel input)
        {
            if(_TargetTagCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            var entObj =input.MapTo<TargetTag>();
            //var entObj= AutoMapper.Mapper.Map<TargetTag>(input);
            var resObj= _TargetTagCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<TargetTagModel>();
            }
        }

        /// <summary>
        /// 删除一条指标标识
        /// </summary>
        /// <param name="input"></param>
        public void DeleteTargetTag(EntityDto<long> input)
        {
            try
            {
                _TargetTagCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取指标标识json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetTargetTagTreeJson()
        {
            var objList= _TargetTagCase.GetAllList();
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
        public List<SelectListItem> TargetTagList()
        {
            var objList = _TargetTagCase.GetAllList();
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
