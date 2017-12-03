using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Domain;
using Easyman.Dto;
using EasyMan.Dtos;
using System.Collections.Generic;
using System.Linq;
using Easyman.Common;
using System;
using System.Data;

namespace Easyman.Service
{
    /// <summary>
    /// 主报表管理
    /// </summary>
    public class ReportAppService : EasymanAppServiceBase, IReportAppService
    {
        #region 初始化
        /// <summary>
        /// 主报表
        /// </summary>
        private readonly IRepository<Report, long> _reportRepository;
        private readonly IDbServerAppService _dbServerApp;
        private readonly ITbReportAppService _tbReportApp;
        private readonly IGlobalVarAppService _globalVarApp;
        private readonly IRdlcReportAppService _rdlcReportApp;

        /// <summary>
        /// 构造函数（注入仓储）
        /// </summary>
        /// <param name="reportRepository"></param>
        /// <param name="dbServerApp"></param>
        /// <param name="tbReportApp"></param>
        /// <param name="globalVarApp"></param>
        /// <param name="rdlcReportApp"></param>
        public ReportAppService(IRepository<Report, long> reportRepository, 
            IDbServerAppService dbServerApp,
            ITbReportAppService tbReportApp,
            IGlobalVarAppService globalVarApp,
            IRdlcReportAppService rdlcReportApp
            )
        {
            _reportRepository = reportRepository;
            _dbServerApp = dbServerApp;
            _tbReportApp = tbReportApp;
            _globalVarApp = globalVarApp;
            _rdlcReportApp = rdlcReportApp;
        }

        #endregion

        #region 公共接口方法
        /// <summary>
        /// 根据code获取基础信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ReportOutput GetReportBase(string code)
        {
            var ent = _reportRepository.FirstOrDefault(p => p.Code == code.Trim());
            if (ent != null)
            {
                var report = AutoMapper.Mapper.Map<ReportOutput>(ent);
                return report;
            }
            return null;
        }

