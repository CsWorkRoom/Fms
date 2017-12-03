using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 月度指标明细
    /// </summary>
    [Table("GP_MONTH_TARGET_DETAIL")]
    public class MonthTargetDetail : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 月份
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
        /// 月度指标固化ID
        /// </summary>
        [Column("MONTH_TARGET_ID")]
        public virtual long? MonthTargetId { get; set; }
        [ForeignKey("MonthTargetId")]
        public virtual MonthTarget MonthTarget { get; set; }
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
        /// 组织编号
        /// </summary>
        [Column("DISTRICT_ID")]
        public virtual long? DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        ///// <summary>
        ///// 是否选中
        ///// </summary>
        //[Column("IS_USE")]
        //public virtual bool? IsUse { get; set; }
        /// <summary>
        /// 客户经理编号
        /// </summary>
        [Column("MANAGER_NO"), StringLength(50)]
        public virtual string ManagerNo { get; set; }
        /// <summary>
        /// 客户经理姓名
        /// </summary>
        [Column("MANAGER_NAME"), StringLength(50)]
        public virtual string ManagerName { get; set; }
        /// <summary>
        /// 指标权重
        /// </summary>
        [Column("WEIGHT")]
        public virtual double? Weight { get; set; }

        ///// <summary>
        ///// 指标得分比重（反面为打分比重）--20171026作废字段
        ///// </summary>
        //[Column("SCORE_WEIGHT")]
        //public virtual double? ScoreWeight { get; set; }
        /// <summary>
        /// 指标目标值
        /// </summary>
        [Column("YEAR_TVALUE")]
        public virtual double? YearTValue { get; set; }
        /// <summary>
        /// 指标目标值
        /// </summary>
        [Column("TVALUE")]
        public virtual double? TValue { get; set; }
        /// <summary>
        /// 指标完成值
        /// </summary>
        [Column("RESULT_VALUE")]
        public virtual double? ResultValue { get; set; }
        /// <summary>
        /// 计分门槛值
        /// </summary>
        [Column("CRISIS_VALUE")]
        public virtual double? CrisisValue { get; set; }
        /// <summary>
        /// 指标得分
        /// </summary>
        [Column("SCORE")]
        public virtual double? Score { get; set; }
        ///// <summary>
        ///// 领导打分
        ///// </summary>
        //[Column("MARK_SCORE")]
        //public virtual double? MarkScore { get; set; }
        ///// <summary>
        ///// 最终得分= 领导打分*（1-得分比重）+指标得分*得分比重
        ///// </summary>
        //[Column("END_SCORE")]
        //public virtual double? EndScore { get; set; }
        ///// <summary>
        ///// 奖金系数
        ///// </summary>
        //[Column("BONUS_RATIO")]
        //public virtual double? BonusRatio { get; set; }
        ///// <summary>
        ///// 奖金额
        ///// </summary>
        //[Column("BONUS_VALUE")]
        //public virtual double? BonusValue { get; set; }
        /// <summary>
        /// 备注(用于对生成分数\系数\金额的过程记录)
        /// </summary>
        [Column("REMARK"), StringLength(300)]
        public virtual string Remark { get; set; }
    }
}
