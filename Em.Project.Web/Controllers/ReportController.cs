using Abp.UI;
using Easyman.Common;
using Easyman.Common.Mvc;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Service;
using Easyman.Sys;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
//using Microsoft.Reporting.WebForms;

namespace Easyman.FwWeb.Controllers
{
    public class ReportController :  EasyManController
    {
        #region 初始化
        private readonly IReportAppService _reportAppService;
        private readonly ITbReportAppService _tbReportAppService;
        private readonly IDbServerAppService _dbServerAppService;
        private readonly IModulesAppService _moduleAppService;
        private readonly IExportAppService _exportAppService;
        private readonly IRdlcReportAppService _rdlcAppService;
        private readonly IChartReportAppService _ChartAppService;


        public ReportController(IReportAppService reportAppService,
            ITbReportAppService tbReportAppService,
            IDbServerAppService dbServerAppService,
            IModulesAppService moduleAppService,
            IExportAppService exportAppService,
            IRdlcReportAppService rdlcAppService,
            IChartReportAppService ChartAppService)
        {
            _reportAppService = reportAppService;
            _tbReportAppService = tbReportAppService;
            _dbServerAppService = dbServerAppService;
            _moduleAppService = moduleAppService;
            _exportAppService = exportAppService;
            _rdlcAppService = rdlcAppService;
            _ChartAppService = ChartAppService;
        }

        #endregion

        #region 编辑或新增一个报表
        /// <summary>
        /// 编辑或新增一个报表
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public ActionResult EditReport(long? reportId)
        {
            if (reportId == null || reportId == 0)
            {
                var model = new ReportOutput();
                return View(model);
            }
            else
            {
                var report = _reportAppService.GetReport(reportId.Value);
                report.IsPlaceholder = report.IsPlaceholder == null ? false : report.IsPlaceholder.Value;
                return View(report);
            }
        }
        #endregion

        #region 根据code得到一个report
        /// <summary>
        /// 根据code得到一个report
        /// →单独在页面使用时作用：根据code打开一个表格报表.
        /// 这时需要校验用户权限,只返回用户能够看到内容（事件）←
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [ValidateUrl]//开启了双重验证
        public Task<ActionResult> TbReport(string code)
        {
            return Task.Factory.StartNew(() =>
            {
                var urlQuery = Request.Url.GetLeftPart(UriPartial.Query).ToString();
                var urlRoot= urlQuery.Split(new[] { "/Report/TbReport" }, StringSplitOptions.None)[0];
                //UrlDecode解码中文
                var curUrl = System.Web.HttpUtility.UrlDecode(urlQuery).Substring(urlRoot.Length + 1);
                var urlToLower = curUrl.ToLower();

                long moduleId = GetModuleIdByParentPage(urlToLower, curUrl);//获取当前链接的模版

                //按当前用户权限返回报表信息（目前只服务于表格报表，其他类报表未实现）
                var report = _reportAppService.GetReport(code, moduleId, true);
                //得到url中的参数信息
                var kvList = GetDefParams(curUrl);
                if (kvList != null && kvList.Count > 0)
                {
                    report.KVJson = JSON.DecodeToStr(kvList);//默认参数赋值
                }

                if (report != null)
                {
                    if (report.ChildReportListJson != null && report.ChildReportListJson != "" && report.ChildReportListJson != "[]")
                    {
                        var childRpList = JSON.EncodeToEntity<List<ChildReportModel>>(report.ChildReportListJson);
                        if (childRpList != null && childRpList.Count > 0)
                        {
                            return View(report);//当具有子报表时返回
                        }
                    }
                }
                return View("");//未找到子报表
            }).ContinueWith<ActionResult>(task =>
            {
                return task.Result;
            });
        }
        #endregion

