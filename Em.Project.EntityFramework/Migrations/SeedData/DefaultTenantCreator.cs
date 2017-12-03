using System.Linq;
using Easyman.EntityFramework;
using Easyman.MultiTenancy;

namespace Easyman.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly EmProjectDbContext _context;

        public DefaultTenantCreator(EmProjectDbContext context)
        {
            _context = context;
        }
        
       public void Create()
       {
           CreateUserAndRoles();
       }



       private void CreateUserAndRoles()
       {
           //Default tenant

           var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.TenantName);
           if (defaultTenant == null)
           {
               _context.Tenants.Add(new Tenant {TenancyName = Tenant.TenantName, Name = Tenant.TenantName });
               _context.SaveChanges();
           }
       }
    }
}
