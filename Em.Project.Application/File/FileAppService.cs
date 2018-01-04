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

    }
}
