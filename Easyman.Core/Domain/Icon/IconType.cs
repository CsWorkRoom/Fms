using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    [Table(SystemConfiguration.TablePrefix + "ICON_TYPE")]
    public class IconType : CommonEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 图标库类型名
        /// </summary>
        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 图标库类说明
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
