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
        /// 所属实例ID
        /// </summary>
        [Column("SCRIPT_NODE_CASE_ID")]
        public virtual long? ScriptNodeCaseId { get; set; }

        /// <summary>
        /// 文件版本批次ID
        /// </summary>
        [Column("FOLDER_VERSION_ID")]
        public virtual long? FolderVersionId { get; set; }

    }
}
