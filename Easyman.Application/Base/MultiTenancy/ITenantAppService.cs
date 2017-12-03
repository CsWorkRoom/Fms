using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.MultiTenancy.Dto;
using System.Collections.Generic;

namespace Easyman.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);

        Task<IEnumerable<object>> GetTenantTreeJson();
    }
}
