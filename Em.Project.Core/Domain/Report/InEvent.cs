using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 内置事件
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "IN_EVENT")]
    public class InEvent : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 报表类型（rdlc、表格、图形...）
        /// </summary>
        [Column("REPORT_TYPE")]
        public virtual short ReportType { get; set; }
        /// <summary>
        /// 事件名
        /// </summary>
        [Column("DISPLAY_NAME"), StringLength(50)]
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 按钮html
        /// </summary>
        [Column("BTN_HTML")]
        public virtual string BtnHtml { get; set; }
        /// <summary>
        /// 按钮js
        /// </summary>
        [Column("BTN_JS")]
        public virtual string BtnJs { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
