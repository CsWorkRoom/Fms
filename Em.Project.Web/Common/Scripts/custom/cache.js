/**********Cookie的操作-start**********/
///读取Cooke中的数据
///name:名称
function GetCookie(name) {
    if (name == null || $.trim(name) == "") {
        return;
    }
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = parent.top.document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}


///删除浏览器中指定的Cookie
///mame:名称
function DelCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        parent.top.document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}

///设置Cookie
///mame:名称
///value:值
///time:有效时间
function SetCookie(name, value, time) {
    if (name == null || $.trim(name) == "" || value == null || $.trim(value) == "") {
        return;
    }
    if (time == null || time == undefined || isNaN(time) == true)
        parent.top.document.cookie = name + "=" + escape(value);
    else
        parent.top.document.cookie = name + "=" + escape(value) + ";expires=" + time;
}


//读取COOKIE集合中的参数
var GetCookieParament = function (CokName, itmeName) {
    var ModeFrameIds = GetCookie(CokName);
    if (ModeFrameIds == null || ModeFrameIds == "") {
        return;
    }
    var FrameIds = ModeFrameIds.split("|");
    for (var i = 0; i < FrameIds.length; i++) {
        if (FrameIds[i] != null) {
            var Ids = FrameIds[i].split(":");
            if (Ids.length < 2 || Ids[0] == null || Ids[0] == "" || Ids[1] == null || Ids[1] == "") {
                break;
            }
            if (Ids[0] == itmeName) {
                return Ids[1];
                break;
            }
        }
    }
    return null;
}

/**********Cookie的操作-end**********/