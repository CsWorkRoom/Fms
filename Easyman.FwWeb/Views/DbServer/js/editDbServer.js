
//#region 页面入口

$(document).ready(function () {
    InitPage();
    //InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    var pwd = $("#Password").val();
    if (pwd != null && pwd != "") {
        var dPwd = Decrypt(pwd);
        if (dPwd == "" || dPwd == null) {
            $("#dbPwd").val(pwd);//当解密的内容为空，则赋原值
        }
        else
            $("#dbPwd").val(dPwd);//解密后赋值给显示密码的控件
    }
}

//#endregion 


//#region 自定义项
function submitDb()
{
    var pwd = $("#dbPwd").val();
    if (pwd != null && pwd != "") {
        $("#Password").val(Encrypt(pwd));//加密后传入到后台保存
    }
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}

//#endregion


