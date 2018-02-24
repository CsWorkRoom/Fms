using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System.Collections.Generic;
using System;

namespace Easyman.Dto
{
    /// <summary>
    /// StartExamp手工启动脚本流方法所用传参Dto
    /// </summary>
    public class StartExampModel
    {
        //short? StartType, long? ExampleId, long? ExampleNode = 0
        public short StartType { get; set; }
        public long ExampleId { get; set; }
        public long ExampleNode { get; set; }

    }
    /// <summary>
    /// 脚本流输入模型
    /// </summary>
    [AutoMap(typeof(Script))]
    public class ScriptInput : EntityDto<long>
    {
        public new long Id { get; set; }
        [Required(ErrorMessage = "脚本流名称不能为空")]
        [Display(Name = "脚本流名称")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 脚本类型ID
        /// </summary>
        public virtual long? ScriptTypeId { get; set; }
        /// <summary>
        /// 脚本类型外键
        /// </summary>
        public virtual ScriptType ScriptType { get; set; }
        /// <summary>
        /// 时间表达式
        /// </summary>
        [Display(Name = "时间表达式")]
        public virtual string Cron { get; set; }
        /// <summary>
        /// 脚本状态   开启=1   关闭=0
        /// </summary>
        [Display(Name = "脚本状态")]
        public virtual short? Status { get; set; }
        /// <summary>
        /// 失败重试次数
        /// </summary>
        [Display(Name = "失败重试次数")]
        public virtual int? RetryTime { get; set; }
        /// <summary>
        /// 脚本说明
        /// </summary>
        [Display(Name = "脚本说明")]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 是否支持并发
        /// </summary>
        public virtual short? IsSupervene { get; set; }
    }

