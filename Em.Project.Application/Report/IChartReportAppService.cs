using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    public interface IChartReportAppService : IApplicationService
    {
        #region 图形报表种类
        /// <summary>
        /// 根据ID获取某个类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ChartTypeModel GetChartType(long id);
        /// <summary>
        /// 更新和新增指标类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ChartTypeModel InsertOrUpdateChartType(ChartTypeModel input);

        /// <summary>
        /// 删除一条指标类型
        /// </summary>
        /// <param name="input"></param>
        void DeleteChartType(EntityDto<long> input);
        /// <summary>
        /// 获取指标类型json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetChartTypeTreeJson();
        /// <summary>
        /// 获取所有类型SelectList
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> ChartTypeSelectList();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<ChartTypeModel> ChartTypeList();
        /// <summary>
        /// 获得所有类型json串
        /// </summary>
        /// <returns></returns>
        string ChartTypeJson();
        #endregion

        #region 图形报表模版
        /// <summary>
        /// 根据ID获取某个图表模版
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ChartTempModel GetChartTemp(long id);
        /// <summary>
        /// 更新和新增图表模版
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ChartTempModel InsertOrUpdateChartTemp(ChartTempModel input);

        /// <summary>
        /// 删除一条图表模版
        /// </summary>
        /// <param name="input"></param>
        void DeleteChartTemp(EntityDto<long> input);
        /// <summary>
        /// 获取图表模版json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetChartTempTreeJson();
        /// <summary>
        /// 获取所有 List<SelectListItem>
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> ChartTempList();
        /// <summary>
        /// 获取所有 List<ChartTempModel>
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        string GetChartTempJsonByType(long? chartTypeId);
        #endregion

        #region 图形报表的处理
        /// <summary>
        /// 获得chart基础信息
        /// </summary>
        /// <param name="chartReportId"></param>
        /// <returns></returns>
        ChartReportModel GetChartReportBase(long chartReportId);
        /// <summary>
        /// 根据chartReportId获取其详细信息
        /// </summary>
        /// <param name="chartReportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// <returns></returns>
        ChartReportModel GetChartReport(long chartReportId, long moduleId, bool checkRole);
        /// <summary>
        /// 返回报表列表（按指定类型规范-ChildReportModel）
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// /// <returns></returns>
        IList<ChildReportModel> GetChildListFromChartReport(long reportId, long moduleId, bool checkRole);
        /// <summary>
        /// 根据主报表ID获取chart报表列表
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// /// <returns></returns>
        IList<ChartReportModel> GetChartReportList(long reportId, long moduleId, bool checkRole);

        /// <summary>
        /// 保存一个chart报表
        /// </summary>
        /// <param name="childrReport"></param>
        /// <param name="reportId"></param>
        /// <param name="code"></param>
        void SaveChartReport(ChildReportModel childrReport, long reportId, string code);
        #endregion
    }
}
