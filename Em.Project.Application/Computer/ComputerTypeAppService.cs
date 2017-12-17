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
    /// 终端类型管理
    /// </summary>
    public class ComputerTypeAppService : EasymanAppServiceBase, IComputerTypeAppService
    {
        #region 初始化

        private readonly IRepository<ComputerType,long> _ComputerTypeCase;
        /// <summary>
        /// 构造函数注入ComputerType仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public ComputerTypeAppService(IRepository<ComputerType, long> ComputerTypeCase)
        {
            _ComputerTypeCase = ComputerTypeCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个终端类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ComputerTypeModel GetComputerType(long id)
        {
            var entObj= _ComputerTypeCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<ComputerTypeModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增终端类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ComputerTypeModel InsertOrUpdateComputerType(ComputerTypeModel input)
        {
            if(_ComputerTypeCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<ComputerType>();
            var entObj = _ComputerTypeCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new ComputerType();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<ComputerType>(input);
            var resObj= _ComputerTypeCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<ComputerTypeModel>();
            }
        }

        /// <summary>
        /// 删除一条终端类型
        /// </summary>
        /// <param name="input"></param>
        public void DeleteComputerType(EntityDto<long> input)
        {
            try
            {
                _ComputerTypeCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取终端类型json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetComputerTypeTreeJson()
        {
            var objList= _ComputerTypeCase.GetAllList();
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
        public List<SelectListItem> ComputerTypeList()
        {
            var objList = _ComputerTypeCase.GetAllList();
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
