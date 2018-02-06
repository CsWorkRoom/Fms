
//#region 页面入口

$(document).ready(function () {
    $("#sumbit-btn").hide();
    InitPage();
    InitEvent();
})
//#endregion

//#region 初始化页面
function InitPage() {
    $('#divAnalysis').collapse("show");
    InitGrid();//初始化子报表列表

    var id = $.trim($("#Id").val());
    //编辑时，不允许修改code代码。禁用
    if (id != null && id != "" && !isNaN(id) && parseInt(id) > 0) {
        //$("#Code").attr("disabled", true);//禁用code
        $("#Code").attr("readonly", "readonly");//禁用code
        ///是否显示保存数据
        $('#saveAllReport').show();
    }

    //默认加载隐藏项
    $("#tbReport").hide();
    //(子-表格报表)添加事件时的验证
    ValidateTbReport();

    //初始化添加子报表的点击事件
    $("#addReportBtn").click(function () {
        OpenReportDiv();
    });
    //初始化(子-表格报表)中的多表头重载事件
    $("#refrushField").click(function () {
        InitTbTopField();
        $("#topFieldDiv").show();
        $(".htCore").attr("class", "table table-bordered");
    });
    //初始化(子-表格报表)中的多表头保存事件
    $("#saveTopF").click(function () {
        SaveTopField();
    });
    SetRegularHt();//正则隐藏赋值

    $("#chartType").change(function () {
        LoadSelect("chartTemp", '~/api/services/api/ChartReport/GetChartTempJsonByType?chartTypeId=' + $(this).val(), "", "get");
    });
}
//#endregion 


//#region 初始化页面表单事件
function InitEvent() {
    $("#saveForm").submitForm({
        beforeSubmit: function () {
            $("#sumbit-btn").button('loading');
            $("#addReportBtn").button('loading');
            $("#closeModel").button('loading');
            $("#saveAllReport").button('loading');
            $("#analysis-sql").button('loading');
            $("#jqGrid a").button('loading');

        },
        success: function (data) {
            if (data.success)
                SavaSuccessData();//刷新父级窗口
            else
                abp.message.error(data.result.message, "保存失败");
            $("#sumbit-btn").button('reset');
            $("#addReportBtn").button('reset');
            $("#closeModel").button('reset');
            $("#saveAllReport").button('reset');
            $("#analysis-sql").button('reset');
            $("#jqGrid a").button('reset');
        },
        error: function () {
            $("#sumbit-btn").button('reset');
            $("#addReportBtn").button('reset');
            $("#closeModel").button('reset');
            $("#saveAllReport").button('reset');
            $("#analysis-sql").button('reset');
            $("#jqGrid a").button('reset');
        }
    })

}
//触发Form
function SaveReport() {
    $("#sumbit-btn").click();
}

//提交信息验证
function handleSubmitForm() {
    //提交前验证表单信息是否正确
    if ($.trim($("#Name").val()) == "") {
        swal("提示信息", "报表名称不能为空！", "error");
        return;
    }
    if ($.trim($("#Code").val()) == "") {
        swal("提示信息", "节点代码不能为空！", "error");
        return;
    }
    if ($.trim($("#Sql").val()) == "") {
        swal("提示信息", "Sql不能为空!", "error");
        return;
    }
    //if ($.trim($("#DbServerId").val()) == "") {
    //    swal("提示信息", "数据库不能为空！", "error");
    //    return;
    //}
    //提交表单信息
    $("#saveForm").submit();
}
//#endregion

//#region 报表管理-公共部分

