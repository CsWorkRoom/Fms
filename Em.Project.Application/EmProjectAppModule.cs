using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Easyman;

namespace Easyman
{
    //[DependsOn(typeof(EasymanCoreModule),
    //    //typeof(EasymanDataModule),
    //    //typeof(EmProjectDataModule),
    //    typeof(EasymanApplicationModule), 
    //    typeof(AbpAutoMapperModule))]
    public class EmProjectAppModule :AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                //Add your custom AutoMapper mappings here...
                //mapper.CreateMap<,>()
            });
            //Configuration.Caching.Configure("singleRequireCache",cache=> {
            //    cache.DefaultSlidingExpireTime = System.TimeSpan.FromSeconds(5);
            //});

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
