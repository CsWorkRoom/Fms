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
    [Table(SystemConfiguration.TablePrefix + "DEFAULT_FIELD")]
    public class DefaultField : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("FIELD_CODE"), StringLength(64)]
        public virtual string FieldCode { get; set; }

        [Column("FIELD_NAME"), StringLength(64)]
        public virtual string FieldName { get; set; }

        [Column("DATA_TYPE"), StringLength(50)]
        public virtual string DataType { get; set; }

        [Column("DEFAULT_VALUE"), StringLength(100)]
        public virtual string DefaultValue { get; set; }

        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        [Column("CREATE_TIME")]
        public virtual DateTime CreateTime { get; set; }

        [Column("DB_TYPE_ID")]
        public virtual long DbTypeId { get; set; }

        public virtual ICollection<ImpTb> ImpTb { get; set; }

    }
}
