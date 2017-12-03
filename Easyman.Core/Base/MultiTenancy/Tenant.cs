using Abp.MultiTenancy;
using Easyman.Users;

namespace Easyman.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public const string TenantName = "监控平台";

        public Tenant()
        {

        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
            
        }
    }
}