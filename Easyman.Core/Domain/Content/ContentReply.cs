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
    /// 内容评论
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_REPLY")]
    public class ContentReply : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("CONTENT_ID")]
        public virtual long ContentId { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        [Column("PARENT_ID")]
        public virtual long? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual ContentReply Parent { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Column("INFO"), StringLength(500)]
        public virtual string Info { get; set; }

        /// <summary>
        /// 评论人
        /// </summary>
        [Column("REPLY_UID")]
        public virtual long ReolyUId { get; set; }

        [ForeignKey("ReolyUId")]
        public virtual User ReplyUser { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        [Column("CREATE_TIME")]
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [Column("IP_ADDR"), StringLength(50)]
        public virtual string IpAddr { get; set; }

        /// <summary>
        /// IP预计    
        /// </summary>
        [Column("IPROMISE"), StringLength(50)]
        public virtual string IpRomise { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [Column("IS_DELETE")]
        public virtual bool? IsDelete { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [Column("DELETE_TIME")]
        public virtual DateTime DeleteTime { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        [Column("DELETE_UID")]
        public virtual long DeleteUid { get; set; }

        [ForeignKey("DeleteUid")]
        public virtual User DeleteUser { get; set; }

        /// <summary>
        /// 删除原因
        /// </summary>
        [Column("DELETE_REASON"), StringLength(500)]
        public virtual string DeleteReason { get; set; }

        public virtual ICollection<ContentReply> ChildContentReply { get; set; }
    }
}
