using Abp.Domain.Entities.Auditing;
using Easyman.Authorization.Roles;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    [Table("EM_FUNCTION_ROLE")]
    public class FunctionRole : CreationAuditedEntity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("FUNCTION_ID"), Required]
        public virtual int FunId { get; set; }


        [Column("ROLE_ID"), Required]
        public virtual int RoleId { get; set; }

        [ForeignKey("FunId")]
        public virtual Function Function { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [Column("CREATION_TIME"), Required]
        public override DateTime CreationTime { get; set; }


        [Column("CREATOR_USER_ID"), Required]
        public override long? CreatorUserId { get; set; }

        public FunctionRole()
        {

        }

        /// <summary>
        /// Creates a new <see cref="UserRole"/> object.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleId">Role id</param>
        public FunctionRole(int funId, int roleId)
        {
            FunId = funId;
            RoleId = roleId;
        }
    }
}
