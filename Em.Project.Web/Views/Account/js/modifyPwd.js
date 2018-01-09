
//#region 页面入口

$(document).ready(function () {
    //InitPage();
    capitalTip('oldPassword');
    capitalTip('newPassword');
    capitalTip('confirmPassword');
    InitEvent();
});

//#endregion
//密码大写输入提示  
function capitalTip(id) {
    var Html = "<div class='capslock' id='capital_" + id + "' style='display:none;float: right;'><span style='color:red;'>Capital has start-up</span></div>";
    $('#' + id).after(Html);
    var capital = false; //聚焦初始化，防止刚聚焦时点击Caps按键提示信息显隐错误  

    // 获取大写提示的标签，并提供大写提示显示隐藏的调用接口  
    var capitalTip = {
        $elem: $('#capital_' + id),
        toggle: function (s) {
            if (s === 'none') {
                this.$elem.hide();
            } else if (s === 'block') {
                this.$elem.show();
            } else if (this.$elem.is(':hidden')) {
                this.$elem.show();
            } else {
                this.$elem.hide();
            }
        }
    }
    $('#' + id).on('keydown.caps', function (e) {
        if (e.keyCode === 20 && capital) { // 点击Caps大写提示显隐切换  
            capitalTip.toggle();
        }
    }).on('focus.caps', function () { capital = false }).on('keypress.caps', function (e) { capsLock(e) }).on('blur.caps', function (e) {

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
function InitEvent() {
    $("#sumbit-btn").click(function () {

        var oldPwd = $("#oldPassword").val();
        var newPwd = $("#newPassword").val();
        var confirmPwd = $("#confirmPassword").val();

        if (oldPwd == "" || newPwd == "" || confirmPwd == "") {
            abp.message.error("Please fill in the old code and the new password", "Save failure");
            return;
        }

        if (newPwd != confirmPwd) {
            abp.message.error("The two new password input is not consistent, please retype！", "Save failure");
            return;
        }

        // 密码加密
        aesEncryptPassword();

        $("#modifyPwdForm").ajaxSubmit({
            url: bootPATH + "/Account/SaveModifiedPwd",
            success: function (data) {

                var result = data.result;

                if (result.isError) {
                    abp.message.error(result.message, "Failure to modify");

                    if (result.code == 1) {

                        $("#oldPassword").val("");
                        $("#newPassword").val("");
                        $("#confirmPassword").val("");
                    } else if (result.code == 2) {
                        abp.message.error(result.message, "Failure to modify");
                        setTimeout(function () { top.location.href = "/Account/Logout"; }, 3000);
                    } else if (result.code == 3) {

                        $("#oldPassword").val("");
                        $("#newPassword").val("");
                        $("#confirmPassword").val("");
                    }

                    return;
                } else {
                    abp.message.success("The system will automatically jump to the login page, please log in again", "Amend the success");
                    setTimeout(function () { top.location.href = bootPATH + "/Account/Logout"; }, 3000);
                }
            },
            error: function (data) {
                abp.message.error(data.result.message, "Failure");
            }
        });
    });

    //$("#modifyPwdForm").submitForm({
    //    beforeSubmit: function () {

    //        $("#sumbit-btn").button('loading');
    //    },
    //    success: function (data) {
    //        if (data.success) {
    //            $("#sumbit-btn").button('reset');
    //            abp.message.success("", "保存成功");

    //        } else {
    //            abp.message.error(data.result.message, "保存失败");
    //            $("#sumbit-btn").button('reset');
    //        }
    //    },
    //    error: function () {
    //        $("#sumbit-btn").button('reset');
    //    }
    //});
}

//对密码进行AES加密
function aesEncryptPassword() {

    var oldPwdObj = $("#oldPassword");
    var newPwdObj = $("#newPassword");
    var confirmPwdObj = $("#confirmPassword");

    var encryptedOld = aesEncrypt(oldPwdObj.val().trim());
    var encryptedNew = aesEncrypt(newPwdObj.val().trim());
    var encryptedConfirm = aesEncrypt(confirmPwdObj.val().trim());

    oldPwdObj.val(encryptedOld);
    newPwdObj.val(encryptedNew);
    confirmPwdObj.val(encryptedConfirm);
}

//#endregion

function aesEncrypt(toEncryptStr) {

    var key = CryptoJS.enc.Utf8.parse("Easyman-easyman3");
    var iv = CryptoJS.enc.Utf8.parse("Easyman-easyman3");
    var encrypted = CryptoJS.AES.encrypt(toEncryptStr, key, {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    return encrypted.toString();
}


