using Abp.Application.Services;
using Abp.Domain.Repositories;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using EasyMan.Common.Data;
using System.Data;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Abp.Domain.Uow;
using System.IO;
using Easyman.Service;
using System.Web;
using System.Configuration;
using System.Xml;
using System.Text;
using EasyMan.Dtos;

namespace Easyman.Import
{
    /// <summary>
    /// 导入信息日志
    /// </summary>
    public class ImportLogAppService : ApplicationService, IImportLogAppService
    {
        #region 初始化

        private readonly IRepository<ImpTb, long> _impTbRepository;
        private readonly IRepository<ImpTbCase, long> _impTbCaseRepository;
        private readonly IRepository<Files, long> _filesRepository;
        private readonly IRepository<ImportLog, long> _importLogRepository;
        private readonly IRepository<Module, long> _moduleRepository;
        private readonly IRepository<ImpTbField, long> _impTbFieldRepository;
        private readonly IDbServerAppService _dbServerAppService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="impTbRepository"></param>
        /// <param name="importLogRepository"></param>
        /// <param name="impTbCaseRepository"></param>
        /// <param name="filesRepository"></param>
        /// <param name="moduleRepository"></param>
        /// <param name="impTbFieldRepository"></param>
        /// <param name="dbServerAppService"></param>
        public ImportLogAppService(IRepository<ImpTb, long> impTbRepository, IRepository<ImportLog, long> importLogRepository,
            IRepository<ImpTbCase, long> impTbCaseRepository, IRepository<Files, long> filesRepository,
            IRepository<Module, long> moduleRepository, IRepository<ImpTbField, long> impTbFieldRepository,
            IDbServerAppService dbServerAppService)
        {
            _impTbRepository = impTbRepository;
            _importLogRepository = importLogRepository;
            _impTbCaseRepository = impTbCaseRepository;
            _filesRepository = filesRepository;
            _moduleRepository = moduleRepository;
            _impTbFieldRepository = impTbFieldRepository;
            _dbServerAppService = dbServerAppService;
        }

