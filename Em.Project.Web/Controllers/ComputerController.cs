using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Easyman.Common;
using Easyman.Common.Mvc;
using Easyman.Dto;
using Easyman.Service;
using Easyman.Sys;
using Easyman.Users;
using EasyMan;
using EasyMan.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using Easyman.Domain;
using EasyMan.Dtos;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using Em.Project.Common.Helper;
using System.Collections;
using System.Data;

namespace Easyman.Web.Controllers
{
    public class ComputerController : EasyManController
    {
        #region 初始化

        private readonly IComputerTypeAppService _ComputerTypeAppService;
        private readonly IComputerAppService _ComputerAppService;
        private readonly IFolderAppService _FolderAppService;
        private readonly IDistrictAppService _DistrictAppService;
        private readonly IFileFormatAppService _FileFormatAppService;
        private readonly IMonitFileAppService _MonitFileAppService;
        private readonly IFolderVersionAppService _FolderVersionAppService;
        private readonly ICaseVersionAppService _CaseVersionAppService;
        private readonly IFileLibraryAppService _FileLibraryAppService;
        private readonly IAttrAppService _AttrAppService;
        private readonly IFileAttrAppService _FileAttrAppService;
      
        public ComputerController(IComputerTypeAppService ComputerTypeAppService,
                                    IComputerAppService ComputerAppService,
                                    IFolderAppService FolderAppService,
                                    IDistrictAppService DistrictAppService,
                                    IFileFormatAppService FileFormatAppService,
                                    IMonitFileAppService MonitFileAppService,
                                    IFolderVersionAppService FolderVersionAppService,
                                    ICaseVersionAppService CaseVersionAppService,
                                    IFileLibraryAppService FileLibraryAppService,
                                    IAttrAppService AttrAppService,
                                    IFileAttrAppService FileAttrAppService)
        {

            _ComputerTypeAppService = ComputerTypeAppService;
            _ComputerAppService = ComputerAppService;
            _FolderAppService = FolderAppService;
            _DistrictAppService = DistrictAppService;
            _FileFormatAppService = FileFormatAppService;
            _MonitFileAppService = MonitFileAppService;
            _FolderVersionAppService = FolderVersionAppService;
            _CaseVersionAppService =CaseVersionAppService;
            _FileLibraryAppService = FileLibraryAppService;
            _AttrAppService = AttrAppService;
            _FileAttrAppService = FileAttrAppService;
        }

        #endregion


