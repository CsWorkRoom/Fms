using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 图表模版管理
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CHART_TEMP")]
    public class ChartTemp : NotDeleteEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 图表种类ID
        /// </summary>
        [Column("CHART_TYPE_ID")]
        public virtual long? ChartTypeId { get; set; }
        [ForeignKey("ChartTypeId")]
        public virtual ChartType ChartType { get; set; }

        [Column("TEMP_TYPE")]
        public virtual short? TempType { get; set; }

        /// <summary>
        /// 模版名
        /// </summary>
        [Column("NAME"), StringLength(50), Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// 模版代码
        /// </summary>
        [Column("TEMP_CODE")]
        public virtual string TempCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public virtual string Remark { get; set; }
    }
}
