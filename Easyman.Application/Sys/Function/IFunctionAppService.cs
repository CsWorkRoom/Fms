using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Domain;
using Easyman.Dto;
using System.Collections.Generic;

namespace Easyman.Sys
{
    public interface IFunctionAppService : IApplicationService
    {
        #region 查询
        Function GetFunction(int id);

        IEnumerable<Function> GetAll();

        Function GetFunction(string code);

        FunctionSerachOutput GetFunsSearch(FunctionSerachInput input);
        #endregion

        #region 操作（新增，编辑，删除）
        void SavePost(FunctionInput input);

        void DeletePost(EntityDto input);

        List<string> GetFuncs(string roleId);

        List<FuncModel> GetAllFuncs();

        bool Checkin(string code);

        #endregion

        #region 其他
        #endregion

    }
}
