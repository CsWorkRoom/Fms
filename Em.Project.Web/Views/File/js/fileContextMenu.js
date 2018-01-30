//右键内容菜单
$(function () {
    $.contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
            var curItem = $(this);
            var dbClickAttr = curItem.attr("ondblclick");//获取双击属性值openModel(1,'file',true)
            //获取文件编号
            var monitFileId = dbClickAttr.substring(10, dbClickAttr.indexOf(','));
            //做该右击事件对应的事
            switch (key) {
                case "log"://日志
                    $.bootstrapLoading.start({ loadingTips: "Loading, please wait..." });
                    var modalId = CreateRandomNum(1, 0, 1000);//取0到1000的随机数
                    //打开监控历史记录报表
                    ModeDialogUrl('modalId' + modalId, 'file log', 'Report/TbReport?code=filelog&CUR_MONIT_FILE_ID=' + monitFileId, 870, 450);
                    $.bootstrapLoading.end();
                    break;
                case "history"://历史记录
                    $.bootstrapLoading.start({ loadingTips: "Loading, please wait..." });
                    var modalId = CreateRandomNum(1, 0, 1000);//取0到1000的随机数
                    //打开监控历史记录报表
                    ModeDialogUrl('modalId' + modalId, 'version history', 'Report/TbReport?code=monitFileHis&CUR_MONIT_FILE_ID=' + monitFileId, 900, 450);
                    $.bootstrapLoading.end();
                    break;
                case "open"://打开
                    //获取文件路径
                    $.bootstrapLoading.start({ loadingTips: "Loading, please wait..." });
                    $.ajax({
                        url: "GetFilePathByMonitFile",
                        data: { monitFileId: monitFileId },
                        type: 'post',
                        success: function (data) {
                            if (data != null && data != "") {
                                //根据路径预览文件内容
                                var fileData = $.parseJSON(data);
                                var name = fileData[0].NAME;
                                var path = fileData[0].SERVER_PATH;
                                var names = new Array(".xls", ".xlsx", ".doc", ".docx", ".txt");

                                if (names.indexOf(name) < 0) {
                                    $.bootstrapLoading.end();
                                    alert("No Support");
                                }
                                else {                                  
                                    $.ajax({
                                        url: "GetHtmlUrl",
                                        data: { url: path, monitFileId: monitFileId },
                                        type: 'post',
                                        success: function (data) {

                                            if (data != null && data != "" && data.indexOf("error!")<0) {
                                                var surl = "../pdfjs/web/viewer.html?file=/fms/pdfjs/pdf/file" + monitFileId + ".pdf";
                                                window.open(surl, 'newwindow', 'height=700,width=600,top=111,left=111,toolbar=no,menubar=no,scrollbars=yes, resizable=no,location=no, status=no');
                                                $.bootstrapLoading.end();
                                            }
                                            else {
                                                $.bootstrapLoading.end();
                                                abp.ui.clearBusy();
                                                alert("The file is not completed in sync, and the preview is not supported.");
                                            }
                                        }
                                    });
                                }                              

                            }
                        },
                        error: function (xhr) {
                            $.bootstrapLoading.end();
                            abp.ui.clearBusy();
                            alert("Acquisition of attribute information failure！");
                        }
                    });

                    break;
                case "attr"://属性
                    $.bootstrapLoading.start({ loadingTips: "Loading, please wait..." });
                    $("#attrTabHeader").empty();//清空头部
                    $("#attrTabBody").empty();//清空包体

                    $.ajax({
                        url: "GetAttrListByMonitFile",
                        data: { monitFileId: monitFileId },
                        type: 'post',
                        success: function (data) {
                            debugger;
                            if (data != null && data != "") {
                                MakeTabs(data);//拼凑各属性类的属性tabs，并添加到模态框attrs

                                //#region 打开模态框
                                $('#attrs').modal('show');//打开tb的模态框
                                //给上级模态窗的关闭按钮，添加下级模态窗口关闭事件
                                parent.$(".close").click(function () {
                                    $('#attrs').modal('hide');
                                });
                                //show完毕前执行
                                $('#attrs').on('shown', function () {
                                    //加上下面这句！解决了~
                                    $(document).off('focusin.modal');
                                });
                                //#endregion
                            }
                            $.bootstrapLoading.end();
                        },
                        error: function (xhr) {
                            //debugger;
                            $.bootstrapLoading.end();
                            abp.ui.clearBusy();
                            alert("Acquisition of attribute information failure！");
                        }
                    });
                    break;
                case "restore"://还原
                    $.bootstrapLoading.start({ loadingTips: "Loading, please wait..." });
                    restoreFile(monitFileId);
                    $.bootstrapLoading.end();
                    break;
                case "down"://下载
                    $.bootstrapLoading.start({ loadingTips: "Loading, please wait..." });
                    DownFile(monitFileId);
                    $.bootstrapLoading.end();
                    break;
            }

            //var m = "clicked: " + key;
            //window.console && console.log(m) || alert(m);
        },
        items: {
            "open": { name: "Open", icon: "open" },//打开
            "restore": { name: "Restore", icon: "history" },//还原
            "down": { name: "Down", icon: "history" },//下载
            //"log": { name: "Log", icon: "history" },//文件日志
            "history": { name: "Version history", icon: "history" },//版本历史
            "attr": { name: "propertys", icon: "attr" }//属性集合
            //"edit": { name: "Edit", icon: "edit" },
            //"cut": { name: "Cut", icon: "cut" },
            //"copy": { name: "Copy", icon: "copy" },
            //"paste": { name: "Paste", icon: "paste" },
            //"delete": { name: "Delete", icon: "delete" },
            //"sep1": "---------",
            //"quit": { name: "Quit", icon: "quit" }
        }
    });

    //$('.context-menu-one').on('click', function (e) {
    //    console.log('clicked', this);
    //})
});


