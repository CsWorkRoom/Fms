using Abp.Dependency;

namespace Easyman.Common
{
    partial interface ISystemConfigurationProvider : ISingletonDependency
    {
        SystemConfig SystemConfig { get; }
    }
}
