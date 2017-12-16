
//#region 页面入口
var userListHtml = "";
var limitUserHtml = "";
var isUpdateRole = false; //判断是否刷新用户列表

var numUser = 0; //已经选择的用户数
var limitNumUser = 0;
var isLimitAdd = true;//判断是否是重复添加限制用户,false 为不可添加
var limitUserHtmlYes = "";
var limitNumUserYes = 0;
var isLimitAddYes = true;//判断是否是重复添加指定用户,false 为不可添加
var table;
$(document).ready(function () {
    InitPage();
    InitEvent();
    GetScript();
    GetCheckboxCount();
    Init();
    BindUploadFile();

    //显示初始上传文件,编辑的时候
     TopContentsFiles();
     
     BindControll();//加载绑定事件
});

//#endregion



function Init() {
    $('input').iCheck({
        checkboxClass: 'icheckbox_flat-blue',
        radioClass: 'iradio_flat-blue'
    });

    //指定用户，同步到textarea
    $("#ConfirmUserListYes").click(function () {
        $("#UserNameList").val("");
        $("#UserNameListId").val("");
        var userNameListYes = "";
        var userNameLimitListYes = "";
        var i = 0;
        $('#limitUserCheckYes p[class="item"]').each(
           function (e, b) {
               i++;
               if (i === 1) {
                   userNameLimitListYes += b.lang;
                   userNameListYes += b.title;
               }
               else {
                   userNameLimitListYes += "," + b.lang;
                   userNameListYes += "," + b.title;
               }
           });
        $("#ConfirmUserListYes").attr("data-dismiss", "modal");
        $("#UserNameList").val(userNameListYes);
        $("#UserNameListId").val(userNameLimitListYes);
    });
    //限制用户，同步到textarea
    $("#ConfirmUserListNo").click(function () {
        $("#UserNameListNo").val("");
        $("#UserNameLimitList").val("");
        var userNameListNo = "";
        var userNameLimitList = "";
        var i = 0;
        $('#limitUserCheck p[class="item"]').each(
           function (e, b) {
               i++;
               if (i === 1) {
                   userNameLimitList += b.lang;
                   userNameListNo += b.title;
               }
               else {
                   userNameLimitList += "," + b.lang;
                   userNameListNo += "," + b.title;
               }
           });
        $("#ConfirmUserListNo").attr("data-dismiss", "modal");
        $("#UserNameListNo").val(userNameListNo);
        $("#UserNameLimitList").val(userNameLimitList);
    });


    $('#summernote').summernote({
        lang: 'zh-CN',
        height: 500,
        minHeight: null,
        maxHeight: null,
        focus: true
    });
    //限制名单，添加方法
    $("#BtnLimitUser").click(function () {
        GetLimiUser();
    });
    //限制用户，根据用户名，模糊查询用户
    GetUserAutocomplete();

    //指定名单，添加方法
    $("#BtnLimitUserYes").click(function () {
        GetLimiUserYes();
    });
    //指定用户，根据用户名，模糊查询用户
    GetUserAutocompleteYes();


    //编辑，数据初始化
    InsertHtml();
    var value = $("#Info").val();
    $('.summernote').summernote('code', value);
}

