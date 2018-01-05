using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// FM_Case_VERSION(版本关联）
    /// </summary>
    public interface ICaseVersionAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个文件夹及文件管理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CaseVersionModel GetCaseVersion(long id);
        /// <summary>
        /// 更新和新增文件夹及文件管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        CaseVersionModel InsertOrUpdateCaseVersion(CaseVersionModel input);

        /// <summary>
        /// 删除一条文件夹及文件管理
        /// </summary>
        /// <param name="input"></param>
        void DeleteCaseVersion(EntityDto<long> input);
        /// <summary>
        /// 获取文件夹及文件管理json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetCaseVersionTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> CaseVersionList();

        /// <summary>
        /// 根据共享文件夹获版本号查询文件信息
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        CaseVersionModel GetCaseVersionByFolder(long folderId);
    }
}
