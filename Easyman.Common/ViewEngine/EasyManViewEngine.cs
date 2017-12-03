#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：EasyManViewEngine
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/16 15:53:12
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion


#region 主体



namespace Easyman.Common.ViewEngine
{
    using System.Web.Mvc;


    public class EasyManViewEngine : BuildManagerViewEngine
    {
        public EasyManViewEngine()
            : this(null)
        {
        }

        public EasyManViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            ViewLocationFormats = new[] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"

            };
            MasterLocationFormats = new[] {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
            PartialViewLocationFormats = new[] { 
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };

            AreaViewLocationFormats = ViewLocationFormats;
            AreaMasterLocationFormats = MasterLocationFormats;
            AreaPartialViewLocationFormats = PartialViewLocationFormats;

            FileExtensions = new[] {
                "cshtml"
            };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new RazorView(controllerContext, partialPath, null, false, FileExtensions, ViewPageActivator);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var view = new RazorView(controllerContext, viewPath, masterPath, true, FileExtensions, ViewPageActivator);

            return view;
        }
    }
}
#endregion
