using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Easyman.Common;

namespace Easyman.Service
{
    /// <summary>
    /// 节点类型
    /// </summary>
    public class ScriptNodeTypeAppService : EasymanAppServiceBase, IScriptNodeTypeAppService
    {
        #region 初始化
        private readonly IRepository<ScriptNodeType, long> _scriptNodeTypeRepository;

        public ScriptNodeTypeAppService(IRepository<ScriptNodeType, long> scriptNodeTypeRepository)
        {
            _scriptNodeTypeRepository = scriptNodeTypeRepository;
            
        }

       
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取节点类型集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ScriptNodeTypeSearchOutput GetScriptNodeTypeSearch(ScriptNodeTypeSearchInput input)
        {
            var rowCount = 0;
            var types = _scriptNodeTypeRepository.GetAll().SearchByInputDto(input, out rowCount);

            var outPut = new ScriptNodeTypeSearchOutput()
            {
                Datas = types.ToList().Select(s => s.MapTo<ScriptNodeTypeOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        /// <summary>
        /// 根据ID获取某个节点类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ScriptNodeTypeOutput GetScriptNodeType(long id)
        {
            
            var type= _scriptNodeTypeRepository.Get(id);
            if (type != null)
            {
                return AutoMapper.Mapper.Map<ScriptNodeTypeOutput>(type);
            }
            throw new UserFriendlyException("未找到编号为【" + id.ToString() + "】的节点类型！");
        }

        /// <summary>
        /// 更新和新增节点类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ScriptNodeTypeOutput InsertOrUpdateScriptNodeType(ScriptNodeTypeInput input)
        {
            if (_scriptNodeTypeRepository.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
            {
                throw new System.Exception("节点类型重复");
            }
            //var type = AutoMapper.Mapper.Map<ScriptNodeTypeInput, ScriptNodeType>(input);
            var type = _scriptNodeTypeRepository.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new ScriptNodeType();
            type = Fun.ClassToCopy(input, type, (new string[] { "Id" }).ToList());
            var nodeType = _scriptNodeTypeRepository.InsertOrUpdate(type);

            if (nodeType != null)
            {
                return AutoMapper.Mapper.Map<ScriptNodeTypeOutput>(nodeType);
            }
            else
            {
                throw new UserFriendlyException("更新失败！");
            }
        }

        /// <summary>
        /// 删除一条任务类型
        /// </summary>
        /// <param name="input"></param>
        public void DeleteScriptNodeType(EntityDto<long> input)
        {
            try
            {
                _scriptNodeTypeRepository.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }

        public IEnumerable<object> GetScriptNodeTypeTreeJson()
        {
            var builder = new StringBuilder();

            var types = _scriptNodeTypeRepository.GetAll();

            var nodeTypes = types.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                open = false,
                iconSkin = "menu"
            }).ToList();

            return nodeTypes;
        }
        /// <summary>
        /// 获取全部类型  适用DropDownListFor
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllScriptNodeType()
        {
            List<SelectListItem> DataList = new List<SelectListItem>();
            var ScriptNodeType = _scriptNodeTypeRepository.GetAll().MapTo<List<ScriptNodeTypeOutput>>();
            DataList.Add(new SelectListItem() { Text = "全部", Value = "0", Selected = true });
            DataList.AddRange(ScriptNodeType.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }));
            return DataList;

        }
        #endregion
    }



    /// <summary>
    /// 节点
    /// </summary>
    public class ScriptNodeAppService : EasymanAppServiceBase, IScriptNodeAppService
    {
        #region 初始化
        private readonly IRepository<ScriptNode, long> _scriptNodeRepository;
        private readonly IRepository<ScriptNodeLog, long> _scriptNodeLogRepository;
        private readonly IRepository<DbServer, long> _dbServer;
        private readonly IRepository<NodePosition, long> _nodePosition;
        private readonly IRepository<Easyman.Domain.ScriptNodeCase, long> _scriptNodeCase;
        private readonly IRepository<ScriptNodeType, long> _scriptNodeTypeRepository;

