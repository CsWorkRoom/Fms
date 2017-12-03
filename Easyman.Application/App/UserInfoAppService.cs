using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Easyman.App.Dto;
using Easyman.Authorization;
using Easyman.Authorization.Roles;
using Easyman.Common.Helper;
using Easyman.Domain;
using Easyman.Users;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Data;
using Easyman.Common;
using System.Text.RegularExpressions;

namespace Easyman.App
{
    /// <summary>
    /// 用户信息服务
    /// </summary>
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserInfoAppService : EasymanAppServiceBase, IUserInfoAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<Module, long> _moduleRepository;
        private readonly IRepository<RoleModule, long> _roleModuleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="moduleRepository"></param>
        /// <param name="roleModuleRepository"></param>
        /// <param name="userRoleRepository"></param>
        public UserInfoAppService(
            IRepository<User, long> userRepository, 
            IRepository<Role, int> roleRepository,
            IRepository<Module, long> moduleRepository,
            IRepository<RoleModule, long> roleModuleRepository,
            IRepository<UserRole, long> userRoleRepository
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _roleModuleRepository = roleModuleRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiUserBean UserSingle(ApiRequestEntityBean request)
        {
            var userId = request.userId;
            var token = request.authToken;
            var userInfo = _userRepository.FirstOrDefault(u => u.Id == userId);
            var roleIds = userInfo.Roles.Select(r => r.RoleId).ToList();

            var roleNameList = new List<string>();

            if (roleIds.Count > 0)
            {
                roleNameList = _roleRepository.GetAll().Where(r => roleIds.Contains(r.Id))
                    .Select(s => s.Name).ToList();
            }

            var retUser = new ApiUserBean
            {
                authToken = token,
                id = userId,
                name = userInfo.Name,
                phoneNumber = userInfo.PhoneNumber,
                iconURL = string.Empty,
                //belonging = userInfo.Department.Name,
                role = string.Join(",", roleNameList),
                distictId = Convert.ToInt32(userInfo.District.Id),
                distictName = userInfo.District.Name,
                distictLevel = userInfo.District.CurLevel.Value,
            };

            return retUser;
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiUserBean UserSave(ApiRequestSaveEntityBean<ApiUserBean> request)
        {
            var userId = request.userId;
            var token = request.authToken;
            var entity = request.entity;
            var userInfo = _userRepository.FirstOrDefault(u => u.Id == userId);
            var roleIds = userInfo.Roles.Select(r => r.RoleId).ToList();
            var roleList = _roleRepository.GetAll().Where(r => roleIds.Contains(r.Id)).ToList();

            userInfo.PhoneNumber = entity.phoneNumber;
            userInfo.Name = entity.name;

            _userRepository.Update(userInfo);

            return entity;
        }

        private String ValidateComplex(string pwd)
        {
            string result = "";

            //进行密码复杂度的校验
            var pwdRule = OperateSection.GetPwdRuleSection();
            if (pwdRule.IsValidatecComplex)//是否启用复杂度校验
            {
                var complexList = OperateSection.GetPwdComplexSetList();
                if (complexList != null && complexList.Count > 0)
                {
                    foreach (var complex in complexList)
                    {
                        if (Regex.IsMatch(pwd, complex.Regular))
                        {
                            result += complex.ErrorMsg + "\r\n";
                        }
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiErrorBean UserEditPwd(ApiRequestSaveEntityBean<ApiKeyValueBean> request)
        {
            var errorInfo = new ApiErrorBean();
            var userId = request.userId;
            var oldPwd = EncryptHelper.AesDecrpt(request.entity.key);
            var newPwd = EncryptHelper.AesDecrpt(request.entity.value);

            try
            {
                DataTable dt = DbHelper.ExecuteGetTable("select id,PASSWORD from ABP_USERS where id='" + userId + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    object uid = dt.Rows[0]["ID"];
                    if (uid != null && Convert.ToInt32(uid) > 0)
                    {
                        #region 验证旧密码
                        var hashHandler = new PasswordHasher();
                        var verifiedResult = hashHandler.VerifyHashedPassword(dt.Rows[0]["PASSWORD"].ToString(), oldPwd);
                        //旧密码不匹配
                        if (!verifiedResult.Equals(PasswordVerificationResult.Success))
                        {
                            errorInfo.isError = true;
                            errorInfo.code = 3;
                            errorInfo.message = "旧密码错误";

                            return errorInfo;
                        }
                        #endregion

                        #region 密码复杂度
                        string resComplex = ValidateComplex(newPwd);
                        if (!string.IsNullOrEmpty(resComplex) && resComplex.Length > 0)
                        {
                            errorInfo.isError = true;
                            errorInfo.code = 3;
                            errorInfo.message = "密码复杂度不够:\r\n" + resComplex;
                            return errorInfo;
                        }
                        #endregion

                        Common.DbHelper.Execute("update ABP_USERS set PASSWORD='" + hashHandler.HashPassword(newPwd) + "' where ID=" + uid);
                    }
                }
                else
                {
                    errorInfo.isError = true;
                    errorInfo.code = 2;
                    errorInfo.message = "当前用户不存在";
                    return errorInfo;
                }
            }
            catch (Exception ex)
            {
                errorInfo.isError = true;
                errorInfo.code = 2;
                errorInfo.message = "异常错误:" + ex.Message;
                return errorInfo;
            }

            errorInfo.isError = false;
            errorInfo.code = 0;
            errorInfo.message = "修改成功";
            return errorInfo;
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiUserBean GetUserLoginInfo(ApiRequestEntityBean request)
        {
            var userInfo = _userRepository.FirstOrDefault(u => u.Id == request.userId);

            if (userInfo == null)
            {
                //var errInfo = new ApiErrorBean
                //{
                //    isError = true,
                //    message = "用户信息不存在"
                //};

                //return JsonConvert.SerializeObject(errInfo);
                return null;
            }

            //var roleIdList = _userRoleRepository.GetAll().Where(ur => ur.UserId == userInfo.Id)
            //    .Select(s => s.RoleId).ToList();
            var roleIdList = userInfo.Roles.Select(r => r.RoleId).ToList();

            var roleNameList = new List<string>();

            if (roleIdList.Count > 0)
            {
                roleNameList = _roleRepository.GetAll().Where(r => roleIdList.Contains(r.Id))
                    .Select(s => s.Name).ToList();
            }

            var userBean = new ApiUserBean
            {
                id = request.userId,
                name = userInfo.Name,
                phoneNumber = userInfo.PhoneNumber,
                distictId = Convert.ToInt32(userInfo.District.Id),
                distictName = userInfo.District.Name,
                distictLevel = userInfo.District.CurLevel.Value,
                role = string.Join(",", roleNameList),
                menu = GetUserMenus(roleIdList)
            };

            //return JsonConvert.SerializeObject(userBean);
            return userBean;
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <returns></returns>
        private List<ApiMenuBean> GetUserMenus(List<int> roleIdList)
        {
            if (roleIdList.Count <= 0)
            {
                return null;
            }

            var allMenuBean = new List<ApiMenuBean>();
            var roleIds = roleIdList.Select(id => (long)id).ToList();

            var allMenu = from rm in _roleModuleRepository.GetAll()
                          join m in _moduleRepository.GetAll() on rm.ModuleId equals m.Id
                          where roleIds.Contains(rm.RoleId)
                          && (m.IsUse == null || (m.IsUse != null && m.IsUse.Value))
                          && m.ApplicationType.Equals("APP")
                          select m;

            foreach (var menu in allMenu.Where(m => m.ParentId == null).OrderBy(o => o.ShowOrder).ToList())
            {
                var menuId = menu.Id;
                var apiMenu = new ApiMenuBean
                {
                    ID = menuId,
                    ICON = menu.Icon,
                    NAME = menu.Name,
                    TYPE = menu.Type == 0 ? "click" : "view",
                    Key = menu.Code,
                    URL = menu.Url,
                    SHOW_ORDER = menu.ShowOrder ?? 0,
                    child = GetSubMenu(allMenu, menuId),
                };

                allMenuBean.Add(apiMenu);
            }

            return allMenuBean;
        }

        /// <summary>
        /// 递归获取子菜单
        /// </summary>
        /// <param name="allMenu"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        private List<ApiMenuBean> GetSubMenu(IQueryable<Module> allMenu, long menuId)
        {
            var allSubMenu = new List<ApiMenuBean>();

            foreach (var subMenu in allMenu.Where(s => s.ParentId == menuId).OrderBy(o => o.ShowOrder).ToList())
            {
                var newBean = new ApiMenuBean
                {
                    ID = subMenu.Id,
                    ICON = subMenu.Icon,
                    NAME = subMenu.Name,
                    TYPE = subMenu.Type == 0 ? "click" : "view",
                    Key = subMenu.Code,
                    URL = subMenu.Url,
                    SHOW_ORDER = subMenu.ShowOrder ?? 0,
                    child = GetSubMenu(allMenu, subMenu.Id),
                };

                allSubMenu.Add(newBean);
            }

            return allSubMenu;
        }
    }
}
