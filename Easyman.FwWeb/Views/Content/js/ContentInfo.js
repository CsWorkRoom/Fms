
        $(document).ready(function () {
            var innerhtml = $("#article_content").val();
            //$("#summernote").val(innerhtml);
            $("#content").html(innerhtml);
            GetCreateReply();
            Init();
            BindUploadFile();
            //显示初始上传文件,查看的时候
            //TopContentsFiles();

            parent.$(".close").click(function () {
                $("#myLimitModal").modal('hide');
                parent.$(".modal-backdrop").remove();
            });
        });

var IsFrist = false;

function GetCreateReply() {
    $("#ReplyByReplyDiv").hide();
    $("#createReply").focus(function () {
        $("#createReplySubmit").show();
    });
}
function Init() {
    //保存上传文件
    $("#Con_Reap_Files").click(function () {
        //原来是根据评论ID和上传文件ID，保存到数据库
        //修改为，保存上传文件ID到FileId中,在后面的保存评论的时候，根据评论一起保存到数据库
        var files_id = $("#FileId").val();
        if (files_id != "") {
            $("#FileIds").val(files_id);
        }
        $("#ReapleFile_id").val("");
        $("#FileId").val("");
        //file_upload_1-queue层删除
        //$("#file_upload_1-queue").remove();
        $("#file_upload_1-queue").empty();

    });
}

