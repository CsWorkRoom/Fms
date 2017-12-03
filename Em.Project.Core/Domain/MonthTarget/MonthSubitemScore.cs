using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 月度指标目标值
    /// </summary>
    [Table("GP_MONTH_SUBITEM_SCORE")]
    public class MonthSubitemScore : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        [Column("MONTH"), StringLength(6)]
        public virtual string Month { get; set; }
        /// <summary>
        /// 打分项
        /// </summary>
        [Column("SUBITEM_ID")]
        public virtual long? SubitemId { get; set; }
        [ForeignKey("SubitemId")]
        public virtual Subitem Subitem { get; set; }
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
        /// 打分值
        /// </summary>
        [Column("MARK_SCORE")]
        public virtual double? MarkScore { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
