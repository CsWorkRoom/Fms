
var blnTop = false;//是否在顶层显示
///动态生成模态窗体（通过字符串生成）
///strModalId:模态窗体ID
///strTitle:模态窗体标题
///strContent:模态窗体html字符串内容
///strFooter:模态窗体右下方html字符串内容
///intWidth:模态窗体的宽度
///intHeight:模态窗体的高度
var ModeDialogContent = function (strModalId, strTitle, strContent, strFooter, intWidth, intHeight) {
    if (strModalId == null || strModalId == '') 
        strModalId = "id" + new Date().getTime();

    //当前窗口的宽高
    var strMargin = "";
    if (intWidth == null || intWidth == "" || intWidth >= window.top.innerWidth)
        intWidth = blnTop ? window.top.innerWidth - 10 : $("body").width();
    if (intHeight == null || intHeight == "" || intHeight >= window.top.innerHeight - 60 || intHeight >= $("body").height() - 60) {
        if (blnTop) {
            intHeight = window.top.innerHeight - 60;
            strMargin = "margin:30px auto !important";
        } else {
            intHeight = $("body").height() - 60;
            strMargin = "margin:30px auto !important";
        }
    }
    var strStyle = " style='width:" + intWidth + "px;height:" + intHeight + "px;" + strMargin + "'";

    var strModalHtml = "<div class='modal fade' id='" + strModalId + "' tabindex='-1' aria-hidden='true' data-backdrop='static' role='dialog' aria-hidden='true'>";
    strModalHtml += "<div class='modal-dialog' " + strStyle + ">";
    strModalHtml += "<div class='modal-content' " + strStyle + ">";
    strModalHtml += "<div class='modal-header'>";
    strModalHtml += "<button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>";
    strModalHtml += "<h4 class='modal-title' >" + strTitle + "</h4></div>";
    strModalHtml += "<div class='modal-body' >" + strContent + "</div>";
    strModalHtml += "<div class='modal-footer'>";
    strModalHtml += strFooter;
   // strModalHtml += "<button type='button' class='btn btn-danger' data-dismiss='modal'><i class='fa fa-times'></i> 关闭</button>";
    strModalHtml += "</div>";
    strModalHtml += "</div>";
    strModalHtml += "</div>";
    strModalHtml += "</div>";

    var objHtml = $(strModalHtml);
    objHtml.find(".modal-body").height($(strModalHtml).find(".modal-content").height() - 47);//设置URL地址的高度
    //是否在顶层显示
    if (blnTop) {
        $(strModalHtml).appendTo(window.top.$("body"));
        window.top.$("#" + strModalId).modal("show");
    }
    else {
        $(objHtml).appendTo(this.$("body"));
        $("#" + strModalId).modal("show");
        if (window.top.$(".modal-backdrop").length <= 0)
            return;
        switch (window.top.$(".modal-backdrop").length) {
            case 1:
                window.top.$(".modal-backdrop").eq(0).appendTo(this.$("body"));
                break;
            default:
                window.top.$(".modal-backdrop").eq(1).appendTo(this.$("body"));
                break;
        }
    }
}

///动态生成模态窗体顶层显示
///strModalId:模态窗体ID
///strTitle:模态窗体标题
///strContent:模态窗体html字符串内容
///strFooter:模态窗体右下方html字符串内容
///intWidth:模态窗体的宽度
///intHeight:模态窗体的高度
var TopModeDialogContent = function (strModalId, strTitle, strContent, strFooter, intWidth, intHeight) {
    blnTop = true;//顶层显示
    ModeDialogContent(strModalId, strTitle, strContent, strFooter, intWidth, intHeight);
    blnTop = false;//修改成默认值
}

