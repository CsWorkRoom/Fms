using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Common;
using Easyman.Domain;

namespace Easyman.Domain
{
    /// <summary>
    /// 评论与文件关联表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_REPLY_FILE")]
    public class ContentReplyFile : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("FILE_ID")]
        public virtual long FileId { get; set; }

        [Column("CONTENT_REPLY_ID")]
        public virtual long ContentReplyId { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }

        [ForeignKey("ContentReplyId")]
        public virtual ContentReply ContentReply { get; set; }

        public ContentReplyFile()
        {

        }

        public ContentReplyFile(int fileId, int contentReplyId)
        {
            FileId = fileId;
            ContentReplyId = contentReplyId;
        }

    }
}
