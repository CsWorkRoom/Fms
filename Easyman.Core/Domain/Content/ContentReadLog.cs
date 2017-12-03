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
    /// 内容阅读日志
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_READ_LOG")]
    public class ContentReadLog : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 内容ID
        /// </summary>
        [Column("CONTENT_ID")]
        public virtual long ContentId { get; set; }

        /// <summary>
        /// 建立主外键关系
        /// </summary>
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        /// <summary>
        /// 阅读人
        /// </summary>
        [Column("USER_ID")]
        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        [Column("CREATE_TIME")]
        public virtual DateTime CreationTime { get; set; }
    }
}
