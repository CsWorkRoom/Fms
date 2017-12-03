using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 主报表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "REPORT")]
    public class Report : CommonEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("DB_SERVER_ID")]
        public virtual long? DbServerId { get; set; }

        [ForeignKey("DbServerId")]
        public virtual DbServer DbServer { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        [Column("CODE"), StringLength(50)]
        public virtual string Code { get; set; }

        [Column("SQL")]
        public virtual string Sql { get; set; }

        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 报表字段信息
        /// </summary>
        [Column("FIELD_JSON")]
        public virtual string FieldJson { get; set; }
        /// <summary>
        /// 是否开启条件替换占位
        /// </summary>
        [Column("IS_PLACEHOLDER")]
        public virtual bool? IsPlaceholder { get; set; }
    }
}
