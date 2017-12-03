//加载rdlc报表。打开模态框
function InitRdlcReportModel() {
    $('#rdlcReport').modal('show');//打开rdlc的模态框
    //给上级模态窗的关闭按钮，添加下级模态窗口关闭事件 
    parent.$(".close").click(function () {
        $('#rdlcReport').modal('hide');
    });
    //show完毕前执行
    $('#rdlcReport').on('shown', function () {
        //加上下面这句！解决了~
        $(document).off('focusin.modal');
    });

    InitRdlcReportBase(); //初始化基础信息

    InitFilter();//初始化筛选grid
}

//加载表格化基础数据
function InitRdlcReportBase() {
    var currChildReport = $("#currRdlcReport").val();
    if (currChildReport != null && currChildReport != "") {
        var rp = $.parseJSON(currChildReport);
        if (rp.ChildReportJson != null && rp.ChildReportJson != "" && rp.ChildReportJson != []) {
            var tbrp = $.parseJSON(rp.ChildReportJson)

            //初始化基础信息
            $("#rdlcRowNum").val(tbrp.RowNum);
            $("#rdlcIsShowFilter").val(tbrp.IsShowFilter.toString());//是否默认展开筛选区
            $("#rdlcXml").val(tbrp.RdlcXml);//xml
            $("#rdlcId").val(tbrp.Id);

            var remark = (tbrp.Remark == null || tbrp.Remark == "") ? "" : tbrp.Remark;
            $("#rdlcRemark").val(remark);//报表说明
        }
        else {
            $("#rdlcRowNum").val(20);
        }
    }
}

//保存表格式报表信息
function SaveRdlcReport() {

    var curReport = $("#currRdlcReport").val();
    if (curReport != null && curReport.length > 0) {
        var rp = $.parseJSON(curReport);
        var tbrp;//声明

        saveRows($("#rdlcFilterGrid"));//停止编辑状态.保存筛选区域

        //已在列表中
        if (rp.ChildReportJson != null && rp.ChildReportJson != "" && rp.ChildReportJson != "[]") {

            //为基础信息赋值
            tbrp = $.parseJSON(rp.ChildReportJson);

            tbrp.IsOpen = rp.IsOpen;//子报表是否开启

            tbrp.RowNum = $("#rdlcRowNum").val();
            tbrp.IsShowFilter = $("#rdlcIsShowFilter").val();//是否显示筛选
            tbrp.Remark = $("#rdlcRemark").val();//报表说明
            tbrp.RdlcXml = $("#rdlcXml").val();//xml

            //筛选数据
            tbrp.FilterListJson = JSON.stringify(DealFilter($("#rdlcFilterGrid").getRowData()));

        }
        else {
            tbrp = {
                'IsOpen': rp.IsOpen,
                'RowNum': $("#rdlcRowNum").val(),
                'IsShowFilter': $("#rdlcIsShowFilter").val(),//是否默认显示筛选区
                'Remark': $("#rdlcRemark").val(),//备注
                'RdlcXml': $("#rdlcXml").val(),//xml

                //筛选数据
                'FilterListJson': JSON.stringify(DealFilter($("#rdlcFilterGrid").getRowData()))

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

        $('#rdlcReport').modal('hide');//关闭tb的模态框

    }
}
//下载rdlc模版
function DownRdlcModule()
{
    //window.location = "DownRDLC?fields=" + $("#FieldJson").val() + "&reportName=" + $("#Name").val();

    construtForm("DownRDLC", { fields: $("#FieldJson").val(), reportName: $("#Name").val() });
}
//下载已配置xml
function DownNowRdlc()
{
    construtForm("DownNowRDLC", { rdlcId: $("#rdlcId").val(), reportName: $("#Name").val() });
}

/** 
 * 构建form表单，以post方式提交 
 * @param actionUrl  提交路径 
 * @param parms      提交参数 
 * @returns {___form0} 
 */
function construtForm(actionUrl, parms) {
    var form = document.createElement("form");
    form.style.display = 'none';;
    form.action = actionUrl;
    form.method = "post";
    document.body.appendChild(form);


    for (var key in parms) {
        var input = document.createElement("input");
        input.type = "hidden";
        input.name = key;
        input.value = parms[key];
        form.appendChild(input);
        // console.log(key);  
        // console.log(parms[key]);  
    }
     form.submit();  

    //return form;

}