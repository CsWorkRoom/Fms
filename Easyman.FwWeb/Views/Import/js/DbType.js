var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/DbType/GetAll",
        pageSize: 10,
        fields: {
            Id: {
                title: "标识",
                filter: false,
                type: "number",
                width: "100px",
                textAlign: "center",

            },
            Name: {
                title: "数据库名称",
                filter: true,
                type: "string",
                textAlign: "left"
            },
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
                template:    "<a><i onclick='onedit(<%Id%>)' class='fa fa-edit ' /> </a> <a><i onclick='ondelete(<%Id%>)' class='fa fa-remove '/> </>"
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
            }
        }
    });
}

//#endregion 

//#region 初始化事件

function InitEvent() {

}

function onAdd() {
    DiyModal.window({
        title: "新增数据库类型",
        url: "/Import/EditDbType",
        width: 850,
        height: 550,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}


function onedit(id) {
    DiyModal.window({
        title: "修改数据库类型",
        url: "/Import/EditDbType?id=" + id,
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
        '数据库类型将会被删除.', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                abp.services.api.dbType.del(id).done(function (data) {
                    abp.message.success("删除成功", "提示");
                    table.reload();
                }).fail(function (data) {
                    abp.message.error("删除失败", "提示");
                });
            }
        }
    );
}