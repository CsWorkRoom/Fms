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
    [Table(SystemConfiguration.TablePrefix + "IMP_TB_CASE")]
    public class ImpTbCase : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("CASE_TABLE_NAME"), StringLength(50)]
        public virtual string CaseTableName { get; set; }

        [Column("IMP_TB_ID")]
        public virtual long ImpTbId { get; set; }

        [ForeignKey("ImpTbId")]
        public virtual ImpTb ImpTb { get; set; }

        public virtual ICollection<ImportLog> ImportLog { get; set; }
    }
}
