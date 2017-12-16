using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Domain;
using Easyman.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Easyman.Sys
{
    public interface IDepartmentAppService : IApplicationService
    {
        #region 查询

        Department GetDepartment(long id);

        Department GetDepartment(string code);

        DepartmentSearchOutput GetDpartmentsSearch(DepartmentSearchInput input);

        Task<IEnumerable<object>> GetDepartmentTreeJson();
        #endregion

        #region 操作（新增，编辑，删除）
        void SavePost(DepartmentInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        void DeletePost(EntityDto<long> input);

        #endregion
    }
}
