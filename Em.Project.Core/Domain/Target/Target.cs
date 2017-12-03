using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 指标管理
    /// </summary>
    [Table("GP_TARGET")]
    public class Target : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
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
        /// 指标名称
        /// </summary>
        [Required]
        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 可选/必选
        /// </summary>
        [Column("CHOOSE_TYPE"), StringLength(20)]
        public virtual string ChooseType { get; set; }

        /// <summary>
        /// 指标权重
        /// </summary>
        [Required,RegularExpression(@"^[0-9]+(.[0-9]{2})?$", ErrorMessage = "应为数值型")]//有两位小数的正实数
        [Column("WEIGHT")]
        public virtual double? Weight { get; set; }
        /// <summary>
        /// 指标说明
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        [Column("IS_USE")]
        public virtual bool IsUse { get; set; }
        /// <summary>
        /// 源头表(结果表)
        /// </summary>
        [Required, Column("END_TABLE"), StringLength(100)]
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

        public virtual ICollection<TargetValue> TargetValue { get; set; }
    }
}
