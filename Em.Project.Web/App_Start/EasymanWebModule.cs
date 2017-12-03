using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.Mvc.Resources;
using Abp.Web.SignalR;
using Abp.WebApi;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.Zero.EntityFramework;
using Easyman;
using Easyman.Api;
using Easyman.Common;
using Easyman.FwWeb;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Easyman.Web
{
    [DependsOn(
        typeof(AbpZeroCoreModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpAutoMapperModule),
        //typeof(AbpHangfireModule), - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(AbpWebMvcModule),
        typeof(AbpZeroEntityFrameworkModule),
        typeof(AbpWebApiModule),

        typeof(EasymanCommonModule),//Easyman.Common
        typeof(EasymanCoreModule),//Easyman.Core
        typeof(EasymanApplicationModule),//Easyman.Application
        typeof(EasymanDataModule),//Easyman.EntityFramework
        typeof(EasymanViewMoudle),//Easyman.FwWeb

        typeof(EmProjectCommonModule),//Em.Project.Common
        typeof(EmProjectCoreModule),//Em.Project.Core
        typeof(EmProjectAppModule),//Em.Project.Application
        typeof(EmProjectDataModule),//Em.Project.EntityFramework
        typeof(EasymanWebApiModule)//Em.Project.WebApi
       )]
    public class EasymanWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<EasymanNavigationProvider>();
            //是否开启验证客户端
            Configuration.Modules.AbpWeb().AntiForgery.IsEnabled = false;

            //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});

            //暴露程序集Easyman.FwWeb下的内嵌资源
            //Configuration.EmbeddedResources.Sources.Add(
            //    new Abp.Resources.Embedded.EmbeddedResourceSet(
            //        "/Views/",
            //        Assembly.Load("Easyman.FwWeb"),
            //         "Easyman.FwWeb.Views"
            //    ));
            //Configuration.Modules.AbpWebCommon().EmbeddedResources.IgnoredFileExtensions.Clear();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            
            //WebResourceHelper.ExposeEmbeddedResources("AbpZero/Metronic", Assembly.GetExecutingAssembly(), "Abp.Zero.Web.UI.Metronic");

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
