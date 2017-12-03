using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 打分项管理
    /// </summary>
    [Table("GP_SUBITEM")]
    public class Subitem : NotDeleteEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 打分类型ID
        /// </summary>
        [Column("SUBITEM_TYPE_ID")]
        public virtual long? SubitemTypeId { get; set; }
        [ForeignKey("SubitemTypeId")]
        public virtual SubitemType SubitemType { get; set; }

        /// <summary>
        /// 打分项
        /// </summary>
        [Required]
        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 打分权重分
        /// </summary>
        [Required,RegularExpression(@"^[0-9]+(.[0-9]{2})?$", ErrorMessage = "应为数值型")]//有两位小数的正实数
        [Column("WEIGHT")]
        public virtual double? Weight { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
