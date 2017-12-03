using Abp.Application.Services;
using Easyman.Dto;
using System.Collections.Generic;

namespace Easyman.Service
{
    public interface IRdlcReportAppService : IApplicationService
    {
        /// <summary>
        /// 获得rdlc基础信息
        /// </summary>
        /// <param name="rdlcReportId"></param>
        /// <returns></returns>
        RdlcReportModel GetRdlcReportBase(long rdlcReportId);
        /// <summary>
        /// 根据rdlcReportId获取其详细信息
        /// </summary>
        /// <param name="rdlcReportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// <returns></returns>
        RdlcReportModel GetRdlcReport(long rdlcReportId, long moduleId, bool checkRole);
        /// <summary>
        /// 返回报表列表（按指定类型规范-ChildReportModel）
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// /// <returns></returns>
        IList<ChildReportModel> GetChildListFromRdlcReport(long reportId, long moduleId, bool checkRole);
        /// <summary>
        /// 根据主报表ID获取rdlc报表列表
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// /// <returns></returns>
        IList<RdlcReportModel> GetRdlcReportList(long reportId, long moduleId, bool checkRole);

        /// <summary>
        /// 保存一个rdlc报表
        /// </summary>
        /// <param name="childrReport"></param>
        /// <param name="reportId"></param>
        /// <param name="code"></param>
        void SaveRdlcReport(ChildReportModel childrReport, long reportId, string code);
    }
}
