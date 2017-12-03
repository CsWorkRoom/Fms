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
    }
}
