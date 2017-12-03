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
    /// 内容与用户关联表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_USER")]
    public class ContentUser : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("USER_ID")]
        public virtual long UserId { get; set; }

        [Column("CONTENT_ID")]
        public virtual long ContentId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        /// <summary>
        /// 是否允许
        /// </summary>
        [Column("IS_ALLOW")]
        public virtual bool? IsAllow { get; set; }

        public ContentUser()
        {

        }

        public ContentUser(int userId, int contentId)
        {
            UserId = userId;
            ContentId = contentId;
        }
    }
}
