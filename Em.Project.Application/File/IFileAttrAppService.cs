using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 文件属性管理
    /// </summary>
    public interface IFileAttrAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个文件属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileAttrModel GetFileAttr(long id);
        /// <summary>
        /// 更新和新增文件属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        FileAttrModel InsertOrUpdateFileAttr(FileAttrModel input);

        /// <summary>
        /// 删除一条文件属性
        /// </summary>
        /// <param name="input"></param>
        void DeleteFileAttr(EntityDto<long> input);
        /// <summary>
        /// 获取文件属性json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetFileAttrTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> FileAttrList();

       /// <summary>
       /// 获取文件属性对应关系
       /// </summary>
       /// <param name="fileLibraryId"></param>
       /// <param name="attrId"></param>
       /// <returns></returns>
        FileAttrModel GetFileAttrByFileAndAttr(long fileLibraryId,long attrId);
    }
}
