using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Authorization.Roles;
using Easyman.Common;
using Easyman.Domain;

namespace Easyman.Domain
{
    /// <summary>
    /// 内容与标签项关联表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_REF_TAG")]
    public class ContentRefTag : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("TAG_ID")]
        public virtual long TagId { get; set; }

        [Column("CONTENT_ID")]
        public virtual long ContentId { get; set; }

        [ForeignKey("TagId")]
        public virtual ContentTag Tag { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        public ContentRefTag()
        {

        }

        public ContentRefTag(int tagId, int contentId)
        {
            TagId = tagId;
            ContentId = contentId;
        }
    }
}
