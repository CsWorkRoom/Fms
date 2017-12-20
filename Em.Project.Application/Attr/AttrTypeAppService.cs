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
    /// 属性类型管理
    /// </summary>
    public class AttrTypeAppService : EasymanAppServiceBase, IAttrTypeAppService
    {
        #region 初始化

        private readonly IRepository<AttrType,long> _AttrTypeCase;
        /// <summary>
        /// 构造函数注入AttrType仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public AttrTypeAppService(IRepository<AttrType, long> AttrTypeCase)
        {
            _AttrTypeCase = AttrTypeCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个属性类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AttrTypeModel GetAttrType(long id)
        {
            var entObj= _AttrTypeCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<AttrTypeModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增属性类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public AttrTypeModel InsertOrUpdateAttrType(AttrTypeModel input)
        {
            if(_AttrTypeCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<AttrType>();
            var entObj = _AttrTypeCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new AttrType();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<AttrType>(input);
            var resObj= _AttrTypeCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<AttrTypeModel>();
            }
        }

        /// <summary>
        /// 删除一条属性类型
        /// </summary>
        /// <param name="input"></param>
        //[System.Web.Http.HttpPost]
        public void DeleteAttrType(EntityDto<long> input)
        {
            try
            {
                _AttrTypeCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取属性类型json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAttrTypeTreeJson()
        {
            var objList= _AttrTypeCase.GetAllList();
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
        public List<SelectListItem> AttrTypeList()
        {
            var objList = _AttrTypeCase.GetAllList();
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
