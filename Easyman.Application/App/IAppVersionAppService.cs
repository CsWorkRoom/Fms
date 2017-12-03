using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.App.Dto;
using Easyman.Domain;

namespace Easyman.App
{
    /// <summary>
    /// APP版本管理服务接口
    /// </summary>
    public interface IAppVersionAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有版本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        AppVersionSearchOutput GetAppVersionSearch(AppVersionSearchInput input);

        /// <summary>
        /// 获取版本详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AppVersion GetAppVersion(long id);

        /// <summary>
        /// 修改版本信息
        /// </summary>
        /// <param name="input"></param>
        void SaveAppVersionEdit(AppVersionInput input);

        /// <summary>
        /// 删除版本信息
        /// </summary>
        /// <param name="input"></param>
        void DeleteAppVersion(EntityDto<long> input);
    }
}
