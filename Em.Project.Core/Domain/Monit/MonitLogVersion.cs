

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 还原和下载批次单
    /// </summary>
    [Table("FM_MONIT_LOG_VERSION")]
    public class MonitLogVersion : Entity<long>
    {
        
        [Key, Column("ID")]
        public override long Id { get; set; }

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
        /// 状态（成功、失败）
        /// </summary>
        [Column("STATUS")]
        public virtual short? Status { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Column("BEGIN_TIME")]
        public virtual DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Column("END_TIME")]
        public virtual DateTime? EndTime { get; set; }

    }
}
