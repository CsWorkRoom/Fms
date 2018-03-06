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
    /// 文件管理
    /// </summary>
    [Table("FM_FILE_LIBRARY")]
    public class FileLibrary : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 文件类型ID
        /// </summary>
        [Column("FILE_FORMAT_ID")]
        public virtual long? FileFormatId { get; set; }
        [ForeignKey("FileFormatId")]
        public virtual FileFormat FileFormat { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Column("NAME"), StringLength(2000)]
        public virtual string Name { get; set; }

        [Column("MD5"), StringLength(2000)]
        public virtual string MD5 { get; set; }

        [Column("SIZE")]
        public virtual double? Size { get; set; }

        /// <summary>
        /// 是否拷贝（1=拷贝、0=未拷贝）
        /// </summary>
        [Column("IS_COPY")]
        public virtual bool? IsCopy { get; set; }

        /// <summary>
        /// 文件说明
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否隐藏文件
        /// </summary>
        [Column("IS_HIDE")]
        public virtual bool? IsHide { get; set; }

    }
}
