using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 文件夹及文件管理
    /// </summary>
    [Table("FM_MONIT_FILE")]
    public class MonitFile : NotDeleteEntityHelper
    {
        
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 文件夹及文件ID
        /// </summary>
        [Column("FOLDER_VERSION_ID")]
        public virtual long? FolderVersionId { get; set; }
        [ForeignKey("FolderVersionId")]
        public virtual FolderVersion FolderVersion { get; set; }

        /// <summary>
        /// 所属终端ID
        /// </summary>
        [Column("COMPUTER_ID")]
        public virtual long? ComputerId { get; set; }

        /// <summary>
        /// 所属终端ID
        /// </summary>
        [Column("FOLDER_ID")]
        public virtual long? FolderId { get; set; }
        [ForeignKey("FolderId")]
        public virtual Folder Folder { get; set; }

        /// <summary>
        /// 上个依赖ID
        /// </summary>
        [Column("RELY_MONIT_FILE_ID")]
        public virtual long? RelyMonitFileId { get; set; }
        //[ForeignKey("RelyMonitFileId")]
        //public virtual MonitFile RelyMonitFile { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        [Column("PARENT_ID")]
        public virtual long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual MonitFile Parent { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [Column("NAME"),StringLength(100)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 文件格式ID
        /// </summary>
        [Column("FILE_FORMAT_ID")]
        public virtual long? FileFormatId { get; set; }
        [ForeignKey("FileFormatId")]
        public virtual FileFormat FileFormat { get; set; }

        /// <summary>
        /// 文件库ID
        /// </summary>
        [Column("FILE_LIBRARY_ID")]
        public virtual long? FileLibraryId { get; set; }

        /// <summary>
        /// 客户端路径
        /// </summary>
        [Column("CLIENT_PATH"), StringLength(100)]
        public virtual string ClientPath { get; set; }
        /// <summary>
        /// 服务器路径
        /// </summary>
        [Column("SERVER_PATH"), StringLength(100)]
        public virtual string ServerPath { get; set; }
        /// <summary>
        /// MD5
        /// </summary>
        [Column("MD5"), StringLength(100)]
        public virtual string MD5 { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Column("STATUS")]
        public virtual short? Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
