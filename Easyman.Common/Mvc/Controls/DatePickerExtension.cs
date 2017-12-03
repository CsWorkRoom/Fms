using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Easyman.Common.Mvc.Controls
{
    public static class DatePickerExtension
    {
        /// <summary>
        /// 日期控件
        /// </summary>
        /// <param name="helper">htmlhelper对象</param>
        /// <param name="name">名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="htmlAttributes"></param>
        public static MvcHtmlString DatePicker(this HtmlHelper helper, string name, object defaultValue, object htmlAttributes = null)
        {
            var htmlAttributesObj = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return DatePickerHelper(helper, name, htmlAttributesObj, defaultValue);
        }

        /// <summary>
        /// 日期控件
        /// </summary>
        /// <param name="helper">htmlhelper对象</param>
        /// <param name="expression">表达式</param>
        /// <param name="htmlAttributes"></param>
        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var factory = new ExtensionFactory<TModel>(helper);

            var name = factory.GetName(expression);
            var value = factory.GetValue(expression);

            return DatePickerHelper(helper, name, htmlAttributes, value);
        }

        private static MvcHtmlString DatePickerHelper(HtmlHelper helper, string name, object htmlAttributes, object defaultValue = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            var htmlAttributesObj = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributesObj);
            tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Text));
            tagBuilder.MergeAttribute("name", name, true);
            tagBuilder.GenerateId(name);

            if (defaultValue != null)
                tagBuilder.MergeAttribute("value", Convert.ToDateTime(defaultValue).ToString("yyyy-MM-dd"));

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));
            stringBuilder.Append(@"
            <script>
                $(function(){
	                $('#{name}').datepicker({
		                format: 'yyyy-mm-dd',
		                autoclose: true,
		                language: 'zh-CN',
		                toggleActive: true
	                });
                });
             </script>".Replace("{name}", name));

            return MvcHtmlString.Create(stringBuilder.ToString());
        }
    }
}
