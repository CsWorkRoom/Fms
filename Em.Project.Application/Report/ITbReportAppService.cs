using Abp.Application.Services;
using Easyman.Dto;
using System.Collections.Generic;

namespace Easyman.Service
{
    /// <summary>
    /// 表格式报表管理
    /// </summary>
    public interface ITbReportAppService : IApplicationService
    {
        #region tbreport表格报表APP
        /// <summary>
        /// 根据filterId获取filter对象 
        /// </summary>
        /// <param name="filterId"></param>
        /// <returns></returns>
        ReportFilterModel GetFilter(long filterId);

        /// <summary>
        /// 根据主报表ID获取表格报表列表
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="checkRole"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        IList<TbReportModel> GetTbReportList(long reportId, long moduleId, bool checkRole);

        /// <summary>
        /// 根据tbReportId获取其详细信息
        /// </summary>
        /// <param name="tbReportId"></param>
        /// <param name="checkRole"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        TbReportModel GetTbReport(long tbReportId, long moduleId, bool checkRole);

        /// <summary>
        /// 保存一个表格报表
        /// </summary>
        /// <param name="childrReport"></param>
        /// <param name="reportId"></param>
        /// <param name="code"></param>
        void SaveTbReport(ChildReportModel childrReport, long reportId,string code);

        /// <summary>
        /// 返回报表列表（按指定类型规范-ChildReportModel）
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="checkRole"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        IList<ChildReportModel> GetChildListFromTbReport(long reportId, long moduleId, bool checkRole);
        /// <summary>
        /// 根据tbReportId获取字段配置信息
        /// </summary>
        /// <param name="tbReportId"></param>
        /// <returns></returns>
        IList<TbReportFieldModel> GetFildList(long tbReportId);
        /// <summary>
        /// 根据tbReportId获取字段多表头信息
        /// </summary>
        /// <param name="tbReportId"></param>
        /// <returns></returns>
        IList<TbReportFieldTopModel> GetFildTopList(long tbReportId);

        /// <summary>
        /// 获取文件下载地址
        /// </summary>
        /// <param name="strCode">编号</param>
        /// <param name="strTyle">类型</param>
        /// <param name="strState">状态</param>
        /// <returns></returns>
        string GetFileUrl(string strCode, string strTyle, string strState);

        #endregion
    }
}