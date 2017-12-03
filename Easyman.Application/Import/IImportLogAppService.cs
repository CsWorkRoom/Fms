using Abp.Application.Services;
using Easyman.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Import
{
    /// <summary>
    /// 导入信息日志
    /// </summary>
    public interface IImportLogAppService : IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        void Add(ImportLogInput input);
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        void UploadFile();
    }
}
