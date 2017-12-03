using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using Easyman.Authorization;
using Easyman.Authorization.Roles;
using Easyman.MultiTenancy;
using Easyman.Users;

namespace Easyman
{
    //[DependsOn(typeof(AbpZeroCoreModule))]
    public class EasymanCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            ////Declare entity types
            //Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            //Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            //Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            ////Remove the following line to disable multi-tenancy.
            //Configuration.MultiTenancy.IsEnabled = true;

            ////Add/remove localization sources here
            //Configuration.Localization.Sources.Add(
            //    new DictionaryBasedLocalizationSource(
            //        EasymanConsts.LocalizationSourceName,
            //        new XmlEmbeddedFileLocalizationDictionaryProvider(
            //            Assembly.GetExecutingAssembly(),
            //            "Easyman.Localization.Source"
            //            )
            //        )
            //    );

            //AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            //Configuration.Authorization.Providers.Add<EasymanAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
