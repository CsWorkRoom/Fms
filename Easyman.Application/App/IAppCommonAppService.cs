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
    /// APP通用功能服务接口
    /// </summary>
    public interface IAppCommonAppService : IApplicationService
    {
        /// <summary>
        /// 检查版本更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiAppVersionBean CheckUpdate(ApiRequestEntityBean request);
    }
}
