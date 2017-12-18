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
    /// 终端共享文件夹管理
    /// </summary>
    public class ComputerShareFolderAppService : EasymanAppServiceBase, IComputerShareFolderAppService
    {
        #region 初始化

        private readonly IRepository<ComputerFolder,long> _ComputerShareFolderCase;
        /// <summary>
        /// 构造函数注入ComputerShareFolder仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public ComputerShareFolderAppService(IRepository<ComputerFolder, long> ComputerShareFolderCase)
        {
            _ComputerShareFolderCase = ComputerShareFolderCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个终端共享文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ComputerShareFolderModel GetComputerShareFolder(long id)
        {
            var entObj= _ComputerShareFolderCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<ComputerShareFolderModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增终端共享文件夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ComputerShareFolderModel InsertOrUpdateComputerShareFolder(ComputerShareFolderModel input)
        {
            if(_ComputerShareFolderCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<ComputerShareFolder>();
            var entObj = _ComputerShareFolderCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new ComputerFolder();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<ComputerShareFolder>(input);
            var resObj= _ComputerShareFolderCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<ComputerShareFolderModel>();
            }
        }

        /// <summary>
        /// 删除一条终端共享文件夹
        /// </summary>
        /// <param name="input"></param>
        public void DeleteComputerShareFolder(EntityDto<long> input)
        {
            try
            {
                _ComputerShareFolderCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取终端共享文件夹json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetComputerShareFolderTreeJson()
        {
            var objList= _ComputerShareFolderCase.GetAllList();
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
        public List<SelectListItem> ComputerShareFolderList()
        {
            var objList = _ComputerShareFolderCase.GetAllList();
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
