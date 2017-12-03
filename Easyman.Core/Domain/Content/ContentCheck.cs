using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Authorization.Roles;
using Easyman.Common;
using Easyman.Users;

namespace Easyman.Domain
{
    /// <summary>
    /// 全选配置记录表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_CHECK")]
    public class ContentCheck : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("CONTENT_ID")]
        public virtual long ContentId { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
        /// <summary>
        /// 是否全选用户
        /// 1true,0false
        /// </summary>
        [Column("IS_CHECK_USER")]
        public virtual bool? IsCheckUser { get; set; }
        /// <summary>
        /// 是否全选角色
        /// </summary>
        [Column("IS_CHECK_ROLE")]
        public virtual bool? IsCheckRole { get; set; }
        /// <summary>
        /// 是否全选组织
        /// </summary>
        [Column("IS_CHECK_DISTRICT")]
        public virtual bool? IsCheckDistrict { get; set; }

        public ContentCheck()
        {

        }

        public ContentCheck(long contentId,bool isUserall,bool isRoleall,bool isDisall)
        {
            ContentId = contentId;
            IsCheckUser = isUserall;
            IsCheckRole = isRoleall;
            IsCheckDistrict = isDisall;
        }
    }
}
