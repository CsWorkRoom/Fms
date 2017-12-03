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

namespace Easyman.Service
{
    /// <summary>
    /// 表格式报表管理
    /// </summary>
    public class TbReportAppService : EasymanAppServiceBase, ITbReportAppService
    {
        #region 初始化
        /// <summary>
        /// 主报表
        /// </summary>
        private readonly IRepository<TbReport, long> _tbReportRepository;
        private readonly IRepository<TbReportField, long> _tbReportFieldRepository;
        private readonly IRepository<TbReportFieldTop, long> _tbReportFieldTopRepository;
        private readonly IRepository<TbReportOutEvent, long> _tbReportOutEventRepository;
        private readonly IRepository<Param, long> _paramRepository;
        private readonly IRepository<ReportFilter, long> _reportFilterRepository;
        private readonly IRepository<RoleModuleEvent, long> _roleModuleEventRepository;
        private readonly IRepository<Analysis, long> _analysisRepository;
        private readonly IRepository<ModuleEvent, long> _moduleEventRepository;
        private readonly IRepository<InEvent, long> _inEventRepository;//内置事件

        private readonly ModuleManager _moduleManager;

        /// <summary>
        /// 构造函数（注入仓储）
        /// </summary>
        /// <param name="tbReportRepository"></param>
        /// <param name="tbReportFieldRepository"></param>
        /// <param name="tbReportFieldTopRepository"></param>
        /// <param name="tbReportOutEventRepository"></param>
        /// <param name="paramRepository"></param>
        /// <param name="reportFilterRepository"></param>
        /// <param name="roleModuleEventRepository"></param>
        /// <param name="analysisRepository"></param>
        /// <param name="moduleEventRepository"></param>
        /// <param name="inEventRepository"></param>
        /// <param name="moduleManager"></param>
        public TbReportAppService(
            IRepository<TbReport, long> tbReportRepository,
            IRepository<TbReportField, long> tbReportFieldRepository,
            IRepository<TbReportFieldTop, long> tbReportFieldTopRepository,
            IRepository<TbReportOutEvent, long> tbReportOutEventRepository,
            IRepository<Param, long> paramRepository,
            IRepository<ReportFilter, long> reportFilterRepository,
            IRepository<RoleModuleEvent, long> roleModuleEventRepository,
            IRepository<Analysis, long> analysisRepository,
            IRepository<ModuleEvent, long> moduleEventRepository,
            IRepository<InEvent, long> inEventRepository,
            ModuleManager moduleManager)
        {
            _tbReportRepository = tbReportRepository;
            _tbReportFieldRepository = tbReportFieldRepository;
            _tbReportFieldTopRepository = tbReportFieldTopRepository;
            _tbReportOutEventRepository = tbReportOutEventRepository;
            _paramRepository = paramRepository;
            _reportFilterRepository = reportFilterRepository;
            _roleModuleEventRepository = roleModuleEventRepository;
            _analysisRepository = analysisRepository;
            _moduleEventRepository = moduleEventRepository;
            _inEventRepository = inEventRepository;

            _moduleManager = moduleManager;
        }

        #endregion

        #region 公共接口方法
        /// <summary>
        /// 根据filterId获取filter对象
        /// </summary>
        /// <param name="filterId"></param>
        /// <returns></returns>
        public ReportFilterModel GetFilter(long filterId)
        {
            ReportFilterModel ft = new ReportFilterModel();
            var filter = _reportFilterRepository.FirstOrDefault(filterId);
            if (filter != null)
            {
                ft = AutoMapper.Mapper.Map(filter, ft);
            }
            return ft;
        }

        /// <summary>
        /// 根据主报表ID获取表格报表列表
        /// </summary>
        /// <param name="reportId">主reportId</param>
        /// <param name="checkRole">是否验证用户权限</param>
        /// <param name="moduleId">模块页ID</param>
        /// <returns></returns>
        public IList<TbReportModel> GetTbReportList(long reportId, long moduleId, bool checkRole)
        {
            var entList = _tbReportRepository.GetAllList(p => p.ReportId == reportId);
            if (entList != null & entList.Count > 0)
            {
                var tbReportList = new List<TbReportModel>();
                //循环表格表格，取出其配置信息
                foreach (var ent in entList)
                {
                    TbReportModel tb = GetTbReport(ent.Id, moduleId, checkRole);
                    tbReportList.Add(tb);
                }
                return tbReportList;
            }
            return null;
        }

