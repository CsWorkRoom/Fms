using Abp.Authorization.Users;
using Abp.Extensions;
using Easyman.Domain;
using Easyman.MultiTenancy;
using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Easyman.Common;

namespace Easyman.Users
{
    public class User : AbpUser<User>
    {
        public User()
        {
            //改为在配置文件中获取默认密码
            Password = new PasswordHasher().HashPassword(OperateSection.GetPwdRuleSection().DefualtPwd);
            //Password = new PasswordHasher().HashPassword(DefaultPassword);
        }

        //public string DefaultPassword = OperateSection.GetPwdRuleSection().DefualtPwd;

        //public const string DefaultPassword = "123qwe";

        [Column("DISTRICT_ID")]
        public virtual long? DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public virtual District  District { get; set; }

        [Column("PHONE_NO"),StringLength(100)]
        public virtual string PhoneNo { get; set; }

        [Column("DEPARTMENT_ID")]
        public virtual long? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [Column("MODIFY_PWD")]
        public virtual int ModifyPwd { get; set; }

        [Column("PROJECT_FLY")]
        public virtual int ProjectFlg { get; set; }

        [Column("LOGIN_FAIL_COUNT")]
        public virtual int LoginFailCount { get; set; }

        [Column("LOCKED_REASON")]
        public virtual string LockedReason { get; set; }


        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };
        }
    }
}