var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/ScriptNode/GetScriptNodeSearch",
        pageSize: 10,
        fields: {
            Id: {
                title: "标识",
                filter: false,
                type: "number",
                width: "100px",
                textAlign: "center"

            },
            Name: {
                title: "节点名",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            //Code: {
            //    title: "节点代码",
            //    filter: true,
            //    type: "string",
            //    textAlign: "left"
            //},
            ScriptNodeTypeName: {
                title: "节点类型",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            DbServerName: {
                title: "承载数据库",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            ScriptModel: {
                title: "脚本模式",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            Remark: {
                title: "备注",
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

function onAdd() {
    //TopModeDialogUrl('modalId341', '修改、查看任务', '~/ScriptNode/EditScriptNode?ScriptNodeId=' + id, '850', '450');
    DiyModal.window({
        title: "新增任务",
        url:bootPATH+ "/ScriptNode/InsertScriptNode?TaskSpecific=''",
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
        title: "修改、查看任务",
        url:bootPATH+ "/ScriptNode/EditScriptNode?ScriptNodeId=" + id,
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
        '节点将会被删除.', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                App.post("../api/services/api/ScriptNode/DeleteScriptNode", JSON.stringify({ id: id }), function (result) {
                    abp.message.success("删除成功", "提示");
                    table.reload();
                });
            }
        }
    );
}