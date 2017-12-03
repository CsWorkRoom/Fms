using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Easyman.Common;

namespace Easyman.Domain
{
    /// <summary>
    /// 脚本类型
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "SCRIPT_TYPE")]
    public class ScriptType:CommonEntityHelper
    {
        [Key,Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>
        [Column("NAME"),StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"),StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
