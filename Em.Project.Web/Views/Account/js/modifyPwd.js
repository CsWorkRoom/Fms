
//#region 页面入口

$(document).ready(function () {
    //InitPage();
    InitEvent();
});

//#endregion

function InitEvent() {
    $("#sumbit-btn").click(function () {

        var oldPwd = $("#oldPassword").val();
        var newPwd = $("#newPassword").val();
        var confirmPwd = $("#confirmPassword").val();

        if (oldPwd == "" || newPwd == "" || confirmPwd == "") {
            abp.message.error("请填写旧密码和新密码", "提交失败");
            return;
        }

        if (newPwd != confirmPwd) {
            abp.message.error("两次新密码输入不一致，请重新输入！", "提交失败");
            return;
        }

        // 密码加密
        aesEncryptPassword();

        $("#modifyPwdForm").ajaxSubmit({
            url: "/Account/SaveModifiedPwd",
            success: function (data) {
                
                var result = data.result;

                if (result.isError) {
                    abp.message.error(result.message, "修改失败");

                    if (result.code == 1) {

                        $("#oldPassword").val("");
                        $("#newPassword").val("");
                        $("#confirmPassword").val("");
                    } else if (result.code == 2) {
                        abp.message.error(result.message, "修改失败");
                        setTimeout(function () { top.location.href = "/Account/Logout"; }, 3000);
                    } else if (result.code == 3) {

                        $("#oldPassword").val("");
                        $("#newPassword").val("");
                        $("#confirmPassword").val("");
                    }

                    return;
                } else {
                    abp.message.success("系统将自动跳转至登录页面，请重新登录", "修改成功");
                    setTimeout(function () { top.location.href = "/Account/Logout"; }, 3000);
                }
            },
            error:function(data) {
                abp.message.error(data.result.message, "错误");
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


