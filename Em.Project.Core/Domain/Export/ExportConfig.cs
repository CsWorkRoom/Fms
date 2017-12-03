using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 导出配置
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "EXPORT_CONFIG")]
    public class ExportConfig : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 归属应用
        /// </summary>
        [Column("APP"), StringLength(50)]
        public virtual string App { get; set; }
        /// <summary>
        /// 导出的文件目录
        /// </summary>
        [Column("PATH"), StringLength(200)]
        public virtual string Path { get; set; }
        /// <summary>
        /// 文件有效时间
        /// </summary>
        [Column("VALID_DAY")]
        public virtual int? ValidDay { get; set; }
        /// <summary>
        /// 大文件定义
        /// </summary>
        [Column("DATA_SIZE")]
        public virtual int? DataSize { get; set; }
        /// <summary>
        /// 最大导出时长（毫秒）
        /// </summary>
        [Column("MAX_TIME")]
        public virtual int? MaxTime { get; set; }
        /// <summary>
        /// 单文件最大行数
        /// </summary>
        [Column("MAX_ROW_NUM")]
        public virtual int? MaxRowNum { get; set; }
        /// <summary>
        /// 抽样最大等待时长(毫秒)
        /// </summary>
        [Column("WAIT_TIME")]
        public virtual int? WaitTime { get; set; }
    }
}
