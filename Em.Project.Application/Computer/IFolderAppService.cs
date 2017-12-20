using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 终端共享文件夹管理
    /// </summary>
    public interface IFolderAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个终端共享文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FolderModel GetFolder(long id);
        /// <summary>
        /// 更新和新增终端共享文件夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        FolderModel InsertOrUpdateFolder(FolderModel input);

        /// <summary>
        /// 删除一条终端共享文件夹
        /// </summary>
        /// <param name="input"></param>
        void DeleteFolder(EntityDto<long> input);
        /// <summary>
        /// 获取终端共享文件夹json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetFolderTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> FolderList();
    }
}
