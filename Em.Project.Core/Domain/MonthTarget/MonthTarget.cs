using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 月度指标-固化
    /// </summary>
    [Table("GP_MONTH_TARGET")]
    public class MonthTarget : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 月份 yyyyMM
        /// </summary>
        [Column("MONTH"), StringLength(6)]
        public virtual string Month { get; set; }
        /// <summary>
        /// 固化单ID
        /// </summary>
        [Column("MONTH_BILL_ID")]
        public virtual long? MonthBillId { get; set; }
        [ForeignKey("MonthBillId")]
        public virtual MonthBill MonthBill { get; set; }

        /// <summary>
        /// 指标类型ID
        /// </summary>
        [Column("TARGET_TYPE_ID")]
        public virtual long? TargetTypeId { get; set; }
        [ForeignKey("TargetTypeId")]
        public virtual TargetType TargetType { get; set; }
        /// <summary>
        /// 指标标识ID
        /// </summary>
        [Column("TARGET_TAG_ID")]
        public virtual long? TargetTagId { get; set; }
        [ForeignKey("TargetTagId")]
        public virtual TargetTag TargetTag { get; set; }

        /// <summary>
        /// 指标ID
        /// </summary>
        [Column("TARGET_ID")]
        public virtual long? TargetId { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        [Column("TARGET_NAME"), StringLength(50)]
        public virtual string TargetName { get; set; }

        /// <summary>
        /// 可选/必选
        /// </summary>
        [Column("CHOOSE_TYPE"), StringLength(20)]
        public virtual string ChooseType { get; set; }

        /// <summary>
        /// 指标权重
        /// </summary>
        [Column("WEIGHT")]
        public virtual double? Weight { get; set; }
        /// <summary>
        /// 指标说明
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 源头表(结果表)
        /// </summary>
        [Column("END_TABLE"), StringLength(100)]
        public virtual string EndTable { get; set; }
        /// <summary>
        /// 主字段
        /// </summary>
        [Column("MAIN_FIELD"), StringLength(100)]
        public virtual string MainField { get; set; }

        /// <summary>
        /// 计分门槛值
        /// </summary>
        [Column("CRISIS_VALUE")]
        public virtual double? CrisisValue { get; set; }

    }
}
