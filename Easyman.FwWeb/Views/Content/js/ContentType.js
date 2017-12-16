
//#region 页面入口
var table;
$(document).ready(function() {
    InitPage();
    InitEvent();
   
});

//#endregion

//#region 初始化页面

function InitPage() {

    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/Content/SearchContentType",
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
            Name: {
                title: "名称",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            DefineName: {
                title: "内容块",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            ParentName: {
                title: "上级",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            ShowOrder: {
                title: "排序",
                order: false,
                textAlign: "center",
                type: "string"
            } ,
            op: {
                title: "操作",
                order: false,
                width: "100px",
                textIsHtml: true,
                textAlign: "center",
                template: "<a class='btn btn-xs' title='编辑'><i onclick='onedit(<%Id%>)' class='fa fa-edit ' /> </a><a><i onclick='ondelete(<%Id%>)' class='fa fa-remove '/> </>"
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
        title: "编辑内容类别",
        url: bootPATH + "/Content/EditContentType?&navId=" + id,
        width: 850,
        height: 600,
        fullscreen: true,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

//#region 自定义项

function onAdd() {
    DiyModal.window({
        title: "新增内容类别",
        url: bootPATH + "/Content/AddContentType",
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
        '内容定义将会被删除.', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                abp.services.api.content.delContentType(id).done(function (data) {
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


