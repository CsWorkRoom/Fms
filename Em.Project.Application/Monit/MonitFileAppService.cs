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
    /// 文件夹及文件管理
    /// </summary>
    public class MonitFileAppService : EasymanAppServiceBase, IMonitFileAppService
    {
        #region 初始化

        private readonly IRepository<MonitFile,long> _MonitFileCase;
        /// <summary>
        /// 构造函数注入MonitFile仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public MonitFileAppService(IRepository<MonitFile, long> MonitFileCase)
        {
            _MonitFileCase = MonitFileCase;
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
            if(_MonitFileCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<MonitFile>();
            var entObj = _MonitFileCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new MonitFile();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<MonitFile>(input);
            var resObj= _MonitFileCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<MonitFileModel>();
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
                var MonitFile = _MonitFileCase.GetAllList(p => p.FolderVersionId == versionId);
                if (MonitFile != null)
                    return MonitFile.MapTo<List<MonitFileModel>>();
                else
                    return null;
            }
            else
                return null;
        }


        #endregion
    }
}