    /// <summary>
    /// 脚本流输出模型
    /// </summary>
    [AutoMap(typeof(Script))]
    public class ScriptOutput : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 脚本流名称
        /// </summary>
        [Required(ErrorMessage = "脚本流名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 脚本类型ID
        /// </summary>
        public virtual long? ScriptTypeId { get; set; }
        
        /// <summary>
        /// 脚本分类名称
        /// </summary>
        public virtual string ScriptTypeName { get; set; }
        /// <summary>
        /// 时间表达式
        /// </summary>
        public virtual string Cron { get; set; }
        /// <summary>
        /// 脚本状态
        /// </summary>
        public virtual short? Status { get; set; }
        /// <summary>
        /// 脚本状态 中文
        /// </summary>
        public virtual string StatusName {

            get {
                if (this.Status == 1)
                {
                    return "开启";
                }
                else if (this.Status == 0)
                {
                    return "关闭";
                }
                else
                {
                    return "数据错误";
                }
            }
            set { StatusName = value; }


        }
        /// <summary>
        /// 失败重试次数
        /// </summary>
        public virtual int? RetryTime { get; set; }
        /// <summary>
        /// 脚本说明
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 脚本流容器div高
        /// </summary>
        public virtual int? DivHigh { get; set; }
        /// <summary>
        /// 脚本流容器div宽
        /// </summary>
        public virtual int? DivWide { get; set; }

        /// <summary>
        /// 是否支持并发
        /// </summary>
        public virtual short? IsSupervene { get; set; }

    }
    /// <summary>
    /// 提供给报表的返回数据(带分页)
    /// </summary>
    public class ScriptSearchOutput : SearchOutputDto<ScriptOutput, long> { }

    public class ScriptSearchInput : SearchInputDto { }

    /// <summary>
    /// 连接线
    /// </summary>
    [AutoMap(typeof(ConnectLine))]
    public class ConnectLineOutput
    {
        public virtual long Id { get; set; }
        /// <summary>
        /// 起点DIV的(html)ID
        /// </summary>
        public virtual string FromDivId { get; set; }
        /// <summary>
        /// 起点位置(插件需要知道从DIV的哪个位置开始画线)
        /// </summary>
        public virtual string FromPointAnchors { get; set; }
        /// <summary>
        /// 脚本流Id
        /// </summary>
        public virtual long? ScriptId { get; set; }
        /// <summary>
        /// 结束点DIV的(html)ID
        /// </summary>
        public virtual string ToDivId { get; set; }
        /// <summary>
        /// 结束点位置 插件
        /// </summary>
        public virtual string ToPintAnchors { get; set; }
        /// <summary>
        /// 连接 线说明
        /// </summary>
        public virtual string Content { get; set; }
    }
    /// <summary>
    /// 脚本流节点
    /// </summary>
    [AutoMap(typeof(ScriptRefNode))]
    public class ScriptRefNodeOutput
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// 脚本流ID
        /// </summary>
        public virtual long? ScriptId { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public virtual long? ParentNodeId { get; set; }
        /// <summary>
        /// 当前节点ID
        /// </summary>
        public virtual long? CurrNodeId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
    /// <summary>
    /// 节点位置
    /// </summary>
    [AutoMap(typeof(NodePosition))]
    public class NodePositionOutput
    {
        public virtual long Id { get; set; }
        /// <summary>
        /// 脚本流ID
        /// </summary>
        public virtual long? ScriptId { get; set; }
        /// <summary>
        /// 脚本节点ID
        /// </summary>
        public virtual long? ScriptNodeId { get; set; }
        /// <summary>
        /// 节点X位置
        /// </summary>
        public virtual int? X { get; set; }
        /// <summary>
        /// 节点Y位置
        /// </summary>
        public virtual int? Y { get; set; }
        /// <summary>
        /// DIV  的Id  (node_加上脚本流ID)
        /// </summary>
        public virtual string DivId { get; set; }
    }

    public class EditScript: ScriptOutput
    {
        /// <summary>
        /// 连接线集合
        /// </summary>
        public List<ConnectLineOutput> ConnectLine { get; set; }
        public string ConnectLineJson { get; set; }
        /// <summary>
        /// 节点位置集合
        /// </summary>
        public List<NodePositionOutput> NodePosition { get; set; }
        public string NodePositionJson { get; set; }
    }


    #region 脚本流实例

    public class ExampleScriptInput : SearchInputDto { }
    /// <summary>
    /// 脚本流实例
    /// </summary>
    public class ExampleScriptOutput : SearchOutputDto<ExampleScript,long>
    {
       
    }

    [AutoMap(typeof(ScriptCase))]
    public class ExampleScriptCase
    {
        public  long Id { get; set; }
        /// <summary>
        /// 脚本流名称
        /// </summary>
        public  string Name { get; set; }
        /// <summary>
        /// 脚本流ID
        /// </summary>
        public  long? ScriptId { get; set; }

        public string SCRIPT_ID_STR { get; set; }
        /// <summary>
        /// 失败重启次数
        /// </summary>
        public  int? RetryTime { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        public  DateTime? StartTime { get; set; }
        public string START_TIME_STR
        {
            get
            {
                return this.StartTime == null ? "" : this.StartTime.Value.ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 启动模式(自动、手工)
        /// </summary>
        public  short? StratModel{ get; set; }
        public string START_MODEL_STR
        {
            get
            {
                if (this.StratModel == 0)
                {
                    return "自动";
                }
                else
                {
                    return "手工";
                }
            }
        }
        /// <summary>
        /// 启动人
        /// </summary>
        public  long? UserId { get; set; }

        public string USER_ID_STR { get; set; }
        /// <summary>
        /// 运行状态(等待、执行中、停止)
        /// </summary>
        public  short? RunStatus { get; set; }
        public string RUN_STATUS_STR
        {
            get
            {
                switch (this.RunStatus)
                {
                    case 0:
                        return "停止";
                    case 1:
                        return "等待";
                    case 2:
                        return "执行中";
                    default:
                        return "状态出错";
                }
            }
        }
        /// <summary>
        /// 是否有失败节点
        /// </summary>
        public  short? IsHaveFail { get; set; }
        public string IS_HAVE_FAIL_STR
        {
            get
            {
                if (this.IsHaveFail == 0)
                {
                    return "是";
                }
                else
                {
                    return "否";
                }
            }
        }
        /// <summary>
        /// 结束标识(成功、失败)
        /// </summary>
        public  short? ReturnCode { get; set; }
        public string RETURN_CODE_STR
        {
            get
            {
                if (this.ReturnCode == 1)
                {
                    return "成功";
                }
                else if (this.ReturnCode == 0)
                {
                    return "失败";
                }
                else
                {
                    return "未执行";
                }
            }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public  DateTime? EndTime { get; set; }

        public string END_TIME_STR
        {
            get
            {
                return this.EndTime == null ? "" : this.EndTime.Value.ToString("yyyy-MM-dd");
            }
        }
    }

    public class ExampleScript:EntityDto<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 脚本流实例名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 脚本流ID
        /// </summary>
        public long? SCRIPT_ID { get; set; }
        /// <summary>
        /// 脚本流名
        /// </summary>
        public string SCRIPT_ID_STR { get; set; }
        /// <summary>
        /// 失败重启次数
        /// </summary>
        public long? RETRY_TIME { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime? START_TIME { get; set; }

        public string START_TIME_STR
        {
            get
            {
                return this.START_TIME == null ? "" : this.START_TIME.Value.ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 启动模式
        /// </summary>
        public short? START_MODEL { get; set; }
        /// <summary>
        /// 启动模式
        /// </summary>
        public string START_MODEL_STR
        {
            get
            {
                if (this.START_MODEL == 0)
                {
                    return "自动";
                }
                else
                {
                    return "手工";
                }
            }
        }
        /// <summary>
        /// 启动人
        /// </summary>
        public long? USER_ID { get; set; }

        /// <summary>
        /// 启动人
        /// </summary>
        public string USER_ID_STR { get; set; }
        /// <summary>
        /// 启动状态
        /// </summary>
        public short? RUN_STATUS { get; set; }
        /// <summary>
        /// 启动状态
        /// </summary>
        public string RUN_STATUS_STR
        {
            get
            {
                switch (this.RUN_STATUS)
                {
                    case 0:
                        return "停止";
                    case 1:
                        return "等待";
                    case 2:
                        return "执行中";
                    default:
                        return "状态出错";
                }
            }
        }
        /// <summary>
        /// 是否有失败节点
        /// </summary>
        public short? IS_HAVE_FAIL { get; set; }
        /// <summary>
        /// 是否有失败节点
        /// </summary>
        public string IS_HAVE_FAIL_STR
        {
            get
            {
                if (this.IS_HAVE_FAIL == 0)
                {
                    return "是";
                }
                else
                {
                    return "否";
                }
            }
        }
        /// <summary>
        /// 结束标识
        /// </summary>
        public short? RETURN_CODE { get; set; }
        /// <summary>
        /// 结束标识
        /// </summary>
        public string RETURN_CODE_STR
        {
            get
            {
                if (this.RETURN_CODE == 1)
                {
                    return "成功";
                }
                else if (this.RETURN_CODE == 0)
                {
                    return "失败";
                }
                else
                {
                    return "未执行";
                }
            }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? END_TIME { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string END_TIME_STR
        {
            get
            {
                return this.END_TIME == null ? "" : this.END_TIME.Value.ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 脚本流容器div高
        /// </summary>
        public virtual int? DivHigh { get; set; }
        /// <summary>
        /// 脚本流容器div宽
        /// </summary>
        public virtual int? DivWide { get; set; }
    }

    /// <summary>
    /// 脚本流实例节点
    /// </summary>
    public class ScriptNodeCase
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 脚本流实例ID
        /// </summary>
        public long? SCRIPT_CASE_ID { get; set; }
        /// <summary>
        /// 脚本流实例名
        /// </summary>
        public string SCRIPT_CASE_ID_STR { get; set; }
        /// <summary>
        /// 脚本流id
        /// </summary>
        public long? SCRIPT_ID { get; set; }
        /// <summary>
        /// 脚本流名
        /// </summary>
        public string SCRIPT_ID_STR { get; set; }
        /// <summary>
        /// 节点ID
        /// </summary>
        public long? SCRIPT_NODE_ID { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string SCRIPT_NODE_ID_STR { get; set; }
        /// <summary>
        /// 启动数据库ID
        /// </summary>
        public long? DATABASE_ID { get; set; }
        /// <summary>
        /// 脚本模式
        /// </summary>
        public short SCRIPT_MODEL { get; set; }
        /// <summary>
        /// 脚本模式
        /// </summary>
        public string SCRIPT_MODEL_STR { get
            {
                if (this.SCRIPT_MODEL == 1)
                {
                    return "建表";
                }
                else if (this.SCRIPT_MODEL == 2)
                {
                    return "命令段";
                }
                else
                {
                    return "";
                }
            } }
        /// <summary>
        /// 脚本内容
        /// </summary>
        public string CONTENT { get; set; }
        /// <summary>
        /// 备注 
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        /// 表英文名
        /// </summary>
        public string E_TABLE_NAME { get; set; }
        /// <summary>
        /// 表中文名
        /// </summary>
        public string C_TABLE_NAME { get; set; }
        /// <summary>
        /// 表类型
        /// </summary>
        public short? TABLE_TYPE { get; set; }
        /// <summary>
        /// 表类型
        /// </summary>
        public string TABLE_TYPE_STR
        {
            get
            {
                if (this.TABLE_TYPE == 0)
                {
                    return "私有表";
                }
                else if (this.TABLE_TYPE == 1)
                {
                    return "公用表";
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 建表模式
        /// </summary>
        public short? TABLE_MODEL { get; set; }
        /// <summary>
        /// 建表模式
        /// </summary>
        public string TABLE_MODEL_STR
        {
            get
            {
                if (this.TABLE_MODEL == 1)
                {
                    return "新建";
                }
                else if (this.TABLE_MODEL == 2)
                {
                    return "复制";
                }
                else { return ""; }
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CREATE_TIME { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CREATE_TIME_STR { get { return this.CREATE_TIME==null?"": this.CREATE_TIME.Value.ToString("yyyy-MM-dd"); } }
        /// <summary>
        /// 创建人
        /// </summary>
        public long? USER_ID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string USER_ID_STR { get; set; }
        /// <summary>
        /// 表名后缀
        /// </summary>
        public long? TABLE_SUFFIX { get; set; }
        /// <summary>
        /// 实例启动时间
        /// </summary>
        public DateTime? START_TIME { get; set; }
        /// <summary>
        /// 实例启动时间
        /// </summary>
        public string START_TIME_STR { get
            {
                return this.START_TIME==null?"": this.START_TIME.Value.ToString("yyyy-MM-dd");
            } }
        /// <summary>
        /// 运行状态(等待、执行中、停止)
        /// </summary>
        public short? RUN_STATUS { get; set; }
        /// <summary>
        /// 运行状态
        /// </summary>
        public string RUN_STATUS_STR { get {
                if (this.RUN_STATUS == 0)
                {
                    return "停止";
                }
                else if (this.RUN_STATUS == 1)
                {
                    return "等待";
                }
                else if (this.RUN_STATUS == 2)
                {
                    return "执行中";
                }
                else
                {
                    return "";
                }

            } }
        /// <summary>
        /// 结束标识
        /// </summary>
        public short RETURN_CODE { get; set; }
        /// <summary>
        /// 结束标识
        /// </summary>
        public string RETURN_CODE_STR { get
            {
                if (this.RETURN_CODE==1) {
                    return "成功";
                }else if (this.RETURN_CODE==0) { return "失败";  }else { return ""; }
            } }
        /// <summary>
        /// 已重启次数
        /// </summary>
        public int? RETRY_TIME { get; set; }
        /// <summary>
        /// 实例结束时间
        /// </summary>
        public DateTime? END_TIME { get; set; }
        /// <summary>
        /// 实例结束时间
        /// </summary>
        public string END_TIME_STR { get { return this.END_TIME == null ? "": this.END_TIME.Value.ToString("yyyy-MM-dd"); } }
    }

    /// <summary>
    /// 查看实例需要
    /// </summary>
    [AutoMap(typeof(ExampleScriptCase))]
    public class InitExampleScript: ExampleScriptCase
    {
        /// <summary>
        /// 实例节点
        /// </summary>
         public string ScriptNodeCaseJson { get; set; }
        /// <summary>
        /// 连接线
        /// </summary>
        public string ConnectLineJson { get; set; }
        /// <summary>
        /// 节点位置
        /// </summary>
        public string NodePositionJson { get; set; }

        //节点实例ID
        public long LOG_NODE_ID { get; set; }
        //节点是否成功
        public string RETURN_CODE { get; set; }
    }

    /// <summary>
    /// 脚本节点for实例
    /// </summary>
    [AutoMap(typeof(ScriptNodeForCase))]
    public class ScriptNodeForCaseDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 脚本流实例ID
        /// </summary>
        public long? SCRIPT_CASE_ID
        { get { return ScriptCaseId; }  }
        /// <summary>
        /// 脚本流实例ID
        /// </summary>
        public long? ScriptCaseId { get; set; }
        /// <summary>
        /// 脚本流实例名
        /// </summary>
        public string SCRIPT_CASE_ID_STR { get; set; }
        /// <summary>
        /// 脚本流id
        /// </summary>
        public long? SCRIPT_ID { get; set; }
        /// <summary>
        /// 脚本流名
        /// </summary>
        public string SCRIPT_ID_STR { get; set; }
        /// <summary>
        /// 节点ID
        /// </summary>
        public long? SCRIPT_NODE_ID { get { return ScriptNodeId; } }
        /// <summary>
        /// 节点ID
        /// </summary>
        public long? ScriptNodeId { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string SCRIPT_NODE_ID_STR { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string NODE_NAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? LOG_NODE_ID { get; set; }

        /// <summary>
        /// 启动数据库ID
        /// </summary>
        public long? DATABASE_ID { get { return DbServerId; } }

        public long? DbServerId { get; set; }
        /// <summary>
        /// 脚本模式
        /// </summary>
        public short? SCRIPT_MODEL { get { return ScriptModel; } }

        public short? ScriptModel { get; set; }
        /// <summary>
        /// 脚本模式
        /// </summary>
        public string SCRIPT_MODEL_STR
        {
            get
            {
                if (this.ScriptModel == 1)
                {
                    return "建表";
                }
                else if (this.ScriptModel == 2)
                {
                    return "命令段";
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 脚本内容
        /// </summary>
        public string CONTENT { get { return Content; } }

        public string Content { get; set; }
        /// <summary>
        /// 备注 
        /// </summary>
        public string REMARK { get { return Remark; } }

        public string Remark { get; set; }
        /// <summary>
        /// 表英文名
        /// </summary>
        public string E_TABLE_NAME { get { return EnglishTabelName; } }

        public string EnglishTabelName { get; set; }
        /// <summary>
        /// 表中文名
        /// </summary>
        public string C_TABLE_NAME { get { return ChineseTabelName; } }

        public string ChineseTabelName { get; set; }
        /// <summary>
        /// 表类型
        /// </summary>
        public short? TABLE_TYPE { get { return TableType; } }

        public short? TableType { get; set; }
        /// <summary>
        /// 表类型
        /// </summary>
        public string TABLE_TYPE_STR
        {
            get
            {
                if (this.TableType == 0)
                {
                    return "私有表";
                }
                else if (this.TableType == 1)
                {
                    return "公用表";
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 建表模式
        /// </summary>
        public short? TABLE_MODEL { get { return TableModel; } }

        public short? TableModel { get; set; }
        /// <summary>
        /// 建表模式
        /// </summary>
        public string TABLE_MODEL_STR
        {
            get
            {
                if (this.TableModel == 1)
                {
                    return "新建";
                }
                else if (this.TableModel == 2)
                {
                    return "复制";
                }
                else { return ""; }
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CREATE_TIME { get { return CreationTime; } }

        public DateTime? CreationTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CREATE_TIME_STR { get { return this.CreationTime == null ? "" : this.CreationTime.Value.ToString("yyyy-MM-dd"); } }
        /// <summary>
        /// 创建人
        /// </summary>
        public long? USER_ID { get { return CreatorUserId; } }

        public long? CreatorUserId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string USER_ID_STR { get; set; }
        /// <summary>
        /// 表名后缀
        /// </summary>
        public long? TABLE_SUFFIX { get; set; }
        /// <summary>
        /// 实例启动时间
        /// </summary>
        public DateTime? START_TIME { get; set; }
        /// <summary>
        /// 实例启动时间
        /// </summary>
        public string START_TIME_STR
        {
            get
            {
                return this.START_TIME == null ? "" : this.START_TIME.Value.ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 运行状态(等待、执行中、停止)
        /// </summary>
        public short? RUN_STATUS { get; set; }
        /// <summary>
        /// 运行状态
        /// </summary>
        public string RUN_STATUS_STR
        {
            get;

            set;
        }
        /// <summary>
        /// 结束标识
        /// </summary>
        public short RETURN_CODE { get; set; }
        /// <summary>
        /// 结束标识
        /// </summary>
        public string RETURN_CODE_STR
        {
            get;
            set;
            
        }
        /// <summary>
        /// 已重启次数
        /// </summary>
        public int? RETRY_TIME { get; set; }
        /// <summary>
        /// 实例结束时间
        /// </summary>
        public DateTime? END_TIME { get; set; }
        /// <summary>
        /// 实例结束时间
        /// </summary>
        public string END_TIME_STR { get { return this.END_TIME == null ? "" : this.END_TIME.Value.ToString("yyyy-MM-dd"); } }
    }
    #endregion
}
