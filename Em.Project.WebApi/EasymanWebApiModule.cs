using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Easyman;
using Swashbuckle.Application;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Easyman.Api
{
    //[DependsOn(/*typeof(AbpWebApiModule), */typeof(EasymanApplicationModule), typeof(EmProjectAppModule))]
    public class EasymanWebApiModule : AbpModule
    {
        public override void PreInitialize()
        {
            ////关闭跨站脚本攻击
            Configuration.Modules.AbpWeb().AntiForgery.IsEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());


            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(EasymanApplicationModule).Assembly, "api")
                //.WithConventionalVerbs()  //请求类型取决于方法开头 缺省值POST
                .Build();

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
               .ForAll<IApplicationService>(typeof(EmProjectAppModule).Assembly, "api")
               //.WithConventionalVerbs()  //请求类型取决于方法开头 缺省值POST
               .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
            //调用SwaggerUi
            ConfigureSwaggerUi();
        }

        /// <summary>
        /// 配置SwaggerUi
        /// </summary>
        private void ConfigureSwaggerUi()
        {
            Configuration.Modules.AbpWebApi().HttpConfiguration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Easyman.WebAPI文档");
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    //将application层中的注释添加到SwaggerUI中
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    var commentsFileName = "bin//Easyman.Application.XML";
                    var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                    //将注释的XML文档添加到SwaggerUI中
                    c.IncludeXmlComments(commentsFile);
                })
                .EnableSwaggerUi("apis/{*assetPath}", b =>
                {
                    //对js进行了拓展
                    b.InjectJavaScript(Assembly.GetExecutingAssembly(), "Easyman.SwaggerUi.scripts.swagger.js");
                });
        }
    }
}
