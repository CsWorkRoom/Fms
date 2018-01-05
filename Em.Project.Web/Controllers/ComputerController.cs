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

        public static List<MonitFileTemp> waitFiles = new List<MonitFileTemp>();
        public static List<MonitFileModel> monitFileModels = new List<MonitFileModel>();
        public string masterPath = "D:\\";
        #region 开始监听
        /// <summary>
        /// 监听入口
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="folderName"></param>
        public void MonitorStart(string ip, string folderName)
        {
            ip = "10.108.226.21";
            folderName = "Coding\\ceshi\\";
         
            ComputerModel computer = _ComputerAppService.GetComputerByIp(ip);
            FolderModel folder;
            if (computer != null)
            {
                this.CheckDir(masterPath+ip);
               
                folder = _FolderAppService.GetFolderByComputerAndName(computer.Id,folderName);
                if (folder == null)
                {
                    this.CheckDir(masterPath + ip+"\\"+ folder.Name);
                    folder.Name = folderName;
                    folder.ComputerId = computer.Id;
                    folder.InUse = 1;
                    _FolderAppService.InsertOrUpdateFolder(folder);
                }               
            }
            else {
                Logger.Info("不存在此IP！");
                return;
            }

            //获取上一个最新版本
            FolderVersionModel folderVersionOld= _FolderVersionAppService.GetFolderVersionByFolder(folder.Id);
            if (folderVersionOld != null)
            {
                monitFileModels = _MonitFileAppService.GetMonitFileByVersion(folderVersionOld.Id);
            }
           
            string userName = "lcz2016";// computer.UserName.Trim();//lcz2016
            string pwd = "lcz201314";// computer.Pwd.Trim();//lcz201314
            // 通过IP 用户名 密码 访问远程目录  不需要权限
            using (SharedTool tool = new SharedTool(userName, pwd, ip))
            {
                string selectPath = @"\\" + ip + "\\" + folderName;
                var dicInfo = new DirectoryInfo(selectPath);//选择的目录信息 
                this.RecycleDir(dicInfo,computer, folder,null);
                if (waitFiles != null && waitFiles.Count > 0)
                {
                    var files = from f in waitFiles
                                where f.IsChange == true
                                select f;
                    long scriptNodeCaseId = 1;
                    if (files != null && files.Count() > 0)
                    {
                        FolderVersionModel folderVersion = CheckFolderVersion(folder.Id, "add");
                        CaseVersionModel caseVersionModel=SaveCaseVersion(folderVersion, scriptNodeCaseId);
                        foreach (MonitFileTemp f in waitFiles)
                        {
                            if (string.IsNullOrEmpty(f.ParentId))
                            {
                                RecursionSaveMonitFile(null, f, folderVersion, caseVersionModel);
                            }
                          
                        }
                        var deleteFileModels = monitFileModels.Where(p => p.IsDelete !=false);
                        foreach (MonitFileModel f in deleteFileModels)
                        {
                            f.Status = (short)MonitStatus.Delete;
                            _MonitFileAppService.InsertOrUpdateMonitFile(f);
                        }
                    }
                    else
                    {
                        FolderVersionModel folderVersion = CheckFolderVersion(folder.Id,"get");
                        SaveCaseVersion(folderVersion, scriptNodeCaseId);
                    }
                }
            }

        }

        private void RecursionSaveMonitFile(long? parentId, MonitFileTemp f, FolderVersionModel folderVersion, CaseVersionModel caseVersionModel)
        {
            //检查文件类型
            FileFormatModel fileFormat = CheckFileFormat(f.FormatName, f.IsDir);
            //检查保存FM_FILE_LIBRARY
            FileLibraryModel fileLibrary = CheckFileLibrary(f, fileFormat);
            //保存属性 和对应关系
            CheckAttr(fileLibrary, f.ClientPath);
            //保存monitFile
            MonitFileModel pfileModel= SaveMonitFile(parentId,f, folderVersion, fileLibrary, caseVersionModel);
            if (fileFormat.IsFolder == true)
            {
                 var files= waitFiles.Where(p => p.ParentId == f.Id);
                foreach (MonitFileTemp p in files)
                {
                    RecursionSaveMonitFile(pfileModel.Id, p, folderVersion, caseVersionModel);
                }
                 
            }
        }

        /// <summary>
        /// 判断文件夹是都存在，不存在建立
        /// </summary>
        /// <param name="directory"></param>
        public void CheckDir(string directory)
        {
            //如果不存在就创建file文件夹
            if (Directory.Exists(Server.MapPath(directory)) == false)
            {
                Directory.CreateDirectory(Server.MapPath(directory));
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
        public bool CheckFile(MonitFileModel monitFile, string md5)
        {

            if (monitFile==null||monitFile.MD5 != md5.Trim())
            {
                return true;
            }
            else
            {
                return false;
            }  
     

        }

        /// <summary>
        /// 递归文件夹获取
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="computer"></param>
        /// <param name="folder"></param>
        /// <param name="pguid"></param>
        public void RecycleDir(DirectoryInfo directory, ComputerModel computer,FolderModel folder,string pguid)
        {
            //获取当前目录下的的文件    
            FileInfo[] textFiles = directory.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo temp in textFiles)
            {
                MonitFileTemp _monitFile = InitMonitFile( false, temp.Name,  temp.FullName,  temp.Extension, pguid, computer,  folder);
                waitFiles.Add(_monitFile);                              
            }
            //获取当前目录下的文件夹
            DirectoryInfo[] dic = directory.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
            foreach (DirectoryInfo temp in dic)
            {
                MonitFileTemp _monitFile = InitMonitFile(true, temp.Name, temp.FullName, temp.Extension, pguid, computer, folder);
                waitFiles.Add(_monitFile);
                CheckDir(temp.FullName);
                this.RecycleDir(temp, computer,  folder, _monitFile.Id);
            }
        }

        #region 文件相关类实例化
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
        private MonitFileTemp InitMonitFile(bool isDir, string fileName, string fullName, string fileFormat, string pguid, ComputerModel computer, FolderModel folder)
        {
            MonitFileTemp _monitFile = new MonitFileTemp();
            _monitFile.Id = Guid.NewGuid().ToString();
            _monitFile.ComputerId = computer.Id;
            _monitFile.IsDir = isDir;
            _monitFile.ParentId = pguid;
            _monitFile.Name = fileName;
            _monitFile.ClientPath = fullName;
            _monitFile.FormatName = fileFormat;
            _monitFile.ServerPath = computer.Ip + "\\" + folder.Name;
            if (isDir)
            {
                MonitFileModel relyMonitFile = GetPMonitFile(fullName);
                _monitFile.IsChange = relyMonitFile != null ? false : true;
                _monitFile.FileStatus = relyMonitFile != null ? MonitStatus.UnChanged : MonitStatus.Add;
                _monitFile.Properties = FileTool.GetDictionaryByDir(fullName);
            }
            else
            {
                MonitFileModel relyMonitFile = GetPMonitFile(fullName);
                _monitFile.RelyMonitFileId = relyMonitFile != null ? relyMonitFile.Id : 0;
                _monitFile.MD5 = FileTool.GetFileHash(fullName);
                _monitFile.IsChange = CheckFile(relyMonitFile, _monitFile.MD5);
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

                _monitFile.Properties = FileTool.GetProperties(fullName);
            }

            return _monitFile;
        }



        #endregion
        #region 向数据库中保存数据
        /// <summary>
        /// 保存FM_MONIT_FILE(文件夹及文件管理)
        /// </summary>
        /// <param name="monitFile"></param>
        private MonitFileModel SaveMonitFile(long? parentId, MonitFileTemp monitFile, FolderVersionModel folderVersion, FileLibraryModel fileLibrary, CaseVersionModel caseVersionModel)
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
            monitFileModel.CopyStatus = monitFile.IsChange ? (short)1 : (short)0;
            return _MonitFileAppService.InsertOrUpdateMonitFile(monitFileModel);
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
        private void CheckAttr(FileLibraryModel fileLibrary, string path)
        {
         Dictionary<string,string>  properties=  FileTool.GetProperties(path);
            foreach (var k in properties)
            {
                AttrModel attrModel = _AttrAppService.GetAttrByName(k.Key);
                if (attrModel == null)
                {
                    attrModel = new AttrModel();
                    attrModel.Name = k.Key;
                    attrModel=_AttrAppService.InsertOrUpdateAttr(attrModel);
                }
                FileAttrModel fileAttr = _FileAttrAppService.GetFileAttrByFileAndAttr(fileLibrary.Id,attrModel.Id);
                if (fileAttr == null)
                {
                    fileAttr = new FileAttrModel();
                    fileAttr.FileLibraryId = fileLibrary.Id;
                    fileAttr.AttrId = attrModel.Id;                    
                }
                fileAttr.AttrValue = k.Value;
                _FileAttrAppService.InsertOrUpdateFileAttr(fileAttr);

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
                fileLibrary.Name = monitFile.MD5;
                fileLibrary.FileFormatId = fileFormat.Id;
                fileLibrary.Size = 100;
                return _FileLibraryAppService.InsertOrUpdateFileLibrary(fileLibrary);
            }
            else {
                return _FileLibraryAppService.GetFileLibraryByMD5(monitFile.MD5);
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
            if (fileFormat == null)
            {
                return fileFormat;
            }
            else
            {
                if (isDir)
                {
                    fileFormat.IsFolder = true;
                    fileFormat.Name = formatName;
                }
                else
                {
                    fileFormat.IsFolder = false;
                    fileFormat.Name = "folder";
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
  

    }
}
