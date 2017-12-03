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
    /// 内容
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT")]
    public class Content : CommonEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        [Column("DEFINE_TYPE_ID")]
        public virtual long DefineTypeId { get; set; }

        /// <summary>
        /// 建立主外键关系
        /// </summary>
        [ForeignKey("DefineTypeId")]
        public virtual ContentType DefineType { get; set; }


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
        /// 标题
        /// </summary>
        [Column("TITLE"), StringLength(150)]
        public virtual string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Column("SUMMARY"), StringLength(500)]
        public virtual string Summary { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        [Column("INFO")]
        public virtual string Info { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Column("IMAGE")]
        public virtual string Image { get; set; }

        /// <summary>
        /// 重要
        /// </summary>
        [Column("IS_IMPORT")]
        public virtual bool? IsImport { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        [Column("BEGIN_TIME")]
        public virtual DateTime? BeginTime { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        [Column("END_TIME")]
        public virtual DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column("IS_USE")]
        public virtual bool? IsUse { get; set; }

        /// <summary>
        /// 置顶
        /// </summary>
        [Column("IS_URGENT")]
        public virtual bool? IsUrgent { get; set; }

        //public virtual ICollection<ContentTag> ContentTag { get; set; }
    }
}
