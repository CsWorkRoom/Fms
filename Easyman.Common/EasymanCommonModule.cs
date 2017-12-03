using Abp.Modules;
using Easyman.Common.Data;
using Easyman.Common.ViewEngine;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Easyman.Common
{
    public class EasymanCommonModule : AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            if (HostingEnvironment.IsHosted)
            {
                var easymanProvider = new EasyManViewVirtualPathProvider();
                HostingEnvironment.RegisterVirtualPathProvider(easymanProvider);
            }



            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new EasyManViewEngine());
            DatabaseRegister.InitDataBase();
        }
    }
}
