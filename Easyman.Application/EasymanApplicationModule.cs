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
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
