using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Easyman.App.Dto;
using Easyman.Authorization;
using Easyman.Domain;
using Newtonsoft.Json;

namespace Easyman.App
{
    /// <summary>
    /// 文件服务
    /// </summary>
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    public class FileAppService : EasymanAppServiceBase, IFileAppService
    {
        private readonly IRepository<Files, long> _fileRepository;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="fileRepository"></param>
        public FileAppService(IRepository<Files, long> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        public ApiFileBean FileUp()
        {
            if (HttpContext.Current.Request.Files.Count <= 0)
            {
                return null;

            }

            var fileData = HttpContext.Current.Request.Files[0];
            var userId = HttpContext.Current.Request.Params["userId"];
            //var authToken = HttpContext.Current.Request.Params["authToken"];

            var saveFile = SaveFile(fileData, "UpFiles");

            var retFile = new ApiFileBean
            {
                ID = saveFile.Id,
                USER_ID = saveFile.UserId,
                LENGTH = saveFile.Length,
                NAME = saveFile.Name,
                PATH = saveFile.Path,
                URL = saveFile.Url,
                REMARK = saveFile.Remark,
                UPLOAD_TIME = saveFile.UploadTime,
                FILE_TYPE = saveFile.FileType
            };

            return retFile;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiErrorBean FileDel(ApiRequestEntityBean request)
        {
            var userId = request.userId;
            var token = request.authToken;
            var delId = Convert.ToInt32(request.id);

            var fileInfo = _fileRepository.FirstOrDefault(f => f.Id == delId);

            if (fileInfo == null)
            {
                var errInfo = new ApiErrorBean
                {
                    isError = true,
                    message = "删除失败！"
                };

                return errInfo;
            }

            _fileRepository.Delete(fileInfo);
            File.Delete(HttpContext.Current.Server.MapPath(fileInfo.Url));

            var successInfo = new ApiErrorBean
            {
                isError = false,
                message = "删除成功！"
            };

            return successInfo;
        }

        /// <summary>
        /// 保存提交的文件
        /// </summary>
        /// <returns></returns>
        public ApiFileBean SavePostedSetupFile()
        {
            if (HttpContext.Current.Request.Files.Count <= 0)
            {
                return null;
            }

            var fileData = HttpContext.Current.Request.Files[0];
            var saveFile = SaveFile(fileData,"SetupFiles");

            var retFile = new ApiFileBean
            {
                ID = saveFile.Id,
                USER_ID = saveFile.UserId,
                LENGTH = saveFile.Length,
                NAME = saveFile.Name,
                PATH = saveFile.Path,
                URL = saveFile.Url,
                REMARK = saveFile.Remark,
                UPLOAD_TIME = saveFile.UploadTime,
                FILE_TYPE = saveFile.FileType
            };

            return retFile;
        }

        /// <summary>
        /// 保存文件到本地及数据表
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private Files SaveFile(HttpPostedFile fileData, string folderName)
        {
            try
            {
                // 文件上传后的保存路径
                //var filePath = HttpContext.Current.Server.MapPath("~/UpFiles/");
                var filePath = HttpContext.Current.Server.MapPath("~/" + folderName + "/");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                //文件后缀
                var fileExtension = Path.GetExtension(fileData.FileName);

                //原文件名，无后缀
                var fileName = Path.GetFileNameWithoutExtension(fileData.FileName);

                //原文件名
                var trueName = Path.GetFileName(fileData.FileName);

                //系统保存名
                var serviceName = fileName + "-" + DateTime.Now.ToString("yyyyMMddhhssmm") + fileExtension;
                fileData.SaveAs(filePath + serviceName);

                //var urlPath = "~/UpFiles/" + serviceName;
                var urlPath = "~/" + folderName + "/" + serviceName;

                var saveFile = new Files
                {
                    //UserId = Convert.ToInt32(userId),
                    UserId = AbpSession.UserId.Value,
                    TrueName = trueName,
                    Name = serviceName,
                    Length = (fileData.ContentLength / 1024),
                    Path = urlPath,
                    Url = urlPath,
                    UploadTime = DateTime.Now,
                    FileType = fileExtension
                };

                saveFile.Id = _fileRepository.InsertAndGetId(saveFile);

                return saveFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
