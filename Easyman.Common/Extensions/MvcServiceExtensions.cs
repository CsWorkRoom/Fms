#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：IMvcServiceExtensions
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/23 17:53:48
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion


#region 主体



namespace EasyMan
{
    using System.Text;
    using System.Web.Mvc;
    using Abp.Application.Services;

    public static class MvcServiceExtensions
    {
        public static JsonResult Json(this IApplicationService service, object data)
        {
            return Json(service, data, null /* contentType */, null /* contentEncoding */, JsonRequestBehavior.DenyGet);
        }

        public static JsonResult Json(this IApplicationService service, object data, string contentType)
        {
            return Json(service, data, contentType, null /* contentEncoding */, JsonRequestBehavior.DenyGet);
        }

        public static JsonResult Json(this IApplicationService service, object data, string contentType, Encoding contentEncoding)
        {
            return Json(service, data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        public static JsonResult Json(this IApplicationService service, object data, JsonRequestBehavior behavior)
        {
            return Json(service, data, null /* contentType */, null /* contentEncoding */, behavior);
        }

        public static JsonResult Json(this IApplicationService service, object data, string contentType, JsonRequestBehavior behavior)
        {
            return Json(service, data, contentType, null /* contentEncoding */, behavior);
        }

        public static JsonResult Json(this IApplicationService service, object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}
#endregion
