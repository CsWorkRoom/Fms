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
    [Table(SystemConfiguration.TablePrefix + "PRE_DATA_TYPE")]
    public class PreDataType : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        [Column("DATA_TYPE"), StringLength(50)]
        public virtual string DataType { get; set; }

        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        [Column("DB_TYPE_ID")]
        public virtual long DbTypeId { get; set; }

        [ForeignKey("DbTypeId")]
        public virtual DbType DbType { get; set; }

    }
}
