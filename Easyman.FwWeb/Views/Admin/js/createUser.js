
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

   // console.log(window.location.href);

    var currentId = parseInt($('#Id').attr('value'));
    $.post('../api/services/api/Role/GetRoleTreeJsonByUserId?userId=' + currentId, {}, function (data) {
        var tree = $.fn.zTree.init($('#roleTree'), setting, eval(data.result));
    });

    $("#text_GroupId").typeahead({
        source: function (query, process) {
            $.ajax({
                url: "../api/services/api/District/GetDistrict?conDistrict=" + $("#text_GroupId").val(),
                type: 'POST',
                dataType: 'JSON',
                data: { query: query },
                success: function (result) {
                    $("#EmptyContent").hide();
                    $("#EmptyContent").html('');
                    var builder = '<ul class="typeahead dropdown-menu" style="height: 200px; overflow-y: auto; overflow-x: hidden;width: 92%;left: 4%;">';
                    $.each(result["result"].data, function (e, item) {
                        builder += '<li data-value="' + item.name + '" class=""><a href="javascript:;" onclick="selectItem(' + item.value + ')" id=' + item.value + '>' + item.name + '</a></li>';
                    });
                    builder += '</ul>';
                    $("#EmptyContent").html(builder);
                    if (result["result"].data.length > 0) {
                        $("#EmptyContent").show();
                        $(".typeahead").show();
                    }
                }
            });
        }
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
        //电话号码验证
        //var phone = $("#PhoneNo").val();
        //if (!(/^1[34578]\d{9}$/.test(phone))) {
        //    //alert("手机号码有误，请重填");
        //    alert("手机号码有误，请重填!");
        //    return ;
        //}
        //拼装已选择角色
        var treeObj = $.fn.zTree.getZTreeObj("roleTree");
        var checkNodes = treeObj.getCheckedNodes(true);
        var names = [];
        var roles = [];

        $(checkNodes).each(function () {
            names.push(this.code);
            roles.push(this.id);
        });
        $("#RoleNames").val(names.splice(","));
        $("#RoleIds").val(roles.splice(","));

        $("#saveForm").submit();
    });

    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}

//#endregion

//#region 自定义项

function selectItem(value) {
    $("#text_GroupId").val($("#" + value).html());
    $("#GroupId").val(value);
    $("#EmptyContent").hide();
    $("#EmptyContent").html('');
}

function subjectItemshow() {
    $("#EmptyContent").show();
}
function subjectItemhide() {
    $("#EmptyContent").hide();
}

//#endregion


