using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 字典类型
    /// </summary>
    public interface IDictionaryTypeAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DictionaryTypeModel GetDictionaryType(long id);
        /// <summary>
        /// 更新和新增类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DictionaryTypeModel InsertOrUpdateDictionaryType(DictionaryTypeModel input);

        /// <summary>
        /// 删除一条类型
        /// </summary>
        /// <param name="input"></param>
        void DeleteDictionaryType(EntityDto<long> input);
        /// <summary>
        /// 获取类型json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetDictionaryTypeTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> DictionaryTypeList();
    }
}
