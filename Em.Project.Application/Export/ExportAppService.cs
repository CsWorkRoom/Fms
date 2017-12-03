using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Common.Helper;
using Easyman.Domain;
using Easyman.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Easyman.Service
{
    /// <summary>
    /// 数据导出管理
    /// </summary>
    public class ExportAppService : EasymanAppServiceBase, IExportAppService
    {
        #region 初始化
        /// <summary>
        /// 生成导出数据
        /// </summary>
        private readonly IRepository<ExportData, long> _exportDataRepository;
        private readonly IRepository<Files, long> _filesRepository;
        private readonly IRepository<DownData, long> _downDataRepository;
        private readonly IRepository<ExportConfig, long> _exportConfigRepository;
       private readonly IDbServerAppService _dbServerAppService;
        

        private static int intPagIndex = 0;//计算总页数
        private static System.Timers.Timer t=null;//准备一个定时器，定时查看是线程是否全部都执行完成
        private static string strPathAll = "";//所有导出的文件虚拟文件集合
        private static string strMapPathAll = "";//物理路径集合
        private static DateTime dtmEndTime = new DateTime ();//线程等待最大时间长
        private static long lngDbServerId = 0;//数据库ID
        List<Thread> ThreadList = new List<Thread>();//记录线程信息
        List<Thread> TempThreadList = new List<Thread>();//临时记录线程信息

        /// <summary>
        /// 构造函数（注入仓储）
        /// </summary>
        /// <param name="globalVarRepository"></param>
        public ExportAppService(IRepository<ExportData, long> exportDataRepository,
            IRepository<Files, long> filesRepository,
            IRepository<DownData, long> downDataRepository,
            IRepository<ExportConfig, long> exportConfigRepository,
            IDbServerAppService dbServerAppService
            )
        {
            _exportDataRepository = exportDataRepository;
            _filesRepository = filesRepository;
            _downDataRepository = downDataRepository;
            _exportConfigRepository = exportConfigRepository;
           _dbServerAppService = dbServerAppService;
        }
        #endregion

        #region 公共接口
        #region 在线导出
        public string OnlineExportData(ExportDataModel exp)
        {
            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            err.IsError = false;
            try
            {
                //非压缩导出
                string strFileName = exp.FileName;
                lngDbServerId = (long)exp.DbServerId;
                strMapPathAll = "";
                strPathAll = "";
                string strPath = exp.FilePath;
                Easyman.Common.Fun.GetPath(ref strFileName, exp.FileFormat, ref strMapPathAll, ref strPath);//重新整理保存路径
                DataTable dt = _dbServerAppService.ExecuteGetTable((int)exp.DbServerId, exp.Sql); //要导出的数据集
                string ColumnHeader = "";
                switch (exp.FileFormat.ToLower())
                {
                    case ".xlsx":
                        ColumnHeader = GetHeadStr(exp.TopFields);//将表头对像转成字符串
                        OutExcelHelper.ExportExcel(dt, strFileName, strMapPathAll, ColumnHeader, true);
                        break;
                    case ".csv":
                        OutExcelHelper.ExportCsv(dt, strFileName, strMapPathAll, ColumnHeader, true);
                        break;
                    case ".txt":
                        OutExcelHelper.ExportTxt(dt, strFileName, strMapPathAll, ColumnHeader, true);
                        break;
                    default:
                        ColumnHeader = GetHeadStr(exp.TopFields);//将表头对像转成字符串
                        OutExcelHelper.ExportExcel(dt, strFileName, strMapPathAll, ColumnHeader, true);
                        break;
                }
                exp.FileName += exp.FileFormat;
                SavaDBSql(strPath, strMapPathAll, exp);
                return strPath;
            }
            catch (Exception ex)
            {
                err.IsError = true;
                err.Message = ex.Message;
                throw;
            }
        }
        #endregion

        #region 离线导出
        /// <summary>
        /// 离线导出
        /// </summary>
        /// <param name="exp">数据集</param>
        /// <param name="intCountNum">数据总量</param>
        public string OfflineExportData(ExportDataModel exp, int intCountNum)
        {
            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            err.IsError = false;
            var config= GetExportConfig("report");
            var intTheadCount = 1000;//线程最大值
            if (ThreadList.Count >= intTheadCount)
            {
                return "导出时，由于当前运行的线程数已达到了最大值，请稍后再尝试离线下载，或请联系管理给于帮助！";
            }

            int intRowNum = (int)config.MaxRowNum;//得到配置的每个文档最大行数
            dtmEndTime = DateTime.Now.AddSeconds((int)config.MaxTime);//得到配置的最大导出时长（秒）
            intPagIndex = intCountNum / intRowNum; //计算总页数
            string strFileName = exp.FileName;
            lngDbServerId = (long)exp.DbServerId;
            strMapPathAll = "";
            strPathAll = "";
            exp.UserId = GetCurrentUserAsync().Result.Id;

            if (intCountNum % intRowNum != 0)
            {
                intPagIndex++;
            }
            #region 转换表头
            if (exp.FileFormat.ToLower() == ".xlsx")
                exp.ColumnHeader = GetHeadStr(exp.TopFields);//将表头对像转成字符串
            else
                exp.ColumnHeader = "";
            string strFilePath = exp.FilePath;//保存原始数据
            #endregion

            object obj = new object();
            if (intPagIndex > 1)//如果大于1页，就启用分文件再压缩的方式离线导出文件
            {
              string strTempFileFormat = exp.FileFormat;//暂存文件格式
                exp.FileFormat = ".zip";//压缩文件格式
                SavaDBSql(null, null, exp);//添加初始值
                exp.FileFormat = strTempFileFormat;//还原文件格式
                TempThreadList = new List<Thread>();//记录线程信息
                for (int i = 1; i <= intPagIndex; i++)
                {
                    string strTempFileName = strFileName + "_" + i +exp.FileFormat;
                    exp.FileName = strTempFileName;
                    DataTable dt = _dbServerAppService.ExecuteGetTable((int)exp.DbServerId, exp.Sql, i, intRowNum); //要导出的数据集
                    dt.Columns.Remove("N");//删除分页引起的多一列数据
                    exp.ObjParam = dt;
                    string strPath = strFilePath;//虚拟地址
                    string strMapPath = "";//物理地址
                    Easyman.Common.Fun.GetPath(ref strTempFileName, exp.FileFormat, ref strMapPath, ref strPath);//重新整理保存路径
                    exp.FilePath = strMapPath;
                    strPathAll += (strPathAll == "" ? "" : "|") + strPath;
                    strMapPathAll += (strMapPathAll == "" ? "" : "|") + strMapPath;
                    
                    Thread thread = new Thread(new ParameterizedThreadStart(OutExportFile));//启用多线程处理数据
                    thread.IsBackground = true;
                    thread.Name = strTempFileName.Replace(exp.FileFormat, "");
                    obj = new object();
                    obj = exp;
                    thread.Start(obj);
                    ThreadList.Add(thread);//添加到总记录里面
                    TempThreadList.Add(thread);//添加到临时记录
                }
                ExportDataModel ExportDataM = new ExportDataModel();
                ExportDataM = exp;

                t = new System.Timers.Timer(10000);//实例化Timer类，设置间隔时间为10000毫秒；
                t.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => OutTime(s, e, strFilePath, strFileName,(ExportDataModel)obj, TempThreadList));
                t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；        
                t.Start();
            }
            else
            {
                SavaDBSql(null, null, exp);//添加初始值
                //非压缩导出
                TempThreadList = new List<Thread>();//记录线程信息
                string strPath = exp.FilePath;
                Easyman.Common.Fun.GetPath(ref strFileName, exp.FileFormat, ref strMapPathAll, ref strPath);//重新整理保存路径
                exp.FileName = strFileName;
                exp.FilePath = strMapPathAll;
                exp.ObjParam = _dbServerAppService.ExecuteGetTable((int)exp.DbServerId, exp.Sql); //要导出的数据集
                Thread thread = new Thread(new ParameterizedThreadStart(OutExportFile));//启用多线程处理数据
                thread.IsBackground = true;
                thread.Name = strFileName.Replace(exp.FileFormat, "");
                obj = new object();
                obj = exp;
                thread.Start(obj);
                ThreadList.Add(thread);//添加到总记录里面
                TempThreadList.Add(thread);//添加到临时记录

                ExportDataModel ExportDataM = new  ExportDataModel();
                ExportDataM = exp;

                t = new System.Timers.Timer(10000);//实例化Timer类，设置间隔时间为10000毫秒；
                t.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => OutTime(s, e, strPath, strFileName, ExportDataM, TempThreadList));
                t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；        
                t.Start();
            }
            return "导出命令已发出";
        }
        #endregion

        #region 得到转换表头
        /// <summary>
        /// 得到转换表头
        /// </summary>
        /// <param name="TopFields">表头</param>
        /// <returns></returns>
        private string GetHeadStr(object TopFields)
        {
            string strColumnHeader = "";
            string strList = "";
            if (TopFields.ToString() != "")
            {
                foreach (ExcelTop item in (ExcelTop[])TopFields)
                {
                    strList = "";
                    foreach (int itemList in item.list)
                    {
                        strList += (strList == "" ? "" : ",") + itemList;
                    }
                    strColumnHeader += (strColumnHeader != "" ? "," : "");
                    strColumnHeader += "{name:'" + item.name + "',list:[" + strList + "]}";
                }
                strColumnHeader = "[" + strColumnHeader + "]";
            }
            return strColumnHeader;
        }
        #endregion

        #region 多线程执行体
        /// <summary>
        /// 多线程执行体
        /// </summary>
        /// <param name="exp"></param>
        private static void OutExportFile(object exp)
        {
            ExportDataModel ExportDataM = (ExportDataModel)exp;
            DataTable dt = (DataTable)ExportDataM.ObjParam;
            switch (ExportDataM.FileFormat.ToLower())
            {
                case ".xlsx":
                    OutExcelHelper.ExportExcel(dt, ExportDataM.FileName, ExportDataM.FilePath, ExportDataM.ColumnHeader, false);
                    break;
                case ".csv":
                    OutExcelHelper.ExportCsv(dt, ExportDataM.FileName, ExportDataM.FilePath, ExportDataM.ColumnHeader, false);
                    break;
                case ".txt":
                    OutExcelHelper.ExportTxt(dt, ExportDataM.FileName, ExportDataM.FilePath, ExportDataM.ColumnHeader, false);
                    break;
                default:
                    OutExcelHelper.ExportExcel(dt, ExportDataM.FileName, ExportDataM.FilePath, ExportDataM.ColumnHeader, false);
                    break;
            }

        }
        #endregion

        #region 定时器执行内容
        /// <summary>
        /// 检测线程是否全部执行完成
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OutTime(object source, System.Timers.ElapsedEventArgs e, string strPath,string strFileName, ExportDataModel exp, List<Thread> LstThread)
        {
            #region 检测线程是否全部执行完成
            List<Thread> outThreadList = LstThread;
            List<Thread> TempOutThreadList = LstThread;
            #region 清除已停止的线程     
            for (int i = 0; i < TempOutThreadList.Count; i++)
            {
                Thread item = TempOutThreadList[i];
                if (item.IsAlive == false)
                {
                    outThreadList.Remove(item);
                    ThreadList.Remove(item);
                }
            }
            #endregion

            #region 到时停止现有运行的线程
            if (DateTime.Now >= dtmEndTime)
            {
                TempOutThreadList = outThreadList;
                for (int i = 0; i < TempOutThreadList.Count; i++)
                {
                    Thread outItem = TempOutThreadList[i];
                    if (outItem.IsAlive == false)
                    {
                        outItem.Abort();//停止线程
                        outThreadList.Remove(outItem);
                    }
                }
            }
            #endregion

            #endregion

            if (ThreadList.Count == 0)
            {
                t.Stop();//停止循环检测
                string strMapPath = "";
                strMapPath = strMapPathAll.Split('|')[0];
                string[] panth = strMapPath.Split('\\');
                strMapPath = strMapPath.Replace(panth[panth.Length-1], "");
                strMapPath = strMapPath + strFileName;
                exp.FilePath = strMapPath;
                if (1 < intPagIndex)
                 {
                    ZipHelper zip = new ZipHelper();
                    strMapPath += ".zip";
                    exp.FileFormat = ".zip";
                    exp.FileName = strFileName+".zip";
                    strPath += "/" + DateTime.Now.Month + "/" + strFileName + ".zip";
                    exp.FilePath += ".zip";
                    zip.ZipFiles(strMapPathAll, strMapPath);
                }
                SavaDBSql(strPath, strMapPath, exp);
            }
        }
        #endregion

        #region 将生成的数据保存数据库
        /// <summary>
        /// 将生成的数据保存数据库
        /// </summary>
        /// <param name="strPath">保存后的虚拟路径</param>
        /// <param name="strMapPath">保存后的物理路径</param>
        public void SavaDBSql(string strPath, string strMapPath, ExportDataModel exp)
        {
            long lngUserId = (long)(exp.ExportWay == "在线" ? GetCurrentUserAsync().Result.Id : exp.UserId);

            #region 更新文件表
            //文件管理列表
            Easyman.Domain.Files files = new Easyman.Domain.Files();
            files.FileType = exp.ExportWay;
            files.Name = exp.DisplayName;
            files.TrueName = exp.FileName;
            files.UploadTime = DateTime.Now;
            files.UserId = lngUserId;

            if (exp.ExportWay == "在线" )
            {
                files.Id = 0;
            }
            else
            {
                files.Id = exp.FilesId ==null ? 0 : (int)exp.FilesId;
            }
            if(strPath != null && strMapPath != null && strPath != "" && strMapPath != "")
            {
                strPath = strPath.Replace("\\", "/");
                System.IO.FileInfo FileObj = new FileInfo(strMapPath);               
                files.Length = FileObj.Length;//文件大小 
                files.Path = strPath;
                files.Url = strPath;

                exp.FileSize = (int)FileObj.Length;
                exp.EndTime = DateTime.Now;
                exp.FilePath = strPath;
                exp.Status = "生成成功";
            }
            exp.FilesId = _filesRepository.InsertOrUpdateAndGetId(files); //更新文件管理列表
            if (exp.FilesId <= 0)
            {
                lngUserId = 0;
                EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
                err.IsError = false;
                err.Message = "导出文件后，保存至文件管理数据库出错";
                return;
            }
            #endregion


            exp.FilesId = exp.FilesId;
            exp.UserId = lngUserId;
            if (exp.ExportWay == "在线")
            {
                exp.BeginTime = DateTime.Now;
                exp.Id = 0;
            }
            
            if (strPath == null || strMapPath == null)
            {
                ///向导出数据生成记录里面插入数据
                exp.BeginTime = DateTime.Now;
                exp.FileSize = 0;
                exp.Status = "生成中";
            }

            var ent = AutoMapper.Mapper.Map<ExportData>(exp);
            exp.Id = _exportDataRepository.InsertOrUpdateAndGetId(ent); //更新导出数据生成记录

            if (exp.Id <= 0)
            {
                EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
                err.IsError = false;
                err.Message = "导出文件后，保存至生成记录数据库出错";
                return;
            }

            if (exp.ExportWay == "在线")
            {
                DateTime dtmCreateTime = DateTime.Now;
                //数据下载记录表
                DownData downData = new DownData();
                downData.DisplayName = exp.DisplayName;
                downData.DownBeginTime = dtmCreateTime;
                downData.DownEndTime = dtmCreateTime;
                downData.ExportDataId = exp.Id;
                downData.FileName = exp.FileName;
                downData.FilePath = strPath;
                downData.FileSize = exp.FileSize;
                downData.Status = "成功";
                downData.UserId = lngUserId;
                if (_downDataRepository.InsertOrUpdateAndGetId(downData) <= 0)
                {
                    EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
                    err.IsError = false;
                    err.Message = "导出文件后，保存至下载记录表数据库出错";
                    return;
                }
            }
        }
        #endregion

        #region 下载记录
        public DownData DownLoadRecord(int intFileId)
        {
            ExportData expData = _exportDataRepository.Get(intFileId);
            var user = GetCurrentUserAsync().Result;//当前登录者
            if (expData ==null || expData.Id<=0)
            {
                EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
                err.IsError = false;
                err.Message = "导出文件后，保存至下载记录表数据库出错";
                return null;
            }
            //数据下载记录表
            DownData downData = new DownData();
            downData.DisplayName = expData.DisplayName;
            downData.DownBeginTime = DateTime.Now;
            downData.DownEndTime = DateTime.Now;
            downData.ExportDataId = intFileId;
            downData.FileName = expData.FileName;
            downData.FilePath = expData.FilePath;
            downData.FileSize = expData.FileSize;
            downData.Status = "成功";
            downData.UserId = user.Id;
            long lngResult = _downDataRepository.InsertOrUpdateAndGetId(downData);
            if (lngResult <= 0)
            {
                EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
                err.IsError = false;
                err.Message = "导出文件后，保存至下载记录表数据库出错";
                return null;
            }
            return downData;
        }
        #endregion
        
        public ExportConfig GetExportConfig(int id)
        {
            return _exportConfigRepository.Get(id);
        }
        public ExportConfig GetExportConfig(string app)
        {
            return _exportConfigRepository.FirstOrDefault(p => p.App == app);
        }
        #endregion
    }
}
