using Abp.MultiTenancy;
using Easyman.Users;

namespace Easyman.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public const string TenantName = "管理后台";

        public Tenant()
        {

        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
            
        }
    }
}