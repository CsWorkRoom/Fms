
//#region 页面入口

$(document).ready(function () {
  

    InitPages();
    InitPagesNo();

})

//#endregion

//#region 初始化页面

function InitPages() {
    var setting_NavIds = {
        callback: {
            onCheck: function (event, treeId, treeNode) {
                onGetCheckedValue_NavIds(event, treeId, treeNode);
            }
        },
        view: {
            showLine: true,
            selectedMulti: true
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

   
    var cId = $("#Id").val();
    $.ajax({
        type: 'POST',
        url: bootPATH + '/api/services/api/Content/GetDistrictParentTreeJson?cId=' + cId,
        dataType: 'json',
        contentType: 'application/json',
        beforeSend: function () {
            abp.ui.setBusy('#zTreeMilti_NavIds');
        },
        success: function (data) {
            abp.ui.clearBusy('#zTreeMilti_NavIds');
            if (data.result.contentEncoding)
                data = data.result.data;
            else
                data = data.result;
            var tree = $.fn.zTree.init($('#zTreeMilti_NavIds'), setting_NavIds, data);
            var tempValue = '';
            if (typeof tempValue === 'undefined' || tempValue === '') {
                onGetCheckedValue_NavIds();
            }
        },
        error: function (xhr) {
            abp.ui.clearBusy('#zTreeMilti_NavIds');
            console.log(xhr);
        }
    });

}

function InitPagesNo() {

   
    //初始化设置
    var setting_NavIds = {
        callback: {
            onCheck: function (event, treeId, treeNode) {
                onGetCheckedValue_NavIdsNo(event, treeId, treeNode);
            }
        },
        view: {
            showLine: true,
            selectedMulti: true
        },
        data: {
            simpleData: {
                enable: true
            }
        },
        check: {
            enable: true,
            chkStyle: "checkbox",
            chkboxType: { "Y" : "s", "N" : "" }
        }
    };


    var cId = $("#Id").val();
    $.ajax({
        type: 'POST',
        url: bootPATH + '/api/services/api/Content/GetDistrictParentTreeJsonNo?cId=' + cId,
        dataType: 'json',
        contentType: 'application/json',
        beforeSend: function () {
            abp.ui.setBusy('#zTreeMilti_NavIdsNo');
        },
        success: function (data) {
            abp.ui.clearBusy('#zTreeMilti_NavIdsNo');
            if (data.result.contentEncoding)
                data = data.result.data;
            else
                data = data.result;
            var tree = $.fn.zTree.init($('#zTreeMilti_NavIdsNo'), setting_NavIds, data);
            var tempValue = '';
            if (typeof tempValue === 'undefined' || tempValue === '') {
                onGetCheckedValue_NavIds();
            }
        },
        error: function (xhr) {
            abp.ui.clearBusy('#zTreeMilti_NavIdsNo');
            console.log(xhr);
        }
    });

}

//#endregion 

//#region 初始化事件


//#endregion

//#region 自定义项



function setFun() {
    var ids = getAllChecked();
    $("#FunIds").val(ids);
    $("#DistrictListId").val(ids);
   
}

function ChangeClick() {
    setFun();
}

function getAllChecked() {
    var ids = [];
    $("input[name='oneFunId']:checked").each(function () {
        ids.push($(this).val());
    });
    return ids.join(',');
}

function GetCheckAll() {
    var result = $("#checkAll").prop("checked");
    if (result) {
        $("input[name='oneFunId']").each(function () {
            $(this).prop("checked", result);
        });
    } else {
        $("input[name='oneFunId']").each(function () {
            $(this).prop("checked", result);
        });
    }
}

function GetUrl() {
    ///api/services/api/Modules/GetModuleEventByUrl
    
}

function onGetCheckedValue_NavIds(event, treeId, parentNode) {

    var treeObj = $.fn.zTree.getZTreeObj('zTreeMilti_NavIds');
    try {
        var checkNodes = treeObj.getCheckedNodes(true);
        var ids = [];
        var parentIds = [];
        var childIds = [];
        $(checkNodes).each(function () {
            ids.push(this.id);
        });
        $('#NavIds').val(ids.splice(','));

    } catch (e) {

    }
}

function onGetCheckedValue_NavIdsNo(event, treeId, parentNode) {
    var treeObj = $.fn.zTree.getZTreeObj('zTreeMilti_NavIdsNo');
    try {
        var checkNodes = treeObj.getCheckedNodes(true);
        var ids = [];
        var parentIds = [];
        var childIds = [];
        $(checkNodes).each(function () {
            ids.push(this.id);
        });
        $('#NavIdsNo').val(ids.splice(','));
    } catch (e) {
    }
}
//#endregion


