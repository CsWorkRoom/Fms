using Abp.Authorization.Roles;
using Easyman.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Authorization.Roles
{
    public class Role : AbpRole<User>
    {
        public virtual int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Role Parent { get; set; }

        public virtual ICollection<Role> ChildRoles { get; set; }

        public Role()
        {

        }

        public Role(int? tenantId, string displayName)
            : base(tenantId, displayName)
        {

        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {

        }
    }
}