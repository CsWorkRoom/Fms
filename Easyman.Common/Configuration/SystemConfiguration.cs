using Abp.Dependency;

namespace Easyman.Common
{
    public static class SystemConfiguration
    {
        public static SystemConfig Config
        {
            get { return IocManager.Instance.Resolve<ISystemConfigurationProvider>().SystemConfig; }
        }

        public const string TablePrefix = "EM_";
    }
}
