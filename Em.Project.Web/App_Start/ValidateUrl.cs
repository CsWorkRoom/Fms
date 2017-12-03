using Easyman.Managers;
using Easyman.Sys;
using System;
using System.Web.Mvc;

namespace Easyman.Common.Mvc
{
    public class ValidateUrl : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IActionFilter
    {

        #region 执行action后执行这个方法
        /// <summary>
        /// 执行action后执行这个方法
        /// </summary>
        /// <param name="filterContext"></param>
        void System.Web.Mvc.IActionFilter.OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {

        }
        #endregion


        #region 执行action前执行这个方法 
        /// <summary>
        /// 执行action前执行这个方法
        /// </summary>
        /// <param name="filterContext"></param>
        void System.Web.Mvc.IActionFilter.OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            string controllerName= filterContext.RouteData.Values["controller"].ToString().ToLower();
            //string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();//方式一
            string actionName = filterContext.ActionDescriptor.ActionName.ToLower();//方式二

            string url = filterContext.HttpContext.Request.Url.ToString();//url路径
            var curUrl = System.Web.HttpUtility.UrlDecode(url).Substring(url.ToLower().IndexOf(controllerName + "/" + actionName));
            //从ioc容器中获取当前待使用接口的实例
            var _modulesService = Abp.Dependency.IocManager.Instance.Resolve<IModulesAppService>();
            var result = _modulesService.ValidateUrlRole(curUrl);
            if(!result)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "NoAccess" }));  //重定向
                #region 其他写法
                //filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new Dictionary<string, object>() { { "controller", "ActionFilterTest" }, { "action", "Login" } }));   //重定向

                //filterContext.Result = new System.Web.Mvc.RedirectToRouteResult("Default", new System.Web.Routing.RouteValueDictionary(new Dictionary<string, object>() { { "controller", "ActionFilterTest" }, { "action", "Login" } }));    //重定向

                //filterContext.Result = new System.Web.Mvc.RedirectToRouteResult("Default", new System.Web.Routing.RouteValueDictionary(new Dictionary<string, object>() { { "controller", "ActionFilterTest" }, { "action", "Login" } }), true);  //重定向

                //filterContext.Result = new System.Web.Mvc.RedirectToRouteResult("MyRoute", new System.Web.Routing.RouteValueDictionary(new Dictionary<string, object>() { { "controller", "ActionFilterTest" }, { "action", "Login" } }), true);    //重定向
                #endregion
                return;
            }
        }
        #endregion
    }
}
