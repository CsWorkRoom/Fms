using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;

namespace Easyman.Common
{
    partial class SystemConfigurationProvider : ISystemConfigurationProvider
    {
        private const string ConfigVirtualPath = "~/Config/systemConfig.config";

        public SystemConfigurationProvider()
        {
            SystemConfig = new SystemConfig();
            SetDefulatValue();
            ResolveConfig();
        }

        public SystemConfig SystemConfig { get; private set; }

        private void SetDefulatValue()
        {
            SystemConfig.TablePrefix = "EASYMAN";
        }

        private void ResolveConfig()
        {
            var filePath = HostingEnvironment.MapPath(ConfigVirtualPath);
            if (File.Exists(filePath))
            {
                var xmlReader = XDocument.Load(filePath);
                var root = xmlReader.Element(XNames.Root);

                if (root != null)
                {
                    foreach (var pi in typeof(SystemConfig).GetProperties())
                    {
                        var xElement = root.Elements().FirstOrDefault(w => String.Equals(w.Name.ToString(), pi.Name, StringComparison.CurrentCultureIgnoreCase));
                        if (xElement == null) continue;
                        var value = xElement.Attribute(XNames.Value);
                        if (value != null)
                        {
                            try
                            {

                                if (pi.PropertyType == typeof(Boolean))
                                {
                                    pi.SetValue(SystemConfig, Convert.ToBoolean(value.Value));
                                }
                                else
                                {
                                    pi.SetValue(SystemConfig, value.Value);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }
    }

    public static class XNames
    {
        public const string Xmlns = "";
        public static readonly XName Root = XName.Get("EasyMan", Xmlns);
        public static readonly XName Value = XName.Get("value");
    }
}

