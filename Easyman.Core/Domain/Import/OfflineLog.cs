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
    [Table(SystemConfiguration.TablePrefix + "OFFLINE_LOG")]
    public class OfflineLog : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("BEGIN_TIME")]
        public virtual DateTime BeginTime { get; set; }

        [Column("END_TIME")]
        public virtual DateTime EndTime { get; set; }

        [Column("STATUS"), StringLength(50)]
        public virtual string Status { get; set; }

        [Column("RESULT"), StringLength(50)]
        public virtual string Result { get; set; }

        [Column("IMPORT_LOG_ID")]
        public virtual long ImportLogId { get; set; }

        [ForeignKey("ImportLogId")]
        public virtual ImportLog ImportLog { get; set; }
    }
}
