using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 文件格式类型
    /// </summary>
    [Table("FM_FILE_FORMAT")]
    public class FileFormat : CommonEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 文件格式类型名
        /// </summary>
        [Column("NAME"), StringLength(50),Required]
        public virtual string Name { get; set; }
        /// <summary>
        /// 文件图标
        /// </summary>
        [Column("ICON"), StringLength(50), Required]
        public virtual string Icon { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
