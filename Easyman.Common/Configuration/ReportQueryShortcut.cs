using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Common.Configuration
{
    [Table(SystemConfiguration.TablePrefix + "REPORT_QUERY_SHORTCUT")]
    public class ReportQueryShortcut : DateTimeEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("QUERY_ID")]
        public virtual long QueryId { get; set; }

        [Column("REPORT_ID")]
        public virtual long ReportId { get; set; }

        [Column("ORDER")]
        public virtual int Order { get; set; }
    }
}
