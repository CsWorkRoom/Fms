using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 数据下载记录表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "DOWN_DATA")]
    public class DownData : Entity<long>
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 数据生成记录ID
        /// </summary>
        [Column("EXPORT_DATA_ID")]
        public virtual long? ExportDataId { get; set; }

        [ForeignKey("ExportDataId")]
        public virtual ExportData ExportData { get; set; }
        /// <summary>
        /// 下载人
        /// </summary>
        [Column("USER_ID")]
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 下载开始时间
        /// </summary>
        [Column("DOWN_BEGIN_TIME")]
        public virtual DateTime? DownBeginTime { get; set; }
        /// <summary>
        /// 下载结束时间
        /// </summary>
        [Column("DOWN_END_TIME")]
        public virtual DateTime? DownEndTime { get; set; }
        /// <summary>
        /// 下载状态（成功、失败）
        /// </summary>
        [Column("STATUS"), StringLength(50)]
        public virtual string Status { get; set; }
        /// <summary>
        /// 文件显示名（动态生成）
        /// </summary>
        [Column("DISPLAY_NAME"), StringLength(50)]
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        [Column("FILE_NAME"), StringLength(50)]
        public virtual string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [Column("FILE_PATH"), StringLength(200)]
        public virtual string FilePath { get; set; }
        /// <summary>
        /// 文件大小(kb)
        /// </summary>
        [Column("FILE_SIZE")]
        public virtual int? FileSize { get; set; }
    }
}
