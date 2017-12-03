using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Easyman.Common;
using Abp.Domain.Entities;
using System;

namespace Easyman.Domain
{
    /// <summary>
    /// 自定义函数管理
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "SCRIPT_FUNCTION")]
    public class ScriptFunction : Entity<long>
    {
        [Key,Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 函数中文名
        /// </summary>
        [Column("NAME"),StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 函数代码体
        /// </summary>
        [Column("CONTENT")]
        public virtual string Content { get; set; }
        /// <summary>
        /// 函数说明
        /// </summary>
        [Column("REMARK"),StringLength(500)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 函数状态(开启、关闭)
        /// </summary>
        [Column("STATUS")]
        public virtual short? Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATE_TIME")]
        public virtual DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column("USER_ID")]
        public virtual long? CreateUid { get; set; }
        /// <summary>
        /// 编译状态(通过、失败)
        /// </summary>
        [Column("COMPILE_STATUS")]
        public virtual short? CompileStatus { get; set; }
    }
}
