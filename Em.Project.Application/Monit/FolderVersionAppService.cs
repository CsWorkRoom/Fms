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
    /// FM_FOLDER_VERSION(更新版本批次)
    /// </summary>
    public class FolderVersionAppService : EasymanAppServiceBase, IFolderVersionAppService
    {
        #region 初始化

        private readonly IRepository<FolderVersion,long> _FolderVersionCase;
        /// <summary>
        /// 构造函数注入FolderVersion仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public FolderVersionAppService(IRepository<FolderVersion, long> FolderVersionCase)
        {
            _FolderVersionCase = FolderVersionCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个版本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FolderVersionModel GetFolderVersion(long id)
        {
            var entObj= _FolderVersionCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<FolderVersionModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增文件库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FolderVersionModel InsertOrUpdateFolderVersion(FolderVersionModel input)
        {

            try
            {

                //var entObj =input.MapTo<FolderVersion>();
                var entObj = _FolderVersionCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new FolderVersion();
                entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
                //var entObj= AutoMapper.Mapper.Map<FolderVersion>(input);
                var id = _FolderVersionCase.InsertOrUpdateAndGetId(entObj);
                return entObj.MapTo<FolderVersionModel>();

            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        /// <summary>
        /// 删除一条文件库
        /// </summary>
        /// <param name="input"></param>
        public void DeleteFolderVersion(EntityDto<long> input)
        {
            try
            {
                _FolderVersionCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取版本json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetFolderVersionTreeJson()
        {
            var objList= _FolderVersionCase.GetAllList();
            if(objList!=null&& objList.Count>0)
            {
                return objList.Select(s => new
                {
                    id = s.Id,
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
        public List<SelectListItem> FolderVersionList()
        {
            var objList = _FolderVersionCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.Select(p => new SelectListItem
                {
                    Text = p.Id.ToString(),
                    Value = p.Id.ToString()
                }).ToList();
            }
            return null;
        }
        /// <summary>
        /// 根据文件ID获取文件版本
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public FolderVersionModel GetFolderVersionByFolder(long folderId)
        {
            var FolderVersion = _FolderVersionCase.GetAllList().OrderByDescending(p=>p.Id).FirstOrDefault(p => p.FolderId == folderId);
            if (FolderVersion != null)
                return FolderVersion.MapTo<FolderVersionModel>();
            else
                return null;

        }


        #endregion
    }
}
