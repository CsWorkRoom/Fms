
//#region 页面入口
$(document).ready(function () {
    InitEvent();
})
//#endregion


//#region 初始化事件
function InitEvent() {
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}
//#endregion



$(document).click(function (e) {
    var _con = $('#divTypeIcon');   // 设置目标区域
    if (!_con.is(e.target) && _con.has(e.target).length === 0) {
        $('#divTypeIcon').hide();
    }
})

//部分视图传参数
function ContentIconType(value) {
    
    $("#divTypeIcon").hide();
    $("#icon_type_img").attr("class", value);
    $("#Icon").val($("#icon_type_img").attr("class"));
   
}

//#endregion


