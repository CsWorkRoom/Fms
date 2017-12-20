using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 文件格式管理
    /// </summary>
    public interface IFileFormatAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个文件格式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileFormatModel GetFileFormat(long id);
        /// <summary>
        /// 更新和新增文件格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        FileFormatModel InsertOrUpdateFileFormat(FileFormatModel input);

        /// <summary>
        /// 删除一条文件格式
        /// </summary>
        /// <param name="input"></param>
        void DeleteFileFormat(EntityDto<long> input);
        /// <summary>
        /// 获取文件格式json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetFileFormatTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> FileFormatList();
    }
}
