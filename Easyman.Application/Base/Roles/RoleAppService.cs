using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.UI;
using Easyman.Authorization.Roles;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easyman.Service
{
    public class RoleAppService : ApplicationService, IRoleAppService
    {

        #region 初始化

        private readonly RoleManager _roleManager;
        private readonly RoleStore _roleStore;
        private readonly ModuleManager _moduleManager;

        public RoleAppService(
            RoleManager roleManager, RoleStore roleStore, ModuleManager moduleManager)
        {
            _roleManager = roleManager;
            _roleStore = roleStore;
            _moduleManager = moduleManager;
        }

        #endregion

        #region 公有方法

        #region 查询

        public Role GetRole(int id)
        {
            return _roleManager.GetRoleByIdAsync(id).Result;
        }

        public Role GetRole(string name)
        {
            return _roleManager.GetRoleByNameAsync(name).Result;
        }

        public RoleSearchOutput GetRoleSearch(NavigationSerachInput input)
        {
            var parentSearch = input.SearchList.FirstOrDefault(f => f.Name == "ParentName");

            if (parentSearch != null)
            {
                input.SearchList.Remove(parentSearch);
                parentSearch.Name = "Parent.Name";
                input.SearchList.Add(parentSearch);
            }

            var rowCount = 0;
            var navs = _roleStore.Query.SearchByInputDto(input, out rowCount);
            var outPut = new RoleSearchOutput
            {
                Datas = navs.ToList().Select(s => s.MapTo<RoleOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        #endregion

        #region 返回角色 Tree 的 json
        public Task<IEnumerable<object>> GetRoleTreeJson(string ids = "")
        {
            return _roleStore.GetRoleTreeJson(ids);
        }

        public Task<IEnumerable<object>> GetRoleTreeJsonByNavId(int navId)
        {
            return _roleStore.GetRoleTreeJsonByNavId(navId);
        }

        public Task<IEnumerable<object>> GetRoleTreeJsonByUserId(int userId)
        {
            return _roleStore.GetRoleTreeJsonByUserId(userId);
        }

        public Task<IEnumerable<object>> GetRoleTreeJsonByFunId(int funId)
        {
            return _roleStore.GetRoleTreeJsonByFunId(funId);
        }
        #endregion

        #region 操作（ 新增，编辑，删除）

        public void SavePost(RoleInput input)
        {
            try
            {
                var role = _roleManager.FindById(input.Id) ?? new Role();

                role.Name = input.Name;
                role.DisplayName = input.DisplayName;
                role.ParentId = input.ParentId;
                role.TenantId = input.TenantId;

                if (input.Id == 0)
                    _roleManager.Create(role);
                else
                    _roleManager.Update(role);



                _roleStore.SetNavs(role.Id, input.ParentNavIds);
                _moduleManager.SetEvent(role.Id, input.ChildNavIds);
                //_roleStore.SetFuns(role.Id, input.FunIds);
                CurrentUnitOfWork.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public void DeletePost(EntityDto input)
        {
            var role = _roleManager.GetRoleByIdAsync(input.Id).Result;

            if (role != null)
            {
                if (_roleStore.GetNavgationRoleByRoleId(role.Id).Result.Any())
                {
                    throw new UserFriendlyException("角色有菜单关联，不能删除");
                }

                if (_roleStore.GetUserRoleByRoleId(role.Id).Result.Any())
                {
                    throw new UserFriendlyException("角色有用户关联，不能删除");
                }


                _roleManager.DeleteAsync(role);
            }
        }
        #endregion

        #endregion

    }
}
