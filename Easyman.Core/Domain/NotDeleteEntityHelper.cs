using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Abp.Domain.Entities;

namespace Easyman.Common
{
    /// <summary>
    /// 不含有删除审计接口
    /// </summary>
    public class NotDeleteEntityHelper : Entity<long>, IAudited
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
