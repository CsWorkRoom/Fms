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
using Easyman.Common.Helper;
using Easyman.Domain;
using EasyMan;
using EasyMan.Dtos;
using Newtonsoft.Json;

namespace Easyman.App
{
    /// <summary>
    /// App通用功能服务
    /// </summary>
    public class AppCommonAppService : EasymanAppServiceBase, IAppCommonAppService
    {
        private readonly IRepository<AppVersion, long> _versionRepository;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="versionRepository"></param>
        public AppCommonAppService(IRepository<AppVersion, long> versionRepository)
        {
            _versionRepository = versionRepository;
        }

        /// <summary>
        /// 检查版本更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiAppVersionBean CheckUpdate(ApiRequestEntityBean request)
        {
            if (request == null)
            {
                var error = new ApiAppVersionBean
                {
                    Is_Upgrade = false,
                    IS_MUST = false,
                    VERSION_CODE = string.Empty,
                    VERSION_NAME = string.Empty,
                    UPDATE_URL = string.Empty
                };

                return error;
            }

            var type = string.Empty;
            long code = 0;
            var typePara = request.para.SingleOrDefault(x => x.key == "type");
            var codePara = request.para.SingleOrDefault(x => x.key == "versionCode");

            if (typePara != null)
            {
                type = typePara.value;
            }

            if (codePara != null)
            {
                code = Convert.ToInt64(codePara.value);
            }

            if (string.IsNullOrEmpty(type))
            {
                var error = new ApiAppVersionBean
                {
                    Is_Upgrade = false,
                    IS_MUST = false,
                    VERSION_CODE = string.Empty,
                    VERSION_NAME = string.Empty,
                    UPDATE_URL = string.Empty
                };

                return error;
            }

            var hasHigherVersion = false;
            var higherVersion = _versionRepository.GetAll().Where((v => v.VersionCode > code && v.Type == type))
                .OrderByDescending(o => o.VersionCode).ToList();

            if (higherVersion.Count > 0)
            {
                hasHigherVersion = true;
            }

            if (hasHigherVersion)
            {
                var retInfo = new ApiAppVersionBean
                {
                    Is_Upgrade = true,
                    IS_MUST = higherVersion[0].IsMust == 1,
                    VERSION_CODE = higherVersion[0].VersionCode.ToString(),
                    VERSION_NAME = higherVersion[0].VersionName,
                    UPDATE_URL = higherVersion[0].UpdateUrl
                };

                return retInfo;
            }

            var noUpradeInfo = new ApiAppVersionBean
            {
                Is_Upgrade = false,
                IS_MUST = false,
                VERSION_CODE = string.Empty,
                VERSION_NAME = string.Empty,
                UPDATE_URL = string.Empty
            };

            return noUpradeInfo;
        }
    }
}
