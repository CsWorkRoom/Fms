
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 表格报表字段
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "TB_REPORT_FIELD")]
    public class TbReportField : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("REPORT_ID")]
        public virtual long? ReportId { get; set; }

        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        [Column("TB_REPORT_ID")]
        public virtual long? TbReportId { get; set; }

        [ForeignKey("TbReportId")]
        public virtual TbReport TbReport { get; set; }

        [Column("TB_REPORT_FIELD_TOP_ID")]
        public virtual long? TbReportFieldTopId { get; set; }

        [ForeignKey("TbReportFieldTopId")]
        public virtual TbReportFieldTop TbReportFieldTop { get; set; }

        [Column("FIELD_CODE"), StringLength(50)]
        public virtual string FieldCode { get; set; }

        [Column("FIELD_NAME"), StringLength(50)]
        public virtual string FieldName { get; set; }

        [Column("DATA_TYPE"), StringLength(20)]
        public virtual string DataType { get; set; }

        [Column("IS_ORDER")]
        public virtual bool IsOrder { get; set; }

        [Column("IS_SHOW")]
        public virtual bool IsShow { get; set; }

        [Column("WIDTH")]
        public virtual int? Width { get; set; }

        [Column("IS_SEARCH")]
        public virtual bool IsSearch { get; set; }

        [Column("IS_FROZEN")]
        public virtual bool IsFrozen { get; set; }

        [Column("ALIGN"),StringLength(20)]
        public virtual string Align { get; set; }

        [Column("ORDER_NUM")]
        public virtual int? OrderNum { get; set; }

        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        [Column("TB_REPORT_OUTEVENT_ID")]
        public virtual long? TbReportOutEventId { get; set; }

    }
}
