using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 指标标识管理
    /// </summary>
    public interface ITargetTagAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个指标标识
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TargetTagModel GetTargetTag(long id);
        /// <summary>
        /// 更新和新增指标标识
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TargetTagModel InsertOrUpdateTargetTag(TargetTagModel input);

        /// <summary>
        /// 删除一条指标标识
        /// </summary>
        /// <param name="id"></param>
        void DeleteTargetTag(EntityDto<long> input);
        /// <summary>
        /// 获取指标标识json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetTargetTagTreeJson();
        /// <summary>
        /// 获取所有标识List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> TargetTagList();
    }
}