//#region 初始化页面
function InitPage() {
    $("#text_ContentTypeId").val("");

    $(".navBut li").click(function () {
        $(".navBut li").removeClass("active");
        $(this).addClass("active");
    });


}
function GetCheckboxCount() {
    if ($("#IsUse").is(':checked'))
        $("#IsUse").val("true");
    else
        $("#IsUse").val("false");
    if ($("#IsUrgent").is(':checked'))
        $("#IsUrgent").val("true");
    else
        $("#IsUrgent").val("false");

    if ($("#IsAllUser").is(':checked'))
        $("#IsAllUser").val("true");
    else
        $("#IsAllUser").val("false");

    if ($("#IsAllRole").is(':checked'))
        $("#IsAllRole").val("true");
    else
        $("#IsAllRole").val("false");

    if ($("#IsAllDistrict").is(':checked'))
        $("#IsAllDistrict").val("true");
    else
        $("#IsAllDistrict").val("false");
}
//功能定义
function LoadType() {

    var id = $("#DefineId").val();
    $('#ContentTypeId').val(0);
    $('#text_ContentType').val("");
    GetDefineTree(id);
    //根据功能定义获取权限
    GetContentCheck(id);


}
//根据功能定义ID获取用户，角色，组织权限,上传文件
function GetContentCheck(id) {
    $.ajax({
        url: bootPATH+"Content/GetDefineConfigCheck",
        type: 'get',
        data: { id: id },
        dataType: 'json',
        success: function (data) {
           
                data = data.result.data;

                if (data != null) {
                    //用户权限
                    if (data.isChenkUser) {
                        $("#DivIsCheckUser").show();
                        $("#liIsCheckUser").show();
                    } else {
                        $("#DivIsCheckUser").hide();
                        $("#liIsCheckUser").hide()
                    }
                    //角色权限
                    if (data.isChenkRole) {
                        $("#DivIsCheckRole").show();
                        $("#liIsCheckRole").show();
                    } else {
                        $("#DivIsCheckRole").hide();
                        $("#liIsCheckRole").hide()
                    }
                    //组织权限
                    if (data.isChenkDistrict) {
                        $("#DivIsCheckDistrict").show();
                        $("#liIsCheckDistrict").show();
                    }
                    else {
                        $("#DivIsCheckDistrict").hide();
                        $("#liIsCheckDistrict").hide()
                    }
                    //全部
                    if (data.isChenkUser || data.isChenkRole || data.isChenkDistrict)
                        $("#liAllRole").show();
                    else
                        $("#liAllRole").hide();

                    //上传附件权限
                    if (data.isContentFile) {
                        $("#content_file").show();
                    }
                    else {
                        $("#content_file").hide();
                    }
                }
        },
        error: function (err) {
            alert("根据功能定义,获取权限失败",err);
        }
    });
   
}

