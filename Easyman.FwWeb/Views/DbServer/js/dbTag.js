
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
        action: "../api/services/api/DbTag/GetDbTagSearch",
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
                title: "名称",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            Remark: {
                title: "说明",
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
                className: "btn-success",
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
        title: "新增标识",
        url: bootPATH + "/DbServer/InserDbTag",
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
        title: "编辑标识",
        url: bootPATH + "/DbServer/EditDbTag?&dbTagId=" + id,
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
        '数据库标识将会被删除.', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                App.post("../api/services/api/DbTag/DeleteDbTag", JSON.stringify({ id: id }), function (result) {
                    abp.message.success("删除成功", "提示");
                    table.reload();
                });
            }
        }
    );
}

//#endregion


