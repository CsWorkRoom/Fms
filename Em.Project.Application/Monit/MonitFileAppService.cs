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
        /// <summary>
        /// 构造函数注入MonitFile仓储
        /// </summary>
        /// <param name="MonitFileCase"></param>
        /// <param name="MonitLogCase"></param>
        public MonitFileAppService(IRepository<MonitFile, long> MonitFileCase,
            IRepository<MonitLog, long> MonitLogCase,
            IRepository<FileLibrary, long> FileLibraryCase)
        {
            _MonitFileCase = MonitFileCase;
            _MonitLogCase = MonitLogCase;
            _FileLibraryCase = FileLibraryCase;
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
        /// 上传文件到服务器
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
        /// 还原服务端的文件到客户端
        /// </summary>
        /// <param name="monitFileId"></param>
        [System.Web.Http.HttpGet]
        public string DownFileByMonitFile(long? monitFileId)
        {
            ErrorInfo err = new ErrorInfo();//初始化
            err.IsError = false;
            err.Message = "";

            long? caseVersionId = null;
            short logType = (short)LogType.DownLog;
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
                    //Log(caseVersionId, monitFileId, logType, "开始把monitFileId[" + monitFileId.ToString() + "]文件从服务端拷贝到客户端");
                    CopyFile(monitFile, LogType.DownLog,ref err);
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

        /// <summary>
        /// 根据LogType拷贝文件
        /// </summary>
        /// <param name="monitFile"></param>
        /// <param name="logType"></param>
        private void CopyFile(MonitFile monitFile,LogType logType,ref ErrorInfo err)
        {
            //考虑让这个方法返回bool类型，用于在还原时的判断依据
            if (monitFile != null)
            {
                string fromPath ="";//初始化
                string toPath = "";//初始化
                switch (logType)
                {
                    case LogType.UpLog:
                        fromPath = monitFile.ClientPath;
                        toPath = monitFile.ServerPath;
                        break;
                    case LogType.DownLog:
                        fromPath = monitFile.ServerPath;
                        toPath = monitFile.ClientPath;
                        break;
                }
                string userName = monitFile.Folder.Computer.UserName;// 
                string pwd = monitFile.Folder.Computer.Pwd;// 
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
                                Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, "开始把编号monitFileId[" + monitFile.Id.ToString() + "]的文件从客户端["+ fromPath + "]迁移到服务端["+ toPath + "]");
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
                                    Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, msg);
                                }
                                catch (Exception e)
                                {
                                    var msg = "编号monitFileId[" + monitFile.Id.ToString() + "]的文件从客户端[" + fromPath + "]迁移到服务端[" + toPath + "]拷贝失败：" + e.Message;
                                    Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, msg);
                                    err.IsError = true;
                                    err.Message = msg;
                                    SaveMonitFile(monitFile, CopyStatus.Fail);//修改monitFile的状态为失败
                                }
                                break;
                            case LogType.DownLog:
                                Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, "开始把编号monitFileId[" + monitFile.Id.ToString() + "]的文件从服务端[" + fromPath + "]迁移到客户端[" + toPath + "]");
                                //SaveMonitFile(monitFile, CopyStatus.Excuting);

                                //先重命名客户端原来文件
                                var repVar = toPath.Substring(toPath.LastIndexOf('.'));
                                //重命名其实就可以是服务端的名称，不用时间ticks
                                var renamePath= toPath.Replace(repVar, DateTime.Now.Ticks.ToString() + repVar);
                                try
                                {
                                    RemaneFile(toPath, renamePath);//重命名
                                    Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, "把客户端文件重命名成功:从[" + toPath + "]到[" + renamePath + "]");
                                    try
                                    {
                                        File.Copy(fromPath, toPath, false);//从服务端拷贝文件到客户端(非覆盖式拷贝)
                                        //SaveMonitFile(monitFile, CopyStatus.Success);
                                        try
                                        {
                                            File.Delete(renamePath);//删除重命名的文件
                                            Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, "删除客户端重命名的文件[" + renamePath + "]成功");

                                            var msg = "编号monitFileId[" + monitFile.Id.ToString() + "]的文件从服务端[" + fromPath + "]迁移到客户端[" + toPath + "]拷贝成功";
                                            Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, msg);
                                        }
                                        catch (Exception e)
                                        {
                                            var msg = "删除客户端重命名的文件[" + renamePath + "]失败:" + e.Message;
                                            Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, msg);
                                            err.IsError = false;
                                            err.Message = msg;//警告信息
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        var msg = "编号monitFileId[" + monitFile.Id.ToString() + "]的文件从服务端[" + fromPath + "]迁移到客户端[" + toPath + "]拷贝失败：" + e.Message;
                                        Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, msg);
                                        err.IsError = true;
                                        err.Message = msg;
                                    }
                                }
                                catch (Exception e)
                                {
                                    var msg = "把客户端文件重命名失败:从[" + toPath + "]到[" + renamePath + "]拷贝失败：" + e.Message;
                                    Log(monitFile.CaseVersionId, monitFile.Id, (short)logType, msg);
                                    err.IsError = true;
                                    err.Message = msg;
                                }
                                break;
                        }
                    }
                    else
                    {
                        var msg = "源文件[" + fromPath + "]不存在";
                        Log(monitFile.CaseVersionId, monitFile.Id, (short)logType,msg);
                        err.IsError = true;
                        err.Message = msg;
                    }
                }
            }
        }

        private void SaveMonitFile(MonitFile monitFile,CopyStatus status)
        {
            monitFile.CopyStatus = (short)status;
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




    }
}
