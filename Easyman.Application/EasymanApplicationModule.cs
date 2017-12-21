using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;

namespace Easyman
{
    //[DependsOn(/*typeof(EasymanCoreModule),*/
    //    typeof(AbpAutoMapperModule))]
    public class EasymanApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                //Add your custom AutoMapper mappings here...
                //mapper.CreateMap<,>()
            });

            //为所有缓存配置有效期
            Configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = System.TimeSpan.FromHours(8);//8小时
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