        /// <summary>
        /// 根据id获取基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReportOutput GetReportBase(long id)
        {
            try
            {
                var ent = _reportRepository.Get(id);
                if (ent != null)
                {
                    var report = AutoMapper.Mapper.Map<ReportOutput>(ent);
                    return report;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }

        /// <summary>
        /// 根据报表ID获取一条报表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReportOutput GetReport(long id)
        {
            var ent = _reportRepository.Get(id);
            if(ent!=null)
            {
               var report= AutoMapper.Mapper.Map<ReportOutput>(ent);
                //获取子报表集合
                var childReportList= ChildReportList(id,0,false);
                if (childReportList != null && childReportList.Count > 0)
                {
                    report.ChildReportListJson = JSON.DecodeToStr(childReportList);
                }
                return report;
            }
            return null;
        }

        /// <summary>
        /// 根据报表code获取一条报表信息
        /// </summary>
        /// <param name="code">模块编码</param>
        /// <param name="moduleId">菜单号</param>
        /// <param name="checkRole"></param>
        /// <returns></returns>
        public ReportOutput GetReport(string code,long moduleId,bool checkRole=false)
        {
            //获取报表
            var ent = _reportRepository.FirstOrDefault(p => p.Code == code.Trim());
            if (ent != null)
            {
                var report = AutoMapper.Mapper.Map<ReportOutput>(ent);
                //获取子报表集合
                var childReportList = ChildReportList(ent.Id, moduleId, checkRole);
                if (childReportList != null && childReportList.Count > 0)
                {
                    report.ChildReportListJson = JSON.DecodeToStr(childReportList);
                }
                report.EmId = moduleId;//菜单ID
                return report;
            }
            return null;
        }
        /// <summary>
        /// 新增或修改报表信息
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public bool InsertOrUpdateReport(ReportInput report)
        {
            if (report != null)
            {
                if (_reportRepository.GetAll().Any(p => p.Id != report.Id && (p.Name == report.Name || p.Code == report.Code)))
                {
                    throw new System.Exception("报表名称或编码重复");
                }

                var ent = AutoMapper.Mapper.Map<Report>(report);

                //var currReport = _reportRepository.InsertOrUpdate(ent);
                //更新主报表（基础信息）
                var reportId = _reportRepository.InsertOrUpdateAndGetId(ent);

                //更新子报表信息
                if (report.ChildReportListJson != null && report.ChildReportListJson.Length > 0)
                {
                    var reportList = JSON.EncodeToEntity<List<ChildReportModel>>(report.ChildReportListJson);
                    if (reportList != null && reportList.Count > 0)
                    {
                        foreach (var rp in reportList)
                        {
                            //表格式报表保存逻辑
                            if (rp.ChildReportType == (short)ReportEnum.ReportType.KeyValue ||
                                rp.ChildReportType == (short)ReportEnum.ReportType.Table)
                            {
                                //调用表格报表的保存逻辑
                                _tbReportApp.SaveTbReport(rp, reportId,report.Code);
                            }

                            //RDLC报表保存逻辑
                            if (rp.ChildReportType == (short)ReportEnum.ReportType.Rdlc)
                            {
                                //调用RDLC报表保存逻辑
                                _rdlcReportApp.SaveRdlcReport(rp, reportId, report.Code);
                            }

                            //RDLC报表保存--待补充
                            //else if()
                            //{ }
                            //图像报表保存--待补充
                            //else if()
                            //{ }
                        }
                    }
                }
                return true;
            }
            else
                throw new UserFriendlyException("传入信息为空！");
        }

        /// <summary>
        /// 解析sql文的字段
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbserverId"></param>
        /// <returns></returns>
        public string AnalysisSql(string sql, long? dbserverId)
        {
            //err初始化
            ErrorInfo err = new ErrorInfo();
            err.IsError = true;

            IDataReader dr = null;//初始化dr
            sql = string.Format(@"select * from ({0}) where 1<>1", sql);
            //替换全局变量
            sql = ReplaceGlobalVar(sql);
            //替换变量（内置或自定义、外置）
            sql = ReplaceDefaultValue(sql, null);

            if (dbserverId != null)
            {
                try
                {
                    if (dbserverId != 0)
                    {
                        dr = _dbServerApp.ExecuteDataReader(dbserverId.Value, sql);//选择库
                    }
                    else
                    {
                        dr = DbHelper.ExecuteDataReader(sql);//本地承载库
                    }

                    //声明字段集合
                    IList<TbReportFieldModel> fields = new List<TbReportFieldModel>();
                    if (dr != null && dr.FieldCount > 0)
                    {
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            TbReportFieldModel field = new TbReportFieldModel
                            {
                                FieldCode = dr.GetName(i),
                                FieldName = dr.GetName(i),
                                DataType = dr.GetFieldType(i).Name,
                                IsOrder = true,
                                IsShow = true,
                                Width = 60,
                                IsSearch = false,
                                IsFrozen = false,
                                Align = "center",
                                Remark = ""
                            };
                            fields.Add(field);
                        }

                        err.Params = JSON.DecodeToStr(fields);
                        err.IsError = false;
                    }
                    else
                    {
                        err.IsError = true;
                        err.Message = "未解析出字段信息";
                    }
                }
                catch(Exception ex)
                {
                    err.IsError = true;
                    err.Message = "解析失败："+ex.Message;
                }
                finally
                {
                    if(dr!=null)
                    {
                        dr.Close();
                        dr.Dispose();//关闭资源
                    }
                }
            }
            else
            {
                err.IsError = true;
                err.Message = "未选择数据库";
            }
            return JSON.DecodeToStr(err);
        }

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
        public string ExcuteReportSql(string code, int rows, int page,string queryParams,string sidx,string sord, ref ErrorInfo err)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var ent = _reportRepository.FirstOrDefault(p => p.Code == code);
                if (ent != null)
                {
                    var dbserver = _dbServerApp.GetDbServer(ent.DbServerId.Value);
                    if (dbserver != null)
                    {
                        //替换全局变量
                        string sql = ReplaceGlobalVar(ent.Sql);
                        //替换变量（内置或自定义、外置）
                        sql = ReplaceDefaultValue(sql, queryParams);
                        //当IsPlaceholder==true时,拼凑查询筛选条件
                        if (ent.IsPlaceholder == null || !ent.IsPlaceholder.Value)
                        {
                            sql = AnalysisParam(dbserver.DbTypeName, sql, queryParams);
                        }
                        //获取总数
                        string sqlC = string.Format(@"select count(1) from ({0})", sql);
                        object obj = _dbServerApp.ExecuteScalar(ent.DbServerId.Value, sqlC, ref err);

                        sql = SqlForOrder(sql, sidx, sord);//生成排序sql
                        //拼凑分页sql
                        string sqlPage= SqlForPage(dbserver, sql, page, rows, ref err);
                        //执行带有分页的sql
                        DataTable endTable = _dbServerApp.ExecuteGetTable(ent.DbServerId.Value, sqlPage);
                       
                        int records = Convert.ToInt32(obj);

                        //拼凑json串
                        string result = "{\"records\":" + records + ",\"page\":" + page + ",\"total\":" + Math.Ceiling(decimal.Divide(records, rows)) + ",\"rows\":" + JSON.DecodeToStr(endTable) + "}";
                        return result;
                    }
                    else
                    {
                        err.IsError = true;
                        err.Message = "未找到当前code对应的数据库编号["+ ent.DbServerId.Value + "]！";
                    }
                }
                else
                {
                    err.IsError = true;
                    err.Message = "未找到编号为["+code+"]的报表！";
                }
            }
            else
            {
                err.IsError = true;
                err.Message = "传入的代码不能为空！";
            }
            return "";
        }
        /// <summary>
        /// 根据code代码及传入条件拼凑sql
        /// </summary>
        /// <param name="code"></param>
        /// <param name="queryParams"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public string GetDebugSql(string code, string queryParams, ref ErrorInfo err)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(code))
            {
                var ent = _reportRepository.FirstOrDefault(p => p.Code == code);
                if (ent != null)
                {
                    var dbserver = _dbServerApp.GetDbServer(ent.DbServerId.Value);
                    if (dbserver != null)
                    {
                        //替换全局变量
                        sql = ReplaceGlobalVar(ent.Sql);
                        //替换变量（内置或自定义、外置）
                        sql = ReplaceDefaultValue(sql, queryParams);
                        //当IsPlaceholder==true时,拼凑查询筛选条件
                        if (ent.IsPlaceholder == null || !ent.IsPlaceholder.Value)
                        {
                            sql = AnalysisParam(dbserver.DbTypeName, sql, queryParams);
                        }
                    }
                    else
                    {
                        err.IsError = true;
                        err.Message = "未找到当前code对应的数据库编号[" + ent.DbServerId.Value + "]！";
                    }
                }
                else
                {
                    err.IsError = true;
                    err.Message = "未找到编号为[" + code + "]的报表！";
                }
            }
            else
            {
                err.IsError = true;
                err.Message = "传入的code代码不能为空！";
            }
            return sql;
        }

        public string GetSqlForField(string code, string queryParams,long tbReportId, ref ErrorInfo err)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(code))
            {
                var ent = _reportRepository.FirstOrDefault(p => p.Code == code);
                if (ent != null)
                {
                    var dbserver = _dbServerApp.GetDbServer(ent.DbServerId.Value);
                    if (dbserver != null)
                    {
                        //替换全局变量
                        sql = ReplaceGlobalVar(ent.Sql);
                        //替换变量（内置或自定义、外置）
                        sql = ReplaceDefaultValue(sql, queryParams);
                        //当IsPlaceholder==true时,拼凑查询筛选条件
                        if (ent.IsPlaceholder == null || !ent.IsPlaceholder.Value)
                        {
                            sql = AnalysisParam(dbserver.DbTypeName, sql, queryParams);
                        }
                        //如果为0时，表示非常规表报
                        if (tbReportId==0) {
                            return sql;
                        }

                        //按照字段排序拼凑需要sql字段及顺序
                        var fieldList = _tbReportApp.GetFildList(tbReportId);//获取字段集合
                        if (fieldList != null && fieldList.Count > 0)
                        {
                            string fields = "";
                            foreach (var fd in fieldList)
                            {
                                if (fd.IsShow)
                                    fields += fd.FieldCode + " as " + fd.FieldName + ",";
                            }
                            if (!string.IsNullOrEmpty(fields) && fields.Length > 0)
                            {
                                fields = fields.Substring(0, fields.Length - 1);
                            }
                            sql = string.Format(@"select {0} from ({1})", fields, sql);
                        }
                    }
                    else
                    {
                        err.IsError = true;
                        err.Message = "未找到当前code对应的数据库编号[" + ent.DbServerId.Value + "]！";
                    }
                }
                else
                {
                    err.IsError = true;
                    err.Message = "未找到编号为[" + code + "]的报表！";
                }
            }
            else
            {
                err.IsError = true;
                err.Message = "传入的code代码不能为空！";
            }
            return sql;
        }

        /// <summary>
        /// 根据传入code执行报表，返回datatable
        /// </summary>
        /// <param name="code"></param>
        /// <param name="queryParams"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromCode(string code, string queryParams, ref ErrorInfo err)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(code))
            {
                var ent = _reportRepository.FirstOrDefault(p => p.Code == code);
                if (ent != null)
                {
                    var dbserver = _dbServerApp.GetDbServer(ent.DbServerId.Value);
                    if (dbserver != null)
                    {
                        //替换全局变量
                        string sql = ReplaceGlobalVar(ent.Sql);
                        //替换变量（内置或自定义、外置）
                        sql = ReplaceDefaultValue(sql, queryParams);
                        //当IsPlaceholder==true时,拼凑查询筛选条件
                        if (ent.IsPlaceholder == null || !ent.IsPlaceholder.Value)
                        {
                            sql = AnalysisParam(dbserver.DbTypeName, sql, queryParams);
                        }
                        dt = _dbServerApp.ExecuteGetTable(dbserver.Id, sql);//执行sql
                    }
                    else
                    {
                        err.IsError = true;
                        err.Message = "未找到当前code对应的数据库编号[" + ent.DbServerId.Value + "]！";
                    }
                }
                else
                {
                    err.IsError = true;
                    err.Message = "未找到编号为[" + code + "]的报表！";
                }
            }
            else
            {
                err.IsError = true;
                err.Message = "传入的code代码不能为空！";
            }
            return dt;
        }

        /// <summary>
        /// 根据传入code执行报表，返回datatable
        /// </summary>
        /// <param name="code"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public string GetSqlByCode(string code, string queryParams)
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(code))
            {
                var ent = _reportRepository.FirstOrDefault(p => p.Code == code);
                if (ent != null)
                {
                    var dbserver = _dbServerApp.GetDbServer(ent.DbServerId.Value);
                    if (dbserver != null)
                    {
                        //替换全局变量
                        sql = ReplaceGlobalVar(ent.Sql);
                        //替换变量（内置或自定义、外置）
                        sql = ReplaceDefaultValue(sql, queryParams);
                        //当IsPlaceholder==true时,拼凑查询筛选条件
                        if (ent.IsPlaceholder == null || !ent.IsPlaceholder.Value)
                        {
                            sql = AnalysisParam(dbserver.DbTypeName, sql, queryParams);
                        }
                        //dt = _dbServerApp.ExecuteGetTable(dbserver, sql);//执行sql
                    }
                }
            }
            return sql;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 替换全局变量
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string ReplaceGlobalVar(string sql)
        {
            var globalList= _globalVarApp.GetGlobalVarList();
            if(globalList!=null&&globalList.Count>0)
            {
                foreach(var global in globalList)
                {
                    sql = sql.Replace("@("+global.Name+")", global.Value);
                }
            }
            return sql;
        }

        /// <summary>
        /// 解析报表查询,返回带查询条件的sql
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="sql"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        private string AnalysisParam(string dbType, string sql, string queryParams)
        {
            string searchs = "";//查询条件初始化

            if(queryParams!=null&&queryParams!=""&& queryParams!="[]")
            {
                List<QueryParam> paramList = JSON.EncodeToEntity<List< QueryParam>>(queryParams);
                if (paramList != null && paramList.Count > 0)
                {
                    searchs += " WHERE 1=1 ";//有条件参数的初始化
                    foreach (var param in paramList)
                    {
                        if(!string.IsNullOrEmpty( param.Value)&&!string.IsNullOrEmpty( param.OpType))
                        {
                            //"Decimal:数值型;String:字符串;Int32:整型;Int64:长整型;Int16:短整型;DateTime:时间"
                            switch (param.DataType)
                            {
                                case "String":
                                    if (param.OpType == "like")
                                    {
                                        searchs += " AND " + param.FieldCode + " "+param.OpType + " '%" + param.Value + "%'";
                                    }
                                    else if (param.OpType == "in")
                                    {
                                        string vas = "";
                                        //按以下分隔符进行分隔
                                        string[] valArr = param.Value.Split(new char[] { ',', '|', '，', '、', ' ' });
                                        if (valArr != null && valArr.Length > 0)
                                        {
                                            foreach (var v in valArr)
                                            {
                                                vas += "'" + v + "',";
                                            }
                                        }
                                        searchs += " AND " + param.FieldCode +" "+ param.OpType + " (" + vas.Substring(0, vas.Length - 1) + ")";
                                    }
                                    else if (param.OpType == "=")
                                    {
                                        searchs += " AND " + param.FieldCode + " " + param.OpType + " '" + param.Value + "'";
                                    }
                                    else
                                    {
                                        searchs += " AND " + param.FieldCode + " " + param.OpType + " '" + param.Value + "'";
                                    }
                                    break;
                                case "Decimal":case "Int32":case "Int64":case "Int16":
                                    switch (param.OpType)
                                    {
                                        case "=":case ">":case "<":
                                            searchs += " AND " + param.FieldCode + " " + param.OpType + " " + param.Value;
                                            break;
                                        case "in":
                                            //先替换分隔符
                                            string[] valArr = param.Value.Split(new char[] { ',', '|', '，', '、', ' ' });
                                            searchs += " AND " + param.FieldCode + " " + param.OpType + " (" + string.Join(",", valArr) + ")";
                                            break;
                                        default:
                                            searchs += " AND " + param.FieldCode + " " + param.OpType + " " + param.Value;
                                            break;
                                    }
                                    break;
                                case "DateTime"://事件字段处理
                                    switch (dbType.ToUpper())
                                    {
                                        case "DB2":
                                        case "ORACLE":
                                            if (param.FilterType == ReportEnum.FilterType.DataYYYYMMDD.GetHashCode().ToString())
                                            {
                                                switch (param.OpType)
                                                {
                                                    case ">":
                                                        searchs += " AND " + param.FieldCode + " >= TO_DATE('" + param.Value + " 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                        break;
                                                    case "=":
                                                        searchs += " AND " + param.FieldCode + " >= TO_DATE('" + param.Value + " 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                        searchs += " AND " + param.FieldCode + " <= TO_DATE('" + param.Value + " 23:59:59','YYYY-MM-DD HH24:MI:SS') ";
                                                        break;
                                                    case "<":
                                                        searchs += " AND " + param.FieldCode + " < TO_DATE('" + param.Value + " 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                        break;
                                                    case "in":
                                                        //先替换分隔符
                                                        //按以下分隔符进行分隔
                                                        string[] valArr = param.Value.Split(new char[] { ',', '|', '，', '、', ' ' });
                                                        if (valArr != null && valArr.Length > 0)
                                                        {
                                                            searchs += " AND (";
                                                            for (int i = 0; i < valArr.Length; i++)
                                                            {
                                                                var v = valArr[i];
                                                                searchs += " (" + param.FieldCode + " >= TO_DATE('" + v + " 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                                searchs += " AND " + param.FieldCode + " <= TO_DATE('" + v + " 23:59:59','YYYY-MM-DD HH24:MI:SS')) ";
                                                                if (i != valArr.Length - 1)
                                                                {
                                                                    searchs += " OR ";
                                                                }
                                                            }
                                                            searchs += ") ";
                                                        }
                                                        break;
                                                    default:
                                                        searchs += " AND " + param.FieldCode + " " + param.OpType + " TO_DATE('" + param.Value + " 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                        break;
                                                }
                                            }
                                            else if(param.FilterType == ReportEnum.FilterType.DataYYYYMM.GetHashCode().ToString())
                                            {
                                                switch (param.OpType)
                                                {
                                                    case ">":
                                                        searchs += " AND " + param.FieldCode + " >= TO_DATE('" + param.Value + "-01 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                        break;
                                                    case "=":
                                                        searchs += " AND " + param.FieldCode + " >= TO_DATE('" + param.Value + "-01 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                        DateTime endT= Convert.ToDateTime(param.Value + "-01 00:00:00").AddMonths(1);
                                                        searchs += " AND " + param.FieldCode + " < TO_DATE('"+ endT + "','YYYY-MM-DD HH24:MI:SS') ";
                                                        break;
                                                    case "<":
                                                        searchs += " AND " + param.FieldCode + " < TO_DATE('" + param.Value + "-01 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                        break;
                                                    case "in":
                                                        //先替换分隔符
                                                        //按以下分隔符进行分隔
                                                        string[] valArr = param.Value.Split(new char[] { ',', '|', '，', '、', ' ' });
                                                        if (valArr != null && valArr.Length > 0)
                                                        {
                                                            searchs += " AND (";
                                                            for (int i = 0; i < valArr.Length; i++)
                                                            {
                                                                var v = valArr[i];
                                                                searchs += " (" + param.FieldCode + " >= TO_DATE('" + v + "-01 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                                DateTime end = Convert.ToDateTime(v + "-01 00:00:00").AddMonths(1);
                                                                searchs += " AND " + param.FieldCode + " < TO_DATE('"+ end + "','YYYY-MM-DD HH24:MI:SS')) ";
                                                                if (i != valArr.Length - 1)
                                                                {
                                                                    searchs += " OR ";
                                                                }
                                                            }
                                                            searchs += ") ";
                                                        }
                                                        break;
                                                    default:
                                                        searchs += " AND " + param.FieldCode + " " + param.OpType + " TO_DATE('" + param.Value + "01 00:00:00','YYYY-MM-DD HH24:MI:SS') ";
                                                        break;
                                                }
                                            }
                                                break;
                                        case "SQLSERVER":
                                            break;
                                        case "MYSQL":
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                default:
                                    break;
                            };
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchs)&& searchs!= " WHERE 1=1 ")
            {
                searchs = " WHERE " + searchs.Substring(searchs.IndexOf("AND") + 3);
                return "SELECT * FROM (" + sql + ") " + searchs; 
            }
            else
                return sql;
        }

        /// <summary>
        /// 替换内置变量+外置变量,返回替换变量后的sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        private string ReplaceDefaultValue(string sql, string queryParams)
        {
            Users.User user= GetCurrentUserAsync().Result;
            if (user != null)
            {
                //预设置的内置表里有
                // @{USER_ID} @{ALL_ROLE} @{USER_NAME} @{NOW_DATE}=yyyy-MM-dd
                // @{DEPARTMENT_ID} @{DEPARTMENT_NAME}
                // @{DISTRICT_ID} @{DISTRICT_NAME}
                var user_id = user.Id;
                var user_name = user.Name;
                var roles = string.Join(",", user.Roles.Select(p => p.Id));
                var now_date = DateTime.Now.ToString("yyyy-MM-dd");
                var department_id = user.DepartmentId ?? 0;
                var department_name = user.Department == null ? "无组织" : user.Department.Name;
                var district_id = user.DistrictId ?? 0;
                var district_name = user.District == null ? "无组织" : user.District.Name;
                sql = sql.
                    Replace("@{USER_ID}", user_id.ToString()).
                    Replace("@{ALL_ROLE}", roles.ToString()).
                    Replace("@{USER_NAME}", user_name.ToString()).
                    Replace("@{DEPARTMENT_ID}", department_id.ToString()).
                    Replace("@{DEPARTMENT_NAME}", department_name.ToString()).
                    Replace("@{DISTRICT_ID}", district_id.ToString()).
                    Replace("@{DISTRICT_NAME}", district_name.ToString()).
                    Replace("@{NOW_DATE}", now_date.ToString());
            }

            //替换自定义-外置变量queryParams
            if (queryParams != null && queryParams != "")
            {
                List<QueryParam> paramList = JSON.EncodeToEntity<List<QueryParam>>(queryParams);
                if (paramList != null && paramList.Count > 0)
                {
                    foreach (var par in paramList)
                    {
                        #region 下载参数特殊处理
                        //if (sql.IndexOf("@(souce)") >= 0)
                        //{
                        //    if (par.FieldParam == "souce")
                        //    {
                        //        if (par.Value != "")
                        //            sql = sql.Replace("@(" + par.FieldParam + ")", " and lower(from_url) like '%" + par.Value + "%'");
                        //        else
                        //            sql = sql.Replace("@(" + par.FieldParam + ")", " and lower(from_url) not like '%" + par.Value + "%'");
                        //    }
                        //}
                        #endregion

                        if (!string.IsNullOrEmpty(par.FieldCode))
                        {
                            sql = sql.Replace("@(" + par.FieldCode + ")", par.Value);
                        }
                        else
                        {
                            sql = sql.Replace("@(" + par.FieldParam + ")", par.Value);
                        }
                    }
                }
            }
            #region 处理在没有参数的情况下的参数
            //if (sql.IndexOf("@(souce)") >= 0)
            //{
            //    sql = sql.Replace("@(souce)", "");
            //}
            #endregion

            return sql;
        }

        /// <summary>
        /// 获取子报表列表
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="moduleId"></param>
        /// <param name="checkRole"></param>
        /// <returns></returns>
        private IList<ChildReportModel> ChildReportList(long reportId,long moduleId, bool checkRole)
        {
            //声明一个子报表的集合
            var ChildReportList = new List<ChildReportModel>();
            //获取表格报表
            var tbList= _tbReportApp.GetChildListFromTbReport(reportId, moduleId, checkRole);
            if(tbList!=null&&tbList.Count>0)
            {
                ChildReportList= ChildReportList.Concat(tbList).ToList();
            }

            //获取图形报表(待补充)

            //获取RDLC报表
            var rdlcList= _rdlcReportApp.GetChildListFromRdlcReport(reportId, moduleId, checkRole);
            if(rdlcList!=null&&rdlcList.Count>0)
            {
                ChildReportList = ChildReportList.Concat(rdlcList).ToList();
            }

            return ChildReportList;
        }
        /// <summary>
        /// 拼凑分页sql
        /// </summary>
        /// <param name="dbServer"></param>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        private string SqlForPage(DbServerOutput dbServer, string sql, int pageIndex, int pageSize, ref ErrorInfo err)
        {
            string rownNum = "ROWNUM ";//初始化编号

            switch (dbServer.DbTypeName.ToUpper())
            {
                case "DB2":
                    rownNum = string.Format(" ROW_NUMBER() OVER( PARTITION BY 1 ) ");
                    break;
                case "ORACLE":
                    rownNum = "ROWNUM ";
                    break;
                case "SQLSERVER":
                    string nullSql = "SELECT * FROM ( " + sql + " ) T WHERE 1<>1";
                    DataTable dt = _dbServerApp.ExecuteGetTable(dbServer.Id, nullSql, ref err);
                    rownNum = dt.Columns[0].Caption;
                    rownNum = string.Format("ROW_NUMBER() OVER (ORDER BY {0} DESC)", rownNum);
                    break;
            }
            int startNum = (pageIndex - 1) * pageSize;
           
            string nowsql = string.Format(@"        
                SELECT *
                FROM (SELECT {1} N, T.*
                        FROM ({0}) T)
                WHERE N > {2} AND N <= {3}", sql, rownNum, startNum, startNum + pageSize);
            return nowsql;
        }

        /// <summary>
        /// 拼凑sql排序信息
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <returns></returns>
        private string SqlForOrder(string sql, string sidx, string sord)
        {
            if (!string.IsNullOrEmpty(sidx))
            {
                //var orderGroup = sidx.Split(',');
                //if(orderGroup!=null&&orderGroup.Length>0)
                //{
                //    string orders = "";
                //    foreach(var od in orderGroup)
                //    {
                //        var oneO = od.Replace(' ','+').Split('+');
                //        if(oneO!=null&&oneO.Length>0)
                //        {
                //            if(oneO.Length>1)
                //            {
                //            orders +=" "+oneO[0].Trim()+" "+oneO[1].Trim()+",";
                //            }
                //            else
                //            {
                //                orders += " " + oneO[0].Trim() + " " + sord.Trim() + ",";
                //            }
                //        }
                //    }
                //    //拼凑order
                //    if(orders!=null&&orders.Length>0)
                //    {
                //        sql = string.Format(@"select * from ({0}) order by {1}", sql, orders.Substring(0,orders.Length-1));
                //    }
                //}

                sql = string.Format(@"select * from ({0}) order by {1}", sql, sidx+" "+sord);
            }
            return sql;
        }

        #endregion
    }
}