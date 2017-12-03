using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Easyman.Domain;
using Easyman.Users;
using EasyMan;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Easyman.Authorization.Roles
{
    public class RoleStore : AbpRoleStore<Role, User>
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionSettingRepository;
        private readonly IRepository<RoleModule, long> _rolemoduleRepository;
        private readonly IRepository<RoleModuleEvent, long> _rolemoduleeventRepository;
        private readonly IRepository<FunctionRole, long> _funRoleRepository;

        public RoleStore(
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository,
            IRepository<RoleModule, long> rolemoduleRepository,
            IRepository<FunctionRole, long> funRoleRepository)
            : base(
                roleRepository,
                userRoleRepository,
                rolePermissionSettingRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionSettingRepository = rolePermissionSettingRepository;
            _rolemoduleRepository = rolemoduleRepository;
            _funRoleRepository = funRoleRepository;
        }

        public IQueryable<Role> Query
        {
            get { return _roleRepository.GetAll(); }
        }

        public Task<IQueryable<Role>> QueryAsync
        {
            get { return Task.FromResult(_roleRepository.GetAll()); }
        }

        public async Task<IEnumerable<object>> GetRoleTreeJsonByNavId(int navId)
        {
            var navRoleList = _rolemoduleRepository.GetAll().Where(w => w.ModuleId == navId).ToList();

            if (!navRoleList.Any())
            {
                return await GetRoleTreeJson();
            }
            var roleIds = navRoleList.Select(s => s.RoleId.ToString(CultureInfo.InvariantCulture)).ToList().Aggregate((a, b) => a + "," + b);
            return await GetRoleTreeJson(roleIds);
        }

        public async Task<IEnumerable<object>> GetRoleTreeJsonByFunId(int funId)
        {
            var funRoleList = _funRoleRepository.GetAll().Where(w => w.FunId == funId).ToList();

            if (!funRoleList.Any())
            {
                return await GetRoleTreeJson();
            }
            var roleIds = funRoleList.Select(s => s.RoleId.ToString(CultureInfo.InvariantCulture)).ToList().Aggregate((a, b) => a + "," + b);
            return await GetRoleTreeJson(roleIds);
        }

        public async Task<IEnumerable<object>> GetRoleTreeJsonByUserId(int userId)
        {
            var userRoleList = _userRoleRepository.GetAll().Where(w => w.UserId == userId).ToList();

            if (!userRoleList.Any())
            {
                return await GetRoleTreeJson();
            }
            var roleIds = userRoleList.Select(s => s.RoleId.ToString(CultureInfo.InvariantCulture)).ToList().Aggregate((a, b) => a + "," + b);
            return await GetRoleTreeJson(roleIds);
        }

        public async Task<IEnumerable<object>> GetRoleTreeJson(string ids = "")
        {
            var idList = ids.Split(',').Where(w => w.HasValue());
            var roles = await _roleRepository.GetAllListAsync();

            var result = roles.Select(s => new
            {
                id = s.Id,
                name = s.DisplayName,
                code = s.Name,
                open = false,
                pId = s.ParentId,
                @checked = idList.Any(a => a == s.Id.ToString(CultureInfo.InvariantCulture)) ? "true" : "false",
                iconSkin = s.ChildRoles.Any() ? "folder" : "file"
            }).ToList();

            return result;
        }

        public async Task<IEnumerable<UserRole>> GetUserRoleByRoleId(int roleId)
        {
            return await _userRoleRepository.GetAllListAsync(w => w.RoleId == roleId);
        }

        public async Task<IEnumerable<RoleModule>> GetNavgationRoleByRoleId(int roleId)
        {
            return await _rolemoduleRepository.GetAllListAsync(w => w.RoleId == roleId);
        }

        public void SetNavs(int roleId, string navIds)
        {
            var oldNavRoles = _rolemoduleRepository.GetAllList(w => w.RoleId == roleId);
            var newNavIdList = navIds.HasValue() ?
                navIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt32()).Where(x => x != 0).ToArray() :
                new int[0];

            foreach (var navRole in oldNavRoles)
            {
                var nav = navRole.Module;
                if (nav != null && newNavIdList.All(navId => navId != nav.Id))
                {
                    _rolemoduleRepository.Delete(navRole);
                }
            }

            //Add to added roles
            foreach (var navId in newNavIdList)
            {
                if (oldNavRoles.All(a => a.ModuleId != navId))
                {
                    _rolemoduleRepository.Insert(new RoleModule(navId.ToInt32(), roleId));
                }
            }
        }

        public void SetModules(int roleId, string moduleIds)
        {

        }

        public void SetFuns(int roleId, string funIds)
        {
            var oldFunctionRoles = _funRoleRepository.GetAllList(w => w.RoleId == roleId);
            var newFunctionIdArray = funIds.HasValue() ?
                funIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt32()).Where(x => x != 0).ToArray() :
                new int[0];

            //删除已经去掉的功能权限
            foreach (var funRole in oldFunctionRoles)
            {
                var fun = funRole.Function;
                if (fun != null && newFunctionIdArray.All(funId => funId != fun.Id))
                {
                    _funRoleRepository.Delete(funRole);
                }
            }

            //增加还没有的功能权限
            foreach (var funId in newFunctionIdArray)
            {
                if (oldFunctionRoles.All(a => a.FunId != funId))
                {
                    _funRoleRepository.Insert(new FunctionRole(funId, roleId));
                }
            }
        }
    }
}