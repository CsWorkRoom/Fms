using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Easyman.Domain
{
    [Table("EM_FUNCTION")]
    public class Function : Entity, IMustHaveTenant, ISoftDelete, IHasCreationTime
    {
        public Function()
        {

        }

        [Key, Column("ID")]
        public override int Id { get; set; }

        [Column("TENANT_ID")]
        public virtual int TenantId { get; set; }

        [Column("CODE"), StringLength(100), Required]
        public virtual string Code { get; set; }

        [Column("NAME"), StringLength(100)]
        public virtual string Name { get; set; }

        [Column("DISCRIBITION"), StringLength(1000)]
        public virtual string Discribition { get; set; }

        [Column("TYPE"), StringLength(100)]
        public virtual string Type { get; set; }

        [Column("IS_DELETED")]
        public virtual bool IsDeleted { get; set; }

        [Column("CREATION_TIME")]
        public virtual DateTime CreationTime { get; set; }

        public virtual ICollection<FunctionRole> Roles { get; set; }
    }
}
