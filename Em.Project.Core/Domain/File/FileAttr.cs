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
        /// 文件ID
        /// </summary>
        [ForeignKey("FILE_LIBRARY_ID")]
        public virtual FileLibrary FileLibrary { get; set; }

        [ForeignKey("ATTR_ID")]
        public virtual Attr Attr { get; set; }

        /// <summary>
        /// 文件属性值
        /// </summary>
        [Column("VALUE")]
        public virtual string Value { get; set; }
       
       
    }
}
