
//#region 页面入口
var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
    GetScript();
});

//#endregion

//#region 初始化页面
function InitPage() {
    $("#text_ParentId").val("");

}

function LoadType() {
    var id = $("#DefineId").val();
    $('#ParentId').val(0);
    $('#text_ContentType').val("");
    GetDefineTree(id);
}

function GetDefineTree(id) {

    var settingContentType = {
        callback: {
            onClick: function (event, treeId, treeNode) {
                if (treeNode.chkDisabled)
                    return;
                $('#ParentId').val(treeNode.id);
                $('#text_ContentType').val(treeNode.name);
                $('#dropDownTree_ContentType').toggle();

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
        }
    };
    $.ajax({
        url: bootPATH+"Content/GetDefineTree",
        type: 'get',
        data: { id: id, conntentTypeId: $("#PIdType").val() },
        dataType: 'json',
        success: function (data) {
            if (data.result.contentEncoding)
                data = data.result.data;
            else
                data = data.result.data;

            var tree = $.fn.zTree.init($('#menuTree_ContentType'), settingContentType, eval(data));
            //var value = data[0] == null ? ' ' : data[0].checked;

            //var defualtNode = tree.getNodeByParam('id', value, null);
            var isEdit = $("#IsEdit").val();
            if (isEdit == "True") {//判断是否是编辑

                var tyId = $("#PIdType").val();
                for (var i = 0; i < data.length; i++) {
                    //var value = data[i] == null ? ' ' : data[i].checked;

                    var typeId = data[i].id;
                    var defualtNode = tree.getNodeByParam('id', typeId, null);
                    if (parseInt(tyId) === typeId) {
                        if (defualtNode) {
                            $('#text_ContentType').val(defualtNode.name);
                        }
                    }
                }
            } else {
                $('#text_ContentType').val("");
                $('#ParentId').val(0);
            }
        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}

function GetScript() {
    $("#htmlTreeDiv").html();
    var html = "<div class='input-group'><input type='text' id='text_ContentType' value='' class='form-control' readonly />" +
    "<input type='hidden' id='ParentId' name='ParentId' value='1' /><div id='dropDownTree_ContentType' class='dropdown-menu dropdown-tree col-xs-12'>" +
        "<ul id='menuTree_ContentType' class='ztree'></ul>" + "</div><span class='input-group-btn'>" +
        "<button id='btn_ContentType' class='btn btn-default' type='button'><i class='fa fa-chevron-down'></i></button>" +
        "</span></div>";
    $("#htmlTreeDiv").html(html);
    $('#btn_ContentType').on('click',
    function () {
        $('#dropDownTree_ContentType').toggle();
    });

    var id = $("#DefineId").val();
    GetDefineTree(id);
}


//#endregion 

//#region 初始化事件

function InitEvent() {

    $("#sumbit-btn").click(function () {
        $("#saveForm").submit();
    });

    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}


//#endregion

//#region 自定义项

//#endregion


