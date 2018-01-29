using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.IO;
using System.Data;
using Easyman.Common.Helper;
using System.Web;

namespace Easyman.Service
{
    /// <summary>
    /// 文件夹及文件管理
    /// </summary>
    public class MonitFileAppService : EasymanAppServiceBase, IMonitFileAppService
    {
        #region 初始化

        private readonly IRepository<MonitFile,long> _MonitFileCase;
        private readonly IRepository<MonitLog, long> _MonitLogCase;
        private readonly IRepository<FileLibrary, long> _FileLibraryCase;
        private readonly IRepository<MonitLogVersion, long> _MonitLogVersionCase;

        /// <summary>
        /// 构造函数注入MonitFile仓储
        /// </summary>
        /// <param name="MonitFileCase"></param>
        /// <param name="MonitLogCase"></param>
        /// <param name="FileLibraryCase"></param>
        /// <param name="MonitLogVersionCase"></param>
        public MonitFileAppService(IRepository<MonitFile, long> MonitFileCase,
            IRepository<MonitLog, long> MonitLogCase,
            IRepository<FileLibrary, long> FileLibraryCase,
            IRepository<MonitLogVersion, long> MonitLogVersionCase)
        {
            _MonitFileCase = MonitFileCase;
            _MonitLogCase = MonitLogCase;
            _FileLibraryCase = FileLibraryCase;
            _MonitLogVersionCase = MonitLogVersionCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个文件库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MonitFileModel GetMonitFile(long id)
        {
            var entObj= _MonitFileCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<MonitFileModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增文件库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MonitFileModel InsertOrUpdateMonitFile(MonitFileModel input)
        {
            try
            {               
                //var entObj =input.MapTo<MonitFile>();
                var entObj = _MonitFileCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new MonitFile();
                entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
                //var entObj= AutoMapper.Mapper.Map<MonitFile>(input);
                var id = _MonitFileCase.InsertOrUpdateAndGetId(entObj);

                return entObj.MapTo<MonitFileModel>();

            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        /// <summary>
        /// 删除一条文件库
        /// </summary>
        /// <param name="input"></param>
        public void DeleteMonitFile(EntityDto<long> input)
        {
            try
            {
                _MonitFileCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取文件库json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetMonitFileTreeJson()
        {
            var objList= _MonitFileCase.GetAllList();
            if(objList!=null&& objList.Count>0)
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
        public List<SelectListItem> MonitFileList()
        {
            var objList = _MonitFileCase.GetAllList();
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
        /// 根据文件目录获取文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public MonitFileModel GetMonitFileByPath(string path)
        {

            if (!string.IsNullOrEmpty(path))
            {
                var MonitFile = _MonitFileCase.GetAllList().OrderByDescending(p => p.Id).FirstOrDefault(p => p.ClientPath == path.Trim());
                if (MonitFile != null)
                    return MonitFile.MapTo<MonitFileModel>();
                else
                    return null;
            }
            else
                return null;


        }

        /// <summary>
        ///  根据版本获取上一个目录
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>       

        public List<MonitFileModel> GetMonitFileByVersion(long versionId)
        {
            if (versionId != null)
            {
                var MonitFile = _MonitFileCase.GetAllList(p => p.FolderVersionId == versionId&&p.Status!=(short)MonitStatus.Delete);
                if (MonitFile != null)
                    return MonitFile.MapTo<List<MonitFileModel>>();
                else
                    return null;
            }
            else
                return null;
        }


        #endregion

        #region 监控日志写入
        /// <summary>
        /// 插入一条监控日志
        /// </summary>
        /// <param name="log">日志实例</param>
        public void Log(MonitLogModel log)
        {
            var curLog = log.MapTo<MonitLog>();
            curLog.LogTime = DateTime.Now;
            _MonitLogCase.Insert(curLog);//插入日志
        }
        /// <summary>
        /// 插入一条监控日志
        /// </summary>
        /// <param name="caseVersionId"></param>
        /// <param name="mointFileId"></param>
        /// <param name="logType"></param>
        /// <param name="logMsg"></param>
        public void Log(long? caseVersionId,long? mointFileId,short? logType,string logMsg)
        {
            var log = new MonitLog {
                CaseVersionId=caseVersionId,
                MonitFileId=mointFileId,
                LogType=logType,
                LogMsg=logMsg,
                LogTime=DateTime.Now
            };
            _MonitLogCase.Insert(log);//插入日志
        }
        /// <summary>
        /// 插入一条监控日志(含日志批次ID)
        /// </summary>
        /// <param name="caseVersionId"></param>
        /// <param name="monitLogVersionId"></param>
        /// <param name="mointFileId"></param>
        /// <param name="logType"></param>
        /// <param name="logMsg"></param>
        public void Log(long? caseVersionId, long? monitLogVersionId, long? mointFileId, short? logType, string logMsg)
        {
            var log = new MonitLog
            {
                CaseVersionId = caseVersionId,
                MonitLogVersionId = monitLogVersionId,
                MonitFileId = mointFileId,
                LogType = logType,
                LogMsg = logMsg,
                LogTime = DateTime.Now
            };
            _MonitLogCase.Insert(log);//插入日志
        }
        #endregion

        #region 上传监控文件到服务端+将服务端文件(夹)还原到客户端

        #region 公共方法
        /// <summary>
        /// 监控并上传监控文件到服务器（单个文件的上传）
        /// </summary>
        /// <param name="monitFileId"></param>
        [System.Web.Http.HttpGet]
        public string UpFileByMonitFile(long? monitFileId)
        {
            ErrorInfo err = new ErrorInfo();//初始化
            err.IsError = false;
            err.Message = "";

            long? caseVersionId = null;
            short logType = (short)LogType.UpLog;
            if (monitFileId == null)
            {
                var msg = "传入的监控文件编号[" + monitFileId + "]为空值";
                Log(caseVersionId, monitFileId, logType, msg);
                err.IsError = true;
                err.Message = msg;
            }
            else
            {
                var monitFile= _MonitFileCase.FirstOrDefault(monitFileId.Value);
                if (monitFile == null)
                {
                    var msg = "未找到编号为[" + monitFileId.ToString() + "]的监控文件";
                    Log(caseVersionId, monitFileId, logType, msg);
                    err.IsError = true;
                    err.Message = msg;
                }
                else
                {
                    caseVersionId = monitFile.CaseVersionId;//赋值
                    switch (monitFile.CopyStatus.Value)
                    {
                        case (short)CopyStatus.Fail:
                        case (short)CopyStatus.Wait:
                            //Log(caseVersionId, monitFileId, logType, "开始把monitFileId[" + monitFileId.ToString() + "]文件从终端拷贝到服务端");
                            CopyFile(monitFile, LogType.UpLog,ref err);
                            break;
                        case (short)CopyStatus.Excuting:
                        case (short)CopyStatus.Success:
                            break;
                    }
                }
            }
            if (err.IsError)
            {
                return err.Message;
            }
            else
               return "";
        }
        /// <summary>
        /// 还原服务端的文件到客户端（含文件夹和文件两种形式还原处理）
        /// </summary>
        /// <param name="monitFileId"></param>
        [System.Web.Http.HttpGet]
        public string RestoreFileByMonitFile(long? monitFileId)
        {
            ErrorInfo err = new ErrorInfo();//初始化
            err.IsError = false;
            err.Message = "";

            long? caseVersionId = null;
            short logType = (short)LogType.RestoreLog;
            if (monitFileId == null)
            {
                var msg = "传入的监控文件编号[" + monitFileId + "]为空值";
                Log(caseVersionId, monitFileId, logType, msg);
                err.IsError = true;
                err.Message = msg;
            }
            else
            {
                var monitFile = _MonitFileCase.FirstOrDefault(monitFileId.Value);
                if (monitFile == null)
                {
                    var msg = "未找到编号为[" + monitFileId.ToString() + "]的监控文件";
                    Log(caseVersionId, monitFileId, logType, msg);
                    err.IsError = true;
                    err.Message = msg;
                }
                else
                {
                    caseVersionId = monitFile.CaseVersionId;//赋值

                    //验证当前monitFileId是否有正还原中的进程
                    var hasTask = _MonitLogVersionCase.FirstOrDefault(p => p.MonitFileId == monitFile.Id && p.Status == (short)LogStatus.Executing && p.LogType == logType);
                    if (hasTask != null)
                    {
                        err.IsError = true;
                        string msg = "文件[" + monitFile.Name + "]已存在正在还原中的进程,当前还原任务被取消！";
                        err.Message = msg;
                    }
                    else
                    {
                        //创建日志版本 
                        MonitLogVersion logVersion = new MonitLogVersion();
                        logVersion.LogType = logType;
                        logVersion.MonitFileId = monitFileId;
                        logVersion.Status = (short)LogStatus.Executing;
                        logVersion.BeginTime = DateTime.Now;
                        var logVersionId = _MonitLogVersionCase.InsertAndGetId(logVersion);//插入到库

                        var msg = "创建日志版本，编号[" + logVersionId + "]";
                        Log(caseVersionId, monitFileId, logType, msg);

                        //判断文件是否为文件夹
                        if (monitFile.FileFormat.IsFolder.Value)//为文件夹时
                        {
                            msg = "被还原的为文件夹，开始进入文件夹还原方法体处理";
                            Log(caseVersionId, monitFileId, logType, msg);

                            CopyDirectory(monitFile,ref err, logVersionId);
                        }
                        else
                        {
                            msg = "被还原的为文件，开始进入文件还原方法体处理";
                            Log(caseVersionId, monitFileId, logType, msg);

                            CopyFile(monitFile, LogType.RestoreLog, ref err, logVersionId);
                        }

                        if (err.IsError)//有错时
                        {
                            logVersion.Status = (short)LogStatus.Fail;//失败
                            logVersion.EndTime = DateTime.Now;
                            _MonitLogVersionCase.Update(logVersion);
                        }
                        else
                        {
                            logVersion.Status = (short)LogStatus.Success;//成功
                            logVersion.EndTime = DateTime.Now;
                            _MonitLogVersionCase.Update(logVersion);
                        }
                    }
                }
            }

            //返回结果数据
            if (err.IsError)
            {
                return err.Message;
            }
            else
                return "";
        }
        #endregion

        #region 私有函数
        /// <summary>
        /// 根据LogType拷贝文件(含客户端到服务端+服务端到客户端)
        /// </summary>
        /// <param name="monitFile"></param>
        /// <param name="logType"></param>
        private void CopyFile(MonitFile monitFile, LogType logType, ref ErrorInfo err, long? monitLogVersionId = null)
        {
            //考虑让这个方法返回bool类型，用于在还原时的判断依据
            if (monitFile != null)
            {
                string fromPath = "";//初始化
                string toPath = "";//初始化
                switch (logType)
                {
                    case LogType.UpLog:
                        fromPath = monitFile.ClientPath;
                        toPath = monitFile.ServerPath;
                        break;
                    case LogType.RestoreLog:
                        fromPath = monitFile.ServerPath;
                        toPath = monitFile.ClientPath;
                        break;
                }
                string userName = monitFile.Folder.Computer.UserName;// 
                string pwd = GetDecryptPwd(monitFile.Folder.Computer.Pwd);// 
                string ip = monitFile.Folder.Computer.Ip;

                // 通过IP 用户名 密码 访问远程目录  不需要权限
                using (SharedTool tool = new SharedTool(userName, pwd, ip))
                {
                    if (File.Exists(fromPath))
                    {
                        switch (logType)
                        {
                            case LogType.UpLog:
                                //注意：修改FM_FILE_LIBRARY的IS_COPY字段；修改FM_MONIT_FILE的COPY_STATUS字段（待处理）


                                //参数1：要复制的源文件路径，
                                //参数2：复制后的目标文件路径，
                                //参数3：是否覆盖相同文件名
                                Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, "开始把编号monitFileId[" + monitFile.Id.ToString() + "]的文件从客户端[" + fromPath + "]迁移到服务端[" + toPath + "]");
                                SaveMonitFile(monitFile, CopyStatus.Excuting);//修改monitFile的状态为拷贝中
                                try
                                {
                                    File.Copy(fromPath, toPath, true);//从客户端拷贝文件到服务端(覆盖式拷贝)
                                    SaveMonitFile(monitFile, CopyStatus.Success);//修改monitFile的状态为拷贝成功
                                    //修改文件库为已拷贝状态
                                    var fileL = _FileLibraryCase.FirstOrDefault(monitFile.FileLibraryId.Value);
                                    fileL.IsCopy = true;
                                    _FileLibraryCase.Update(fileL);
                                    var msg = "编号monitFileId[" + monitFile.Id.ToString() + "]的文件从客户端[" + fromPath + "]迁移到服务端[" + toPath + "]拷贝成功";
                                    Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, msg);
                                }
                                catch (Exception e)
                                {
                                    var msg = "编号monitFileId[" + monitFile.Id.ToString() + "]的文件从客户端[" + fromPath + "]迁移到服务端[" + toPath + "]拷贝失败：" + e.Message;
                                    Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, msg);
                                    err.IsError = true;
                                    err.Message = msg;
                                    SaveMonitFile(monitFile, CopyStatus.Fail);//修改monitFile的状态为失败
                                }
                                break;
                            case LogType.RestoreLog:
                                Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, "开始把编号monitFileId[" + monitFile.Id.ToString() + "]的文件从服务端[" + fromPath + "]迁移到客户端[" + toPath + "]");
                                //SaveMonitFile(monitFile, CopyStatus.Excuting);

                                //先重命名客户端原来文件
                                var repVar = toPath.Substring(toPath.LastIndexOf('.'));
                                //重命名其实就可以是服务端的名称，不用时间ticks
                                var renamePath = toPath.Replace(repVar, DateTime.Now.Ticks.ToString() + repVar);
                                try
                                {
                                    RemaneFile(toPath, renamePath);//重命名
                                    Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, "把客户端文件重命名成功:从[" + toPath + "]到[" + renamePath + "]");
                                    try
                                    {
                                        File.Copy(fromPath, toPath, false);//从服务端拷贝文件到客户端(非覆盖式拷贝)
                                        //SaveMonitFile(monitFile, CopyStatus.Success);
                                        try
                                        {
                                            File.Delete(renamePath);//删除重命名的文件
                                            Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, "删除客户端重命名的文件[" + renamePath + "]成功");

                                            var msg = "编号monitFileId[" + monitFile.Id.ToString() + "]的文件从服务端[" + fromPath + "]迁移到客户端[" + toPath + "]拷贝成功";
                                            Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, msg);
                                        }
                                        catch (Exception e)
                                        {
                                            var msg = "删除客户端重命名的文件[" + renamePath + "]失败:" + e.Message;
                                            Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, msg);
                                            err.IsError = false;
                                            err.Message = msg;//警告信息
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        var msg = "编号monitFileId[" + monitFile.Id.ToString() + "]的文件从服务端[" + fromPath + "]迁移到客户端[" + toPath + "]拷贝失败：" + e.Message;
                                        Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, msg);
                                        err.IsError = true;
                                        err.Message = msg;
                                    }
                                }
                                catch (Exception e)
                                {
                                    var msg = "把客户端文件重命名失败:从[" + toPath + "]到[" + renamePath + "]拷贝失败：" + e.Message;
                                    Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, msg);
                                    err.IsError = true;
                                    err.Message = msg;
                                }
                                break;
                        }
                    }
                    else
                    {
                        var msg = "源文件[" + fromPath + "]不存在";
                        Log(monitFile.CaseVersionId, monitLogVersionId, monitFile.Id, (short)logType, msg);
                        err.IsError = true;
                        err.Message = msg;
                        SaveMonitFile(monitFile, CopyStatus.NotExist);//修改monitFile的状态为不存在状态
                    }
                }
            }
        }

