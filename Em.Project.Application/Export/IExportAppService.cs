using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easyman.Dto;
using Easyman.Domain;

namespace Easyman.Service
{
    /// <summary>
    /// 数据导出管理
    /// </summary>
    public interface IExportAppService : IApplicationService
    {
        string OnlineExportData(ExportDataModel exp);
        string OfflineExportData(ExportDataModel exp, int intCountNum);
        DownData DownLoadRecord(int intFileId);
        #region 导出配置
        ExportConfig GetExportConfig(int id);
        ExportConfig GetExportConfig(string app);
        #endregion
    }
}