//解析sql语句，获得字段集合
function AnalysisSql() {
    var sql = $("#Sql").val();
    var dbid = $("#DbServerId").val();

    if ($.trim(sql) == "") {
        abp.message.error("Sql不能为空！！", "提示信息");
        return;
    }
    //if ($.trim(dbid) == "") {
    //    abp.message.error("数据库不能为空！", "提示信息");
    //    return;
    //}
    $("#analysis-sql").button('loading');
    $.ajax({
        type: "post",
        //url: "../api/services/api/Report/AnalysisSql",
        url: "AnalysisSql",
        data: {
            sql: sql,
            dbserverId: dbid
        },
        success: function (e) {
            if (e != "" && e != null) {
                var err = $.parseJSON(e);
                if (err.IsError) {
                    $('#divAnalysis').collapse("hide");
                    $('#saveAllReport').hide();
                    if (err.Message.indexOf("<title>") != -1) {
                        var start = err.Message.indexOf("<title>");
                        var end = err.Message.indexOf("</title>");
                        abp.message.error(err.Message.substring(start + 7, end), "解析失败");
                    }
                    else
                        abp.message.error(err.Message, "解析失败");
                    $("#analysis-sql").button('reset');
                    return;
                }
                else {
                    //解析后的字段集合
                    var nowFieldList = $.parseJSON(err.Params);
                    //原有的字段集合-隐藏域
                    var oldFields = $("#FieldJson").val();

                    //赋值字段json至隐藏控件
                    //以备其他报表字段绑定需要（如：绑定字段至表格列表）
                    $("#FieldJson").val(JSON.stringify(nowFieldList));
                    abp.message.success("", "解析成功");
                    $('#divAnalysis').collapse("show");
                    $('#saveAllReport').show();
                    $("#analysis-sql").button('reset');
                }
            }
        },
        error: function (e) {
            if (e.responseText.indexOf("<title>") != -1) {
                var start = e.responseText.indexOf("<title>");
                var end = e.responseText.indexOf("</title>");
                abp.message.error(e.responseText.substring(start + 7, end), "解析失败");
            }
            else
                abp.message.error(e.responseText, "解析失败");
            $('#saveAllReport').hide();
            $("#analysis-sql").button('reset');
        }
    });
}

//根据选择的报表项打开对应的报表编辑框
//只支持新增报表初始化
function OpenReportDiv() {
    var rpType = $("#AReportType").val();
    var appType = $("#AAppType").val();

    //此处应该存放当前子报表的完整信息
    var childRp = {
        'ChildReportId': 0,
        'ChildReportType': rpType,
        'ApplicationType': appType,
        'IsOpen': true,
        'ChildReportJson': []
    };
    //校验当前的报表是否已添加
    if (ValidateReport(rpType, appType)) {
        abp.message.info("已添加[" + rpType + "][" + appType + "]类型的子报表", "信息提示");
        return;
    }
    $("#curReportType").val(rpType);//当前编辑的报表类型

    //表格式报表(初始化)
    if (rpType == 1 || rpType == 2) {

        //初始化新增报表
        $("#currTbReport").val(JSON.stringify(childRp));

        InitTbReportModel();
    }
        //图形报表
    else if (rpType == 3) {
        //初始化新增报表
        $("#currChartReport").val(JSON.stringify(childRp));
        InitChartReportModel();
    }
        //RDLC报表
    else if (rpType == 4) {
        //初始化新增报表
        $("#currRdlcReport").val(JSON.stringify(childRp));
        InitRdlcReportModel();
    }

    //显示保存提交事件
    $('#saveAllReport').show();
}


//校验报表是否已添加
function ValidateReport(rpType, appType) {
    var childs = $("#ChildReportListJson").val();
    if (childs != null && childs.length > 0) {
        var childArr = $.parseJSON(childs);
        if (childArr != null && childArr.length > 0) {
            for (var i = 0; i < childArr.length; i++) {
                var child = childArr[i];
                if (child.ChildReportType == rpType && child.ApplicationType == appType) {
                    return true;
                }
            }
        }
    }
    return false;
}

//#endregion



//#region 子报表列表

//#region 子报表列表grid加载
function InitGrid() {
    var childs = $("#ChildReportListJson").val();

    if (childs != null && childs.length > 0) {
        var childJson = $.parseJSON(childs);

        $.jgrid.gridUnload("jqGrid");//先卸载

        //$('#jqGrid').jqGrid("GridUnload");

        //如果高度过小，就设置成300高度
        var intHeight = window.innerHeight - 527;
        if (intHeight < 200) {
            intHeight = 200;
        }
        // alert($("#saveForm").width());
        //再加载
        $("#jqGrid").jqGrid({
            data: childJson,
            styleUI: 'Bootstrap',
            datatype: "local",
            colModel: [
                 {
                     label: "操作区",
                     name: "actions",
                     width: 100,
                     formatter: ActFmatter
                 },
                { label: '编号ID', name: 'ChildReportId', width: 150 },
                {
                    label: '报表类型', name: 'ChildReportType', width: 150,
                    formatter: function (cellvalue, options, rowObject) {
                        var res = "";
                        switch (cellvalue.toString()) {
                            case "1":
                                res = "表格报表";
                                break;
                            case "2":
                                res = "键值报表";
                                break;
                            case "3":
                                res = "图形报表";
                                break;
                            case "4":
                                res = "RDLC报表";
                                break;
                        }
                        return res;
                    }
                },
                { label: '应用侧', name: 'ApplicationType', width: 150 },
                //{ label: 'Freight', name: 'Freight', width: 150, summaryType: 'sum', formatter: 'number' },
                { label: '是否开启', name: 'IsOpen', width: 150 }
            ],
            viewrecords: false,
            loadonce: false,
            height: intHeight,
            // width: $("#saveForm").width(),//window.innerWidth - 25,
            gridComplete: function () {
                $("#jqGrid").setGridWidth($("#saveForm").width());
                $("#jqGrid").closest(".ui-jqgrid-bdiv").css({ "overflow-x": "hidden" });
                $("#jqGrid").closest(".ui-jqgrid-bdiv").css({ "overflow-y": "hidden" });
            },
            rowNum: 20,//默认分页大小-在框架动态赋值
            rowList: [20, 30, 50],//传入分页大小的下拉-在框架动态复制
            rownumbers: true,//显示行号
            rownumWidth: 25,//行号列的宽度
            pager: "#jqGridPager",
            caption: "<i class='fa fa-bar-chart'></i> <b>子报表列表</b>"//设置和显示表格标题
        });
    }
}
//#endregion

