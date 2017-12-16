
//#region 页面入口

$(document).ready(function () {
    InitEvent();
})

//#endregion



//#region 初始化事件

function InitEvent() {
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}