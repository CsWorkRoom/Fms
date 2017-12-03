using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// rdlc报表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "RDLC_REPORT")]
    public class RdlcReport : CommonEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("REPORT_ID")]
        public virtual long? ReportId { get; set; }

        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        [Column("APPLICATION_TYPE"), StringLength(50)]
        public virtual string ApplicationType { get; set; }
        [Column("ROW_NUM")]
        public virtual int? RowNum { get; set; }

        [Column("IS_OPEN")]
        public virtual bool IsOpen { get; set; }
        /// <summary>
        /// rdlc配置xml
        /// </summary>
        [Column("RDLC_XML")]
        public virtual string RdlcXml { get; set; }
        /// <summary>
        /// 是否默认显示筛选区
        /// </summary>
        [Column("IS_SHOW_FILTER")]
        public virtual bool? IsShowFilter { get; set; }
        /// <summary>
        /// 表格报表说明
        /// </summary>
        [Column("REMARK")]
        public virtual string Remark { get; set; }
    }
}
