using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Threading;
using Abp.UI;
using Abp.Web.Models;
using Easyman.Authorization;
using Easyman.Authorization.Roles;
using Easyman.MultiTenancy;
using Easyman.Users;
using Easyman.Domain;
using Easyman.Web.Controllers.Results;
using Easyman.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Abp.Auditing;
using Abp.Domain.Repositories;
using Easyman.App;
using Easyman.App.Dto;
using Easyman.Common.Helper;
using Em.Project.Common.Helper;
using Newtonsoft.Json;
using Easyman.Common;
using System.Text.RegularExpressions;
using Easyman.Sys;

namespace Easyman.Web.Controllers
{
    public class AccountController : EasymanControllerBase
    {
        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly LogInManager _logInManager;
        private readonly IUserInfoAppService _userInfoAppService;
        private readonly IUserPwdAppService _userPwdAppService;
        private readonly UserStore _userStore;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IUnitOfWorkManager unitOfWorkManager,
            IMultiTenancyConfig multiTenancyConfig,
            LogInManager logInManager,
            IUserInfoAppService userInfoAppService,
            IUserPwdAppService userPwdAppService)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWorkManager = unitOfWorkManager;
            _multiTenancyConfig = multiTenancyConfig;
            _logInManager = logInManager;
            _userInfoAppService = userInfoAppService;
            _userPwdAppService = userPwdAppService;
        }

        #region Login / Logout

        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            return View(
                new LoginFormViewModel
                {
                    ReturnUrl = returnUrl,
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled
                });
        }

        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            err.IsError = false;
            try
            {
                CheckModelState();
                // 对密码进行AES解密
                loginModel.Password = EncryptHelper.AesDecrpt(loginModel.Password);

                var verifyCode = loginModel.VerifyCode;
                var isMatch = ToolHelper.MatchVerifyCode(verifyCode);

                if (!isMatch)
                {
                    //throw new UserFriendlyException("登录失败", "验证码错误");
                    throw new Exception("登录失败：验证码错误！");
                }

                var loginResult = await GetLoginResultAsync(
                           loginModel.UsernameOrEmailAddress,
                           loginModel.Password,
                           loginModel.TenancyName
                           );

                ValidateCycleAndComplex(loginModel, loginResult);//密码复杂度和周期校验

                await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);

                #region // 原-生成页面水印
                //var systemName = ConfigurationManager.AppSettings["SysName"];
                //var waterMark = ToolHelper.CreateWatermark(systemName, loginResult.User.UserName);

                //var filePath = AppDomain.CurrentDomain.BaseDirectory + "/UpFiles/Bg/";

                //if (!Directory.Exists(filePath))
                //{
                //    Directory.CreateDirectory(filePath);
                //}

                //var fileFullName = filePath + loginResult.User.Id + ".jpg";
                //System.IO.File.WriteAllBytes(fileFullName, waterMark);
                #endregion

                if (string.IsNullOrWhiteSpace(returnUrl) || returnUrl == @"/")
                {
                    //returnUrl = Request.ApplicationPath;
                    returnUrl = Url.Content("~/Home/Index");
                }

                if (!string.IsNullOrWhiteSpace(returnUrlHash))
                {
                    returnUrl = returnUrl + returnUrlHash;
                }
                err.IsError = false;
                err.Message = returnUrl;
                return Json(err);
                //return Json(new AjaxResponse { TargetUrl = returnUrl });
            }
            catch (Exception e)
            {
                err.IsError = true;
                err.Message = e.Message;
                err.Excep = e;
                return Json(err);
            }
        }

        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> AppLogin(ApiLoginBean loginBean)
        {
            CheckModelState();

            // 对密码进行AES解密
            loginBean.username = EncryptHelper.AesDecrpt(loginBean.username);
            loginBean.password = EncryptHelper.AesDecrpt(loginBean.password);

            var loginModel = new LoginViewModel
            {
                UsernameOrEmailAddress = loginBean.username,
                Password = loginBean.password,
                TenancyName = loginBean.tenancyname
            };

            var loginResult = await GetLoginResultAsync(
            loginModel.UsernameOrEmailAddress,
            loginModel.Password,
            loginModel.TenancyName
            );

            ValidateCycleAndComplex(loginModel, loginResult);//密码复杂度和周期校验

            await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);

            var userBean = new ApiUserBean
            {
                id = loginResult.User.Id,
                name = loginResult.User.UserName,
            };

            //return Json(userBean);
            return Json(EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(userBean)));
        }

        #region 登录的私有方法
        /// <summary>
        /// cs于2017.11.22上午进行了修改
        /// 验证登录信息，返回对应登录结果数据
        /// </summary>
        /// <param name="usernameOrEmailAddress"></param>
        /// <param name="password"></param>
        /// <param name="tenancyName"></param>
        /// <returns></returns>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);
            var pwdRule = OperateSection.GetPwdRuleSection();

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    if (pwdRule.IsTrialError)//若启用了试错，则重置错误信息
                    {
                        if (!string.IsNullOrEmpty(pwdRule.TrialErrorCount) && loginResult.User.LoginFailCount > Convert.ToInt32(pwdRule.TrialErrorCount.Trim()))
                        {
                            throw new Exception("登录失败：当前用户（" + usernameOrEmailAddress + ")已被锁定！");
                        }
                        else
                        {
                            //var user = loginResult.User;
                            //user.LoginFailCount = 0;
                            //user.LockedReason = "";
                            //user.IsActive = true;
                            //_userManager.Update(user);
                            DbHelper.Execute("UPDATE ABP_USERS SET LOGIN_FAIL_COUNT=0,LOCKED_REASON='',IS_ACTIVE=1 WHERE ID=" + loginResult.User.Id);
                        }
                    }
                    return loginResult;
                case AbpLoginResultType.InvalidPassword://当密码错误时
                    //先验证是否启用了试错
                    if (pwdRule.IsTrialError)//启用了试错
                    {
                        if (!string.IsNullOrEmpty(pwdRule.TrialErrorCount) && loginResult.User.LoginFailCount > Convert.ToInt32(pwdRule.TrialErrorCount.Trim()))
                        {
                            throw new Exception("登录失败：当前用户（" + usernameOrEmailAddress + ")已被锁定！");
                        }
                        else if (!string.IsNullOrEmpty(pwdRule.TrialErrorCount) && loginResult.User.LoginFailCount <= Convert.ToInt32(pwdRule.TrialErrorCount.Trim()))
                        {
                            string exceptionMsg = "";
                            //去累加失败次数，锁定用户
                            var user = loginResult.User;
                            user.LoginFailCount = user.LoginFailCount + 1;
                            //当累计错误大于试错数时
                            if (user.LoginFailCount > Convert.ToInt32(pwdRule.TrialErrorCount.Trim()))
                            {
                                exceptionMsg = "当前用户（" + usernameOrEmailAddress + "）密码已累计输错【" + pwdRule.TrialErrorCount + "】次，帐号已被锁定!";
                                user.IsActive = false;//锁定用户
                            }
                            else
                            {
                                exceptionMsg = "当前用户（" + usernameOrEmailAddress + "）密码已累计输错【" + user.LoginFailCount + "】次，累计输错【" + pwdRule.TrialErrorCount + "】次将被锁定!";
                            }
                            user.LockedReason = exceptionMsg;
                            //_userManager.Update(user);
                            //CurrentUnitOfWork.SaveChanges();
                            DbHelper.Execute(string.Format(@"UPDATE ABP_USERS SET LOGIN_FAIL_COUNT={0},LOCKED_REASON='{1}',IS_ACTIVE=1 WHERE ID={2}",
                                user.LoginFailCount, exceptionMsg, user.Id));
                            throw new Exception("登录失败：" + exceptionMsg);
                        }
                        else
                        {
                            throw new Exception("登录失败：用户或密码错误（系统未配置试错次数）");
                        }
                    }
                    else
                        throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }
        /// <summary>
        /// 写入登录数据（登录验证通过后，调用该方法）
        /// </summary>
        /// <param name="user"></param>
        /// <param name="identity"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    //return new UserFriendlyException("登录失败", "用户名或密码错误");
                    return new Exception("登录失败：用户名或密码错误！");
                case AbpLoginResultType.InvalidTenancyName:
                    //return new UserFriendlyException("登录失败", "当前租户（" + tenancyName + ")不存在");
                    return new Exception("登录失败：当前租户（" + tenancyName + ")不存在！");
                case AbpLoginResultType.TenantIsNotActive:
                    //return new UserFriendlyException("登录失败", "当前租户（" + tenancyName + ")无效");
                    return new Exception("登录失败：当前租户（" + tenancyName + ")无效！");
                case AbpLoginResultType.UserIsNotActive:
                    //return new UserFriendlyException("登录失败", "当前用户（" + usernameOrEmailAddress + ")无效，无法登录");
                    return new Exception("登录失败：当前用户（" + usernameOrEmailAddress + ")无效，可能被锁定，无法登录！");
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    //return new UserFriendlyException("登录失败", "您的邮箱还未确认，无法登录"); //TODO: localize message
                    return new Exception("登录失败：您的邮箱还未确认，无法登录！");
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    //return new UserFriendlyException("登录失败");
                    return new Exception("登录失败：" + result + "！");
            }
        }

        /// <summary>
        /// 验证密码复杂度（不能为默认密码）和密码修改周期
        /// </summary>
        /// <param name="loginModel"></param>
        /// <param name="loginResult"></param>
        private void ValidateCycleAndComplex(LoginViewModel loginModel, AbpLoginResult<Tenant, User> loginResult)
        {
            //进行密码复杂度的校验
            var pwdRule = OperateSection.GetPwdRuleSection();
            if (pwdRule.IsValidatecComplex)//是否启用复杂度校验
            {
                var complexList = OperateSection.GetPwdComplexSetList();
                if (complexList != null && complexList.Count > 0)
                {
                    foreach (var complex in complexList)
                    {
                        if (Regex.IsMatch(loginModel.Password, complex.Regular))
                        {
                            throw new Exception("密码复杂度不够：" + complex.ErrorMsg + "。即将跳入密码修改页面...");
                        }
                    }
                }
                if (pwdRule.DefualtPwd == loginModel.Password)
                {
                    throw new Exception("不能为系统默认密码。即将跳入密码修改页面...");
                }
            }

            //密码周期性验证
            if (pwdRule.IsCycle)//启用了周期性验证
            {
                var dd = _userPwdAppService.GetAllPwdLog(loginResult.User.Id);
                var lastPwdLog = _userPwdAppService.GetLastPwdLog(loginResult.User.Id);
                if (lastPwdLog != null)
                {
                    if (!string.IsNullOrEmpty(pwdRule.CycleTime) && Convert.ToInt32(pwdRule.CycleTime) < DateTime.Now.Subtract(lastPwdLog.CreationTime).Duration().Days)
                    {
                        throw new Exception("当前密码累计使用已超过【" + pwdRule.CycleTime + "】天，请修改密码。即将跳入密码修改页面...");
                    }
                }
            }
        }

        #endregion


        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        #endregion

        #region Register

        public ActionResult Register()
        {
            return RegisterView(new RegisterViewModel());
        }

        private ActionResult RegisterView(RegisterViewModel model)
        {
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return View("Register", model);
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                CheckModelState();

                //Get tenancy name and tenant
                if (!_multiTenancyConfig.IsEnabled)
                {
                    model.TenancyName = Tenant.TenantName;
                }
                else if (model.TenancyName.IsNullOrEmpty())
                {
                    throw new UserFriendlyException(L("TenantNameCanNotBeEmpty"));
                }

                var tenant = await GetActiveTenantAsync(model.TenancyName);

                //Create user
                var user = new User
                {
                    TenantId = tenant.Id,
                    Name = model.Name,
                    Surname = model.Surname,
                    EmailAddress = model.EmailAddress,
                    IsActive = true
                };

                //Get external login info if possible
                ExternalLoginInfo externalLoginInfo = null;
                if (model.IsExternalLogin)
                {
                    externalLoginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (externalLoginInfo == null)
                    {
                        throw new ApplicationException("Can not external login!");
                    }

                    user.Logins = new List<UserLogin>
                    {
                        new UserLogin
                        {
                            TenantId = tenant.Id,
                            LoginProvider = externalLoginInfo.Login.LoginProvider,
                            ProviderKey = externalLoginInfo.Login.ProviderKey
                        }
                    };

                    if (model.UserName.IsNullOrEmpty())
                    {
                        model.UserName = model.EmailAddress;
                    }

                    model.Password = Users.User.CreateRandomPassword();

                    if (string.Equals(externalLoginInfo.Email, model.EmailAddress, StringComparison.InvariantCultureIgnoreCase))
                    {
                        user.IsEmailConfirmed = true;
                    }
                }
                else
                {
                    //Username and Password are required if not external login
                    if (model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                    {
                        throw new UserFriendlyException(L("FormIsNotValidMessage"));
                    }
                }

                user.UserName = model.UserName;
                user.Password = new PasswordHasher().HashPassword(model.Password);

                //Switch to the tenant
                _unitOfWorkManager.Current.EnableFilter(AbpDataFilters.MayHaveTenant); //TODO: Needed?
                _unitOfWorkManager.Current.SetTenantId(tenant.Id);

                //Add default roles
                user.Roles = new List<UserRole>();
                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    user.Roles.Add(new UserRole { RoleId = defaultRole.Id });
                }

                //Save user
                CheckErrors(await _userManager.CreateAsync(user));
                await _unitOfWorkManager.Current.SaveChangesAsync();

                //Directly login if possible
                if (user.IsActive)
                {
                    AbpLoginResult<Tenant, User> loginResult;
                    if (externalLoginInfo != null)
                    {
                        loginResult = await _logInManager.LoginAsync(externalLoginInfo.Login, tenant.TenancyName);
                    }
                    else
                    {
                        loginResult = await GetLoginResultAsync(user.UserName, model.Password, tenant.TenancyName);
                    }

                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        await SignInAsync(loginResult.User, loginResult.Identity);
                        return Redirect(Url.Action("Index", "Home"));
                    }

                    Logger.Warn("New registered user could not be login. This should not be normally. login result: " + loginResult.Result);
                }

                //If can not login, show a register result page
                return View("RegisterResult", new RegisterResultViewModel
                {
                    TenancyName = tenant.TenancyName,
                    NameAndSurname = user.Name + " " + user.Surname,
                    UserName = user.UserName,
                    EmailAddress = user.EmailAddress,
                    IsActive = user.IsActive
                });
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
                ViewBag.ErrorMessage = ex.Message;

                return View("Register", model);
            }
        }

        #endregion

        #region External Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(
                provider,
                Url.Action(
                    "ExternalLoginCallback",
                    "Account",
                    new
                    {
                        ReturnUrl = returnUrl
                    })
                );
        }

        [UnitOfWork]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl, string tenancyName = "")
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            //Try to find tenancy name
            if (tenancyName.IsNullOrEmpty())
            {
                var tenants = await FindPossibleTenantsOfUserAsync(loginInfo.Login);
                switch (tenants.Count)
                {
                    case 0:
                        return await RegisterView(loginInfo);
                    case 1:
                        tenancyName = tenants[0].TenancyName;
                        break;
                    default:
                        return View("TenantSelection", new TenantSelectionViewModel
                        {
                            Action = Url.Action("ExternalLoginCallback", "Account", new { returnUrl }),
                            Tenants = tenants.MapTo<List<TenantSelectionViewModel.TenantInfo>>()
                        });
                }
            }

            var loginResult = await _logInManager.LoginAsync(loginInfo.Login, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    await SignInAsync(loginResult.User, loginResult.Identity, false);

                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        returnUrl = Url.Action("Index", "Home");
                    }

                    return Redirect(returnUrl);
                case AbpLoginResultType.UnknownExternalLogin:
                    return await RegisterView(loginInfo, tenancyName);
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, loginInfo.Email ?? loginInfo.DefaultUserName, tenancyName);
            }
        }

        private async Task<ActionResult> RegisterView(ExternalLoginInfo loginInfo, string tenancyName = null)
        {
            var name = loginInfo.DefaultUserName;
            var surname = loginInfo.DefaultUserName;

            var extractedNameAndSurname = TryExtractNameAndSurnameFromClaims(loginInfo.ExternalIdentity.Claims.ToList(), ref name, ref surname);

            var viewModel = new RegisterViewModel
            {
                TenancyName = tenancyName,
                EmailAddress = loginInfo.Email,
                Name = name,
                Surname = surname,
                IsExternalLogin = true
            };

            if (!tenancyName.IsNullOrEmpty() && extractedNameAndSurname)
            {
                return await Register(viewModel);
            }

            return RegisterView(viewModel);
        }

        [UnitOfWork]
        protected virtual async Task<List<Tenant>> FindPossibleTenantsOfUserAsync(UserLoginInfo login)
        {
            List<User> allUsers;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                allUsers = await _userManager.FindAllAsync(login);
            }

            return allUsers
                .Where(u => u.TenantId != null)
                .Select(u => AsyncHelper.RunSync(() => _tenantManager.FindByIdAsync(u.TenantId.Value)))
                .ToList();
        }

        private static bool TryExtractNameAndSurnameFromClaims(List<Claim> claims, ref string name, ref string surname)
        {
            string foundName = null;
            string foundSurname = null;

            var givennameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (givennameClaim != null && !givennameClaim.Value.IsNullOrEmpty())
            {
                foundName = givennameClaim.Value;
            }

            var surnameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
            if (surnameClaim != null && !surnameClaim.Value.IsNullOrEmpty())
            {
                foundSurname = surnameClaim.Value;
            }

            if (foundName == null || foundSurname == null)
            {
                var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (nameClaim != null)
                {
                    var nameSurName = nameClaim.Value;
                    if (!nameSurName.IsNullOrEmpty())
                    {
                        var lastSpaceIndex = nameSurName.LastIndexOf(' ');
                        if (lastSpaceIndex < 1 || lastSpaceIndex > (nameSurName.Length - 2))
                        {
                            foundName = foundSurname = nameSurName;
                        }
                        else
                        {
                            foundName = nameSurName.Substring(0, lastSpaceIndex);
                            foundSurname = nameSurName.Substring(lastSpaceIndex);
                        }
                    }
                }
            }

            if (!foundName.IsNullOrEmpty())
            {
                name = foundName;
            }

            if (!foundSurname.IsNullOrEmpty())
            {
                surname = foundSurname;
            }

            return foundName != null && foundSurname != null;
        }

        #endregion

        #region Common private methods

        private async Task<Tenant> GetActiveTenantAsync(string tenancyName)
        {
            var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIsNotActive", tenancyName));
            }

            return tenant;
        }

        #endregion

        public ActionResult ModifyPassword()
        {
            var uname = Request["username"];
            if (!string.IsNullOrEmpty(uname))
            {
                object user_id = 0;
                try
                {
                    user_id = DbHelper.ExecuteScalar("select max(id) from ABP_USERS where USER_NAME='" + uname + "'");
                }
                catch { }
                ViewData["UserId"] = user_id;
            }
            else if (AbpSession.UserId != null)
            {
                ViewData["UserId"] = AbpSession.UserId;
            }
            else
                throw new UserFriendlyException("未传入用户信息！");

            return View();
        }

        public JsonResult SaveModifiedPwd(ModifyPwdViewModel modifyModel)
        {
            var errorInfo = new ApiErrorBean();

            // 确认密码不匹配
            if (!modifyModel.NewPassword.Trim().Equals(modifyModel.ConfirmPassword.Trim()))
            {
                errorInfo.isError = true;
                errorInfo.code = 1;
                errorInfo.message = "两次新密码输入不一致，请重新输入！";

                return Json(errorInfo);
            }

            var keyValue = new ApiKeyValueBean
            {
                key = modifyModel.OldPassword,
                value = modifyModel.NewPassword
            };

            var savePara = new ApiRequestSaveEntityBean<ApiKeyValueBean>
            {
                userId = modifyModel.UserId,
                entity = keyValue
            };

            // 保存修改密码
            errorInfo = _userInfoAppService.UserEditPwd(savePara);

            if(!errorInfo.isError)//写入密码修改记录
            {
                _userPwdAppService.InsertUserPwdLog(new Dto.UserPwdLogDto
                {
                    NewPwd = EncryptHelper.AesDecrpt(modifyModel.NewPassword),
                    OldPwd = EncryptHelper.AesDecrpt(modifyModel.OldPassword),
                    UserId = modifyModel.UserId
                });
            }

            return Json(errorInfo);
        }

        public void GetVerifyCode()
        {
            ToolHelper.GenerateVerifyCode();
        }
    }
}