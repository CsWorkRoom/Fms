using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Easyman.Authorization.Roles;
using Easyman.Editions;
using Easyman.MultiTenancy.Dto;
using Easyman.Users;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.MultiTenancy
{
    public class TenantAppService : EasymanAppServiceBase, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly RoleManager _roleManager;
        private readonly EditionManager _editionManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly IRepository<Tenant> _tenantRepository;

        public TenantAppService(
            TenantManager tenantManager, 
            RoleManager roleManager, 
            EditionManager editionManager, 
            IAbpZeroDbMigrator abpZeroDbMigrator,
            IRepository<Tenant> tenantRepository)
        {
            _tenantManager = tenantManager;
            _roleManager = roleManager;
            _editionManager = editionManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _tenantRepository = tenantRepository;
    }

        public ListResultDto<TenantListDto> GetTenants()
        {
            return new ListResultDto<TenantListDto>(
                _tenantManager.Tenants
                    .OrderBy(t => t.TenancyName)
                    .ToList()
                    .MapTo<List<TenantListDto>>()
                );
        }

        public async Task CreateTenant(CreateTenantInput input)
        {
            //Create tenant
            var tenant = input.MapTo<Tenant>();
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }

            CheckErrors(await TenantManager.CreateAsync(tenant));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new tenant's id.

            //Create tenant database
            _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

            //We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                //Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); //To get static role ids

                //grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                //Create admin user for the tenant
                //var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress, User.DefaultPassword);
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress, Common.OperateSection.GetPwdRuleSection().DefualtPwd);
                CheckErrors(await UserManager.CreateAsync(adminUser));
                await CurrentUnitOfWork.SaveChangesAsync(); //To get admin user's id

                //Assign admin user to role!
                CheckErrors(await UserManager.AddToRoleAsync(adminUser.Id, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<object>> GetTenantTreeJson()
        {
            var builder = new StringBuilder();

            var tenants = await _tenantRepository.GetAllListAsync();

            var tenantNodes = tenants.Select(s => new
            {
                id = s.Id,
                name = s.TenancyName,
                open = false,
                iconSkin = "menu"
            }).ToList();

            return tenantNodes;
        }
    }
}