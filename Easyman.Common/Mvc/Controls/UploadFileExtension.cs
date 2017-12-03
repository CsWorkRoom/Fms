using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EasyMan;
using System.Web.Mvc;
using System;

namespace Easyman.Common.Mvc.Controls
{
    public static class UploadFileExtension
    {
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="helper">htmlhelper对象</param>
        /// <param name="name">名称</param>
        /// <param name="isAutoUpload">是否自动上传</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="extensions">文件后缀名（如csv,xls）</param>
        /// <param name="fileNumLimit">最大文件数量</param>
        public static MvcHtmlString UploadFile(this HtmlHelper helper, string name, bool isAutoUpload = false, object defaultValue = null, string extensions = null, int fileNumLimit = 0)
        {
            return UploadFileHelper(helper, name, isAutoUpload, defaultValue, extensions, fileNumLimit);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="helper">htmlhelper对象</param>
        /// <param name="expression">表达式</param>
        /// <param name="isAutoUpload">是否自动上传</param>
        /// <param name="extensions">文件后缀名（如csv,xls）</param>
        /// <param name="fileNumLimit">最大文件数量</param>
        public static MvcHtmlString UploadFileFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool isAutoUpload = false, string extensions = null, int fileNumLimit = 0)
        {
            var factory = new ExtensionFactory<TModel>(helper);

            var name = factory.GetName(expression);
            var value = factory.GetValue(expression);

            return UploadFileHelper(helper, name, isAutoUpload, value, extensions, fileNumLimit);
        }

        private static MvcHtmlString UploadFileHelper(HtmlHelper helper, string name, bool isAutoUpload, object defaultValue, string extensions = null, int fileNumLimit = 0)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name属性不能为空", "name");

