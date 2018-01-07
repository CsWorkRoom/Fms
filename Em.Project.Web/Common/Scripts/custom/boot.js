/// <reference path="boot.js" />

///得到URL地址的单个参数
///strKey：URL地址中的参数名称
var GetUrlParam = function (strKey) {
    var reg = new RegExp("(^|&)" + strKey + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

///解析URL参数为JSON
///strUrl:URL地址
var GetParamJson = function (strUrl) {
    var strParam = "";
    var strJson = "";
    if (strUrl.indexOf("?") != -1) {
        strParam = strUrl.split("?")[1];
    }
    strParam = strParam.replace(new RegExp("&", "gm"), "','");
    var strJson = strParam.replace(new RegExp("=", "gm"), "':'");
    return strJson;
};

///解析URL参数为JSON
///strUrl:URL地址
var GetParamJsonByUrl = function (strUrl) {
    if (strUrl.indexOf("?") != -1) {
        var strParam = strUrl.split("?")[1];
        strParam = strParam.replace(new RegExp("&", "gm"), "','").replace(new RegExp("=", "gm"), "':'");
        strParam = "{'" + strParam + "'}";
        var parJson = JSON.parse(strParam.replace(/'/g, "\""));
        return parJson;
    }
    return null;
};

///根据相对路径得到完整URL
///strUrl:URL相对地址
var GetPathoOld = function (strUrl) {
    if (strUrl.toLowerCase().indexOf("https:") != -1 || strUrl.toLowerCase().indexOf("http:") != -1 || strUrl.toLowerCase().indexOf("file:") != -1) {
        return strUrl;
    }

    var strHref = window.location.href.split("/")[0] + "//" + window.location.host;
    if (strUrl.indexOf("/") == 0 || strUrl.indexOf("~/") == 0) {
        strUrl = bootPATH + strUrl.replace("~/", "/");
        //strUrl = strHref + strUrl.replace("~/", "/");
    }
    else {
        var arrHref = window.location.pathname.split("/");//获取当前的相对路径级

        var intBackNum = 1;
        //对../进行退级计算
        var strBack = "../";
        while (strUrl.indexOf(strBack) == 0) { //退回上一级目录
            strBack += strBack;
            intBackNum++;
        }
        //减去多余的../符
        if (intBackNum > 1) {
            strBack = strBack.replace("../", "");
        }
        strUrl = strUrl.replace(strBack, "");//替换退格符
        arrHref.length = arrHref.length - intBackNum;//减去路径级

        var strPath = arrHref.join("/");//组成路径
        var strSpace = (strPath.length == 0 ? "" : "/");
        strUrl = strHref + "/" + strPath + strSpace + strUrl;
    }
    return strUrl;
};

///根据相对路径得到完整URL
///strUrl:URL相对地址
var GetPath = function (strUrl) {
    if (strUrl.toLowerCase().indexOf("https:") != -1 || strUrl.toLowerCase().indexOf("http:") != -1 || strUrl.toLowerCase().indexOf("file:") != -1) {
        return strUrl;
    }

    if (strUrl.indexOf("/") == 0 || strUrl.indexOf("~/") == 0) {
        strUrl = bootPATH + strUrl.replace("~/", "/").substr(1);
    }
    else {
        strUrl = bootPATH + strUrl;
    }
    return strUrl;
};


//数组对象排序
var compare = function (prop) {
    return function (obj1, obj2) {
        var val1 = obj1[prop];
        var val2 = obj2[prop];
        if (!isNaN(Number(val1)) && !isNaN(Number(val2))) {
            val1 = Number(val1);
            val2 = Number(val2);
        }
        if (val1 < val2) {
            return -1;
        } else if (val1 > val2) {
            return 1;
        } else {
            return 0;
        }
    }
}

//深复制对象
var cloneObj = function (obj) {
    var newObj = {};
    if (obj instanceof Array) {
        newObj = [];
    }
    for (var key in obj) {
        var val = obj[key];
        //newObj[key] = typeof val === 'object' ? arguments.callee(val) : val; //arguments.callee 在哪一个函数中运行，它就代表哪个函数, 一般用在匿名函数中。  
        newObj[key] = typeof val === 'object' ? cloneObj(val) : val;
    }
    return newObj;
};

//验证url合法性
function IsURL(str_url) {
    var strRegex = '^((https|http|ftp|rtsp|mms)?://)'
    + '?(([0-9a-z_!~*\'().&=+$%-]+: )?[0-9a-z_!~*\'().&=+$%-]+@)?' //ftp的user@ 
    + '(([0-9]{1,3}.){3}[0-9]{1,3}' // IP形式的URL- 199.194.52.184 
    + '|' // 允许IP和DOMAIN（域名） 
    + '([0-9a-z_!~*\'()-]+.)*' // 域名- www. 
    + '([0-9a-z][0-9a-z-]{0,61})?[0-9a-z].' // 二级域名 
    + '[a-z]{2,6})' // first level domain- .com or .museum 
    + '(:[0-9]{1,4})?' // 端口- :80 
    + '((/?)|' // a slash isn't required if there is no file name 
    + '(/[0-9a-z_!~*\'().;?:@&=+$,%#-]+)+/?)$';
    var re = new RegExp(strRegex);
    //re.test() 
    if (re.test(str_url)) {
        return (true);
    } else {
        return (false);
    }
}



//获取一个唯一数
function generateUUID() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
};


//把json对象转化为字符串
function SerializeJsonToStr(oJson) {
    if (typeof (oJson) == typeof (false)) {
        return oJson;
    }
    if (oJson == null) {
        return "null";
    }
    if (typeof (oJson) == typeof (0))
        return oJson.toString();
    if (typeof (oJson) == typeof ('') || oJson instanceof String) {
        oJson = oJson.toString();
        oJson = oJson.replace(/\r\n/, '\\r\\n');
        oJson = oJson.replace(/\n/, '\\n');
        oJson = oJson.replace(/\"/, '\\"');
        return '"' + oJson + '"';
    }
    if (oJson instanceof Date) {
        return "parseDate('" + oJson.format("yyyy-MM-dd") + "')";
    }
    if (oJson instanceof Array) {
        var strRet = "[";
        for (var i = 0; i < oJson.length; i++) {
            var value = SerializeJsonToStr(oJson[i]);
            if (value != "null") {
                if (strRet.length > 1) {
                    strRet += ",";
                }
                strRet += value;
            }
        }
        strRet += "]";
        return strRet;
    }
    if (typeof (oJson) == typeof ({})) {
        var strRet = "{";
        var rowFlag = false;
        for (var p in oJson) {
            if (strRet.length > 1)
                strRet += ",";
            var value = SerializeJsonToStr(oJson[p]);
            if (value != null && value != "null") {
                strRet += '"' + p.toString() + '":' + value;
                rowFlag = true;
            }
        }
        strRet += "}";
        if (!rowFlag) {
            return "null";
        }
        return strRet;
    }
}

/*
生成随机数列表（可能会有重复）
intLentgh：要产生多少个随机数
intMinNum：产生随机数的最小值
intMaxNum：产生随机数的最大值
*/
var CreateRandomNum = function (intLentgh, intMinNum, intMaxNum) {
    var arr = [];
    for (var i = intMinNum; i <= intMaxNum; i++)
        arr.push(i);
    arr.sort(function () {
        return 0.5 - Math.random();
    });
    arr.length = intLentgh;
    return arr;
}

/*
生成随机数列表（不会重复）
intLentgh：要产生多少个随机数
intMinNum：产生随机数的最小值
intMaxNum：产生随机数的最大值
*/
var CreateRandomNumS = function (intLentgh, intMinNum, intMaxNum) {
    var arr = [];
    var json = {};
    while (arr.length < intLentgh) {
        //产生单个随机数
        var ranNum = Math.ceil(Math.random() * (intMaxNum - from)) + intMinNum;
        //通过判断json对象的索引值是否存在 来标记 是否重复
        if (!json[ranNum]) {
            json[ranNum] = 1;
            arr.push(ranNum);
        }
    }
    return arr;
}

///读取IFRAMID
var GetIfram = function () {
    //查询父级IFRAM的ID集
    var framIds = parent.frames;
    var framId = "";//IFRAM的ID
    for (var i = 0; i < framIds.length; i++) {
        if (framIds[i].location.href == location.href) {
            framId = framIds[i].name;
            break;
        }
    }
    return framId;
}
///end读取IFRAMID

//刷新父级列表
var RefreshFram = function () {
    var strFramId = GetIfram();
    if (strFramId == null || strFramId == "" || strFramId == undefined) {
        return;
    }
    var ModeFrameIds = GetCookie("ModeFrameIds");//window.top.$("#hidModeFrameIds").val();
    if (ModeFrameIds == null || ModeFrameIds == "") {
        return;
    }

    strFramId = GetCookieParament("ModeFrameIds", strFramId);//查找父级的ID
    var objFramId = parent.top.frames[strFramId];//先用ID在顶级查父级对像
    if (objFramId == null || objFramId == undefined) {
        objFramId = parent.frames[strFramId];//再用ID在当前父级查找父级对像
    }
    if (objFramId == null || objFramId == undefined) {
        objFramId = GetRefreshFramObj(GetCookieParament("ModeFrameIds", strFramId), strFramId);
    }
    if (objFramId != null && objFramId != undefined) {//如果查到了，就刷新
        //判断刷新方式
        if (objFramId.DoSearch && typeof (objFramId.DoSearch) == "function") {//判断是否存在查询列表的方法
            objFramId.window.DoSearch();
        } else {
            objFramId.src = objFramId.src;
           // objFramId.location.reload();
        }
    } else {
        window.location.reload();
    }
}

var GetRefreshFramObj = function (strFramId, FramId) {
    if (strFramId == null || strFramId=="") {
        return null;
    }
    if (parent.frames[strFramId] != null) {
      var iframes = parent.frames[strFramId].$("iframe");
      for (var i = 0; i < iframes.length; i++) {
          if (iframes[i].id == FramId) {
              return iframes[i];
              break;
          }
        }
    }
    GetRefreshFramObj(GetCookieParament("ModeFrameIds", strFramId), FramId);
}


//数据保存成功刷新父级窗口
var SavaSuccessData = function () {
    RefreshFram();//刷新父级页面
    //提示消息 
    swal({
        title: "保存提示",
        text: "数据已保存成功!是否返回列表页?",
        type: "success",
        confirmButtonText: "是",
        cancelButtonText: "否",
        showCancelButton: true,
    },
   function (isConfirm) {
       if (isConfirm) {
           ColseModel();
       }
   });
}
//关闭模型窗口
var ColseModel = function () {
    var closeNum = parent.$(".close").length;
    if (closeNum >= 1) {
        parent.$(".close").eq(closeNum - 1).click();
    }
    else {
        if ($(".colose").length >= 1) {
            $(".close").click();
        }
        else {
            var blnOpenWind = true;//是否以打开浏览器方式打开窗口
            $(parent.$(".page-tabs-content a")).each(function () {//如果是tabs时，就关闭tabs
                if ($(this).attr("data-id").indexOf(window.location.href) >= 0 || window.location.href.indexOf($(this).attr("data-id")) >= 0) {
                    blnOpenWind = false;
                    $(this).find(".fa-remove").click();
                    return true;
                }
            });
            if (blnOpenWind) {//如果以浏览器窗口打开时，就执行以下方法
                window.close();
            }
        }
    }
}

//提交数据
var SubmitFormData = function (fromID, subButId) {
    $(fromID).submitForm({
        beforeSubmit: function () {           
            $(subButId).button('loading');
        },
        success: function (data) {
            if (data.success)
                SavaSuccessData();//刷新父级窗口
            else
                abp.message.error(data.result.message, "保存失败");
            $(subButId).button('reset');
        },
        error: function () {
            $(subButId).button('reset');
        }
    })
}

////点关闭菜单时刷新父级列表
//$(function () {
//    parent.$(".close").click(function () {
//        var isSubmint = $("#isSubmint").val();
//        if (isSubmint != null && isSubmint != undefined && isSubmint == 1) {//是否刷新父窗口
//            RefreshFram();
//        }
//    });
//});

///底部按钮区悬浮
$(window).scroll(function () {
    $(".bottomPage").animate({ bottom: -getScrollTop() }, { queue: false });
});
$(window).scroll();

function getScrollTop() {
    var scrollPos;
    if (window.pageYOffset) {
        scrollPos = window.pageYOffset;
    }
    else if (document.compatMode && document.compatMode != 'BackCompat')
    { scrollPos = document.documentElement.scrollTop; }
    else if (document.body) { scrollPos = document.body.scrollTop; }
    return scrollPos;
}
///end底部按钮区悬浮

//设置JqGrid列表高宽
var WinReJqGridSize = function (strNavMenu, jqGridId) {
    var intNavMenu = 0;
    //判断条件label高度是否置顶
    if (strNavMenu != null) {
        $("#" + strNavMenu + " .control-label").each(function () {
            if ($(this).height() > 17) {
                $(this).css("margin-top", "0px");
            } else {
                $(this).css("margin-top", "10px");
            }
        });
        intNavMenu = $("#" + strNavMenu).height();
    }
    $("#" + jqGridId).setGridWidth($(window).width() - 45);
    $("#gview_" + jqGridId + " .ui-jqgrid-bdiv").css("width", "auto");
    $("#" + jqGridId).css("width", "auto");

    $("#gbox_" + jqGridId).css("width", $("#gbox_" + jqGridId).width() + 3);
    var grdHeight = ($(window).height() - intNavMenu - $("#gview_" + jqGridId + " .ui-jqgrid-hdiv").height() - 28);
    $("#" + jqGridId).setGridHeight(grdHeight);
}

//获取浏览器类型
var GetBrowser = function () {
    //浏览器的判断
    var OsObject = navigator.userAgent.toLowerCase();
    if (OsObject.indexOf("msie") > 0) {
        return "msie";
    }
    if (OsObject.indexOf("firefox") > 0) {
        return "firefox";
    }
    if (OsObject.indexOf("safari") > 0) {
        return "safari";
    }
    if (OsObject.indexOf("camino") > 0) {
        return "camino";
    }
    if (OsObject.indexOf("gecko/") > 0) {
        return "gecko";
    }
    return OsObject;
}

//禁止记录密码
$(function () {
    $(".noRecordPwd").bind("click keyup change paste keydown input", function () {
        if ($(this).val().length > 0)
            $(this).attr("type", "password");
        else
            $(this).attr("type", "text");
    });
    $(".noRecordPwd").click();
});