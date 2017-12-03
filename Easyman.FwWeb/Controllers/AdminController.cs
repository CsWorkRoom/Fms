using Abp.AutoMapper;
using Abp.Domain.Uow;
using Easyman.Common.Mvc;
using Easyman.Dto;
using Easyman.Service;
using Easyman.Sys;
using Easyman.Users;
using EasyMan;
using EasyMan.Export;
using System;
using System.Web.Mvc;
using Easyman.App;
using Easyman.App.Dto;
using System.Linq.Expressions;
using System.Linq;
using Abp.UI;

namespace Easyman.FwWeb.Controllers
{
    public class AdminController : EasyManController
    {
        #region 初始化

        private readonly IUserAppService _userService;
        private readonly IModulesAppService _modulesService;
        private readonly IRoleAppService _roleAppService;
        private readonly IFunctionAppService _functionAppService;
        private readonly IDepartmentAppService _departmentAppService;
        private readonly IDistrictAppService _districtAppService;
        private readonly IIconAppService _iconAppService;
        //private readonly IIconTypeAppService _iconTypeAppService;
        private readonly IAppVersionAppService _appVersionAppService;

        public AdminController(IDepartmentAppService departmentAppService, IFunctionAppService functionAppService, 
            IUserAppService userService, IModulesAppService modulesService, IRoleAppService roleAppService,
            IIconAppService iconAppService, IAppVersionAppService appVersionAppService,
            IDistrictAppService districtAppService)
        {
            _userService = userService;
            _modulesService = modulesService;
            _roleAppService = roleAppService;
            _functionAppService = functionAppService;
            _departmentAppService = departmentAppService;
            _iconAppService = iconAppService;
            //_iconTypeAppService = iconTypeAppService;
            _appVersionAppService = appVersionAppService;
            _districtAppService = districtAppService;
        }

        #endregion

        #region 用户

        public new ActionResult User()
        {
            return View("Easyman.FwWeb.Views.Admin.User");
        }

        public ActionResult Createuser()
        {
            var model = new UserInput();
            model.TenantId = AbpSession.TenantId;
            return View("Easyman.FwWeb.Views.Admin.Createuser",model);
        }

        [UnitOfWork]
        public ActionResult EditUser(int navId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var user = _userService.GetUser(navId);

                var model = user == null ? new UserInput() : user.MapTo<UserInput>();
                return View("Easyman.FwWeb.Views.Admin.Createuser", model);
            }
        }

        [FileDownload]
        public ActionResult ExportUser(UserSearchInput input)
        {
            var result = _userService.ExportUserBytes(input);

            return File(result, "application/x-xls", "{0}.xlsx".FormatWith("技术管理平台工号"));
        }

        #endregion

        #region WEB菜单

        public ActionResult NavigationPage()
        {
            return View("Easyman.FwWeb.Views.Admin.NavigationPage");
        }

        public ActionResult CreateNavigation()
        {
            var model = new NavigationInput { TenantId = AbpSession.TenantId };
           
            return View("Easyman.FwWeb.Views.Admin.EditNavigation", model);
        }

        public ActionResult NavigationEdit(int navId)
        {
            var nav = _modulesService.GetNavigation(navId);
            var model = nav == null ? new NavigationInput() : nav.MapTo<NavigationInput>();
           
            return View("Easyman.FwWeb.Views.Admin.EditNavigation", model);
        }

        /// <summary>
        /// 校验当前用户访问页面权限（校验继承了母版页的视图页）
        /// 需要迁移至其他地方
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool ValidateUrlRole(string url)
        {
            url = System.Web.HttpUtility.UrlDecode(url);
            if (url.ToLower().Contains("Home/NoAccess".ToLower())) return true;
            if (url.Contains("#")) return true;
            if (url.ToLower().Contains("home/index")) return true;
            if (url.ToLower().Contains("Account/ModifyPassword".ToLower())) return true;
            if (string.IsNullOrEmpty(url)) return true;

            return _modulesService.ValidateUrlRole(url);
        }
        #endregion

        #region APP菜单

        public ActionResult AppMenu()
        {
            return View("Easyman.FwWeb.Views.Admin.AppMenu");
        }

