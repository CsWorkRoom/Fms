
//加载表格/键值报表。打开模态框
function InitTbReportModel() {
    $('#tbReport').modal('show');//打开tb的模态框
    //给上级模态窗的关闭按钮，添加下级模态窗口关闭事件 
    parent.$(".close").click(function () {
        $('#tbReport').modal('hide');
    });
    //show完毕前执行
    $('#tbReport').on('shown', function () {
        //加上下面这句！解决了~
        $(document).off('focusin.modal');
    });
    var $currChildReport = $("#currTbReport");

    InitTbGrid();//初始化表格字段grid

    InitTbReportBase(); //初始化基础信息

    InitTbReportEvent()//初始化事件列表

    InitTbTopField();//初始化加载多表头

    InitFilter()//初始化筛选grid
}

//保存表格式报表信息
function SaveTbReport() {
    $('#tbReport').bootstrapValidator('validate');

    var curReport = $("#currTbReport").val();
    if (curReport != null && curReport.length > 0) {
        var rp = $.parseJSON(curReport);
        var tbrp;//声明

        saveRows($("#tbFilterGrid"));//停止编辑状态.保存筛选区域

        saveRows($("#fieldGrid"));//停止编辑状态.保存字段配置

        //已在列表中
        if (rp.ChildReportJson != null && rp.ChildReportJson != "" && rp.ChildReportJson != "[]") {

            //为基础信息赋值
            tbrp = $.parseJSON(rp.ChildReportJson);

            tbrp.ReportStyle = $("#tbReportStyle").val();
            tbrp.IsAutoLoad = $("#isAutoLoad").val();
            //tbrp.IsDebug = $("#isDebug").val();//于2017.7.27取消
            tbrp.IsCheck = $("#isCheck").val();
            tbrp.IsBigdataLoad = $("#isBigdataLoad").val();
            tbrp.IsPaination = $("#isPaination").val();
            tbrp.IsScroll = $("#isScroll").val();
            tbrp.RowNum = $("#rowNum").val();
            tbrp.EmptyRecord = $("#emptyRecord").val();
            tbrp.RowList = $("#rowList").val();
            tbrp.RowStyle = $("#rowStyle").val();
            tbrp.MaxExportCount = $("#maxExpCount").val();
            //tbrp.IsPlaceholder = $("#isPlaceholder").val();
            tbrp.IsOpen = rp.IsOpen;//子报表是否开启

            tbrp.IsRowNumber = $("#isRowNumber").val();//是否显示行头
            tbrp.RownumWidth = $("#rownumWidth").val();//行头的列宽度
            tbrp.MultiboxOnly = $("#multiboxOnly").val();//行选互斥复选框否？
            tbrp.IsMultiSort = $("#isMultiSort").val();//是否组合排序
            tbrp.IsShowHeader = $("#isShowHeader").val();//是否组合排序
            tbrp.IsShowFilter = $("#isShowFilter").val();//是否组合排序
            tbrp.Remark = $("#remark").val();//报表说明
            tbrp.JsFun = $("#jsFun").val();//自定义js代码


            //为字段赋值
            var grid = $("#fieldGrid");
            //获取所有行数据
            var rows = grid.getRowData();
            if (rows != null && rows.length > 0) {
                tbrp.FieldListJson = JSON.stringify(rows);
            }

            //事件数据
            tbrp.OutEventListJson = $("#tbEventJson").val();

            //多表头数据
            tbrp.FieldTopListJson = $("#tbTopFieldJson").val();//多表头赋值
            tbrp.MergeCellJson = $("#tbTopMerge").val();//多表头合并项信息

            //筛选数据
            tbrp.FilterListJson = JSON.stringify(DealFilter($("#tbFilterGrid").getRowData()));

        }
        else {
            tbrp = {
                'ReportStyle': $("#tbReportStyle").val(),
                'IsAutoLoad': $("#isAutoLoad").val(),
                //'IsDebug': $("#isDebug").val(),
                'IsCheck': $("#isCheck").val(),
                'IsBigdataLoad': $("#isBigdataLoad").val(),
                'IsPaination': $("#isPaination").val(),
                'IsScroll': $("#isScroll").val(),
                'RowNum': $("#rowNum").val(),
                'EmptyRecord': $("#emptyRecord").val(),
                'RowList': $("#rowList").val(),
                'RowStyle': $("#rowStyle").val(),
                'MaxExportCount': $("#maxExpCount").val(),
                //'IsPlaceholder': $("#isPlaceholder").val(),
                'IsOpen': rp.IsOpen,

                'IsRowNumber': $("#isRowNumber").val(),//是否显示行头
                'RownumWidth': $("#rownumWidth").val(),//行头的列宽度
                'MultiboxOnly': $("#multiboxOnly").val(),//行选互斥复选框否
                'IsMultiSort': $("#isMultiSort").val(),//是否组合排序
                'IsShowHeader': $("#isShowHeader").val(),//是否显示头部
                'IsShowFilter': $("#isShowFilter").val(),//是否默认显示筛选区
                'Remark': $("#remark").val(),//是否默认显示筛选区
                'JsFun' : $("#jsFun").val(),//自定义js代码


                //字段信息
                'FieldListJson': JSON.stringify($("#fieldGrid").getRowData()),
                //事件数据
                'OutEventListJson': $("#tbEventJson").val(),
                //多表头数据
                'FieldTopListJson': $("#tbTopFieldJson").val(),//多表头赋值
                'MergeCellJson': $("#tbTopMerge").val(),//多表头合并项信息
                //筛选数据
                'FilterListJson': JSON.stringify(DealFilter($("#tbFilterGrid").getRowData()))

            };
        }
        rp.ChildReportJson = JSON.stringify(tbrp);


        //将添加的子报表加入报表列表
        var reports = $("#ChildReportListJson").val();
        //当前已有的子报表
        if (reports != null && reports.length > 0) {
            var reportArr = $.parseJSON(reports);
            //判断是否已存在
            for (var i = 0; i < reportArr.length; i++) {
                var item = reportArr[i];
                if (item.ChildReportType == rp.ChildReportType &&
                    item.ApplicationType == rp.ApplicationType &&
                    item.ChildReportId == rp.ChildReportId) {
                    reportArr[i] = rp;//修改子报表
                    break;
                }
                if(i==reportArr.length-1)
                {
                    reportArr.push(rp);//添加到报表列表
                }
            }
            $("#ChildReportListJson").val(JSON.stringify(reportArr));
        }
        else {
            var reportArr = $.parseJSON("[]");//初始化
            reportArr.push(rp);//添加到报表列表
            $("#ChildReportListJson").val(JSON.stringify(reportArr));
        }

        InitGrid();

        $('#tbReport').modal('hide');//关闭tb的模态框

    }
}

