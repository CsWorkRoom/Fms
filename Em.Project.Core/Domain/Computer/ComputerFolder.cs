using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 共享文件夹
    /// </summary>
    [Table("FM_FOLDER")]
    public class ComputerFolder : CommonEntityHelper
    {
        
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 终端ID
        /// </summary>
        [ForeignKey("COMPUTER_ID")]
        public virtual Computer Computer { get; set; }

        /// <summary>
        /// 共享文件名
        /// </summary>
        [Column("NAME"), StringLength(100),Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// 权限信息
        /// </summary>
        [Column("POWER_MSG")]
        public virtual string PowerMsg { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
