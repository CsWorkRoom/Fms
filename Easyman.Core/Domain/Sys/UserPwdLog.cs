using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Easyman.Common;
using System.Collections.Generic;

namespace Easyman.Domain
{
    [Table("EM_USER_PWD_LOG")]
    public class UserPwdLog : NotDeleteEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 用户编号：USER_ID
        /// </summary>
        [Column("USER_ID")]
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 原密码
        /// </summary>
        [Column("OLD_PWD"), StringLength(128)]
        public virtual string OldPwd { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Column("NEW_PWD"), StringLength(128)]
        public virtual string NewPwd { get; set; }
       
    }
}
