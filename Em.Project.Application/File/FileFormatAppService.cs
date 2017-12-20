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
    /// 文件格式管理
    /// </summary>
    public class FileFormatAppService : EasymanAppServiceBase, IFileFormatAppService
    {
        #region 初始化

        private readonly IRepository<FileFormat,long> _FileFormatCase;
        /// <summary>
        /// 构造函数注入FileFormat仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public FileFormatAppService(IRepository<FileFormat, long> FileFormatCase)
        {
            _FileFormatCase = FileFormatCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个文件格式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileFormatModel GetFileFormat(long id)
        {
            var entObj= _FileFormatCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<FileFormatModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增文件格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileFormatModel InsertOrUpdateFileFormat(FileFormatModel input)
        {
            if(_FileFormatCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<FileFormat>();
            var entObj = _FileFormatCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new FileFormat();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<FileFormat>(input);
            var resObj= _FileFormatCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<FileFormatModel>();
            }
        }

        /// <summary>
        /// 删除一条文件格式
        /// </summary>
        /// <param name="input"></param>
        public void DeleteFileFormat(EntityDto<long> input)
        {
            try
            {
                _FileFormatCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取文件格式json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetFileFormatTreeJson()
        {
            var objList= _FileFormatCase.GetAllList();
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
        public List<SelectListItem> FileFormatList()
        {
            var objList = _FileFormatCase.GetAllList();
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
        #endregion
    }
}
