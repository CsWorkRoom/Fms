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
    /// 终端管理
    /// </summary>
    public class ComputerAppService : EasymanAppServiceBase, IComputerAppService
    {
        #region 初始化

        private readonly IRepository<Computer,long> _ComputerCase;
        /// <summary>
        /// 构造函数注入Computer仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public ComputerAppService(IRepository<Computer, long> ComputerCase)
        {
            _ComputerCase = ComputerCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个终端
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ComputerModel GetComputer(long id)
        {
            var entObj= _ComputerCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<ComputerModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增终端
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ComputerModel InsertOrUpdateComputer(ComputerModel input)
        {
            if(_ComputerCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<Computer>();
            var entObj = _ComputerCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new Computer();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<Computer>(input);
            var resObj= _ComputerCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<ComputerModel>();
            }
        }

        /// <summary>
        /// 删除一条终端
        /// </summary>
        /// <param name="input"></param>
        public void DeleteComputer(EntityDto<long> input)
        {
            try
            {
                _ComputerCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取终端json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetComputerTreeJson()
        {
            var objList= _ComputerCase.GetAllList();
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
        public List<SelectListItem> ComputerList()
        {
            var objList = _ComputerCase.GetAllList();
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
