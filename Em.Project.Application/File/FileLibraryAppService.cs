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
    /// 文件库管理
    /// </summary>
    public class FileLibraryAppService : EasymanAppServiceBase, IFileLibraryAppService
    {
        #region 初始化

        private readonly IRepository<FileLibrary,long> _FileLibraryCase;
        /// <summary>
        /// 构造函数注入FileLibrary仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public FileLibraryAppService(IRepository<FileLibrary, long> FileLibraryCase)
        {
            _FileLibraryCase = FileLibraryCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个文件库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileLibraryModel GetFileLibrary(long id)
        {
            var entObj= _FileLibraryCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<FileLibraryModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增文件库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileLibraryModel InsertOrUpdateFileLibrary(FileLibraryModel input)
        {
            try
            {
                if (_FileLibraryCase.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
                {
                    return _FileLibraryCase.GetAll().FirstOrDefault(x => x.MD5 == input.MD5).MapTo<FileLibraryModel>();

                }
                //var entObj =input.MapTo<FileLibrary>();
                var entObj = _FileLibraryCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new FileLibrary();
                entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
                //var entObj= AutoMapper.Mapper.Map<FileLibrary>(input);
                var id = _FileLibraryCase.InsertAndGetId(entObj);

                return entObj.MapTo<FileLibraryModel>();
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
        public void DeleteFileLibrary(EntityDto<long> input)
        {
            try
            {
                _FileLibraryCase.Delete(input.Id);
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
        public IEnumerable<object> GetFileLibraryTreeJson()
        {
            var objList= _FileLibraryCase.GetAllList();
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
        public List<SelectListItem> FileLibraryList()
        {
            var objList = _FileLibraryCase.GetAllList();
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
        /// 
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        public FileLibraryModel GetFileLibraryByMD5(string md5)
        {
            if (!string.IsNullOrEmpty(md5))
            {
                var FileLibrary = _FileLibraryCase.FirstOrDefault(p => p.MD5 == md5.Trim());
                if (FileLibrary != null)
                    return FileLibrary.MapTo<FileLibraryModel>();
                else
                    return null;
            }
            else
                return null;
        }
        #endregion
    }
}
