using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using System;

namespace Easyman.Domain
{
    /// <summary>
    /// 手工触发记录表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "HAND_RECORD")]
    public class HandRecord : Entity<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 手工类型(脚本流、脚本节点)
        /// </summary>
        [Column("HAND_TYPE")]
        public virtual short? HandType { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        [Column("USER_ID")]
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 承载编号(脚本流ID或脚本节点ID)
        /// </summary>
        [Column("OBJECT_ID")]
        public virtual long? ObjectId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Column("ADD_TIME")]
        public virtual DateTime? AddTime { get; set; }
        /// <summary>
        /// 是否处理(由服务自动判断)
        /// </summary>
        [Column("IS_CANCEL")]
        public virtual short? IsCancel { get; set; }
        /// <summary>
        /// 处理说明(服务自动备注)
        /// </summary>
        [Column("CANCEL_REASON"), StringLength(200)]
        public virtual string CancelReason { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        [Column("START_TIME")]
        public virtual DateTime? StartTime { get; set; }
        /// <summary>
        /// 承载编号(脚本流ID或脚本节点ID)
        /// </summary>
        [Column("OBJECT_CASE_ID")]
        public virtual long? ObjectCaseId { get; set; }
    }
}
