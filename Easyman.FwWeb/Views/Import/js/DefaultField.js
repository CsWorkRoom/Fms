var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/DefaultField/GetAll",
        pageSize: 10,
        fields: {
            Id: {
                title: "标识",
                filter: false,
                type: "number",
                width: "100px",
                textAlign: "center",

            },
            FieldName: {
                title: "字段别名",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            FieldCode: {
                title: "字段编码",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            DataType: {
                title: "字段类型",
                filter: false,
                type: "string",
                textAlign: "left"
            },
            DefaultValue: {
                title: "默认值",
                filter: false,
                type: "string",
                textAlign: "left"
            },
            Remark: {
                title: "字段说明",
                filter: false,
                type: "string",
                textAlign: "left"
            },
            CreateTime: {
                title: "创建时间",
                filter: false,
                type: "string",
                textAlign: "left"
            },
            DbTypeName: {
                title: "数据库类型",
                filter: true,
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
        title: "新增内置字段",
        url: bootPATH + "/Import/EditDefaultField",
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
        title: "修改内置字段",
        url: bootPATH + "/Import/EditDefaultField?id=" + id,
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
        '内置字段将会被删除.', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                abp.services.api.defaultField.del(id).done(function (data) {
                    abp.message.success("删除成功", "提示");
                    table.reload();
                }).fail(function (data) {
                    abp.message.error("删除失败", "提示");
                });

            }
        }
    );
}