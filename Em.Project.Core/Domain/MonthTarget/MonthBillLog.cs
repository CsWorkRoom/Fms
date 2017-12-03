using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 固化日志
    /// </summary>
    [Table("GP_MONTH_BILL_LOG")]
    public class MonthBillLog : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 固化单ID
        /// </summary>
        [Column("MONTH_BILL_ID")]
        public virtual long? MonthBillId { get; set; }
        [ForeignKey("MonthBillId")]
        public virtual MonthBill MonthBill { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        [Column("LOG")]
        public virtual string Log { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        [Column("LOG_TIME")]
        public virtual DateTime? LogTime { get; set; }
        /// <summary>
        /// 执行结果
        /// </summary>
        [Column("LOG_RESULT"),StringLength(50)]
        public virtual string LogResult { get; set; }

    }
}
