using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// FM_FOLDER_VERSION(更新版本批次)
    /// </summary>
    public interface IFolderVersionAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个文件夹及文件管理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FolderVersionModel GetFolderVersion(long id);
        /// <summary>
        /// 更新和新增文件夹及文件管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        FolderVersionModel InsertOrUpdateFolderVersion(FolderVersionModel input);

        /// <summary>
        /// 删除一条文件夹及文件管理
        /// </summary>
        /// <param name="input"></param>
        void DeleteFolderVersion(EntityDto<long> input);
        /// <summary>
        /// 获取文件夹及文件管理json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetFolderVersionTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> FolderVersionList();

        /// <summary>
        /// 根据共享文件夹获版本号查询文件信息
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        FolderVersionModel GetFolderVersionByFolder(long folderId);
    }
}
