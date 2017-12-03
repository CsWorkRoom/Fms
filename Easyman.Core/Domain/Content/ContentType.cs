using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Common;
using Easyman.Users;

namespace Easyman.Domain
{
    /// <summary>
    /// 内容类型
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_TYPE")]
    public class ContentType : CommonEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }


        /// <summary>
        /// 内容定义ID
        /// </summary>
        [Column("DEFINE_ID")]
        public virtual long DefineId { get; set; }

        /// <summary>
        /// 建立主外键关系
        /// </summary>
        [ForeignKey("DefineId")]
        public virtual Define Define { get; set; }

        /// <summary>
        /// 类别名
        /// </summary>
        [Column("NAME"), StringLength(150)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        [Column("PARENT_ID")]
        public virtual long ParentId { get; set; }

        /// <summary>
        /// 层次编码
        /// </summary>
        [Column("PATH_ID")]
        public virtual long PathId { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Column("LEVEL")]
        public virtual long Level { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        [Column("SHOW_ORDER")]
        public virtual long ShowOrder { get; set; }

        public virtual ICollection<ContentType> ChildContentType { get; set; }
    }
}
