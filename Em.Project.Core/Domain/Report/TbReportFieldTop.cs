using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 表格报表（键值）
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "TB_REPORT_FIELD_TOP")]
    public class TbReportFieldTop : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("PARENT_ID")]
        public virtual long? ParentID { get; set; }

        [ForeignKey("ParentID")]
        public virtual TbReportFieldTop Parent { get; set; }

        [Column("TB_REPORT_ID")]
        public virtual long? TbReportId { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }
        
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        [Column("TB_REPORT_OUTEVENT_ID")]
        public virtual long? TbReportOutEventId { get; set; }

    }
}
