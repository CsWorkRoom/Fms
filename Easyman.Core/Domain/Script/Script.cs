using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Easyman.Common;
using System;

namespace Easyman.Domain
{
    /// <summary>
    /// 脚本流
    /// </summary>
    [Table(SystemConfiguration.TablePrefix+ "SCRIPT")]
    public class Script:CommonEntityHelper
    {
        [Key,Column("ID")]
        public override long Id { get; set ; }
        /// <summary>
        /// 脚本流名称
        /// </summary>
        [Column("NAME"),StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 脚本类型
        /// </summary>
        [Column("SCRIPT_TYPE_ID")]
        public virtual long ScriptTypeId { get; set; }
        /// <summary>
        /// 建立主外键关系
        /// </summary>
        [ForeignKey("ScriptTypeId")]
        public virtual ScriptType ScriptType { get; set; }
        /// <summary>
        /// 事件表达式
        /// </summary>
        [Column("CRON"),StringLength(50)]
        public virtual string Cron { get; set; }
        /// <summary>
        /// 脚本状态 开启=1 关闭=0
        /// </summary>
        [Column("STATUS")]
        public virtual short? Status { get; set; }
        /// <summary>
        /// 失败重启次数
        /// </summary>
        [Column("RETRY_TIME")]
        public virtual int? RetryTime { get; set; }
        /// <summary>
        /// 脚本说明
        /// </summary>
        [Column("REMARK"),StringLength(500)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 任务组DIV  高度
        /// </summary>
        [Column("DIV_HIGH")]
        public virtual int? DivHigh { get; set; }
        /// <summary>
        /// 任务组DIV 宽度
        /// </summary>
        [Column("DIV_WIDE")]
        public virtual int? DivWide { get; set; }
    }
}
