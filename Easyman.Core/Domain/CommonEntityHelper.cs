using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Abp.Domain.Entities;

namespace Easyman.Common
{
    /// <summary>
    /// 该类用于对基础资料公共属性管理
    /// </summary>
    public class CommonEntityHelper : Entity<long>, IAudited, IDeletionAudited
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATE_TIME")]
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATE_UID")]
        public virtual long? CreatorUserId { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        [Column("DELETE_UID")]
        public virtual long? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [Column("DELETE_TIME")]
        public virtual DateTime? DeletionTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column("IS_DELETE")]
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        [Column("UPDATE_TIME")]
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后一次修改人
        /// </summary>
        [Column("UPDATE_UID")]
        public virtual long? LastModifierUserId { get; set; }
    }
}
