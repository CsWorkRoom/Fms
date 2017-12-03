using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Easyman.Authorization;
using Easyman.Authorization.Roles;
using Easyman.DefaultData;
using Easyman.EntityFramework;
using Easyman.Users;
using System.Linq;

namespace Easyman.Migrations.SeedData
{
    public class TenantRoleAndUserBuilder
    {
        private readonly EmProjectDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(EmProjectDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            //Admin role

            var adminRole = _context.Roles.FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticDatas.Host.AdminRole.Name);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticDatas.Host.AdminRole.Name, StaticDatas.Host.AdminRole.Name) { IsStatic = true });
                _context.SaveChanges();

                //Grant all permissions to admin role
                var permissions = PermissionFinder
                    .GetAllPermissions(new EasymanAuthorizationProvider())
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant))
                    .ToList();

                foreach (var permission in permissions)
                {
                    _context.Permissions.Add(
                        new RolePermissionSetting
                        {
                            TenantId = _tenantId,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRole.Id
                        });
                }

                _context.SaveChanges();
            }

            //admin user

            var adminUser = _context.Users.FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == User.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com", "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
        }
    }
}