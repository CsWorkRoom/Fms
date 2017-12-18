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
        /// 文件ID
        /// </summary>
        [ForeignKey("FILE_LIBRARY_ID")]
        public virtual FileLibrary FileLibrary { get; set; }

        [ForeignKey("USER_ID")]
        public virtual User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("USER_NAME")]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 认领时间
        /// </summary>
        [Column("CREATE_TIME")]
        public virtual DateTime CreateTime { get; set; }



    }
}