        /// <summary>
        /// 根据tbReportId获取其详细信息
        /// </summary>
        /// <param name="tbReportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// <returns></returns>
        public TbReportModel GetTbReport(long tbReportId, long moduleId, bool checkRole)
        {
            var ent = _tbReportRepository.Get(tbReportId);
            if (ent != null)
            {
                //赋值基础属性
                var tb = AutoMapper.Mapper.Map<TbReportModel>(ent);
                tb.IsMultiSort = ent.IsMultiSort == null ? false : ent.IsMultiSort.Value;
                tb.IsShowHeader = ent.IsShowHeader == null ? true : ent.IsShowHeader.Value;
                tb.IsShowFilter = ent.IsShowFilter == null ? false : ent.IsShowFilter.Value;
                tb.InEventListJson = GetInEventJson(ent, tbReportId, moduleId, checkRole);//内置事件

                //赋值字段配置
                tb.FieldListJson = JSON.DecodeToStr(GetFildList(tbReportId));

                //赋值多表头
                tb.FieldTopListJson = JSON.DecodeToStr(GetFildTopList(tbReportId));

                //赋值事件
                tb.OutEventListJson = JSON.DecodeToStr(GetOutEventList(ent, tbReportId, moduleId, checkRole));

                //筛选字段
                tb.FilterListJson = JSON.DecodeToStr(GetFilterList(tbReportId));

                return tb;
            }
            return null;
        }

        /// <summary>
        /// 保存一个表格报表（其他类报表也需要提供类似方法：参数部分相同）
        /// </summary>
        /// <param name="childrReport"></param>
        /// <param name="reportId"></param>
        /// <param name="code"></param>
        public void SaveTbReport(ChildReportModel childrReport, long reportId, string code)
        {
            if (childrReport.ChildReportJson != null && childrReport.ChildReportJson.Length > 0)
            {
                var tbReport = JSON.EncodeToEntity<TbReportModel>(childrReport.ChildReportJson);
                if (tbReport != null)
                {
                    #region 基础信息修改,得到tbReportId
                    TbReport tb = AutoMapper.Mapper.Map<TbReport>(tbReport);
                    tb.ReportId = reportId;
                    tb.ApplicationType = childrReport.ApplicationType;
                    tb.ReportType = childrReport.ChildReportType;
                    tb.IsOpen = childrReport.IsOpen;
                    tb.Id = childrReport.ChildReportId;
                    var tbReportId = _tbReportRepository.InsertOrUpdateAndGetId(tb);//修改表格报表并获得ID
                    #endregion

                    tbReport.ReportType = childrReport.ChildReportType;
                    tbReport.ApplicationType = childrReport.ApplicationType;

                    SaveRpFieldList(tbReport, reportId, tbReportId);//保存列信息

                    SaveRpEventList(tbReport, reportId, tbReportId, code);//保存事件信息

                    SaveRpTopList(tbReport, tbReportId);//保存多表头信息

                    SaveRpFilterList(tbReport, tbReportId);//保存筛选条件信息
                }
            }
        }

        /// <summary>
        /// 返回报表列表（按指定规范）
        /// </summary>
        /// <param name="reportId">主reportId</param>
        /// <param name="checkRole">是否验证用户权限</param>
        /// <param name="moduleId">模块页ID</param>
        /// <returns></returns>
        public IList<ChildReportModel> GetChildListFromTbReport(long reportId, long moduleId, bool checkRole)
        {
            var tbList = GetTbReportList(reportId, moduleId, checkRole);
            if (tbList != null && tbList.Count > 0)
            {
                var childList = new List<ChildReportModel>();
                //循环表格表格，取出其配置信息
                foreach (var ent in tbList)
                {
                    ChildReportModel child = new ChildReportModel();

                    child.ChildReportId = ent.Id;
                    child.IsOpen = ent.IsOpen;
                    child.ChildReportType = ent.ReportType;
                    child.ApplicationType = ent.ApplicationType;

                    child.ChildReportJson = JSON.DecodeToStr(ent);

                    childList.Add(child);
                }
                return childList;
            }
            return null;
        }

