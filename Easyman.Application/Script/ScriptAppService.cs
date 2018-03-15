using Easyman.Dto;
using Easyman.Domain;
using Abp.Domain.Repositories;
using EasyMan.Dtos;
using Abp.AutoMapper;
using EasyMan;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Abp.Domain.Uow;
using System.Configuration;
using System;
using System.Data;
using System.Reflection;
using Abp.UI;
using Easyman.Common;
using Easyman.Users;
using Abp.Application.Services.Dto;

namespace Easyman.Service
{
    /// <summary>
    /// 脚本流
    /// </summary>
    public class ScriptAppService : EasymanAppServiceBase, IScriptAppService
    {

        #region 初始化
        private readonly IRepository<Script, long> _script;
        private readonly IRepository<NodePosition, long> _nodePosition;
        private readonly IRepository<ScriptRefNode, long> _scriptRefNode;
        private readonly IRepository<ConnectLine, long> _connectLine;
        private readonly IRepository<ScriptNode, long> _scriptNode;
        private readonly IRepository<ScriptCase, long> _scriptCase;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<NodePositionForCase, long> _nodePositionForCase;
        private readonly IRepository<ConnectLineForCase, long> _connectLineForCase;
        private readonly IRepository<ScriptNodeForCase, long> _scriptNodeForCase;
        private readonly IRepository<Easyman.Domain.ScriptNodeCase, long> _scriptNodeCase;
        private readonly IRepository<ScriptCaseLog, long> _scriptCaseLog;
        private readonly IRepository<HandRecord, long> _handRecord;
        private readonly IRepository<ScriptNodeCaseLog, long> _scriptNodeCaseLog;

        public ScriptAppService(
            IRepository<Script, long> script,
            IRepository<NodePosition, long> nodePosition,
            IRepository<ScriptRefNode, long> scriptRefNode,
            IRepository<ConnectLine, long> connectLine,
            IRepository<ScriptNode, long> scriptNode,
            IRepository<ScriptCase, long> scriptCase,
            IRepository<User, long> userRepository,
            IRepository<NodePositionForCase, long> nodePositionForCase,
            IRepository<ConnectLineForCase, long> connectLineForCase,
            IRepository<ScriptNodeForCase, long> scriptNodeForCase,
            IRepository<Easyman.Domain.ScriptNodeCase, long> scriptNodeCase,
            IRepository<ScriptCaseLog, long> scriptCaseLog,
            IRepository<HandRecord, long> handRecord,
            IRepository<ScriptNodeCaseLog,long> scriptNodeCaseLog)
        {
            _script = script;
            _nodePosition = nodePosition;
            _scriptRefNode = scriptRefNode;
            _connectLine = connectLine;
            _scriptNode = scriptNode;
            _scriptCase = scriptCase;
            _userRepository = userRepository;
            _nodePositionForCase = nodePositionForCase;
            _connectLineForCase = connectLineForCase;
            _scriptNodeForCase = scriptNodeForCase;
            _scriptNodeCase = scriptNodeCase;
            _scriptCaseLog = scriptCaseLog;
            _handRecord = handRecord;
            _scriptNodeCaseLog = scriptNodeCaseLog;
        }

        #endregion

        #region 公用方法
        /// <summary>
        /// 获取全部脚本流(带分页)
        /// </summary>
        /// <returns></returns>
        public ScriptSearchOutput GetAllScript(ScriptSearchInput input)
        {
            int reordCount;
            var Script = _script.GetAll().SearchByInputDto(input, out reordCount);
            var ScriptRes = new ScriptSearchOutput();
            //转换
            ScriptRes.Datas = Script.ToList().MapTo<List<ScriptOutput>>();
            Pager page = new Pager();
            page = input.Page;
            page.TotalCount = reordCount;
            ScriptRes.Page = page;
            return ScriptRes;
        }

