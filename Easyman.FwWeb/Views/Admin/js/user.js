
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
        action: "../api/services/api/User/Search",
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
            UserName: {
                title: "工号",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            Name: {
                title: "姓名",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            DistrictGroupName: {
                title: "地区",
                filter: false,
                order: false,
                type: "string",
                textAlign: "center"
            },
            DepartmentName: {
                title: "部门",
                order: false,
                textAlign: "center",
                type: "string",
            },
            State: {
                title: "状态",
                order: false,
                textAlign: "center",
                textIsHtml: true,
                template: function (data) {
                    var className = "";
                    switch (data.state) {
                        case "正常":
                            className = "text-success";
                            break;
                        case "锁定":
                            className = "text-warning";
                            break;
                        case "注销":
                            className = "text-error";
                            break;
                        default:
                            className = "text-success";
                    }

                    return "<span class='" + className + "'>" + data.state + "</span>";
                }
            },
            creationTime: {
                title: "创建时间",
                filter: false,
                type: "date",
                width: "180px",
                format: "yyyy-mm-dd hh:ii:ss",
                order: false,
                textAlign: "center",
            },
            LastLoginTime: {
                title: "最后登录时间",
                filter: false,
                width: "180px",
                type: "date",
                format: "yyyy-mm-dd hh:ii:ss",
                order: false,
                textAlign: "center",
            },
            op: {
                title: "操作",
                order: false,
                width: "100px",
                textIsHtml: true,
                textAlign: "center",
                template: "<a class='btn btn-xs' title='编辑'><i onclick='onedit(<%Id%>)' class='fa fa-edit ' /> </a>" +
                    '<div class="btn-group ">' +
                    '<a class="btn btn-xs" title="更多"  data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i  class="fa fa-cog"/> </a>' +
                    '<ul class="dropdown-menu dropdown-menu-right">' +
                    '<li><a href="#" onclick="onCannel(<%Id%>)"><i class="fa fa-user-times" aria-hidden="true"></i>&nbsp;注销</a></li>' +
                    '<li><a href="#" onclick="onUnlock(<%Id%>)"><i class="fa fa-unlock" aria-hidden="true"></i>&nbsp;&nbsp;解锁</a></li>' +
                    '<li><a href="#" onclick="onLock(<%Id%>)"><i class="fa fa-lock" aria-hidden="true"></i>&nbsp;&nbsp;&nbsp;锁定</a></li>' +
                    '<li><a href="#" onclick="onResetPwd(<%Id%>)"><i class="fa fa-reply" aria-hidden="true"></i></i>&nbsp;&nbsp;重置密码</a></li>' +
                    '</ul></div>'
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
            },
            "delete": {
                text: "注销",
                icon: "fa-power-off",
                style: "",
                className: "btn-delete",
                event: onDelete,
                index: 2
            },
            "export": {
                text: "导出",
                icon: "fa-download",
                style: "",
                className: "btn-export",
                event: onExport,
                index: 3
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
        title: "新增用户",
        url: bootPATH + "/Admin/Createuser",
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
        title: "编辑用户",
        url: bootPATH + "/Admin/EditUser?&navId=" + id,
        width: 850,
        height: 600,
        fullscreen: false,
        afterClose: function () {
            table.reload();
        }
    }).open();
}

function oncannel(id) {
    abp.message.confirm(
        '用户将会注销.', //确认提示
        '确定注销?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                App.post("../api/services/api/User/Cannel", JSON.stringify({ id: id }), function (result) {
                    abp.message.success("注销成功", "提示");
                    table.reload();
                });
            }
        }
    );
}

function onlock(id) {
    abp.message.confirm(
        '该用户将会被锁定.', //确认提示
        '确定锁定?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                App.post("../api/services/api/User/Lock", JSON.stringify({ id: id }), function (result) {
                    abp.message.success("解锁成功", "提示");
                    table.reload();
                });
            }
        }
    );
}

function onunlock(id) {
    abp.message.confirm(
        '该用户将会被解锁.', //确认提示
        '确定解锁?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                App.post("../api/services/api/User/Unlock", JSON.stringify({ id: id }), function (result) {
                    abp.message.success("解锁成功", "提示");
                    table.reload();
                });
            }
        }
    );
}

function onresetpwd(id) {
    abp.message.confirm(
        '用户密码将被重置为初始密码.', //确认提示
        '重置密码?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                App.post("../api/services/api/User/ResetPwd", JSON.stringify({ id: id }), function (result) {
                    abp.message.success("重置密码成功", "提示");
                    table.reload();
                });
            }
        }
    );
}

function onExport() {
    abp.ui.setBusy();
    $.fileDownload('/Admin/ExportUser', {
        httpMethod: "POST",
        data: JSON.stringify(table.data.params),
        successCallback: function (url) {
            abp.ui.clearBusy();
        },
        failCallback: function (responseHtml, url) {
            alert(responseHtml);
            abp.ui.clearBusy();
        }
    });
}

function onDelete() {
    var count = table.option.selectedCheck.length;
    abp.message.confirm(
        '将有'+count+'个用户被注销', //确认提示
        '确定注销?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                if (count > 0) {
                    App.post("../api/services/api/User/Delete",JSON.stringify({ id: table.option.selectedCheck.join(',') }), function (result) {
                        abp.message.success("注销成功", "提示");
                        table.reload();
                    });
                }
            }
        }
    );
}

//#endregion


