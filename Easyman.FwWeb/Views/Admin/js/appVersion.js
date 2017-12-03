
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
        action: "../api/services/api/AppVersion/GetAppVersionSearch",
        pageSize: 10,
        fields: {
            Id: {
                title: "标识",
                filter: false,
                type: "number",
                width: "100px",
                textAlign: "center",

            },
            VersionCode: {
                title: "版本编码",
                filter: true,
                type: "number",
                textAlign: "left"
            },
            VersionName: {
                title: "版本名",
                filter: true,
                type: "string",
                textAlign: "left"
            },
            Type: {
                title: "APP类型",
                filter: true,
                order: false,
                type: "string",
                textAlign: "left"
            },
            IsNew: {
                title: "是否最新",
                filter: false,
                order: false,
                type: "string",
                textAlign: "left",
                template: function (data) {
                    var text = "";
                    switch (data.isNew) {
                        case 1:
                            text = "是";
                            break;
                        case 0:
                            text = "否";
                            break;
                        default:
                            break;
                    }

                    return "<span>" + text + "</span>";
                }
            },
            IsMust: {
                title: "是否强制更新",
                filter: false,
                order: false,
                type: "string",
                textAlign: "left",
                template: function (data) {
                    var text = "123";
                    switch (data.isMust) {
                        case 1:
                            text = "是";
                            break;
                        case 0:
                            text = "否";
                            break;
                        default:
                            break;
                    }

                    return "<span>" + text + "</span>";
                }
            },
            UpdateUrl: {
                title: "版本更新地址",
                order: false,
                textAlign: "center"
            },
            UpdateTime: {
                title: "更新时间",
                filter: false,
                type: "date",
                format: "yyyy-mm-dd",
                order: false,
                textAlign: "center",
            },
            op: {
                title: "操作",
                order: false,
                width: "100px",
                textIsHtml: true,
                textAlign: "center",
                template: "<a><i onclick='onedit(<%Id%>)' class='fa fa-edit ' /></a>&nbsp;<a><i onclick='ondelete(<%Id%>)' class='fa fa-remove '/></>"
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
        title: "新增APP版本",
        url: "/Admin/CreateAppVersion",
        width: 850,
        height: 620,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}


function onedit(id) {
    DiyModal.window({
        title: "编辑APP版本",
        url: "/Admin/AppVersionEdit?&versionId=" + id,
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
        '版本信息将会被删除.', //确认提示
        '确定删除?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                App.post("../api/services/api/AppVersion/DeleteAppVersion", JSON.stringify({ id: id }), function (result) {
                    abp.message.success("删除成功", "提示");
                    table.reload();
                });
            }
        }
    );
}

//#endregion


