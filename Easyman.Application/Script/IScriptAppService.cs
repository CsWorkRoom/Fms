using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Dto;
using EasyMan.Dtos;
using System.Collections.Generic;

namespace Easyman.Service
{
    /// <summary>
    /// 脚本流
    /// </summary>
    public interface IScriptAppService:IApplicationService
    {
        /// <summary>
        /// 获取全部脚本流(带分页)
        /// </summary>
        /// <returns></returns>
        ScriptSearchOutput GetAllScript(ScriptSearchInput input);
        /// <summary>
        /// 查询一个脚本流
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        EditScript SingScript(long  Id);
        /// <summary>
        /// 新增、修改
        /// </summary>
        /// <param name="input"></param>
        void EditScript(EditScript input);
        /// <summary>
        /// 新增脚本流
        /// </summary>
        /// <param name="input"></param>
        void AddScript(EditScript input);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        void UpdateScript(EditScript input);
        /// <summary>
        /// 删除一个脚本流
        /// </summary>
        /// <param name="input"></param>
        void DelScript(EntityDto<long> input);
        /// <summary>
        /// 获取全部脚本实例
        /// </summary>
        /// <returns></returns>
        ExampleScriptOutput GetAllExampleScriptSearch(ExampleScriptInput input, long scriptId = 0);
        /// <summary>
        /// 根据脚本流实例/脚本流 ID  获取一条脚本流实例
        /// </summary>
        /// <param name="ScriptId">脚本流ID</param>
        /// <param name="ExampleId">脚本流实例ID</param>
        /// <returns></returns>
        InitExampleScript GetSignExampleScript(long? ScriptId=null,long? ExampleId = null);
        /// <summary>
        /// 根据 脚本实例流ID 获取脚本实例流日志
        /// </summary>
        /// <param name="ExampleId">脚本实例流ID</param>
        /// <returns></returns>
        string GetExampLog(long? ExampleId);
        /// <summary>
        /// 根据 脚本节点实例ID 获取脚本节点实例日志
        /// </summary>
        /// <param name="ExampleNodeId">脚本节点实例ID</param>
        /// <returns></returns>
        string GetExampNodeLog(long? ExampleNodeId);
        /// <summary>
        /// 手工启动脚本流实例/脚本节点实例 
        /// </summary>
        /// <param name="StartType">启动类型 1 脚本流实例 2 脚本节点实例</param>
        /// <param name="ExampleId">脚本流实例ID</param>
        /// <param name="ExampleNode">脚本流节点实例ID</param>
        /// <returns></returns>
        bool StartExamp(StartExampModel input);

        /// <summary>
        /// 运行监测页面，失败后手工启动脚本流实例/脚本节点实例 
        /// </summary>
        /// <param name="StartType">启动类型 1 脚本流实例 2 脚本节点实例</param>
        /// <param name="ExampleId">脚本流实例ID</param>
        /// <param name="ExampleNode">脚本流节点实例ID</param>
        /// <returns></returns>
        bool StartExampSelect(short StartType ,long ExampleId,long ExampleNode);
    }
}
