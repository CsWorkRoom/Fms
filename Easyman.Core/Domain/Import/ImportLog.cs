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
    [Table(SystemConfiguration.TablePrefix + "IMPORT_LOG")]
    public class ImportLog : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("NAME"), StringLength(100)]
        public virtual string Name { get; set; }

        [Column("CODE"), StringLength(100)]
        public virtual string Code { get; set; }

        [Column("USER_ID")]
        public virtual long UserId { get; set; }

        [Column("IMP_TB_ID")]
        public virtual long ImpTbId { get; set; }

        [Column("IMP_TB_CASE_ID")]
        public virtual long ImpTbCaseId { get; set; }

        [Column("CASE_TABLE_NAME"), StringLength(50)]
        public virtual string CaseTableName { get; set; }

        [Column("DURATION")]
        public virtual long Duration { get; set; }

        [Column("FILE_ID")]
        public virtual long FileId { get; set; }

        [Column("IMP_MODE"), StringLength(50)]
        public virtual string ImpMode { get; set; }

        [Column("FILE_NAME"), StringLength(50)]
        public virtual string FileName { get; set; }

        [ForeignKey("ImpTbId")]
        public virtual ImpTb ImpTb { get; set; }

        [ForeignKey("ImpTbCaseId")]
        public virtual ImpTbCase ImpTbCase { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }

        public virtual ICollection<OfflineLog> OfflineLog { get; set; }
    }
}
