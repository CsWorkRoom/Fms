using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Domain;
using Easyman.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Easyman.Sys
{
    public interface IModulesAppService : IApplicationService
    {
        IEnumerable<MenuItem> GetNavigationByCurrentUser();

        NavigationSerachOutput GetNavsSearch(NavigationSerachInput input);

        NavigationSerachOutput GetAppMenuSearch(NavigationSerachInput input);

        Task<IEnumerable<object>> GetNavTreeJson(string applicationType = "");

        Task<IEnumerable<object>> GetNavTreeJsonByRoleId(long roleId);

        Task<IEnumerable<object>> GetNavTreeJsonByRoleIdForModule(long roleId);

        Module GetNavigation(int id);

        Module GetNavigation(Expression<Func<Module, bool>> predicate);

        Task<List<Module>> GetModuleList(Expression<Func<Module, bool>> predicate);

        List<ModuleEvent> GetModuleEventList(Expression<Func<ModuleEvent, bool>> predicate);

        IEnumerable<RoleModuleEvent> GetRoleModuleEvent(long eventId);

        void SaveNavigationEdit(NavigationInput input);

        void SaveAppMenuEdit(NavigationInput input);

        void DeletePost(EntityDto<long> input);

        /// <summary>
        /// 校验是否具有页面权限
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool ValidateUrlRole(string url);
        /// <summary>
        /// 根据url获取归属的module
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Module GetModuleByUrl(string url);
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        Users.User GetCurUser();
    }
}