        private readonly IRepository<ScriptNodeCaseLog, long> _scriptNodeCaseLogRepository;
        public ScriptNodeAppService(IRepository<ScriptNode, long> scriptNodeRepository,
            IRepository<ScriptNodeLog, long> scriptNodeLogRepository,
            IRepository<DbServer, long> dbServer, 
            IRepository<NodePosition, long> nodePosition,
            IRepository<Easyman.Domain.ScriptNodeCase, long> scriptNodeCase,
            IRepository<ScriptNodeCaseLog, long> scriptNodeCaseLogRepository)
        {
            _scriptNodeRepository = scriptNodeRepository;
            _scriptNodeLogRepository = scriptNodeLogRepository;
            _dbServer = dbServer;
            _nodePosition = nodePosition;
            _scriptNodeCase = scriptNodeCase;
            _scriptNodeCaseLogRepository = scriptNodeCaseLogRepository;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        private readonly string connStr = ConfigurationManager.ConnectionStrings["Default"].ToString();
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取节点集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ScriptNodeSearchOutput GetScriptNodeSearch(ScriptNodeSearchInput input)
        {
            var rowCount = 0;
            var nodes = _scriptNodeRepository.GetAll().SearchByInputDto(input, out rowCount);

            var outPut = new ScriptNodeSearchOutput()
            {
                //Datas = nodes.ToList().Select(s => s.MapTo<ScriptNodeOutput>()).ToList(),
                Datas = nodes.ToList().Select(s =>
                {
                    var temp = s.MapTo<ScriptNodeOutput>();
                    temp.DbServerName = s.DbServer.ByName;
                    temp.ScriptNodeTypeName = s.ScriptNodeType.Name;
                    return temp;
                }).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        /// <summary>
        /// 根据ID获取某个节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ScriptNodeOutput GetScriptNode(long id)
        {
            var node = _scriptNodeRepository.Get(id);
            ScriptNodeOutput res = new ScriptNodeOutput();
            if (node != null)
            {
                res = AutoMapper.Mapper.Map<ScriptNodeOutput>(node);
                if (res.DbServerId != null)
                {

                    res.DbServerName = _dbServer.Get(res.DbServerId.Value).ByName;
                }
                return res;
                
            }
            throw new UserFriendlyException("未找到编号为【" + id.ToString() + "】的节点！");
        }

        /// <summary>
        /// 脚本流配置页面  新增节点特有方法
        /// 根据TaskSpecific字段获取一条数据
        /// </summary>
        /// <param name="TaskSpecific"></param>
        /// <returns></returns>
        public ScriptNodeOutput GetScriptNodeEx(string TaskSpecific)
        {
            try
            {
                var node = _scriptNodeRepository.Single(m => m.TaskSpecific == TaskSpecific);
                if (node != null)
                {
                    return node.MapTo<ScriptNodeOutput>();
                }
            }
            catch (Exception ex)
            {
                
            }
            return new ScriptNodeOutput();
        }

        /// <summary>
        /// 更新和新增节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ScriptNodeOutput InsertOrUpdateScriptNode(ScriptNodeInput input)
        {
            if (_scriptNodeRepository.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
            {
                throw new System.Exception("节点重复");
            }
            if (_scriptNodeRepository.GetAll().Any(p => p.Id != input.Id && p.ScriptModel == (short)PubEnum.ScriptModel.CreateTb && p.EnglishTabelName == input.EnglishTabelName))
            {
                throw new System.Exception("表名重复");
            }
            //var dbServer = _dbServerRepository.Get(input.Id) ?? new DbServer();

            //var dbServer = AutoMapper.Mapper.Map<DbServer>(input);

            // var node = AutoMapper.Mapper.Map<ScriptNodeInput, ScriptNode>(input);
            var node = _scriptNodeRepository.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new ScriptNode();
            node = Fun.ClassToCopy(input, node, (new string[] { "Id" }).ToList());



            #region 修改节点的时候是否修改实例节点
            if (input.Id != 0 && input.IsUpdateExample == 1)
            {
                
                #region 查询是否有节点在执行中
               
                var scriptNodeCaseTemp = _scriptNodeCase.GetAllList(x => x.RunStatus == 2 && x.ScriptNodeId == input.Id);
                
                #endregion
                try
                {

                    if (scriptNodeCaseTemp.Count != 0)
                    {
                        string nodeId = string.Empty;
                        for (int i=0;i< scriptNodeCaseTemp.Count;i++)
                        {
                            nodeId = nodeId + scriptNodeCaseTemp[i].Id + ",";
                        }
                        throw new UserFriendlyException("更新失败，有节点实例正在执行！节点ID：" + nodeId);
                    }
                    var temp = _scriptNodeCase.GetAllList(x => x.ScriptNodeId == input.Id);
                    for (int i = 0; i < temp.Count; i++)
                    {
                        temp[i].DbServerId = input.DbServerId;
                        temp[i].ScriptModel = input.ScriptModel;
                        temp[i].Content = input.Content;
                        temp[i].Remark = input.Remark;
                        temp[i].EnglishTabelName = input.EnglishTabelName;
                        temp[i].ChineseTabelName = input.ChineseTabelName;
                        temp[i].TableType = input.TableType;
                        temp[i].TableModel = input.TableModel;
                        _scriptNodeCase.Update(temp[i]);
                    }

                }
                catch (Exception e)
                {
                    throw new UserFriendlyException("更新节点实例失败！" + e.Message);
                }
               
            }
           
            #endregion
            //修改节点
            var scriptNode = _scriptNodeRepository.InsertOrUpdate(node);

            #region 写入节点的调整日志

            var log = AutoMapper.Mapper.Map<ScriptNodeInput, ScriptNodeLog>(input);
            log.ScriptNodeId = scriptNode.Id;
            log.LogModel = (short)(input.Id == 0 ? 1 : 2);
            log.Id = 0;
            _scriptNodeLogRepository.Insert(log);
            #endregion

            if (scriptNode != null)
            {
                return AutoMapper.Mapper.Map<ScriptNodeOutput>(scriptNode);
            }
            else
            {
                throw new UserFriendlyException("更新失败！");
            }
        }

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="input"></param>
        public void DeleteScriptNode(EntityDto<long> input)
        {
            try
            {
                var listNodePosition = _nodePosition.GetAllList(m => m.ScriptNodeId == input.Id);
                if (listNodePosition.Count > 0)
                {
                    throw new UserFriendlyException("删除失败，该任务被任务组使用！");
                }

                #region 写入节点的调整日志
                var node = _scriptNodeRepository.Get(input.Id);
                var nodeInput = AutoMapper.Mapper.Map<ScriptNode, ScriptNodeInput>(node);
                var log = AutoMapper.Mapper.Map<ScriptNodeInput, ScriptNodeLog>(nodeInput);
                log.ScriptNodeId = node.Id;
                log.LogModel = (short)0;
                log.Id = 0;
                _scriptNodeLogRepository.Insert(log);
                #endregion

                _scriptNodeRepository.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }
        
        /// <summary>
        /// 根据节点类型  获取节点 0=全部
        /// </summary>
        /// <returns></returns>
        public List<ScriptNodeOutput> GetScriptNodeType(long ScriptNodeTypeId)
        {
            List<ScriptNodeOutput> res = new List<ScriptNodeOutput>();
            if (ScriptNodeTypeId == 0)
            {
                res = _scriptNodeRepository.GetAll().MapTo<List<ScriptNodeOutput>>();
            }
            else
            {
                res = _scriptNodeRepository.GetAllList(m => m.ScriptNodeTypeId == ScriptNodeTypeId).MapTo<List<ScriptNodeOutput>>();
            }
            foreach (var temp in res)
            {
                if (temp.DbServerId != null)
                {
                    temp.DbServerName = _dbServer.Get(temp.DbServerId.Value).ByName;
                }
            }
            return res;
        }

        /// <summary>
        /// 根据节点实例ID或者执行日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ScriptNodeCaseLog> GetScriptNodeCaseLog(long id)
        {
            try
            {
                var data = _scriptNodeCaseLogRepository.GetAll().Where(x => x.ScriptNodeCaseId == id).OrderBy(x=>x.Id).ToList();
                return data;
            }catch(Exception ex)
            {
                throw new UserFriendlyException("操作失败！");
            }
        }
        #endregion

    }

}