        public ActionResult CreateAppMenu()
        {
            var model = new NavigationInput { TenantId = AbpSession.TenantId };
            return View("Easyman.FwWeb.Views.Admin.EditAppMenu", model);
        }

        public ActionResult AppMenuEdit(int navId)
        {
            var nav = _modulesService.GetNavigation(navId);
            var model = nav == null ? new NavigationInput() : nav.MapTo<NavigationInput>();
            return View("Easyman.FwWeb.Views.Admin.EditAppMenu", model);
        }


        #endregion

        #region APP版本管理

        public ActionResult AppVersion()
        {
            return View("Easyman.FwWeb.Views.Admin.AppVersion");
        }

        public ActionResult CreateAppVersion()
        {
            var model = new AppVersionInput();
            return View("Easyman.FwWeb.Views.Admin.EditAppVersion", model);
        }

        public ActionResult AppVersionEdit(long versionId)
        {
            var version = _appVersionAppService.GetAppVersion(versionId);
            var model = version == null ? new AppVersionInput() : version.MapTo<AppVersionInput>();
            return View("Easyman.FwWeb.Views.Admin.EditAppVersion", model);
        }

        #endregion

        #region 角色

        public ActionResult RolePage()
        {
            return View("Easyman.FwWeb.Views.Admin.RolePage");
        }

        public ActionResult InserRole()
        {
            var model = new RoleInput { TenantId = AbpSession.TenantId };
            return View("Easyman.FwWeb.Views.Admin.EditRole", model);
        }

        public ActionResult EditRole(int roleId)
        {
            var role = _roleAppService.GetRole(roleId);
            var model = role == null ? new RoleInput() : role.MapTo<RoleInput>();
            return View("Easyman.FwWeb.Views.Admin.EditRole", model);
        }

        #endregion

        #region 部门

        public ActionResult InserDepart()
        {
            var model = new DepartmentInput { TenantId = AbpSession.TenantId,IsUse=true };
            return View("Easyman.FwWeb.Views.Admin.EditDepart", model);
        }

        public ActionResult EditDepart(long departId)
        {
            var nav = _departmentAppService.GetDepartment(departId);
            var model = nav == null ? new DepartmentInput() : nav.MapTo<DepartmentInput>();
            return View("Easyman.FwWeb.Views.Admin.EditDepart", model);
        }

        public ActionResult DepartentPage()
        {
            return View("Easyman.FwWeb.Views.Admin.DepartentPage");
        }


        #endregion

        #region 组织
        public ActionResult EditDistrict(long? id)
        {
            var model = new DistrictInput { TenantId = AbpSession.TenantId, IsUse=true };
            if (id!=null&&id!=0)
            {
                var nav = _districtAppService.GetDistrict(id.Value);
                model = nav.MapTo<DistrictInput>();
            }
            return View("Easyman.FwWeb.Views.Admin.EditDistrict", model);
        }
        #endregion

        #region Function

        public ActionResult Function()
        {
            return View("Easyman.FwWeb.Views.Admin.FunctionPage");
        }

        public ActionResult InsertFunction()
        {
            var model = new FunctionInput { TenantId = AbpSession.TenantId };

            return View("Easyman.FwWeb.Views.Admin.EditFunction", model);
        }

        public ActionResult EditFunction(int funId)
        {
            var fun = _functionAppService.GetFunction(funId);
            var model = fun == null ? new FunctionInput() : fun.MapTo<FunctionInput>();
            return View("Easyman.FwWeb.Views.Admin.EditFunction", model);
        }

        #endregion

        #region 图标

        public ActionResult IconPage()
        {
            return View("Easyman.FwWeb.Views.Admin.IconPage");
        }

        public ActionResult EditIcon(long? id)
        {
            var data = new IconInput();
            ViewData["IconType"] = _iconAppService.GetIconType();
            if (id != null)
                data = _iconAppService.Get(id.Value);
            return View("Easyman.FwWeb.Views.Admin.EditIcon", data);
        }
       
        //图标类型
        public ActionResult EditIconType(long? id)
        {
            var data = new IconTypeInput();
            if (id != null)
            {
                data = _iconAppService.GetIconType(id.Value);
               // data = _iconTypeAppService.Get(id.Value);
            }
            return View("Easyman.FwWeb.Views.Admin.EditIconType",data);
        }
       


        #endregion

    }
}
