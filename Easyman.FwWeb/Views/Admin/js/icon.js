
//#region 页面入口
var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {

    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/Icon/GetAll",
        pageSize: 10,
        multiselect: true,
        fields: {
            Id: {
                title: "标识",
                filter: false,
                type: "number",
                width: "100px",
                textAlign: "center",

            },
            GroupName: {
                title: "图标名称",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            IconClass: {
                title: "图标",
                filter: false,
                order: false,
                type: "string",
                textAlign: "center"
            },
            op: {
                title: "操作",
                order: false,
                width: "100px",
                textIsHtml: true,
                textAlign: "center",
                template:    "<a><i onclick='onedit(<%Id%>)' class='fa fa-edit ' /></a>&nbsp;<a><i onclick='ondelete(<%Id%>)' class='fa fa-remove '/></>"
            },
        },
        buttons: {
            insert: {
                text: "新增",
                icon: "fa-plus",
                style: "",
                className: "btn-insert",
                event: onAdd,
                index: 1
            }
        }
    });
}

//#endregion 

//#region 初始化事件

function InitEvent() {

}

//#endregion

//#region 自定义项

function onAdd() {
    DiyModal.window({
        title: "新增图标",
        url: bootPATH + "/Admin/EditIcon",
        width: 850,
        height: 600,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

function onedit(id) {
    DiyModal.window({
        title: "编辑图标",
        url: bootPATH + "/Admin/EditIcon?&id=" + id,
        width: 850,
        height: 600,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

function ondelete(id) {
    abp.message.confirm(
        '图标将会被删除.', //确认提示
        '确定注销?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                abp.services.api.icon.del(id).done(function (data) {
                    abp.message.success("删除成功", "提示");
                    table.reload();
                }).fail(function (data) {
                    abp.message.error("删除失败", "提示");
                });
            }
        }
    );
}

//#endregion


