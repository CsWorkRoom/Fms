using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 监控版本
    /// </summary>
    [Table("FM_CASE_VERSION")]
    public class CaseVersion : CommonEntityHelper
    {
        
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 所属实例
        /// </summary>
        [ForeignKey("CASE_ID")]
        public virtual long CaseId { get; set; }
        /// <summary>
        /// 所属版本
        /// </summary>
        [ForeignKey("FOLDER_VERSION_ID")]
        public virtual FolderVersion FolderVersion { get; set; }

    }
}
