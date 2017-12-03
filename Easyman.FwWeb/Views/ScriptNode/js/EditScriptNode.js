
//#region 页面入口

$(document).ready(function () {
    InitPage();
    InitEvent();
    InitTbMsg();
})

//#endregion

//#region 初始化页面

function InitPage() {
}

//#endregion 

//#region 初始化事件

function InitEvent() {
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}

function handleSubmitForm() {
    $("#saveForm").submit();
}


//显示或隐藏建表相关的控件
function InitTbMsg() {

    var val = $('input:radio[name="ScriptModel"]:checked').val();
    if (val == "2") {
        $("#tbGroup").hide();
    }
    else if (val == "1") {
        $("#tbGroup").show();
    }
}

$(function () {
    $('input:radio[name="ScriptModel"]').click(function () {
        InitTbMsg();
    });
});