        #endregion
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input">输入实体</param>
        [UnitOfWork]
        public void Add(ImportLogInput input)
        {
            //用户ID
            var userId = AbpSession.UserId;
            //获取外导表
            var impTb = _impTbRepository.FirstOrDefault(a => a.Code == input.Code);
            if (impTb == null) "找不到表信息".ErrorMsg();
            //获取上传文件
            var file = _filesRepository.FirstOrDefault(a => a.Id == input.FileId.Value);
            if (file == null || (file != null && !File.Exists(file.Path))) "文件不存在!".ErrorMsg();
            var dbServer = impTb.DbServer;//得到数据库对像
            //设置实例名称
            string caseTableName = impTb.EnTableName;
            switch (impTb.Rule)
            {
                case "1":
                    caseTableName = impTb.EnTableName + "_" + DateTime.Now.ToString("yyMMdd");
                    break;
                case "3":
                    caseTableName = impTb.EnTableName + "_" + dbServer.User.ToString();
                    break;
                case "4":
                    caseTableName = impTb.EnTableName + "_" + input.Suffix.ToString();
                    break;
            }
                        
            //创建外导表实例
            var impTbCase = new ImpTbCase()
            {
                CaseTableName = caseTableName,
                ImpTbId = impTb.Id
            };
            var impTbCaseId = _impTbCaseRepository.InsertAndGetId(impTbCase);
            //批次代码和批次名
            var module = _moduleRepository.FirstOrDefault(a => a.Code == input.ModuleCode);
            var batchName = module != null ? module.Name : file.Name;
            var batchHeader = DateTime.Now.ToString("yyMMdd");
            var batchNumber = 1;
            var implogNext = _importLogRepository.GetAll().OrderByDescending(a => a.Id)
                .FirstOrDefault(a => a.UserId == userId && a.Code.Contains(batchHeader));
            if (implogNext != null)
            {
                var batchArr = implogNext.Code.Split('-');
                if (batchArr.Count() > 0)
                {
                    if (batchArr[0] == batchHeader)
                    {
                        batchNumber = Int32.Parse(batchArr[1]) + 1;
                    }
                }
            }
            batchName += "-" + batchHeader;
            var batchCode = string.Format("{0}-{1}", batchHeader, batchNumber);
            //创建外导信息日志
            var impLog = new ImportLog()
            {
                FileId = input.FileId.Value,
                FileName = file.Name,
                ImpMode = input.ImportMode,
                ImpTbId = impTb.Id,
                ImpTbCaseId = impTbCaseId,
                CaseTableName = caseTableName,
                Duration = 0,
                Code = batchCode,
                Name = batchName,
                UserId = userId.Value
            };
            var result = _importLogRepository.Insert(impLog);
            //数据导入操作
            if (result != null)
            {
                //创建表执行脚本
                var sqlScript = string.Format(impTb.Sql, result.CaseTableName);
                try
                {
                    #region 判断表是否存在                    
                    string strSql = Easyman.Common.DatabaseHelper.GetIsDataBaseTableSql(dbServer.DataCase, caseTableName, dbServer.User);
                    ErrorInfo er = new ErrorInfo();
                    object objReValu = _dbServerAppService.ExecuteScalar(dbServer.Id, strSql, ref er);
                    if (Convert.ToInt32(objReValu) <= 0)
                        _dbServerAppService.Execute(dbServer.Id, sqlScript);
                    #endregion
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("名称已由现有对象使用") <= -1)
                    {
                        //throw new ArgumentNullException(ex.Message);
                        throw new Exception("sql执行失败：" + ex.Message);
                    }
                }
                if (result.ImpMode == "离线导入") //0.判断导入模式是否离线，离线则进度任务调度中去，否则继续执行
                {
                    //进入离线模式
                    return;
                }
                //输入导入操作
                var importReult = ImportData(file.Path, impTb, batchCode, result.CaseTableName, dbServer.Id);
                if (importReult) File.Delete(file.Path);

                //old code
                //var connectionString = string.Empty;
                //DatabaseType databaseType = DatabaseType.Oracle;
                //switch (dbType)
                //{
                //    default://默认oracle数据库
                //        connectionString = string.Format("Data Source={0}:{1}/{2};User Id={3};Password={4};Connect Timeout =200",
                //  dbServer.Ip, dbServer.Port, dbServer.DataCase, dbServer.User, dbServer.Password);
                //        break;
                //    case "db2":
                //        connectionString = dbServer.Port == null ?
                //            string.Format("Driver={IBM DB2 ODBC DRIVER};Server={0}:{1};DSN={2};UID={3};PWD={4};Protocol=TCPIP",
                //            dbServer.Ip, dbServer.Port, dbServer.DataCase, dbServer.User, dbServer.Password)
                //            : string.Format("Driver={IBM DB2 ODBC DRIVER};Server={0};DSN={1};UID={2};PWD={3};Protocol=TCPIP",
                //            dbServer.Ip, dbServer.DataCase, dbServer.User, dbServer.Password);
                //        databaseType = DatabaseType.Db2;
                //        break;
                //    case "mysql":
                //        connectionString = string.Format("Server={0};Port={1};Database={2];Uid={3};Pwd={4];",
                //            dbServer.Ip, dbServer.Port, dbServer.DataCase, dbServer.User, dbServer.Password);
                //        databaseType = DatabaseType.MySql;
                //        break;
                //    case "sqlserver":
                //        connectionString = dbServer.Port == null ?
                //            string.Format("server={0};uid={2};pwd={3};database={4}", dbServer.Ip, dbServer.User,
                //            dbServer.Password, dbServer.DataCase)
                //            : string.Format("server={0}:{1};uid={2};pwd={3};database={4}", dbServer.Ip, dbServer.Port,
                //            dbServer.User, dbServer.Password, dbServer.DataCase);
                //        databaseType = DatabaseType.SqlServer;
                //        break;
                //}
                //if (!string.IsNullOrEmpty(connectionString))
                //{
                //    //创建表执行脚本
                //    var sqlScript = string.Format(impTb.Sql, result.CaseTableName);
                //    //创建表操作
                //    CreateTable(connectionString, sqlScript, databaseType);
                //    //执行数据导入操作
                //    if (result.ImpMode == "离线导入") //0.判断导入模式是否离线，离线则进度任务调度中去，否则继续执行
                //    {
                //        //进入离线模式
                //        return;
                //    }
                //    //输入导入操作
                //    var importReult = ImportData(file.Path, impTb, batchCode, result.CaseTableName,
                //        connectionString, dbType);
                //    if (importReult) File.Delete(file.Path);
                //}
            }
        }

