//加载chart报表。打开模态框
function InitChartReportModel() {
   // $('#ifm').attr('src', $('#ifm').attr('src'));
    $('#chartReport').modal('show');//打开chart的模态框
    //给上级模态窗的关闭按钮，添加下级模态窗口关闭事件 
    parent.$(".close").click(function () {
        $('#chartReport').modal('hide');
    });
    //show完毕前执行
    $('#chartReport').on('shown', function () {
        //加上下面这句！解决了~
        $(document).off('focusin.modal');
    });
    InitChartReportBase(); //初始化基础信息

    InitFilter();//初始化筛选grid

    //获取查询结果
    GetDataFromBack();
}

//获得报表查询值
function GetDataFromBack()
{
    var data = $.ajax({
        type: "post",
        data: { code: $("#Code").val(), queryParams: "" },
        url: "GetChartData",
        async: false
    });
    if (data != null && data.responseText != null && data.responseText.length > 0) {
        $("#ifm").contents().find("#seachData").val(data.responseText);
        //var dataArr = $.parseJSON(data.responseText);
    }

    $("#ifm").contents().find("#SearchBtn").click();//触发CHART子页面的查询按钮
}

//加载表格化基础数据
function InitChartReportBase() {
    var currChildReport = $("#currChartReport").val();
    if (currChildReport != null && currChildReport != "") {
        var rp = $.parseJSON(currChildReport);
        if (rp.ChildReportJson != null && rp.ChildReportJson != "" && rp.ChildReportJson != []) {
            var tbrp = $.parseJSON(rp.ChildReportJson)

            //初始化基础信息
            $("#chartIsShowFilter").val(tbrp.IsShowFilter.toString());//是否默认展开筛选区
            $("#chartId").val(tbrp.Id);

            LoadSelect("chartType", "~/api/services/api/ChartReport/ChartTypeJson", tbrp.ChartTypeId.toString());
            LoadSelectByOther("chartType", "chartTemp", "~/api/services/api/ChartReport/GetChartTempJsonByType?chartTypeId=" + $("#chartType").val(), tbrp.ChartTempId.toString(), "get");

            var remark = (tbrp.Remark == null || tbrp.Remark == "") ? "" : tbrp.Remark;
            $("#chartRemark").val(remark);//报表说明
            $("#chartEndCode").val((tbrp.EndCode == null || tbrp.EndCode == "") ? "" : tbrp.EndCode);//代码体

            $("#ifm").contents().find("#endCode").val(Encrypt(tbrp.EndCode));
        }
        else {
            $("#chartIsShowFilter").val("true");
            $("#chartRemark").val("");
            $("#chartEndCode").val("");
            LoadSelect("chartType", "~/api/services/api/ChartReport/ChartTypeJson");
            LoadSelectByOther("chartType", "chartTemp", "~/api/services/api/ChartReport/GetChartTempJsonByType?chartTypeId=" + $("#chartType").val(), "", "get");
        }
    }
}

