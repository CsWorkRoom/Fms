using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 图形报表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CHART_REPORT")]
    public class ChartReport : CommonEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("REPORT_ID")]
        public virtual long? ReportId { get; set; }

        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        [Column("APPLICATION_TYPE"), StringLength(50)]
        public virtual string ApplicationType { get; set; }


        [Column("IS_OPEN")]
        public virtual bool IsOpen { get; set; }

        /// <summary>
        /// 是否默认显示筛选区
        /// </summary>
        [Column("IS_SHOW_FILTER")]
        public virtual bool? IsShowFilter { get; set; }
        /// <summary>
        /// 图表配置方式
        /// </summary>
        [Column("MAKE_WAY")]
        public virtual short? MakeWay { get; set; }
        /// <summary>
        /// 图表类型ID
        /// </summary>
        [Column("CHART_TYPE_ID")]
        public virtual long? ChartTypeId { get; set; }

        [ForeignKey("ChartTypeId")]
        public virtual ChartType ChartType { get; set; }
        /// <summary>
        /// 图表模版ID
        /// </summary>
        [Column("CHART_TEMP_ID")]
        public virtual long? ChartTempId { get; set; }
        /// <summary>
        /// 表格报表说明
        /// </summary>
        [Column("END_CODE")]
        public virtual string EndCode { get; set; }

        /// <summary>
        /// 表格报表说明
        /// </summary>
        [Column("REMARK")]
        public virtual string Remark { get; set; }
    }
}
