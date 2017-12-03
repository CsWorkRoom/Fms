
//#region 页面入口

$(document).ready(function() {
    InitPage();
    InitEvent();
});

//#endregion

//#region 初始化页面

function InitPage() {
    initWebUploader();
}

//#endregion 

//#region 初始化事件

function InitEvent() {
    $("#sumbit-btn").click(function () {
        var fileId = $("#FileId").val();
        if (fileId == "") {
            abp.message.error("请先上传文件！", "保存失败");
            return;
        }
        $("#saveForm").submit();
    });
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}

//#endregion

// 初始化webUploader
function initWebUploader() {

    var fileUploader = WebUploader.create({
        auto: true,
        swf: '~/Views/Admin/asset/Uploader.swf',
        server: '/api/services/api/File/SavePostedSetupFile',
        pick: '#filePicker',
        fileNumLimit: 1,
        accept: {
            title: 'Application',
            extensions: 'ipa,apk',
            mimeTypes: 'application/*'
        }
    });

    fileUploader.on('fileQueued', function (file) {

        if (fileUploader.getFiles().length > 1) {
            fileUploader.removeFile(fileUploader.getFile(file.id));
            return;
        }

    });

    // 文件上传过程中创建进度条实时显示。
    fileUploader.on('uploadProgress', function (file, percentage) {

    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    fileUploader.on('uploadSuccess', function (file, data) {

        var retInfo = $.parseJSON(data._raw);
        var fileInfo = retInfo.result;

        $("#FileId").val(fileInfo.id);
        $("#postedFileName").text(fileInfo.name);
        $("#UpdateUrl").val(fileInfo.url);
    });

    // 文件上传失败，显示上传出错。
    fileUploader.on('uploadError', function (file) {

    });

    // 完成上传完了，成功或者失败，先删除进度条。
    fileUploader.on('uploadComplete', function (file) {

    });
}



