// 文件上传
jQuery(function () {  
    initUpload();
});

function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
        + " " + date.getHours() + seperator2 + date.getMinutes()
        + seperator2 + date.getSeconds();
    return currentdate;
} 

function initUpload()
{
    var $ = jQuery,
          $list = $('#thelist'),
          $btn = $('#ctlBtn'),
          state = 'pending',
          uploader;

    // 优化retina, 在retina下这个值是2
    ratio = window.devicePixelRatio || 1,

    // 缩略图大小
    thumbnailWidth = 100 * ratio,
    thumbnailHeight = 100 * ratio,


     curWwwPath = window.document.location.href,
     pathName = window.document.location.pathname,
     pos = curWwwPath.indexOf(pathName),
     localhostPaht = curWwwPath.substring(0, pos),
     projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1),
    console.log(localhostPaht + projectName);
    uploader = WebUploader.create({
        // 自动上传。
        //auto: true,
        // 不压缩image
        resize: false,
        // swf文件路径
        swf: localhostPaht + projectName + '/Content/webuploader/dist/Uploader.swf',

        // 文件接收服务端。
        server: "Upload",

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#picker',

        InputModel: true,
        // 只允许选择文件，可选。
        accept: {
            title: 'Upload file format error！',
            mimeTypes: 'text/comma-separated-values,text/plain'
        },
        timeout: 0,
        fileNumLimit: 100,
        ButtonText: "Submission of annexes",
        InputModel: true,
        duplicate: true,
        table_style: "margin-top: -4px;"
    });

    // 当有文件添加进来的时候
    uploader.on('fileQueued', function (file) {

        $list.append('<div style="background-color:#f5f5f5; color: #000000;" id="' + file.id + '" class="item">' +
            '<h4 class="info" style="margin:5px;font: 15px/21px Arial, Helvetica, simsun, sans-serif;" >' + file.name + '</h4>' +
            '<p class="state"  style="margin:5px;" >Wait to upload...</p>' +
            '</div>');     

    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {

        var $li = $('#' + file.id),
            $percent = $li.find('.progress .progress-bar');
        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<div class="progress progress-striped active">' +
                '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                '</div>' +
                '</div>').appendTo($li).find('.progress-bar');
        }
        $li.find('p.state').text('File Loading');
        $percent.css('width', percentage * 100 + '%');

    });

    uploader.on('uploadSuccess', function (file) {
        $('#' + file.id).find('p.state').text('File Uploaded');
    });

    uploader.on('uploadError', function (file) {
        $('#' + file.id).find('p.state').text('File Upload Error');
    });

    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').fadeOut();
    });
    uploader.on('all', function (type) {
        if (type === 'startUpload') {
            state = 'uploading';
        } else if (type === 'stopUpload') {
            state = 'paused';
        } else if (type === 'uploadFinished') {
            state = 'done';
        }
        if (state === 'uploading') {
            $btn.text('Pause Upload');
        } else {
            $btn.text('Start Upload');
        }

    });

    $btn.on('click', function () {
        var nowtime = getNowFormatDate();
       uploader.options.formData = {
           nowtime: nowtime
       };   
        if (state === 'uploading') {
            uploader.stop();
            //buttom按钮阻止提交
            return false;
        } else {
            uploader.upload();
            //buttom按钮阻止提交
            return false;
        }
    });

}