        #region 数据导入
        /// <summary>
        /// 数据导入
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="impTb">外导表实例</param>
        /// <param name="batchCode">批次码</param>
        /// <param name="CaseTableName">实例名称</param>
        /// <param name="dbId">执行库id</param>
        /// <returns>执行情况</returns>
        // [UnitOfWork]
        private bool ImportData(string filePath, ImpTb impTb, string batchCode, string CaseTableName, long dbId)
        {
            //1.拿到指定的excel文件中的数据到datatable中
            if (!File.Exists(filePath)) "导入的文件不存在".ErrorMsg();
            var dataTable = ExcelHelper.ToDataTable(filePath);
            //获取默认字段和表字段
            var defaultFields = impTb.DefaultField;
            var impTbFields = _impTbFieldRepository.GetAllList(a => a.ImpTbId == impTb.Id);
            //2.更换中文表头为英文表头及添加内置字段表头
            foreach (DataColumn col in dataTable.Columns)
            {
                col.ColumnName = impTbFields.FirstOrDefault(a => a.FieldName == col.ColumnName).FieldCode;
            }
            //3.增加内置字段到datatable中
            foreach (var head in defaultFields)
            {
                dataTable.Columns.Add(head.FieldCode);
            }
            //3.1、添加内嵌字段IMP_DATA_COMPLETE_DEFAULT到datatable中
            dataTable.Columns.Add("IMP_DATA_COMPLETE_DEFAULT");
            dataTable.Columns.Add("IMP_ERROR_MSG_DEFAULT");

            #region 添加系统字段
            dataTable.Columns.Add("createDate");
            dataTable.Columns.Add("createUser");
            dataTable.Columns.Add("createImport");
            #endregion

            //4.添加内置字段到datatable中，且对datatable中的数据进行正则校验
            var importTime = DateTime.Now.ToString("yyyy-MM-dd");
            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                foreach (var field in defaultFields)
                {
                    try
                    {
                        var isContain = dataTable.Columns.Contains(field.FieldCode);
                        if (isContain)
                        {
                            var defaultValue = field.DefaultValue;                           
                            dataTable.Rows[row][field.FieldCode] = defaultValue;
                        }
                    }
                    catch (Exception) { }
                }
                #region 添加系统字段               
                dataTable.Rows[row]["createDate"] = importTime;
                dataTable.Rows[row]["createImport"] = batchCode;
                dataTable.Rows[row]["createUser"] = AbpSession.UserId.ToString();                
                #endregion
                dataTable.Rows[row]["IMP_DATA_COMPLETE_DEFAULT"] = "完整性数据";
                foreach (DataColumn col in dataTable.Columns)
                {
                    var value = dataTable.Rows[row][col.ColumnName].ToString();
                    var field = impTbFields.FirstOrDefault(a => a.FieldCode == col.ColumnName);
                    if (field != null && !Regex.IsMatch(value, field.Regular.Regular == null ? "" : field.Regular.Regular))
                    {
                        try
                        {
                            var jsonData = dataTable.Rows[row]["IMP_ERROR_MSG_DEFAULT"].ToString();
                            var errorMsgList = JsonConvert.DeserializeObject<List<object>>(jsonData);
                            if (errorMsgList == null) errorMsgList = new List<object>();
                            errorMsgList.Add(new { ColumnName = field.FieldName, OriginValue = value, ErrorMsg = field.Regular.ErrorMsg });
                            dataTable.Rows[row]["IMP_ERROR_MSG_DEFAULT"] += JsonConvert.SerializeObject(errorMsgList);
                        }
                        catch (Exception ex) { }
                        //  dataTable.Rows[row][col.ColumnName] = null;
                        //   dataTable.Rows[row]["IMP_DATA_COMPLETE_DEFAULT"] = "非完整性数据";
                    }
                }
            }
            //5.把数据存储到表中去
            dataTable.TableName = CaseTableName;
            DataTable newDt = dataTable.Clone();
            bool bolResult = false;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                    DataRow row = newDt.NewRow();
                foreach (DataColumn item in dataTable.Columns)
                {
                    row[item.ColumnName] = dataTable.Rows[i][item.ColumnName];
                }
                    newDt.Rows.Add(row);
                if ((i % 500 == 0 && i != 0 )|| i == dataTable.Rows.Count - 1)
                {
                    string strData = Easyman.Common.JSON.Dtb2Json(newDt);//DT转成JSON数据库
                    string strSql = GetSql(newDt, CaseTableName);//得到SQL
                    
                    bolResult = _dbServerAppService.Execute(impTb.DbServer.Id, strSql, strData);
                    
                    newDt.Clear();//清除数据
                    if (!bolResult)
                    {
                        break;
                    }
                }
            }
            return bolResult;
        }
        #endregion

        #region 得出SQL
        /// <summary>
        /// 得出SQL
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private string GetSql(DataTable dt,string TableName)
        {
            string strResult = "select * from "+ TableName;
            if (TableName ==null)
                return "";
            
            if (dt.Rows.Count <= 0)
            {
                return strResult;
            }
            return strResult;

            //string strColum = "";
            //string strValues = "";
            //foreach (DataColumn itemColum in dt.Columns)
            //{
            //    strColum += (strColum != "" ? "," : "") + itemColum;
            //    strValues+= (strValues != "" ? "," : "") + "@"+itemColum;
            //}
            //strResult= "insert into " + TableName +" ("+ strColum + ") values(" + strValues + ")";
            //return strResult;
        }
        #endregion

     

        #region 上传文件方法
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        public void UploadFile()
        {
            var fileLists = System.Web.HttpContext.Current.Request.Files;
            if (fileLists.Count <= 0)
            {
                //SendData(0);
                return;
            }
            long FilesId = 0;
            try
            {
                for (int i = 0; i < fileLists.Count; i++)
                {
                    HttpPostedFile fileItem = fileLists[i];
                    string strFileName = fileItem.FileName;
                    string strSaveFile = "";
                    //判断根目录
                    if (System.Web.HttpContext.Current.Request.ApplicationPath == "/")
                    {
                         strSaveFile = ConfigurationManager.AppSettings["SaveFile"].ToString();
                    }
                    else
                    {
                        strSaveFile = System.Web.HttpContext.Current.Request.ApplicationPath + ConfigurationManager.AppSettings["SaveFile"].ToString();
                    }

                    string strUrl = strSaveFile + "/" + DateTime.Now.Month;
                    string strPath = HttpContext.Current.Server.MapPath(strUrl);
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }

                    //新文件名称
                    string [] strSuffix = fileItem.FileName.Split('.');
                    string strFileNm = DateTime.Now.ToString("yyyyMMddHHmmssffff") + (strSuffix.Length >= 2 ? "."+strSuffix[strSuffix.Length - 1] : "");
                    strUrl += "/" + strFileNm;
                    strPath += "\\" + strFileNm;
                    fileItem.SaveAs(strPath);//保存文件

                    #region 存数据库
                    Files filesData = new Files();
                    filesData.FileType = fileItem.ContentType;
                    filesData.Length = fileItem.ContentLength;
                    filesData.Path = strPath;
                    filesData.Name = strFileNm;
                    filesData.TrueName = strFileName;
                    filesData.Remark = strFileNm+"|"+strFileName;
                    filesData.UploadTime = DateTime.Now;
                    filesData.Url = strUrl;
                    filesData.UserId = (long)AbpSession.UserId;
                    FilesId = _filesRepository.InsertAndGetId(filesData);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                //SendData(0);
            }
            SendData(FilesId);
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="objData"></param>
        private void SendData(object objData)
        {
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Write(objData);
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Flush();
        }
        #endregion

    }
}
