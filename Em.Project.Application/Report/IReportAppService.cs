using Abp.Application.Services;
using Easyman.Dto;
using EasyMan.Dtos;
using System.Collections.Generic;
using System.Data;

namespace Easyman.Service
{
    /// <summary>
    /// 主报表管理
    /// </summary>
    public interface IReportAppService : IApplicationService
    {
        #region report
        /// <summary>
        /// 根据id获取基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReportOutput GetReportBase(long id);

        /// <summary>
        /// 根据code获取基础信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ReportOutput GetReportBase(string code);

        /// <summary>
        /// 根据报表ID获取一条报表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReportOutput GetReport(long id);

        /// <summary>
        /// 根据报表code获取一条报表信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="checkRole"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        ReportOutput GetReport(string code,long moduleId, bool checkRole);

        /// <summary>
        /// 新增或修改报表信息
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        bool InsertOrUpdateReport(ReportInput report);


        /// <summary>
        /// 解析sql串，得到字段json串
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbserverId"></param>
        /// <returns></returns>
        string AnalysisSql(string sql, long? dbserverId);
        /// <summary>
        /// 根据code代码及传入条件拼凑和执行sql
        /// </summary>
        /// <param name="code"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="queryParams"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        string ExcuteReportSql(string code, int rows, int page,string queryParams, string sidx, string sord, ref ErrorInfo err);
        /// <summary>
        /// 根据code获取当前sql
        /// </summary>
        /// <param name="code"></param>
        /// <param name="queryParams"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        string GetDebugSql(string code, string queryParams, ref ErrorInfo err);
        /// <summary>
        /// 根据code及条件拼凑sql(含需要的字段及显示顺序)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="queryParams"></param>
        /// <param name="tbReportId"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        /// <summary>
        /// 根据传入code执行报表，返回datatable
        /// </summary>
        /// <param name="code"></param>
        /// <param name="queryParams"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        DataTable GetDataTableFromCode(string code, string queryParams, ref ErrorInfo err);
        /// <summary>
        /// 根据code和参数返回sql
        /// </summary>
        /// <param name="code"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        string GetSqlByCode(string code, string queryParams);
        string GetSqlForField(string code, string queryParams, long tbReportId, ref ErrorInfo err);
        string SqlForPage(string dbType, string sql, int pageIndex, int pageSize, ref ErrorInfo err);
        #endregion

    }
}