///动态生成模态窗体（通过对像生成）
///strModalId:模态窗体ID
///strTitle:模态窗体标题
///strContent:模态窗体对像内容
///strFooter:模态窗体右下角对像内容
///intWidth:模态窗体的宽度
///intHeight:模态窗体的高度
var ModeDialogObjContent = function (strModalId, strTitle, objContent, objFooter, intWidth, intHeight) {
    if (strModalId == null || strModalId == '')
        strModalId = "id" + new Date().getTime();

    //当前窗口的宽高
    var strMargin = "";
    //宽度调整
    if (intWidth == null || intWidth == "") {
        intWidth = blnTop ? window.top.innerWidth - 10 : $("body").width() - 6;
    } else if (blnTop) {
        if (intWidth >= window.top.innerWidth - 10) {
            intWidth = window.top.innerWidth - 10;
        } else if (intWidth >= window.top.innerWidth - 6) {
            intWidth = window.top.innerWidth - 10;
        }
    }
    //高度调整
    if (intHeight == null || intHeight == "") {
        if (blnTop) {
            intHeight = window.top.innerHeight - 60;
            strMargin = "margin:30px auto !important";
        } else {
            intHeight = $("body").height() - 60;
            strMargin = "margin:30px auto !important";
        }
    } else if (blnTop) {
        if (intHeight >= window.top.innerHeight - 60) {
            intHeight = window.top.innerHeight - 60;
            strMargin = "margin:30px auto !important";
        }
    } else if (intHeight >= $("body").height() - 60) {
        intHeight = $("body").height() - 60;
        strMargin = "margin:30px auto !important";
    }
    var strStyle = " style='width:" + intWidth + "px;height:" + intHeight + "px;" + strMargin + "'";

    var strModalHtml = "<div class='modal fade' id='" + strModalId + "' tabindex='-1' aria-hidden='true' data-backdrop='static' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>";
    strModalHtml += "<div class='modal-dialog' " + strStyle + ">";
    strModalHtml += "<div class='modal-content' " + strStyle + ">";
    strModalHtml += "<div class='modal-header'>";
    strModalHtml += "<button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>";
    strModalHtml += "<h4 class='modal-title'>" + strTitle + "</h4></div>";
    strModalHtml += "<div class='modal-body' ></div>";
    strModalHtml += "<div class='modal-footer'>";
   // strModalHtml += "<button type='button' class='btn btn-default' data-dismiss='modal'><i class='fa fa-times'></i> 关闭</button>";
    strModalHtml += "</div>";
    strModalHtml += "</div>";
    strModalHtml += "</div>";
    strModalHtml += "</div>";

    var objHtml = $(strModalHtml);
    objHtml.find(".modal-body").append(objContent);
    objHtml.find(".modal-footer").append(objFooter);
    objHtml.find(".modal-body").height($(strModalHtml).find(".modal-content").height() - 47);//设置URL地址的高度
    //是否在顶层显示
    if (blnTop) {
        window.top.$("#" + strModalId).remove();//先移出
        $(objHtml).appendTo(window.top.$("body"));
        window.top.$("#" + strModalId).modal("show");
    }
    else {
        $("#" + strModalId).remove();//先移出
        $(objHtml).appendTo(this.$("body"));
        $("#" + strModalId).modal("show");
        if (window.top.$(".modal-backdrop").length <= 0)
            return;
        switch (window.top.$(".modal-backdrop").length) {
            case 1:
                window.top.$(".modal-backdrop").eq(0).appendTo(this.$("body"));
                break;
            default:
                window.top.$(".modal-backdrop").eq(1).appendTo(this.$("body"));
                break;
        }
    }
}

///动态生成模态窗体顶层显示（通过对像生成）
///strModalId:模态窗体ID
///strTitle:模态窗体标题
///strContent:模态窗体对像内容
///strFooter:模态窗体右下角对像内容
///intWidth:模态窗体的宽度
///intHeight:模态窗体的高度
var TopModeDialogObjContent = function (strModalId, strTitle, objContent, objFooter, intWidth, intHeight) {
    blnTop = true;
    ModeDialogObjContent(strModalId, strTitle, objContent, objFooter, intWidth, intHeight);
    blnTop = false;
}

