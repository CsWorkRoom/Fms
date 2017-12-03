using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 月度奖金额
    /// </summary>
    [Table("GP_MONTH_BONUS")]
    public class MonthBonus : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 月份 yyyyMM
        /// </summary>
        [Column("MONTH"), StringLength(6),Required]
        public virtual string Month { get; set; }
        /// <summary>
        /// 奖金总额
        /// </summary>
        [Column("BONUS_VALUE"),Required, RegularExpression(@"^[0-9]+(.[0-9]{2})?$",ErrorMessage ="应为数值型")]//有两位小数的正实数
        public virtual double? BonusValue { get; set; }
        /// <summary>
        /// 录入方式
        /// </summary>
        [Column("IN_WAY"), StringLength(20)]
        public virtual string InWay { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
