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
    /// 内容与推送关联表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_PUSH_WAY")]
    public class ContentPushWay : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("PUSH_WAY_ID")]
        public virtual long PushWayId { get; set; }

        [Column("CONTENT_ID")]
        public virtual long ContentId { get; set; }

        [ForeignKey("PushWayId")]
        public virtual PushWay PushWay { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        public ContentPushWay()
        {

        }

        public ContentPushWay(int pushWayId, int contentId)
        {
            PushWayId = pushWayId;
            ContentId = contentId;
        }
    }
}
