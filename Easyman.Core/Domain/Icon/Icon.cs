using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    [Table(SystemConfiguration.TablePrefix + "ICON")]
    public class Icon : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 图标类型ID
        /// </summary>
        [Column("ICON_TYPE_ID")]
        public virtual long? IconTypeId { get; set; }
        /// <summary>
        /// 图标类实例
        /// </summary>
        [ForeignKey("IconTypeId")]
        public virtual IconType IconType { get; set; }
        /// <summary>
        /// 图标名
        /// </summary>
        [Column("DISPLAY_NAME"),StringLength(50)]
        public string DisplyName { get; set; }
        /// <summary>
        /// 图标类型class
        /// </summary>
        [Column("CLASS_NAME"), StringLength(50)]
        public string ClassName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