///动态生成模态窗体（通过URL生成）
///strModalId:模态窗体ID
///strTitle:模态窗体标题
///strUrl:模态窗体URL
///intWidth:模态窗体的宽度
///intHeight:模态窗体的高度
var ModeDialogUrl = function (strModalId, strTitle, strUrl, intWidth, intHeight) {
    if (strUrl == null || strUrl == '') {
        abp.message.error("无连接地址", "消息提示");
        return;
    }
    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(strUrl, "消息提示");
        return;
    }
    var strTime = new Date().getTime();
    if (strModalId == null || strModalId == '')
        strModalId = "id" + strTime;

    //#region 记录IframID
    var strIframe = SetIframID(strModalId);
    //#endRegion 记录IframID

    strUrl = GetPath(strUrl);//标准化地址
    //当前窗口的宽高
    var strMargin = "";
    //宽度调整
    if (intWidth == null || intWidth == "" ) {
        intWidth = blnTop ? window.top.innerWidth - 10 : $("body").width() - 6;
    } else if (blnTop) {
        if (intWidth >= window.top.innerWidth - 10) {
            intWidth = window.top.innerWidth - 10;
        } else if (intWidth >= window.top.innerWidth - 6) {
            intWidth = window.top.innerWidth - 10;
        }
    }
    //高度调整
    if (intHeight == null || intHeight == "" ) {
        if (blnTop) {
            intHeight = window.top.innerHeight - 60;
            strMargin = "margin:30px auto !important";
        } else {
            intHeight = $("body").height() - 60;
            strMargin = "margin:30px auto !important";
        }
    } else if (blnTop) {
        if (intHeight >= window.top.innerHeight - 60 ) {
            intHeight = window.top.innerHeight - 60;
            strMargin = "margin:30px auto !important";
        } 
    }else if (intHeight >= $("body").height() - 60) {
            intHeight = $("body").height() - 60;
            strMargin = "margin:30px auto !important";
        }

    var strStyle = " style='width:" + intWidth + "px;height:" + intHeight + "px;" + strMargin + "'";
        
    var strModalHtml = "<div class='modal fade' id='" + strModalId + "' tabindex='-1' role='dialog' aria-hidden='true' aria-hidden='true' data-backdrop='static'>";
    strModalHtml += "<div class='modal-dialog' " + strStyle + ">";
    strModalHtml += "<div class='modal-content' " + strStyle + ">";
    strModalHtml += "<div class='modal-header'>";
    strModalHtml += "<button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>";
    strModalHtml += "<h4 class='modal-title' >" + strTitle + "</h4></div>";
    strModalHtml += "<div class='modal-body' style='width:100%;'><iframe id=" + strIframe + " name=" + strIframe + " style='width:100%;height:100%' frameborder='0' scrolling='auto' src='" + strUrl + "'></iframe></div>";
    strModalHtml += "</div>";
    strModalHtml += "</div>";
    strModalHtml += "</div>";
    var objHtml = $(strModalHtml);
    objHtml.find(".modal-body").height($(strModalHtml).find(".modal-content").height() - 47);//设置URL地址的高度

    ////是否在顶层显示
    if (blnTop) {
        window.top.$("#" + strModalId).remove();//先移出
        $(objHtml).appendTo(window.top.$("body"));       
        window.top.$("#" + strModalId).modal("show");
    }
    else {
        $("#" + strModalId).remove();//先移出
        $(objHtml).appendTo(this.$("body"));
        $("#" + strModalId).modal("show");
        if (window.top.$(".modal-backdrop").length <= 0)
            return;
        switch (window.top.$(".modal-backdrop").length) {
            case 1:
                window.top.$(".modal-backdrop").eq(0).appendTo(this.$("body"));
                break;
            default:
                window.top.$(".modal-backdrop").eq(1).appendTo(this.$("body"));
                break;
        }
    }
}

///动态生成模态窗体顶层显示（通过对像生成）
///strModalId:模态窗体ID
///strTitle:模态窗体标题
///strUrl:模态窗体URL
///intWidth:模态窗体的宽度
///intHeight:模态窗体的高度
var TopModeDialogUrl = function (strModalId, strTitle, strUrl, intWidth, intHeight) {
    blnTop = true;
    ModeDialogUrl(strModalId, strTitle, strUrl, intWidth, intHeight);
    blnTop = false;
}