        /// <summary>
        /// 查询一个脚本流
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EditScript SingScript(long Id)
        {
            try
            {
                EditScript Res = new EditScript();
                Res = _script.Get(Id).MapTo<EditScript>();


                //节点位置
                Res.NodePosition = _nodePosition.GetAllList(x => x.ScriptId == Res.Id).MapTo<List<NodePositionOutput>>();
                Res.NodePositionJson = JsonConvert.SerializeObject(Res.NodePosition);

                //连接线
                Res.ConnectLine = _connectLine.GetAllList(x => x.ScriptId == Res.Id).MapTo<List<ConnectLineOutput>>();
                Res.ConnectLineJson = JsonConvert.SerializeObject(Res.ConnectLine);


                return Res;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }
        /// <summary>
        /// 新增、修改脚本流
        /// </summary>
        public void EditScript(EditScript input)
        {
            if (_script.GetAll().Any(x => x.Name == input.Name && x.Id != input.Id))
            {
                throw new System.Exception("已有脚本流：" + input.Name);
            }
            if (input.Id == 0)//新增
            {
                AddScript(input);
            }
            else//修改
            {
                UpdateScript(input);
            }
        }

        /// <summary>
        /// 新增脚本流
        /// </summary>
        /// <param name="input"></param>
        [UnitOfWork]
        public void AddScript(EditScript input)
        {
            //添加 脚本流表 并返回ID
            var scriptId = _script.InsertAndGetId(input.MapTo<Script>());
            input.Id = scriptId;
            AddNodeLinePosition(input);
        }
        /// <summary>
        /// 修改脚本流
        /// </summary>
        /// <param name="input"></param>
        [UnitOfWork]
        public void UpdateScript(EditScript input)
        {
            //var model = input.MapTo<Script>();
            //_script.Update(model);
            var type = _script.GetAll().FirstOrDefault(x => x.Id == input.Id);
            type = Fun.ClassToCopy(input, type, (new string[] { "Id" }).ToList());
            _script.Update(type);

            DelNodeLinePosition(input.Id);
            AddNodeLinePosition(input);
        }
        /// <summary>
        /// 删除脚本流节点配置、连接线、节点位置
        /// </summary>
        /// <param name="ScriptId"></param>
        [UnitOfWork]
        public void DelNodeLinePosition(long ScriptId)
        {
            //删除脚本流节点配置
            _scriptRefNode.Delete(x => x.ScriptId == ScriptId);
            //删除连接线
            _connectLine.Delete(x => x.ScriptId == ScriptId);
            //删除节点位置
            _nodePosition.Delete(x => x.ScriptId == ScriptId);
        }
        /// <summary>
        /// 添加脚本流节点配置、连接线、节点位置
        /// </summary>
        /// <param name="input"></param>
        [UnitOfWork]
        public void AddNodeLinePosition(EditScript input)
        {
            // 添加 连接线管理  表
            var ConnectLineOutput = JsonConvert.DeserializeObject<List<ConnectLineOutput>>(input.ConnectLineJson);
            var ConnectLine = ConnectLineOutput.MapTo<List<ConnectLine>>();
            foreach (var temp in ConnectLine)
            {
                temp.ScriptId = input.Id;
                _connectLine.Insert(temp);
            }
            //节点位置管理 表
            var NodePositionOutput = JsonConvert.DeserializeObject(input.NodePositionJson, typeof(List<NodePositionOutput>));
            var NodePosition = NodePositionOutput.MapTo<List<NodePosition>>();

            //添加 脚本流节点配置 表 
            foreach (var temp in NodePosition)
            {
                //添加 节点位置管理 表
                temp.ScriptId = input.Id;
                _nodePosition.Insert(temp);

                bool IsAdd = true;

                foreach (var connTemp in ConnectLine)
                {
                    var FromDivId = connTemp.FromDivId.Substring(connTemp.FromDivId.LastIndexOf('_') + 1);

                    var ToDivId = connTemp.ToDivId.Substring(connTemp.ToDivId.LastIndexOf('_') + 1);

                    if (ToDivId == temp.ScriptNodeId.ToString())
                    {
                        ScriptRefNode model = new ScriptRefNode();
                        model.ParentNodeId = long.Parse(FromDivId);
                        model.CurrNodeId = temp.ScriptNodeId;
                        model.ScriptId = input.Id;
                        _scriptRefNode.Insert(model);
                        IsAdd = false;
                    }
                }
                if (IsAdd)
                {
                    ScriptRefNode model = new ScriptRefNode();
                    model.CurrNodeId = temp.ScriptNodeId;
                    model.ScriptId = input.Id;
                    _scriptRefNode.Insert(model);
                }
            }
        }

        /// <summary>
        /// 删除一个脚本流
        /// </summary>
        /// <param name="input"></param>
        public void DelScript(EntityDto<long> input)
        {
            try
            {
                DelNodeLinePosition(input.Id);
                //删除脚本
                _script.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }


        /// <summary>
        /// 获取全部脚本实例
        /// </summary>
        /// <returns></returns>
        public ExampleScriptOutput GetAllExampleScriptSearch(ExampleScriptInput input, long scriptId = 0)
        {
            string sql = @"SELECT A.*, B.NAME AS USER_ID_STR, C.NAME AS SCRIPT_ID_STR
  FROM EM_SCRIPT_CASE A
       LEFT JOIN ABP_USERS B ON A.USER_ID = B.ID
       LEFT JOIN EM_SCRIPT C ON A.SCRIPT_ID = C.ID";

            _scriptCase.GetAllList(x => x.ScriptId == scriptId);
            if (scriptId != 0)
            {
                sql += " WHERE A.SCRIPT_ID=" + scriptId;
            }
            try
            {
                DataTable dt = new DataTable();
                dt = DbHelper.ExecuteGetTable(sql);
                ExampleScriptOutput res = new ExampleScriptOutput();
                res.Datas = Fun.DatatableToList<ExampleScript>(dt);
                Pager page = new Pager();
                page.TotalCount = 1;
                page.PageIndex = 1;
                page.PageSize = 20;
                res.Page = page;
                return res;
            }
            catch (Exception ex)
            {
                return new ExampleScriptOutput();
            }
        }
        /// <summary>
        /// 根据脚本流实例ID  获取一条脚本流实例
        /// </summary>
        /// <param name="ScriptId">脚本流ID</param>
        /// <param name="ExampleId">脚本流实例ID</param>
        /// <returns></returns>
        public InitExampleScript GetSignExampleScript(long? ScriptId = null, long? ExampleId = null)
        {
            #region 脚本流实例 
            ExampleScriptCase es = new ExampleScriptCase();
            if (ScriptId != null)//根据脚本流ID 查询
            {
                var temp = _scriptCase.FirstOrDefault(x => x.ScriptId == ScriptId.Value);
                es = temp.MapTo<ExampleScriptCase>();
            }
            else if (ExampleId != null)//根据脚本流实例ID 查询
            {
                var temp = _scriptCase.FirstOrDefault(x => x.Id == ExampleId.Value);
                es = temp.MapTo<ExampleScriptCase>();
            }
            if (es.UserId != null && es.UserId != 0)
            {
                es.USER_ID_STR = _userRepository.Get(es.UserId.Value).UserName;
            }

            es.SCRIPT_ID_STR = _script.Get(es.ScriptId.Value).Name;
            #endregion

            #region 查询脚本流实例所有节点的位置
            List<NodePositionForCase> npfc = new List<NodePositionForCase>();
            if (ScriptId != null)//根据脚本流ID 查询
            {
                npfc = _nodePositionForCase.GetAllList(x => x.ScriptId == ScriptId.Value);
            }
            else if (ExampleId != null)//根据脚本流实例ID 查询
            {
                npfc = _nodePositionForCase.GetAllList(x => x.ScriptCaseId == ExampleId.Value);

            }
            #endregion

            #region 查询脚本实例连接线SQL
            List<ConnectLineForCase> clfc = new List<ConnectLineForCase>();
            if (ScriptId != null)//根据脚本流ID 查询
            {
                clfc = _connectLineForCase.GetAllList(x => x.ScriptId == ScriptId.Value);
            }
            else if (ExampleId != null)//根据脚本流实例ID 查询
            {
                clfc = _connectLineForCase.GetAllList(x => x.ScriptCaseId == ExampleId.Value);
            }
            #endregion

            #region 脚本流实例节点SQL
            List<ScriptNodeForCaseDto> node_list = _scriptNodeForCase.GetAllList(x => x.ScriptCaseId == ExampleId.Value).MapTo<List<ScriptNodeForCaseDto>>();

            for (int i = 0; i < node_list.Count; i++)
            {
                var scriptCaseId = node_list[i].SCRIPT_CASE_ID;
                var scriptNodeId = node_list[i].SCRIPT_NODE_ID;
                var temp = _scriptNodeCase.FirstOrDefault(x => x.ScriptCaseId == scriptCaseId && x.ScriptNodeId == scriptNodeId);
                if (temp==null)
                {
                    node_list[i].RUN_STATUS_STR = "未执行";
                    node_list[i].RETURN_CODE_STR = "";
                    continue;
                }

                switch (temp.RunStatus)
                {
                    case 0:
                        node_list[i].RUN_STATUS_STR = "停止";
                        break;
                    case 1:
                        node_list[i].RUN_STATUS_STR = "等待";
                        break;
                    case 2:
                        node_list[i].RUN_STATUS_STR = "执行中";
                        break;
                    default:
                        node_list[i].RUN_STATUS_STR = "未执行";
                        break;
                }

                switch (temp.ReturnCode)
                {
                    case 0:
                        node_list[i].RETURN_CODE_STR = "失败";
                        break;
                    case 1:
                        node_list[i].RETURN_CODE_STR = "成功";
                        break;
                    case 2:
                        node_list[i].RETURN_CODE_STR = "预警";
                        break;
                    default:
                        node_list[i].RETURN_CODE_STR = "";
                        break;
                }

                node_list[i].LOG_NODE_ID = temp.Id;

                node_list[i].NODE_NAME = node_list[i].NAME;
            }

            #endregion

            try
            {
                InitExampleScript Res = new InitExampleScript();
                Res = es.MapTo<InitExampleScript>();


                #region 脚本实例节点位置
                Res.NodePositionJson = JsonConvert.SerializeObject(npfc);

                #endregion

                #region 连接线
                Res.ConnectLineJson = JsonConvert.SerializeObject(clfc);
                #endregion

                #region 脚本流实例节点
                Res.ScriptNodeCaseJson = JsonConvert.SerializeObject(node_list);
                #endregion
                return Res;
            }
            catch (Exception ex)
            {
                return new InitExampleScript();
            }
        }

        /// <summary>
        /// 根据脚本实例流ID  获取脚本实例流日志
        /// </summary>
        /// <param name="ExampleId">脚本实例流ID</param>
        /// <returns></returns>
        public string GetExampLog(long? ExampleId)
        {
            try
            {
                List<ScriptCaseLogModel> temp = _scriptCaseLog.GetAllList(x => x.ScriptCaseId == ExampleId).MapTo<List<ScriptCaseLogModel>>();

                if (temp.Count > 0)
                {
                    var scriptName = _scriptCase.FirstOrDefault(x => x.Id == ExampleId).Name;
                    for (int i = 0; i < temp.Count; i++)
                    {
                        temp[i].Name = scriptName;
                    }
                }
                return JsonConvert.SerializeObject(temp);
            }
            catch (Exception ex)
            {
                return "出现异常!" + ex.Message;
            }
        }

        /// <summary>
        /// 根据 脚本节点实例ID 获取脚本节点实例日志
        /// </summary>
        /// <param name="ExampleNodeId">脚本节点实例ID</param>
        /// <returns></returns>
        public string GetExampNodeLog(long? ExampleNodeId)
        {
  //          string sql = string.Format(@"SELECT A.*,C.NAME
  //FROM EM_SCRIPT_NODE_CASE_LOG A
  //     LEFT JOIN EM_SCRIPT_NODE_CASE B ON A.SCRIPT_NODE_CASE_ID = B.ID
  //     LEFT JOIN EM_SCRIPT_NODE_FORCASE C ON B.SCRIPT_NODE_ID=C.SCRIPT_NODE_ID
  //     WHERE A.SCRIPT_NODE_CASE_ID={0}
  //     ORDER BY LOG_TIME DESC", ExampleNodeId);
            try
            {
                List<ScriptNodeCaseLogModel> temp = _scriptNodeCaseLog.GetAllList(x => x.ScriptNodeCaseId == ExampleNodeId).MapTo<List<ScriptNodeCaseLogModel>>();
            var temp_scriptNodeCase = _scriptNodeCase.FirstOrDefault(ExampleNodeId.Value);

            var name = _scriptNodeForCase.FirstOrDefault(x => x.ScriptNodeId == temp_scriptNodeCase.ScriptNodeId && x.ScriptCaseId == temp_scriptNodeCase.ScriptCaseId).Name;

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].Name = name;
            }
            
                return JsonConvert.SerializeObject(temp);
            }
            catch (Exception ex)
            {
                return "出现异常!" + ex.Message;
            }
        }

