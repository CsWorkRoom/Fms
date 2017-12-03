using EasyMan;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Easyman.Common.Mvc.Controls
{
    public static class DropDownTreeMiltiExtension
    {
        public static MvcHtmlString DropDownTreeMilti(this HtmlHelper helper, string name, string actionUrl, object defaultValue = null, object htmlAttributes = null, string callBackFunc = null, string httpMethod = "GET")
        {
            return DropDownTreeMiltiHelper(helper, name, actionUrl, defaultValue, htmlAttributes, callBackFunc, httpMethod);
        }
        public static MvcHtmlString DropDownTreeMiltiFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string actionUrl, object htmlAttributes = null, string callBackFunc = null, string httpMethod = "GET")
        {
            var factory = new ExtensionFactory<TModel>(helper);

            var name = factory.GetName(expression);
            var value = factory.GetValue(expression);

            return DropDownTreeMiltiHelper(helper, name, actionUrl, value, htmlAttributes, callBackFunc, httpMethod);
        }

        private static MvcHtmlString DropDownTreeMiltiHelper(HtmlHelper helper, string name, string actionUrl, object defaultValue, object htmlAttributes = null, string callBackFunc = null, string httpMethod = "GET")
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name属性不能为空", "name");
            }

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var htmlBuilder = new StringBuilder();
            var scriptBuilder = new StringBuilder();
            var value = defaultValue == null ? string.Empty : defaultValue.ToString();

            var htmlAttributesObj = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var tagBuilder = new TagBuilder("input");

            if (!htmlAttributesObj.ContainsKey("readonly"))
                htmlAttributesObj.Add("readonly", "readonly");
            if (!htmlAttributesObj.ContainsKey("class"))
                htmlAttributesObj.Add("class", "form-control");

            tagBuilder.MergeAttributes(htmlAttributesObj);
            tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Text));
            tagBuilder.MergeAttribute("name", name, true);
            tagBuilder.MergeAttribute("value", value, true);
            tagBuilder.MergeAttribute("id", "text_" + name, true);

            htmlBuilder.Append("<div class='input-group'>");
            htmlBuilder.AppendLine("<input type='hidden' id='{id}' name='{id}' value='{value}' />");
            htmlBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.SelfClosing));
            htmlBuilder.AppendLine("<div id='dropDownTreeMilti_{id}' class='dropdown-menu dropdown-tree col-xs-12'>");
            htmlBuilder.AppendLine("<ul id='zTreeMilti_{id}' class='ztree'></ul>");
            htmlBuilder.AppendLine("<div class='col-md-12 padding-top-10'>");
            htmlBuilder.AppendLine("<button class='btn btn-warning btn-xs' type='button' onclick='onClickOk_{id}()'><i class='fa fa-save'></i> 确定</button>");
            htmlBuilder.AppendLine("</div>");
            htmlBuilder.AppendLine("</div>");
            htmlBuilder.AppendLine("<span class='input-group-btn'>");
            htmlBuilder.AppendLine("<button id='btn_{id}' class='btn btn-default' type='button'><i class='fa fa-chevron-down'></i></button>");
            htmlBuilder.AppendLine("</span>");
            htmlBuilder.AppendLine("</div>");

            scriptBuilder.Append(@"
            <script> 
            $(function () {
                $('#btn_{id}').on('click',
                function () {
                    $('#dropDownTreeMilti_{id}').toggle();
                });
                
                var setting_{id} = {
                    callback: {
                        onCheck: function(event, treeId, treeNode){
                            
                         }
                    },
                    view: {
                        showLine: true,
                        selectedMulti: true
                    },
                    data: {
                        simpleData: {
                            enable: true
                        }
                    },
                    check: {
                        enable: true
                    }   
                };

                $.ajax({
                        type: '{httpMethod}',
                        url: '{actionUrl}',
                        dataType: 'json',
                        contentType: 'application/json',
                        beforeSend: function(){
                            abp.ui.setBusy('#zTreeMilti_{id}');
                        },
                        success: function (data) {
                            abp.ui.clearBusy('#zTreeMilti_{id}');
                            if(data.result.contentEncoding)
		                        data = data.result.data;
	                        else
		                        data = data.result;
                            var tree= $.fn.zTree.init($('#zTreeMilti_{id}'), setting_{id}, data);
                            var value = '{value}'==''?null:'{value}';

                            var defualtNode = tree.getNodeByParam('id',value,null);
                
                            if(defualtNode)
                                $('#text_{id}').val(defualtNode.name);
                        },
                        error: function (xhr) {
                            abp.ui.clearBusy('#zTreeMilti_{id}');
                            console.log(xhr);
                        }
                });
            });

            function onClickOk_{id}() {
                var treeObj = $.fn.zTree.getZTreeObj('zTreeMilti_{id}');
                var checkNodes = treeObj.getCheckedNodes(true);
                var ids = [];
                var names = [];
                $(checkNodes).each(function () {
                    if (this.canChecked) {
                        ids.push(this.id);
                        names.push(this.name);
                    }
                });
                if({isValidationValue} && ids.length == 0){
                    abp.message.error('', '请至少选择一个节点！');
                    return;
                }
                $('#{id}').val(ids.splice(','));
                $('#text_{id}').val(names.splice(','));
                $('#dropDownTreeMilti_{id}').toggle();
                {callBackFunc}
            }
             </script>");

            var result = htmlBuilder.Append(scriptBuilder)
                .Replace("{id}", name)
                .Replace("{value}", value)
                .Replace("{actionUrl}", actionUrl)
                .Replace("{httpMethod}", httpMethod)
                .Replace("{isValidationValue}", htmlAttributesObj.ContainsKey("Required") ? "true" : "false")
                .Replace("{callBackFunc}", !string.IsNullOrWhiteSpace(callBackFunc) ? "if({0}){1} {0}(); {2}".FormatWith(callBackFunc, "{", "}") : "");

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
