using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 公式管理
    /// </summary>
    public interface ITargetFormulaAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个公式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TargetFormulaModel GetTargetFormula(long id);
        /// <summary>
        /// 更新和新增公式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TargetFormulaModel InsertOrUpdateTargetFormula(TargetFormulaModel input);

        /// <summary>
        /// 删除一条公式
        /// </summary>
        /// <param name="input"></param>
        void DeleteTargetFormula(EntityDto<long> input);
        /// <summary>
        /// 获取公式json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetTargetFormulaTreeJson();
        /// <summary>
        /// 获取所有标识List
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> TargetFormulaList();
    }
}
