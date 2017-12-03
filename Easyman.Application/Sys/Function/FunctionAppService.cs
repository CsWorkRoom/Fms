using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using Easyman.Users;
using EasyMan;
using EasyMan.Common.Data;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easyman.Sys
{
    public class FunctionAppService : EasymanAppServiceBase, IFunctionAppService
    {
        #region 初始化

        private readonly FunctionManager _functionManager;
        private readonly UserManager _userManager;

        #endregion

        #region 公用方法

        public FunctionAppService(FunctionManager FunctionManager, UserManager userManager)
        {
            _functionManager = FunctionManager;
            _userManager = userManager;
        }

        #region 查询
        public Function GetFunction(int id)
        {
            return _functionManager.GetFunction(id);
        }

        public Function GetFunction(string code)
        {
            return _functionManager.GetFunction(code);
        }

        public IEnumerable<Function> GetAll()
        {
            return _functionManager.Query;
        }

        public FunctionSerachOutput GetFunsSearch(FunctionSerachInput input)
        {
            var rowCount = 0;
            var funs = _functionManager.Query.SearchByInputDto(input, out rowCount);
            var outPut = new FunctionSerachOutput
            {
                Datas = funs.ToList().Select(s => s.MapTo<FunctionOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        #endregion

        #region 操作（新增，编辑，删除）

        public void SavePost(FunctionInput input)
        {
            var fun = _functionManager.GetFunction(input.Id) ?? new Function();
            fun.Code = input.Code;
            fun.Name = input.Name;
            fun.Discribition = input.Discribition;
            fun.Type = input.Type;
            fun.CreationTime = DateTime.Now;
            fun.TenantId = input.TenantId.HasValue ? input.TenantId.Value : 1;
            fun.Roles = fun.Roles ?? new List<FunctionRole>();

            var funId = _functionManager.SaveOrUpdateFunction(fun);
            _functionManager.SetRoles(funId, input.RoleIds);
        }

        public void DeletePost(EntityDto input)
        {
            _functionManager.DeleteFungitaion(input.Id);
        }

        #endregion

        //根据code判断功能权限接口
        public bool Checkin(string code)
        {
            long Id = CurrentUser.Id;
            using (var session = DatabaseSession.OpenSession())
            {
                var sql = @"SELECT DISTINCT E.CODE
                            FROM ""AbpUsers"" A
                                   INNER JOIN ""AbpUserRoles"" B
                                      ON A.""Id"" = B.""UserId""
                                   INNER JOIN ""AbpRoles"" C
                                      ON B.""RoleId"" = C.""Id""
                                   LEFT JOIN ""AbpFunctionRole"" D
                                      ON D.""ROLE_ID"" = C.""Id""
                                   INNER JOIN ""AbpFunction"" E
                                      ON D.""FUNCTION_ID"" = E.""ID""
                            WHERE A.""Id""= {0}";

                var resultL = session.Query<string>(sql.FormatWith(Id));
                if (resultL.ToList().Contains(code))
                {
                    return true;
                }
            }
            return false;
        }
        public List<FuncModel> GetAllFuncs()
        {
            var restult = new List<FuncModel>();
            long Id = CurrentUser.Id;
            using (var session = DatabaseSession.OpenSession())
            {
                var sql = @"SELECT DISTINCT E.Id,E.CODE,E.NAME
                            FROM ""AbpFunction"" E 
                            WHERE E.IS_DELETED=0";

                var resultL = session.Query<FuncModel>(sql.FormatWith(Id));
                var FunCs = resultL as FuncModel[] ?? resultL.ToArray();
                return FunCs.ToList();
            }
        }

        public List<string> GetFuncs(string roleId)
        {
            var restult = new List<string>();
            using (var session = DatabaseSession.OpenSession())
            {
                var sql = @"SELECT DISTINCT E.Id
                            FROM ""AbpRoles"" C
                                   LEFT JOIN ""AbpFunctionRole"" D
                                      ON D.""ROLE_ID"" = C.""Id""
                                   INNER JOIN ""AbpFunction"" E
                                      ON D.""FUNCTION_ID"" = E.""ID""
                            WHERE C.""Id""= {0}";

                var resultL = session.Query<string>(sql.FormatWith(roleId));
                var FunCs = resultL as string[] ?? resultL.ToArray();
                return FunCs.ToList();
            }
        }

        #endregion

        #region 私有方法

        private User CurrentUser
        {
            get
            {
                if (AbpSession.UserId == null || !AbpSession.UserId.HasValue)
                {
                    return new User();
                }
                var user = _userManager.GetUserByIdAsync(AbpSession.UserId.Value);

                return user.Result;
            }
        }

        #endregion
    }
}
