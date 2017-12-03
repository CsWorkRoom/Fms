using Abp.Domain.Entities;
using Easyman.Authorization.Roles;
using Easyman.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    [Table(SystemConfiguration.TablePrefix + "ROLE_MODULE")]
    public class RoleModule : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("ROLE_ID")]
        public virtual int RoleId { get; set; }

        [Column("MODULE_ID")]
        public virtual long ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public RoleModule()
        {

        }

        public RoleModule(long moduleId, int roleId)
        {
            ModuleId = moduleId;
            RoleId = roleId;
        }
    }
}
