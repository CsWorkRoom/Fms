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
    /// 字典管理
    /// </summary>
    public class DictionaryAppService : EasymanAppServiceBase, IDictionaryAppService
    {
        #region 初始化

        private readonly IRepository<Dictionary,long> _DictionaryCase;
        /// <summary>
        /// 构造函数注入Dictionary仓储
        /// </summary>
        /// <param name="DictionaryCase"></param>
        public DictionaryAppService(IRepository<Dictionary, long> DictionaryCase)
        {
            _DictionaryCase = DictionaryCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DictionaryModel GetDictionary(long id)
        {
            var entObj= _DictionaryCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<DictionaryModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DictionaryModel InsertOrUpdateDictionary(DictionaryModel input)
        {
            if(_DictionaryCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            var entObj =input.MapTo<Dictionary>();
            //var entObj= AutoMapper.Mapper.Map<Dictionary>(input);
            var resObj= _DictionaryCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<DictionaryModel>();
            }
        }

        /// <summary>
        /// 删除一条字典
        /// </summary>
        /// <param name="input"></param>
        public void DeleteDictionary(EntityDto<long> input)
        {
            try
            {
                _DictionaryCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取字典json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetDictionaryTreeJson()
        {
            var objList= _DictionaryCase.GetAllList();
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
        /// 根据字典类型获取字典集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetDictionaryByTypeTreeJson(EntityDto<long> input)
        {
            var objList = _DictionaryCase.GetAllList(p => p.DictionaryTypeId == input.Id);//获取字典类型

            if (objList != null && objList.Count > 0)
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
        public IList<SelectListItem> DictionaryList()
        {
            var objList = _DictionaryCase.GetAllList();
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

        public List<DictionaryModel> AllDictionaryList()
        {
            var objList = _DictionaryCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.MapTo<List<DictionaryModel>>();
            }
            return null;
        }
        #endregion
    }
}
