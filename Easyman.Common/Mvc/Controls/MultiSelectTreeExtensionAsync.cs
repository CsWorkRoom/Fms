using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Easyman.Common.Mvc.Controls
{
    public static class MultiSelectTreeExtensionAsync
    {
        public static MvcHtmlString AsyncMultiSelectTree(this HtmlHelper helper, string name, string actionUrl, string asyncUrl, string key, object defaultValue = null ,string httpMethod = "GET")
        {
            return MultiSelectTreeHelper(helper, name, actionUrl,asyncUrl, key ,defaultValue, httpMethod);
        }
        public static MvcHtmlString AsyncMultiSelectTreeFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string actionUrl,string asyncUrl,string key ,string httpMethod = "POST", string callBackFunc = null)
        {
            var factory = new ExtensionFactory<TModel>(helper);

            var name = factory.GetName(expression);
            var value = factory.GetValue(expression);

            return MultiSelectTreeHelper(helper, name, actionUrl, asyncUrl,key ,value, httpMethod);
        }

        private static MvcHtmlString MultiSelectTreeHelper(HtmlHelper helper, string name, string actionUrl, string asyncUrl, string key, object defaultValue, string httpMethod)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name属性不能为空", "name");
            }

            var htmlBuilder = new StringBuilder();
            var scriptBuilder = new StringBuilder();
            var value = defaultValue == null ? string.Empty : defaultValue.ToString();

            htmlBuilder.AppendLine("<input type='hidden' id='{id}' name='{id}' value='{value}' />");
            htmlBuilder.AppendLine("<input type='hidden' id='Parent{id}' name='parent{id}' value='{value}' />");
            htmlBuilder.AppendLine("<input type='hidden' id='Child{id}' name='child{id}' value='{value}' />");
            htmlBuilder.AppendLine("<ul id='zTreeMilti_{id}' class='ztree'></ul>");

            scriptBuilder.Append(@"
            <script> 
            $(function () {
                var setting_{id} = {
                    callback: {
                        onCheck: function(event, treeId, treeNode){
                            onGetCheckedValue_{id}(event, treeId, treeNode);
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
                    },
                    async:{
                        enable:true,
                        url:'{asyncUrl}',
                        autoParam:['{key}=Id'],
                        dataFilter:ajaxDataFilter
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
            
            function ajaxDataFilter(treeId, parentNode,data) {
                    if (data.success) {
                        return data.result;
                    } else {
                        abp.message.error(data.result.message, '提示');
                        return [];
                    }
            }
            function onGetCheckedValue_{id}(event,treeId, parentNode) {
                var treeObj = $.fn.zTree.getZTreeObj('zTreeMilti_{id}');
                var checkNodes = treeObj.getCheckedNodes(true);
                var ids = [];
                var parentIds = [];
                var childIds = [];
                $(checkNodes).each(function () {
                    ids.push(this.id);
                    if(this.isParent){
                        parentIds.push(this.id);
                    }else{
                        childIds.push(this.id);
                    }
                });
                $('#{id}').val(ids.splice(','));
                $('#Parent{id}').val(parentIds.splice(','));
                $('#Child{id}').val(childIds.splice(','));
            }
             </script>");

            var result = htmlBuilder.Append(scriptBuilder)
                .Replace("{id}", name)
                .Replace("{value}", value)
                .Replace("{httpMethod}", httpMethod)
                .Replace("{actionUrl}", actionUrl)
                .Replace("{asyncUrl}", asyncUrl)
                .Replace("{key}",key);

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
