using Abp.Application.Services;
using Easyman.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Easyman.Dto;
using Abp.Application.Services.Dto;

namespace Easyman.Sys
{
    public interface IDistrictAppService : IApplicationService
    {
        #region 查询
        /// <summary>
        /// 获取所有组织
        /// </summary>
        /// <returns></returns>
        List<District> GetAllDistrcit();

        District GetDistrict(long id);

        District GetDistrict(string code);

        DistrictSearchOutput GetDpartmentsSearch(DistrictSearchInput input);

        Task<IEnumerable<object>> GetDistrictTreeJson();
        #endregion

        #region 操作（新增，编辑，删除）
        void SavePost(DistrictInput input);

        void DeletePost(EntityDto<long> input);

        #endregion
    }
}
