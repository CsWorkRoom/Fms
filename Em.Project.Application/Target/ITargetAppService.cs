using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 指标管理
    /// </summary>
    public interface ITargetAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个指标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TargetModel GetTarget(long id);
        /// <summary>
        /// 更新和新增指标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TargetModel InsertOrUpdateTarget(TargetModel input);

        /// <summary>
        /// 删除一条指标
        /// </summary>
        /// <param name="input"></param>
        void DeleteTarget(EntityDto<long> input);
        /// <summary>
        /// 获取指标json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetTargetTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> TargetList();

        List<TargetModel> AllTargetList();
    }
}
