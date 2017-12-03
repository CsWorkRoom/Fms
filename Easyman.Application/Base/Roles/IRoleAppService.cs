using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Authorization.Roles;
using Easyman.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Easyman.Service
{
    public interface IRoleAppService : IApplicationService
    {
        #region 查询

        Role GetRole(int id);

        Role GetRole(string name);

        RoleSearchOutput GetRoleSearch(NavigationSerachInput input);
        #endregion

        #region 返回角色 Tree 的 json

        /// <summary>
        /// 获取角色树的json字符串
        /// </summary>
        /// <param name="ids">选中的ID字符串，以“,”隔开</param>
        /// <returns></returns>
        Task<IEnumerable<object>> GetRoleTreeJson(string ids = "");

        /// <summary>
        /// 获取角色树的json字符串
        /// </summary>
        /// <param name="navId">菜单ID</param>
        /// <returns></returns>
        Task<IEnumerable<object>> GetRoleTreeJsonByNavId(int navId);

        /// <summary>
        /// 获取角色树的json字符串
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Task<IEnumerable<object>> GetRoleTreeJsonByUserId(int userId);

        /// <summary>
        /// 获取角色树的json字符串
        /// </summary>
        /// <param name="navId">菜单ID</param>
        /// <returns></returns>
        Task<IEnumerable<object>> GetRoleTreeJsonByFunId(int funId);
        #endregion

        #region 操作（ 新增，编辑，删除）
        void SavePost(RoleInput input);

        void DeletePost(EntityDto input);
        #endregion
    }
}
