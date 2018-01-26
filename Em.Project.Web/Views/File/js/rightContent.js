//双击文件或目录的下钻操作
function openModel(id, nodeType, isFolder) {
    switch (nodeType) {
        case "computer":
            getChildByComputer(id);
            break;
        case "folder":
            getChildByFolder(id);
            break;
        case "file"://文件夹
            if (isFolder) {
                getChildByFile(id);
            }
            else alert("No subitem");
            break;
    }
}

//获取终端列表
function getComputerList() {
    $.ajax({
        url: "GetComputerListByCurUser",
        type: 'post',
        success: function (data) {
            //debugger;
            zNodes = data.result;
            //$.fn.zTree.init($("#treeDemo"), setting, zNodes);
            loadHtml(data.result, "first");
        },
        error: function (xhr) {
            //debugger;
            abp.ui.clearBusy();

        }
    });
}
//根据终端获取共享目录
function getChildByComputer(computerId) {
    $.ajax({
        url: bootPATH + "/api/services/api/Folder/GetFolderListByComputer?computerId=" + computerId,
        type: 'get',
        success: function (data) {
            //debugger;
            zNodes = data.result;
            //$.fn.zTree.init($("#treeDemo"), setting, zNodes);
            loadHtml(data.result, "computer");
        },
        error: function (xhr) {
            //debugger;
            abp.ui.clearBusy();

        }
    });
}

//根据共享目录获得子文件（夹）
function getChildByFolder(folderId) {
    //$.ajax({
    //    url: bootPATH + "/api/services/api/File/GetCurFileListByFolder?folderId=" + folderId,
    //    type: 'get',
    //    success: function (data) {
    //        //debugger;
    //        zNodes = data.result;
    //        //$.fn.zTree.init($("#treeDemo"), setting, zNodes);
    //        loadHtml(data.result, "folder");
    //    },
    //    error: function (xhr) {
    //        //debugger;
    //        abp.ui.clearBusy();
    //    }
    //});

    $.ajax({
        url: "GetFileListByFolder",
        data: { folderId: folderId },
        type: 'post',
        success: function (data) {
            //debugger;
            zNodes = data.result;
            //$.fn.zTree.init($("#treeDemo"), setting, zNodes);
            loadHtml(data.result, "folder");
        },
        error: function (xhr) {
            //debugger;
            abp.ui.clearBusy();

        }
    });
}

//根据文件id获取子项
function getChildByFile(fileId) {

    $.ajax({
        url: "GetFileListByFile",
        data: { fileId: fileId },
        type: 'post',
        success: function (data) {
            //debugger;
            zNodes = data.result;
            //$.fn.zTree.init($("#treeDemo"), setting, zNodes);
            loadHtml(data.result, "file");
        },
        error: function (xhr) {
            //debugger;
            abp.ui.clearBusy();

        }
    });
}

//根据集合和类型拼凑内容
function loadHtml(dataArr, nodeType) {
    var htm = "";
    if (dataArr != null && dataArr.length > 0) {
        for (var i = 0; i < dataArr.length; i++) {
            var sname = dataArr[i].name.length > 10 ? dataArr[i].name.substring(0,10) + "..." : dataArr[i].name;
            switch (nodeType) {
                case "first":
                    htm += '<div class="col-sm-2" style="cursor:pointer;float:left;width:100px;height:100px;margin-left:35px;margin-bottom:20px;word-wrap:break-word;overflow:hidden;text-align:center" ondblclick="openModel(' + dataArr[i].id.substring(dataArr[i].id.indexOf("_") + 1) + ',\'computer\',false)"  title="' + dataArr[i].name + '"><span ><i class="fa fa-folder-open-o fa-4x" style="font-size:2em; "></i><br /><span class="text-overflow">' + sname + '</span></span></div>';
                    break;
                case "computer":
                    htm += '<div class="col-sm-2" style="cursor:pointer;float:left;width:100px;height:100px;margin-left:35px;margin-bottom:20px;word-wrap:break-word;overflow:hidden;text-align:center" ondblclick="openModel(' + dataArr[i].id + ',\'folder\',false)"  title="' + dataArr[i].name + '"><span><i class="fa fa-folder-open-o fa-4x" style="font-size:2em;"></i><br /><span class="text-overflow">' + sname + '</span></span></div>';
                    break;
                case "folder":
                    //是否为文件夹的判断
                    if (dataArr[i].isFolder) {
                        htm += '<div class="col-sm-2 context-menu-one" style="cursor:pointer;float:left;width:100px;height:100px;margin-left:35px;margin-bottom:20px;word-wrap:break-word;overflow:hidden;text-align:center" ondblclick="openModel(' + dataArr[i].id.substring(dataArr[i].id.indexOf("_") + 1) + ',\'file\',true)"  title="' + dataArr[i].name + '"><span ><i class="fa fa-folder-open-o fa-4x" style="font-size:2em;"></i><br /><p class="text-overflow">' + sname + '</p></span></div>';
                    }
                    else {
                        htm += '<div class="col-sm-2 context-menu-one" style="cursor:pointer;float:left;width:100px;height:100px;margin-left:35px;margin-bottom:20px;word-wrap:break-word;overflow:hidden;text-align:center" ondblclick="openModel(' + dataArr[i].id.substring(dataArr[i].id.indexOf("_") + 1) + ',\'file\',false)"  title="' + dataArr[i].name + '"><span ><i class="fa ' + dataArr[i].fileFormatIcon + ' fa-4x" style="font-size:2em;"></i><br /><p class="text-overflow">' + sname + '</p></span></div>';
                    }
                    break;
                case "file"://文件夹
                    if (dataArr[i].isFolder) {
                        htm += '<div class="col-sm-2 context-menu-one" style="cursor:pointer;float:left;width:100px;height:100px;margin-left:35px;margin-bottom:20px;word-wrap:break-word;overflow:hidden;text-align:center" ondblclick="openModel(' + dataArr[i].id.substring(dataArr[i].id.indexOf("_") + 1) + ',\'file\',true)"  title="' + dataArr[i].name + '"><span><i class="fa fa-folder-open-o fa-4x" style="font-size:2em;"></i><br /><p class="text-overflow">' + sname + '</p></span></div>';
                    }
                    else {
                        htm += '<div class="col-sm-2 context-menu-one" style="cursor:pointer;float:left;width:100px;height:100px;margin-left:35px;margin-bottom:20px;word-wrap:break-word;overflow:hidden;text-align:center" ondblclick="openModel(' + dataArr[i].id.substring(dataArr[i].id.indexOf("_") + 1) + ',\'file\',false)"  title="' + dataArr[i].name + '"><span><i class="fa ' + dataArr[i].fileFormatIcon + ' fa-4x" style="font-size:2em;"></i><br /><p class="text-overflow">' + sname + '</p></span></div>';
                    }
                    break;
            }
        }
    }
    $("#fileModule").empty();//清理子元素
    $("#fileModule").html(htm);
}
