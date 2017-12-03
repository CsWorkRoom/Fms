using System.Linq.Expressions;
using System.Web.Mvc;
using System;
using System.Text;
using EasyMan;

namespace Easyman.Common.Mvc.Controls
{
    public static class DropDownTreeExtension
    {
        public static MvcHtmlString DropDownTree(this HtmlHelper helper, string name, string actionUrl, object defaultValue = null, string callBackFunc = null)
        {
            return DropDownTreeHelper(helper, name, actionUrl, defaultValue, callBackFunc);
        }
        public static MvcHtmlString DropDownTreeFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string actionUrl,string callBackFunc = null)
        {
            var factory = new ExtensionFactory<TModel>(helper);

            var name = factory.GetName(expression);
            var value = factory.GetValue(expression);

            return DropDownTreeHelper(helper, name, actionUrl, value, callBackFunc);
        }

        private static MvcHtmlString DropDownTreeHelper(HtmlHelper helper, string name, string actionUrl, object defaultValue,string callBackFunc = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name属性不能为空", "name");
            }

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var htmlBuilder = new StringBuilder();
            var scriptBuilder = new StringBuilder();

            htmlBuilder.Append("<div class='input-group'>");
            htmlBuilder.AppendLine("<input type='text' id='text_{id}' value='{value}' class='form-control' readonly  />");
            htmlBuilder.AppendLine("<input type='hidden' id='{id}' name='{id}' value='{value}' />");
            htmlBuilder.AppendLine("<div id='dropDownTree_{id}' class='dropdown-menu dropdown-tree col-xs-12'>");
            htmlBuilder.AppendLine("<ul id='menuTree_{id}' class='ztree'></ul></div>");
            htmlBuilder.AppendLine("<span class='input-group-btn'>");
            htmlBuilder.AppendLine("<button id='btn_{id}' class='btn btn-default' type='button'><i class='fa fa-chevron-down'></i></button>");
            htmlBuilder.AppendLine("</span>");
            htmlBuilder.AppendLine("</div>");

            scriptBuilder.Append(@"
            <script> 
            $(function () {
                $('#btn_{id}').on('click',
                function () {
                    $('#dropDownTree_{id}').toggle();
                });

                var setting_{id} = {
                    callback: {
                        onClick: function(event, treeId, treeNode){
                            if(treeNode.chkDisabled)
                                return;
                            $('#{id}').val(treeNode.id);
                            $('#text_{id}').val(treeNode.name);
                            $('#dropDownTree_{id}').toggle();
                            {callBackFunc}
                         }
                    },
                    view: {
                        showLine: true,
                        selectedMulti: false
                    },
                    data: {
                        simpleData: {
                            enable: true
                        }
                    }
                };

                $.post('{actionUrl}', {}, function (data) {
                    if(data.result.contentEncoding)
                        data = data.result.data;
                    else
                        data = data.result;

                    var tree= $.fn.zTree.init($('#menuTree_{id}'), setting_{id}, eval(data));
                    var value = '{value}'==''?null:'{value}';

                    var defualtNode = tree.getNodeByParam('id',value,null);
                
                    if(defualtNode)
                        $('#text_{id}').val(defualtNode.name);
                });
            });
             </script>");

            var result = htmlBuilder.Append(scriptBuilder);
            result = result.Replace("{id}", name);
            result = result.Replace("{value}", defaultValue == null ? "" : defaultValue.ToString());
            result = result.Replace("{actionUrl}", actionUrl);
            result = result.Replace("{callBackFunc}", !string.IsNullOrWhiteSpace(callBackFunc) ? "if({0}){1} {0}(); {2}".FormatWith(callBackFunc, "{", "}") : "");

            return MvcHtmlString.Create(result.ToString());
        }

    }
}
