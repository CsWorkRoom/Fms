
//#region 页面入口

$(document).ready(function () {
    //InitPage();
    InitEvent();
});

//#endregion

//#region 初始化页面

function InitPage() {
    InitSupersized();
}

//#endregion 

//#region 初始化事件

function InitEvent() {

    $("#loginForm").ajaxForm({
        beforeSubmit: function () {
            // To do something before submit
            $("#submit").val("登录中...");
            $("#submit").attr("disabled", true);
        },
        success: function (data) {
            if (data.success) {
                $('#error-message').html("");
                window.location.href = data.targetUrl;
            } else {
                $("#submit").val("登录");
                $("#submit").attr("disabled", false);
                $('#error-message').html(data.result.message);
            }
        },
        error: function (e) {

            $("#submit").val("登录");
            $("#submit").attr("disabled", false);

            $("#pwd").val("");
            $("#vertify").val("");
            ToggleCode($("#verifyCodeInfo"), 'GetVerifyCode');

            //abp.message.error(e.result.message, "登录失败");

            //#region 抛出错误
            if (e.responseText.indexOf("<title>") != -1) {
                var start = e.responseText.indexOf("<title>");
                var end = e.responseText.indexOf("</title>");
                var error = e.responseText.substring(start + 7, end);
                $('#error-message').html(error);

                if (error.indexOf("即将跳入密码修改页面") != -1) {
                    swal({
                        title: "即将跳转至密码修改页面",
                        text: error,
                        type: "warning",
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "确定",
                    },
                    modifyPwd);
                    //setTimeout(modifyPwd(), 9000);
                }
            }
            else
                $('#error-message').html(e.responseText);
            //$('#error-message').html(JSON.parse(e.responseText).error.details);
            //#endregion

            $("#pwd").focus();
        }
    });
    $('#ReturnUrlHash').val(location.hash);

}

var modifyPwd = function () {
    var modalId = CreateRandomNum(1, 0, 1000);//取0到1000的随机数
    TopModeDialogUrl("modalId" + modalId, "修改密码", "Account/ModifyPassword?username=" + $("#UsernameOrEmailAddress").val(), 560, 350);
}

//#endregion

//#region 自定义项

function InitSupersized() {
    $.supersized({
        // Functionality
        slide_interval: 6000,    // Length between transitions
        transition: 1,    // 0-None, 1-Fade, 2-Slide Top, 3-Slide Right, 4-Slide Bottom, 5-Slide Left, 6-Carousel Right, 7-Carousel Left
        transition_speed: 3000,    // Speed of transition
        performance: 1,    // 0-Normal, 1-Hybrid speed/quality, 2-Optimizes image quality, 3-Optimizes transition speed // (Only works for Firefox/IE, not Webkit)

        // Size & Position
        min_width: 0,    // Min width allowed (in pixels)
        min_height: 0,    // Min height allowed (in pixels)
        vertical_center: 1,    // Vertically center background
        horizontal_center: 1,    // Horizontally center background
        fit_always: 0,    // Image will never exceed browser width or height (Ignores min. dimensions)
        fit_portrait: 1,    // Portrait images will not exceed browser height
        fit_landscape: 0,    // Landscape images will not exceed browser width

        // Components
        slide_links: 'blank',    // Individual links for each slide (Options: false, 'num', 'name', 'blank')
        slides: [    // Slideshow Images
                                 { image: '/Views/Account/img/1.jpg' },
                                 { image: '/Views/Account/img/2.jpg' },
                                 { image: '/Views/Account/img/3.jpg' },
								 { image: '/Views/Account/img/4.jpg' }
        ]

    });
}

//js 登录校验
function validationLogin() {
    var u = $("input[name=UsernameOrEmailAddress]");
    var p = $("input[name=Password]");
    if (u.val() == '' || p.val() == '') {
        $("#ts").html("用户名与密码不能为空");
        return false;
    }
}

//对密码进行AES加密
function aesEncryptPassword() {
    var pwdObj = $("#pwd");

    var key = CryptoJS.enc.Utf8.parse("Easyman-easyman3");
    var iv = CryptoJS.enc.Utf8.parse("Easyman-easyman3");
    var encryptedPwd = CryptoJS.AES.encrypt(pwdObj.val().trim(), key, {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    pwdObj.val(encryptedPwd.toString());
}

//#endregion

function ToggleCode(obj, codeurl) {
    $(obj).children("img").eq(0).attr("src", codeurl + "?time=" + Math.random());
    return false;
}

///*
//生成随机数列表（可能会有重复）
//intLentgh：要产生多少个随机数
//intMinNum：产生随机数的最小值
//intMaxNum：产生随机数的最大值
//*/
//var CreateRandomNum = function (intLentgh, intMinNum, intMaxNum) {
//    var arr = [];
//    for (var i = intMinNum; i <= intMaxNum; i++)
//        arr.push(i);
//    arr.sort(function () {
//        return 0.5 - Math.random();
//    });
//    arr.length = intLentgh;
//    return arr;
//}

/////读取IFRAMID
//var GetIfram = function () {
//    //查询父级IFRAM的ID集
//    var framIds = parent.frames;
//    var framId = "";//IFRAM的ID
//    for (var i = 0; i < framIds.length; i++) {
//        if (framIds[i].location.href == location.href) {
//            framId = framIds[i].name;
//            break;
//        }
//    }
//    return framId;
//}

/////根据相对路径得到完整URL
/////strUrl:URL相对地址
//var GetPath = function (strUrl) {
//    if (strUrl.toLowerCase().indexOf("https:") != -1 || strUrl.toLowerCase().indexOf("http:") != -1 || strUrl.toLowerCase().indexOf("file:") != -1) {
//        return strUrl;
//    }

//    if (strUrl.indexOf("/") == 0 || strUrl.indexOf("~/") == 0) {
//        strUrl = bootPATH + strUrl.replace("~/", "/").substr(1);
//    }
//    else {
//        strUrl = bootPATH + strUrl;
//    }
//    return strUrl;
//};