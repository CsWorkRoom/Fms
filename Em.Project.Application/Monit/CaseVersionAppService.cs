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
    ///FM_CASE_VERSION(监控版本)
    /// </summary>
    public class CaseVersionAppService : EasymanAppServiceBase, ICaseVersionAppService
    {
        #region 初始化

        private readonly IRepository<CaseVersion,long> _CaseVersionCase;
        /// <summary>
        /// 构造函数注入CaseVersion仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public CaseVersionAppService(IRepository<CaseVersion, long> CaseVersionCase)
        {
            _CaseVersionCase = CaseVersionCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个版本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CaseVersionModel GetCaseVersion(long id)
        {
            var entObj= _CaseVersionCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<CaseVersionModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增文件库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CaseVersionModel InsertOrUpdateCaseVersion(CaseVersionModel input)
        {

            try
            {
                //var entObj =input.MapTo<CaseVersion>();
                var entObj = _CaseVersionCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new CaseVersion();
                entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
                //var entObj= AutoMapper.Mapper.Map<CaseVersion>(input);
                var id = _CaseVersionCase.InsertAndGetId(entObj);

                return entObj.MapTo<CaseVersionModel>();

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
        public void DeleteCaseVersion(EntityDto<long> input)
        {
            try
            {
                _CaseVersionCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取版本json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetCaseVersionTreeJson()
        {
            var objList= _CaseVersionCase.GetAllList();
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
        public List<SelectListItem> CaseVersionList()
        {
            var objList = _CaseVersionCase.GetAllList();
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

        public CaseVersionModel GetCaseVersionByFolder(long folderId)
        {
            //var CaseVersion = _CaseVersionCase.FirstOrDefault(p => p.FolderId == folderId);
            //if (CaseVersion != null)
            //    return CaseVersion.MapTo<CaseVersionModel>();
            //else
                return null;

        }


        #endregion
    }
}
