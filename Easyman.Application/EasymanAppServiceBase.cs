using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Easyman.MultiTenancy;
using Easyman.Users;
using Microsoft.AspNet.Identity;
using Castle.Core;

namespace Easyman
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    [Interceptor(typeof(ExceptionInterceptor))]
    public abstract class EasymanAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected EasymanAppServiceBase()
        {
            LocalizationSourceName = EasymanConsts.LocalizationSourceName;
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected long CurrUserId() { return AbpSession.GetUserId(); }
    }
}