///模式窗口弹出(指定大小)
///strTitle:窗体标题
///strUrl:内容显示地址
///intWidth:窗体宽度
///intHeight:窗休息高度
var ModelDialog = function (strTitle, strUrl, intWidth, intHeight) {
    if (strUrl == null || strUrl == '') {
        abp.message.error("无连接地址", "消息提示");
        return;
    }
    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(strUrl, "消息提示");
        return;
    }
    strUrl = GetPath(strUrl);//标准化地址

    //当前窗口的宽高
    if (intWidth == null || intWidth == "" || intWidth >= window.top.innerWidth)
        intWidth = blnTop ? window.top.innerWidth: $("body").width();

    if (intHeight == null || intHeight == "" || intHeight >= window.top.innerHeight)
        intHeight = blnTop ? window.top.innerHeight: $("body").height();

    DiyModal.window({
        title: strTitle,
        url: strUrl,
        width: intWidth,
        height: intHeight,
        fullscreen: false
        ////afterClose: function () {
        ////    table.reload();
        ////}
    }).open();
};

///模式窗口弹出(全屏展示)
///strTitle:窗体标题
///strUrl:内容显示地址
var FullModelDialog = function (strTitle, strUrl) {
    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(strUrl, "消息提示");
        return;
    }
    ModelDialog(strTitle, strUrl, (window.innerWidth * 0.96), (window.innerHeight * 0.96));
};

var blnSon = false;//是否子窗口打开
///浏览器弹出窗口
///strTitle:窗体标题
///strUrl:内容显示地址
///intWidth:窗体宽度
///intHeight:窗休息高度
var WindowOpen = function (strTitle, strUrl, intWidth, intHeight) {

    if (strUrl == null || strUrl == '') {
        abp.message.error("无连接地址", "消息提示");
        return;
    }
    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(strUrl, "消息提示");
        return;
    }
    strUrl = GetPath(strUrl);//标准化地址

    if (strTitle == null || strTitle == '') {
        strTitle = '';
    }

    //当前窗口的宽高
    if (intWidth == null || intWidth == "" || intWidth >= window.top.innerWidth)
        intWidth = blnTop ? window.top.innerWidth: $("body").width();

    if (intHeight == null || intHeight == "" || intHeight >= window.top.innerHeight)
        intHeight = blnTop ? window.top.innerHeight: $("body").height();

    window.open(strUrl, strTitle, (blnSon ? 'fullscreen=0,' : '') + 'height=' + intHeight + ', width=' + intWidth + ', top=0,left=0, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no')
};

///浏览器弹出窗口
///strTitle:窗体标题
///strUrl:内容显示地址
///intWidth:窗体宽度
///intHeight:窗休息高度
var WindowSonOpen = function (strTitle, strUrl, intWidth, intHeight) {
    blnSon = true;
    WindowOpen(strTitle, strUrl, intWidth, intHeight);
    blnSon = false;
};

///当前浏览器原地址跳转
///strUrl:内容显示地址
var WindowLocation = function (strUrl) {
    if (strUrl == null || strUrl == '') {
        abp.message.error("无连接地址", "消息提示");
        return;
    }
    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(strUrl, "消息提示");
        return;
    }
    strUrl = GetPath(strUrl);//标准化地址
    window.location.href = strUrl;
};

///当前浏览器顶级窗口地址跳转
///strUrl:内容显示地址
var WindowTopLocation = function (strUrl) {
    if (strUrl == null || strUrl == '') {
        abp.message.error("无连接地址", "消息提示");
        return;
    }
    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(strUrl, "消息提示");
        return;
    }
    strUrl = GetPath(strUrl);//标准化地址
    window.top.location.href = strUrl;
};

///当前浏览器父级窗口地址跳转
///strUrl:内容显示地址
var WindowParentLocation = function (strUrl) {
    if (strUrl == null || strUrl == '') {
        abp.message.error("无连接地址", "消息提示");
        return;
    }
    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(strUrl, "消息提示");
        return;
    }
    strUrl = GetPath(strUrl);//标准化地址
    window.parent.location.href = strUrl;
};

///当前浏览器指定框架地址重定向
///strUrl:内容显示地址
///strFrame:是重定向的框架名称
var WindowFramesLocation = function (strFrame, strUrl) {
    if (strUrl == null || strUrl == '') {
        abp.message.error("无连接地址", "消息提示");
        return;
    }
    strUrl = GetPath(strUrl);//标准化地址

    if (strFrame == null || strFrame == '') {
        abp.message.error("框架名称为空", "消息提示");
        return;
    }
    if (window.parent.iframeName != null) {//查找父级IFRAME
        window.parent.iframeName.location.href = strUrl;
    } else if (window.parent.frames[strFrame] != null) { //查找父级下面的IFRAME  
        window.parent.frames[strFrame].location.href = strUrl;
    } else if (window.frames[strFrame] != null) {//查找下级的IFAME
        window.frames[strFrame].location.href = strUrl;
    } else {
        swal("消息提示", "找不到您所指定的框架名", "error");
    }
};

