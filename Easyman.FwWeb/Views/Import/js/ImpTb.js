var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/ImpTb/GetAll",
        pageSize: 10,
        fields: {
            Id: {
                title: "标识",
                filter: false,
                type: "number",
                width: "100px",
                textAlign: "center",

            },
            Code: {
                title: "代码",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            CnTableName: {
                title: "表别名",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            EnTableName: {
                title: "表名称",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            Rule: {
                title: "建表规则",
                filter: false,
                type: "string",
                textAlign: "left"
            },
            DbServerName: {
                title: "执行库",
                filter: false,
                type: "string",
                textAlign: "left"
            },
            ImpTypeName: {
                title: "外导表分类",
                filter: false,
                type: "string",
                textAlign: "left"
            },
            op: {
                title: "操作",
                order: false,
                width: "150px",
                textIsHtml: true,
                textAlign: "center",
                template: "<a><i onclick='onedit(<%Id%>)' class='fa fa-edit ' /></a>" +
                          " <a><i onclick='onaddfor(<%Id%>)' class='fa fa-cog ' /> </a>" +
                          " <a><i onclick='ondelete(<%Id%>)' class='fa fa-remove '/> </a>"
                          
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
            "import": {
                text: "导入",
                icon: "fa-plus",
                style: "",
                className: "btn-import",
                event: onImport,
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
        title: "新增定义",
        url: bootPATH + "/Import/EditImpTb",
        width: 850,
        height: 550,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

function onaddfor(id) {
    DiyModal.window({
        title: "字段管理",
        url: bootPATH + "/Import/ImpTbFieldPage?id=" + id,
        width: 900,
        height: 600,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

function onImport() {
    DiyModal.window({
        title: "导入",
        url: bootPATH + "/Import/CommonImport?importCode=CJ&moduleCode=M30006",
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
        title: "修改定义",
        url: bootPATH + "/Import/EditImpTb?id=" + id,
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
        '信息将会被删除.', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                abp.services.api.impTb.del(id).done(function (data) {
                    abp.message.success("删除成功", "提示");
                    table.reload();
                }).fail(function (data) {
                    abp.message.error("删除失败", "提示");
                });
            }
        }
    );
}