
//#region 页面入口
$(document).ready(function () {
    InitEvent();
})
//#endregion


//#region 初始化事件
function InitEvent() {
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}
function handleSubmitForm(postUrl) {
    //修改页面Form表单action提交地址，参数
    $("#saveForm").attr("action", postUrl);
    $("#sumbit-btn").attr("type", "submit");
}
//#endregion

