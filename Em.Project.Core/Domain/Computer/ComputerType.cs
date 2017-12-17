using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 终端类型
    /// </summary>
    [Table("FM_COMPUTER_TYPE")]
    public class ComputerType : CommonEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 终端类型名
        /// </summary>
        [Column("NAME"), StringLength(50),Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
