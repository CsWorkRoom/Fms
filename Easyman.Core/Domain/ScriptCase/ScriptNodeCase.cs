using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 脚本节点实例
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "SCRIPT_NODE_CASE")]
    public class ScriptNodeCase : Entity<long>
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 脚本流实例ID
        /// </summary>
        [Column("SCRIPT_CASE_ID")]
        public virtual long? ScriptCaseId { get; set; }
        /// <summary>
        /// 脚本流实例
        /// </summary>
        [ForeignKey("ScriptCaseId")]
        public virtual ScriptCase ScriptCase { get; set; }
        /// <summary>
        /// 脚本流ID
        /// </summary>
        [Column("SCRIPT_ID")]
        public virtual long? ScriptId { get; set; }
        /// <summary>
        /// 脚本节点ID
        /// </summary>
        [Column("SCRIPT_NODE_ID")]
        public virtual long? ScriptNodeId { get; set; }
        /// <summary>
        /// 启动数据库ID
        /// </summary>
        [Column("DB_SERVER_ID")]
        public virtual long? DbServerId { get; set; }

        /// <summary>
        /// 脚本模式(命令段、建表)
        /// </summary>
        [Column("SCRIPT_MODEL")]
        public virtual short? ScriptModel { get; set; }
        /// <summary>
        /// 脚本内容
        /// </summary>
        [Column("CONTENT")]
        public virtual string Content { get; set; }
        /// <summary>
        /// 编译后的脚本内容
        /// </summary>
        [Column("COMPILE_CONTENT")]
        public virtual string CompileContent { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        #region 建表节点特有
        /// <summary>
        /// 表英文名
        /// </summary>
        [Column("E_TABLE_NAME"), StringLength(50)]
        public virtual string EnglishTabelName { get; set; }

        /// <summary>
        /// 表中文名
        /// </summary>
        [Column("C_TABLE_NAME"), StringLength(50)]
        public virtual string ChineseTabelName { get; set; }

        /// <summary>
        /// 表类型（公用表、私有表）
        /// </summary>
        [Column("TABLE_TYPE")]
        public virtual short? TableType { get; set; }

        /// <summary>
        /// 建表模式（复制、新建）
        /// </summary>
        [Column("TABLE_MODEL")]
        public virtual short? TableModel { get; set; }
        #endregion

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATE_TIME")]
        public virtual DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column("USER_ID")]
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 表名后缀（私有表特有）
        /// </summary>
        [Column("TABLE_SUFFIX")]
        public Nullable<long> TableSuffix { get; set; }
        /// <summary>
        /// 实例启动时间
        /// </summary>
        [Column("START_TIME")]
        public Nullable<System.DateTime> StartTime { get; set; }
        /// <summary>
        /// 运行状态(等待、执行中、停止)
        /// </summary>
        [Column("RUN_STATUS")]
        public Nullable<short> RunStatus { get; set; }
        /// <summary>
        /// 结束标识(成功、失败)
        /// </summary>
        [Column("RETURN_CODE")]
        public Nullable<short> ReturnCode { get; set; }
        /// <summary>
        /// 已重试次数
        /// </summary>
        [Column("RETRY_TIME")]
        public Nullable<int> RetryTime { get; set; }
        /// <summary>
        /// 实例结束时间
        /// </summary>
        [Column("END_TIME")]
        public Nullable<System.DateTime> EndTime { get; set; }
    }
}
