
//#region 页面入口

$(document).ready(function () {
    InitPage();
    InitEvent();
   
})

//#endregion

//#region 初始化页面

function InitPage() {
    //$.post("../api/services/api/Icon/GetAllIcons", {}, function (data) {
    //    var result = data.result;

    //    $(result).each(function () {
    //        var pannel = $("<div />").appendTo($("#iconMenuDiv"));
    //        $("<h5 />").text(this.key).appendTo(pannel);
    //        var iconDiv = $("<div />").addClass("padding-10").appendTo(pannel);

    //        $(this.icons).each(function () {
    //            iconDiv.append("<span><i class='" + this.value + "'></i></span>");
    //        });
    //    });

    //    $("#iconMenuDiv span").click(function () {
    //        $("#iconMenuDiv").parent().find(".btn-value").html($(this).html());
    //        $("#Icon").val($(this).find('i').attr("class"));
    //    });
    //});
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
        $("#saveForm").submit();
    });

    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}

//#endregion

//点击显示模态窗口，模态窗使用分部视图显示类型小图标
function MyModelTypeIcon() {
    var divicon = $("#divTypeIcon");
    
    if (divicon.is(':hidden')) {
        divicon.show();
       
    } else {
        divicon.hide();
       
    }
    //$(document).ready(function () {
    //    //$("#divTypeIcon").modal("show");
    //  $("#divTypeIcon").show();
    //})
}

$(document).click(function(e){
    var _con = $('#divTypeIcon');   // 设置目标区域
    if(!_con.is(e.target) && _con.has(e.target).length === 0){
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

//#region 自定义项
//根据图标类型ID获取图标
//function ToptypeIconsDiv() {

//    //弹出子窗体
//    ModeDialogUrl('modalId173', '图标展示', '~/Admin/EditIcons?&framId=iframe18', '900', '450');
  
//}
//function modelHide(value) {
//    //参数传过来关闭子页面
//    $(".close").click();
//    //显示选择图标样式
//    $("#icon_type_img").attr("class", value);

//    //$("#icon_type_img").addClass(value);
//    //$("#modalId173").modelHide(value);
//}





//#endregion



