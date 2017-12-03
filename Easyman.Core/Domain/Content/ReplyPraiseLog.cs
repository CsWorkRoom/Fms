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
    /// 内容评论点赞日志
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_REPLY_PRAISE_LOG")]
    public class ReplyPraiseLog : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 评论ID
        /// </summary>
        [Column("CONTENT_REPLY_ID")]
        public virtual long ContentReplyId { get; set; }

        /// <summary>
        /// 建立主外键关系
        /// </summary>
        [ForeignKey("ContentReplyId")]
        public virtual ContentReply ContentReply { get; set; }

        /// <summary>
        /// 点赞人
        /// </summary>
        [Column("USER_ID")]
        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// 点赞时间
        /// </summary>
        [Column("CREATE_TIME")]
        public virtual DateTime CreationTime { get; set; }
    }
}