        #endregion

        #region 扩展方法（部分公开化）

        #region 获取表格报表的字段|多表头|事件|筛选
        /// <summary>
        /// 获取字段配置集合
        /// </summary>
        /// <returns></returns>
        public IList<TbReportFieldModel> GetFildList(long tbReportId)
        {
            //字段集合
            var fieldList = _tbReportFieldRepository.GetAllList(p => p.TbReportId == tbReportId);
            if (fieldList != null && fieldList.Count > 0)
            {
                var fieldModelList = fieldList.Select(p =>
                {
                    var fd = p.MapTo<TbReportFieldModel>();
                    if (p.TbReportFieldTop != null)
                    {
                        fd.TbReportFieldTopName = p.TbReportFieldTop.Name;
                    }
                    else
                    {
                        fd.TbReportFieldTopName = "";
                    }
                    fd.OrderNum = (p.OrderNum == null ? 0 : p.OrderNum);
                    return fd;
                }).OrderByDescending(p => p.IsFrozen).ThenBy(p => p.OrderNum).ThenBy(p => p.Id).ToList();

                return fieldModelList;
            }
            return null;
        }
        /// <summary>
        /// 获取多表头配置集合
        /// </summary>
        /// <returns></returns>
        public IList<TbReportFieldTopModel> GetFildTopList(long tbReportId)
        {
            //多表头集合
            var entList = _tbReportFieldTopRepository.GetAllList(p => p.TbReportId == tbReportId);
            if (entList != null && entList.Count > 0)
            {
                var topFieldList = new List<TbReportFieldTopModel>();//初始化多表头集合

                var fieldList = GetFildList(tbReportId);//获取字段集合
                if (fieldList != null && fieldList.Count > 0)
                {
                    foreach (var fd in fieldList)
                    {
                        var fieldAsTop = new TbReportFieldTopModel
                        {
                            ParentID = fd.TbReportFieldTopId,
                            ParentName = fd.TbReportFieldTopName,
                            Name = fd.FieldName,
                            FieldCode = fd.FieldCode
                        };
                        topFieldList.Add(fieldAsTop);
                    }
                }
                topFieldList = topFieldList.Concat(AutoMapper.Mapper.Map<List<TbReportFieldTopModel>>(entList)).ToList();
                return topFieldList;
            }
            return null;
        }
        /// <summary>
        /// 获取事件配置集合
        /// </summary>
        /// <returns></returns>
        private IList<TbReportOutEventModel> GetOutEventList(TbReport ent, long tbReportId, long moduleId, bool checkRole)
        {
            var outEventList = _tbReportOutEventRepository.GetAllList(p => p.TbReportId == tbReportId);
            if (outEventList != null && outEventList.Count > 0)
            {
                //映射model，同时为每个事件赋唯一值
                var eventOutputList = outEventList.Select(p =>
                {
                    var outEvent = p.MapTo<TbReportOutEventModel>();
                    outEvent.Identifier = p.Id.ToString();//赋唯一标识符
                    return outEvent;
                }).ToList();
                if (checkRole)//验证用户权限（只返回权限内的事件）
                {
                    Users.User user = GetCurrentUserAsync().Result;
                    var roleids = user.Roles.Select(p => Convert.ToInt64(p.RoleId)).ToList();
                    var evids = _roleModuleEventRepository.GetAllList(p => roleids.Contains(p.RoleId) && p.ModuleId == moduleId).Select(p => p.EventId);
                    var tbEvs = _moduleEventRepository.GetAllList(p => evids.Contains(p.Id)).Select(p => p.SourceTableId);

                    eventOutputList = eventOutputList.Where(p => tbEvs.Contains(p.Id)).ToList();
                }
                //循环获取每个事件的参数列表
                foreach (var ev in eventOutputList)
                {
                    //获取事件参数
                    var paramList = _paramRepository.GetAllList(p => p.TbReportOutEventId == ev.Id).OrderBy(p=>p.OrderNum).ToList();
                    if (paramList != null && paramList.Count > 0)
                    {
                        ev.ParamListJson = JSON.DecodeToStr(AutoMapper.Mapper.Map<List<ParamModel>>(paramList));
                    }
                }
                return eventOutputList;
            }
            return null;
        }
        /// <summary>
        /// 获取当前报表的内置事件
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="tbReportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// <returns></returns>
        private string GetInEventJson(TbReport ent, long tbReportId, long moduleId, bool checkRole)
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
        /// <returns></returns>
        private IList<ReportFilterModel> GetFilterList(long tbReportId)
        {
            var rpFilterList = new List<ReportFilterModel>();//初始化筛选配置集合

            //获取表格字段的筛选信息
            var ftList = _reportFilterRepository.GetAllList(p => p.TbReportId == tbReportId);
            var filterList = AutoMapper.Mapper.Map<List<ReportFilterModel>>(ftList);

            //获取表格报表的字段信息
            var fieldList = _tbReportFieldRepository.GetAllList(p => p.TbReportId == tbReportId);

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
                        TbReportId = tbReportId,
                        RdlcReportId=0,
                        ChartReportId=0,
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
                                filter.RdlcReportId = ft.RdlcReportId == null ? 0 : ft.RdlcReportId;
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
                        TbReportId = tbReportId,
                        RdlcReportId=0,
                        ChartReportId=0,
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
        #endregion

        #region 保存表格报表信息:字段|多表头|事件|筛选
        /// <summary>
        /// 保存列信息
        /// </summary>
        /// <param name="tbReport"></param>
        /// <param name="reportId"></param>
        /// <param name="tbReportId"></param>
        private void SaveRpFieldList(TbReportModel tbReport, long reportId, long tbReportId)
        {
            if (tbReport.FieldListJson != null && tbReport.FieldListJson.Length > 0)
            {
                var fieldList = JSON.EncodeToEntity<List<TbReportFieldModel>>(tbReport.FieldListJson);
                if (fieldList != null && fieldList.Count > 0)
                {
                    //循环添加或修改字段信息
                    for (var i = 0; i < fieldList.Count; i++)
                    //foreach (var field in fieldList)
                    {
                        var field = fieldList[i];
                        var fd = AutoMapper.Mapper.Map<TbReportField>(field);

                        //修改已有字段信息
                        var hasFd = _tbReportFieldRepository.FirstOrDefault(p => p.FieldCode == field.FieldCode && p.TbReportId == tbReportId);
                        if (hasFd != null)
                        {
                            hasFd.FieldName = field.FieldName;
                            hasFd.DataType = field.DataType;
                            hasFd.IsOrder = field.IsOrder;
                            hasFd.IsShow = field.IsShow;
                            hasFd.Width = field.Width;
                            hasFd.IsSearch = field.IsSearch;
                            hasFd.IsFrozen = field.IsFrozen;
                            hasFd.Align = field.Align;
                            hasFd.OrderNum = (field.OrderNum == 0 || field.OrderNum == null) ? (i + 1) : field.OrderNum;
                            hasFd.Remark = field.Remark;
                            hasFd.Width = field.Width;
                            _tbReportFieldRepository.Update(hasFd);//修改
                        }
                        else//新增未有的字段
                        {
                            fd.ReportId = reportId;
                            fd.TbReportId = tbReportId;
                            _tbReportFieldRepository.Insert(fd);//新增
                        }
                    }
                }
                //循环从表中删除未在新字段列表中找到的字段
                var fdCodes = fieldList.Select(k => k.FieldCode);
                var fdList = _tbReportFieldRepository.GetAllList(p => p.TbReportId == tbReportId
                 && !fdCodes.Contains(p.FieldCode)).ToList();//找到未在信息列表中的字段
                if (fdList != null && fdList.Count > 0)
                {
                    for (int i = fdList.Count - 1; i >= 0; i--)
                    {
                        _tbReportFieldRepository.Delete(fdList[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 保存事件信息（外置）,同时向模版事件表中添加内置事件信息
        /// </summary>
        /// <param name="tbReport"></param>
        /// <param name="reportId"></param>
        /// <param name="tbReportId"></param>
        /// <param name="code"></param>
        private void SaveRpEventList(TbReportModel tbReport, long reportId, long tbReportId, string code)
        {
            //表格解析地址
            var analysis = _analysisRepository.FirstOrDefault(p => p.Url == "Report/TbReport");

            #region 外置事件保存
            if (tbReport.OutEventListJson != null && tbReport.OutEventListJson.Length > 0)
            {
                //当前事件列表
                var eventList = JSON.EncodeToEntity<List<TbReportOutEventModel>>(tbReport.OutEventListJson);
                //var nullEventL = eventList.Where(p => string.IsNullOrEmpty(p.DisplayName) || string.IsNullOrEmpty(p.Url));
                eventList.RemoveAll(p => string.IsNullOrEmpty(p.DisplayName) || string.IsNullOrEmpty(p.Url));//移除空事件
                //原事件列表
                var oldEventList = _tbReportOutEventRepository.GetAllList(p => p.TbReportId == tbReportId);

                //循环从表中删除未在新事件列表中的事件及事件参数
                //同时，删除在解析表中的模块事件和事件赋权信息
                foreach (var ev in oldEventList)
                {
                    if (!eventList.Select(p => p.Id).Contains(ev.Id))
                    {
                        _tbReportOutEventRepository.Delete(ev);//删除事件
                        //循环删除事件的参数列表
                        var parList = _paramRepository.GetAllList(p => p.TbReportOutEventId == ev.Id);
                        if (parList.Any())
                        {
                            foreach (var par in parList)
                            {
                                _paramRepository.Delete(par);//删除一个事件参数
                            }
                        }

                        if (analysis != null)
                        {
                            //删除事件解析表事件及赋权信息
                            _moduleManager.DeleteEvent(analysis.Id, code, ev.Id);
                        }
                    }
                }

                //循环修改或新增事件
                if (eventList.Any())
                {
                    foreach (var ev in eventList)
                    {
                        TbReportOutEvent even = new TbReportOutEvent();
                        var oldEv = oldEventList.FirstOrDefault(p => p.Id == ev.Id);
                        if (oldEv != null)
                        {
                            even = oldEv;
                        }

                        even = AutoMapper.Mapper.Map(ev, even);
                        even.TbReportId = tbReportId;
                        //新增或修改事件信息
                        var eventId = _tbReportOutEventRepository.InsertOrUpdateAndGetId(even);

                        //修改已在解析事件表中的事件信息（url）
                        //更新EM_MODULE_EVENT（url）
                        if (oldEv != null)
                        {
                            string lambda = "p=>p.SourceTableId==" + oldEv.Id + " && p.SourceTable==\"EM_TB_REPORT_OUTEVENT\" && p.Code==\"" + code + "\"";
                            Expression<Func<ModuleEvent, bool>> exp = StringToLambda.LambdaParser.Parse<Func<ModuleEvent, bool>>(lambda);
                            var mdEvList = _moduleManager.GetModuleEventList(exp);
                            if (mdEvList != null && mdEvList.Count > 0)
                            {
                                var mdev = mdEvList[0];
                                mdev.Url = ev.Url;
                                mdev.EventFrom = "外置";
                                mdev.EventName = ev.DisplayName;
                                mdev.EventType = ev.EventType;
                                _moduleManager.SaveModuleEvent(mdev);
                            }
                            else
                            {
                                //保存事件至EM_MODULE_EVENT
                                ModuleEvent mdEv = new ModuleEvent
                                {
                                    AnalysisId = analysis.Id,
                                    Code = code,
                                    EventName = ev.DisplayName,
                                    EventType = ev.EventType,
                                    SourceTable = "EM_TB_REPORT_OUTEVENT",
                                    SourceTableId = eventId,
                                    EventFrom = "外置",
                                    Url = ev.Url//链接
                                };
                                _moduleManager.SaveEvent(mdEv);
                            }
                        }
                        if (ev.Id == 0)//新增事件到解析事件表
                        {
                            //保存事件至EM_MODULE_EVENT
                            ModuleEvent mdEv = new ModuleEvent
                            {
                                AnalysisId = analysis.Id,
                                Code = code,
                                EventName = ev.DisplayName,
                                EventType = ev.EventType,
                                SourceTable = "EM_TB_REPORT_OUTEVENT",
                                SourceTableId = eventId,
                                EventFrom = "外置",
                                Url = ev.Url//链接
                            };
                            _moduleManager.SaveEvent(mdEv);
                        }

                        #region 覆盖式更新事件参数列表（先删除再添加）
                        //删除参数列表
                        var parList = _paramRepository.GetAllList(p => p.TbReportOutEventId == eventId);
                        if (parList.Any())
                        {
                            foreach (var par in parList)
                            {
                                _paramRepository.Delete(par);
                            }
                        }
                        //保存事件的参数列表
                        if (ev.ParamListJson != null && ev.ParamListJson.Length > 0)
                        {
                            var paramList = JSON.EncodeToEntity<List<ParamModel>>(ev.ParamListJson);
                            foreach (var param in paramList)
                            {
                                var pm = AutoMapper.Mapper.Map<Param>(param);
                                pm.Id = 0;//新增方式插入
                                pm.TbReportOutEventId = eventId;//事件ID
                                _paramRepository.InsertOrUpdate(pm);//保存参数
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            #region 内置事件保存
            //内置事件列表
            var inEvList = _inEventRepository.GetAllList(p => p.ReportType == tbReport.ReportType).ToList();
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
                            EventName = inev.DisplayName,
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
                            EventName = inEv.DisplayName,
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
        /// 保存多表头信息
        /// 覆盖式保存多表头信息（先删除再添加）
        /// </summary>
        /// <param name="tbReport"></param>
        /// <param name="tbReportId"></param>
        private void SaveRpTopList(TbReportModel tbReport, long tbReportId)
        {
            if (tbReport.FieldTopListJson != null && tbReport.FieldTopListJson.Length > 0)
            {
                var oldTopList = _tbReportFieldTopRepository.GetAllList(p => p.TbReportId == tbReportId);
                //循环删除原多表头信息
                for (int i = 0; i < oldTopList.Count; i++)
                {
                    var top = oldTopList[i];
                    _tbReportFieldTopRepository.Delete(top);
                }
                //CurrentUnitOfWork.SaveChanges();

                //循环添加多表头信息（从顶端到末梢递归添加）
                var topList = JSON.EncodeToEntity<List<TbReportFieldTopModel>>(tbReport.FieldTopListJson);
                if (topList != null && topList.Count > 0)
                {
                    foreach (var top in topList)
                    {
                        if (string.IsNullOrEmpty(top.ParentName))//当父级为空时
                        {
                            //递归保存顶级节点及其子节点
                            RecursionSaveTopField(null, tbReportId, top, topList);
                        }
                    }
                }
            }
            else//如果在数据库中存在多表头的信息则删除
            {
                var fieldList = JSON.EncodeToEntity<List<TbReportFieldModel>>(tbReport.FieldListJson);
                if (fieldList != null && fieldList.Count > 0)
                {
                    var fdCodes = fieldList.Select(p => p.FieldCode);
                    //删除字段的topID
                    var fdList = _tbReportFieldRepository.GetAllList(p => p.TbReportId == tbReportId
                    && !fdCodes.Contains(p.FieldCode));
                    foreach (var fd in fdList)
                    {
                        if (fd.TbReportFieldTopId != null)
                        {
                            fd.TbReportFieldTopId = null;//修改字段的表头ID
                            _tbReportFieldRepository.Update(fd);//保存修改
                        }
                    }
                }
                
                var oldTopList = _tbReportFieldTopRepository.GetAllList(p => p.TbReportId == tbReportId);
                //循环删除原多表头信息
                for (int i = 0; i < oldTopList.Count; i++)
                {
                    var top = oldTopList[i];
                    _tbReportFieldTopRepository.Delete(top);
                }
            }
        }

        /// <summary>
        /// 保存筛选条件信息
        /// 将未设置筛选的字段一起保存（未设置的字段IsSearch = false）
        /// </summary>
        /// <param name="tbReport"></param>
        /// <param name="tbReportId"></param>
        private void SaveRpFilterList(TbReportModel tbReport, long tbReportId)
        {
            //新增或修改筛选
            if (tbReport.FilterListJson != null && tbReport.FilterListJson.Length > 0)
            {
                //保存说明：无差别式保存.然后在报表解析时去差别处理

                //tbReport.FilterListJson返回的所有字段+自定义筛选的内容
                var ftList = JSON.EncodeToEntity<List<ReportFilterModel>>(tbReport.FilterListJson);
                //原筛选项
                var oldFtList = _reportFilterRepository.GetAllList(p => p.TbReportId == tbReportId);
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
                        curFt.TbReportId = tbReportId;
                        _reportFilterRepository.InsertOrUpdate(curFt);//保存筛选

                    }
                }
            }
        }

        #region 多表头保存的子方法
        /// <summary>
        /// 从上往下递归：保存多表头
        /// </summary>
        /// <param name="parentTopId"></param>
        /// <param name="tbReportId"></param>
        /// <param name="top"></param>
        /// <param name="topList"></param>
        private void RecursionSaveTopField(long? parentTopId, long tbReportId, TbReportFieldTopModel top, List<TbReportFieldTopModel> topList)
        {
            //当前节点为字段时，不添加表头而去修改字段的top
            if (!string.IsNullOrEmpty(top.FieldCode) && top.FieldCode.Length > 0)
            {
                //根据FieldCode和tbReportId查找字段
                var fd = _tbReportFieldRepository.FirstOrDefault(p => p.FieldCode == top.FieldCode && p.TbReportId == tbReportId);
                if (fd != null && parentTopId != null)
                {
                    fd.TbReportFieldTopId = parentTopId;//修改字段的表头ID
                    _tbReportFieldRepository.Update(fd);//保存修改
                }
            }
            else
            {
                top.Id = 0;//新增式插入
                var curParentTopId = SaveTopField(top, tbReportId, parentTopId);
                foreach (var curTop in topList)
                {
                    if (curTop.ParentName == top.Name)//当前节点的子节点
                    {
                        RecursionSaveTopField(curParentTopId, tbReportId, curTop, topList);//保存子节点
                    }
                }
            }
        }

        /// <summary>
        /// 保存一个多表头信息
        /// </summary>
        /// <param name="top"></param>
        /// <param name="tbReportId"></param>
        /// <param name="parentTopId"></param>
        private long? SaveTopField(TbReportFieldTopModel top, long tbReportId, long? parentTopId)
        {
            var curTop = AutoMapper.Mapper.Map<TbReportFieldTop>(top);
            curTop.ParentID = parentTopId;
            curTop.TbReportId = tbReportId;
            return _tbReportFieldTopRepository.InsertOrUpdateAndGetId(curTop);
        }

        #endregion

        #region 文件下载事件处理

        #region 下载程序调度
        /// <summary>
        /// 获取文件下载地址
        /// </summary>
        /// <param name="strCode">编号</param>
        /// <param name="strTyle">类型</param>
        /// <param name="strState">状态</param>
        /// <returns></returns>
        public string GetFileUrl(string strCode, string strTyle, string strState)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 格式化表头
        private void GetTableHead(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            foreach (DataRow item in dt.Rows)
            {

            }

            string strColumnHeader = "";
           // OutExcelHelper.ExportExcel(dt, "", strColumnHeader, true);
        }
        #endregion

        #endregion
        #endregion
        #endregion
    }
}