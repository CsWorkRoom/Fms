using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 字典管理
    /// </summary>
    public interface IDictionaryAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DictionaryModel GetDictionary(long id);
        /// <summary>
        /// 更新和新增字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DictionaryModel InsertOrUpdateDictionary(DictionaryModel input);

        /// <summary>
        /// 删除一条字典
        /// </summary>
        /// <param name="input"></param>
        void DeleteDictionary(EntityDto<long> input);
        /// <summary>
        /// 获取字典json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetDictionaryTreeJson();

        IEnumerable<object> GetDictionaryByTypeTreeJson(EntityDto<long> input);
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> DictionaryList();

        List<DictionaryModel> AllDictionaryList();
    }
}
