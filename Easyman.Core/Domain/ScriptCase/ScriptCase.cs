using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Easyman.Common;
using System;
using Abp.Domain.Entities;

namespace Easyman.Domain
{
    /// <summary>
    /// 脚本流实例
    /// </summary>
    [Table(SystemConfiguration.TablePrefix+ "SCRIPT_CASE")]
    public class ScriptCase :Entity<long>
    {
        [Key,Column("ID")]
        public override long Id { get; set ; }
        /// <summary>
        /// 脚本流名称
        /// </summary>
        [Column("NAME"),StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 脚本流ID
        /// </summary>
        [Column("SCRIPT_ID")]
        public virtual long? ScriptId { get; set; }
        /// <summary>
        /// 失败重启次数
        /// </summary>
        [Column("RETRY_TIME")]
        public virtual int? RetryTime { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        [Column("START_TIME")]
        public virtual DateTime? StartTime { get; set; }
        /// <summary>
        /// 启动模式(自动、手工)
        /// </summary>
        [Column("START_MODEL")]
        public virtual short? StartModel { get; set; }
        /// <summary>
        /// 启动人
        /// </summary>
        [Column("USER_ID")]
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 运行状态(等待、执行中、停止)
        /// </summary>
        [Column("RUN_STATUS")]
        public virtual short? RunStatus { get; set; }
        /// <summary>
        /// 是否有失败节点
        /// </summary>
        [Column("IS_HAVE_FAIL")]
        public virtual short? IsHaveFail { get; set; }
        /// <summary>
        /// 结束标识(成功、失败)
        /// </summary>
        [Column("RETURN_CODE")]
        public virtual short? ReturnCode { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Column("END_TIME")]
        public virtual DateTime? EndTime { get; set; }
    }
}
