using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Users.Dto;
using Easyman.Dto;
using System.Collections.Generic;
using Easyman.App.Dto;

namespace Easyman.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<ListResultDto<UserListDto>> GetUsers();

        Task CreateUser(CreateUserInput input);

        void DeleteUser(EntityDto<long> input);

        List<User> GetAllUser();

        User AddUser(User user);

        UserSearchOutput Search(UserSearchInput input);

        User GetUser(long id);

        /// <summary>
        /// 批量注销账号
        /// </summary>
        /// <param name="input"></param>
        void Delete(EntityDto<string> input);

        /// <summary>
        /// 新增/编辑用户 Id 0：新增 1：编辑
        /// </summary>
        /// <param name="userInput"></param>
        void UpdateOrInserUser(UserInput userInput);

        /// <summary>
        /// 解锁工号
        /// </summary>
        /// <param name="userId"></param>
        void Unlock(EntityDto<long> inout);

        /// <summary>
        /// 锁定工号
        /// </summary>
        /// <param name="userId"></param>
        void Lock(EntityDto<long> inout);

        /// <summary>
        /// 注销账号
        /// </summary>
        /// <param name="inout"></param>
        void Cannel(EntityDto<long> inout);

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="id"></param>
        string ResetPwd(int id);

        

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        byte[] ExportUserBytes(UserSearchInput input);

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns></returns>
        long GetCurrentUserId();


        /// <summary>
        /// 获取当前用户的用户名
        /// </summary>
        /// <returns></returns>
        string GetCurrentUserName();

        /// <summary>
        /// 获取当前系统名
        /// </summary>
        /// <returns></returns>
        string GetCurrentSysName();
    }
}