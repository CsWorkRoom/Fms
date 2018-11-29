//**********************************************
//该页面仅支持PC端表格报表的展示
//**********************************************

$(document).ready(function () {
    $(window).resize();

    InitGrid();
});

//随着浏览器的变化而变化
$(window).resize(function () {
    WinResize("jqGrid", "navMenu", "jqGridPager");
});


//设置JqGrid表头背景
var SetJqGridHtableBkBroundColor = function (strColor) {
    if ($.trim(strColor) == "") {
        strColor = "#f5f6fa";
        return;
    }
    $(".ui-jqgrid-htable,.ui-jqgrid-labels").css("background-color", strColor);
}

var InitGrid = function () {
    var dbPath = $("#dbPath").val();//获得db备份数据路径
    $.jgrid.gridUnload("jqGrid");//先卸载
    $("#jqGrid").jqGrid({
        url: 'GetSubs',
        postData: {
            path: dbPath
        },
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",//如果url中需要回调函数，则此处格式为jsonp
        colModel: [
           { label: '类型', name: 'Type', width: 50, key: true, editable: false },
            {
                label: '名称',
                name: 'Name',
                width: 100,
                editable: false
            },
            {
                label: '完整名称',
                name: 'FullName',
                width: 100,
                hidden: true,
                editable: false
            },
            {
                label: '大小',
                name: 'Size',
                width: 100,
                editable: false
            },
            {
                label: '创建时间',
                name: 'CreateTime',
                width: 100,
                editable: false
            },
             {
                 label: '更新时间',
                 name: 'UpdateTime',
                 width: 50,
                 editable: false
             }
             //,{
             //    label: '操作',
             //    name: 'actions',
             //    align: "center",//横向位置
             //    hidden: false,//是否隐藏
             //    width: 80,//列宽度
             //    formatter: function (cellvalue, options, rowObject) {
             //        var lbl = "";
             //        var filePath = rowObject["FullName"];//文件路径
             //        return lbl;
             //    }
             //}
        ],
        autowidth: true,
        altRows: true,
        showPager: false,
        rownum: -1,
        shrinkToFit: true//是否列宽度自适应。true=适应 false=不适应
        //,pager: "#jqGridPager"
    });

    SetJqGridHtableBkBroundColor("rgba(244, 247, 251, 0.34);");//设置表头背景色，如果为空就为#f5f5f5
    $(window).unbind("onresize");
    $("#jqGrid").setGridHeight($(window).height());
    $(window).bind("onresize", this);

    var o = jQuery("#jqGrid");
    var total = o.jqGrid('getGridParam', 'records'); //获取查询得到的总记录数量

    //设置rowNum为总记录数量并且刷新jqGrid，使所有记录现出来调用getRowData方法才能获取到所有数据
    o.jqGrid('setGridParam', { rowNum: total }).trigger('reloadGrid');

}