using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Easyman.Common;
using Abp.Domain.Entities;
using System;

namespace Easyman.Domain
{
    /// <summary>
    /// 脚本节点实例日志
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "SCRIPT_NODE_CASE_LOG")]
    public class ScriptNodeCaseLog : Entity<long>
    {
        [Key,Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 脚本流实例ID
        /// </summary>
        [Column("SCRIPT_NODE_CASE_ID")]
        public virtual long? ScriptNodeCaseId { get; set; }
        /// <summary>
        /// 日志时间
        /// </summary>
        [Column("LOG_TIME")]
        public virtual DateTime? LogTime { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>
        [Column("LOG_LEVEL")]
        public virtual short? LogLevel { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>
        [Column("LOG_MSG")]
        public virtual string LogMsg { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>
        [Column("SQL_MSG")]
        public virtual string SqlMsg { get; set; }
    }
}
