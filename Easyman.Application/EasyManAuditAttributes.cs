using Abp.Authorization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Easyman
{
    /// <summary>
    /// Action权限校验
    /// </summary>
    public class EasyManAuditAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return false;
        }
    }
}
