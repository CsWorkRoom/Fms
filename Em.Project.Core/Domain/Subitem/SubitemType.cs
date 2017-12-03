using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 打分项类型
    /// </summary>
    [Table("GP_SUBITEM_TYPE")]
    public class SubitemType : CommonEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 打分项类型
        /// </summary>
        [Column("NAME"), StringLength(50),Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
