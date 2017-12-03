
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 报表全局变量
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "GLOBAL_VAR")]
    public class GlobalVar : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        [Column("VALUE")]
        public virtual string Value { get; set; }

        [Column("REMARK"), StringLength(500)]
        public virtual string Remark { get; set; }

    }
}
