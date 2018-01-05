using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using Easyman.Users;
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
    /// 文件管理
    /// </summary>
    public class FileAppService : EasymanAppServiceBase, IFileAppService
    {
        #region 初始化

        private readonly IRepository<MonitFile,long> _MonitFileCase;
        private readonly IRepository<FolderVersion,long> _FolderVersionCase;
        /// <summary>
        /// 构造函数注入FileFormat仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public FileAppService(IRepository<MonitFile, long> MonitFileCase, 
            IRepository<FolderVersion, long> FolderVersionCase)
        {
            _MonitFileCase = MonitFileCase;
            _FolderVersionCase = FolderVersionCase;
        }
        #endregion

        /// <summary>
        /// 获取当前目录的最新文件列表
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<MonitFileModel> GetCurFileListByFolder(long folderId)
        {
            var monitFile = _FolderVersionCase.GetAllList(p => p.FolderId == folderId).OrderByDescending(p => p.Id).FirstOrDefault();
            if (monitFile != null)
            {

                return _MonitFileCase.GetAllList(p => p.FolderId == folderId && p.FolderVersionId == monitFile.Id).MapTo<List<MonitFileModel>>();
            }
            else
                return null;
        }

        public virtual User GetCurrentUser()
        {
            return GetCurrentUserAsync().Result;
        }
        [System.Web.Http.HttpGet]
        public void CopyFile(string fromPath, string toPath)
        {
            ErrorInfo err = new ErrorInfo();
            if (File.Exists(fromPath))
            {
                string userName = "cs1";// computer.UserName.Trim();//lcz2016
                string pwd = "1";// computer.Pwd.Trim();//lcz201314
                string ip = "10.108.226.76";
                // 通过IP 用户名 密码 访问远程目录  不需要权限
                using (SharedTool tool = new SharedTool(userName, pwd, ip))
                {
                    //参数1：要复制的源文件路径，
                    //参数2：复制后的目标文件路径，
                    //参数3：是否覆盖相同文件名
                    File.Copy(fromPath, toPath, false);
                }
            }
            else
            {
                err.IsError = true;
                err.Message = "源路径【" + fromPath + "】不存在";
            }
        }


    }
}