///添加选项卡
///strUrl:选项卡URL地址
///strName:选项卡名称
var AddTab = function (strName, strUrl) {
    if (strUrl == undefined || $.trim(strUrl).length == 0 || strName == undefined || $.trim(strName).length == 0) {       
        abp.message.error("地址及选项卡名称不能为空", "消息提示");
        return false;
    }
    if (strUrl.indexOf("程序异常") >= 0) {        
        abp.message.error(strUrl, "消息提示");
        return;
    }
    strUrl = GetPath(strUrl);//标准化地址
    //var iframId = GetIfram();
    //if (iframId != null && iframId != "") {
    //    if (window.top.$("#hidModeFrameIds").val().length >= 10000)//如果记录数大于1万时，就清零
    //        window.top.$("#hidModeFrameIds").val("");
    //    window.top.$("#hidModeFrameIds").val(window.top.$("#hidModeFrameIds").val() + strIframe + ":" + iframId + "|");
    //}

    var strTime = new Date().getTime();
    //#region记录IframID
    var strModalId = "iframe" + strTime;
    var strIframe = SetIframID(strModalId);
    //#endRegion记录IframID

    var flag = true;//是否存在此选项卡
    //查询选项卡中是否已存在了要添加的选项卡，如果存在了，就选中处理  
    window.parent.$('.menuTab').each(function () {
        if ($(this).data('id') == strUrl) {
            if (!$(this).hasClass('active')) {
                $(this).addClass('active').siblings('.menuTab').removeClass('active');
                $.learuntab.scrollToTab(this);
                window.parent.$('.mainContent .LRADMS_iframe').each(function () {
                    if ($(this).data('id') == strUrl) {
                        $(this).show().siblings('.LRADMS_iframe').hide();
                        return false;
                    }
                });
            }
            flag = false;
            return false;
        }
    });
    //添加选项卡
    if (flag) {
        var str = '<a href="javascript:;" class="active menuTab" data-id="' + strUrl + '">' + strName + ' <i class="fa fa-remove"></i></a>';
        window.parent.$('.menuTab').removeClass('active');
        var str1 = '<iframe class="LRADMS_iframe" id="' + strIframe + '" name="' + strIframe + '"  width="100%" height="100%" src="' + strUrl + '" frameborder="0" data-id="' + strUrl + '" seamless></iframe>';
        window.parent.$('.mainContent').find('iframe.LRADMS_iframe').hide();
        window.parent.$('.mainContent').append(str1);
        window.parent.$('.menuTabs .page-tabs-content').append(str);
        if ($.learuntab != null) {
            $.learuntab.scrollToTab($('.menuTab.active'));
        }
    }
};

///AJAX请求
///strUrl:请求URL
///btnObj:请求按钮对像
var AjaxPostFun = function (strUrl, btnObj) {
    var btnName = btnObj.innerText;//获得控件名

    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(btnName + "异常，提交失败!", btnName + "提示");
        return;
    }

    $.ajax({
        url: GetPath(strUrl).split("?")[0],
        type: 'post',
        data: GetParamJsonByUrl(strUrl),
        success: function (result) {
            if (result != null && result != undefined && result != "") {
                RefreshFram();//刷新父级页面
                abp.message.error(btnName + "提交成功!", btnName + "提示");
            }
            else {
                abp.message.success(btnName + "成功!", btnName + "提示");
            }
        },
        error: function (xhr) {
            abp.ui.clearBusy();
            var data = JSON.parse(xhr.responseText);
            try {
                if (data.success === false) {
                    if (data.error.validationErrors) {
                        abp.message.error(data.error.details, data.error.message);
                    }
                    else {
                        abp.message.error(data.error.message, btnName + '失败');
                    }
                }
                else {
                    abp.message.error(data.responseText, btnName + '失败');
                }
            } catch (e) {
                console.log(e);
            }
            return false;
        }
    });
};


