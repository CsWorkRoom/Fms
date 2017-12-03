using Abp.Authorization;
using Easyman.Authorization.Roles;
using Easyman.MultiTenancy;
using Easyman.Users;

namespace Easyman.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
