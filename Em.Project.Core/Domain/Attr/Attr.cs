using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 属性
    /// </summary>
    [Table("FM_ATTR")]
    public class Attr : CommonEntityHelper
    {
        
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 属性类型ID
        /// </summary>
        [Column("ATTR_TYPE_ID")]
        public virtual long? AttrTypeId { get; set; }
        [ForeignKey("AttrTypeId")]
        public virtual AttrType AttrType { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
