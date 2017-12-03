using Abp.Dependency;
using Easyman.Dto;
using Easyman.Users;

namespace Easyman.Sys
{
    public interface IAuthorizationService : ITransientDependency
    {
        void CheckAccess(Rolession permission, User user);
        bool TryCheckAccess(Rolession rolession, User user);
        bool IsAdministrator(User user);
    }
}
