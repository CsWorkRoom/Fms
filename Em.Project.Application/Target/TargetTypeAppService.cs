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
    /// 指标类型管理
    /// </summary>
    public class TargetTypeAppService : EasymanAppServiceBase, ITargetTypeAppService
    {
        #region 初始化

        private readonly IRepository<TargetType,long> _TargetTypeCase;
        /// <summary>
        /// 构造函数注入TargetType仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public TargetTypeAppService(IRepository<TargetType, long> TargetTypeCase)
        {
            _TargetTypeCase = TargetTypeCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个指标类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TargetTypeModel GetTargetType(long id)
        {
            var entObj= _TargetTypeCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<TargetTypeModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增指标类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TargetTypeModel InsertOrUpdateTargetType(TargetTypeModel input)
        {
            if(_TargetTypeCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<TargetType>();
            var entObj = _TargetTypeCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new TargetType();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<TargetType>(input);
            var resObj= _TargetTypeCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<TargetTypeModel>();
            }
        }

        /// <summary>
        /// 删除一条指标类型
        /// </summary>
        /// <param name="input"></param>
        public void DeleteTargetType(EntityDto<long> input)
        {
            try
            {
                _TargetTypeCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取指标类型json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetTargetTypeTreeJson()
        {
            var objList= _TargetTypeCase.GetAllList();
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
        public List<SelectListItem> TargetTypeList()
        {
            var objList = _TargetTypeCase.GetAllList();
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
