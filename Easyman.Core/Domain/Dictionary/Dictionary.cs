using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    [Table(SystemConfiguration.TablePrefix + "DICTIONARY")]
    public class Dictionary : NotDeleteEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        [Column("DICTIONARY_TYPE_ID")]
        public virtual long? DictionaryTypeId { get; set; }
        /// <summary>
        /// 类实例
        /// </summary>
        [ForeignKey("DictionaryTypeId")]
        public virtual DictionaryType DictionaryType { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        [Column("PARENT_ID")]
        public virtual long? ParentId { get; set; }
        /// <summary>
        /// 父级实例
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual Dictionary Parent { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Column("NAME"),StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
