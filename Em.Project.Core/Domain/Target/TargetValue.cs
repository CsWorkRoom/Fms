using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 指标目标值
    /// </summary>
    [Table("GP_TARGET_VALUE")]
    public class TargetValue : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 年份/月份
        /// </summary>
        [Column("YEAR_MONTH"), StringLength(6)]
        public virtual string YearMonth { get; set; }

        /// <summary>
        /// 指标
        /// </summary>
        [Column("TARGET_ID")]
        public virtual long? TargetId { get; set; }
        [ForeignKey("TargetId")]
        public virtual Target Target { get; set; }
        /// <summary>
        /// 指标标识ID
        /// </summary>
        [Column("TARGET_TAG_ID")]
        public virtual long? TargetTagId { get; set; }
        [ForeignKey("TargetTagId")]
        public virtual TargetTag TargetTag { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        [Column("DISTRICT_ID")]
        public virtual long? DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        #region 20171026新增两字段MANAGER_NO、MANAGER_NAME
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
        #endregion

        /// <summary>
        /// 目标值
        /// </summary>
        [Column("TVALUE")]
        public virtual double? TValue { get; set; }

        ///// <summary>
        ///// 指标得分比重（反面为打分比重） --20171026该字段作废
        ///// </summary>
        //[Column("SCORE_WEIGHT")]
        //public virtual double? ScoreWeight { get; set; }
        ///// <summary>
        ///// 是否在用  --20171030该字段作废
        ///// </summary>
        //[Column("IS_USE")]
        //public virtual bool IsUse { get; set; }

    }
}
