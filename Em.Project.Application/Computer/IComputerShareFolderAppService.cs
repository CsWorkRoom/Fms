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
    public interface IComputerShareFolderAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个终端共享文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ComputerShareFolderModel GetComputerShareFolder(long id);
        /// <summary>
        /// 更新和新增终端共享文件夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ComputerShareFolderModel InsertOrUpdateComputerShareFolder(ComputerShareFolderModel input);

        /// <summary>
        /// 删除一条终端共享文件夹
        /// </summary>
        /// <param name="input"></param>
        void DeleteComputerShareFolder(EntityDto<long> input);
        /// <summary>
        /// 获取终端共享文件夹json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetComputerShareFolderTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> ComputerShareFolderList();
    }
}
