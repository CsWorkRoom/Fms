
//#region 页面入口
var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
});

//#endregion

//#region 初始化页面

function InitPage() {

    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/Content/SearchContent",
        pageSize: 10,
        multiselect: true,
        fields: {
            Id: {
                title: "标识",
                filter: false,
                type: "number",
                width: "100px",
                textAlign: "center"

            },
            Title: {
                title: "标题",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            Summary: {
                title: "摘要",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            CreateUId: {
                title: "创建人",
                filter: false,
                order: false,
                type: "string",
                textAlign: "center"
            },
            CreateTime: {
                title: "创建时间",
                order: false,
                textAlign: "center",
                type: "string"
            },
            op: {
                title: "操作",
                order: false,
                width: "200px",
                textIsHtml: true,
                textAlign: "center",
                template: "<a class='menuItem' title='查看详情' href='/Content/ContentInfo?&navId=<%Id%>'>查看详情</a>" +
                    "<a class='btn btn-xs' title='编辑'><i onclick='onedit(<%Id%>)' class='fa fa-edit ' /> </a><a>" +
                    "<i onclick='ondelete(<%Id%>)' class='fa fa-remove '/> </>"
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

//#endregion


function onedit(id) {
    DiyModal.window({
        title: "编辑内容",
        url: "/Content/EditContentIndex?&navId=" + id,
        width: 850,
        height: $(window).height() * 0.9,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

function getcontentinfo(id) {
    DiyModal.window({
        title: "查看详情",
        url: "/Content/ContentInfo?&navId=" + id,
        width: 850,
        height: $(window).height() * 0.9,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

//function GetContentInfo(id) {
//    alert("AAA");
//    DiyModal.window({
//        title: "查看详情",
//        url: "/Content/ContentInfo?&navId=" + id,
//        width: 850,
//        height: 900,
//        fullscreen: false,
//        afterClose: function () {
//            table.reload();
//        }
//    }).open();
////}

//#region 自定义项

function onAdd() {
    DiyModal.window({
        title: "新增内容",
        url: "/Content/AddContentIndex",
        width: 850,
        height: $(window).height() * 0.9,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}


function ondelete(id) {
    abp.message.confirm(
        '内容将会被删除,删除后数据不可恢复', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                abp.services.api.content.delContentIndex(id).done(function (data) {
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