//#region 属性集合及处理

//拼凑各属性类的属性tabs
function MakeTabs(data) {
    var attrArr = $.parseJSON(data);
    var attrTypeNameArr = GetAttrTypeArr(attrArr);//根据属性集合获取属性大类

    if (attrTypeNameArr != null && attrTypeNameArr.length > 0) {
        var headTab = "";
        var bodyTab = "";
        for (var i = 0; i < attrTypeNameArr.length; i++) {
            //拼凑头部
            if (i == 0) {
                headTab += '<li role="presentation" class="active"><a href="#attrTab_' + i.toString() + '" aria-controls="home" role="tab" data-toggle="tab">' + attrTypeNameArr[i] + '</a></li>';
            }
            else {
                headTab += '<li role="presentation"><a href="#attrTab_' + i.toString() + '"  aria-controls="profile" role="tab" data-toggle="tab">' + attrTypeNameArr[i] + '</a></li>';
            }

            var tab = "";//初始化
            for (var j = 0; j < attrArr.length; j++) {
                //拼凑包体
                if (attrArr[j].ATTR_TYPE_NAME == attrTypeNameArr[i]) {
                    tab += '<div class="form-group">';
                    tab += '   <label for="Name1" class="col-xs-4 control-label" style="text-align:right">' + attrArr[j].ATTR_NAME + '</label>';
                    tab += '  <div class="col-xs-6">';
                    tab += '  <label for="Name1" class="control-label" style="text-align:right">' + attrArr[j].ATTR_VAL + '</label>';
                    tab += '  </div>';
                    tab += '  </div>';
                }
            }
            if (tab != "" && tab.length > 0) {
                if (i == 0) {
                    bodyTab += ' <div role="tabpanel" class="tab-pane active form-horizontal" id="attrTab_' + i.toString() + '">';
                    bodyTab += tab;
                    bodyTab += ' </div>';
                }
                else {
                    bodyTab += ' <div role="tabpanel" class="tab-pane form-horizontal" id="attrTab_' + i.toString() + '">';
                    bodyTab += tab;
                    bodyTab += ' </div>';
                }
            }
        }
        $("#attrTabHeader").append(headTab);//添加tab头部
        $("#attrTabBody").append(bodyTab);//添加tab包体
    }
}

//根据属性集合获取属性大类
function GetAttrTypeArr(attrArr) {
    var attrTypeNameArr = []//初始化属性类型集合
    if (attrArr != null && attrArr.length > 0) {
        for (var i = 0; i < attrArr.length; i++) {
            //未找到元素时，添加
            if ($.inArray(attrArr[i].ATTR_TYPE_NAME, attrTypeNameArr) == -1) {
                attrTypeNameArr.push(attrArr[i].ATTR_TYPE_NAME);
            }
        }
    }
    return attrTypeNameArr;
}
//#endregion

//还原文件
function restoreFile(monitFileId) {
    $.ajax({
        url: bootPATH + "/api/services/api/MonitFile/RestoreFileByMonitFile?monitFileId=" + monitFileId,
        type: 'get',
        success: function (data) {
            debugger;
            if (data.result == "") {
                alert("restore success！");
            }
            else {
                alert("restore failed：" + data.result);
            }
        },
        error: function (xhr) {
            //debugger;
            abp.ui.clearBusy();
            alert("restore failed！");
        }
    });
}
//下载文件和文件夹的压缩包
function DownFile(monitFileId)
{
    $.ajax({
        url: bootPATH + "/api/services/api/MonitFile/GenerateFile?monitFileId=" + monitFileId,
        type: 'get',
        success: function (data) {
            debugger;
            var fileName = data.result;
            if (fileName != "") {
                window.downloadFile(bootPATH + "tempFolder/" + fileName);//下载文件
                //删除临时文件夹tempFolder之下的文件
                $.ajax({
                    //url: bootPATH + "/api/services/api/MonitFile/DeleteFile?fileName=" + fileName,
                    url: bootPATH + "/api/services/api/MonitFile/DeleteTempFiles",
                    type: 'get',
                });
            }
            else {
                alert("Download failure！");
            }
        },
        error: function (xhr) {
            return "error！";
        }
    });
}

//下载网站指定路径的文件
window.downloadFile = function (sUrl) {

    //iOS devices do not support downloading. We have to inform user about this.
    if (/(iP)/g.test(navigator.userAgent)) {
        alert('Your device does not support files downloading. Please try again in desktop browser.');
        return false;
    }

    //If in Chrome or Safari - download via virtual link click
    if (window.downloadFile.isChrome || window.downloadFile.isSafari) {
        //Creating new link node.
        var link = document.createElement('a');
        link.href = sUrl;

        if (link.download !== undefined) {
            //Set HTML5 download attribute. This will prevent file from opening if supported.
            var fileName = sUrl.substring(sUrl.lastIndexOf('/') + 1, sUrl.length);
            link.download = fileName;
        }

        //Dispatching click event.
        if (document.createEvent) {
            var e = document.createEvent('MouseEvents');
            e.initEvent('click', true, true);
            link.dispatchEvent(e);
            return true;
        }
    }

    // Force file download (whether supported by server).
    if (sUrl.indexOf('?') === -1) {
        sUrl += '?download';
    }

    window.open(sUrl, '_self');
    return true;
}
window.downloadFile.isChrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
window.downloadFile.isSafari = navigator.userAgent.toLowerCase().indexOf('safari') > -1;