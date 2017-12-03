using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Easyman.Common.Mvc.Controls
{
    public class ExtensionFactory<TModel>
    {

        public ExtensionFactory(HtmlHelper<TModel> htmlHelper)
        {
            Helper = htmlHelper;
            UrlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
        }

        private HtmlHelper<TModel> Helper
        {
            get;
            set;
        }
        private UrlHelper UrlHelper
        {
            get;
            set;
        }


        public string GetName(LambdaExpression expression)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            return Helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        }

        public string GetValue<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            object model = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, Helper.ViewData).Model;

            return model != null && model.GetType().IsPredefinedType() ? Convert.ToString(model) : string.Empty;
        }

        public string GetActionrUrl(string actionName)
        {
            return UrlHelper.Action(actionName);
        }

        public string GetActionrUrl(string actionName, object routeValues)
        {
            return UrlHelper.Action(actionName, routeValues);
        }
        public string GetActionrUrl(string actionName, RouteValueDictionary routeValues)
        {
            return UrlHelper.Action(actionName, routeValues);
        }

        public string GetActionrUrl(string actionName, string controllerName)
        {
            return UrlHelper.Action(actionName, controllerName);
        }

        public string GetActionrUrl(string actionName, string controllerName, object routeValues)
        {

            return UrlHelper.Action(actionName, controllerName, routeValues);
        }
    }
}
