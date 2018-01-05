
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 监控日志表
    /// </summary>
    [Table("FM_MONIT_LOG")]
    public class MonitLog : Entity<long>
    {
        
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 监控版本ID
        /// </summary>
        [Column("CASE_VERSION_ID")]
        public virtual long? CaseVersionId { get; set; }

        /// <summary>
        /// 文件及文件夹ID
        /// </summary>
        [Column("MONIT_FILE_ID")]
        public virtual long? MonitFileId { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        [Column("LOG_TYPE")]
        public virtual short? LogType { get; set; }


        /// <summary>
        /// 日志信息
        /// </summary>
        [Column("LOG_MSG")]
        public virtual string LogMsg { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        [Column("LOG_TIME")]
        public virtual DateTime? LogTime { get; set; }

    }
}
