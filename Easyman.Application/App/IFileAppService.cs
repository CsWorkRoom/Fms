using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Abp.Application.Services;
using Easyman.App.Dto;

namespace Easyman.App
{
    /// <summary>
    /// 文件服务接口
    /// </summary>
    public interface IFileAppService : IApplicationService
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        ApiFileBean FileUp();

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiErrorBean FileDel(ApiRequestEntityBean request);

        /// <summary>
        /// 保存提交的文件
        /// </summary>
        /// <returns></returns>
        ApiFileBean SavePostedSetupFile();
    }
}
