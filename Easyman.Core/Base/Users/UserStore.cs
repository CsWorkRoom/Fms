using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Easyman.Authorization.Roles;
using Easyman.MultiTenancy;
using System.Linq;

namespace Easyman.Users
{
    public class UserStore : AbpUserStore<Role, User>
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        public UserStore(
            IRepository<User, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserClaim, long> userClaimStore,
            IRepository<Tenant> tenantRepository)
            : base(
              userRepository,
              userLoginRepository,
              userRoleRepository,
              roleRepository,
              userPermissionSettingRepository,
              unitOfWorkManager,
              userClaimStore)
        {
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
        }
        public User CreateUserAsync(User user)
        {
            var result = _userRepository.Insert(user);

            return result;
        }

        public User UpdateUserAsync(User user)
        {
            return _userRepository.Update(user);
        }

        public void LoginSucess(string usernameOrEmail, string tenancyName)
        {
            var user = GetUserByUsernameOrEmail(usernameOrEmail, tenancyName);
            if (user != null)
            {
                user.LoginFailCount = 0;
                _userRepository.Update(user);
            }
        }

        public void LoginFaild(string usernameOrEmail, string tenancyName)
        {
            var user = GetUserByUsernameOrEmail(usernameOrEmail, tenancyName);
            if (user != null)
            {
                if (++user.LoginFailCount > 5)
                {
                    user.IsActive = false;
                }
                _userRepository.Update(user);
            }
        }

        public IQueryable<User> Query
        {
            get { return _userRepository.GetAll(); }
        }


        private User GetUserByUsernameOrEmail(string usernameOrEmail, string tenancyName)
        {
            var tenant = _tenantRepository.FirstOrDefault(x => x.TenancyName == tenancyName);
            return _userRepository.FirstOrDefault(x => (x.UserName == usernameOrEmail || x.EmailAddress == usernameOrEmail) && x.TenantId == tenant.Id);
        }
    }
}