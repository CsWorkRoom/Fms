
//#region 页面入口

$(document).ready(function () {
    InitPage();
    InitEvent();
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
    IsSubmit();
    $("#saveForm").submit();
}