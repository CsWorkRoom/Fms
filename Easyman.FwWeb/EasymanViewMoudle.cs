using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Resources.Embedded;
using Abp.Web.Mvc;
using Abp.Web.Mvc.Resources;
using Abp.Zero;
using System.Reflection;

namespace Easyman.FwWeb
{
    //[DependsOn(/*typeof(AbpZeroCoreModule),*/ typeof(AbpWebMvcModule))]
    public class EasymanViewMoudle : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.EmbeddedResources.Sources.Add(
                new EmbeddedResourceSet(
                "/Views/",
                Assembly.GetExecutingAssembly(),
                "Easyman.FwWeb.Views"
            ));
            //Configuration.Modules.AbpWebCommon().EmbeddedResources.IgnoredFileExtensions.Clear();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
