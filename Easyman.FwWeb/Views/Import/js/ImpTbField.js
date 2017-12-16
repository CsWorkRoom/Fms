var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/ImpTbField/GetAll?impTbId=" + $("#impTbId").val(),
        pageSize: 10,
        fields: {
            Id: {
                title: "标识",
                filter: false,
                type: "number",
                width: "100px",
                textAlign: "center",

            },
            FieldCode: {
                title: "编码",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            FieldName: {
                title: "别名",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            DataType: {
                title: "数据类型",
                filter: false,
                type: "string",
                textAlign: "left"
            },
            //RegularName: {
            //    title: "正则表达式",
            //    filter: false,
            //    type: "string",
            //    textAlign: "left"
            //},
            //CreateTime: {
            //    title: "创建时间",
            //    filter: false,
            //    type: "string",
            //    textAlign: "left"
            //},
            //ImpTbName: {
            //    title: "归属表",
            //    filter: false,
            //    type: "string",
            //    textAlign: "left"
            //},
            Remark: {
                title: "备注",
                filter: false,
                type: "string",
                textAlign: "left"
            },
            op: {
                title: "操作",
                order: false,
                width: "100px",
                textIsHtml: true,
                textAlign: "center",
                template: "<a><i onclick='onedit(<%Id%>)' class='fa fa-edit'/></a>" +
                          "<a><i onclick='ondelete(<%Id%>)' class='fa fa-remove '/> </a>"
            }
        },
        buttons: {
            insert: {
                text: "新增",
                icon: "fa-plus",
                style: "",
                className: "btn-insert",
                event: onAdd,
                index: 1
            },
            "save": {
                text: "保存",
                icon: "fa-plus",
                style: "",
                className: "btn-success",
                event: onSave,
                index: 1
            }
        }
    });
}

//#endregion 

//#region 初始化事件

function InitEvent() {

}

function onAdd() {
    var impTbId = $("#impTbId").val();
    var dbTypeId = $("#dbTypeId").val();
    DiyModal.window({
        title: "新增字段",
        url: bootPATH + "/Import/EditImpTbField?impTbId=" + impTbId + "&dbTypeId=" + dbTypeId,
        width: 850,
        height: 550,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

function onedit(id, impTbId) {
    var impTbId = $("#impTbId").val();
    var dbTypeId = $("#dbTypeId").val();
    DiyModal.window({
        title: "修改字段",
        url: bootPATH + "/Import/EditImpTbField?id=" + id + "&impTbId=" + impTbId + "&dbTypeId=" + dbTypeId,
        width: 850,
        height: 550,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

function ondelete(id) {
    abp.message.confirm(
        '字段将会被删除.', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                abp.services.api.impTbField.del(id).done(function (data) {
                    abp.message.success("删除成功", "提示");
                    table.reload();
                }).fail(function (data) {
                    abp.message.error("删除失败", "提示");
                });

            }
        }
    );
}

function onSave() {
    var impTbId = $("#impTbId").val();
    $.ajax({
        url: bootPATH+"Import/SaveSqlScript",
        data: { id: impTbId },
        dataType: "json",
        type: "post",
        async: true,
        success: function (data) {
            if (data.state) {
                abp.message.success("", data.msg);

            } else {
                abp.message.error(data.msg, "保存失败");
            }
            //alert(data.msg);
        },
        error: function () {
            alert("error");
            alert(bootPATH + "Import/GetJson");
        }
    });
}