using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using System.Collections.Generic;
using System.Linq;
using Easyman.Common;
using System;
using System.Data;
using System.Linq.Expressions;
using Easyman.Common.Helper;
using Abp.UI;
using System.Web.Mvc;
using Abp.Application.Services.Dto;

namespace Easyman.Service
{
    public class ChartReportAppService : EasymanAppServiceBase, IChartReportAppService
    {
        private readonly IRepository<Report, long> _reportRepository;
        private readonly IRepository<ChartReport, long> _chartReportRepository;
        private readonly IRepository<ReportFilter, long> _reportFilterRepository;
        private readonly IRepository<RoleModuleEvent, long> _roleModuleEventRepository;
        private readonly IRepository<Analysis, long> _analysisRepository;
        private readonly IRepository<ModuleEvent, long> _moduleEventRepository;
        private readonly IRepository<InEvent, long> _inEventRepository;//内置事件
        private readonly ModuleManager _moduleManager;
        private readonly IRepository<ChartType, long> _ChartTypeCase;
        private readonly IRepository<ChartTemp, long> _ChartTempCase;

        public ChartReportAppService(
           IRepository<Report, long> reportRepository,
           IRepository<ChartReport, long> chartReportRepository,
           IRepository<ReportFilter, long> reportFilterRepository,
           IRepository<RoleModuleEvent, long> roleModuleEventRepository,
           IRepository<Analysis, long> analysisRepository,
           IRepository<ModuleEvent, long> moduleEventRepository,
           IRepository<InEvent, long> inEventRepository,
           ModuleManager moduleManager,
           IRepository<ChartType, long> ChartTypeCase,
           IRepository<ChartTemp, long> ChartTempCase)
        {
            _reportRepository = reportRepository;
            _chartReportRepository = chartReportRepository;
            _reportFilterRepository = reportFilterRepository;
            _roleModuleEventRepository = roleModuleEventRepository;
            _analysisRepository = analysisRepository;
            _moduleEventRepository = moduleEventRepository;
            _inEventRepository = inEventRepository;
            _moduleManager = moduleManager;
            _ChartTypeCase = ChartTypeCase;
            _ChartTempCase = ChartTempCase;
        }
        #region 图形报表种类
        /// <summary>
        /// 根据ID获取某个图表类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChartTypeModel GetChartType(long id)
        {
            var entObj = _ChartTypeCase.FirstOrDefault(id);
            if (entObj != null)
            {
                return AutoMapper.Mapper.Map<ChartTypeModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【" + id.ToString() + "】的对象！可能已被删除");
        }
        /// <summary>
        /// 更新和新增图表类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ChartTypeModel InsertOrUpdateChartType(ChartTypeModel input)
        {
            if (_ChartTypeCase.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            var entObj = input.MapTo<ChartType>();
            //var entObj= AutoMapper.Mapper.Map<ChartType>(input);
            var resObj = _ChartTypeCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<ChartTypeModel>();
            }
        }

        /// <summary>
        /// 删除一条图表类型
        /// </summary>
        /// <param name="input"></param>
        public void DeleteChartType(EntityDto<long> input)
        {
            try
            {
                _ChartTypeCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取图表类型json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetChartTypeTreeJson()
        {
            var objList = _ChartTypeCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    open = false,
                    iconSkin = "menu"
                }).ToList();
            }
            return null;
        }
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ChartTypeSelectList()
        {
            var objList = _ChartTypeCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();
            }
            return null;
        }

        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        public List<ChartTypeModel> ChartTypeList()
        {
            var objList = _ChartTypeCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.MapTo<List<ChartTypeModel>>();
            }
            else return null;
        }
        /// <summary>
        /// 获得所有类型json串
        /// </summary>
        /// <returns></returns>
        public string ChartTypeJson()
        {
            var objList = _ChartTypeCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return JSON.DecodeToStr(objList.MapTo<List<ChartTypeModel>>());
            }
            else return null;
        }
        #endregion

        #region 图形报表模版
        /// <summary>
        /// 根据ID获取某个图表模版
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChartTempModel GetChartTemp(long id)
        {
            var entObj = _ChartTempCase.FirstOrDefault(id);
            if (entObj != null)
            {
                return AutoMapper.Mapper.Map<ChartTempModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【" + id.ToString() + "】的对象！");
        }
        /// <summary>
        /// 更新和新增图表模版
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ChartTempModel InsertOrUpdateChartTemp(ChartTempModel input)
        {
            if (_ChartTempCase.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            var entObj = input.MapTo<ChartTemp>();
            //var entObj= AutoMapper.Mapper.Map<ChartTemp>(input);
            var resObj = _ChartTempCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<ChartTempModel>();
            }
        }

        /// <summary>
        /// 删除一条图表模版
        /// </summary>
        /// <param name="input"></param>
        public void DeleteChartTemp(EntityDto<long> input)
        {
            try
            {
                _ChartTempCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取图表模版json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetChartTempTreeJson()
        {
            var objList = _ChartTempCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    open = false,
                    iconSkin = "menu"
                }).ToList();
            }
            return null;
        }
        /// <summary>
        /// 获取所有类型 List<SelectListItem>
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> ChartTempList()
        {
            var objList = _ChartTempCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();
            }
            return null;
        }
        /// <summary>
        /// 获取所有类型 List<ChartTempModel>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetChartTempJsonByType(long? chartTypeId)
        {
            var objList = _ChartTempCase.GetAllList(p => 
            (chartTypeId != null && p.ChartTypeId == chartTypeId) ||
            (chartTypeId == null && 1 == 1)).OrderBy(p => p.Id).ToList();
            if (objList != null && objList.Count > 0)
            {
                return JSON.DecodeToStr(
                  objList.Select(p =>
                  {
                      var obj = p.MapTo<ChartTempModel>();
                      obj.ChartTypeName = p.ChartType == null ? "" : p.ChartType.Name;
                      return obj;
                  }).ToList());
            }
            return null;
        }
        #endregion

        #region 图形报表的处理
        /// <summary>
        /// 获得chart基础信息
        /// </summary>
        /// <param name="chartReportId"></param>
        /// <returns></returns>
        public ChartReportModel GetChartReportBase(long chartReportId)
        {
            try
            {
                return _chartReportRepository.Get(chartReportId).MapTo<ChartReportModel>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }

        /// <summary>
        /// 根据chartReportId获取其详细信息
        /// </summary>
        /// <param name="chartReportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// <returns></returns>
        public ChartReportModel GetChartReport(long chartReportId, long moduleId, bool checkRole)
        {
            var ent = _chartReportRepository.Get(chartReportId);
            if (ent != null)
            {
                //赋值基础属性
                var chart = AutoMapper.Mapper.Map<ChartReportModel>(ent);

                chart.IsShowFilter = ent.IsShowFilter == null ? false : ent.IsShowFilter.Value;

                if (checkRole)
                    chart.InEventListJson = GetInEventJson(moduleId, checkRole);//内置事件

                //筛选字段
                chart.FilterListJson = JSON.DecodeToStr(GetFilterList(ent.ReportId.Value, chartReportId));
                return chart;
            }
            return null;
        }
        /// <summary>
        /// 返回报表列表（按指定类型规范-ChildReportModel）
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// /// <returns></returns>
        public IList<ChildReportModel> GetChildListFromChartReport(long reportId, long moduleId, bool checkRole)
        {
            var tbList = GetChartReportList(reportId, moduleId, checkRole);
            if (tbList != null && tbList.Count > 0)
            {
                var childList = new List<ChildReportModel>();
                //循环表格表格，取出其配置信息
                foreach (var ent in tbList)
                {
                    ChildReportModel child = new ChildReportModel();

                    child.ChildReportId = ent.Id;
                    child.IsOpen = ent.IsOpen;
                    child.ChildReportType = (short)ReportEnum.ReportType.Chart;
                    child.ApplicationType = ent.ApplicationType;

                    child.ChildReportJson = JSON.DecodeToStr(ent);

                    childList.Add(child);
                }
                return childList;
            }
            return null;
        }
        /// <summary>
        /// 根据主报表ID获取chart报表列表
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// /// <returns></returns>
        public IList<ChartReportModel> GetChartReportList(long reportId, long moduleId, bool checkRole)
        {
            var entList = _chartReportRepository.GetAllList(p => p.ReportId == reportId);
            if (entList != null & entList.Count > 0)
            {
                var chartReportList = new List<ChartReportModel>();
                //循环chart报表，取出其配置信息
                foreach (var ent in entList)
                {
                    ChartReportModel tb = GetChartReport(ent.Id, moduleId, checkRole);
                    chartReportList.Add(tb);
                }
                return chartReportList;
            }
            return null;
        }

        /// <summary>
        /// 获取当前报表的内置事件
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// <returns></returns>
        private string GetInEventJson(long moduleId, bool checkRole)
        {
            Users.User user = GetCurrentUserAsync().Result;
            var roleids = user.Roles.Select(p => Convert.ToInt64(p.RoleId)).ToList();
            var evids = _roleModuleEventRepository.GetAllList(p => roleids.Contains(p.RoleId) && p.ModuleId == moduleId).Select(p => p.EventId);
            var tbEvs = _moduleEventRepository.GetAllList(p => evids.Contains(p.Id) && p.EventFrom == "内置").Select(p => p.SourceTableId);
            var inEventList = _inEventRepository.GetAllList(p => tbEvs.Contains(p.Id));
            return JSON.DecodeToStr(AutoMapper.Mapper.Map<List<InEvent>, List<InEventModel>>(inEventList));
        }

        /// <summary>
        /// 获取筛选配置集合
        /// 将所有字段拼凑为筛选项（由IsSearch进行区分）
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="ChartReportId"></param>
        /// <returns></returns>
        private IList<ReportFilterModel> GetFilterList(long reportId, long ChartReportId)
        {
            var rpFilterList = new List<ReportFilterModel>();//初始化筛选配置集合

            //获取表格字段的筛选信息
            var ftList = _reportFilterRepository.GetAllList(p => p.ChartReportId == ChartReportId);
            var filterList = AutoMapper.Mapper.Map<List<ReportFilterModel>>(ftList);

            //获取表格报表的字段信息
            var rp = _reportRepository.Get(reportId);
            var fieldList = JSON.EncodeToEntity<List<TbReportFieldModel>>(rp.FieldJson);

            #region 含字段的全量筛选
            if (fieldList != null && fieldList.Count > 0)
            {
                fieldList = fieldList.OrderBy(p => p.OrderNum).ToList();//按查询字段顺序排序
                //循环生成每个字段的筛选
                foreach (var field in fieldList)
                {
                    //生成一个可设置筛选的项
                    ReportFilterModel filter = new ReportFilterModel
                    {
                        Id = 0,
                        ChartReportId = ChartReportId,
                        TbReportId=0,
                        RdlcReportId=0,
                        FieldCode = field.FieldCode,
                        FieldName = field.FieldName,
                        FieldParam = field.FieldCode,//参数名默认为字段编码FieldCode
                        DataType = field.DataType,
                        DefaultValue = null,
                        OrderNum = field.OrderNum == null ? 0 : field.OrderNum,
                        IsQuick = false,//是否快捷筛选
                        FilterSql = "",
                        FilterType = ReportEnum.FilterType.Text.ToString(),//文本框
                        RegularId = null,
                        IsCustom = false,
                        IsSearch = false//默认为不筛选
                    };
                    //是否存在该筛选项。若存在就替换
                    if (filterList != null && filterList.Count > 0)
                    {
                        for (int i = 0; i < filterList.Count; i++)
                        {
                            var ft = filterList[i];
                            //在已有筛选中找到
                            if (filter.FieldCode == ft.FieldCode)
                            {
                                filter = ft;//赋新值
                                filter.OrderNum = ft.OrderNum == null ? 0 : ft.OrderNum;
                                filter.ChartReportId = ft.ChartReportId == null ? 0 : ft.ChartReportId;
                                filter.ChartReportId = ft.ChartReportId == null ? 0 : ft.ChartReportId;
                                filter.TbReportId = ft.TbReportId == null ? 0 : ft.TbReportId;
                                filter.IsCustom = false;
                                break;
                            }
                            ////当为自定的筛选时,直接添加
                            //if (string.IsNullOrEmpty(ft.FieldCode))
                            //{
                            //    ft.IsCustom = true;
                            //    //ft.IsSearch = true;
                            //    rpFilterList.Add(ft);//添加自定义筛选项
                            //}
                        }
                    }
                    rpFilterList.Add(filter);//添加字段筛选项
                }
            }
            #endregion

            #region 追加自定义筛选
            filterList = filterList.Where(p => p.FieldCode == null || p.FieldCode == "").ToList();
            if (filterList != null && filterList.Count > 0)
            {
                foreach (var ft in filterList)
                {
                    //生成一个可设置筛选的项
                    ReportFilterModel filter = new ReportFilterModel
                    {
                        Id = ft.Id,
                        ChartReportId = ChartReportId,
                        TbReportId=0,
                        RdlcReportId=0,
                        FieldCode = ft.FieldCode,
                        FieldName = ft.FieldName,
                        FieldParam = ft.FieldParam,
                        DataType = ft.DataType,
                        DefaultValue = ft.DefaultValue,
                        OrderNum = ft.OrderNum == null ? 0 : ft.OrderNum,
                        IsQuick = ft.IsQuick,
                        FilterSql = ft.FilterSql,
                        FilterType = ft.FilterType,
                        RegularId = ft.RegularId,
                        IsCustom = true,
                        IsSearch = ft.IsSearch,
                        Placeholder = ft.Placeholder
                    };
                    rpFilterList.Add(filter);
                }
            }
            #endregion

            return rpFilterList;
        }

        /// <summary>
        /// 保存一个chart报表
        /// </summary>
        /// <param name="childrReport"></param>
        /// <param name="reportId"></param>
        /// <param name="code"></param>
        public void SaveChartReport(ChildReportModel childrReport, long reportId, string code)
        {
            if (childrReport.ChildReportJson != null && childrReport.ChildReportJson.Length > 0)
            {
                var chartReport = JSON.EncodeToEntity<ChartReportModel>(childrReport.ChildReportJson);
                if (chartReport != null)
                {
                    #region 基础信息修改,得到chartReportId
                    ChartReport chart = AutoMapper.Mapper.Map<ChartReport>(chartReport);
                    chart.ReportId = reportId;
                    chart.ApplicationType = childrReport.ApplicationType;
                    chart.IsOpen = childrReport.IsOpen;
                    chart.Id = childrReport.ChildReportId;
                    chart.MakeWay = 1;//自定义

                    var chartReportId = _chartReportRepository.InsertOrUpdateAndGetId(chart);//修改chart报表并获得ID
                    #endregion
                    SaveChartInEventList(chartReport, reportId, chartReportId, code);//保存内置事件

                    SaveRpFilterList(chartReport, chartReportId);//保存筛选条件信息
                }
            }
        }

        private void SaveChartInEventList(ChartReportModel tbReport, long reportId, long tbReportId, string code)
        {
            //表格解析地址
            var analysis = _analysisRepository.FirstOrDefault(p => p.Url == "Report/ChartReport");

            #region 内置事件保存
            //内置事件列表
            var inEvList = _inEventRepository.GetAllList(p => p.ReportType == (short)ReportEnum.ReportType.Chart).ToList();
            var inEvIds = inEvList.Select(p => p.Id);

            string inlambda = "p=>p.Code==\"" + code + "\" && p.EventFrom==\"内置\" && p.SourceTable==\"EM_INEVENT\" &&p.AnalysisId==" + analysis.Id;
            Expression<Func<ModuleEvent, bool>> exp1 = StringToLambda.LambdaParser.Parse<Func<ModuleEvent, bool>>(inlambda);
            var mdInEvList = _moduleManager.GetModuleEventList(exp1);
            if (mdInEvList != null && mdInEvList.Count > 0)
            {
                //删除不在内置事件列表中的模版事件
                var delInEvList = mdInEvList.Where(p => !inEvIds.Contains(p.SourceTableId)).ToList();
                if (delInEvList != null && delInEvList.Count > 0)
                {
                    foreach (var inEv in delInEvList)
                    {
                        if (analysis != null)
                        {
                            //删除事件解析表事件及赋权信息
                            _moduleManager.DeleteEvent(analysis.Id, code, inEv.SourceTableId);
                        }
                    }
                }
                //查找没有添加的内置事件List
                var insertInEvList = inEvList.Where(p => !mdInEvList.Select(k => k.SourceTableId).Contains(p.Id));
                if (insertInEvList != null && insertInEvList.Count() > 0)
                {
                    //循环添加内置事件
                    foreach (var inev in insertInEvList)
                    {
                        ModuleEvent mdEv = new ModuleEvent
                        {
                            AnalysisId = analysis.Id,
                            Code = code,
                            EventName = inev.DisplayName + "(chart)",
                            EventFrom = "内置",
                            SourceTable = "EM_INEVENT",
                            SourceTableId = inev.Id,
                        };
                        _moduleManager.SaveEvent(mdEv);
                    }
                }
            }
            else//未找到时，添加内置事件
            {
                if (inEvList != null && inEvList.Count > 0)
                {
                    foreach (var inEv in inEvList)
                    {
                        //保存事件至EM_MODULE_EVENT
                        ModuleEvent mdEv = new ModuleEvent
                        {
                            AnalysisId = analysis.Id,
                            Code = code,
                            EventName = inEv.DisplayName+"(chart)",
                            EventFrom = "内置",
                            SourceTable = "EM_INEVENT",
                            SourceTableId = inEv.Id
                        };
                        _moduleManager.SaveEvent(mdEv);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 保存筛选条件信息
        /// 将未设置筛选的字段一起保存（未设置的字段IsSearch = false）
        /// </summary>
        /// <param name="chartReport"></param>
        /// <param name="chartReportId"></param>
        private void SaveRpFilterList(ChartReportModel chartReport, long chartReportId)
        {
            //新增或修改筛选
            if (chartReport.FilterListJson != null && chartReport.FilterListJson.Length > 0)
            {
                //保存说明：无差别式保存.然后在报表解析时去差别处理

                //chartReport.FilterListJson返回的所有字段+自定义筛选的内容
                var ftList = JSON.EncodeToEntity<List<ReportFilterModel>>(chartReport.FilterListJson);
                //原筛选项
                var oldFtList = _reportFilterRepository.GetAllList(p => p.ChartReportId == chartReportId);
                //获得新的字段
                var fdCodes = ftList.Select(p => p.FieldCode);
                //未在新字段的(应被删的)
                var delFtList = oldFtList.Where(p => p.FieldCode != null && !fdCodes.Contains(p.FieldCode)).ToList();

                //批量修改原筛选全部fasle
                if (oldFtList != null && oldFtList.Count > 0)
                {
                    for (int j = oldFtList.Count - 1; j >= 0; j--)
                    {
                        var ft = oldFtList[j];

                        if (delFtList.Contains(ft))
                        {
                            _reportFilterRepository.Delete(ft);
                        }
                        else
                        {
                            ft.IsSearch = false;
                            _reportFilterRepository.Update(ft);
                        }
                    }
                    //CurrentUnitOfWork.SaveChanges();
                }

                //批量添加或修改筛选信息
                if (ftList != null && ftList.Count > 0)
                {
                    for (int i = 0; i < ftList.Count; i++)
                    {
                        var ft = ftList[i];

                        ReportFilter curFt = new ReportFilter();
                        if (ft.IsCustom)//自定筛选
                        {
                            if (ft.Id != 0)
                            {
                                curFt = oldFtList.FirstOrDefault(p => p.Id == ft.Id);
                            }
                            else
                            {
                                curFt = oldFtList.FirstOrDefault(p => p.FieldParam == ft.FieldParam);//根据参数名判断
                            }
                        }
                        else
                        {
                            curFt = oldFtList.FirstOrDefault(p => p.FieldCode == ft.FieldCode);//根据字段编码判断
                        }
                        curFt = AutoMapper.Mapper.Map(ft, curFt);
                        curFt.ChartReportId = chartReportId;
                        _reportFilterRepository.InsertOrUpdate(curFt);//保存筛选

                    }
                }
            }
        }
        #endregion
    }
}