        /// <summary>
        /// 手工启动脚本流实例/脚本节点实例 
        /// </summary>
        /// <param name="StartType">启动类型 1 脚本流实例 2 脚本节点实例</param>
        /// <param name="ExampleId">脚本流实例ID</param>
        /// <param name="ExampleNode">脚本流节点实例ID</param>
        /// <returns>备注：在配置报表哪里直接调用方法，跟删除方法类似，不能直接使用传参数的方式，要建立一个Dto类型，类型里面包含这些参数</returns>
        public bool StartExamp(StartExampModel input)
        {
            //short? StartType, long? ExampleId, long? ExampleNode = 0
            short StartType = input.StartType;
            long ExampleId = input.ExampleId;
            long ExampleNode = input.ExampleNode;
            long OBJECT_ID = 0;
            PubEnum.HandType start = (PubEnum.HandType)StartType;

            if (start == PubEnum.HandType.Script)
            {
                OBJECT_ID = ExampleId;
            }
            else if (start == PubEnum.HandType.ScriptNode)
            {
                OBJECT_ID = ExampleNode;
            }
            try
            {
                HandRecord hr = new HandRecord();
                hr.HandType = (short)StartType;
                hr.UserId = AbpSession.UserId.Value;
                hr.ObjectId = OBJECT_ID;
                hr.AddTime = DateTime.Now;
                #region 任务启动时间和实例ID由服务去添加
                //hr.StartTime = DateTime.Now;
                //hr.ObjectCaseId = ExampleId;
                #endregion
                _handRecord.Insert(hr);
                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("启动失败！" + ex.Message);
            }
        }

