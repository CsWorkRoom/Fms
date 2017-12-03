using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Easyman.App.Dto;

namespace Easyman.App
{
    /// <summary>
    /// 用户信息服务接口
    /// </summary>
    public interface IUserInfoAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiUserBean UserSingle(ApiRequestEntityBean request);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiUserBean UserSave(ApiRequestSaveEntityBean<ApiUserBean> request);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiErrorBean UserEditPwd(ApiRequestSaveEntityBean<ApiKeyValueBean> request);

        /// <summary>
        /// 登录时获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiUserBean GetUserLoginInfo(ApiRequestEntityBean request);
    }
}
