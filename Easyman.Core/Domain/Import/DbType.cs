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
    [Table(SystemConfiguration.TablePrefix + "DB_TYPE")]
    public class DbType : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual String Name { get; set; }

        [Column("REMARK"), StringLength(200)]
        public virtual String Remark { get; set; }

        public virtual ICollection<PreDataType> PreDataType { get; set; }

    }
}
