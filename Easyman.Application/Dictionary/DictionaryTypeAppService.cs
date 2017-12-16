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
using Easyman.Common;

namespace Easyman.Service
{
    /// <summary>
    /// 字典类型管理
    /// </summary>
    public class DictionaryTypeAppService : EasymanAppServiceBase, IDictionaryTypeAppService
    {
        #region 初始化

        private readonly IRepository<DictionaryType,long> _DictionaryTypeCase;
        private readonly IRepository<Dictionary, long> _DictionaryCase;
        /// <summary>
        /// 构造函数注入DictionaryType仓储
        /// </summary>
        /// <param name="DictionaryTypeCase"></param>
        public DictionaryTypeAppService(IRepository<DictionaryType, long> DictionaryTypeCase, IRepository<Dictionary, long> DictionaryCase)
        {
            _DictionaryTypeCase = DictionaryTypeCase;
            _DictionaryCase = DictionaryCase;
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
           
            if (_DictionaryTypeCase.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //使用MapTo和 Map会把没有传值过来的字段清空
            //var entObj = AutoMapper.Mapper.Map<DictionaryType>(input);
            //var entObj = input.MapTo<DictionaryType>();
            //使用框架方法Fun.ClassToCopy,复制一个类到另一个类进行赋值备注：需要排除ID（主键）
            var type = _DictionaryTypeCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new DictionaryType();
            type = Fun.ClassToCopy(input, type, (new string[] {"Id" }).ToList());
            var res=  _DictionaryTypeCase.InsertOrUpdate(type);
            if (res == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return res.MapTo<DictionaryTypeModel>();
            }


        }

        /// <summary>
        /// 删除一条字典类型
        /// </summary>
        /// <param name="input"></param>
        public void DeleteDictionaryType(EntityDto<long> input)
        {
           
            
                var type = _DictionaryTypeCase.Get(input.Id);
                if (type == null)
                {
                    throw new UserFriendlyException("操作出错，对象或已被删除！");
                }
                var content = _DictionaryCase.GetAll().Count(a => a.DictionaryTypeId == input.Id);
                if (content > 0)
                {
                    throw new UserFriendlyException("删除出错，字典类型正在被使用，请先删除字典，在执行此删除操作！");

                }
                else
                {
                    _DictionaryTypeCase.Delete(type);

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
