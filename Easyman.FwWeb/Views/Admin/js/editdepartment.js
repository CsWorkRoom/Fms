
//#region 页面入口

$(document).ready(function () {
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    $.post("../api/services/api/Icon/GetAllIcons", {}, function (data) {
        var result = data.result;

        $(result).each(function () {
            var pannel = $("<div />").appendTo($("#iconMenuDiv"));
            $("<h5 />").text(this.key).appendTo(pannel);
            var iconDiv = $("<div />").addClass("padding-10").appendTo(pannel);

            $(this.icons).each(function () {
                iconDiv.append("<span><i class='" + this.value + "'></i></span>");
            });
        });

        $("#iconMenuDiv span").click(function () {
            $("#iconMenuDiv").parent().find(".btn-value").html($(this).html());
            $("#Icon").val($(this).find('i').attr("class"));
        });
    });
}

//#endregion 

//#region 初始化事件

function InitEvent() {
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}

//#endregion

//#region 自定义项

//点击显示模态窗口，模态窗使用分部视图显示类型小图标
function MyModelTypeIcon() {
    var divicon = $("#divTypeIcon");

    if (divicon.is(':hidden')) {
        divicon.show();

    } else {
        divicon.hide();
    }
}

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


