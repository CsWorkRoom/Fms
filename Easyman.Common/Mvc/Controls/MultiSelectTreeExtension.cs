using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Easyman.Common.Mvc.Controls
{
    public static class MultiSelectTreeExtension
    {
        public static MvcHtmlString MultiSelectTree(this HtmlHelper helper, string name, string actionUrl, object defaultValue = null, string httpMethod = "GET")
        {
            return MultiSelectTreeHelper(helper, name, actionUrl, defaultValue, httpMethod);
        }
        public static MvcHtmlString MultiSelectTreeFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string actionUrl, string httpMethod = "POST")
        {
            var factory = new ExtensionFactory<TModel>(helper);

            var name = factory.GetName(expression);
            var value = factory.GetValue(expression);

            return MultiSelectTreeHelper(helper, name, actionUrl, value, httpMethod);
        }

        private static MvcHtmlString MultiSelectTreeHelper(HtmlHelper helper, string name, string actionUrl, object defaultValue, string httpMethod)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name属性不能为空", "name");
            }

            var htmlBuilder = new StringBuilder();
            var scriptBuilder = new StringBuilder();
            var value = defaultValue == null ? string.Empty : defaultValue.ToString();

            htmlBuilder.AppendLine("<input type='hidden' id='{id}' name='{id}' value='{value}' />");
            htmlBuilder.AppendLine("<ul id='zTreeMilti_{id}' class='ztree'></ul>");

            scriptBuilder.Append(@"
            <script> 
            $(function () {
                var setting_{id} = {
                    callback: {
                        onCheck: function(event, treeId, treeNode){
                            onGetCheckedValue_{id}();
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
                            var tempValue = '{value}';
                            if(typeof tempValue === 'undefined' || tempValue === ''){
	                            onGetCheckedValue_{id}();
                            }
                        },
                        error: function (xhr) {
                            abp.ui.clearBusy('#zTreeMilti_{id}');
                            console.log(xhr);
                        }
                });
            });

            function onGetCheckedValue_{id}() {
                var treeObj = $.fn.zTree.getZTreeObj('zTreeMilti_{id}');
                var checkNodes = treeObj.getCheckedNodes(true);
                var ids = [];
                $(checkNodes).each(function () {
                    ids.push(this.id);
                });
                $('#{id}').val(ids.splice(','));
            }
             </script>");

            var result = htmlBuilder.Append(scriptBuilder)
                .Replace("{id}", name)
                .Replace("{value}", value)
                .Replace("{httpMethod}", httpMethod)
                .Replace("{actionUrl}", actionUrl);

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
