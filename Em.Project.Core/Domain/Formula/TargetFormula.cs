using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 公式管理
    /// </summary>
    [Table("GP_TARGET_FORMULA")]
    public class TargetFormula : CommonEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 公式类型
        /// </summary>
        [Column("TYPE"), StringLength(50)]
        public virtual string Type { get; set; }
        /// <summary>
        /// 公式名
        /// </summary>
        [Column("NAME"), StringLength(50),Required]
        public virtual string Name { get; set; }
        /// <summary>
        /// 中文表达式
        /// </summary>
        [Column("CN_EXPRESSION"), StringLength(100),Required]
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
