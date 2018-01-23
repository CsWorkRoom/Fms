using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 文件属性
    /// </summary>
    [Table("FM_FILE_UPLOAD")]
    public class FileUpload : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
    
        /// <summary>
        /// 上传用户ID
        /// </summary>
        [Column("USER_ID")]
        public virtual int UserId { get; set; }

        /// <summary>
        /// 上传用户名称
        /// </summary>
        [Column("USER_NAME")]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        [Column("UPLOAD_TIME")]
        public virtual DateTime  UploadTime { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Column("FILE_NAME")]
        public virtual string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [Column("FILE_PATH")]
        public virtual string FilePath { get; set; }


    }
}
