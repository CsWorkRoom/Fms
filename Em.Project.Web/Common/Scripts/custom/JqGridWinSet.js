
//设置列表高宽
var WinResize = function (jqGrid, navMenu, jqGridPager) {
    var fieldWds = 0;
    var bolShrinkToFit = true;//默认自适应
    if ($("#columnSumWidth") != null) {//检查是否有宽度保存
        fieldWds = parseInt($("#columnSumWidth").val());
    } else {
        var columnNames = $("#" + jqGrid + " tr").eq(0).find("td:visible");//过虑隐藏的列，得到真实的列数
        fieldWds = columnNames.length * 60;
    }
    if (fieldWds > window.innerWidth) {//如果每列平均最小宽度小于60px时，停止用自适应
        bolShrinkToFit = false;
    }

    //向JQGrid动态注入事件
    $("#" + jqGrid).jqGrid('setGridParam', {  // 重新加载数据
        shrinkToFit: bolShrinkToFit,
        loadError: function (xhr, status, error) {
            abp.message.error("信息加载有异常，请联系管理员", "错误信息");
        },
        gridComplete: function () {
            $("#" + jqGrid + " .ui-row-ltr:first").focus();//加载完数据滚动条置顶
            //终止表列头事件冒泡
            $.each(document.getElementsByName("hrefLike"), function () {
                this.addEventListener('click', function (e) {
                    e.stopPropagation();
                }, false);
            });
            ////设置冻结列的内容页的起起始坐标
            $("#gview_" + jqGrid + " .frozen-bdiv").css("top", $("#gview_" + jqGrid + " .ui-jqgrid-hdiv").height());
            SetShrinkToFit(jqGrid);
            SetDefultDataImg(jqGrid);
        },
        resizeStop: function () {//改变列宽度后引发
            SetFrozenTr(jqGrid, navMenu, jqGridPager);
        },
        loadComplete: function () {
            SetDefultDataImg(jqGrid);
        },
        onPaging: function () {
            SetFrozenTr(jqGrid, navMenu, jqGridPager);
        },
        beforeRequest: function () {
            $(".Nodata").remove();//移除暂无数据层
        }
    });
    //End向JQGrid动态注入事件
    //计算高宽    
    $("#" + jqGrid).setGridWidth($(window).width() - 1);
    //end计算高宽

    //消除滚动条的设置
    var jqDdiv = $("#gview_" + jqGrid + " .ui-jqgrid-bdiv").eq(0);
    $(jqDdiv).css("width", $(jqDdiv).width() + 5);
    //end消除滚动条的设置

    //设置表格的标题栏的宽度 
    var jagridHd = $("#gview_" + jqGrid + " .ui-jqgrid-hdiv .ui-jqgrid-hbox table tr").eq(0).find("th");
    var jqGridFrozenHd = $("#gview_" + jqGrid + " .frozen-div table tr").eq(0).find("th");
    for (var h = 0; h < jqGridFrozenHd.length; h++) {
        $(jqGridFrozenHd[h]).width($(jagridHd[h]).width());
    }
    //end设置表格的内容每行的宽度 

    //设置表格的内容每行的宽度 
    var jagridTd = $("#" + jqGrid + " tr").eq(0).find("td");
    var jqGridFrozenTd = $("#" + jqGrid + "_frozen tr").eq(0).find("td");;
    for (var d = 0; d < jqGridFrozenTd.length; d++) {
        $(jqGridFrozenTd[d]).width($(jagridTd[d]).width());
    }
    //end设置表格的内容每行的宽度 
    SetFrozenTr(jqGrid, navMenu, jqGridPager);
    $("#gview_" + jqGrid).css("overflow", "hidden");//冻结引起的区域滚动条
    $(".jqg-second-row-header").addClass("table-bordered");//多表头首行无边线

    SetShrinkToFit(jqGrid);
}
//设置冻结的高度
var SetFrozenTr = function (jqGrid, navMenu, jqGridPager) {    
    SetPeportSize(jqGrid, navMenu, jqGridPager);//设置报表大小
    //设置每行的高度 
    var jagridTr = $("#" + jqGrid + " tr");
    var jqGridFrozen = $("#" + jqGrid + "_frozen tr");
    for (var h = 1; h < jagridTr.length; h++) {
        $(jqGridFrozen[h]).height($(jagridTr[h]).height());
    }
    $("#gview_" + jqGrid + " td").css("vertical-align", "middle");
    var gviewTableHeight = $("#gview_" + jqGrid + " .ui-jqgrid-hdiv .ui-jqgrid-hbox table").height() - 1;

    //设置标题栏
    $("#gview_" + jqGrid + " .frozen-div").height(gviewTableHeight);
    $("#gview_" + jqGrid + " .frozen-div table").height(gviewTableHeight);
    var jagridHeadTr = $("#gview_" + jqGrid + " .ui-jqgrid-hdiv .ui-jqgrid-hbox .ui-jqgrid-htable tr");
    var jqGridFrozenHeadTr = $("#gview_" + jqGrid + " .frozen-div .ui-jqgrid-htable tr");

    for (var i = 0; i < jagridHeadTr.length; i++) {
        $(jqGridFrozenHeadTr[i]).height($(jagridHeadTr[i]).height());
    }
    $("#gview_" + jqGrid + " .frozen-bdiv").css("top", $("#gview_" + jqGrid + " .ui-jqgrid-hdiv").height());
    //End设置标题栏
}

