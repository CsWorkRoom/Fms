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
    /// 文件属性管理
    /// </summary>
    public class FileAttrAppService : EasymanAppServiceBase, IFileAttrAppService
    {
        #region 初始化

        private readonly IRepository<FileAttr, long> _FileAttrCase;
        /// <summary>
        /// 构造函数注入FileAttr仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public FileAttrAppService(IRepository<FileAttr, long> FileAttrCase)
        {
            _FileAttrCase = FileAttrCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个文件属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileAttrModel GetFileAttr(long id)
        {
            var entObj = _FileAttrCase.FirstOrDefault(id);
            if (entObj != null)
            {
                return AutoMapper.Mapper.Map<FileAttrModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【" + id.ToString() + "】的对象！");
        }
        /// <summary>
        /// 更新和新增文件属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileAttrModel InsertOrUpdateFileAttr(FileAttrModel input)
        {

            try
            {
                //var entObj =input.MapTo<FileAttr>();
                var entObj = _FileAttrCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new FileAttr();
                entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
                //var entObj= AutoMapper.Mapper.Map<FileAttr>(input);
                var id = _FileAttrCase.InsertAndGetId(entObj);

                return entObj.MapTo<FileAttrModel>();
            } catch (Exception ex)
            {
                return null;
                throw ex;
            }
        } 

        /// <summary>
        /// 删除一条文件属性
        /// </summary>
        /// <param name="input"></param>
        public void DeleteFileAttr(EntityDto<long> input)
        {
            try
            {
                _FileAttrCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取文件属性json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetFileAttrTreeJson()
        {
            var objList= _FileAttrCase.GetAllList();
            if(objList!=null&& objList.Count>0)
            {
                return objList.Select(s => new
                {
                    id = s.Id,
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
        public List<SelectListItem> FileAttrList()
        {
            var objList = _FileAttrCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.Select(p => new SelectListItem
                {
                    Text = p.Id.ToString(),
                    Value = p.Id.ToString()
                }).ToList();
            }
            return null;
        }

        /// <summary>
        /// 获取文件对应的一条属性
        /// </summary>
        /// <param name="fileLibraryId"></param>
        /// <param name="attrId"></param>
        /// <returns></returns>
        public FileAttrModel  GetFileAttrByFileAndAttr(long fileLibraryId, long attrId)
        {
            var FileAttr = _FileAttrCase.FirstOrDefault(p => p.FileLibraryId==fileLibraryId && p.AttrId==attrId);
            if (FileAttr != null)
                return FileAttr.MapTo<FileAttrModel>();
            else
                return null;
        }

        #endregion
    }
}
