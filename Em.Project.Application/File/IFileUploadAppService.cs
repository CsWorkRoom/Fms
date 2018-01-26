using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 文件上传管理
    /// </summary>
    public interface IFileUploadAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个文件上传
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileUploadModel GetFileUpload(long id);
        /// <summary>
        /// 更新和新增文件上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        FileUploadModel InsertOrUpdateFileUpload(FileUploadModel input);

        /// <summary>
        /// 删除一条文件上传
        /// </summary>
        /// <param name="input"></param>
        void DeleteFileUpload(EntityDto<long> input);

     }
}
