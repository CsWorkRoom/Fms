using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 更新版本批次
    /// </summary>
    [Table("FM_FOLDER_VERSION")]
    public class FolderVersion : CommonEntityHelper
    {
        
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 共享文件夹ID
        /// </summary>
        [ForeignKey("FOLDER_ID")]
        public virtual ComputerFolder ComputerFolder { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Column("BEGIN_TIME")]
        public virtual DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Column("END_TIME")]
        public virtual DateTime EndTime { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