//#region 子报表列表格式化操作区

//cellvalue - 当前cell的值
//options - 该cell的options设置，包括{rowId, colModel,pos,gid}
//rowObject - 当前cell所在row的值，如{ id=1, name="name1", price=123.1, ...}
function ActFmatter(cellvalue, options, rowObject) {

    var d = '<a href="javascript:void(0)" onclick="EditReport(' + rowObject.ChildReportId + ',' + rowObject.ChildReportType + ',\'' + rowObject.ApplicationType + '\')">编辑</a> ';
    d += '<a href="javascript:void(0)" onclick="DeleteReport(' + rowObject.ChildReportId + ',' + rowObject.ChildReportType + ',\'' + rowObject.ApplicationType + '\')">关闭</a> ';
    return d;
};
//#endregion

//#region 子报表-编辑
function EditReport(ChildReportId, ChildReportType, ApplicationType) {

    //加载tb报表数据
    var childs = $("#ChildReportListJson").val();
    if (childs != null && childs.length > 0) {
        var childJson = $.parseJSON(childs);
        for (var i = 0; i < childJson.length; i++) {
            var child = childJson[i];

            if (child.ChildReportType == ChildReportType && child.ChildReportId == ChildReportId
                && child.ApplicationType == ApplicationType) {

                $("#curReportType").val(ChildReportType);//当前编辑的报表类型

                //表格式报表
                if (ChildReportType == 1 || ChildReportType == 2) {
                    //给子报表隐藏区域赋值
                    $("#currTbReport").val(JSON.stringify(child));
                    InitTbReportModel();//打开表格模态，并加载相关数据
                }
                    //图形报表
                else if (ChildReportType == 3) {
                    //给子报表隐藏区域赋值
                    $("#currChartReport").val(JSON.stringify(child));
                    InitChartReportModel();//打开rdlc模态，并加载相关数据
                }
                    //RDLC报表
                else if (ChildReportType == 4) {
                    //给子报表隐藏区域赋值
                    $("#currRdlcReport").val(JSON.stringify(child));
                    InitRdlcReportModel();//打开rdlc模态，并加载相关数据
                }

            }
        }
    }
}
//#endregion

//#endregion

//放大SQL编辑区
var strModalId = "";//记录模态框ID
function MaxSql() {
    if (strModalId != "") {
        $("#" + strModalId).remove();
    }
    var intWidth = $(window).width();
    var intHeight = $(window).height();
    var strContent = '<div class="sqlIde" style="width:' + (intWidth - 71) + 'px;height:' + (intHeight - 150) + 'px"></div>';
    var strFooter = "<button class='btn btn-primary' type='button' title='将显示修改后的值，但并未保存数据。' onclick='RefreshSql()'><i class='fa fa-save'></i> 确定</button>";
    strModalId = "id" + new Date().getTime()
    ModeDialogContent(strModalId, "查看SQL", strContent, strFooter, (intWidth - 30), (intHeight - 30));
    LoadTxtIde("#" + strModalId + " .sqlIde", $("#Sql").val());
}
//刷新SQL
var RefreshSql = function () {
    var strSql = GetTxtIdeVal("#" + strModalId + " .sqlIde");
    $("#Sql").val(strSql);
    $("#" + strModalId).modal("hide");
}
//End放大SQL编辑区