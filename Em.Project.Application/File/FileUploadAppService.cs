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

namespace Easyman.Service
{
    /// <summary>
    /// 文件上传管理
    /// </summary>
    public class FileUploadAppService : EasymanAppServiceBase, IFileUploadAppService
    {
        #region 初始化

        private readonly IRepository<FileUpload, long> _FileUploadCase;
        /// <summary>
        /// 构造函数注入FileUpload仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public FileUploadAppService(IRepository<FileUpload, long> FileUploadCase)
        {
            _FileUploadCase = FileUploadCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个文件上传
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileUploadModel GetFileUpload(long id)
        {
            var entObj = _FileUploadCase.FirstOrDefault(id);
            if (entObj != null)
            {
                return AutoMapper.Mapper.Map<FileUploadModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【" + id.ToString() + "】的对象！");
        }
        /// <summary>
        /// 更新和新增文件上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileUploadModel InsertOrUpdateFileUpload(FileUploadModel input)
        {

            try
            {
                //var entObj =input.MapTo<FileUpload>();
                var entObj = _FileUploadCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new FileUpload();
                entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
                //var entObj= AutoMapper.Mapper.Map<FileUpload>(input);
                var id = _FileUploadCase.InsertOrUpdateAndGetId(entObj);

                return entObj.MapTo<FileUploadModel>();
            } catch (Exception ex)
            {
                return null;
                throw ex;
            }
        } 

        /// <summary>
        /// 删除一条文件上传
        /// </summary>
        /// <param name="input"></param>
        public void DeleteFileUpload(EntityDto<long> input)
        {
            try
            {
                _FileUploadCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }

        #endregion
    }
}
