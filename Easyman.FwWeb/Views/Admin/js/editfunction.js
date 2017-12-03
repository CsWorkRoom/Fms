
//#region 页面入口

$(document).ready(function () {
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    var setting = {
        callback: {
            beforeCheck: function (treeId, treeNode) {
                var isCtrl = $("#isCtrl").val() == "yes";
                if (isCtrl) {
                    var treeObj = $.fn.zTree.getZTreeObj("roleTree");
                    treeObj.checkNode(treeNode, !treeNode.checked, false, false);
                    return false;
                }
            }
        },
        view: {
            showLine: true,
            selectedMulti: false
        },
        data: {
            simpleData: {
                enable: true
            }
        },
        check: {
            enable: true
        }
    };
    var currentId = parseInt($('#Id').attr('value'));
    $.post('../api/services/api/Role/GetRoleTreeJsonByFunId?funId=' + currentId, {}, function (data) {
        var tree = $.fn.zTree.init($('#roleTree'), setting, eval(data.result));
    });
}

//#endregion 

//#region 初始化事件

function InitEvent() {
    $(document).keyup(function () {
        $("#isCtrl").val("no");
    });

    $("#sumbit-btn").click(function () {
        //拼装已选择角色
        var treeObj = $.fn.zTree.getZTreeObj("roleTree");
        var checkNodes = treeObj.getCheckedNodes(true);
        var names = [];

        $(checkNodes).each(function () {
            names.push(this.id);
        });
        $("#RoleIds").val(names.splice(","));

        $("#saveForm").submit();
    });
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}

//#endregion

//#region 自定义项


//#endregion


