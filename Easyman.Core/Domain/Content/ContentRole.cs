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
using Easyman.Users;

namespace Easyman.Domain
{
    /// <summary>
    /// 内容与角色关联表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_ROLE")]
    public class ContentRole : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("ROLE_ID")]
        public virtual int RoleId { get; set; }

        [Column("CONTENT_ID")]
        public virtual long ContentId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        [Column("IS_ALLOW")]
        public virtual bool? IsAllow { get; set; }

        public ContentRole()
        {

        }

        public ContentRole(int roleId, long contentId)
        {
            RoleId = roleId;
            ContentId = contentId;
        }
    }
}