///AJAX请求
///strUrl:请求URL
///btnObj:请求按钮对像
var AjaxGetFun = function (strUrl, btnObj) {
    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(btnName + "异常", btnName + "提示");
        return;
    }
    var btnName = btnObj.innerText;
    var strUrl = GetPath(strUrl);

    $.ajax({
        url: strUrl,
        type: 'GET',
        success: function (result) {
            if (result != null && result != undefined && result != "") {
                RefreshFram();//刷新父级页面
                abp.message.success(btnName + "已提交成功", btnName + "提示");
            }
        }
    });
};

///AJAX请求
///strUrl:请求URL
///btnObj:请求按钮对像
var AjaxPostConfirm = function (strUrl, btnObj) {
    var btnName = btnObj.innerText;//获得控件名
    if (strUrl.indexOf("程序异常") >= 0) {
        abp.message.error(btnName + "异常，提交失败!", btnName + "提示");
        return;
    }

    abp.message.confirm(
    '将进行' + btnName, //确认提示
    '确定' + btnName + '?', //确认提示（可选参数）
    function (isConfirmed) {
        if (isConfirmed) {
            $.ajax({
                url: GetPath(strUrl).split("?")[0],
                type: 'post',
                data: GetParamJsonByUrl(strUrl),
                success: function (result) {
                    if (result != null && result != undefined && result != "") {
                        window.location.reload();
                        abp.message.success(btnName + "提交成功!", btnName + "提示");
                    }
                    else {
                        swal(btnName + "成功!", "success");
                        abp.message.error(btnName + "提交失败!", btnName + "提示");
                    }
                },
                error: function (xhr) {
                    abp.ui.clearBusy();
                    var data = JSON.parse(xhr.responseText);
                    try {
                        if (data.success === false) {
                            if (data.error.validationErrors) {
                                abp.message.error(data.error.details, data.error.message);
                            }
                            else {
                                abp.message.error(data.error.message, btnName + '失败');
                            }
                        }
                        else {
                            abp.message.error(data.responseText, btnName + '失败');
                        }
                    } catch (e) {
                        console.log(e);
                    }
                    return false;
                }
            });
        }
    }
);
};

//动态调用自定义js方法
var CallFunName = function (fn, args) {
    var funName = "";
    var fArr = [];//函数自带的参数集合
    var aArr = [];//传入的参数集合
    if (fn.indexOf("(") != -1 && fn.indexOf(")") != -1) {
        funName = fn.substr(0, fn.indexOf("("));
        var arss = fn.substring(fn.indexOf("(") + 1, fn.lastIndexOf(")"));
        fArr = arss.split(",");
    }
    else {
        funName = fn;
    }

    if (args != null && args != "") {
        aArr = args.split(",,,");
    }

    try {
        fn = eval(funName);
    } catch (e) {
        console.log(e);
        alert(funName + '方法不存在！');
    }
    if (typeof fn === 'function') {
        try {
           return fn.apply(this, fArr.concat(aArr));//返回函数值
        }
        catch (ex) {
            console.log(ex);
            abp.message.error(funName + '变量个数不对', '信息提示');
        }
    }
}

var SetIframID = function (strIframe) {
    if (strIframe == null || $.trim(strIframe) == "")
        return;
    var iframId = GetIfram();
    if (iframId != null && iframId != "") {
        var ModeFrameIds = GetCookie("ModeFrameIds");
        if (ModeFrameIds == undefined || ModeFrameIds == null || ModeFrameIds.toLowerCase() == "null")
            ModeFrameIds = "";
        else
            if (ModeFrameIds.length >= 20000) {//如果缓存大于1万就清除缓存
                SetCookie("ModeFrameIds", "", null);
            }

        //覆盖以前旧集合中的参数
        var strIframeVal = GetCookieParament("ModeFrameIds", strIframe);//得到已前存入的数据;
        if (strIframeVal != null && strIframeVal != "") {
            ModeFrameIds = ModeFrameIds.replace(strIframe + ":" + strIframeVal + "|", "");
        }
        ModeFrameIds += strIframe + ":" + iframId + "|";
        SetCookie("ModeFrameIds", ModeFrameIds, null);//存入cookie
    }
    return strIframe;
}
