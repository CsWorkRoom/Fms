using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 表格报表外置事件
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "TB_REPORT_OUTEVENT")]
    public class TbReportOutEvent : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("TB_REPORT_ID")]
        public virtual long? TbReportId { get; set; }

        [ForeignKey("TbReportId")]
        public virtual TbReport TbReport { get; set; }

        [Column("EVENT_TYPE"), StringLength(20)]
        public virtual string EventType { get; set; }

        [Column("FIELD_CODE"), StringLength(50)]
        public virtual string FieldCode { get; set; }

        [Column("DISPLAY_WAY"), StringLength(20)]
        public virtual string DisplayWay { get; set; }

        [Column("DISPLAY_CONDITION"), StringLength(500)]
        public virtual string DisplayCondition { get; set; }

        [Column("OPEN_WAY"), StringLength(50)]
        public virtual string OpenWay { get; set; }

        [Column("URL"), StringLength(2000)]
        public virtual string Url { get; set; }

        [Column("DISPLAY_NAME"), StringLength(20)]
        public virtual string DisplayName { get; set; }

        [Column("ICON"), StringLength(50)]
        public virtual string Icon { get; set; }

        [Column("STYLE"), StringLength(50)]
        public virtual string Style { get; set; }

        [Column("TITLE"), StringLength(50)]
        public virtual string Title { get; set; }

        [Column("HEIGHT")]
        public virtual int? Height { get; set; }

        [Column("WIDTH")]
        public virtual int? Width { get; set; }

        [Column("GROUP_NAME"),StringLength(50)]
        public virtual string GroupName { get; set; }

        [Column("ORDER_NUM")]
        public virtual int? OrderNum { get; set; }

    }
}
