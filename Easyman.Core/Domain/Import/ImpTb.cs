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
    /// <summary>
    /// 解析地址
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "IMP_TB")]
    public class ImpTb : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("CODE"), StringLength(100)]
        public virtual string Code { get; set; }

        [Column("CN_TABLE_NAME"), StringLength(100)]
        public virtual string CnTableName { get; set; }

        [Column("EN_TABLE_NAME"), StringLength(100)]
        public virtual string EnTableName { get; set; }

        [Column("RULE"), StringLength(50)]
        public virtual string Rule { get; set; }

        [Column("SQL")]
        public virtual string Sql { get; set; }

        [Column("DB_SERVER_ID")]
        public virtual long DbServerId { get; set; }

        [Column("IMP_TYPE_ID")]
        public virtual long ImpTypeId { get; set; }

        [ForeignKey("DbServerId")]
        public virtual DbServer DbServer { get; set; }

        [ForeignKey("ImpTypeId")]
        public virtual ImpType ImpType { get; set; }

        public virtual ICollection<ImpTbField> ImpTbField { get; set; }

        public virtual ICollection<DefaultField> DefaultField { get; set; }

        public virtual ICollection<ImportLog> ImportLog { get; set; }

        public virtual ICollection<ImpTbCase> ImpTbCase { get; set; }
    }
}
