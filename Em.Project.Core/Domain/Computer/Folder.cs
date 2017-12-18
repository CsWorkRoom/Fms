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
    public class Folder : NotDeleteEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 终端ID
        /// </summary>
        [Column("COMPUTER_ID")]
        public virtual long? ComputerId { get; set; }
        [ForeignKey("ComputerId")]
        public virtual Computer Computer { get; set; }

        /// <summary>
        /// 共享文件名
        /// </summary>
        [Column("NAME"), StringLength(100)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 权限信息
        /// </summary>
        [Column("POWER_MSG"), StringLength(100)]
        public virtual string PowerMsg { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column("IS_USE")]
        public virtual bool? IsUse { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
