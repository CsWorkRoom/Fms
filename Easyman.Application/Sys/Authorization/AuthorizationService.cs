using Easyman.Authorization.Roles;
using Easyman.Dto;
using Easyman.Managers;
using Easyman.Users;
using System;
using System.Linq;

namespace Easyman.Sys
{
    public class RoleBaseAuthorizationService : IAuthorizationService
    {

        #region 初始化

        private readonly string[] _adminRoleNames = new[] { "Admin", "admin", "administrator", "Administrator" };
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly ModuleManager _moduleManager;

        public RoleBaseAuthorizationService(ModuleManager moduleManager, RoleManager roleManager, UserManager userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _moduleManager = moduleManager;
        }

        #endregion

        #region 公有方法

        public void CheckAccess(Rolession permission, User user)
        {
            if (!TryCheckAccess(permission, user))
            {
                throw new Exception("安全错误");

            }
        }

        public bool TryCheckAccess(Rolession rolession, User user)
        {
            if (user == null)
            {
                return false;
            }

            //取消admin用所有权限功能，仍然通过赋权得到
            //if (IsAdministrator(user))
            //    return true;

            var nav = _moduleManager.GetNavigation(rolession.Code);

            return nav.RoleModule.Select(s => s.RoleId).Intersect(user.Roles.Select(s => s.RoleId)).Any();
        }


        private string _isAdmin = "";
        public bool IsAdministrator(User user)
        {
            if (_isAdmin == "")
            {
                var adminRoleIdList = _adminRoleNames.Select(s => _roleManager.FindByNameAsync(s).Result).Where(w => w != null).Select(s => s.Id);

                _isAdmin = adminRoleIdList.Intersect(user.Roles.Select(s => s.RoleId)).Any() ? "Y" : "N";
            }

            return _isAdmin == "Y";
        }

        #endregion

    }
}
