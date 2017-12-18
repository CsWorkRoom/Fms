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
    public class MonitFile : CommonEntityHelper
    {
        
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 版本更新批次号ID
        /// </summary>
        [ForeignKey("FOLDER_VERSION_ID")]
        public virtual FolderVersion FolderVersion { get; set; }
       /// <summary>
        /// 所属终端ID
        /// </summary>
        [Column("COMPUTER_ID")]
        public virtual long ComputerId { get; set; }
        /// <summary>
        /// FOLDER_ID
        /// </summary>
        [Column("FOLDER_ID")]
        public virtual long FolderId { get; set; }
        /// <summary>
        /// 上个依赖ID
        /// </summary>
        [Column("MONIT_FILE_ID")]
        public virtual long MonitFileId { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        [Column("PARENT_ID")]
        public virtual long ParentMonitFileId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        [Column("NAME")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 文件格式ID
        /// </summary>
        [ForeignKey("FILE_FORMAT_ID")]
        public virtual FileFormat FileFormat { get; set; }
        /// <summary>
        /// FILE_LIBRARY_ID
        /// </summary>
        [Column("FILE_LIBRARY_ID")]
        public virtual long FileLibraryId { get; set; }
        /// <summary>
        /// 客户端路径
        /// </summary>
        [Column("CLIENT_PATH")]
        public virtual string ClientPath { get; set; }
        /// <summary>
        /// 服务器路径
        /// </summary>
        [Column("SERVER_PATH")]
        public virtual string ServerPath { get; set; }
        /// <summary>
        /// MD5
        /// </summary>
        [Column("MD5")]
        public virtual string MD5 { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Column("STATUS")]
        public virtual FileStatus FileStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

    }
}
