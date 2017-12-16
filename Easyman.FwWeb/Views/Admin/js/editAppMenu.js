
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
    $.post('../api/services/api/Role/GetRoleTreeJsonByNavId?navId=' + currentId, {}, function (data) {
        var tree = $.fn.zTree.init($('#roleTree'), setting, eval(data.result));
    });
}

//#endregion 

//#region 初始化事件

function InitEvent() {
    $(document).keydown(function (event) {
        if (event.ctrlKey) {
            $("#isCtrl").val("yes");
        }
    });

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
        $("#sumbit-btn").attr("type", "submit");
    });

    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}

//#endregion

//#region 自定义项

//点击显示模态窗口，模态窗使用分部视图显示类型小图标
//function MyModelTypeIcon() {
//    var divicon = $("#divTypeIcon");

//    if (divicon.is(':hidden')) {
//        divicon.show();

//    } else {
//        divicon.hide();

//    }
//    //$(document).ready(function () {
//    //    //$("#divTypeIcon").modal("show");
//    //  $("#divTypeIcon").show();
//    //})
//}

$(document).click(function (e) {
    var _con = $('#divTypeIcon');   // 设置目标区域
    if (!_con.is(e.target) && _con.has(e.target).length === 0) {
        $('#divTypeIcon').hide();
    }
})

//部分视图传参数
function ContentIconType(value) {
    // alert(value);
    $("#divTypeIcon").hide();
    $("#icon_type_img").attr("class", value);
    $("#Icon").val($("#icon_type_img").attr("class"));
    // parent.modelHide(value);
}
//#endregion


