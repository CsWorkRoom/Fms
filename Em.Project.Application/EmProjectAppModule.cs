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
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