//保存表格式报表信息
function SaveChartReport() {
    //预留一个从子页获取编辑后代码的方法
    //触发子页面的一个按钮，把子页编辑区的内容放在一个隐藏控件
    //之后，在以下代码中实现从子页的隐藏控件取值并赋值给原$("#chartEndCode")控件
    $("#ifm").contents().find("#getEndCode").click();
    var ec = $("#ifm").contents().find("#endCode").val();
    if (ec != null && ec != "" && ec.length > 0)
    {
        $("#chartEndCode").val(Decrypt(ec));
    }

    var curReport = $("#currChartReport").val();
    if (curReport != null && curReport.length > 0) {
        var rp = $.parseJSON(curReport);
        var tbrp;//声明

        saveRows($("#chartFilterGrid"));//停止编辑状态.保存筛选区域

        //已在列表中
        if (rp.ChildReportJson != null && rp.ChildReportJson != "" && rp.ChildReportJson != "[]") {

            //为基础信息赋值
            tbrp = $.parseJSON(rp.ChildReportJson);

            tbrp.IsOpen = rp.IsOpen;//子报表是否开启

            tbrp.IsShowFilter = $("#chartIsShowFilter").val();//是否显示筛选
            tbrp.Remark = $("#chartRemark").val();//报表说明

            //筛选数据
            tbrp.FilterListJson = JSON.stringify(DealFilter($("#chartFilterGrid").getRowData()));
            tbrp.ChartTypeId = $("#chartType").val();//图表种类
            tbrp.ChartTempId = $("#chartTemp").val();//模版
            tbrp.EndCode = $("#chartEndCode").val();//代码体
        }
        else {
            tbrp = {
                'IsOpen': rp.IsOpen,
                'IsShowFilter': $("#chartIsShowFilter").val(),//是否默认显示筛选区
                'Remark': $("#chartRemark").val(),//备注
                'ChartTypeId': $("#chartType").val(),//图表种类
                'ChartTempId': $("#chartTemp").val(),//模版
                'EndCode': $("#chartEndCode").val(),//代码体

                //筛选数据
                'FilterListJson': JSON.stringify(DealFilter($("#chartFilterGrid").getRowData()))

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

        $('#chartReport').modal('hide');//关闭tb的模态框

    }
}

//加载模版
function ChooseTemp() {
    var code = $("#chartEndCode").val();
    if (code != null && code.length > 0) {
        abp.message.confirm(
        '将覆盖原有代码', //确认提示
        '确认继续覆盖吗?', //确认提示（可选参数）
        function (isConfirmed) {
            if (isConfirmed) {
                coverCode();
            }
        });
    }
    else
        coverCode();
}
//覆盖代码体
function coverCode()
{
    var data = $.ajax({
        type: "GET",
        url: GetPath("~/api/services/api/ChartReport/GetChartTemp?id=" + $("#chartTemp").val()),
        async: false
    });
    if (data != null && data.responseJSON.result != null) {
        $("#ifm").contents().find("#endCode").val(Encrypt(data.responseJSON.result.tempCode));
        $("#chartEndCode").val(data.responseJSON.result.tempCode);
        $("#ifm").contents().find("#SearchBtn").click();//触发CHART子页面的查询按钮
    }
}

//加载下拉控件
function LoadSelect( controlName,url, defaultVal, subWay) {
    var options = "";//初始化下拉项
    //get
    if (subWay != undefined && subWay.toLocaleLowerCase() == "get") {

        //$.get(GetPath(url), function (data, status) {
        //    options = GetOptions(data, defaultVal);
        //});

        var data = $.ajax({
            type: "GET",
            url: GetPath(url),
            async: false
        });

        options = GetOptions(data, defaultVal);
    }
    else//post
    {
        var dataJson = GetParamJsonByUrl(url);//获取链接中的参数{ key:value , ...}
        dataJson = (dataJson == null ? {} : dataJson);
        //$.post(GetPath(url).split("?")[0], dataJson,
        //      function (data, status) {
        //          options = GetOptions(data, defaultVal);
        //      });

        var data = $.ajax({
            type: "post",
            data: dataJson,
            url: GetPath(url).split("?")[0],
            async: false
        });
        options = GetOptions(data, defaultVal);
    }
    if (options != "")
    {
        var $contorl = $("#" + controlName);//$
        $contorl.html(options);
    }
}

function LoadSelectByOther(relyControl,nowControl,url, defaultVal, subWay)
{
    var $rely = $("#" + relyControl);
    var $now = $("#" + nowControl);

    LoadSelect(nowControl, url, defaultVal, subWay);

    //$other.change(function () {
    //    LoadSelect(nowControl, url.replace("{rely_contr}", $rely.val()), defaultVal, subWay);
    //});
}

//获取下拉项s
function GetOptions(data, defaultVal)
{
    var options = "";
    if (data != null && data.responseJSON.result!=null) {
        var arr = $.parseJSON(data.responseJSON.result);
        if (arr != null && arr.length > 0) {
            for (var i = 0; i < arr.length; i++) {
                var item = arr[i];
                //默认值等于某下拉项设置被选中
                if (defaultVal != undefined && defaultVal != null && $.trim(defaultVal) != "" && defaultVal == item.Id) {
                    options += '<option selected="selected" value="' + item.Id + '">' + item.Name + '</option>';

                }
                else {
                    options += '<option value="' + item.Id + '">' + item.Name + '</option>';
                }
            }
        }
    }
    return options;
}
