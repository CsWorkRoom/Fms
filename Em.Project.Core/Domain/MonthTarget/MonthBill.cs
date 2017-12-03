using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 月度固化单
    /// </summary>
    [Table("GP_MONTH_BILL")]
    public class MonthBill : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 月份 yyyyMM
        /// </summary>
        [Column("MONTH"), StringLength(6)]
        public virtual string Month { get; set; }

        /// <summary>
        /// 奖金总额
        /// </summary>
        [Column("BONUS_VALUE")]
        public virtual double? BonusValue { get; set; }
        /// <summary>
        /// 固化方式
        /// </summary>
        [Column("CUR_WAY"), StringLength(20)]
        public virtual string CurWay { get; set; }
        /// <summary>
        /// 固化人
        /// </summary>
        [Column("CUR_USER_ID")]
        public virtual long? CurUserId { get; set; }
        /// <summary>
        /// 固化时间
        /// </summary>
        [Column("CUR_TIME")]
        public virtual DateTime? CurTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Column("STATUS"), StringLength(50)]
        public virtual string Status { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        [Column("IS_USE")]
        public virtual bool IsUse { get; set; }
        /// <summary>
        /// 抢盘阶段(根据此状态判别当前进行到哪个阶段)
        /// </summary>
        [Column("STAGE_STATUS"), StringLength(50)]
        public virtual string StageStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 日志列表集合
        /// </summary>
        public virtual ICollection<MonthBillLog> MonthBillLog { get; set; }
        public virtual ICollection<MonthTarget> MonthTarget { get; set; }
    }
}
