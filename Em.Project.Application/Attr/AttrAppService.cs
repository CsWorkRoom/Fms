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
    /// 属性管理
    /// </summary>
    public class AttrAppService : EasymanAppServiceBase, IAttrAppService
    {
        #region 初始化

        private readonly IRepository<Attr,long> _AttrCase;
        /// <summary>
        /// 构造函数注入Attr仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public AttrAppService(IRepository<Attr, long> AttrCase)
        {
            _AttrCase = AttrCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AttrModel GetAttr(long id)
        {
            var entObj= _AttrCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<AttrModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public AttrModel InsertOrUpdateAttr(AttrModel input)
        {
            if (_AttrCase.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<Attr>();
            var entObj = _AttrCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new Attr();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<Attr>(input);
            var id = _AttrCase.InsertOrUpdateAndGetId(entObj);
            return entObj.MapTo<AttrModel>();
        }

        /// <summary>
        /// 删除一条属性
        /// </summary>
        /// <param name="input"></param>
        public void DeleteAttr(EntityDto<long> input)
        {
            try
            {
                _AttrCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取属性json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAttrTreeJson()
        {
            var objList= _AttrCase.GetAllList();
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
        public List<SelectListItem> AttrList()
        {
            var objList = _AttrCase.GetAllList();
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

        public AttrModel GetAttrByName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var Attr = _AttrCase.FirstOrDefault(p => p.Name == name.Trim());
                if (Attr != null)
                   // return Attr.MapTo<AttrModel>();
                return AutoMapper.Mapper.Map<AttrModel>(Attr);
                else
                    return null;
            }
            else
                return null;
        }
        #endregion
    }
}
