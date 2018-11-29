using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using Abp.Authorization.Users;
using Abp.Web.Mvc.Controllers;
using Abp.UI;
using Microsoft.AspNet.Identity;
using EasyMan;
using Abp.IdentityFramework;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Runtime.Session;

namespace Easyman.Common.Mvc
{
    [DependsOn(typeof(AbpWebMvcModule))]
    public class EasyManController : AbpController
    {
        /// <summary>
        /// 发布网站的根目录（URL中的应用程序目录）
        /// </summary>
        public static string ApplicationPath = "";

        protected EasyManController()
        {
            LocalizationSourceName = "EasyMan";
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public ActionResult EasyView(string viewPath)
        {
            return EasyView(viewPath, null);
        }

        public ActionResult EasyView(string viewPath, object model)
        {
            if (!viewPath.HasValue())
            {
                return View(model);
            }
            else
            {

                return View(viewPath, model);
            }

        }
        public long CurrUserId() { return AbpSession.GetUserId(); }

        /// <summary>
        /// 在调用Action方法前，验证访问权限
        /// </summary>
        /// <param name="context"></param>
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            string actionName = context.ActionDescriptor.ActionName.ToLower();
            //不带参数的路径
            string path = path = "/" + context.RequestContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath.ToLower().Trim(new char[] { '~', '/' });
            //BLog.Write(BLog.LogLevel.DEBUG, "path:" + path);
            string rawUrl = context.HttpContext.Request.RawUrl;
            if (Request.ApplicationPath != "/" && ApplicationPath != Request.ApplicationPath)
            {
                ApplicationPath = Request.ApplicationPath;
            }
        }
    }
}