//加载表格报表的字段配置信息jqGrid
function InitTbGrid() {
    var fieldJson = [];//初始化

    var currChildReport = $("#currTbReport").val();
    if (currChildReport != null && currChildReport != "") {
        var rp = $.parseJSON(currChildReport);
        if (rp.ChildReportJson != null && rp.ChildReportJson != "") {
            var tbRp = $.parseJSON(rp.ChildReportJson);
            if (tbRp.FieldListJson != null && tbRp.FieldListJson != "") {

                //#region 合并原表格字段和隐藏域中表格字段，以隐藏域中的字段为主体
                var nowFieldList;
                var oldFields;

                var fields = $("#FieldJson").val();//隐藏的字段信息
                if (fields != null && fields != "" && fields.length > 0) {
                    nowFieldList = $.parseJSON(fields);
                }

                var oldFieldList = $.parseJSON(tbRp.FieldListJson);;

                for (var i = 0; i < nowFieldList.length; i++) {
                    for (var j = 0; j < oldFieldList.length; j++) {
                        if (nowFieldList[i].FieldCode == oldFieldList[j].FieldCode) {
                            nowFieldList[i] = oldFieldList[j];
                        }
                    }
                }
                //#endregion

                fieldJson = nowFieldList;
            }
        }
        else {
            var fields = $("#FieldJson").val();
            if (fields != null && fields != "" && fields.length > 0) {
                fieldJson = $.parseJSON(fields);
            }
        }
    }

    LoadTbGrid(fieldJson);
}

