using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 指标类型管理
    /// </summary>
    public interface ITargetTypeAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个指标类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TargetTypeModel GetTargetType(long id);
        /// <summary>
        /// 更新和新增指标类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TargetTypeModel InsertOrUpdateTargetType(TargetTypeModel input);

        /// <summary>
        /// 删除一条指标类型
        /// </summary>
        /// <param name="input"></param>
        void DeleteTargetType(EntityDto<long> input);
        /// <summary>
        /// 获取指标类型json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetTargetTypeTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> TargetTypeList();
    }
}
