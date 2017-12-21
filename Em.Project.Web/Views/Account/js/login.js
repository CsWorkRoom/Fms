
//#region 页面入口

$(document).ready(function () {
    capitalTip('pwd');
    InitCheckPwd();
    InitCheckPwds();
    InitEvent();
});

//#endregion

//#region 初始化页面

//显示密码
function InitCheckPwd() {
    $(".pwsShow").click(function () {
        $("#showPwd").hide();
        //holdPwd显示
        $("#holdPwd").hide();
        if (this.id == "showPwd") {
            //holdPwd显示
            $("#holdPwd").show();
            var loginPasswd = $("#pwd").attr("type", "text");
            //同时密码框获取焦点事件
            $("#pwd").focus();
        } else {
            //showPwd显示
            $("#showPwd").show();
            var loginPasswd = $("#pwd").attr("type", "password");
        }

    });  
}
//密码框失去焦点事件
function InitCheckPwds() {
  
    $('#pwd').blur(function () {
        //showPwd显示
        $("#showPwd").show();
        //holdPwd隐藏
        $("#holdPwd").hide();
        var loginPasswd = $("#pwd").attr("type", "password");
        
    });
}
    //密码大写输入提示  
    function capitalTip(id){  
        var Html = "<div class='capslock' id='capital_" + id + "' style='display:none;float: right;'><span style='color:red;'>大写锁定已开启</span></div>";
        $('#' + id).after(Html);
        var capital = false; //聚焦初始化，防止刚聚焦时点击Caps按键提示信息显隐错误  
          
        // 获取大写提示的标签，并提供大写提示显示隐藏的调用接口  
        var capitalTip = {  
            $elem: $('#capital_'+id),  
            toggle: function (s) {  
                if(s === 'none'){  
                    this.$elem.hide();  
                }else if(s === 'block'){  
                    this.$elem.show();  
                }else if(this.$elem.is(':hidden')){  
                    this.$elem.show();  
                }else{  
                    this.$elem.hide();  
                }  
            }  
        }  
        $('#' + id).on('keydown.caps',function(e){  
            if (e.keyCode === 20 && capital) { // 点击Caps大写提示显隐切换  
                capitalTip.toggle();  
            }  
        }).on('focus.caps',function(){capital = false}).on('keypress.caps',function(e){capsLock(e)}).on('blur.caps',function(e){  
              
            //输入框失去焦点，提示隐藏  
            capitalTip.toggle('none');  
        });  
        function capsLock(e) {
            var keyCode = e.keyCode || e.which;// 按键的keyCode  
            var isShift = e.shiftKey || keyCode === 16 || false;// shift键是否按住  
            if (keyCode === 9) {
                capitalTip.toggle('none');
            } else {
                //指定位置的字符的 Unicode 编码 , 通过与shift键对于的keycode，就可以判断capslock是否开启了  
                // 90 Caps Lock 打开，且没有按住shift键  
                if (((keyCode >= 65 && keyCode <= 90) && !isShift) || ((keyCode >= 97 && keyCode <= 122) && isShift)) {
                    // 122 Caps Lock打开，且按住shift键  
                    capitalTip.toggle('block'); // 大写开启时弹出提示框  
                    capital = true;
                } else {
                    capitalTip.toggle('none');
                    capital = true;
                }
            }
        }
    };
    //调用提示  
   

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
           
            var isResult = data.result.isError;
            if (isResult==false) {
                $('#error-message').html("");
                window.location.href = data.result.message;
            } else {
                $("#submit").val("登录");
                $("#submit").attr("disabled", false);
                $('#error-message').html(data.result.message);
                $("#pwd").val("");
                $("#vertify").val("");
                ToggleCode($("#verifyCodeInfo"), 'GetVerifyCode');
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
    pwdObj.type = "password";

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


//
