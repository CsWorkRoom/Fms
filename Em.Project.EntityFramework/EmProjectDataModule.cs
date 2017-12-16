using Abp.AutoMapper;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Easyman.EntityFramework;
using System.Data.Entity;
using System.Reflection;

namespace Easyman
{
    //[DependsOn(
    //    typeof(AbpZeroEntityFrameworkModule),
    //    typeof(EasymanCoreModule),
    //    typeof(EmProjectCoreModule),
    //    typeof(AbpAutoMapperModule)
    //    //,typeof(EasymanDataModule),
    //    )]
    //[DependsOn(typeof(AbpZeroEntityFrameworkModule))]
    public class EmProjectDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<EmProjectDbContext>());
            Configuration.Auditing.IsEnabled = true;
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;
            Configuration.UnitOfWork.IsTransactional = false;

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
