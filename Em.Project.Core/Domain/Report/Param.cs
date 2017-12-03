using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 外置事件参数定义
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "PARAM")]
    public class Param : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("TB_REPORT_OUTEVENT_ID")]
        public virtual long? TbReportOutEventId { get; set; }

        [ForeignKey("TbReportOutEventId")]
        public virtual TbReportOutEvent TbReportOutEvent { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        [Column("IS_FIELD")]
        public virtual bool IsField { get; set; }

        [Column("FIELD_CODE"),StringLength(50)]
        public virtual string FieldCode { get; set; }

        [Column("P_VALUE"),StringLength(50)]
        public virtual string PValue { get; set; }

        [Column("REMARK"),StringLength(200)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 参数顺序号。于2017.11.15日添加
        /// </summary>
        [Column("ORDER_NUM")]
        public virtual int? OrderNum { get; set; }

    }
}
