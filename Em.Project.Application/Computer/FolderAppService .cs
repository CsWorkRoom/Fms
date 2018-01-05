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
    public class FolderAppService : EasymanAppServiceBase, IFolderAppService
    {
        #region 初始化

        private readonly IRepository<Folder,long> _FolderCase;
        /// <summary>
        /// 构造函数注入Folder仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public FolderAppService(IRepository<Folder, long> FolderCase)
        {
            _FolderCase = FolderCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个终端共享文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FolderModel GetFolder(long id)
        {
            var entObj= _FolderCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<FolderModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增终端共享文件夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FolderModel InsertOrUpdateFolder(FolderModel input)
        {
            if(_FolderCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<Folder>();
            var entObj = _FolderCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new Folder();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<Folder>(input);
            var resObj= _FolderCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<FolderModel>();
            }
        }

        /// <summary>
        /// 删除一条终端共享文件夹
        /// </summary>
        /// <param name="input"></param>
        public void DeleteFolder(EntityDto<long> input)
        {
            try
            {
                _FolderCase.Delete(input.Id);
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
        public IEnumerable<object> GetFolderTreeJson()
        {
            var objList= _FolderCase.GetAllList();
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
        public List<SelectListItem> FolderList()
        {
            var objList = _FolderCase.GetAllList();
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
        /// <summary>
        /// 根据终端编号获取共享目录
        /// </summary>
        /// <param name="computerId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<FolderModel> GetFolderListByComputer(long computerId)
        {
            return _FolderCase.GetAllList(p => p.ComputerId == computerId).MapTo<List<FolderModel>>();
        }
        /// <summary>
        /// 根据终端，文件夹名称获取共享文件夹
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public FolderModel GetFolderByComputerAndName(long cid, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var folder = _FolderCase.FirstOrDefault(p => p.ComputerId == cid && p.Name == name);
                if (folder != null)
                    return folder.MapTo<FolderModel>();
                else
                    return null;
            }
            else
                return null;
        }
            #endregion
        }
}
