using Abp.Application.Services;
using Easyman.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Easyman.Dto;
using Abp.Application.Services.Dto;
using Easyman.Users;

namespace Easyman.Sys
{
    public interface IUserPwdAppService : IApplicationService
    {
        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetUser(long id);
        /// <summary>
        /// 根据name获取用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        User GetUser(string name);

        void InsertOrUpdate(User user);

        /// <summary>
        /// 插入一条密码修改记录
        /// </summary>
        /// <param name="pwdLog"></param>
        void InsertUserPwdLog(UserPwdLogDto pwdLog);

        /// <summary>
        /// 获取用户的所有密码修改记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserPwdLogDto> GetAllPwdLog(long userId);
        /// <summary>
        /// 获取用户的最后一条密码修改记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserPwdLogDto GetLastPwdLog(long userId);
    }
}
