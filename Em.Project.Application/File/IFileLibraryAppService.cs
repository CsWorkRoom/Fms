using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 文件库管理
    /// </summary>
    public interface IFileLibraryAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个文件库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileLibraryModel GetFileLibrary(long id);
        /// <summary>
        /// 更新和新增文件库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        FileLibraryModel InsertOrUpdateFileLibrary(FileLibraryModel input);

        /// <summary>
        /// 删除一条文件库
        /// </summary>
        /// <param name="input"></param>
        void DeleteFileLibrary(EntityDto<long> input);
        /// <summary>
        /// 获取文件库json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetFileLibraryTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> FileLibraryList();

        /// <summary>
        /// 根据文件MD5值获取libirary
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        FileLibraryModel GetFileLibraryByMD5(string md5);
    }
}
