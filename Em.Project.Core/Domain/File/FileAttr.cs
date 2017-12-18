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
    [Table("FM_FILE_ATTR")]
    public class FileAttr : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 文件库ID
        /// </summary>
        [Column("FILE_LIBRARY_ID")]
        public virtual long? FileLibraryId { get; set; }
        [ForeignKey("FileLibraryId")]
        public virtual FileLibrary FileLibrary { get; set; }

        /// <summary>
        /// 文件库ID
        /// </summary>
        [Column("ATTR_ID")]
        public virtual long? AttrId { get; set; }
        [ForeignKey("AttrId")]
        public virtual Attr Attr { get; set; }

        /// <summary>
        /// 文件属性值
        /// </summary>
        [Column("ATTR_VAL"), StringLength(300)]
        public virtual string AttrValue { get; set; }
       
       
    }
}
