using Abp.Application.Services;
using Easyman.Dto;
using System.Web.Mvc;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Easyman.Service
{
    public interface IScriptTypeAppService:IApplicationService
    {
        /// <summary>
        /// 获取全部(带分页)
        /// </summary>
        /// <returns></returns>
        ScriptTypeSearchOutput GetAllScriptType(ScriptTypeSearchInput input);
        /// <summary>
        /// 获取全部  用于页面下拉
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> GetAllScriptTypeList();
        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="input"></param>
        void editScriptType(ScriptTypeInput input);
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ScriptTypeInput SingScriptType(long Id);
        /// <summary>
        /// 删除一条
        /// </summary>
        /// <param name="input"></param>
        void DelScriptType(EntityDto<long> input);
    }
}
