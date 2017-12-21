// 当前系统名
var sysName = "";

// 当前UserName
var userName = "";

$(window).resize(function () {
    var w = $(document).width();
    var h = $(document).height();
    try {
        DrawWaterMark(w, h, sysName, userName);
    } catch (e) {
        return;
    }
});

//#region 校验页面和事件权限
$(document).ready(function () {
    ValidataUrlRole(); //注释则关闭页面端验证
    // 初始化水印
    InitBgWatermarkByCanvas("body");
    try {
        // 绘制水印
        DrawWaterMark($(document).width(), $(document).height(), sysName, userName);
    } catch (e) {
        return;
    }
});


//校验url权限
var ValidataUrlRole = function () {
    //var urlHost = window.location.host;
    var urlFull = window.location.href;
    var urlModule = urlFull.split(bootPATH)[1];//获得当前页面的module路径

    //校验url权限ValidateUrl
    var data = $.ajax({
        url: bootPATH + "/Admin/ValidateUrlRole",
        data: { url: urlModule },
        async: false
    });
    if (data != null && data.responseText != null && data.responseText.length > 0 && data.responseText.toLowerCase() == "false") {
        SendErrorInfo("权限提示", "当前用户无此页面的访问权限！");
        //window.location.href = "/Home/NoAccess";
        //abp.message.error("当前用户不具备该页访问权限！\r\n请赋权后再操作", "权限提示：");
    }
    else {
    }
}


function InitBgWatermarkByCanvas(target) {
    // 声明画布
    var convas = '<canvas class="watermark" width = "200px"  height = "150px" style="display:none;"></canvas>' + '<canvas class="repeat-watermark"></canvas>';

    //系统名称
    var sysNameObj = parent.top.$("#sysName");
    if (sysNameObj == null || sysNameObj.val()==null || sysNameObj.val() == "") {
        var sysNameResponse = $.ajax({
            url: bootPATH + "api/services/api/User/GetCurrentSysName",
            type: "post",
            async: false
        });
        sysName = sysNameResponse.responseJSON.result;
        if (sysNameObj != null)
            sysNameObj.val(sysName);
    } else {
        sysName = sysNameObj.val();
    }
    //登录名称
    var userNameObj = parent.top.$("#LgUName");
    if (userNameObj == null || userNameObj.val()==null || userNameObj.val() == "") {
        var userNameResponse = $.ajax({
            url: bootPATH + "api/services/api/User/GetCurrentUserName",
            type: "post",
            async: false
        });
        userName = userNameResponse.responseJSON.result;
        if (userName != null)
            userNameObj.val(userName);
    } else {
        userName = userNameObj.val();
    }
    sysName = $.trim(sysName);
    userName = $.trim(userName);
    
    var currentTime = new Date();
    userName +="  "
        + currentTime.getFullYear() + "-"
        + (currentTime.getMonth() + 1) + "-"
        + currentTime.getDate();

    if (userName != "") {
        $(target).append(convas);
    }
}

function DrawWaterMark(docWidth, docHeight, sysName, userName) {
    var cw = $('.watermark')[0];
    var crw = $('.repeat-watermark')[0];

    crw.width = docWidth;
    crw.height = docHeight;

    var ctx = cw.getContext("2d");

    //清除小画布
    ctx.clearRect(0, 0, 200, 150);
    ctx.font = "16px Arial";

    //文字倾斜角度
    ctx.rotate(-20 * Math.PI / 180);

    ctx.fillStyle = "rgba(219, 219, 234, 0.7)";
    //第一行文字
    ctx.fillText(sysName, -20, 80);

    //第二行文字 
    ctx.fillText(userName, -20, 100);

    //坐标系还原
    ctx.rotate(20 * Math.PI / 180);
    var ctxr = crw.getContext("2d");
    //清除整个画布
    ctxr.clearRect(0, 0, crw.width, crw.height);
    //平铺--重复小块的canvas
    var pat = ctxr.createPattern(cw, "repeat");
    ctxr.fillStyle = pat;

    ctxr.fillRect(0, 0, crw.width, crw.height);
}


function InitBgWatermark() {
    var userId = 0;
    var response = $.ajax({
        url: bootPATH + "api/services/api/User/GetCurrentUserId",
        type: "post",
        async: false
    });
    userId = response.responseJSON.result;

    if (userId != 0) {
        var bgUrl = bootPATH + "UpFiles/Bg/" + userId + ".jpg";
        $("body").css("background", "url(" + bgUrl + ")");
    }


}