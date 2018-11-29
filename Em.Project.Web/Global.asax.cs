using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using System.Web;
using Abp.PlugIns;
using Easyman.Web;
using System.IO;
using System.Configuration;

//[assembly: PreApplicationStartMethod(typeof(PreStarter), "Start")]
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

        /// <summary>
        /// 发布网站的根目录（URL中的应用程序目录）
        /// </summary>
        public static string ApplicationPath
        {
            get { return Easyman.Common.Mvc.EasyManController.ApplicationPath; }
        }

        /// <summary>
        /// 获得BD备份目录
        /// </summary>
        public static string DbPath
        {
            get { return ConfigurationManager.AppSettings["DbPath"].ToString(); }
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
