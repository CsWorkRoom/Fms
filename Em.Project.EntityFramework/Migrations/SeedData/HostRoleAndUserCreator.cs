using System.Linq;
using Abp.Authorization;
using Abp.MultiTenancy;
using Easyman.Authorization;
using Easyman.EntityFramework;
using Microsoft.AspNet.Identity;
using Easyman.DefaultData;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Easyman.Authorization.Roles;
using Easyman.Users;

namespace Easyman.Migrations.SeedData
{
    public class HostRoleAndUserCreator
    {
        private readonly EmProjectDbContext _context;

        public HostRoleAndUserCreator(EmProjectDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
        }

        private void CreateHostRoleAndUsers()
        {
            //Admin role for host

            var adminRoleForHost = _context.Roles.FirstOrDefault(r => r.TenantId == null && r.Name == StaticDatas.Host.AdminRole.Name);
            if (adminRoleForHost == null)
            {
                adminRoleForHost = _context.Roles.Add(new Role { Name = StaticDatas.Host.AdminRole.Name, DisplayName = StaticDatas.Host.AdminRole.Name, IsStatic = true });
                _context.SaveChanges();

                //Grant all tenant permissions
                var permissions = PermissionFinder
                    .GetAllPermissions(new EasymanAuthorizationProvider())
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host))
                    .ToList();

                foreach (var permission in permissions)
                {
                    _context.Permissions.Add(
                        new RolePermissionSetting
                        {
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRoleForHost.Id
                        });
                }

                _context.SaveChanges();
            }

            //Admin user for tenancy host

            //var adminUserForHost = _context.Users.FirstOrDefault(u => u.TenantId == null && u.UserName == User.AdminUserName);
            var adminUserForHost = _context.Users.FirstOrDefault(u => u.TenantId == null && u.UserName == "system");
            if (adminUserForHost == null)
            {
                adminUserForHost = _context.Users.Add(
                    new User
                    {
                        //UserName = User.AdminUserName,
                        UserName = "system",
                        Name = "System",
                        Surname = "Administrator",
                        EmailAddress = "admin@aspnetboilerplate.com",
                        IsEmailConfirmed = true,
                        Password= new PasswordHasher().HashPassword(Common.OperateSection.GetPwdRuleSection().DefualtPwd)
                        //Password = new PasswordHasher().HashPassword(User.DefaultPassword)
                    });

                _context.SaveChanges();

                _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));

                _context.SaveChanges();
            }
        }
    }
}