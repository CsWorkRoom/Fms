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
    [Table("GP_MONTH_BONUS_DETAIL")]
    public class MonthBonusDetail : NotDeleteEntityHelper
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
        /// 月份
        /// </summary>
        [Column("MONTH"), StringLength(6)]
        public virtual string Month { get; set; }
        /// <summary>
        /// 指标标识ID
        /// </summary>
        [Column("TARGET_TAG_ID")]
        public virtual long? TargetTagId { get; set; }
        [ForeignKey("TargetTagId")]
        public virtual TargetTag TargetTag { get; set; }
        /// <summary>
        /// 组织编号
        /// </summary>
        [Column("DISTRICT_ID")]
        public virtual long? DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
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
        /// 指标总得分
        /// </summary>
        [Column("TARGET_SCORE")]
        public virtual double? TargetScore { get; set; }
        /// <summary>
        /// 领导打分总得分
        /// </summary>
        [Column("MARK_SCORE")]
        public virtual double? MarkScore { get; set; }
        /// <summary>
        /// 最终得分= (月度总得分+打分总分)/2
        /// </summary>
        [Column("MONTH_SCORE")]
        public virtual double? MonthScore { get; set; }

        /// <summary>
        /// 奖金系数
        /// </summary>
        [Column("BONUS_RATIO")]
        public virtual double? BonusRatio { get; set; }
        /// <summary>
        /// 奖金额
        /// </summary>
        [Column("BONUS_VALUE")]
        public virtual double? BonusValue { get; set; }
        /// <summary>
        /// 备注(用于对生成分数\系数\金额的过程记录),废弃：在日志表中记录
        /// </summary>
        [Column("REMARK"), StringLength(300)]
        public virtual string Remark { get; set; }
    }
}
