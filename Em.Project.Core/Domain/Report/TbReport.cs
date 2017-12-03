
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
    [Table(SystemConfiguration.TablePrefix + "TB_REPORT")]
    public class TbReport : CommonEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("REPORT_ID")]
        public virtual long? ReportId { get; set; }

        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        [Column("APPLICATION_TYPE"), StringLength(50)]
        public virtual string ApplicationType { get; set; }

        [Column("REPORT_TYPE")]
        public virtual short ReportType { get; set; }

        [Column("REPORT_STYLE"), StringLength(50)]
        public virtual string ReportStyle { get; set; }

        [Column("IS_CHECK")]
        public virtual bool IsCheck { get; set; }

        //[Column("IS_DEBUG")]
        //public virtual bool IsDebug { get; set; }

        [Column("IS_AUTO_LOAD")]
        public virtual bool IsAutoLoad { get; set; }

        [Column("IS_BIGDATA_LOAD")]
        public virtual bool IsBigdataLoad { get; set; }

        [Column("MAX_EXPORT_COUNT")]
        public virtual int? MaxExportCount { get; set; }

        [Column("IS_PAINATION")]
        public virtual bool IsPaination { get; set; }

        [Column("IS_SCROLL")]
        public virtual bool IsScroll { get; set; }

        [Column("EMPTY_RECORD"), StringLength(200)]
        public virtual string EmptyRecord { get; set; }

        [Column("ROW_NUM")]
        public virtual int? RowNum { get; set; }

        [Column("ROW_LIST"), StringLength(50)]
        public virtual string RowList { get; set; }

        [Column("ROW_STYLE"), StringLength(50)]
        public virtual string RowStyle { get; set; }

        [Column("SELECT_COLOR"), StringLength(50)]
        public virtual string SelectColor { get; set; }

        //废除表格报表占位符
        //[Column("IS_PLACEHOLDER")]
        //public virtual bool IsPlaceholder { get; set; }

        [Column("IS_OPEN")]
        public virtual bool IsOpen { get; set; }

        /// <summary>
        /// 是否显示行号
        /// </summary>
        [Column("IS_ROWNUMBER")]
        public virtual bool IsRowNumber { get; set; }
        /// <summary>
        /// 行号宽度
        /// </summary>
        [Column("ROWNUM_WIDTH")]
        public virtual int? RownumWidth { get; set; }
        /// <summary>
        /// 单元格合并信息
        /// </summary>
        [Column("MERGE_CELL_JSON")]
        public virtual string MergeCellJson { get; set; }
        /// <summary>
        /// 复选框互斥（行选中）
        /// </summary>
        [Column("MULTIBOX_ONLY")]
        public virtual bool? MultiboxOnly { get; set; }
        /// <summary>
        /// 是否组合排序
        /// </summary>
        [Column("IS_MULTI_SORT")]
        public virtual bool? IsMultiSort { get; set; }
        /// <summary>
        /// 是否显示头部
        /// </summary>
        [Column("IS_SHOW_HEADER")]
        public virtual bool? IsShowHeader { get; set; }
        /// <summary>
        /// 是否默认显示筛选区
        /// </summary>
        [Column("IS_SHOW_FILTER")]
        public virtual bool? IsShowFilter { get; set; }
        /// <summary>
        /// 自定义js
        /// </summary>
        [Column("JS_FUN")]
        public virtual string JsFun { get; set; }
        /// <summary>
        /// 表格报表说明
        /// </summary>
        [Column("REMARK")]
        public virtual string Remark { get; set; }
    }
}