//内容类别
function GetDefineTree(id) {

    var settingContentType = {
        callback: {
            onClick: function (event, treeId, treeNode) {
                if (treeNode.chkDisabled)
                    return;
                $('#ContentTypeId').val(treeNode.id);
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
        data: { id: id, conntentTypeId: $("#ContentTypeId").val() },
        dataType: 'json',
        success: function (data) {
            if (data.result.contentEncoding)
                data = data.result.data;
            else
                data = data.result.data;

            var tree = $.fn.zTree.init($('#menuTree_ContentType'), settingContentType, eval(data));
          
            var isEdit = $("#IsEdit").val();
            if (isEdit == "True") {//判断是否是编辑
                var tyId = $("#ContentTypeId").val();
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
                $('#ContentTypeId').val(0);
            }
        },
        error: function (err) {
            //alert("BBBB",err);
        }
    });
}

function GetScript() {
    $("#htmlTreeDiv").html();
    var html = "<div class='input-group'><input type='text' id='text_ContentType' value='0' class='form-control' readonly />" +
    "<input type='hidden' id='ContentTypeId' name='ContentTypeId' value='0' /><div id='dropDownTree_ContentType' class='dropdown-menu dropdown-tree col-xs-12'>" +
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
    //根据功能定义获取权限
    GetContentCheck(id);
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
        uploader: bootPATH + 'api/services/api/ImportLog/uploadFile',
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
                    $("#fileupload_1_" + file.id).attr("option-data", JSON.stringify({ id: datacfs.id, name: datacfs.name, url: datacfs.upurl ,index:file.index}));
                    var html = "<a  target='_blank'  href='" + datacfs.upurl + "'> " + file.name + "<a/>";
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
//编辑页面，根据编辑ID控制上传文件并显示
function TopContentsFiles() {
   
     //var isEdit = $("#IsEdit").val();
     //if (isEdit == true) {
         var fileids = $("#FileId").val();
         if (fileids != "" && fileids != null) {
             var urlContentssFile = bootPATH + "/api/services/api/Content/GetContentsFileIds?id=" + fileids;
             $.post(urlContentssFile, {}, function (datafiles) {
                 console.log(datafiles);
                 var fileList = datafiles.result;

                 if (fileList.length > 0) {
                     //var html = "";
                     for (var i = 0; i < fileList.length; i++) {
                         var fileidload = "fileupload_1_" + (i + 1);
                         var size = "";
                         if (fileList[i].length > 1024 * 1024) {
                             size = (Math.round(fileList[i].length * 100 / (1024 * 1024)) / 100).toString() + 'MB';
                         }
                         else {
                             size = (Math.round(fileList[i].length * 100 / 1024) / 100).toString() + 'KB';
                         }
                         var strHtml = "<div id='" + fileidload + "' class='uploadify-queue-item'>";
                         strHtml += "          <div class='uploadify-progress'><div class='uploadify-progress-bar' style='width: 100%;'></div></div>";
                         strHtml += "          <span class='up_percent'>100%</span><span class='progressnum'>";
                         strHtml += "          <span class='uploadedsize'>" + size + "</span>/<span class='totalsize'>" + size + "</span></span>";
                         strHtml += "          <span class='up_filename' id='filename" + fileidload + "'><a target='_blank' href='" + fileList[i].upurl + "'> " + fileList[i].name + "</a>";
                         strHtml += "           <a></a></span><span class='delfilebtn' onclick='contentFileDel(" + i + ")'>删除</span>";
                         strHtml += "   </div>";
                         $("#file_upload_1-queue").append(strHtml);
                         $("#fileupload_1_" + (i + 1)).attr("option-data", JSON.stringify({ id: fileList[i].id, name: fileList[i].name, url: fileList[i].upurl, index: (i + 1) }));

                         //onclick="MyModelTypeIcon()"


                     }
                 }
             });
         }
    // }


}
//删除上传文件方法
function contentFileDel(index) {
    $("#fileupload_1_" + (index + 1)).hide();
    var removeItem = index;
    var fileListId = $("#FileId").val();
    var fileIds = fileListId.split(',');

    //修改对应ID
    fileIds[removeItem] = "删除";
    var fileListIds = "";
    if (fileIds.length == 0) { $("#FileId").val(); }
    else if (fileIds.length == 1) { $("#FileId").val(fileIds); }
    else {
        for (var i = 0; i < fileIds.length; i++) {
            if (i == 0) {
                fileListIds = fileIds[i];
            } else {
                fileListIds = fileListIds + "," + fileIds[i];
            }
        }
        $("#FileId").val(fileListIds);
    }


}


//#endregion 

//#region 初始化事件

function InitEvent() {

    $("#sumbit-btn").click(function () {
        GetCheckboxCount();
        //$("#saveForm").submit();
        $("#sumbit-btn").attr("type", "submit");
    });
    SubmitFormData("#saveForm", "#sumbit-btn");//提交数据
}


//#endregion

//#region 自定义项

//编辑，数据初始化
function InsertHtml() {
    
   
    //指定角色名单
    var roleList = $("#RoleListId").val();
    //限制角色名单
    var roleListNo = $("#RoleListIdNo").val();

    //指定组织名单
    var districtList = $("#DistrictListId").val();
    //限制组织名单
    var districtListNo = $("#DistrictListIdNo").val();
    //限制名单UserNameLimitList
    var userNameLimitList = $("#UserNameLimitList").val();
    if (userNameLimitList) {
        $("#limitUserCheck").html("");
        //$("#limitUserList").html("");
        limitUserHtml = "";
        var userList = userNameLimitList.split(",");
        for (var i = 0; i < userList.length; i++) {
            var uId = userList[i];
            $.ajax({
                type: "post",
                dataType: "json",
                url: bootPATH+'Content/GetUserBUserId',
                data: { uId: uId },
                async: false,
                success: function (data) {
                    //$(".modal-body").html(data);
                    var result = data.result.data;
                    isLimitAdd = true;
                    VerificationUser(result.userId);
                    if (isLimitAdd) {
                        limitUserHtml += GetLimiUserHtml(result.userId, result.userName);
                        $("#limitUserCheck").html(limitUserHtml);
                       // $("#limitUserList").html(limitUserHtml);
                        CreateLimitUserClick();//注册限制用户的关闭事件
                        limitNumUser = $("#limitUserCheck span[class='itemdel']").length;
                        $("#LimitUser").html(limitNumUser);
                    }
                }

            });
        }
    }
    //指定名单UserNameListId
    var userListId = $("#UserNameListId").val();
    if (userListId) {
        $("#limitUserCheckYes").html("");
        //$("#limitUserList").html("");
        limitUserHtmlYes = "";
        var userList = userListId.split(",");
        for (var i = 0; i < userList.length; i++) {
            var uId = userList[i];
            $.ajax({
                type: "post",
                dataType: "json",
                url: bootPATH + 'Content/GetUserBUserId',
                data: { uId: uId },
                async: false,
                success: function (data) {
                    
                    var result = data.result.data;
                    isLimitAddYes = true;
                    VerificationUserYes(result.userId);
                    if (isLimitAddYes) {
                        limitUserHtmlYes += GetLimiUserHtmlYes(result.userId, result.userName);
                        $("#limitUserCheckYes").html(limitUserHtmlYes);
                        CreateLimitUserClickYes();//注册指定用户的关闭事件
                        limitNumUserYes = $("#limitUserCheckYes span[class='itemdel']").length;
                        $("#LimitUserYes").html(limitNumUserYes);
                    }
                }

            });
        }
    }
 
}

$(function () {

    //$("#aaaaaa").autocomplete({
    //    source: availableTags
    //}); 感觉此段代码为测试代码，在页面也没有找到，如果后面发现有其它用除，可以取消注释

    $("#Summary").autocomplete({
       // source: availableTags
    });
});
//autocomplete插件
//缓存
var cache = {};

//#region 限制用户操作
//根据用户名称，模糊查询限制用户
function GetUserAutocomplete() {
    //$("#limitUserName").focus(function () {
    $("#limitUserName").autocomplete({
        max: 12,    //列表里的条目数
        minChars: 0,    //自动完成激活之前填入的最小字符
        width: 400,     //提示的宽度，溢出隐藏
        scrollHeight: 300,   //提示的高度，溢出显示滚动条
        matchContains: true,    //包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示
        autoFill: false,    //自动填充
        source: function (request, response) {
            var term = request.term;
            if (term in cache) {
                response($.map(cache[term], function (item) {
                    return {
                        label: item.userName,
                        value: item.userId
                    }
                }));
                return;
            }
            $.ajax({
                url: bootPATH+"Content/GetUserByNameList",
                dataType: "json",

                data: {
                    name: $("#limitUserName").val()
                },
                success: function (data) {
                    var result = data.result;
                    //var temp = JSON.stringify(result);
                    //return temp;
                    response($.map(result.data, function (item, r) {
                        return {
                            label: item.userName,
                            value: item.userId,
                            val: item.userId
                        }
                        //return $.parseJSON(item);
                    }));

                }
            });
        },
        //source: availableTags,
        minLength: 1,
        select: function (event, ui) {
            event.preventDefault();
            $("#limitUserName").val(ui.item.label);
            $("#limitUserId").val(ui.item.value);
            GetLimiUser();
        }

    });
    //});
}

//限制名单，添加方法，获取限制的用户，模糊查询
function GetLimiUser() {
    var uName = $("#limitUserName").val();
    $.ajax({
        type: "post",
        dataType: "json",
        url: bootPATH+'Content/GetUserByName',
        data: { uName: uName },
        async: false,
        success: function (data) {
            //$(".modal-body").html(data);
            var result = data.result.data;
            if (result.userId === 0) {
                abp.message.error('请输入正确的用户名', '添加失败');
            } else {
                isLimitAdd = true;
                VerificationUser(result.userId);
                if (isLimitAdd) {
                    if (limitNumUser === 300)
                        abp.message.error('可限制数已达到上限，请删除后添加', '添加失败');
                    limitUserHtml += GetLimiUserHtml(result.userId, result.userName);
                    $("#limitUserCheck").html(limitUserHtml);
                   // $("#limitUserList").html(limitUserHtml);
                    CreateLimitUserClick();//注册限制用户的关闭事件
                    limitNumUser = $("#limitUserCheck span[class='itemdel']").length;
                    $("#LimitUser").html(limitNumUser);
                }
            }
        }
    });
}

//限制名单，初始化，显示限制用户数据
function GetLimitUser() {
    var userListIdNo = $("#UserNameLimitList").val();
    if (userListIdNo) {
        userListHtmlNo = "";
        var userList = userListIdNo.split(",");
        for (var i = 0; i < userList.length; i++) {
            var uId = userList[i];
            $.ajax({
                type: "post",
                dataType: "json",
                url: bootPATH + 'Content/GetUserBUserId',
                data: { uId: uId },
                async: false,
                success: function (data) {
                    userListHtmlNo += GetLimiUserHtml(data.result.data.userId, data.result.data.userName, "");
                }
            });
        }
        $("#limitUserCheck").html(userListHtmlNo);
        limitNumUser = $("#limitUserCheck span[class='itemdel']").length;
        $("#LimitUser").html(limitNumUser);
        CreateLimitUserClick();//注册限制用户的关闭事件
    }
}

    //判断限制用户是否重复添加
    function VerificationUser(uId) {
        $("#limitUserCheck span[class='itemdel']").each(function (e, b) {
            var userId = $(this)[0].lang;
            if (parseInt(userId) === uId)
                isLimitAdd = false;//返回false证明已经添加 不需要重新添加
            //return true;
        });
    }

    function UserLimitList() {
        $("#UserNameLimitList").val();
        var userList = "";
        var i = 0;
        $("#limitUserCheck span[class='itemdel']").each(function (e, b) {
            i++;
            if (i === 1)
                userList += $(this)[0].lang;
            else
                userList += "," + $(this)[0].lang;
        });
        $("#UserNameLimitList").val(userList);
    }


    //显示限制用户数据html
    function GetLimiUserHtml(uId, uName) {
        var html = "";
        html = "<p class='item' lang='" + uId + "' title='" + uName + "'><span class='nickname'>" + uName + "</span>" +
            "<span class='itemdel' lang='" + uId + "'>x</span></p>";
        return html;
    }

    //删除限制用户后要重新赋值
    function limitUserHtmlAgain() {
        limitUserHtml = "";
        $("#limitUserCheck p[class='item']").each(function (e, b) {
            limitUserHtml += GetLimiUserHtml($(this)[0].lang, $(this)[0].title);
        });
        CreateLimitUserClick();//注册限制用户的关闭事件
        limitNumUser = $("#limitUserCheck span[class='itemdel']").length;
        $("#LimitUser").html(limitNumUser);
    }

    //注册限制用户的关闭事件
    function CreateLimitUserClick() {
        $("#limitUserCheck span[class='itemdel']").each(function (e, b) {
            $(this).click(function () {
                var userId = $(this)[0].lang;
                $(this).parents('.item').remove();
                limitUserHtmlAgain();
            });
        });


    }

//#endregion 限制用户操作


    //#region 指定用户操作
    //根据用户名称，模糊查询指定用户
    function GetUserAutocompleteYes() {
        //$("#limitUserName").focus(function () {
        $("#limitUserNameYes").autocomplete({
            max: 12,    //列表里的条目数
            minChars: 0,    //自动完成激活之前填入的最小字符
            width: 400,     //提示的宽度，溢出隐藏
            scrollHeight: 300,   //提示的高度，溢出显示滚动条
            matchContains: true,    //包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示
            autoFill: false,    //自动填充
            source: function (request, response) {
                var term = request.term;
                if (term in cache) {
                    response($.map(cache[term], function (item) {
                        return {
                            label: item.userName,
                            value: item.userId
                        }
                    }));
                    return;
                }
                $.ajax({
                    url: bootPATH + "Content/GetUserByNameList",
                    dataType: "json",

                    data: {
                        name: $("#limitUserNameYes").val()
                    },
                    success: function (data) {
                        var result = data.result;
                        
                        response($.map(result.data, function (item, r) {
                            return {
                                label: item.userName,
                                value: item.userId,
                                val: item.userId
                            }
                            //return $.parseJSON(item);
                        }));

                    }
                });
            },
            //source: availableTags,
            minLength: 1,
            select: function (event, ui) {
                event.preventDefault();
                $("#limitUserNameYes").val(ui.item.label);
                $("#limitUserIdYes").val(ui.item.value);
                GetLimiUserYes();
            }

        });
        //});
    }

//指定名单，添加方法，获取指定的用户，模糊查询
    function GetLimiUserYes() {
        var uName = $("#limitUserNameYes").val();
        $.ajax({
            type: "post",
            dataType: "json",
            url: bootPATH + 'Content/GetUserByName',
            data: { uName: uName },
            async: false,
            success: function (data) {
                //$(".modal-body").html(data);
                var result = data.result.data;
                if (result.userId === 0) {
                    abp.message.error('请输入正确的用户名', '添加失败');
                } else {
                    isLimitAddYes = true;
                    VerificationUserYes(result.userId);
                    if (isLimitAddYes) {
                        if (limitNumUserYes === 300)
                            abp.message.error('可指定数已达到上限，请删除后添加', '添加失败');
                        limitUserHtmlYes += GetLimiUserHtmlYes(result.userId, result.userName);
                        $("#limitUserCheckYes").html(limitUserHtmlYes);
                        CreateLimitUserClickYes();//注册指定用户的关闭事件
                        limitNumUserYes = $("#limitUserCheckYes span[class='itemdel']").length;
                        $("#LimitUserYes").html(limitNumUserYes);
                    }
                }
            }
        });
    }

//指定名单，初始化，显示指定用户数据
    function GetLimitUserYes() {
        var userListIdYes = $("#UserNameListId").val();
        if (userListIdYes) {
            userListHtmlYes = "";
            var userList = userListIdYes.split(",");
            for (var i = 0; i < userList.length; i++) {
                var uId = userList[i];
                $.ajax({
                    type: "post",
                    dataType: "json",
                    url: bootPATH + 'Content/GetUserBUserId',
                    data: { uId: uId },
                    async: false,
                    success: function (data) {
                        userListHtmlYes += GetLimiUserHtmlYes(data.result.data.userId, data.result.data.userName, "");
                    }
                });
            }
            $("#limitUserCheckYes").html(userListHtmlYes);
            limitNumUserYes = $("#limitUserCheckYes span[class='itemdel']").length;
            $("#LimitUserYes").html(limitNumUserYes);
            CreateLimitUserClickYes();//注册指定用户的关闭事件
        }
    }

//判断指定用户是否重复添加
    function VerificationUserYes(uId) {
        $("#limitUserCheckYes span[class='itemdel']").each(function (e, b) {
            var userId = $(this)[0].lang;
            if (parseInt(userId) === uId)
                isLimitAddYes = false;//返回false证明已经添加 不需要重新添加
            //return true;
        });
    }

    function UserLimitListYes() {
        $("#UserNameListId").val();
        var userList = "";
        var i = 0;
        $("#limitUserCheckYes span[class='itemdel']").each(function (e, b) {
            i++;
            if (i === 1)
                userList += $(this)[0].lang;
            else
                userList += "," + $(this)[0].lang;
        });
        $("#UserNameListId").val(userList);
    }


//显示指定用户数据html
    function GetLimiUserHtmlYes(uId, uName) {
        var html = "";
        html = "<p class='item' lang='" + uId + "' title='" + uName + "'><span class='nickname'>" + uName + "</span>" +
            "<span class='itemdel' lang='" + uId + "'>x</span></p>";
        return html;
    }

//删除指定用户后要重新赋值
    function limitUserHtmlAgainYes() {
        limitUserHtmlYes = "";
        $("#limitUserCheckYes p[class='item']").each(function (e, b) {
            limitUserHtmlYes += GetLimiUserHtmlYes($(this)[0].lang, $(this)[0].title);
        });
        CreateLimitUserClickYes();//注册指定用户的关闭事件
        limitNumUserYes = $("#limitUserCheckYes span[class='itemdel']").length;
        $("#LimitUserYes").html(limitNumUserYes);
    }

//注册指定用户的关闭事件
    function CreateLimitUserClickYes() {
        $("#limitUserCheckYes span[class='itemdel']").each(function (e, b) {
            $(this).click(function () {
                var userId = $(this)[0].lang;
                $(this).parents('.item').remove();
                limitUserHtmlAgainYes();
            });
        });


    }

//#endregion 指定用户操作
 


  


    function sumbitBtn() {
        //setFun();
        //取值
        //指定角色
        CheckboxRoleList();
        //限制角色
        CheckboxRoleListNo();


        //指定组织
        CheckboxDistrictList();
        //限制组织
        CheckboxDistrictListNo();

        //推送模式
        CheckboxPushList();
        var markupStr = $('#summernote').summernote('code');
        $("#Info").val(markupStr);
    }
//指定角色
    function CheckboxRoleList() {
        var roleList = "";
        var i = 0;
        $("#checkboxRoleList input[type='checkbox']:checked").each(
         function (e, b) {
             i++;
             if (i === 1)
                 roleList += b.value;
             else
                 roleList += "," + b.value;
         });
        $("#RoleListId").val(roleList);
    }
//限制角色
    function CheckboxRoleListNo() {
        var roleListNo = "";
        var i = 0;
        $("#checkboxRoleListNo input[type='checkbox']:checked").each(
         function (e, b) {
             i++;
             if (i === 1)
                 roleListNo += b.value;
             else
                 roleListNo += "," + b.value;
         });
        $("#RoleListIdNo").val(roleListNo);
    }
    //指定组织
    function CheckboxDistrictList() {
        var ids = $("#NavIds").val();
        
        $("#DistrictListId").val(ids);   
    }
    //限制组织
    function CheckboxDistrictListNo() {
        var ids = $("#NavIdsNo").val();

        $("#DistrictListIdNo").val(ids);
    }
//推送模式
    function CheckboxPushList() {
        var pushList = "";
        var i = 0;
        $("#pushIdList input[type='checkbox']:checked").each(
         function (e, b) {
             i++;
             if (i === 1)
                 pushList += b.value;
             else
                 pushList += "," + b.value;
         });
        $("#PushId").val(pushList);
    }

//#endregion

    //加载绑定事件
    var BindControll = function () {
        $("#liAllRole").click(function () {
            $("#liIsCheckUser").is(":hidden") ? $("#DivIsCheckUser").hide() : $("#DivIsCheckUser").show();
            //角色权限
            $("#liIsCheckRole").is(":hidden") ? $("#DivIsCheckRole").hide() : $("#DivIsCheckRole").show();
            //组织权限
            $("#liIsCheckDistrict").is(":hidden") ? $("#DivIsCheckDistrict").hide() : $("#DivIsCheckDistrict").show();
        });

        $("#liIsCheckUser").click(function () {
            $("#DivIsCheckUser").show();
            $("#DivIsCheckRole").hide();
            $("#DivIsCheckDistrict").hide();
        });

        $("#liIsCheckRole").click(function () {
            $("#DivIsCheckUser").hide();
            $("#DivIsCheckRole").show();
            $("#DivIsCheckDistrict").hide();
        });
        $("#liIsCheckDistrict").click(function () {
            $("#DivIsCheckUser").hide();
            $("#DivIsCheckRole").hide();
            $("#DivIsCheckDistrict").show();
        });
    }