//发表评论-针对内容发表评论
function replySubmit() {
    IsFrist = true;
    var reply = $("#createReply").val();
    if (reply === "") {
        abp.message.error('请填写评论内容', '评论失败');
        return false;
    }
    //上传附件ID集合
    var file_ids = $("#FileIds").val();
    var contentId = $("#Id").val();
    $.ajax({
        url: bootPATH + "/Content/CareatReply",
        type: 'get',
        data: { contentId: contentId, replyInfo: reply, fileIds: file_ids },
        dataType: 'json',
        success: function (data) {
            var result = data.result.data;
            var html = GetHuiFu(result);
            $("#Content_Reply_ListId").prepend(html);
            //$("#replySubmitDiv").after(html);
            //$("#Content_Reply_ListId").before(html);
            
            //var temp = GetHtml(result);
            //$("#replySubmitDiv").before(temp);
            //var temHtml = GetHuiFu(result);
            //$("#GetHtmlHuiFu_" + result.id).append(temHtml);
            $("#createReply").val("");
            $("#createReplySubmit").hide();
            var contentReplyNum = $("#ReplyCount").html();
            var num = parseInt(contentReplyNum);
            num++;
            $("#ReplyCount").html(num);
            //清空上传附件ID集合
            $("#FileIds").val("");
        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}

//回复评论
function ReplyByReply(id) {
    var html = $("#ReplyByReplyDiv_" + id);
    if (html.css("display") === "none") {
        $(".replyDiv").css("display", "none");
        html.show();
    } else {
        html.hide();
    }

}
//评论附件上传,打开模态框
function ReplyBy_fileId(id) {
    //把评论ID传入模态框
    $("#ReapleFile_id").val(id);
}
//查看附件,打开模态框
function myFilesModal_fileId(id) {
    //根据评论ID查询对应的上传附件，并显示在模态框中
    if (id != null) {
        var urlContentssFile = bootPATH + "/api/services/api/ReplyPraise/ReplyFilesList?repid=" + id;
        $.post(urlContentssFile, {}, function (datafiles) {
            console.log(datafiles);
            var fileList = datafiles.result;
            if (fileList.length > 0) {
                //把返回的上传文件显示出来
                var strHtml = "<ul>";
                for (var i = 0; i < fileList.length; i++) {
                    strHtml += "<li>";
                    strHtml += "     <i class='fa fa-paperclip' aria-hidden='true'></i>";
                    strHtml += "     <a target='_blank' href='" + fileList[i].upurl + "'>" + fileList[i].name + "</a>";
                    strHtml += "              <span>" + fileList[i].lengthKb + "</span>";
                    strHtml += "              <span>" + fileList[i].uptime + "</span>";
                    strHtml += " </li>";
                }
                strHtml += " </ul>";
                //$("#myFilesModalBody").append(strHtml);
                $("#myFilesModalBody").html(strHtml);
               

            }
        });
    }
}


//查看页面，根据评论ID控制该评论上传文件并显示
function TopContentsFiles() {

    var fileids = $("#RealyLIstId").val();
    if (fileids != "" && fileids != null) {
       
       
            //根据该评论ID查询出评论对应的上传文件
        var urlContentssFile = bootPATH + "/api/services/api/ReplyPraise/ContentReplyFileList?repid=" + fileids;
            $.post(urlContentssFile, {}, function (datafiles) {
                console.log(datafiles);
                var fileList = datafiles.result;
                if (fileList.length > 0) {
                    //把返回的上传文件显示出来
                    //<i class="fa fa-file-code-o" aria-hidden="true"></i>
                    for (var i = 0; i < fileList.length; i++) {

                        var fileidload = "ReapFileDiv_" + (fileList[i].id);
                        var titles = fileList[i].name + "-" + fileList[i].lengthKb + "-" + fileList[i].uptime;
                        var strHtml = "<div style='float: left;'>";
                        strHtml += "<span id='" + fileidload + "' title='" + titles + "'>";
                        strHtml += "          <span class='up_filename' id='filename" + fileidload + "'><a target='_blank' href='" + fileList[i].upurl + "'><i  class='fa fa-paperclip'  aria-hidden='true'></i> </a></span>";
        
                        strHtml += "</span>";
                        strHtml += "</div>";
                        $("#Reap_File_Div_" + fileList[i].realyId).append(strHtml);
                    }
                    }
                
            });   
    }   
}

//回复评论-回复一级评论id是评论的ID
function ReplyByReplySubmit(id) {
    IsFrist = false;
    var info = $("#ReplyByReplyInput_" + id).val();
    if (info === "") {
        abp.message.error('请填写评论内容', '评论失败');
        return false;
    }
    //上传附件ID集合
    var file_ids = $("#FileIds").val();
    var contentId = $("#Id").val();
    $.ajax({
        url: bootPATH + "/Content/CareatReply",
        type: 'get',
        data: { contentId: contentId, replyInfo: info, fileIds: file_ids, replyId: id },
        dataType: 'json',
        success: function (data) {
            var result = data.result.data;
            var html = GetHuiFu(result);
            //$("#ReplyByReplyDiv_" + id).before(html);
            //after
            $("#ReplyByReplyDiv_" + id).after(html);
            $("#ReplyByReplyInput_" + id).val("");
            $(".replyDiv").css("display", "none");
            var contentReplyNum = $("#ReplyCount").html();
            var num = parseInt(contentReplyNum);
            num++;
            $("#ReplyCount").html(num);
            //清空上传附件ID集合
            $("#FileIds").val("");
        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}
//回复二级评论
function ReplyByReplySubmitChild(id, fId) {
    IsFrist = false;
    var info = $("#ReplyByReplyInput_" + id).val();
    if (info === "") {
        abp.message.error('请填写评论内容', '评论失败');
        return false;
    }
    //上传附件ID集合
    var file_ids = $("#FileIds").val();
    var contentId = $("#Id").val();
    $.ajax({
        url: bootPATH + "/Content/CareatReply",
        type: 'get',
        data: { contentId: contentId, replyInfo: info, fileIds: file_ids, replyId: id },
        dataType: 'json',
        success: function (data) {
            var result = data.result.data;
            var html = GetHuiFu(result);
            //var html = GetHtml(result);
            $("#ReplyByReplyDiv_" + id).parent().after(html);
            $("#ReplyByReplyInput_" + id).val("");
            $(".replyDiv").css("display", "none");
            var contentReplyNum = $("#ReplyCount").html();
            var num = parseInt(contentReplyNum);
            num++;
            $("#ReplyCount").html(num);
            //清空上传附件ID集合
            $("#FileIds").val("");
        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}

//新增评论，的回复
function ReplyByReplyHtmlSubmit(id) {
    var info = $("#ReplyByReplyInput_" + id).val();
    if (info === "") {
        abp.message.error('请填写评论内容', '评论失败');
        return false;
    }
    //上传附件ID集合
    var file_ids = $("#FileIds").val();
    var contentId = $("#Id").val();
    $.ajax({
        url: bootPATH + "/Content/CareatReply",
        type: 'get',
        data: { contentId: contentId, replyInfo: info, fileIds: file_ids, replyId: id },
        dataType: 'json',
        success: function (data) {
            var result = data.result.data;
            var html = GetHuiFu(result);
            //debugger;
            if (IsFrist)
                $("#ReplyByReplyDiv_" + id).after(html);
            else
                $("#ReplyByReplyDiv_" + id).parent().after(html);
            $("#ReplyByReplyInput_" + id).val("");
            $(".replyDiv").css("display", "none");
            IsFrist = false;
            var contentReplyNum = $("#ReplyCount").html();
            var num = parseInt(contentReplyNum);
            num++;
            $("#ReplyCount").html(num);
            //清空上传附件ID集合
            $("#FileIds").val("");
        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}

function GetHuiFu(data) {
    var html = "";
    html = ' <div class="social-comment" id="GetHtmlHuiFu_' + data.id + '"><a href="" class="pull-left">' +
        '<img alt="' + data.replyUserName + '" title="' + data.replyUserName + '" src="' + bootPATH + '/Views/Content/img/Reply.png" class="userImg"></a><div class="media-body"  id="Reap_File_Div_' + data.id + '"><a href="javascript:">' + data.replyUserName + '</a> ';
    if (!IsFrist) {
        html += '<span>回复 ' + data.parentName + '&nbsp;</span> &nbsp;';
    }
    html += data.info + '<br/>';
    if (data.isLike) {
        html += '<a href="javascript: ReplyPraise(' + data.id + ')"  style="float: left;" class="small">' +
            '<i class="fa fa-thumbs-o-up" id="faReply_' + data.id + '"></i> <span id="ReplyPraiseCount_' + data.id + '">0</span></a>';
    }
    if (data.isReolyFloor) {
        html += '<a href="javascript:ReplyByReply(' + data.id + ')"  style="float: left;" class="small"><i class="fa fa-comments-up"></i> <span id="ReplyByReply_' + data.id + '">回复 </span></a>';
    }
    if (data.isReolyFloorFile) {
        if (data.isFileNumber > 0) {

            html += '<a href="javascript:"  style="float: left;font-size: 10px;margin-left: 5px;" class="small" id="myFilesModalList" data-toggle="modal" data-target="#myFilesModal" onclick="myFilesModal_fileId(' + data.id + ')"> 查看附件</a>';

        }
    }
    html += ' <small class="text-muted">' + data.creationTime.replace("T", " ").substring(0,19) + '</small></div>';
    if (data.isReolyFloor) {
        if (data.isReolyFile)
        {
            html += '<div class="social-comment replyDiv" id="ReplyByReplyDiv_' + data.id + '" style="display:none">' +
          '<div class="media-body">' +
          '<textarea class="form-control" id="ReplyByReplyInput_' + data.id + '" placeholder="填写评论..."></textarea>' +
          '</div>' +
          '<div class="media-body">' +
          '<br /><button  class="btn btn-sm btn-danger"  type="submit" id="ReplyByReplySubmit_' + data.id + '" onclick="ReplyByReplyHtmlSubmit(' + data.id + ')"><i class="fa fa-pencil" aria-hidden="true"></i>发表</button>' +
          ' <button class="btn btn-sm btn-info ui-ml20" id="addUserLimitList" data-toggle="modal" data-target="#myLimitModal" onclick="ReplyBy_fileId(0)" style="margin-left:10px"><i class="fa fa-upload" aria-hidden="true"></i> 上传附件</button>' +
          '</div>' +
          '</div>';
        } else {
            html += '<div class="social-comment replyDiv" id="ReplyByReplyDiv_' + data.id + '" style="display:none">' +
           '<div class="media-body">' +
           '<textarea class="form-control" id="ReplyByReplyInput_' + data.id + '" placeholder="填写评论..."></textarea>' +
           '</div>' +
           '<div class="media-body">' +
           '<br /><button  class="btn btn-sm btn-danger"  type="submit" id="ReplyByReplySubmit_' + data.id + '" onclick="ReplyByReplyHtmlSubmit(' + data.id + ')"><i class="fa fa-pencil" aria-hidden="true"></i>发表</button>' +
           '</div>' +
           '</div>';
        }
       
    }
    return html;
}

function ContentPraise() {
    var contentId = $("#Id").val();
    $.ajax({
        url: bootPATH + "/Content/ContentPraise",
        type: 'get',
        data: { contentId: contentId },
        dataType: 'json',
        success: function (data) {
            var result = data.result.data;
            if (result === "ok") {
                GetContentPraise(contentId);
            }

        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}

function GetContentPraise(contentId) {
    $.ajax({
        url: bootPATH + "/Content/GetContentPraise",
        type: 'get',
        data: { contentId: contentId },
        dataType: 'json',
        success: function (data) {
            var result = data.result.data;
            if (result === "ok") {
                $("#ContentPraiseCount").html(data.result.praiseNum);
                if (data.result.isOrLike) {
                    $("#contentFa").removeAttr("class");
                    $("#contentFa").attr("class", "fa fa-thumbs-up");
                } else {
                    $("#contentFa").removeAttr("class");
                    $("#contentFa").attr("class", "fa fa-thumbs-o-up");
                }

            }
        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}

function GetReplyPraise(replyId) {
    $.ajax({
        url: bootPATH + "/Content/GetReplyPraise",
        type: 'get',
        data: { replyId: replyId },
        dataType: 'json',
        success: function (data) {
            var result = data.result.data;
            if (result === "ok") {
                $("#ReplyPraiseCount_" + replyId).html(data.result.praiseNum);
                if (data.result.isOrLike) {
                    $("#faReply_" + replyId).removeAttr("class");
                    $("#faReply_" + replyId).attr("class", "fa fa-thumbs-up");
                } else {
                    $("#faReply_" + replyId).removeAttr("class");
                    $("#faReply_" + replyId).attr("class", "fa fa-thumbs-o-up");
                }
            }
        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}

function ReplyPraise(id) {
    $.ajax({
        url: bootPATH + "/Content/ReplyPraise",
        type: 'get',
        data: { replyId: id },
        dataType: 'json',
        success: function (data) {
            var result = data.result.data;
            if (result === "ok") {
                //$("#ReplyPraiseCount_" + id).html(data.result.praiseNum);
                GetReplyPraise(id);
            }
        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}


function UpdateFa() {

}

function GetContentInfo() {
    window.history.back();
}

///绑定上传控件
function BindUploadFile() {
    $('#upload').Huploadify({
        auto: true,
        fileTypeExts: '*.xls;*.xlsx;*.csv;*.docx;*.doc;*.jpg;*.gif;*.txt',
        multi: true,
        fileSizeLimit: 9999,
        showUploadedPercent: true,//是否实时显示上传的百分比，如20%
        showUploadedSize: true,
        removeTimeout: 9999999,
        uploader: bootPATH + '/api/services/api/ImportLog/uploadFile',
        onUploadStart: function () {
            //alert('开始上传');
        },
        onInit: function () {
            //alert('初始化');
        },
        onUploadSuccess: function (file, resultId) {
            var fileListId = $("#FileId").val();
            if (fileListId == "") {
                $("#FileId").val(resultId);
            } else {
                fileListId = fileListId + "," + resultId;
                $("#FileId").val(fileListId);
            }
            //fileupload_1_1
            // 根据返回的文件ID查询文件信息
            var urlContentsFile = bootPATH + "/api/services/api/Content/GetContentsFile?id=" + resultId;
            $.post(urlContentsFile, {}, function (datacf) {
                console.log(datacf);
                var datacfs = datacf.result;
                //JSON.stringify({ id: o.id, NODE_NAME: o.NODE_NAME,  SCRIPT_CASE_ID: o.SCRIPT_CASE_ID, LOG_NODE_ID:o.LOG_NODE_ID})
                $("#fileupload_1_" + file.id).attr("option-data", JSON.stringify({ id: datacfs.id, name: datacfs.name, url: datacfs.upurl, index: file.index }));
                var html = "<a href='" + datacfs.upurl + "'> " + file.name + "<a/>";
                // $("#filenamefileupload_1_" + file.id).replace(html);
                $("#filenamefileupload_1_" + file.id).html(html);
            });



        },
        onDelete: function (file) {

            alert("删除");

            console.log('删除的文件：' + file);
            console.log(file);
        }
    });
}

//查看页面，根据内容ID查询评论并显示
function TopContentsFiles() {

    var cid = $("#Id").val();

        //根据该内容ID查询出评论对应的上传文件
    var urlContentssFile = bootPATH + "/api/services/api/ReplyPraise/GetContentReplyListId?contentId=" + cid;
        $.post(urlContentssFile, {}, function (datareply) {
            console.log(datareply);
            var dataList = datareply.result;
            if (dataList.length > 0) {
                //把返回的评论显示出来
               
                for (var i = 0; i < fileList.length; i++) {

                    var fileidload = "ReapFileDiv_" + (fileList[i].id);
                    var titles = fileList[i].name + "-" + fileList[i].lengthKb + "-" + fileList[i].uptime;
                    var strHtml = "<div style='float: left;'>";
                    strHtml += "<span id='" + fileidload + "' title='" + titles + "'>";
                    strHtml += "          <span class='up_filename' id='filename" + fileidload + "'><a target='_blank' href='" + fileList[i].upurl + "'><i  class='fa fa-paperclip'  aria-hidden='true'></i> </a></span>";

                    strHtml += "</span>";
                    strHtml += "</div>";
                    $("#Reap_File_Div_" + fileList[i].realyId).append(strHtml);
                }
            }

        });
    
}

//二级评论，更多显示
function Reply2ReplyDivNumber(id) {
    //显示一级评论下被隐藏的二级评论
  
    
    var replyDiv = $("#GetHtmlHuiFu_" + id).find("div");
    for (var i = 0; i < replyDiv.length; i++) {
         
        if (replyDiv[i].id.indexOf("Reply_2Reply_Div_")>=0)
        {
            if (replyDiv[i].style.display == "none") {
                replyDiv[i].style.display = "block";
            }
        } 
    }
    //取消更多
    $("#Reply_2Reply_DivNumber_" + id).html("");
}
//一级评论，更多显示
function Reply1ReplyDivNumber() {
    //显示一级评论下被隐藏的二级评论


    var replyDiv = $("#Content_Reply_ListId").find("div");
    for (var i = 0; i < replyDiv.length; i++) {

        if (replyDiv[i].id.indexOf("GetHtmlHuiFu_") >= 0) {
            if (replyDiv[i].style.display == "none") {
                replyDiv[i].style.display = "block";
            }
        }
    }
    //取消更多
    $("#Reply_1Reply_DivNumber_GetHuiFu").html("");
}
//把二级评论隐藏的层的ID加入对应的list
function TopReply2ReplyDivNumber() {

}
