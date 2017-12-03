using Abp.Domain.Entities;
using Easyman.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Domain
{
    [Table(SystemConfiguration.TablePrefix + "REGULAR")]
    public class Regulars : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual String Name { get; set; }

        [Column("REGULAR"), StringLength(100)]
        public virtual string Regular { get; set; }

        [Column("ERROR_MSG"), StringLength(100)]
        public virtual string ErrorMsg { get; set; }

        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        public virtual ICollection<ImpTbField> ImpTbField { get; set; }
    }
}
