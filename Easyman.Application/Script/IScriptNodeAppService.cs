using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;
using Easyman.Domain;

namespace Easyman.Service
{
    /// <summary>
    /// 节点类型
    /// </summary>
    public interface IScriptNodeTypeAppService : IApplicationService
    {
        /// <summary>
        /// 获取节点类型集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ScriptNodeTypeSearchOutput GetScriptNodeTypeSearch(ScriptNodeTypeSearchInput input);

        /// <summary>
        /// 根据ID获取某个节点类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ScriptNodeTypeOutput GetScriptNodeType(long id);

        /// <summary>
        /// 更新和新增节点类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ScriptNodeTypeOutput InsertOrUpdateScriptNodeType(ScriptNodeTypeInput input);

        /// <summary>
        /// 删除一条数据库标识
        /// </summary>
        /// <param name="input"></param>
        void DeleteScriptNodeType(EntityDto<long> input);

        IEnumerable<object> GetScriptNodeTypeTreeJson();
        /// <summary>
        /// 获取全部类型  适用DropDownListFor
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> GetAllScriptNodeType();

    }

    /// <summary>
    /// 节点
    /// </summary>
    public interface IScriptNodeAppService : IApplicationService
    {
        /// <summary>
        /// 获取节点集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ScriptNodeSearchOutput GetScriptNodeSearch(ScriptNodeSearchInput input);

        /// <summary>
        /// 根据ID获取某个节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ScriptNodeOutput GetScriptNode(long id);
        /// <summary>
        /// 脚本流配置页面  新增节点特有方法
        /// 根据TaskSpecific字段获取一条数据
        /// </summary>
        /// <param name="TaskSpecific"></param>
        /// <returns></returns>
        ScriptNodeOutput GetScriptNodeEx(string TaskSpecific);

        /// <summary>
        /// 更新和新增节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ScriptNodeOutput InsertOrUpdateScriptNode(ScriptNodeInput input);

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="input"></param>
        void DeleteScriptNode(EntityDto<long> input);
        /// <summary>
        /// 根据节点类型  获取节点
        /// </summary>
        /// <returns></returns>
        List<ScriptNodeOutput> GetScriptNodeType(long ScriptNodeTypeId);

        /// <summary>
        /// 根据节点实例ID或者执行日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ScriptNodeCaseLog> GetScriptNodeCaseLog(long id);
    }

}
