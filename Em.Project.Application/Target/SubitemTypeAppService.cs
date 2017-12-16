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
    /// 打分类型管理
    /// </summary>
    public class SubitemTypeAppService : EasymanAppServiceBase, ISubitemTypeAppService
    {
        #region 初始化

        private readonly IRepository<SubitemType,long> _SubitemTypeCase;
        /// <summary>
        /// 构造函数注入SubitemType仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public SubitemTypeAppService(IRepository<SubitemType, long> SubitemTypeCase)
        {
            _SubitemTypeCase = SubitemTypeCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个打分类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubitemTypeModel GetSubitemType(long id)
        {
            var entObj= _SubitemTypeCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<SubitemTypeModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！可能已被删除");
        }
        /// <summary>
        /// 更新和新增打分类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public SubitemTypeModel InsertOrUpdateSubitemType(SubitemTypeModel input)
        {
            if(_SubitemTypeCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<SubitemType>();
            var entObj = _SubitemTypeCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new SubitemType();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<SubitemType>(input);
            var resObj= _SubitemTypeCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<SubitemTypeModel>();
            }
        }

        /// <summary>
        /// 删除一条打分类型
        /// </summary>
        /// <param name="input"></param>
        public void DeleteSubitemType(EntityDto<long> input)
        {
            try
            {
                _SubitemTypeCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取打分类型json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetSubitemTypeTreeJson()
        {
            var objList= _SubitemTypeCase.GetAllList();
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
        public List<SelectListItem> SubitemTypeList()
        {
            var objList = _SubitemTypeCase.GetAllList();
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
