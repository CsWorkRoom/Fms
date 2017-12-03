using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 月度公式固化
    /// </summary>
    [Table("GP_MONTH_TARGET_FORMULA")]
    public class MonthTargetFormula : CommonEntityHelper
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
        /// 公式ID
        /// </summary>
        [Column("TARGET_FORMULA_ID")]
        public virtual long? TargetFormulaId { get; set; }
        [ForeignKey("TargetFormulaId")]
        public virtual TargetFormula TargetFormula { get; set; }
        /// <summary>
        /// 公式类型
        /// </summary>
        [Column("TYPE"), StringLength(50)]
        public virtual string Type { get; set; }
        /// <summary>
        /// 公式名
        /// </summary>
        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 中文表达式
        /// </summary>
        [Column("CN_EXPRESSION"), StringLength(100)]
        public virtual string CnExpression { get; set; }
        /// <summary>
        /// 表达式(暂未启用)
        /// </summary>
        [Column("EN_EXPRESSION"), StringLength(100)]
        public virtual string EnExpression { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
