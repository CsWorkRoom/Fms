using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;
using Easyman.Users;

namespace Easyman.Service
{
    /// <summary>
    /// 文件管理
    /// </summary>
    public interface IFileAppService : IApplicationService
    {
        /// <summary>
        /// 获取当前目录的最新文件列表
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        List<MonitFileModel> GetCurFileListByFolder(long folderId);

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        User GetCurrentUser();
        [System.Web.Http.HttpGet]
        void CopyFile(string fromPath, string toPath);
    }
}
