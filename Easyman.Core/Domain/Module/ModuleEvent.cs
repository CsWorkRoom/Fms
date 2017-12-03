using Abp.Domain.Entities;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 事件管理
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "MODULE_EVENT")]
    public class ModuleEvent : Entity<long>
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// ANALYSIS_ID
        /// </summary>
        [Column("ANALYSIS_ID")]
        public virtual long AnalysisId { get; set; }

        [ForeignKey("AnalysisId")]
        public virtual Analysis Analysis { get; set; }

        /// <summary>
        /// 功能代码
        /// </summary>
        [Column("CODE"), StringLength(50)]
        public virtual string Code { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        [Column("EVENT_TYPE"), StringLength(20)]
        public virtual string EventType { get; set; }

        /// <summary>
        /// 事件源头(内置、外置)
        /// 2017.7.27由cs添加
        /// </summary>
        [Column("EVENT_FROM"), StringLength(20)]
        public virtual string EventFrom { get; set; }

        /// <summary>
        /// 时间名称
        /// </summary>
        [Column("EVENT_NAME"), StringLength(50)]
        public virtual string EventName { get; set; }

        /// <summary>
        /// 时间源表
        /// </summary>
        [Column("SOURCE_TABLE"), StringLength(50)]
        public virtual string SourceTable { get; set; }

        /// <summary>
        /// 源表ID
        /// </summary>
        [Column("SOURCE_ID")]
        public virtual long SourceTableId { get; set; }

        /// <summary>
        /// 功能代码
        /// </summary>
        [Column("URL"), StringLength(2000)]
        public virtual string Url { get; set; }

    }
}
