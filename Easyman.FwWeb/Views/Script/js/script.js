var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/Script/GetAllScript",
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
                title: "任务组名称",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            ScriptTypeName: {
                title: "任务组类型",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            Cron: {
                title: "时间表达式",
                filter: true,
                type: "string",
                textAlign: "left"
            }, 
            StatusName: {
            title: "任务状态",
            filter: true,
            type: "string",
            textAlign: "left"
            },
            RetryTime: {
                title: "失败重启次数",
                filter: true,
                type: "number",
                textAlign: "left"
            },
            Remark: {
                title: "备注",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            CreateTime: {
                title: "创建时间",
                filter: false,
                type: "string",
                order: false,
                textAlign: "center",
            },
            CreatorUserId: {
                title: "创建人",
                filter: false,
                type: "number",
                order: false,
                textAlign: "center",
            },
            op: {
                title: "操作",
                order: false,
                width: "120px",
                textIsHtml: true,
                textAlign: "center",
                template: "<a><i onclick='onedit(<%Id%>)' class='fa fa-edit ' /> </a> <a><i onclick='ondelete(<%Id%>)' class='fa fa-remove '/></> <a><i onclick='onquery(<%Id%>)' class='fa fa-search' /></a>"
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

function onquery(id) {
    window.location.href = "/Script/ExampleScript";
}
function onAdd() {
    //DiyModal.window({
    //    title: "新增脚流本",
    //    url: bootPATH +"/Script/EditScript",
    //    width: 850,
    //    height: 550,
    //    fullscreen: false,
    //    afterClose: function () {
    //        table.reload();
    //    }
    //}).open();
    window.location.href = "/Script/EditScript";
}


function onedit(id) {
    //DiyModal.window({
    //    title: "修改脚本流",
    //    url: bootPATH +"/Script/EditScript?ScriptId=" + id,
    //    width: 850,
    //    height: 550,
    //    fullscreen: false,
    //    afterClose: function () {
    //        table.reload();
    //    }
    //}).open();
    window.location.href = "/Script/EditScript?ScriptId=" + id;
}

function ondelete(id) {
    abp.message.confirm(
        '脚本流将会被删除.', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                abp.services.api.script.delScript(id).done(function (data) {
                    abp.message.success("删除成功", "提示");
                    table.reload();
                }).fail(function (data) {
                    abp.message.error("删除失败", "提示");
                });

            }
        }
    );
}