        /// <summary>
        /// 根据当前路径获取moduleId
        /// </summary>
        /// <param name="urlToLower"></param>
        /// <param name="curUrl"></param>
        /// <returns></returns>
        private long GetModuleIdByParentPage(string urlToLower,string curUrl)
        {
            #region 获取当前路径对应的模版 --moduleId
            long moduleId = 0;//初始化
            var moduleList = new List<Module>();
            //先校验完整路径及参数后缀
            Expression<Func<Module, bool>> expA = StringToLambda.LambdaParser.Parse<Func<Module, bool>>("p=>p.Url.ToLower().Contains(\"" + urlToLower + "\")");
            Expression<Func<Module, bool>> exp = StringToLambda.LambdaParser.Parse<Func<Module, bool>>("p=>p.Url.ToLower().Contains(\"" + urlToLower.Split('?')[0] + "\")");

            //先获取完整路径的Module
            moduleList = _moduleAppService.GetModuleList(expA).GetAwaiter().GetResult();//得到包含当前url的模版页
            if (moduleList == null || moduleList.Count == 0)
            {
                //完整路径为空时,截取路径
                moduleList = _moduleAppService.GetModuleList(exp).GetAwaiter().GetResult();//得到包含当前url的模版页
                if (moduleList != null && moduleList.Count > 0)
                {
                    int maxLength = 0;
                    var inArr = urlToLower.Split(new[] { '?', '&' });
                    foreach (var md in moduleList)
                    {
                        var curArr = md.Url.ToLower().Split(new[] { '?', '&' });
                        if (inArr[0] == curArr[0] && inArr.Intersect(curArr).Count() == curArr.Length)
                        {
                            if (curArr.Length > maxLength)
                            {
                                moduleId = md.Id;
                                maxLength = curArr.Length;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var md in moduleList)
                {
                    //严格校验模块页（url路径及参数后缀必须一致）
                    if (curUrl == md.Url.Substring(md.Url.Length - curUrl.Length))
                    {
                        moduleId = md.Id;
                        break;
                    }
                }
            }
            #endregion
            return moduleId;
        }

        #region 查询code得到结果集
        /// <summary>
        /// 查询code得到结果集
        /// </summary>
        /// <returns></returns>
        public ActionResult TbQueryList()
        {
            string result = "";
            //获得参数
            string code = Request["code"].Trim();
            int rows = Convert.ToInt32(Request["rows"].Trim());
            int page = Convert.ToInt32(Request["page"].Trim());
            string queryParams = Request["queryParams"];//查询条件
            string sidx = Request["sidx"];//排序条件
            string sord = Request["sord"];//最后个字段排序方式
            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            err.IsError = false;

            //取消获取报表的语句。原因：既然能进这个页面肯定是已具有code代码
            //var report = _reportAppService.GetReport(code);
            //执行sql语句
            try
            {
                result = _reportAppService.ExcuteReportSql(code, rows, page, queryParams, sidx, sord, ref err);
                if (err.IsError)
                {
                    throw new Exception(err.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Content(result);
        }
        #endregion

        /// <summary>
        /// 调试（返回替换变量、组装后的sql）
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDebugSql()
        {
            string sql = "";
            //获得参数
            string code = Request["code"].Trim();
            string queryParams = Request["queryParams"];//查询条件
            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            err.IsError = false;

            //取消获取报表的语句。原因：既然能进这个页面肯定是已具有code代码
            //var report = _reportAppService.GetReport(code);
            //执行sql语句
            try
            {
                sql = _reportAppService.GetDebugSql(code, queryParams, ref err);
                if (err.IsError)
                {
                    throw new Exception(err.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Content(sql);
        }

        #region rdlc相关操作
        /// <summary>
        /// 下载rdlc模版文件
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public ActionResult DownRDLC(string fields, string reportName)
        {
            if (!string.IsNullOrEmpty(fields) && fields != "[]")
            {
                var fieldList = JSON.EncodeToEntity<List<TbReportFieldModel>>(fields);
                string rdlcXml = MakeRdlc(fieldList, reportName);// 获取rdlc的xml

                var tmp = Encoding.UTF8.GetBytes(rdlcXml);
                var fileName = (string.IsNullOrEmpty(reportName) ? "新建rdlc报表" : reportName) + DateTime.Now.ToString("yyyyMMdd") + ".rdlc";

                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/octet-stream";

                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
                Response.BinaryWrite(tmp);
                Response.Flush();
                Response.End();
            }
            return new EmptyResult();
        }
        /// <summary>
        /// 下载已配置的RDLC文件
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public ActionResult DownNowRDLC(long? rdlcId, string reportName)
        {
            if(rdlcId!=null)
            {
                var rdlc= _rdlcAppService.GetRdlcReportBase(rdlcId.Value);
                if(rdlc!=null)
                {
                    var tmp = Encoding.UTF8.GetBytes(rdlc.RdlcXml);
                    var fileName = reportName + DateTime.Now.ToString("yyyyMMdd") + ".rdlc";

                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    Response.ContentType = "application/octet-stream";

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
                    Response.BinaryWrite(tmp);
                    Response.Flush();
                    Response.End();
                }
            }
            return new EmptyResult();
        }
        /// <summary>
        /// 获取rdlc的xml
        /// </summary>
        /// <param name="AllPara"></param>
        /// <returns></returns>
        public string MakeRdlc(List<TbReportFieldModel> AllPara, string reportName)
        {
            string reStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Report xmlns=""http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition"" xmlns:rd=""http://schemas.microsoft.com/SQLServer/reporting/reportdesigner"">
  <Width>6.5in</Width>
  <Body>
    <Height>2in</Height>
  </Body>
  <rd:ReportTemplate>true</rd:ReportTemplate>
  <Page>
  </Page>
{0}
</Report>
";
            string dataStr = @"
<DataSources>
    <DataSource Name=""{1}"">
      <ConnectionProperties>
        <DataProvider>System.Data.DataSet</DataProvider>
        <ConnectString>/* Local Connection */</ConnectString>
      </ConnectionProperties>
      <rd:DataSourceID>{2}</rd:DataSourceID>
    </DataSource>
  </DataSources>
  <DataSets>
    <DataSet Name=""DataSet1"">
      <Query>
        <DataSourceName>{1}</DataSourceName>
        <CommandText>/* Local Query */</CommandText>
      </Query>
      <Fields>
{0}
      </Fields>
      <rd:DataSetInfo>
        <rd:DataSetName>{1}</rd:DataSetName>
        <rd:SchemaPath></rd:SchemaPath>
        <rd:TableName>DataTable1</rd:TableName>
        <rd:TableAdapterFillMethod />
        <rd:TableAdapterGetDataMethod />
        <rd:TableAdapterName />
      </rd:DataSetInfo>
    </DataSet>
  </DataSets>
";
            var fieldStr = @"
        <Field Name=""{0}"">
          <DataField>{1}</DataField>
          <rd:TypeName>{2}</rd:TypeName>
        </Field>
";
            var allFieldStr = "";
            foreach (var t in AllPara)
            {
                allFieldStr += string.Format(fieldStr, t.FieldName, t.FieldCode, t.DataType);
            }
            dataStr = string.Format(dataStr, allFieldStr, reportName + DateTime.Now.Ticks, new Guid().ToString());
            reStr = string.Format(reStr, dataStr);
            return reStr;
        }
        /// <summary>
        /// 根据code加载rdlc报表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<ActionResult> RdlcReport(string code)
        {
            //string controllerName = Request.RequestContext.RouteData.Values["controller"].ToString();
            //string actionName = Request.RequestContext.RouteData.Values["action"].ToString();
            return Task.Factory.StartNew(() =>
            {
                var urlQuery = Request.Url.GetLeftPart(UriPartial.Query).ToString();
                var urlRoot = urlQuery.Split(new[] { "/Report/RdlcReport" }, StringSplitOptions.None)[0];
                //UrlDecode解码中文
                var curUrl = System.Web.HttpUtility.UrlDecode(urlQuery).Substring(urlRoot.Length + 1);
                var urlToLower = curUrl.ToLower();

                long moduleId = GetModuleIdByParentPage(urlToLower, curUrl);//获取当前链接的模版

                //按当前用户权限返回报表信息（目前只服务于表格报表，其他类报表未实现）
                var report = _reportAppService.GetReport(code, moduleId, true);
                //得到url中的参数信息
                var kvList = GetDefParams(curUrl);
                if (kvList != null && kvList.Count > 0)
                {
                    report.KVJson = JSON.DecodeToStr(kvList);//默认参数赋值
                }

                if (report != null)
                {
                    if (report.ChildReportListJson != null && report.ChildReportListJson != "" && report.ChildReportListJson != "[]")
                    {
                        var childRpList = JSON.EncodeToEntity<List<ChildReportModel>>(report.ChildReportListJson);
                        if (childRpList != null && childRpList.Count > 0)
                        {
                            return View(report);//当具有子报表时返回
                        }
                    }
                }
                return View("");//未找到子报表
            }).ContinueWith<ActionResult>(task =>
            {
                return task.Result;
            });
        }
        /// <summary>
        /// 根据code和条件组装并执行sql，返回datatable的json
        /// </summary>
        /// <returns></returns>
        public string ExcuteSql()
        {
            //获得参数
            string code = Request["code"].Trim();
            string queryParams = Request["queryParams"];//查询条件
            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            err.IsError = false;

            //执行sql语句
            try
            {
                DataTable dt = _reportAppService.GetDataTableFromCode(code, queryParams, ref err);
                if (err.IsError)
                {
                    throw new Exception(err.Message);
                }
                return JSON.DecodeToStr(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Echart
        public Task<ActionResult> ChartReport(string code)
        {
            //string controllerName = Request.RequestContext.RouteData.Values["controller"].ToString();
            //string actionName = Request.RequestContext.RouteData.Values["action"].ToString();
            return Task.Factory.StartNew(() =>
            {
                var urlQuery = Request.Url.GetLeftPart(UriPartial.Query).ToString();
                var urlRoot = urlQuery.Split(new[] { "/Report/ChartReport" }, StringSplitOptions.None)[0];
                //UrlDecode解码中文
                var curUrl = System.Web.HttpUtility.UrlDecode(urlQuery).Substring(urlRoot.Length + 1);
                var urlToLower = curUrl.ToLower();

                long moduleId = GetModuleIdByParentPage(urlToLower, curUrl);//获取当前链接的模版

                //按当前用户权限返回报表信息（目前只服务于表格报表，其他类报表未实现）
                var report = _reportAppService.GetReport(code, moduleId, true);
                //得到url中的参数信息
                var kvList = GetDefParams(curUrl);
                if (kvList != null && kvList.Count > 0)
                {
                    report.KVJson = JSON.DecodeToStr(kvList);//默认参数赋值
                }

                if (report != null)
                {
                    if (report.ChildReportListJson != null && report.ChildReportListJson != "" && report.ChildReportListJson != "[]")
                    {
                        var childRpList = JSON.EncodeToEntity<List<ChildReportModel>>(report.ChildReportListJson);
                        if (childRpList != null && childRpList.Count > 0)
                        {
                            return View(report);//当具有子报表时返回
                        }
                    }
                }
                return View("");//未找到子报表
            }).ContinueWith<ActionResult>(task =>
            {
                return task.Result;
            });
        }

        public ActionResult GetChartData()
        {
            //获得参数
            string code = Request["code"].Trim();
            //int rows = Convert.ToInt32(Request["rows"].Trim());
            //int page = Convert.ToInt32(Request["page"].Trim());
            string queryParams = Request["queryParams"];//查询条件
            //string sidx = Request["sidx"];//排序条件
            //string sord = Request["sord"];//最后个字段排序方式
            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            err.IsError = false;

            DataTable dt = new DataTable();

            //取消获取报表的语句。原因：既然能进这个页面肯定是已具有code代码
            //var report = _reportAppService.GetReport(code);
            //执行sql语句
            try
            {
                dt = _reportAppService.GetDataTableFromCode(code, queryParams, ref err);
                //result = _reportAppService.ExcuteReportSql(code, rows, page, queryParams, sidx, sord, ref err);
                if (err.IsError)
                {
                    throw new Exception(err.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Content(JSON.DecodeToStr(dt));
        }
        #endregion

        #region 图表种类
        public ActionResult EditChartType(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new ChartTypeModel());
            }
            try
            {
                var entObj = _ChartAppService.GetChartType(id.Value);
                return View(entObj);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        #endregion

        #region 图表模版
        public ActionResult EditChartTemp(long? id)
        {
            var entObj = new ChartTempModel();//初始化基础数据

            if (id != null && id != 0)
            {
                entObj = _ChartAppService.GetChartTemp(id.Value);
            }
            entObj.ChartTypeList = _ChartAppService.ChartTypeSelectList();
            return View(entObj);
        }
        #endregion

        /// <summary>
        /// 在指定库中解析sql语句,得到字段json串
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbserverId"></param>
        /// <returns></returns>
        public string AnalysisSql(string sql, long? dbserverId)
        {
            return _reportAppService.AnalysisSql(sql, dbserverId);
        }

        #region 在指定库中根据filterId获取下拉键值集合（code所在数据库）
        /// <summary>
        /// 在指定库中根据filterId获取下拉键值集合（code所在数据库）
        /// </summary>
        /// <param name="dictTypeName">字典类型名称</param>
        /// <returns></returns>
        public string GetFilterDropDown(long? filterId, string code)
        {
            if (filterId != null)
            {
                var report = _reportAppService.GetReportBase(code);
                var ft = _tbReportAppService.GetFilter(filterId.Value);
                if (ft != null)
                {
                    DataTable table = new DataTable();
                    //默认的承载库
                    if (report.DbServerId == null || report.DbServerId == 0)
                    {
                        table = DbHelper.ExecuteGetTable(ft.FilterSql);
                    }
                    else//指定库
                    {
                        table = _dbServerAppService.ExecuteGetTable(report.DbServerId.Value, ft.FilterSql);
                    }
                    if (table != null && table.Rows.Count > 0)
                    {
                        return Fun.DataTable2Json(table);
                    }
                }
            }
            return "[]";
        }
        #endregion

        #region 获取url中默认参数
        /// <summary>
        /// 获取url中默认参数
        /// </summary>
        /// <param name="url"></param>
        private List<KV> GetDefParams(string url)
        {
            List<KV> kvList = new List<KV>();//初始化参数集合
            string[] paraArr = url.Split(new string[] { "?", "&" }, StringSplitOptions.None);
            if (paraArr != null && paraArr.Length > 0)
            {
                for (int i = 0; i < paraArr.Length; i++)
                {
                    var par = paraArr[i];
                    //i==0的为url不带参链接，排除
                    if (i != 0 && par != "")
                    {
                        var curP = par.Split('=').Select(p => p.Trim()).ToArray();
                        if (curP[0].ToUpper() != "CODE")
                        {
                            KV kv = new KV { K = curP[0], V = curP[1] };
                            kvList.Add(kv);
                        }
                    }
                }
            }
            return kvList;
        }
        #endregion

        #region 导出数据
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportData()
        {
            string sql = "";
            long tbReportId = 0;//初始化tbReportId
            string strResult = "";//返回参数
            //获得参数
            string code = Request["code"].Trim();
            if (!string.IsNullOrEmpty(Request["tbReportId"].Trim()))
            {
                tbReportId = Convert.ToInt64(Request["tbReportId"].Trim());
            }
            string queryParams = Request["queryParams"];//查询条件
            string url = Request["url"];//页面url
            url = System.Web.HttpUtility.UrlDecode(url);
            string bootUrl = Request["bootUrl"];//网站根目录（含虚拟层级）
            string strHost = bootUrl.Substring(0, bootUrl.IndexOf(Request.Url.Authority.ToLower()) + Request.Url.Authority.Length);//http头
            string exportWay = Request["exportWay"];//导出方式
            string fileFormat = Request["fileFormat"];//文件格式
            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            err.IsError = false;

            #region 逻辑部分
            try
            {
                var report = _reportAppService.GetReportBase(code);
                var expCfg = _exportAppService.GetExportConfig("report");//得到当前功能的配置信息
                #region 默认值
                int intWaitTime = 60000;
                int intMaxRowNum = 1800000;
                int intDataSize = 51200;
                int intValidDay = 15;
                string strPath = "upfiles";
                if (expCfg.WaitTime == null || expCfg.WaitTime <= 0)
                    expCfg.WaitTime = intWaitTime;
                if (expCfg.MaxRowNum == null || expCfg.MaxRowNum <= 0)
                    expCfg.MaxRowNum = intMaxRowNum;
                if (expCfg.DataSize == null || expCfg.DataSize <= 0)
                    expCfg.DataSize = intDataSize;
                if (expCfg.ValidDay == null || expCfg.ValidDay <= 0)
                    expCfg.ValidDay = intValidDay;
                if (expCfg.Path == null || expCfg.Path.Trim() == "")
                    expCfg.Path = (bootUrl.Replace(strHost, "") + strPath).Replace("\\", "/").Replace("//", "/");
                else
                    expCfg.Path = (bootUrl.Replace(strHost, "") + expCfg.Path).Replace("\\", "/").Replace("//", "/");
                #endregion
                sql = _reportAppService.GetSqlForField(code, queryParams, tbReportId, ref err);

                if (err.IsError)
                {
                    throw new Exception(err.Message);
                }
                int intCountSize = IntDataSize(sql, (long)(report == null || report.DbServerId == null ? 0 : report.DbServerId));//返回当前集合条数
                if (intCountSize <= 0)
                {
                    return Content("暂无无数据导出！");
                }
                #region 抽样数据
                double WaitTime = (double)expCfg.WaitTime;
                DateTime datEndDate = DateTime.Now.AddMilliseconds(WaitTime);//最大等待时长
                if (DateTime.Now > datEndDate && exportWay == "在线")
                {
                    return Content("在线导出时，由于数据量过大，在统计数据时超出在线最大等待时长，请转为离线导出。是否转为离线导出？");
                }
                long intPumping = GetPumpingSize((long)report.DbServerId, sql, intCountSize, WaitTime, exportWay);//通过样品数据预估数据集大小,如果小于等于0，表示超时
                if (intPumping <= 0)
                {
                    return Content("在线导出时，由于数据量过大，在数据抽样时超出在线最大等待时长，请转为离线导出。是否转为离线导出？");
                }
                #endregion

                //判断是否为离线
                if ((intCountSize > expCfg.MaxRowNum || intPumping > expCfg.DataSize) && exportWay == "在线")
                {
                    return Content("在线导出最大支持" + expCfg.MaxRowNum + "条数据及" + expCfg.DataSize + "KB字节,是否转为离线导出？");
                }

                object objTopFields = "";
                if (tbReportId != 0)
                {
                    var topFieldArr = _tbReportAppService.GetFildTopList(tbReportId);//获取多表头字段集合
                    var fieldArr = _tbReportAppService.GetFildList(tbReportId);//获取字段集合

                    if (topFieldArr != null && topFieldArr.Count > 0)
                    {
                        objTopFields = GetTopFieldForExcel(topFieldArr.ToArray(), fieldArr.ToArray()); //多表头信息
                    }
                }
                //根据url得到module
                var module = _moduleAppService.GetModuleByUrl(url);
                string strExt = GetExtend(fileFormat.ToLower());
                ExportDataModel exp = new ExportDataModel
                {
                    ReportCode = code,
                    DisplayName = (module.Name == null || module.Name.Trim() == "" ? "" : module.Name + "_") + DateTime.Now.Ticks,
                    ExportWay = exportWay,
                    FromUrl = url,
                    FileFormat = strExt,
                    FilePath = expCfg.Path,
                    ValidDay = expCfg.ValidDay,
                    TopFields = objTopFields,//多表头信息
                    ColumnHeader = "",
                    Sql = sql,
                    DbServerId = report == null || report.DbServerId == null ? 0 : report.DbServerId,
                    FileName = (module == null ? "无名称" : module.Name) + "_" + DateTime.Now.Ticks,
                    Status = "生成中",
                    ObjParam = "",
                    IsClose = false
                };
                if (module.Id > 0)
                    exp.ModuleId = module.Id;

                //针对两种形式的导出处理，待完善
                switch (exportWay)
                {
                    case "离线":
                        strResult = _exportAppService.OfflineExportData(exp, intCountSize);
                        break;
                    case "在线":
                        strResult = strHost + _exportAppService.OnlineExportData(exp);
                        break;
                    default:
                        strResult = strHost + _exportAppService.OnlineExportData(exp);
                        break;
                }
            }
            catch (Exception ex)
            {
                err.IsError = false;
                string strHtml = "<script src=\"../Scripts/jquery-2.2.4.min.js\"></script>";
                strHtml += "<script src=\"../Common/rootUrl.js\"></script>";
                strHtml += "<script src=\"../Common/Scripts/errorPage/error.js\"></script>";
                strHtml += "<script>$(function () {SendErrorInfo('导出提示','导出时，程序异常。代码：" + ex.Message + ",请联系管理员')})</script>";
                return Content(strHtml);
            }
            #endregion
            return Content(strResult);
        }
        #endregion

        #region 得到后缀
        /// <summary>
        /// 得到后缀
        /// </summary>
        /// <param name="strExt">后缀名称</param>
        /// <returns></returns>
        private string GetExtend(string strExt)
        {
            switch (strExt.ToLower())
            {
                case "excel":
                    return ".xlsx";
                case "csv":
                    return ".csv";
                case "txt":
                    return ".txt";
                default:
                    return ".xlsx";
            }
         }
        #endregion

        #region 获取数据
        public ActionResult DownLoadRecord()
        {
            if (Request["id"] == null)
            {
                return Content("导出时，参数有误。");
            }
            int intFileId = Convert.ToInt32(Request["id"].Trim());
            if (intFileId <= 0)
            {
                return Content("导出时，参数值有误。");
            }
            try
            {
                DownData downData = _exportAppService.DownLoadRecord(intFileId);
                if (downData != null)
                {
                    string strMapPath = Request.MapPath(downData.FilePath);
                    FileStream fsObj = new FileStream(strMapPath, FileMode.Open);
                    byte[] bytes = new byte[(int)fsObj.Length];
                    fsObj.Read(bytes, 0, bytes.Length);
                    fsObj.Close();
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream;charset=GB2312";
                    //通知浏览器下载文件而不是打开 
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + downData.FileName);
                    System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {
                string strHtml = "<script src=\"../Scripts/jquery-2.2.4.min.js\"></script>";
                strHtml += "<script src=\"../Common/rootUrl.js\"></script>";
                strHtml += "<script src=\"../Common/Scripts/errorPage/error.js\"></script>";
                strHtml += "<script>$(function () {SendErrorInfo('导出提示1', '导出时，程序异常。代码：" + ex.Message + ",请联系管理员')})</script>";
                return Content(strHtml);
            }
            return Content("");
        }
        #endregion

        #region 预估下载的文件大小
        /// <summary>
        /// 预估下载的文件大小
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <param name="ingDbServerId">服务器ID</param>
        /// <returns>返回是否离线下载</returns>
        private int IntDataSize(string strSql,long ingDbServerId)
        {
            strSql = "select count(0) from (" + strSql + ")td";
            //获得参数
            //string code = Request["code"].Trim();
            //string queryParams = Request["queryParams"];//查询条件

            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            err.IsError = false;

            int esitSize = 0; //获得文件预估大小
            #region 得到执行sql语句
            try
            {
                object objSize = _dbServerAppService.ExecuteScalar(ingDbServerId, strSql, ref err);//获取数据总长度
                esitSize = (objSize == null || objSize.ToString().Trim() == "" ? 0 : Convert.ToInt32(objSize));
                if (err.IsError)
                {
                    throw new Exception(err.Message);
                }
            }
            catch (Exception ex)
            {
                //throw new Abp.UI.UserFriendlyException(ex.Message);
                throw new Exception(ex.Message);
            }
            #endregion

            return esitSize;
        }
        #endregion

        #region 得到多表头字段
        /// <summary>
        /// 得到多表头字段
        /// </summary>
        /// <param name="topFieldArr"></param>
        /// <param name="fieldArr"></param>
        /// <returns></returns>
        public ExcelTop[] GetTopFieldForExcel(TbReportFieldTopModel[] topFieldArr, TbReportFieldModel[] fieldArr)
        {
            //只对显示字段拼凑信息
            if (fieldArr != null && fieldArr.Length > 0)
            {
                fieldArr = fieldArr.Where(p => p.IsShow).ToArray();
            }
            var colHeadArr = fieldArr.Select(p => p.FieldCode).ToArray();//获得字段集合

            #region 得到当前多表头的深度-maxNum
            var maxNum = 1;//默认一层
            if (topFieldArr != null && topFieldArr.Length > 0)
            {
                for (var i = 0; i < topFieldArr.Length; i++)
                {
                    var num = 1;//默认一层
                    var parentName = topFieldArr[i].ParentName;//初始化父级名称
                    while (parentName != null && parentName != "")
                    {
                        for (var j = 0; j < topFieldArr.Length; j++)
                        {
                            if (topFieldArr[j].Name == parentName)
                            {
                                parentName = topFieldArr[j].ParentName;//赋父值
                                num++;
                                break;
                            }
                        }
                    }
                    if (num > maxNum)//当前元素深度大于之前的值时，重新赋值
                    {
                        maxNum = num;
                    }
                }
            }
            #endregion

            #region 循环生成多表头的行数组-topRowArr
            //string[] topRowArr = new string[maxNum];
            List<string[]> torRowList = new List<string[]>();

            //从第二层开始生成
            for (var i = 0; i < maxNum - 1; i++)
            {
                string[] topRow = new string[colHeadArr.Length];
                //有可能出现中文别名调整情况，故获取的第二行数据依据FieldCode来查找
                if (i == 0)
                {
                    for (var k = 0; k < colHeadArr.Length; k++)
                    {
                        topRow[k] = "";//值初始化为空
                        for (var j = 0; j < topFieldArr.Length; j++)
                        {
                            var top = topFieldArr[j];
                            if (top.FieldCode == colHeadArr[k])
                            {
                                topRow[k] = top.ParentName;
                                break;
                            }

                            //if (j == topFieldArr.Length - 1)
                            //{
                            //    topRow[k] = "";//如果没有父级则赋空值
                            //}
                        }
                    }
                    torRowList.Add(topRow);//添加
                }
                else
                {
                    //根据上一行设置当前行
                    //var beforeTop = topRowArr[i - 1];
                    var beforeTop = torRowList[i - 1];
                    for (var k = 0; k < colHeadArr.Length; k++)
                    {
                        topRow[k] = "";//值初始化为空
                        for (var j = 0; j < topFieldArr.Length; j++)
                        {
                            var top = topFieldArr[j];
                            if (top.Name == beforeTop[k])
                            {
                                topRow[k] = top.ParentName;//第二行的属性赋值
                                break;
                            }
                            //if (j == topFieldArr.Length - 1)
                            //{
                            //    topRow[k] = "";//如果没有父级则赋空值
                            //}
                        }
                    }
                    //topRowArr.push(topRow);
                    torRowList.Add(topRow);//添加
                }
            }
            #endregion
            var topRowArr = torRowList.Reverse<string[]>().ToArray();

            //为excle拼凑多表头
            List<ExcelTop> excelTopList = new List<ExcelTop>();
            #region 拼凑列字段部分
            if (fieldArr != null && fieldArr.Length > 0)
            {
                for (var i = 0; i < fieldArr.Length; i++)
                {
                    var fd = fieldArr[i];
                    var rowStart = 0;//开始行
                    var rowEnd = maxNum - 1;//结束行

                    var colStart = i;//起始列
                    var colEnd = i;//结束列

                    if (fd.TbReportFieldTopId != null)
                    {
                        rowStart = maxNum - 1;
                    }

                    var item = new ExcelTop
                    {
                        name = fd.FieldName,
                        list = new int[] { rowStart, rowEnd, colStart, colEnd }
                    };
                    excelTopList.Add(item);
                }
            }
            #endregion
            #region 拼凑多表头部分
            if (topRowArr != null && topRowArr.Length > 0)
            {
                //从第一行开始生成合并项
                for (var i = 0; i < topRowArr.Length; i++)
                {
                    var row = topRowArr[i];//当前行
                    //把每一个列拿去与colHeadArr比较，并得到各个多表头的合并项
                    for (var j = 0; j < colHeadArr.Length; j++)
                    {
                        var topName = "";
                        var rowStart = 0;//开始行
                        var rowEnd = 0;//结束行

                        var colStart = 0;//起始列
                        var colEnd = 0;//结束列

                        var end_j = 0;

                        if (row[j] != null && row[j] != "")
                        {
                            topName = row[j];//多表头名
                            end_j = j;//初始

                            #region 设置当前多表头字段的行合并
                            var topFd = topFieldArr.FirstOrDefault(p => p.Name == topName && string.IsNullOrEmpty(p.FieldCode));
                            if (topFd != null)
                            {
                                if (!string.IsNullOrEmpty(topFd.ParentName))
                                {
                                    rowStart = i;
                                    rowEnd = i;
                                }
                                else
                                {
                                    rowStart = 0;
                                    rowEnd = i;
                                }
                            }
                            #endregion

                            #region 设置当前多表头字段的列合并
                            colStart = j;
                            for (var k = j; k < colHeadArr.Length; k++)
                            {
                                if (row[k] == row[j])
                                {
                                    end_j++;
                                }
                            }
                            colEnd = end_j - 1;
                            #endregion

                            //添加一个合并项
                            var item = new ExcelTop
                            {
                                name = topName,
                                list = new int[] { rowStart, rowEnd, colStart, colEnd }
                            };
                            excelTopList.Add(item);
                            j = end_j - 1;//下一个起始列
                        }
                    }
                }
            }
            #endregion
            return excelTopList.ToArray();
        }
        #endregion

        #region 数据抽样统计
        /// <summary>
        /// 数据抽样统计数量
        /// </summary>
        /// <param name="lngDbid">数据库ID</param>
        /// <param name="strSql">数据SQL</param>
        /// <param name="intCountNum">总计数量</param>
        /// <param name="intPumping">抽样个数</param>
        /// <returns></returns>
        private long GetPumpingSize(long lngDbid, string strSql, int intCountNum, double WaitTime, string exportWay,int intPumping = 10)
        {
            if (intCountNum < intPumping)
            {
                intPumping = intCountNum;
            }
            int intMax = (intCountNum - 1);
            if (intMax <= 0)
            {
                intMax = intCountNum;
            }
            int[] intRandom = Common.Fun.GetRandomUnrepeatArray(1, intMax, intPumping);//样品数组(因为数组是从0开始的，所以最大值要减1)
            string strTemp = "";
            foreach (int item in intRandom)
            {
                DateTime datEndDate = DateTime.Now.AddMilliseconds(WaitTime);//最大等待时长
                DataTable dtTempDb = _dbServerAppService.ExecuteGetTable(lngDbid, strSql, item, 1);
                if (DateTime.Now > datEndDate && exportWay == "在线")
                {
                    return -1;
                }
                if (dtTempDb.Rows.Count > 0)
                {
                    foreach (DataColumn comItem in dtTempDb.Columns)
                    {
                        strTemp += dtTempDb.Rows[0][comItem.ColumnName];
                    }
                }
            }
            if (strTemp.Length<=0) {
                return 0;
            }
           int intStrLength = Common.Fun.GetStringLength(strTemp);
            long lngRowAVG = intStrLength / intPumping * intCountNum;//每行的平均大小，乘以总行数;得到大概总大小
            return lngRowAVG;
        }
        #endregion
    }
}