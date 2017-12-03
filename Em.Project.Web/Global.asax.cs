using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using System.Web;
using Abp.PlugIns;
using Easyman.Web;
using System.IO;

[assembly: PreApplicationStartMethod(typeof(PreStarter), "Start")]
namespace Easyman.Web
{
    public class MvcApplication : AbpWebApplication<EasymanWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );

            //RegisterGlobalFilters(System.Web.Mvc.GlobalFilters.Filters);
            base.Application_Start(sender, e);
        }
        public static void RegisterGlobalFilters(System.Web.Mvc.GlobalFilterCollection filters)
        {
            filters.Add(new System.Web.Mvc.HandleErrorAttribute());
            //filters.Add(new Common.Mvc.ValidateUrAttributel());
        }
    }

    //PlugIn Modules 
    public static class PreStarter
    {
        public static void Start()
        {
            //先注释
            //string pluginPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            //MvcApplication.AbpBootstrapper.PlugInSources.AddFolder(pluginPath);
            //MvcApplication.AbpBootstrapper.PlugInSources.AddToBuildManager();
        }
    }
}
