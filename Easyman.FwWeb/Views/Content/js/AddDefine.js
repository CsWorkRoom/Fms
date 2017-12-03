
//#region 页面入口
var table;
$(document).ready(function() {
    InitPage();
    InitEvent();
});

//#endregion

//#region 初始化页面
function InitPage() {
    GetCheckbox();
    ///$("#text_PushTypeId").val("-请选择-");
}

//#endregion 

//#region 初始化事件

function InitEvent() {

    $("#sumbit-btn").click(function () {
        GetCheckbox();
        $("#saveForm").submit();
    });

    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}


//#endregion

//#region 自定义项
function GetCheckbox() {

    if ($("#IsReoly").is(':checked'))
        $("#IsReoly").val("true");
    else
        $("#IsReoly").val("false");
    if ($("#IsReolyFile").is(':checked'))
        $("#IsReolyFile").val("true");
    else
        $("#IsReolyFile").val("false");
    if ($("#IsReolyFloor").is(':checked'))
        $("#IsReolyFloor").val("true");
    else
        $("#IsReolyFloor").val("false");
    if ($("#IsReolyFloorFile").is(':checked'))
        $("#IsReolyFloorFile").val("true");
    else
        $("#IsReolyFloorFile").val("false");
    if ($("#IsText").is(':checked'))
        $("#IsText").val("true");
    else
        $("#IsText").val("false");
    if ($("#IsLike").is(':checked'))
        $("#IsLike").val("true");
    else
        $("#IsLike").val("false");
    if ($("#IsDelete").is(':checked'))
        $("#IsDelete").val("true");
    else
        $("#IsDelete").val("false");
    if ($("#IsShare").is(':checked'))
        $("#IsShare").val("true");
    else
        $("#IsShare").val("false");

    if ($("#IsChenkUser").is(':checked'))
        $("#IsChenkUser").val("true");
    else
        $("#IsChenkUser").val("false");

    if ($("#IsChenkRole").is(':checked'))
        $("#IsChenkRole").val("true");
    else
        $("#IsChenkRole").val("false");

    if ($("#IsChenkDistrict").is(':checked'))
        $("#IsChenkDistrict").val("true");
    else
        $("#IsChenkDistrict").val("false");

    if ($("#IsContentFile").is(':checked'))
        $("#IsContentFile").val("true");
    else
        $("#IsContentFile").val("false");


}

//#endregion