        /// <summary>
        ///  运行监测页面，失败后手工启动脚本流实例/脚本节点实例 
        /// </summary>
        /// <param name="StartType">启动类型 1 脚本流实例 2 脚本节点实例</param>
        /// <param name="ExampleId">脚本流实例ID</param>
        /// <param name="ExampleNode">脚本流节点实例ID</param>
        /// <returns>备注：在SelectScriptExample调用方法，直接使用传参数的方式</returns>
        public bool StartExampSelect(short StartType, long ExampleId, long ExampleNode)
        {
            //short? StartType, long? ExampleId, long? ExampleNode = 0
            long OBJECT_ID = 0;
            PubEnum.HandType start = (PubEnum.HandType)StartType;

            if (start == PubEnum.HandType.Script)
            {
                OBJECT_ID = ExampleId;
            }
            else if (start == PubEnum.HandType.ScriptNode)
            {
                OBJECT_ID = ExampleNode;
            }
            try
            {
                HandRecord hr = new HandRecord();
                hr.HandType = (short)StartType;
                hr.UserId = AbpSession.UserId.Value;
                //hr.ObjectId = OBJECT_ID;
                hr.AddTime = DateTime.Now;
                //hr.StartTime = DateTime.Now;
                hr.ObjectCaseId = OBJECT_ID;
                #region 赋值脚本节点ID给手工记录
                var nodeCase = _scriptNodeCase.FirstOrDefault(ExampleId);
                if (nodeCase != null)
                {
                    hr.ObjectId = nodeCase.ScriptNodeId;
                }
                #endregion
                _handRecord.Insert(hr);
                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("启动失败！" + ex.Message);
            }
        }
        #endregion
    }
}


