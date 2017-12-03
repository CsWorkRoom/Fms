
//#region 页面入口

$(document).ready(function () {
   // var strFramId = GetUrlParam("framId");
    //var intForm= parent.$(".modal-dialog").height()-111;
    //$("#saveForm .form-body").height(intForm);

    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
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

   
    var roleId = parseInt($('#Id').val());
    $.ajax({
        type: 'POST',
        url: bootPATH+'/api/services/api/Modules/GetNavTreeJsonByRoleIdForModule?roleId=' + roleId+'&'+Date.now(),
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

//#endregion 

//#region 初始化事件

function InitEvent() {
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}

//#endregion

//#region 自定义项

function handleSubmitForm() {
    setFun();
    $("#saveForm").submit();
}

function setFun() {
    var ids = getAllChecked();
    $("#FunIds").val(ids);
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
    var checkNodes = treeObj.getCheckedNodes(true);
    var ids = [];
    var parentIds = [];
    var childIds = [];
    $(checkNodes).each(function () {
        ids.push(this.id);
        if (this.isParent) {
            parentIds.push(this.id);
        } else {
            childIds.push(this.id.replace("event","")+"|"+this.pId);
        }
    });
    $('#NavIds').val(ids.splice(','));
    $('#ParentNavIds').val(parentIds.splice(','));
    $('#ChildNavIds').val(childIds.splice(','));
}


//#endregion