            string extensionConfig;
            if (!string.IsNullOrWhiteSpace(extensions))
            {
                var extensionList = extensions.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()).ToList();
                var extensionss = string.Join(",", extensionList);
                var mimeTypes = string.Join(",", extensionList.Select(x => "." + x));
                extensionConfig = string.Format("{0} title: 'Customs', extensions: '{1}', mimeTypes: '{2}' {3}", "{", extensionss, mimeTypes, "}");
            }
            else
            {
                extensionConfig = @"{
		                        title: 'Images',
		                        extensions: 'gif,jpg,jpeg,bmp,png',
		                        mimeTypes: 'image/*'
	                        },
	                        {
		                        title: 'Docs',
		                        extensions: 'rar,zip,7z,doc,xls,ppt,docx,xlsx,pptx,pdf,chm,csv,txt',
		                        mimeTypes: '.rar,.zip,.7z,.doc,.xls,.ppt,.docx,.xlsx,.pptx,.pdf,.chm,.csv,.txt'
	                        },
	                        {
		                        title: 'Medias',
		                        extensions: 'mp4,3gp,rmvb,wmv,wma,mp3',
		                        mimeTypes: '.mp4,.3gp,.rmvb,.wmv,.wma,.mp3'
	                        }";
            }

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var htmlBuilder = new StringBuilder();
            var scriptBuilder = new StringBuilder();

            htmlBuilder.Append("<div id='upload-{id}-container'>");
            htmlBuilder.AppendLine("<input type='hidden' id='{id}' name='{id}' value='{value}' />");
            htmlBuilder.AppendLine("<div id='{id}fileList' class='uploader-list'></div>");
            htmlBuilder.AppendLine("<div class='btns'>");
            htmlBuilder.AppendLine("<div id='{id}picker'>选择文件</div>");
            if (!isAutoUpload) htmlBuilder.AppendLine("<button id='{id}ctlBtn' onclick='{id}uploader.upload()' type='button' class='btn btn-default'>开始上传</button>");
            htmlBuilder.AppendLine("</div>");
            htmlBuilder.AppendLine("</div>");

            scriptBuilder.Append(@"
            <link href='/Scripts/UpNotice/css/webuploader.css' type='text/css' rel='stylesheet' />
            <style>
                .item{ position:relative;padding:0 5px }
                .item h4{ margin-bottom: 10px; border-radius: 2px; }
                .item span{ position: absolute;right: 5px;top: 2px; }
                .item p{color: blue;display: inline;position: absolute;right: 50px;top: 2px;}
                .noticheck{ padding: 0 18%; }
                .ischeck{ display: inline;margin-right: 20px;   }
                input[type=checkbox] { vertical-align: -2px; }
                .heightckeck {line-height:30px}
                .info{ width: 77%;overflow: hidden;white-space: nowrap;text-overflow: ellipsis}
                .info a{color:black}
            </style>
            <script src='/Scripts/UpNotice/js/webuploader.js'></script>
            <script>
                var {id}uploader;
                $(function () {
                    var applicationPath = window.applicationPath || '../../', ${id}list = $('#{id}fileList');

                    {id}uploader = WebUploader.create({
                        auto: {isAutoUpload}, {fileNumLimit}
                        swf: applicationPath + '/img/Uploader.swf',
                        server: '{url}',
                        pick: '#{id}picker',
                        accept: [
	                        {extensions}
                        ]
                    });

                    {id}uploader.on('fileQueued', function (file) {
                        ${id}list.append('<div id=""' + file.id + '"" class=""item"">' +
                            '<h4 class=""info""><a title=""' + file.name +'"">' + file.name + '</a></h4>' + '<span class=""del""><a href=""#"">删除</a></span>' +
                            '<p class=""state"" style=""color:#5BC0DE"">等待上传...</p>' +
                            '</div>');
                    });

                    {id}uploader.on('uploadProgress', function (file, percentage) {
                        var $li = $('#' + file.id),
                            $percent = $li.find('.progress span');

                        if (!$percent.length) {
                            $percent = $('<p class=""progress""><span></span></p>')
                                .appendTo($li)
                                .find('span');
                        }
                        $percent.css('width', percentage * 100 + '%');
                    });

                    {id}uploader.on('uploadSuccess', function (file, response) {
                        if (response && response.success && response.result && response.result.fileId) {
                            $('#' + file.id).find('p.state').text('已上传').css(""color"", ""green"");
                            var $attachment = $('#{id}');
                            var oldValue = $attachment.val();
                            oldValue = oldValue && oldValue != '' ? (oldValue + ',') : '';
                            $attachment.val(oldValue + response.result.fileId);
                            $('#' + file.id).attr('fileId', response.result.fileId);
                        } else {
                            $('#' + file.id).find('p.state').text('上传出错').css(""color"", ""red"");
                        }
                    });

                    {id}uploader.on('uploadError', function (file, reason) {
	                    abp.ui.clearBusy('#upload-{id}-container');
                        $('#' + file.id).find('p.state').text('上传出错').css(""color"", ""red"");
                    });

                    {id}uploader.on('error', function (type) {
                        var message;
                        switch(type) {
	                        case 'Q_TYPE_DENIED': message = '不支持导入您选择的文件格式'; break;
	                        case 'Q_EXCEED_NUM_LIMIT': message = '添加的文件数量超出最大限制'; break;
	                        case 'Q_EXCEED_SIZE_LIMIT': message = '添加的文件总大小超出最大限制'; break;
	                        default: message = '未知错误'; break;
                        }
                        alert(message);
                    });

                    {id}uploader.on('startUpload', function () {
		                abp.ui.setBusy('#upload-{id}-container');
                    });

                    {id}uploader.on('uploadComplete', function (file) {
                        $('#' + file.id).find('.progress').fadeOut();
                    });

                    {id}uploader.on('uploadFinished', function () {
		                abp.ui.clearBusy('#upload-{id}-container');
                    });

                    ${id}list.on(""click"", "".del"", function () {
                        var $parent = $(this).parent();
                        var id = $parent.attr(""id"");
                        {id}uploader.removeFile({id}uploader.getFile(id, true));
                        $(this).parent().remove();
                        
                        var fileId = $parent.attr('fileId');
                        if(fileId){
                            deleteFile(fileId);
                            var value = $('#{id}').val();
                            if(value && value != ''){
	                            var result = [];
	                            var ids = value.split(',');
	                            $.each(ids, function(i,e){ if(e != '' && e != fileId){ result.push(e); } });
	                            $('#{id}').val(result);
                            }
                        }
                    });
                });

                function deleteFile(id){
	                abp.ajax({
	                    beforeSend: function () {
		                    abp.ui.setBusy('#upload-{id}-container');
	                    },
	                    url: '/api/services/Admin/Accessory/DeleteById?id=' + id,
	                    method: 'POST',
	                    data: JSON.stringify({ id: id })
	                }).done(function (data) {
	                    abp.ui.clearBusy('#upload-{id}-container');
	                    if (data) {
		                    abp.notify.success('移除成功');
	                    } else {
		                    abp.notify.error('移除失败');
	                    }
	                }).fail(function (data) {
	                    abp.ui.clearBusy('#upload-{id}-container');
	                    abp.notify.error(data.message);
	                });
                }
             </script>");

            var result = htmlBuilder.Append(scriptBuilder);
            result = result.Replace("{id}", name)
                .Replace("{isAutoUpload}", isAutoUpload ? "true" : "false")
                .Replace("{url}", urlHelper.Action("Upload", "Notice", new { Area = "Admin" }))
                .Replace("{value}", defaultValue == null ? "" : defaultValue.ToString())
                .Replace("{extensions}", extensionConfig)
                .Replace("{fileNumLimit}", fileNumLimit == 0 ? string.Empty : "fileNumLimit: {0},".FormatWith(fileNumLimit));

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
