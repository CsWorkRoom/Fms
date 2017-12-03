using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 导出数据生成记录表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "EXPORT_DATA")]
    public class ExportData : Entity<long>
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 模版页ID
        /// </summary>
        [Column("MODULE_ID")]
        public virtual long? ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }
        /// <summary>
        /// 报表代码CODE
        /// </summary>
        [Column("REPORT_CODE"),StringLength(50)]
        public virtual string ReportCode { get; set; }
        /// <summary>
        /// 事件来自哪个url
        /// </summary>
        [Column("FROM_URL"), StringLength(200)]
        public virtual string FromUrl { get; set; }
        /// <summary>
        /// 导出发起人
        /// </summary>
        [Column("USER_ID")]
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 导出类型
        /// </summary>
        [Column("EXPORT_WAY"),StringLength(10)]
        public virtual string ExportWay { get; set; }
        /// <summary>
        /// 文件显示名（动态生成）
        /// </summary>
        [Column("DISPLAY_NAME"), StringLength(50)]
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 执行sql（解析之后的）
        /// </summary>
        [Column("SQL")]
        public virtual string Sql { get; set; }
        /// <summary>
        /// 执行库
        /// </summary>
        [Column("DB_SERVER_ID")]
        public virtual long? DbServerId { get; set; }
        /// <summary>
        /// 多表头信息
        /// </summary>
        [Column("TOP_FIELDS")]
        public virtual string TopFields { get; set; }
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
        /// 生成状态（生成中/生成成功/生成失败）
        /// </summary>
        [Column("STATUS"), StringLength(50)]
        public virtual string Status { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [Column("FILE_SIZE")]
        public virtual int? FileSize { get; set; }
        /// <summary>
        /// 文件格式
        /// </summary>
        [Column("FILE_FORMAT"), StringLength(20)]
        public virtual string FileFormat { get; set; }

        [Column("FILES_ID")]
        public virtual long? FilesId { get; set; }
        /// <summary>
        /// 生成开始时间
        /// </summary>
        [Column("BEGIN_TIME")]
        public virtual DateTime? BeginTime { get; set; }
        /// <summary>
        /// 生成结束时间
        /// </summary>
        [Column("END_TIME")]
        public virtual DateTime? EndTime { get; set; }
        /// <summary>
        /// 有效时间(天)
        /// </summary>
        [Column("VALID_DAY")]
        public virtual int? ValidDay { get; set; }
        /// <summary>
        /// 是否失效
        /// </summary>
        [Column("IS_INVALID")]
        public virtual bool? IsInvalid { get; set; }
        /// <summary>
        /// 是否关闭下载
        /// </summary>
        [Column("IS_CLOSE")]
        public virtual bool? IsClose { get; set; }
        /// <summary>
        /// 关闭人
        /// </summary>
        [Column("CLOSER")]
        public virtual long? Closer { get; set; }
        /// <summary>
        /// 关闭时间
        /// </summary>
        [Column("CLOSE_TIME")]
        public virtual DateTime? CloseTime { get; set; }

    }
}
