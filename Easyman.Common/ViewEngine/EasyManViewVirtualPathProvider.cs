#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：EasyManViewVirtualPathProvider
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/16 15:44:04
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

#region 主体



namespace Easyman.Common.ViewEngine
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Hosting;
    using Abp.Dependency;
    using Abp.Runtime.Caching;


    public class EasyManViewVirtualPathProvider : VirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            return base.FileExists(virtualPath) || IsEmbeddedView(virtualPath);
        }

        private bool IsEmbeddedView(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                return false;

            string virtualPathAppRelative = VirtualPathUtility.ToAppRelative(virtualPath);
            if (!virtualPathAppRelative.StartsWith("~/Views/", StringComparison.InvariantCultureIgnoreCase))
                return false;
            var fullyQualifiedViewName = virtualPathAppRelative.Substring(virtualPathAppRelative.LastIndexOf("/", System.StringComparison.Ordinal) + 1, virtualPathAppRelative.Length - 1 - virtualPathAppRelative.LastIndexOf("/", System.StringComparison.Ordinal));
            var result = GetViewMetadata(fullyQualifiedViewName);

            return result != null;
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsEmbeddedView(virtualPath))
            {
                string virtualPathAppRelative = VirtualPathUtility.ToAppRelative(virtualPath);
                var fullyQualifiedViewName = virtualPathAppRelative.Substring(virtualPathAppRelative.LastIndexOf("/", System.StringComparison.Ordinal) + 1, virtualPathAppRelative.Length - 1 - virtualPathAppRelative.LastIndexOf("/", System.StringComparison.Ordinal));

                return new EasyManResourceVirtualFile(GetViewMetadata(fullyQualifiedViewName), virtualPath);
            }

            return Previous.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies,
            DateTime utcStart)
        {
            return IsEmbeddedView(virtualPath)
                ? null : Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        private ViewMetaData GetViewMetadata(string fullViewName)
        {
            var index = fullViewName.ToLower().IndexOf(".views", System.StringComparison.Ordinal);

            if (index == -1)
                return null;

            var assemblyName = fullViewName.Substring(0, index).ToLower();
            var assembly = GetAssembly(assemblyName);
            var stream = assembly.GetManifestResourceStream(fullViewName);

            if (stream != null)
                return new ViewMetaData()
                {
                    Name = fullViewName,
                    AssemblyFullName = assemblyName
                };
            else
                return null;
        }

        private Assembly GetAssembly(string assemblyName)
        {
            var cashManager = IocManager.Instance.Resolve<ICacheManager>();
            var cash = cashManager.GetCache("viewCash");
            var assembly = cash.GetOrDefault(assemblyName) as Assembly;

            if (assembly != null) return assembly;

            assembly = Assembly.Load(assemblyName);
            cash.Set(assemblyName, assembly);

            return assembly;
        }
    }
}
#endregion