        #region 终端类型
        public ActionResult EditComputerType(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new ComputerTypeModel());
            }
            var entObj = _ComputerTypeAppService.GetComputerType(id.Value);
            return View(entObj);
        }
        #endregion

        #region 终端管理
        public ActionResult EditComputer(long? id)
        {
            var entObj = new ComputerModel { IsUse = true };

            if (id != null && id != 0)
            {
                entObj = _ComputerAppService.GetComputer(id.Value);
            }
            entObj.ComputerTypeList = _ComputerTypeAppService.ComputerTypeList();
            return View(entObj);
        }
        #endregion

        #region 终端管理
        public ActionResult EditFolder(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new ComputerModel());
            }
            var entObj = _FolderAppService.GetFolder(id.Value);
            return View(entObj);
        }
        #endregion

        public  List<MonitFileTemp> waitFiles = new List<MonitFileTemp>();
        public  List<MonitFileModel> monitFileModels = new List<MonitFileModel>();
        public string masterPath = ConfigurationManager.AppSettings["MasterPath"];
        #region 开始监听
        /// <summary>
        /// 监听入口
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="folderName"></param>
        public string MonitorStart(string ip, string folderName, long scriptNodeCaseId)
        {
            string tip="" ;
            MonitLogModel monitLog = new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("开启对{0}的{1}监控", ip, folderName), LogTime = DateTime.Now };
            _MonitFileAppService.Log(monitLog);
            _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("服务器IP({0})检测...", ip), LogTime = DateTime.Now });
            ComputerModel computer = _ComputerAppService.GetComputerByIp(ip);

            FolderModel folder = new FolderModel();

            if (computer != null)
            {
                _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("{0}服务器文件夹路径检测...", ip), LogTime = DateTime.Now });
                this.CheckDir(masterPath + ip);
                folder = _FolderAppService.GetFolderByComputerAndName(computer.Id, folderName);
                if (folder == null)
                {
                  
                    this.CheckDir(masterPath + ip + "\\" + folderName);
                    folder = new FolderModel();
                    folder.Name = folderName;
                    folder.ComputerId = computer.Id;
                    folder.IsUse = true;
                    folder = _FolderAppService.InsertOrUpdateFolder(folder);
                    _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("IP({0})的共享目录保存成功{1}", ip, folder.Id), LogTime = DateTime.Now });
                }
            }
            else
            {
            
                MonitLogModel monitLogErr = new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控异常:此IP({0})的终端在数据库中不存在", ip), LogTime = DateTime.Now };
                _MonitFileAppService.Log(monitLogErr);
                return string.Format("结果:false;此IP({0})的终端在数据库中不存在", ip);
            }


            string userName = computer.UserName.Trim();//lcz2016
            string pwd = GetDecryptPwd(computer.Pwd.Trim());//lcz201314

            string outMsg= "";
            // 通过IP 用户名 密码 访问远程目录  不需要权限
            using (SharedTool tool = new SharedTool(userName, pwd, ip))
            {
                try
                {
                    string selectPath = string.Format(@"\\{0}\{1}", ip, folderName);
                    var dicInfo = new DirectoryInfo(selectPath);//选择的目录信息 
                    _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:获取当前服务器的最新目录..."), LogTime = DateTime.Now });

                  
                    RecycleDir(dicInfo, computer, folder, null,ref tip);
                    _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:成功遍历{0}下{1}目录;总个数:{2}",ip, folderName,waitFiles.Count.ToString()), LogTime = DateTime.Now });
                  
                    //批量保存文件信息
                    outMsg=SaveFileInfo(waitFiles,folder.Id,computer.Id,scriptNodeCaseId);

                    //string str = "监控文件夹无文件变化";
                    //if (waitFiles != null && waitFiles.Count > 0)
                    //{
                    //    var files = from f in waitFiles
                    //                where f.IsChange == true
                    //                select f;
                    //    var plusFiles = monitFileModels.Where(a => !waitFiles.Exists(t => a.ClientPath == t.ClientPath));

                    //    if ((files != null && files.Count() > 0) || (plusFiles != null && plusFiles.Count() > 0))
                    //    {
                    //        if (files != null && files.Count() > 0)
                    //            str = "监控文件夹存在文件变动的操作";
                    //        FolderVersionModel folderVersion = CheckFolderVersion(folder.Id, "add");
                    //        CaseVersionModel caseVersionModel = SaveCaseVersion(folderVersion, scriptNodeCaseId);
                    //        MonitLogModel monitLogErr = new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:此({0})的({1})下生成新版本号", ip, folderName), LogTime = DateTime.Now, CaseVersionId = caseVersionModel.Id };
                    //        _MonitFileAppService.Log(monitLogErr);
                    //        _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:此({0})的({1})开始生产新的目录树", ip, folderName), LogTime = DateTime.Now, CaseVersionId = caseVersionModel.Id });

                    //        if (plusFiles != null && plusFiles.Count() > 0)
                    //        {
                    //            str += "/监控文件夹存在文件删除的操作";
                    //            foreach (MonitFileModel f in plusFiles)
                    //            {
                    //                f.Status = (short)MonitStatus.Delete;
                    //                _MonitFileAppService.InsertOrUpdateMonitFile(f);
                    //                _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:({0})由于当前版本文件变动，上一版本标记为删除...", f.Name), LogTime = DateTime.Now, CaseVersionId = caseVersionModel.Id });

                    //            }
                    //        }

                    //    }
                    //    else
                    //    {
                    //        str = "监控文件夹无文件变动";

                    //    }
                    //}
                    //else
                    //{
                    //    MonitLogModel monitLogErr = new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:此({0})的({1})下暂无可监控的子目录", ip, folderName), LogTime = DateTime.Now };
                    //    _MonitFileAppService.Log(monitLogErr);
                    //}

                    if (tip == "")
                    {
                        return string.Format("结果:true;监控提示:对{0}的{1}监控完成!{2};{3}", ip, folderName, tip,outMsg);
                    }
                    else
                    {
                        return string.Format("结果:warn;监控提示:对{0}的{1}监控完成!但包含预警信息:{2};{3}", ip, folderName, tip,outMsg);
                    }
                }
                catch (Exception ex)
                {
                    MonitLogModel monitLogErr = new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控异常:此{0}的{1}监控发生异常,{2}", ip, folderName, ex.Message), LogTime = DateTime.Now };
                    _MonitFileAppService.Log(monitLogErr);
                    return string.Format("结果:false;监控提示:对{0}的{1}监控发生异常,{2},{3}", ip, folderName, ex.Message.ToString(), tip);
                }
            }
        }

        #region 使用新模式保存文件信息

        private string SaveFileInfo(List<MonitFileTemp> waitFiles,long folderId,long computerId,long scriptNodeCaseId)
        {
            string connString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString.ToString();
            Dictionary<string, object> datas = new Dictionary<string, object>();
            Dictionary<string, object> pros = new Dictionary<string, object>();
            int len = waitFiles.Count;
            string operTime = DateTime.Now.Ticks.ToString();
            //定义数据，存放列数据
            string[] Ids = new string[len];
            string[] parentIds = new string[len];
            long?[] computerIds = new long?[len];
            long?[] folderIds = new long?[len];
            long?[] relyMonitFileIds = new long?[len];
            string[] names = new string[len];
            int[] isDirs = new int[len];
            string[] formatNames = new string[len];
            string[] clientPaths = new string[len];
            string[] serverPaths = new string[len];
            string[] md5s = new string[len];
            int[] IsChanges = new int[len];
            double?[] sizes = new double?[len];
            int[] fileStatus = new int[len];
            int[] copyStatus = new int[len];
            int[] isHides = new int[len];
            long[] folderVersionIds = new long[len];
            long[] caseVersionIds = new long[len];
            string[] ticks = new string[len];
            //属性
            ArrayList IdByPros = new ArrayList();
            ArrayList md5Pros = new ArrayList();
            ArrayList keyPros = new ArrayList();
            ArrayList valuePros = new ArrayList();
            ArrayList ticksPros = new ArrayList();
            for (int i = 0; i < len; i++)
            {
                MonitFileTemp mft = waitFiles.ElementAt(i);
                Ids[i] = mft.Id;
                parentIds[i] = mft.ParentId;
                computerIds[i] = mft.ComputerId;
                folderIds[i] = mft.FolderId;
                names[i] = mft.Name;
                isDirs[i] = mft.IsDir == true ? 1 : 0;
                formatNames[i] = mft.FormatName;
                clientPaths[i] = mft.ClientPath;
                serverPaths[i] = mft.ServerPath;
                md5s[i] = mft.MD5;
                sizes[i] = mft.Size;
                isHides[i] = mft.IsHide == true ? 1 : 0;
                ticks[i] = operTime;
                foreach (var item in mft.Properties)
                {
                    IdByPros.Add(mft.Id);
                    md5Pros.Add(mft.MD5);
                    keyPros.Add(item.Key);
                    valuePros.Add(item.Value);
                    ticksPros.Add(operTime);
                }
            }

            //monit_file文件基础信息对应关系
            datas.Add("ID", Ids);
            datas.Add("PARENT_ID", parentIds);
            datas.Add("COMPUTER_ID", computerIds);
            datas.Add("FOLDER_ID", folderIds);
            datas.Add("FILE_NAME", names);
            datas.Add("IS_DIR", isDirs);
            datas.Add("FORMAT_NAME", formatNames);
            datas.Add("CLIENT_PATH", clientPaths);
            datas.Add("SERVER_PATH", serverPaths);
            datas.Add("MD5", md5s);
            datas.Add("SIZES", sizes);
            datas.Add("IS_HIDE", isHides);
            datas.Add("TICKS", ticks);

            //monit_file文件属性对应关系
            pros.Add("ID", IdByPros.ToArray());
            pros.Add("MD5", md5Pros.ToArray());
            pros.Add("PNAME", keyPros.ToArray());
            pros.Add("PVALUE", valuePros.ToArray());
            pros.Add("TICKS", ticksPros.ToArray());
            //monit_file文件基础信息
            string tableName = "FM_MONIT_FILE_TEMP";
            OracleHelper.BatchInsert(tableName, datas, connString, len);
            _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("{1}的共享目录{0}文件信息monit_file_temp加载成功,批次{2}", folderId.ToString(), computerId.ToString(),operTime), LogTime = DateTime.Now });
            //monit_file文件属性保存
            string tableProName = "FM_MONIT_FILE_TEMP_PRO";
            OracleHelper.BatchInsert(tableProName, pros, connString, IdByPros.Count);
            _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("{1}的共享目录{0}文件属性monit_file_temp_pro加载成功,批次{2}", folderId.ToString(), computerId.ToString(), operTime), LogTime = DateTime.Now });
            //调用存储过程保存文件相关信息
            string outMsg=OracleHelper.ExeProduce(connString, operTime,  folderId,  computerId,  scriptNodeCaseId);
            _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("{1}的共享目录{0},成功调用SaveData方法,批次{2},{3}", folderId.ToString(), computerId.ToString(), operTime, outMsg), LogTime = DateTime.Now });
            return outMsg;
        }
        private void SaveFileInfo(List<MonitFileTemp> waitFiles, FolderVersionModel folderVersion, CaseVersionModel caseVersionModel)
        {
           
            string connString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString.ToString();
            Dictionary<string, object> datas = new Dictionary<string, object>();
            Dictionary<string, object> pros = new Dictionary<string, object>();
            int len = waitFiles.Count;
            //定义数据，存放列数据
            string[] Ids = new string[len];
            string[] parentIds = new string[len];
            long?[] computerIds = new long?[len];
            long?[] folderIds = new long?[len];
            long?[] relyMonitFileIds = new long?[len];
            string[] names = new string[len];
            int[] isDirs = new int[len];
            string[] formatNames = new string[len];
            string[] clientPaths = new string[len];
            string[] serverPaths = new string[len];
            string[] md5s = new string[len];
            int[] IsChanges = new int[len];
            double?[] sizes = new double?[len];
            int[] fileStatus = new int[len];
            int[] copyStatus = new int[len];
            int[] isHides= new int[len];
            long[] folderVersionIds = new long[len];
            long[] caseVersionIds = new long[len];
            //属性
            ArrayList IdByPros = new ArrayList();
            ArrayList md5Pros = new ArrayList();
            ArrayList keyPros = new ArrayList();
            ArrayList valuePros = new ArrayList();
            for (int i=0;i<len;i++)
            {
                MonitFileTemp mft = waitFiles.ElementAt(i);
                Ids[i] = mft.Id;
                parentIds[i] = mft.ParentId;
                computerIds[i] = mft.ComputerId;
                folderIds[i] = mft.FolderId;
                relyMonitFileIds[i]=mft.RelyMonitFileId;
                names[i] =mft.Name;
                isDirs[i] = mft.IsDir==true?1:0;
                formatNames[i] = mft.FormatName;
                clientPaths[i] = mft.ClientPath;
                serverPaths[i] = mft.ServerPath;
                md5s[i] = mft.MD5;
                IsChanges[i] = mft.IsChange==true?1:0;
                sizes[i] = mft.Size;
                fileStatus[i] = (int)mft.FileStatus;
                copyStatus[i] = (int)mft.CopyStatus;
                isHides[i] = mft.IsHide==true?1:0;
                folderVersionIds[i] = folderVersion.Id;
                caseVersionIds[i] = caseVersionModel.Id;

                foreach (var item in mft.Properties)
                {
                    IdByPros.Add(mft.Id);
                    md5Pros.Add(mft.MD5);
                    keyPros.Add(item.Key);
                    valuePros.Add(item.Value);
                }
            }

            //monit_file文件基础信息对应关系
            datas.Add("ID", Ids);
            datas.Add("PARENT_ID", parentIds);
            datas.Add("COMPUTER_ID", computerIds);
            datas.Add("FOLDER_ID", folderIds);
            datas.Add("RELY_MONIT_FILE_ID", relyMonitFileIds);
            datas.Add("FILE_NAME", names);
            datas.Add("IS_DIR", isDirs);
            datas.Add("FORMAT_NAME", formatNames);
            datas.Add("CLIENT_PATH", clientPaths);
            datas.Add("SERVER_PATH", serverPaths);
            datas.Add("MD5", md5s);
            datas.Add("IS_CHANGE", IsChanges);
            datas.Add("SIZES", sizes);
            datas.Add("STATUS", fileStatus);
            datas.Add("COPY_STATUS", copyStatus);
            datas.Add("IS_HIDE", isHides);
            datas.Add("FOLDER_VERSION_ID", folderVersionIds);
            datas.Add("CASE_VERSION_ID", caseVersionIds);
            //monit_file文件属性对应关系
            pros.Add("ID", IdByPros.ToArray());
            pros.Add("MD5", md5Pros.ToArray());
            pros.Add("PNAME", keyPros.ToArray());
            pros.Add("PVALUE", valuePros.ToArray());
            //monit_file文件基础信息
            string tableName = "FM_MONIT_FILE_TEMP";
            OracleHelper.BatchInsert(tableName, datas, connString,len);
          

            //monit_file文件属性保存
            string tableProName = "FM_MONIT_FILE_TEMP_PRO";
            OracleHelper.BatchInsert(tableProName, pros, connString, IdByPros.Count);

            //调用存储过程保存文件相关信息
           // OracleHelper.ExeProduce(connString, folderVersion.Id);
        }

       


        /// <summary>
        /// 创建临时表，文件基础信息
        /// </summary>
        /// <param name="tableName"></param>
        private void CreateTableTemp(string tableName)
        {
            string sqlExist = string.Format(@"select count(*) from user_tables where table_name = '{0}'",tableName.ToUpper());
            DataTable dt=DbHelper.ExecuteGetTable(sqlExist);
            if (dt.Rows[0][0].ToString() == "1")
            {
                DeleteTableTemp(tableName);
            }
            string sqlCreate = string.Format(@"CREATE TABLE {0}
                                                (
                                                  ID                  VARCHAR(100),
                                                  PARENT_ID           VARCHAR(100),
                                                  FOLDER_VERSION_ID   NUMBER(19),
                                                  COMPUTER_ID         NUMBER(19),
                                                  FOLDER_ID           NUMBER(19),
                                                  RELY_MONIT_FILE_ID  NUMBER(19),
                                                  FILE_NAME           NVARCHAR2(100),
                                                  FORMAT_NAME         NVARCHAR2(100),
                                                  CLIENT_PATH         NVARCHAR2(2000),
                                                  SERVER_PATH         NVARCHAR2(2000),
                                                  MD5                 NVARCHAR2(2000),
                                                  SIZES               NUMBER(19),
                                                  STATUS              NUMBER(5),
                                                  CASE_VERSION_ID     NUMBER(19),
                                                  COPY_STATUS         NUMBER(5),
                                                  IS_HIDE             NUMBER(1),
                                                  IS_CHANGE           NUMBER(1),
                                                  IS_DIR              NUMBER(1)
                                                )", tableName);

            DbHelper.ExecuteGetTable(sqlCreate);
        }

        /// <summary>
        /// 创建临时表,属性临时表
        /// </summary>
        /// <param name="tableName"></param>
        private void CreateTableTempPro(string tableName)
        {
            string sqlCreate = string.Format(@"CREATE TABLE {0}
                                                (
                                                  ID                  VARCHAR(100),                                                  
                                                  MD5                 VARCHAR2(2000),
                                                  PNAME                 VARCHAR(100), 
                                                  PVALUE                 VARCHAR(100)
                                                )", tableName);

            DbHelper.ExecuteGetTable(sqlCreate);
        }
        /// <summary>
        /// 删除临时表
        /// </summary>
        /// <param name="tableName"></param>
        private void DeleteTableTemp(string tableName)
        {
            string sqlCreate = string.Format(@"DROP TABLE {0}", tableName);
            DbHelper.ExecuteGetTable(sqlCreate);
        }
        #endregion

        /// <summary>
        /// 获得解密后的密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private string GetDecryptPwd(string pwd)
        {
            string aesPwd = pwd;
            try
            {
                var p = Common.Helper.EncryptHelper.AesDecrpt(pwd);
                aesPwd = p;
            }
            catch
            {
            }
            return aesPwd;
        }

        private void RecursionSaveMonitFile(long? parentId, MonitFileTemp f, FolderVersionModel folderVersion, CaseVersionModel caseVersionModel)
        {
            string step = "0";
            try
            {
                //检查文件类型
                FileFormatModel fileFormat = CheckFileFormat(f.FormatName, f.IsDir);
                step += "0.5";
                // _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:获取({0})文件格式,格式为({1})", f.Name, fileFormat.Name), LogTime = DateTime.Now, CaseVersionId = caseVersionModel.Id });
                //检查保存FM_FILE_LIBRARY
                FileLibraryModel fileLibrary = CheckFileLibrary(f, fileFormat);
                step += "1";
                //保存属性 和对应关系
                CheckAttr(fileLibrary, f);
              //  _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:获取({0})文件属性...", f.Name), LogTime = DateTime.Now, CaseVersionId = caseVersionModel.Id });
                step += "2";
                //保存monitFile
                MonitFileModel pfileModel = null;              
                pfileModel = SaveMonitFile(parentId, f, folderVersion, fileLibrary, caseVersionModel, fileFormat);
              //  _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:保存({0})文件信息...", f.Name), LogTime = DateTime.Now, CaseVersionId = caseVersionModel.Id, MonitFileId = pfileModel.Id });
                step += "3";
                if (fileFormat.IsFolder == true)
                {
                    step += "4";
                    _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:({0})属性为文件夹,开始遍历下属节点...", f.ClientPath), LogTime = DateTime.Now, CaseVersionId = caseVersionModel.Id, MonitFileId = pfileModel.Id });
                    var files = waitFiles.Where(p => p.ParentId == f.Id);
                    foreach (MonitFileTemp p in files)
                    {
                        RecursionSaveMonitFile(pfileModel.Id, p, folderVersion, caseVersionModel);
                    }

                }

            }
            catch (Exception ex)
            {
                _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控异常:{0}文件监控异常;{1},{2},{3}", f.Name, f.ClientPath,ex.Message, step), LogTime = DateTime.Now, CaseVersionId = caseVersionModel.Id });
                throw ex;
            }
        }

        /// <summary>
        /// 判断文件夹是都存在，不存在建立
        /// </summary>
        /// <param name="directory"></param>
        public void CheckDir(string directory)
        {
            //如果不存在就创建file文件夹
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }           
            
        }

        /// <summary>
        /// 获取上一个相同文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public MonitFileModel GetPMonitFile(string filePath)
        {
            MonitFileModel fileModel= monitFileModels.FirstOrDefault(p=>p.ClientPath== filePath);
            if (fileModel != null)
            {
                int index = monitFileModels.FindIndex(m => m == fileModel);
                monitFileModels[index].IsDelete = false;               
            }
            return fileModel;
            //return _MonitFileAppService.GetMonitFileByPath(filePath);
        }
        /// <summary>
        /// 判断文件上一个版本比较
        /// </summary>
        /// <param name="file"></param>
        public bool CheckFile(MonitFileModel monitFile,string MD5,long fLength)
        {
            if (monitFile == null)
            {
                return true;
            }
            else
            {
                FileLibraryModel fileLibraryModel = _FileLibraryAppService.GetFileLibrary((long)monitFile.FileLibraryId);
                if (monitFile.MD5 != MD5.Trim())
                {
                    return true;
                }
                else if ( fileLibraryModel != null && fLength != fileLibraryModel.Size)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }           

        }

        /// <summary>
        /// 递归文件夹获取
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="computer"></param>
        /// <param name="folder"></param>
        /// <param name="pguid"></param>
        public void RecycleDir(DirectoryInfo directory, ComputerModel computer,FolderModel folder,string pguid,ref string mess)
        {
            string filterName=MonitConst.RestoreStr;
            string filterDataBase = MonitConst.DataBaseStr;
            //获取当前目录下的的文件    
            FileInfo[] textFiles = directory.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo temp in textFiles)
            {
                   if (!temp.FullName.Contains(filterName)&& !temp.FullName.Contains(filterDataBase))
                {
                    MonitFileTemp _monitFile = InitMonitFileSec(directory.FullName.ToString() + "\\" + temp, false, temp.Name, temp.FullName, temp.Extension, pguid, computer, folder, temp.Length);
                    _monitFile.Size = temp.Length;
                    if ((temp.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        _monitFile.IsHide = true;
                    }
                    else
                    {
                        _monitFile.IsHide = false;
                    }
                    if (_monitFile.MD5 != "")
                        waitFiles.Add(_monitFile);
                }
                                           
            }

            //获取当前目录下的文件夹
            DirectoryInfo[] dic = directory.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
            foreach (DirectoryInfo temp in dic)
            {
                if (!temp.FullName.Contains(filterName) && !temp.FullName.Contains(filterDataBase))
                {
                    MonitFileTemp _monitFile = InitMonitFileSec(directory.FullName.ToString(), true, temp.Name, temp.FullName, temp.Extension, pguid, computer, folder,0);
                    if ((temp.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        _monitFile.IsHide = true;
                    }
                    else
                    {
                        _monitFile.IsHide = false;
                    }

                    waitFiles.Add(_monitFile);

                    try
                    {
                        this.RecycleDir(temp, computer, folder, _monitFile.Id,ref mess);
                    }
                    catch (Exception ex)
                    {
                        mess += temp.FullName+":"+ex.Message + ";";
                        _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:遍历文件夹{0}异常;{1}" , temp.FullName,ex.Message), LogTime = DateTime.Now });
                    }
                }
            }
        }

        #region 文件相关类实例化

        private void UpdateDirState(MonitFileTemp _monitFile)
        {
            var mt = waitFiles.FirstOrDefault(e => e.Id == _monitFile.ParentId);

            if (mt != null && mt.FileStatus == MonitStatus.UnChanged)
            {

                int index = waitFiles.FindIndex(m => m == mt);
                waitFiles[index].FileStatus = MonitStatus.Modify;
                if (mt.ParentId != null)
                {
                    UpdateDirState(mt);
                }
            }
        }


        /// <summary>
        /// 初始化文件类
        /// </summary>
        /// <param name="isDir"></param>
        /// <param name="fileName"></param>
        /// <param name="fullName"></param>
        /// <param name="fileFormat"></param>
        /// <param name="computer"></param>
        /// <param name="folder"></param>
        /// <param name="pMonitFile"></param>
        /// <returns></returns>
        private MonitFileTemp InitMonitFile(string filePath,bool isDir, string fileName, string fullName, string fileFormat, string pguid, ComputerModel computer, FolderModel folder,long fLength)
        {
           
            MonitFileTemp _monitFile = new MonitFileTemp();
            _monitFile.Id = Guid.NewGuid().ToString();
            _monitFile.ComputerId = computer.Id;
            _monitFile.IsDir = isDir;
            _monitFile.ParentId = pguid;
            _monitFile.Name = fileName;
            _monitFile.ClientPath = fullName;
            _monitFile.FormatName = fileFormat;
            _monitFile.FolderId = folder.Id;
            
            if (isDir)
            {
                _monitFile.CopyStatus = CopyStatus.Success;
                _monitFile.MD5 = fullName;
                _monitFile.ServerPath = masterPath + computer.Ip + "\\" + folder.Name + "\\" + fileName;
                MonitFileModel relyMonitFile = GetPMonitFile(fullName);
                if (relyMonitFile != null)
                    _monitFile.RelyMonitFileId = relyMonitFile.Id;
                _monitFile.IsChange = relyMonitFile != null ? false : true;
                _monitFile.FileStatus = relyMonitFile != null ? MonitStatus.UnChanged : MonitStatus.Add;
                _monitFile.Properties = FileTool.GetDictionaryByDir(fullName);
            }
            else
            {
                try
                {
                    _monitFile.Properties = FileTool.GetProperties(fullName);
                }
                catch (Exception ex)
                {
                    _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:文件属性获取异常," + ex.Message), LogTime = DateTime.Now });
                }
                MonitFileModel relyMonitFile = GetPMonitFile(fullName);
                if(relyMonitFile != null)
                    _monitFile.RelyMonitFileId =  relyMonitFile.Id;
                //获取文件的MD5值，进行5次尝试
                _monitFile.MD5 = GetFileMD5(filePath);              

                if (_monitFile.MD5 != "")
                {
                    _monitFile.ServerPath = masterPath + computer.Ip + "\\" + folder.Name + "\\" + _monitFile.MD5 + fileFormat;
                    _monitFile.IsChange = CheckFile(relyMonitFile, _monitFile.MD5, fLength);
                    if (relyMonitFile == null)
                    {
                        _monitFile.FileStatus = MonitStatus.Add;
                    }
                    else if (_monitFile.IsChange)
                    {
                        _monitFile.FileStatus = MonitStatus.Modify;
                    }
                    else
                    {
                        _monitFile.FileStatus = MonitStatus.UnChanged;
                    }

                    if (_monitFile.IsChange)
                    {
                        var mf = from f in waitFiles
                                 where f.MD5 == _monitFile.MD5
                                 select f;
                        if (mf != null && mf.Count() > 0)
                        {
                            _monitFile.CopyStatus = CopyStatus.Success;
                        }
                        else
                        {
                            _monitFile.CopyStatus = CopyStatus.Wait;
                        }
                        //修改上级文件夹
                        UpdateDirState(_monitFile);
                    }
                    else
                    {
                        _monitFile.CopyStatus = CopyStatus.Success;
                    }                 
                  
                   
                }
               
            }

            return _monitFile;
        }


        /// <summary>
        /// 初始化文件类
        /// </summary>
        /// <param name="isDir"></param>
        /// <param name="fileName"></param>
        /// <param name="fullName"></param>
        /// <param name="fileFormat"></param>
        /// <param name="computer"></param>
        /// <param name="folder"></param>
        /// <param name="pMonitFile"></param>
        /// <returns></returns>
        private MonitFileTemp InitMonitFileSec(string filePath, bool isDir, string fileName, string fullName, string fileFormat, string pguid, ComputerModel computer, FolderModel folder, long fLength)
        {

            MonitFileTemp _monitFile = new MonitFileTemp();
            _monitFile.Id = Guid.NewGuid().ToString();
            _monitFile.ComputerId = computer.Id;
            _monitFile.IsDir = isDir;
            _monitFile.ParentId = pguid;
            _monitFile.Name = fileName;
            _monitFile.ClientPath = fullName;
            _monitFile.FormatName = fileFormat;
            _monitFile.FolderId = folder.Id;

            if (isDir)
            {
                _monitFile.CopyStatus = CopyStatus.Success;//可能会影响后期的.M .D的状态
                _monitFile.MD5 = fullName;
                _monitFile.ServerPath = masterPath + computer.Ip + "\\" + folder.Name + "\\" + fileName;
                _monitFile.Properties = FileTool.GetDictionaryByDir(fullName);
            }
            else
            {
                try
                {
                    _monitFile.Properties = FileTool.GetProperties(fullName);
                }
                catch (Exception ex)
                {
                    _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:文件属性获取异常," + ex.Message), LogTime = DateTime.Now });
                }

             
                //获取文件的MD5值，进行5次尝试 (自我生成的MD5)
                _monitFile.MD5 = GetFileMD5(filePath);
                if (_monitFile.MD5 != "")
                {
                    _monitFile.ServerPath = masterPath + computer.Ip + "\\" + folder.Name + "\\" + _monitFile.MD5 + fileFormat;
                }

            }

            return _monitFile;
        }


        #endregion

        #region
        /// <summary>
        /// 尝试5次获取文件属性
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetFileMD5(string filePath)
        {
            string md5 = "";
            for (int i = 0; i < 5; i++)
            {              
                    try
                    {
                        md5 =FileTool.GetFileMd5(filePath);
                    }
                    catch(Exception ex)
                    {
                        MonitLogModel monitLogInfo = new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:文件获取异常," + ex.Message), LogTime = DateTime.Now };
                        _MonitFileAppService.Log(monitLogInfo);
                    }                 
                    if (md5 != null && md5 != "")
                    {
                        break;
                    }
               
            }
            return md5;
        }
       
        #endregion
        #region 向数据库中保存数据
        /// <summary>
        /// 保存FM_MONIT_FILE(文件夹及文件管理)
        /// </summary>
        /// <param name="monitFile"></param>
        private MonitFileModel SaveMonitFile(long? parentId, MonitFileTemp monitFile, FolderVersionModel folderVersion, FileLibraryModel fileLibrary, CaseVersionModel caseVersionModel, FileFormatModel fileFormat)
        {
            try
            {
                MonitFileModel monitFileModel = new MonitFileModel();

                monitFileModel.ComputerId = monitFile.ComputerId;
                monitFileModel.FolderId = monitFile.FolderId;
                monitFileModel.RelyMonitFileId = monitFile.RelyMonitFileId;
                monitFileModel.Name = monitFile.Name;
                monitFileModel.ParentId = parentId;
                monitFileModel.MD5 = monitFile.MD5;
                monitFileModel.Status = (short)monitFile.FileStatus;
                monitFileModel.ClientPath = monitFile.ClientPath;
                monitFileModel.ServerPath = monitFile.ServerPath;
                monitFileModel.FolderVersionId = folderVersion.Id;
                monitFileModel.FileLibraryId = fileLibrary.Id;
                monitFileModel.CaseVersionId = caseVersionModel.Id;
                monitFileModel.FileFormatId = fileFormat.Id;
                monitFileModel.CopyStatus = (short)monitFile.CopyStatus;
                monitFileModel.IsHide = monitFile.IsHide;
               

                return _MonitFileAppService.InsertOrUpdateMonitFile(monitFileModel);
            }
            catch (Exception ex)
            {
                _MonitFileAppService.Log(new MonitLogModel() { LogType = (short)LogType.MonitLog, LogMsg = string.Format("监控提示:保存({0}),{1}", monitFile.Name,ex.Message), LogTime = DateTime.Now, CaseVersionId = caseVersionModel.Id });
                throw ex;
            }
          
        }

        /// <summary>
        /// 保存任务实例和版本之间的对应关系
        /// </summary>
        /// <param name="folderVersion"></param>
        /// <param name="nodeCaseId"></param>
        private CaseVersionModel SaveCaseVersion(FolderVersionModel folderVersion, long nodeCaseId)
        {
            CaseVersionModel caseVersionModel = new CaseVersionModel();
            caseVersionModel.FolderVersionId = folderVersion.Id;
            caseVersionModel.ScriptNodeCaseId = nodeCaseId;
            caseVersionModel.BeginTime = DateTime.Now;
            return _CaseVersionAppService.InsertOrUpdateCaseVersion(caseVersionModel);
        }

        /// <summary>
        /// 保存文件的属性
        /// </summary>
        /// <param name="fileLibrary"></param>
        /// <param name="path"></param>
        private void CheckAttr(FileLibraryModel fileLibrary, MonitFileTemp f)
        {
            if (f.IsChange)
            {
                Dictionary<string, string> properties = f.Properties;


                string insertSql = string.Format(@"insert all  ");            
                foreach (var k in properties)
                {
                    AttrModel attrModel = _AttrAppService.GetAttrByName(k.Key);
                    if (attrModel == null)
                    {
                        attrModel = new AttrModel();
                        attrModel.Name = k.Key;
                        attrModel = _AttrAppService.InsertOrUpdateAttr(attrModel);
                    }
                    insertSql += string.Format(@" into  FM_FILE_ATTR(File_library_id,attr_id,attr_val,create_time) values  ({0},{1},'{2}',current_timestamp)", fileLibrary.Id.ToString(), attrModel.Id.ToString(), k.Value);
                }
                insertSql += "select 1 from  dual ";
                string deleteSql = string.Format("delete from FM_FILE_ATTR where File_library_id ={0}", fileLibrary.Id.ToString());
                DbHelper.Execute(deleteSql);
                DbHelper.Execute(insertSql);
            }
        }

        /// <summary>
        /// 检查filelibary 并保存信息
        /// </summary>
        /// <param name="monitFile"></param>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        private FileLibraryModel CheckFileLibrary(MonitFileTemp monitFile, FileFormatModel fileFormat)
        {
           
            FileLibraryModel fileLibrary = new FileLibraryModel();
            if (monitFile.IsChange)
            {
                fileLibrary.MD5 = monitFile.MD5;
                fileLibrary.Name = monitFile.ClientPath;
                fileLibrary.FileFormatId = fileFormat.Id;
                fileLibrary.IsCopy = monitFile.IsDir ? true : false;
                fileLibrary.Size = monitFile.Size;
                fileLibrary.IsHide = monitFile.IsHide;
                return _FileLibraryAppService.InsertOrUpdateFileLibrary(fileLibrary);
            }
            else {
                if (monitFile.IsDir)
                {
                    var id = monitFileModels.FirstOrDefault(P => P.ClientPath == monitFile.ClientPath).FileLibraryId;
                    if (id != null)
                        return _FileLibraryAppService.GetFileLibrary((long)id);
                    else
                        return null;
                }
                else
                    return _FileLibraryAppService.GetFileLibraryByMD5(monitFile.ClientPath);
            }
        
        }

        /// <summary>
        /// 判断是否存在此类型，不存在就添加
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private FileFormatModel CheckFileFormat(string formatName,bool isDir)
        {
            FileFormatModel fileFormat;
            if (isDir)
                fileFormat = _FileFormatAppService.GetFileFormatByDir();
            else
                fileFormat = _FileFormatAppService.GetFileFormatByName(formatName);
            if (fileFormat != null)
            {
                return fileFormat;
            }
            else
            {
                fileFormat = new FileFormatModel();

                if (isDir)
                {
                    fileFormat.IsFolder = true;
                    fileFormat.Name = "Folder";
                }
                else
                {
                    fileFormat.IsFolder = false;
                    fileFormat.Name = formatName;
                }
               
                return _FileFormatAppService.InsertOrUpdateFileFormat(fileFormat);
            }
        }
        /// <summary>
        /// 检查是否存在共享文件夹版本，不存在新增
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        private FolderVersionModel CheckFolderVersion(long folderId,string operType)
        {
            FolderVersionModel folderVersion = new FolderVersionModel();
            if (operType == "add")
            {
                folderVersion.FolderId = folderId;
                folderVersion.BeginTime = DateTime.Now;
                return _FolderVersionAppService.InsertOrUpdateFolderVersion(folderVersion);
            }
            else
            {
                folderVersion = _FolderVersionAppService.GetFolderVersionByFolder(folderId);
                if (folderVersion == null)
                {
                    folderVersion.FolderId = folderId;
                    folderVersion.BeginTime = DateTime.Now;
                    return _FolderVersionAppService.InsertOrUpdateFolderVersion(folderVersion);
                }
                else
                {
                    return folderVersion;
                }
            }

        }

        #endregion
        /// <summary>
        /// 向远程文件夹保存本地内容，或者从远程文件夹下载文件到本地
        /// </summary>
        /// <param name="src">要保存的文件的路径，如果保存文件到共享文件夹，这个路径就是本地文件路径如：@"D:\1.avi"</param>
        /// <param name="dst">保存文件的路径，不含名称及扩展名</param>
        /// <param name="fileName">保存文件的名称以及扩展名</param>
        public void Transport(string src, string dst, string fileName)
        {

            FileStream inFileStream = new FileStream(src, FileMode.Open);
            if (!Directory.Exists(dst))
            {
                Directory.CreateDirectory(dst);
            }
            dst = dst + fileName;
            FileStream outFileStream = new FileStream(dst, FileMode.OpenOrCreate);

            byte[] buf = new byte[inFileStream.Length];

            int byteCount;

            while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
            {

                outFileStream.Write(buf, 0, byteCount);

            }

            inFileStream.Flush();

            inFileStream.Close();

            outFileStream.Flush();

            outFileStream.Close();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool IsFolder(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if ((fi.Attributes & FileAttributes.Directory) != 0)
                return true;
            else
            {
                return false;
            }
        }

        #endregion


        #region 数据库操作

        public bool DataBaseCmd(string cmdStr)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe"; //bat文件路径        

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;// 不创建新窗口 
                p.StartInfo.RedirectStandardInput = true;// 重定向输入
                p.StartInfo.RedirectStandardOutput = true;// 重定向标准输出  
                p.Start();
                //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                string str = string.Format(@"{0} {1}", cmdStr, "&exit");

                p.StandardInput.WriteLine(str);
                StreamReader reader = p.StandardOutput;//截取输出流

                string line = reader.ReadLine();//每次读取一行
                StringBuilder logBat = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (line != "")
                    {
                        logBat.AppendLine(line);
                    }

                }

                p.WaitForExit();//启用则以同步方式执行命令
                p.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 网络测试

        public string ConnectTest(string userName,string password,string ip)
        {
            try
            {
                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
        
    }
}
