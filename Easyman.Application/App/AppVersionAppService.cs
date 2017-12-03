using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Easyman.App.Dto;
using Easyman.Authorization;
using Easyman.Domain;
using EasyMan;
using EasyMan.Dtos;

namespace Easyman.App
{
    /// <summary>
    /// APP版本管理服务
    /// </summary>
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    public class AppVersionAppService : EasymanAppServiceBase, IAppVersionAppService
    {
        private readonly IRepository<AppVersion, long> _versionRepository;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="versionRepository"></param>
        public AppVersionAppService(IRepository<AppVersion, long> versionRepository)
        {
            _versionRepository = versionRepository;
        }

        /// <summary>
        /// 获取所有版本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public AppVersionSearchOutput GetAppVersionSearch(AppVersionSearchInput input)
        {
            var rowCount = 0;
            var versions = _versionRepository.GetAll().SearchByInputDto(input, out rowCount);
            var outPut = new AppVersionSearchOutput
            {
                Datas = versions.ToList().Select(s => s.MapTo<AppVersionOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        /// <summary>
        /// 获取版本详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AppVersion GetAppVersion(long id)
        {
            return _versionRepository.FirstOrDefault(v => v.Id == id);
        }

        /// <summary>
        /// 修改版本信息
        /// </summary>
        /// <param name="input"></param>
        [UnitOfWork]
        public void SaveAppVersionEdit(AppVersionInput input)
        {
            var versionCode = Convert.ToInt32(input.VersionCode);

            if (_versionRepository.GetAll().Any(v => v.Id != input.Id && v.VersionCode == versionCode))
            {
                throw new Exception("版本信息重复");
            }

            var version = _versionRepository.FirstOrDefault(v => v.Id == input.Id) ?? new AppVersion();
            version.VersionCode = versionCode;
            version.VersionName = input.VersionName;
            version.Type = input.Type;
            version.UpgradeLog = input.UpgradeLog;
            version.UpdateUrl = input.UpdateUrl;
            version.UpdateTime = DateTime.Now;
            version.IsNew = input.IsNew ? 1 : 0;
            version.IsMust = input.IsMust ? 1 : 0;
            version.FileId = input.FileId;

            _versionRepository.InsertOrUpdate(version);

            CurrentUnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 删除版本信息
        /// </summary>
        /// <param name="input"></param>
        public void DeleteAppVersion(EntityDto<long> input)
        {
            _versionRepository.Delete(x => x.Id == input.Id);
        }
    }
}