        /// <summary>
        /// 把文件夹还原到客户端（需要遍历生成文件夹及以下子文件）
        /// </summary>
        /// <param name="monitFile"></param>
        /// <param name="err"></param>
        /// <param name="monitLogVersionId"></param>
        private void CopyDirectory(MonitFile monitFile, ref ErrorInfo err, long? monitLogVersionId = null)
        {
            if (monitFile != null)
            {
                long? caseVersionId = null;
                short logType = (short)LogType.RestoreLog;
                long monitFileId = monitFile.Id;

                string clientPath = monitFile.ClientPath;//客户端路径
                string serverPath = monitFile.ServerPath;//服务端路径

                string userName = monitFile.Folder.Computer.UserName;// 
                string pwd = GetDecryptPwd(monitFile.Folder.Computer.Pwd);// 
                string ip = monitFile.Folder.Computer.Ip;

                //验证文件是否为未删除状态
                if (monitFile.Status == (short)MonitStatus.Delete)
                {
                    var msg = "文件[" + monitFile.Name + "]为已删除状态，不能还原。";
                    Log(caseVersionId, monitLogVersionId, monitFileId, logType, msg);

                    err.IsError = true;
                    err.Message = msg;
                    return;
                }

                // 登录客户端：通过IP 用户名 密码 访问远程目录
                using (SharedTool tool = new SharedTool(userName, pwd, ip))
                {
                    #region 获得当前文件所在共享目录的所有文件列表 clientFiles
                    string selectPath = string.Format(@"\\{0}\{1}", ip, monitFile.Folder.Name);
                    var dicInfo = new DirectoryInfo(selectPath);//选择的目录信息 
                    FileInfo[] clientFiles = GetFilesByDir(dicInfo);//获得客户端所有文件
                    #endregion

                    //新文件夹路径.MonitConst.RestoreStr还原的特殊符号
                    string tempClientPath = clientPath.Substring(0, clientPath.LastIndexOf(@"\") + 1) + monitFile.Name + MonitConst.RestoreStr;
                    DirectoryInfo di = new DirectoryInfo(tempClientPath);
                    // 没有时就创建
                    if (di.Exists == false)
                        di.Create();

                    var msg = "创建临时文件夹：" + tempClientPath;
                    Log(caseVersionId, monitLogVersionId, monitFileId, logType, msg);

                    //查询待还原的文件夹的子目录结构
                    string sql = string.Format(@"    SELECT A.ID,A.PARENT_ID,
                                                   A.NAME,
                                                   A.CLIENT_PATH,
                                                   A.SERVER_PATH,
                                                   A.MD5,
                                                   B.IS_FOLDER
                                              FROM FM_MONIT_FILE A
                                                   LEFT JOIN FM_FILE_FORMAT B ON (A.FILE_FORMAT_ID = B.ID)
                                             WHERE A.STATUS <> 1
                                        CONNECT BY PRIOR A.ID = PARENT_ID
                                        START WITH A.ID = {0}", monitFile.Id);
                    DataTable fileTb = DbHelper.ExecuteGetTable(sql);
                    if (fileTb != null && fileTb.Rows.Count > 0)
                    {
                        msg = "开始给临时文件夹生成子结构...";
                        Log(caseVersionId, monitLogVersionId, monitFileId, logType, msg);

                        //给新文件目录生成子文件信息
                        RecursionDir(monitFile.Id, null, ref err, fileTb, clientPath, tempClientPath, clientFiles);

                        msg = "临时文件夹子结构生成完毕";
                        Log(caseVersionId, monitLogVersionId, monitFileId, logType, msg);
                    }

                    //验证目录生成结果
                    if (err.IsError)
                    {
                        msg = "临时文件夹子结构发生错误：" + err.Message;
                        Log(caseVersionId, monitLogVersionId, monitFileId, logType, msg);

                        msg = "删除临时文件夹：" + tempClientPath;
                        Log(caseVersionId, monitLogVersionId, monitFileId, logType, msg);
                        //当文件还原有错误信息时：删除新文件夹.返回错误信息
                        Directory.Delete(tempClientPath, true);
                    }
                    else//注意以下文件名称变更过程可能出现失败，需要进行异常处理
                    {
                        msg = "原文件夹更名为：" + clientPath + MonitConst.MiddleStr;
                        Log(caseVersionId, monitLogVersionId, monitFileId, logType, msg);
                        //修改原文件夹名字
                        Directory.Move(clientPath, clientPath + MonitConst.MiddleStr);

                        msg = "将临时文件夹更名为新文件夹：" + clientPath;
                        Log(caseVersionId, monitLogVersionId, monitFileId, logType, msg);
                        //将新文件夹名改为原文件夹名
                        Directory.Move(tempClientPath, clientPath);

                        msg = "删除被更名的原文件夹：" + clientPath + MonitConst.MiddleStr;
                        Log(caseVersionId, monitLogVersionId, monitFileId, logType, msg);
                        //删除原文件夹
                        Directory.Delete(clientPath + MonitConst.MiddleStr, true);
                    }
                }

                #region 处理逻辑说明及注意项
                //1、在客户端路径根目录建立一个新的文件夹（文件夹+MonitConst.MiddleStr）

                //2、查询sql，得到当前客户端文件夹以下的所有文件信息datatable

                //3、新增递归方法RecursionDir：传入monitFileId,在新文件夹下生成子文件和文件夹，直至所有生成完成
                //注意项：为避免大文件带来的复制瓶颈，先根据md5在本机共享目录下去检查是否具有相同文件，如果存在直接copy
                //如果没有，再去远端服务器下载到本机目录

                //4、修改本机原文件夹名字，将新建的文件夹该名为原名，删除原文件夹

                //注意项：如果以上环节中出错，处理方式：如果拷贝文件出错，是否要跳出整个还原？已经还原的文件是否要做删除？
                //是否要留给下次还原来遍历如果有相同文件可直接使用，然后是如何删除文件的问题？
                //另外需要在监控服务处，过滤这种还原的临时文件存储名称（）
                //在每次还原的时候，需要先验证该文件夹或文件是否已处于还原状态。
                #endregion
            }
        }

        /// <summary>
        /// 递归生成文件夹下的子文件信息
        /// </summary>
        /// <param name="beginMonitFileId"></param>
        /// <param name="curDataRow"></param>
        /// <param name="err"></param>
        /// <param name="fileTb"></param>
        /// <param name="oldRootPath"></param>
        /// <param name="newRootPath"></param>
        /// <param name="clientFiles"></param>
        private void RecursionDir(long? beginMonitFileId, DataRow curDataRow, ref ErrorInfo err, DataTable fileTb, string oldRootPath, string newRootPath, FileInfo[] clientFiles)
        {
            if (fileTb != null && fileTb.Rows.Count > 0)
            {
                if (beginMonitFileId != null)//第一个层级:顶点
                {
                    foreach (DataRow dr in fileTb.Rows)
                    {
                        //父级不为空且等于初始值时
                        if (!string.IsNullOrEmpty(dr["PARENT_ID"].ToString()) && dr["PARENT_ID"].ToString() == beginMonitFileId.Value.ToString())
                        {
                            //调用递归:扫描并建立子文件结构
                            RecursionDir(null, dr, ref err, fileTb, oldRootPath, newRootPath, clientFiles);
                        }
                    }
                }
                else
                {
                    var curClientPath = curDataRow["CLIENT_PATH"].ToString();//原客户端路径
                    var newClientPath = newRootPath + curClientPath.Substring(oldRootPath.Length);//文件新路径
                    //当前文件为文件夹时
                    if (curDataRow["IS_FOLDER"].ToString() == "1")
                    {
                        DirectoryInfo di = new DirectoryInfo(newClientPath);
                        // 没有时就创建
                        if (di.Exists == false)
                            di.Create();

                        //调用递归:扫描并建立子文件结构
                        foreach (DataRow dr in fileTb.Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["PARENT_ID"].ToString()) && dr["PARENT_ID"].ToString() == curDataRow["ID"].ToString())
                            {
                                RecursionDir(null, dr, ref err, fileTb, oldRootPath, newRootPath, clientFiles);
                            }
                        }
                    }
                    else//为文件时
                    {
                        FileInfo file = null;
                        //依次遍历获得文件，判断其md5值是否与当前文件相同
                        foreach (FileInfo temp in clientFiles)
                        {
                            try
                            {
                                string curMd5 = FileTool.GetFileHash(temp.FullName);
                                if (!string.IsNullOrEmpty(curMd5) && curMd5 == curDataRow["MD5"].ToString())
                                {
                                    file = temp;
                                    break;//跳出循环
                                }
                            }
                            catch
                            {
                            }
                        }
                        //验证当前文件是否与客户端文件匹配
                        //匹配时
                        if (file != null)
                        {
                            File.Copy(file.FullName, newClientPath, true);//将客户机文件复制一份当新目录
                        }
                        else//不匹配时，从服务端拷贝文件
                        {
                            var curServerPath = curDataRow["SERVER_PATH"].ToString();
                            File.Copy(curServerPath, newClientPath, true);//从客户端拷贝文件到服务端(覆盖式拷贝)
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region 客户在web端下载文件(夹)到本机

        #region 公共方法
        /// <summary>
        /// 下载前的准备：生成待下载文件到临时文件夹：根目录+tempFolder
        /// 1）文件夹下载：生成临时文件夹及子目录,压缩成zip包,删除生成临时文件夹 ->返回生成后的压缩文件名
        /// 2）文件下载：生成临时文件 ->返回生成后的临时文件名
        /// </summary>
        /// <param name="monitFileId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public string GenerateFile(long? monitFileId)
        {
            string fileName = "";
            long? caseVersionId = null;
            short logType = (short)LogType.DownLog;

            if (monitFileId != null)
            {
                MonitFile monitFile = _MonitFileCase.FirstOrDefault(monitFileId.Value);
                if (monitFile != null)
                {
                    //创建日志版本 
                    MonitLogVersion logVersion = new MonitLogVersion();
                    logVersion.LogType = logType;
                    logVersion.MonitFileId = monitFileId;
                    logVersion.Status = (short)LogStatus.Executing;
                    logVersion.BeginTime = DateTime.Now;
                    var logVersionId = _MonitLogVersionCase.InsertAndGetId(logVersion);//插入到库

                    if (monitFile.FileFormat.IsFolder.Value)//文件夹
                    {
                        fileName = GenerateZip(monitFile, logVersionId);//在网站根目录生成压缩文件，并返回压缩文件名
                    }
                    else//文件
                    {
                        if (monitFile.CopyStatus == (short)CopyStatus.Success)//文件已拷贝到服务端
                        {
                            string tempVar = DateTime.Now.Ticks.ToString();
                            string tempPath = "";

                            var msg = "开始生成临时文件名...";
                            Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                            string newFileName = "";
                            if (monitFile.Name.LastIndexOf('.') != -1)//有后缀
                            {
                                newFileName = monitFile.Name.Insert(monitFile.Name.LastIndexOf('.'), "_" + tempVar);
                                //tempPath = HttpContext.Current.Server.MapPath("/") + newFileName;
                                tempPath = AppDomain.CurrentDomain.BaseDirectory + "tempFolder\\" + newFileName;
                            }
                            else//无后缀
                            {
                                newFileName = monitFile.Name + "_" + tempVar;
                                //tempPath = HttpContext.Current.Server.MapPath("/") + newFileName;
                                tempPath = AppDomain.CurrentDomain.BaseDirectory + "tempFolder\\" + newFileName;
                            }
                            try
                            {
                                msg = "临时文件名生成完毕：" + tempPath;
                                Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                                msg = "开始拷贝临时文件：" + tempPath;
                                Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                                File.Copy(monitFile.ServerPath, tempPath, true);//复制文件

                                msg = "拷贝临时文件完成：" + tempPath;
                                Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                                msg = "返回临时文件：" + tempPath;
                                Log(caseVersionId, logVersionId, monitFileId, logType, msg);
                                fileName = newFileName;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            var msg = "文件编号[" + monitFileId.ToString() + "]名[" + monitFile.Name + "]未找到";
                            Log(caseVersionId, logVersionId, monitFileId, logType, msg);
                        }
                    }
                }
                else
                {
                    var msg = "未找到编号为[" + monitFileId.ToString() + "]的监控文件";
                    Log(caseVersionId, monitFileId, logType, msg);
                }
            }
            else
            {
                var msg = "传入的监控文件编号[" + monitFileId + "]为空值";
                Log(caseVersionId, monitFileId, logType, msg);
            }
            return fileName;
        }


        #endregion

        #region 私有方法
        /// <summary>
        /// 文件夹下载：生成临时文件夹及子目录,压缩成zip包,删除生成临时文件夹 ->返回生成后的压缩文件名
        /// </summary>
        /// <param name="monitFile"></param>
        /// <param name="logVersionId"></param>
        /// <returns></returns>
        private string GenerateZip(MonitFile monitFile, long? logVersionId)
        {
            string url = "";
            long? caseVersionId = null;
            short logType = (short)LogType.DownLog;
            long monitFileId = monitFile.Id;

            ErrorInfo err = new ErrorInfo();
            err.IsError = false;
            err.Message = "";

            //MonitFile monitFile = _MonitFileCase.FirstOrDefault(monitFileId.Value);
            if (monitFile != null)
            {
                string serverPath = monitFile.ServerPath;//服务端路径
                //在服务端存放监控文件的目录不会设定为被监控。可以考虑用时间戳作为中间变量
                string tempVar = DateTime.Now.Ticks.ToString();//
                //在服务端的ip目录下创建文件夹
                //string tempPath = serverPath.Substring(0, serverPath.IndexOf(monitFile.Folder.Computer.Ip) + monitFile.Folder.Computer.Ip.Length) + "\\" + monitFile.Name + tempVar;

                string tempPath = AppDomain.CurrentDomain.BaseDirectory + "tempFolder\\" + monitFile.Name + tempVar; ;
                //string tempPath = HttpContext.Current.Server.MapPath("/") + monitFile.Name + tempVar;

                var msg = "开始创建临时文件夹：" + tempPath;
                Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                //创建文件夹
                DirectoryInfo di = new DirectoryInfo(tempPath);
                // 没有时就创建
                if (di.Exists == false)
                    di.Create();

                msg = "临时文件夹创建完毕：" + tempPath;
                Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                string clientRootPath = monitFile.ClientPath;//客户端根目录

                msg = "查询当前文件夹子结构...";
                Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                //查询待还原的文件夹的子目录结构
                string sql = string.Format(@"    SELECT A.ID,A.PARENT_ID,A.COPY_STATUS,
                                                   A.NAME,
                                                   A.CLIENT_PATH,
                                                   A.SERVER_PATH,
                                                   A.MD5,
                                                   B.IS_FOLDER
                                              FROM FM_MONIT_FILE A
                                                   LEFT JOIN FM_FILE_FORMAT B ON (A.FILE_FORMAT_ID = B.ID)
                                             WHERE A.STATUS <> 1
                                        CONNECT BY PRIOR A.ID = PARENT_ID
                                        START WITH A.ID = {0}", monitFile.Id);
                DataTable fileTb = DbHelper.ExecuteGetTable(sql);

                msg = "文件夹子结构查询完毕.";
                Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                if (fileTb != null && fileTb.Rows.Count > 0)
                {
                    msg = "文件夹子文件(夹)数：" + fileTb.Rows.Count;
                    Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                    msg = "开始递归生成文件夹树结构...";
                    Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                    //给新文件目录生成子文件信息
                    RecursionDownDir(monitFile.Id, null, ref err, fileTb, tempPath, clientRootPath);

                    msg = "生成树结构完毕.";
                    Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                    msg = "开始压缩文件夹...";
                    Log(caseVersionId, logVersionId, monitFileId, logType, msg);
                    //压缩文件夹
                    string sourceDir = tempPath + "\\";//待压缩目录
                    //string targetFile = HttpContext.Current.Server.MapPath("/") + monitFile.Name + "_" + tempVar + ".zip";//压缩后的文件
                    string targetFile = AppDomain.CurrentDomain.BaseDirectory + "tempFolder\\" + monitFile.Name + "_" + tempVar + ".zip";//压缩后的文件
                    ZipHelper zip = new ZipHelper();
                    zip.ZipFileDirectory(sourceDir, targetFile);//压缩文件

                    msg = "压缩完毕，文件名：" + monitFile.Name + "_" + tempVar + ".zip";
                    Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                    //压缩后删除文件夹
                    Directory.Delete(tempPath, true);

                    msg = "删除临时文件夹：" + tempPath;
                    Log(caseVersionId, logVersionId, monitFileId, logType, msg);

                    url = monitFile.Name + "_" + tempVar + ".zip";

                    msg = "返回文件：" + url;
                    Log(caseVersionId, logVersionId, monitFileId, logType, msg);
                }
            }
            return url;
        }
        /// <summary>
        /// 服务于文件夹下载：递归生成文件夹的子文件结构
        /// </summary>
        /// <param name="beginMonitFileId"></param>
        /// <param name="curDataRow"></param>
        /// <param name="err"></param>
        /// <param name="fileTb"></param>
        /// <param name="tempRootPath"></param>
        /// <param name="clientRootPath"></param>
        private void RecursionDownDir(long? beginMonitFileId, DataRow curDataRow, ref ErrorInfo err, DataTable fileTb, string tempRootPath, string clientRootPath)
        {
            if (fileTb != null && fileTb.Rows.Count > 0)
            {
                if (beginMonitFileId != null)
                {
                    foreach (DataRow dr in fileTb.Rows)
                    {
                        //父级不为空且等于初始值时
                        if (!string.IsNullOrEmpty(dr["PARENT_ID"].ToString()) && dr["PARENT_ID"].ToString() == beginMonitFileId.Value.ToString())
                        {
                            //调用递归:扫描并建立子文件结构
                            RecursionDownDir(null, dr, ref err, fileTb, tempRootPath, clientRootPath);
                        }
                    }
                }
                else
                {
                    //复制文件或创建文件夹
                    string clientPath = curDataRow["CLIENT_PATH"].ToString();
                    string fileName = curDataRow["NAME"].ToString();//文件名
                    string curPaht = tempRootPath + clientPath.Substring(clientRootPath.Length);//待创建

                    //当前文件为文件夹时
                    if (curDataRow["IS_FOLDER"].ToString() == "1")
                    {
                        DirectoryInfo di = new DirectoryInfo(curPaht);
                        // 没有时就创建
                        if (di.Exists == false)
                            di.Create();

                        //调用递归:扫描并建立子文件结构
                        foreach (DataRow dr in fileTb.Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["PARENT_ID"].ToString()) && dr["PARENT_ID"].ToString() == curDataRow["ID"].ToString())
                            {
                                RecursionDownDir(null, dr, ref err, fileTb, tempRootPath, clientRootPath);
                            }
                        }
                    }
                    else//为文件时
                    {
                        var curServerPath = curDataRow["SERVER_PATH"].ToString();
                        try
                        {
                            if (curDataRow["COPY_STATUS"].ToString() == "1")
                            {
                                File.Copy(curServerPath, curPaht, true);//复制文件
                            }
                        }
                        catch { }
                    }
                }
            }

        }

        #endregion

        #endregion

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        [System.Web.Http.HttpGet]
        public void DeleteFile(string fileName)
        {
            //File.Delete(HttpContext.Current.Server.MapPath("/") + fileName);
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "tempFolder\\" + fileName);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        [System.Web.Http.HttpGet]
        public void DeleteTempFiles()
        {
            var tempDir = AppDomain.CurrentDomain.BaseDirectory + "tempFolder\\";
            FileInfo[] files = GetFilesByDir(tempDir);
            if(files!=null&&files.Length>0)
            {
                //循环删除临时文件夹下的文件
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file.FullName);//删除能删掉的文件
                    }
                    catch (Exception ex) { }
                }
            }
        }

        /// <summary>
        /// 获取当前用户管辖共享文件夹的监控错误数
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public int GetErrorNumByUser()
        {
            int num = 0;

            #region 作废sql
            //string sql = string.Format(@"SELECT COUNT(1) FROM 
            //            EM_SCRIPT_NODE_CASE F,
            //            FM_CASE_VERSION D,
            //            FM_FOLDER_VERSION A,
            //            FM_FOLDER B,
            //            (
            //            SELECT A.ID
            //                FROM FM_COMPUTER A,
            //                    (    SELECT ID
            //                            FROM EM_DISTRICT
            //                    CONNECT BY PRIOR ID = PARENT_ID
            //                    START WITH ID = (SELECT DISTRICT_ID
            //                                        FROM ABP_USERS
            //                                        WHERE ID = {0})) B
            //                WHERE A.DISTRICT_ID = B.ID
            //                ) C
            //                WHERE B.COMPUTER_ID=C.ID
            //                AND A.FOLDER_ID=B.ID
            //                AND D.FOLDER_VERSION_ID=A.ID
            //                AND F.ID=SCRIPT_NODE_CASE_ID
            //                AND F.RETURN_CODE=0", CurrUserId());
            #endregion

            #region sql
            string sql = string.Format(@"SELECT COUNT (1)
                    FROM (SELECT 0 MONIT_FILE_ID,
                                D.ID CASE_VERSION_ID,
                                C.NAME COMPUTER_NAME,
                                C.IP,
                                B.NAME FOLDER_NAME,
                                '共享目录监控' TASK_TYPE,
                                F.END_TIME ERROR_TIME
                            FROM EM_SCRIPT_NODE_CASE F,
                                FM_CASE_VERSION D,
                                FM_FOLDER_VERSION A,
                                FM_FOLDER B,
                                (SELECT A.*
                                    FROM FM_COMPUTER A,
                                        (    SELECT ID
                                                FROM EM_DISTRICT
                                        CONNECT BY PRIOR ID = PARENT_ID
                                        START WITH ID = (SELECT DISTRICT_ID
                                                            FROM ABP_USERS
                                                            WHERE ID = {0})) B
                                    WHERE A.DISTRICT_ID = B.ID) C
                            WHERE     B.COMPUTER_ID = C.ID
                                AND A.FOLDER_ID = B.ID
                                AND D.FOLDER_VERSION_ID = A.ID
                                AND F.ID = SCRIPT_NODE_CASE_ID
                                AND F.RETURN_CODE = 0
                        UNION
                        SELECT A.ID MONIT_FILE_ID,
                                A.CASE_VERSION_ID,
                                B.NAME COMPUTER_NAME,
                                B.IP,
                                C.NAME FOLDER_NAME,
                                '拷贝文件到服务端' TASK_TYPE,
                                A.COPY_STATUS_TIME ERROR_TIME
                            FROM FM_MONIT_FILE A,
                                (SELECT A.*
                                    FROM FM_COMPUTER A,
                                        (    SELECT ID
                                                FROM EM_DISTRICT
                                        CONNECT BY PRIOR ID = PARENT_ID
                                        START WITH ID = (SELECT DISTRICT_ID
                                                            FROM ABP_USERS
                                                            WHERE ID = {0})) B
                                    WHERE A.DISTRICT_ID = B.ID) B,
                                FM_FOLDER C
                            WHERE     A.COMPUTER_ID = B.ID
                                AND A.COPY_STATUS = 3
                                AND A.FOLDER_ID = C.ID)", CurrUserId());
            #endregion
            DataTable dt = DbHelper.ExecuteGetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                num = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            return num;
        }

        #region 其他可共用私有方法
        /// <summary>
        /// 根据目录路径获得其下的所有文件集合
        /// </summary>
        /// <param name="dirPaht">E:\easyman\AEasymanProject\</param>
        /// <returns></returns>
        private FileInfo[] GetFilesByDir(string dirPaht)
        {
            DirectoryInfo directory = new DirectoryInfo(dirPaht);
            FileInfo[] textFiles = directory.GetFiles("*.*", SearchOption.AllDirectories);
            return textFiles;
        }

        /// <summary>
        /// 根据目录路径获得其下的所有文件集合
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private FileInfo[] GetFilesByDir(DirectoryInfo directory)
        {
            FileInfo[] textFiles = directory.GetFiles("*.*", SearchOption.AllDirectories);
            return textFiles;
        }

        /// <summary>
        /// 修改监控文件的复制状态
        /// </summary>
        /// <param name="monitFile"></param>
        /// <param name="status"></param>
        private void SaveMonitFile(MonitFile monitFile,CopyStatus status)
        {
            monitFile.CopyStatus = (short)status;
            monitFile.CopyStatusTime = DateTime.Now;
            _MonitFileCase.Update(monitFile);//修改复制状态
            CurrentUnitOfWork.SaveChanges();//保存
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        private void RemaneFile(string oldPath,string newPath)
        {
            FileInfo fileInfo = new FileInfo(oldPath);
            fileInfo.MoveTo(newPath);
        }
       
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
                var p = EncryptHelper.AesDecrpt(pwd);
                aesPwd = p;
            }
            catch
            {
            }
            return aesPwd;
        }
        #endregion

        #region 上传文件下载
        /// <summary>
        /// 文件路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        
        public string GenerateUploadFile(string url)
        {
            try
            {
                string fileName = "";
                string newFileName = "";
                string tempPath = "";
                string tempVar = DateTime.Now.Ticks.ToString();
                if (File.Exists(url))
                {

                    fileName = Path.GetFileNameWithoutExtension(url);

                    newFileName = fileName + "_" + tempVar + ".zip";

                    tempPath = AppDomain.CurrentDomain.BaseDirectory + "tempUploader\\" + newFileName;
                    ZipHelper zip = new ZipHelper();
                    zip.ZipFileOne(url, tempPath);//压缩文件
                    fileName = newFileName;
                    return fileName;
                }
                else
                {
                    return "error!file is not exist" ;
                }

             
            }
            catch(Exception ex)
            {
                return "error!"+ex.Message;
            }
           
        }
            #endregion

        }
    }
