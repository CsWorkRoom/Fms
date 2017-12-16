
//增加编辑区
var LoadTxtIde = function (ide, value) {
    if (ide==null || $.trim(ide)=="")
        return;
    var divTxtIDE = '<div class="divTxt">';
    divTxtIDE += '<textarea cols="2" rows="10" class="txtLiNum" disabled></textarea>';
    divTxtIDE += '<textarea style="resize:none;width:' + intWidth + ';height:' + intHeight + '" name="co" cols="60" rows="10" wrap="off" class="txtIDEContent"></textarea>';
    divTxtIDE += '</div>';
    $(ide).html(divTxtIDE);
    $(ide).find(".txtIDEContent").val(value);
    $(".txtIDEContent").bind("keyup change", function () {
        keyUp();
    });
    $(".txtIDEContent").scroll(function () {
        autoScroll();
    });
    var intHeight = $(ide).height();
    var intWidth = $(ide).width();
    $(".txtIDEContent").height(intHeight);
    $(".txtIDEContent").width(intWidth);
    $(".txtLiNum").height($(".txtIDEContent").height());
    $(".txtLiNum").width();
    keyUp();
}
//读取IDE值
var GetTxtIdeVal = function (ide) {
    return $(ide).find(".txtIDEContent").val();
}
//读取IDE值
var SetIdeVal = function (ide, value) {
    $(ide).find(".txtIDEContent").val(value);
}


var num = "";//用于记录行号
//键盘弹起事件
var keyUp = function () {
    var str = $(".txtIDEContent").val();
    var strstr = str.replace(/\r/gi, "");
    strstr = strstr.split("\n");
    var n = strstr.length;
    line(n);
}

//清加文本续号
var line = function (n) {
    for (var i = 1; i <= n; i++) {
        if (document.all) {
            num += i + "\r\n";
        } else {
            num += i + "\n";
        }
    }
    $(".txtLiNum").val(num);
    num = "";
}

//滚动条事件
var autoScroll = function () {
    var nV = 0;
    if (!document.all) {
        nV = $(".txtIDEContent").scrollTop();
        $(".txtLiNum").scrollTop(nV);
        setTimeout("autoScroll()", 20);
    }
}