//加载表格化基础数据
function InitTbReportBase() {
    var currChildReport = $("#currTbReport").val();
    if (currChildReport != null && currChildReport != "") {
        var rp = $.parseJSON(currChildReport);
        if (rp.ChildReportJson != null && rp.ChildReportJson != "" && rp.ChildReportJson != []) {
            var tbrp = $.parseJSON(rp.ChildReportJson)

            //初始化基础信息
            $("#tbReportStyle").val(tbrp.ReportStyle);
            $("#isAutoLoad").val(tbrp.IsAutoLoad.toString());

            //$("#isDebug").val(tbrp.IsDebug.toString());
            $("#isCheck").val(tbrp.IsCheck.toString());
            $("#isBigdataLoad").val(tbrp.IsBigdataLoad.toString());
            $("#isPaination").val(tbrp.IsPaination.toString());
            $("#isScroll").val(tbrp.IsScroll.toString());
            $("#rowNum").val(tbrp.RowNum);

            $("#emptyRecord").val(tbrp.EmptyRecord);
            $("#rowList").val(tbrp.RowList);
            $("#rowStyle").val(tbrp.RowStyle);

            $("#maxExpCount").val(tbrp.MaxExportCount);
            //$("#isPlaceholder").val(tbrp.IsPlaceholder);

            $("#isRowNumber").val(tbrp.IsRowNumber.toString());
            $("#rownumWidth").val(tbrp.RownumWidth);

            $("#multiboxOnly").val(tbrp.MultiboxOnly.toString());//行选互斥复选框否？
            $("#isMultiSort").val(tbrp.IsMultiSort.toString());//是否组合排序
            $("#isShowHeader").val(tbrp.IsShowHeader.toString());//是否显示表格头部
            $("#isShowFilter").val(tbrp.IsShowFilter.toString());//是否默认展开筛选区

            var remark = (tbrp.Remark == null || tbrp.Remark == "") ? "" : tbrp.Remark;
            $("#remark").val(remark);//报表说明

            var jsfun = (tbrp.JsFun == null || tbrp.JsFun == "") ? "" : tbrp.JsFun;
            $("#jsFun").val(jsfun);//自定义js代码

            //初始化多表头隐藏控件
            $("#tbTopMerge").val(tbrp.MergeCellJson);
            $("#tbTopFieldJson").val(tbrp.FieldTopListJson);
        }
        else {
            $("#rowNum").val(10);
            $("#emptyRecord").val("已经到底啦！");
            $("#rowList").val("[10,20,30]");
            $("#maxExpCount").val(20000);
            $("#rownumWidth").val(30);

            //加入其他控件的默认值
            $("#tbTopMerge").val("");
            $("#tbTopFieldJson").val("");
        }
    }
}

//显示模态框时的动作
$('#tbReport').on('show.bs.modal', function () {
    // 执行一些动作...
})

//验证表格报表信息
function ValidateTbReport() {
    $('#tbReport').bootstrapValidator({
        message: 'This value is not valid',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            rowNum: {
                message: '分页大小验证失败',
                validators: {
                    notEmpty: {
                        message: '分页大小不能为空'
                    },
                    regexp: {
                        regexp: /^[1-9]\d*$/,
                        message: '只能输入正整数'
                    }
                }
            },
            maxExpCount: {
                validators: {
                    regexp: {
                        regexp: /^[1-9]\d*$/,
                        message: '只能输入正整数'
                    }
                }
            }
        }
    });
}