//设置报表大小
var SetPeportSize = function (jqGrid, navMenu, jqGridPager) {
    var intTitleBar = $(".ui-jqgrid-titlebar").height() + 20;
    if ($(".ui-jqgrid-titlebar").is(":hidden"))
        intTitleBar = $(".ui-jqgrid-titlebar").height() - 17;
    var grdHeight = $(window).height() - (navMenu != null && navMenu != "" ? $("#" + navMenu).height() : 0) - $(".ui-jqgrid-hdiv").height() - (jqGridPager != null && jqGridPager != "" ? $("#" + jqGridPager).height() : 0) - intTitleBar;
    $("#" + jqGrid).setGridHeight(grdHeight);
}

//设置没有数据时的默认图片及文字
var SetDefultDataImg = function (jqGrid) {
    var rowData = $("#" + jqGrid).jqGrid("getRowData");
    var parentVid = $("#" + jqGrid).parent();
    $(parentVid).css("text-align", "center");
    if (rowData.length <= 0) {
        if ($(".Nodata").length <= 0) {
            var strHtml = "<span class='Nodata'><i aria-hidden='true' class='fa fa-recycle fa-4x' style='line-height: 20px;'></i><span style='font-size: 40px;margin-left: 20px;'>未查找到数据！</span></span>";
            $(parentVid).append(strHtml);
        }
        $("#gview_" + jqGrid + " .frozen-bdiv").hide();
    } else {
        $("#gview_" + jqGrid + " .frozen-bdiv").show();
        $(".Nodata").remove();
    }
}

//解决多表头时顶部线变粗及对其它浏览器错位的支持
var SetShrinkToFit = function (jqGrid) {
    if ($("#gview_" + jqGrid + " .ui-jqgrid-hdiv .ui-jqgrid-hbox table tr").length > 1) {
        $("#gview_" + jqGrid + " .ui-jqgrid-hdiv").css("top", "-1px");
        $("#gview_" + jqGrid + " .ui-jqgrid-bdiv").eq(0).css("top", "-1px");
        var strTop = $("#gview_" + jqGrid + " .frozen-bdiv").css("top");
       // var intTop = $("#gview_" + jqGrid + " .frozen-bdiv").position().top;//此种获取位标的方式，有部份浏览器下，可能出现不兼容的情况。
        if (strTop != null && $.trim(strTop) !="") {
            var intTop = parseInt(strTop.replace("px", ""));
            var frozenBdivTop = intTop - 1;
            $("#gview_" + jqGrid + " .frozen-bdiv").css("top", frozenBdivTop + "px");
        }
    }
   //针对不同浏览器的处理
    //var BrowserType = GetBrowser();//得到浏览器版本
    //switch (BrowserType) {
    //    case "firefox"://针对火狐浏览器做特殊处理
    //      //  $("#gview_" + jqGrid + " .ui-jqgrid-bdiv").css("left", "1px");
    //        break;
    //}
}