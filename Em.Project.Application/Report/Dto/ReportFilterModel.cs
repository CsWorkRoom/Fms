using Easyman.Domain;
using Abp.AutoMapper;

namespace Easyman.Dto
{
    [AutoMap(typeof(ReportFilter))]
    public class ReportFilterModel
    {

        public virtual long Id { get; set; }
        /// <summary>
        /// 表格报表ID
        /// </summary>
        public virtual long? TbReportId { get; set; }
        /// <summary>
        /// RDLC报表ID
        /// </summary>
        public virtual long? RdlcReportId { get; set; }
        /// <summary>
        /// 图形报表ID
        /// </summary>
        public virtual long? ChartReportId { get; set; }
        /// <summary>
        /// 字段编码
        /// </summary>
        public virtual string FieldCode { get; set; }
        /// <summary>
        /// 参数代码
        /// </summary>
        public virtual string FieldParam { get; set; }
        /// <summary>
        /// 参数中文名（字段名称）
        /// </summary>
        public virtual string FieldName { get; set; }
        /// <summary>
        /// 正则表达式ID
        /// </summary>
        public virtual long? RegularId { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public virtual string DefaultValue { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public virtual string DataType { get; set; }
        /// <summary>
        /// 筛选类型
        /// </summary>
        public virtual string FilterType { get; set; }
        /// <summary>
        /// 筛选sql(下拉中使用)
        /// </summary>
        public virtual string FilterSql { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public virtual int? OrderNum { get; set; }
        /// <summary>
        /// 是否快捷查询
        /// </summary>
        public bool IsQuick { get; set; }

        /// <summary>
        /// 是否自定义筛选标识
        /// </summary>
        public virtual bool IsCustom { get; set; }
        /// <summary>
        /// 是否筛选
        /// </summary>
        public virtual bool IsSearch { get; set; }
        /// <summary>
        /// 筛选控件提示语
        /// </summary>
        public virtual string Placeholder { get; set; }
    }
}