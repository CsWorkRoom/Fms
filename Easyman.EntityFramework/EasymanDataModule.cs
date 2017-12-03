using Abp.Modules;
using Abp.Zero.EntityFramework;
using System.Reflection;

namespace Easyman
{
   // [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(EasymanCoreModule))]
    public class EasymanDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<EasymanDbContext>());
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
