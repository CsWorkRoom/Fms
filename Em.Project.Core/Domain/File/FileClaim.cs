using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using Easyman.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 文件认领
    /// </summary>
    [Table("FM_FILE_CLAIM")]
    public class FileClaim : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 文件夹及文件ID
        /// </summary>
        [Column("MONIT_FILE_ID")]
        public virtual long? MonitFileId { get; set; }
        [ForeignKey("MonitFileId")]
        public virtual MonitFile MonitFile { get; set; }

        /// <summary>
        /// 认领人
        /// </summary>
        [Column("USER_ID")]
        public virtual long? UserId { get; set; }

        /// <summary>
        /// 认领人名
        /// </summary>
        [Column("USER_NAME"),StringLength(50)]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 认领人名
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
