using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 表格报表筛选条件
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "REPORT_FILTER")]
    public class ReportFilter : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 表格报表ID
        /// </summary>
        [Column("TB_REPORT_ID")]
        public virtual long? TbReportId { get; set; }
        /// <summary>
        /// RDLC报表ID
        /// </summary>
        [Column("RDLC_REPORT_ID")]
        public virtual long? RdlcReportId { get; set; }
        /// <summary>
        /// 图形报表ID
        /// </summary>
        [Column("CHART_REPORT_ID")]
        public virtual long? ChartReportId { get; set; }
        /// <summary>
        /// 字段编码
        /// </summary>
        [Column("FIELD_CODE"), StringLength(50)]
        public virtual string FieldCode { get; set; }
        /// <summary>
        /// 参数代码
        /// </summary>
        [Column("FIELD_PARAM"), StringLength(50)]
        public virtual string FieldParam { get; set; }
        /// <summary>
        /// 参数中文名（字段名称）
        /// </summary>
        [Column("FIELD_NAME"), StringLength(50)]
        public virtual string FieldName { get; set; }
        /// <summary>
        /// 正则表达式ID
        /// </summary>
        [Column("REGULAR_ID")]
        public virtual long? RegularId { get; set; }

        [ForeignKey("RegularId")]
        public virtual Regulars Regulars { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [Column("DEFAULT_VALUE"), StringLength(100)]
        public virtual string DefaultValue { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        [Column("DATA_TYPE"), StringLength(20)]
        public virtual string DataType { get; set; }
        /// <summary>
        /// 筛选类型
        /// </summary>
        [Column("FILTER_TYPE"), StringLength(20)]
        public virtual string FilterType { get; set; }
        /// <summary>
        /// 筛选sql(下拉中使用)
        /// </summary>
        [Column("FILTER_SQL")]
        public virtual string FilterSql { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        [Column("ORDER_NUM")]
        public virtual int? OrderNum { get; set; }
        /// <summary>
        /// 是否快捷查询
        /// </summary>
        [Column("IS_QUICK")]
        public bool IsQuick { get; set; }
        /// <summary>
        /// 是否筛选查询
        /// </summary>
        [Column("IS_SEARCH")]
        public bool IsSearch { get; set; }
        /// <summary>
        /// 筛选控件提示语
        /// </summary>
        [Column("PLACEHOLDER"),StringLength(100)]
        public virtual string Placeholder { get; set; }
    }
}
