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
    /// 字典类型管理
    /// </summary>
    public class DictionaryTypeAppService : EasymanAppServiceBase, IDictionaryTypeAppService
    {
        #region 初始化

        private readonly IRepository<DictionaryType,long> _DictionaryTypeCase;
        /// <summary>
        /// 构造函数注入DictionaryType仓储
        /// </summary>
        /// <param name="DictionaryTypeCase"></param>
        public DictionaryTypeAppService(IRepository<DictionaryType, long> DictionaryTypeCase)
        {
            _DictionaryTypeCase = DictionaryTypeCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个字典类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DictionaryTypeModel GetDictionaryType(long id)
        {
            var entObj= _DictionaryTypeCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<DictionaryTypeModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增字典类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DictionaryTypeModel InsertOrUpdateDictionaryType(DictionaryTypeModel input)
        {
            if(_DictionaryTypeCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            var entObj =input.MapTo<DictionaryType>();
            //var entObj= AutoMapper.Mapper.Map<DictionaryType>(input);
            var resObj= _DictionaryTypeCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<DictionaryTypeModel>();
            }
        }

        /// <summary>
        /// 删除一条字典类型
        /// </summary>
        /// <param name="input"></param>
        public void DeleteDictionaryType(EntityDto<long> input)
        {
            try
            {
                _DictionaryTypeCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取字典类型json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetDictionaryTypeTreeJson()
        {
            var objList= _DictionaryTypeCase.GetAllList();
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
        public List<SelectListItem> DictionaryTypeList()
        {
            var objList = _DictionaryTypeCase.GetAllList();
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
