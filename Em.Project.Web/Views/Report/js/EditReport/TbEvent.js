//#region 事件管理代码块

//表格报表-事件管理
//*不验证事件的唯一性*
//*只确保事件新增或修改*
function SaveTbEvent() {
    //声明+获取标识符
    var ident = $("#EventIdentifier").val();

    //新增
    if (ident == null || ident == "") {
        ident = generateUUID();//设置一个唯一数值
    }

    //对参数列表Grid关闭编辑
    saveRows($("#eventParamGrid"));

    if ($("#eventUrl").val() == null || $("#eventUrl").val()==""||
        $("#eventDisplayName").val() == null || $("#eventDisplayName").val() == "")
    {
        abp.message.error("事件名称或事件URL均不能为空！", "事件添加失败");
        return;
    }

    //获取当前事件信息，并拼凑为对象
    var currEvent = {
        'Id':0,//默认id
        'EventType': $("#eventType").val(),//事件类型
        'FieldCode': $("#eventField").val(),//依附字段
        'DisplayWay': $("#eventDisplayWay").val(),//显示形式
        'DisplayName': $("#eventDisplayName").val(),
        'Icon': $("#eventIcon").val(),
        'Style': $("#eventStyle").val(),
        'Title': $("#eventTitle").val(),
        'OpenWay': $("#eventOpenWay").val(),
        'Url': $("#eventUrl").val(),
        'Height': $("#eventHeight").val(),
        'Width': $("#eventWidth").val(),
        'OrderNum': $("#eventOrderNum").val() == "" ? 0 : $("#eventOrderNum").val(),//排序号
        'Identifier': ident,//赋值标识符值
        'ParamListJson': JSON.stringify($("#eventParamGrid").getRowData()) //参数列表
    };

    var eventArr = $.parseJSON("[]");//初始化事件列表

    var events = $("#tbEventJson").val();
    if (events != null && events != "" && events != "[]") {
        eventArr = $.parseJSON(events);

        var isHave = -1;//初始化-不存在
        for (var i = 0; i < eventArr.length; i++) {
            var event = eventArr[i];
            if (event.Identifier == ident) {
                currEvent.Id = event.Id;//保留传入的id值
                eventArr[i] = currEvent;
                isHave = i;
                break;
            }
        }
        //未找到已添加，则新增
        if (isHave == -1) {
            eventArr.push(currEvent);//添加到事件列表
        }
    }
    else {
        eventArr.push(currEvent);//添加到事件列表
    }

    //更新当前事件列表至显示表格
    InitTbRpEventGrid(eventArr);

    //给事件隐藏控件赋值
    $("#tbEventJson").val(JSON.stringify(eventArr));

    abp.message.success("", "事件保存成功！");

    //初始化事件编辑区域
    InitEventBase();
}

//重置
function ResetTbEvent() {
    InitEventBase();
}

//初始化事件编辑区域
function InitEventBase() {
    //初始化事件编辑区域
    $("#eventType").val(0);
    $("#eventField").val("");

    $("#eventDisplayWay").val(1);
    $("#eventDisplayName").val("");
    $("#eventIcon").val("");
    $("#eventStyle").val("");
    $("#eventTitle").val("");
    $("#eventOrderNum").val("0");

    $("#eventOpenWay").val(1);
    $("#eventUrl").val("");
    $("#eventHeight").val(600);
    $("#eventWidth").val(800);

    $("#EventIdentifier").val("");//事件标识赋值空

    //#region 初始化参数编辑区
    InitTbRpParamGrid([]);
    //#endregion
}

//加载事件列表
function InitTbReportEvent() {
    var tbEventArr = [];//初始化事件列表

    var currChildReport = $("#currTbReport").val();
    if (currChildReport != null && currChildReport != "" && currChildReport != "[]") {
        var rp = $.parseJSON(currChildReport);
        if (rp.ChildReportJson != null && rp.ChildReportJson != "") {
            var tbRp = $.parseJSON(rp.ChildReportJson);
            if (tbRp.OutEventListJson != null && tbRp.OutEventListJson != "") {
                $("#tbEventJson").val(tbRp.OutEventListJson);//给隐藏控件赋值
                tbEventArr = $.parseJSON(tbRp.OutEventListJson);
            }
        }
        else {
            $("#tbEventJson").val("[]");//给隐藏控件赋初始值
        }
    }

    //#region 初始化事件表格
    InitTbRpEventGrid(tbEventArr);
    //#endregion


    //初始化事件编辑区域
    InitEventBase();
}

//加载表格事件列表
function InitTbRpEventGrid(tbEventArr) {
    $.jgrid.gridUnload("eventGrid");//先卸载

    //再加载
    $("#eventGrid").jqGrid({
        data: tbEventArr,
        styleUI: 'Bootstrap',
        datatype: "local",
        colModel: [
             {
                 label: "操作区",
                 name: "actions",
                 width: 100,
                 formatter: EventActFmatter
             },
            { label: '编号', name: 'Id', hidden: true, width: 0 },
            { label: '标识符', name: 'Identifier', hidden: true, width: 0 },
            //{ label: '事件类型', name: 'EventType', width: 80 },
            {
                label: '事件类型', name: 'EventType', width: 80,
                formatter: function (cellvalue, options, rowObject) {
                    var res = "";
                    switch (cellvalue.toString()) {
                        case "1":
                            res = "行事件";
                            break;
                        case "2":
                            res = "全局事件";
                            break;
                        case "3":
                            res = "内容事件";
                            break;
                        case "4":
                            res = "列事件";
                            break;
                        case "5":
                            res = "多表头事件";
                            break;
                        case "6":
                            res = "内容格式化";
                            break;
                    }
                    return res;
                }
            },
            { label: '依附字段', name: 'FieldCode', width: 80 },
            //{ label: '显示方式', name: 'DisplayWay', width: 80 },
            {
                label: '显示方式', name: 'DisplayWay', width: 80,
                formatter: function (cellvalue, options, rowObject) {
                    var res = "";
                    switch (cellvalue.toString()) {
                        case "1":
                            res = "超链接";
                            break;
                        case "2":
                            res = "按钮";
                            break;
                    }
                    return res;
                }
            },
            { label: '事件名称', name: 'DisplayName', width: 80 },
            { label: 'URL', name: 'Url', width: 120 },
            //{ label: '打开方式', name: 'OpenWay', width: 80 },
            {
                label: '打开方式', name: 'OpenWay', width: 80,
                formatter: function (cellvalue, options, rowObject) {
                    var res = "";
                    switch (cellvalue.toString()) {
                        case "1":
                            res = "弹出框";
                            break;
                        case "2":
                            res = "顶级弹出框";
                            break;
                        case "3":
                            res = "当页跳转";
                            break;
                        case "4":
                            res = "新开网页";
                            break;
                        case "5":
                            res = "新开Tab页";
                            break;
                        case "7":
                            res = "弹子页";
                            break;
                        case "6":
                            res = "ajax执行Post";
                            break;
                        case "8":
                            res = "ajax执行Get";
                            break;
                        case "9":
                            res = "ajax执行Post(含确认提示)";
                            break;
                        case "10":
                            res = "调用js方法";
                            break;
                    }
                    return res;
                }
            },
            { label: '窗体高', name: 'Height', width: 70 },
            { label: '窗体宽', name: 'Width', width: 70 },
        ],
        viewrecords: true,
        loadonce: false,
        width: ($(".modal-body").width() - 20),
        //height: 250,
        rowNum: 20,//默认分页大小-在框架动态赋值
        rowList: [20, 30, 50],//传入分页大小的下拉-在框架动态复制
        //rownumbers: true,//显示行号
        //rownumWidth: 25,//行号列的宽度
        pager: "#eventGridPager",
        caption: "<i class='fa fa-hand-pointer-o' ></i> <b>事件列表</b>"//设置和显示表格标题
    });
}
//格式化事件操作区-EventActFmatter
function EventActFmatter(cellvalue, options, rowObject) {

    var d = '<a href="javascript:void(0)" onclick="EditTbRpEvent(\'' + rowObject.Identifier + '\')">编辑</a> ';
    d += '<a href="javascript:void(0)" onclick="DeleteTbRpEvent(\'' + rowObject.Identifier + '\')">删除</a> ';
    return d;
};

//加载表格事件的参数列表
function InitTbRpParamGrid(paramArr) {
    $.jgrid.gridUnload("eventParamGrid");//先卸载

    $("#eventParamGrid").jqGrid({
        data: paramArr,
        editurl: 'clientArray',
        styleUI: 'Bootstrap',
        datatype: "local",

        //url: 'data.json',
        //editurl: 'clientArray',
        //datatype: "json",

        colModel: [
            {
                label: '编号',
                name: 'Id',
                width: 75,
                hidden: true,
                formatter: function (cellvalue, options, rowObject) {
                    if (!isNaN(rowObject.Id)) {
                        return rowObject.Id
                    }
                    else {
                        return 0;
                    }
                }
            },
            {
                label: '表格ID',
                name: 'TbReportOutEventId',
                width: 75,
                hidden: true,
                formatter: function (cellvalue, options, rowObject) {
                    if (!isNaN(rowObject.TbReportOutEventId)) {
                        return rowObject.TbReportOutEventId
                    }
                    else {
                        return 0;
                    }
                }
            },
            {
                label: '参数名',
                name: 'Name',
                width: 75,
                key: true,//设置主键
                editable: true
            },
            {
                label: '是否字段',
                name: 'IsField',
                width: 80,
                editable: true,
                edittype: "select",
                editoptions: {
                    value: "true:true;false:false"
                }
            },
            {
                label: '字段编码',
                name: 'FieldCode',
                width: 100,
                editable: true
            },
            {
                label: '参数值',
                name: 'PValue',
                width: 80,
                editable: true
            },
            {
                label: '备注',
                name: 'Remark',
                width: 150,
                editable: true
            },
            {
                label: '排序号',
                name: 'OrderNum',
                width: 60,
                editable: true
            }
        ],
        //sortname: 'EmployeeID',
        loadonce: true,
        width: ($(".modal-body").width()-20),
        height: 120,
        rowNum: 150,
        pager: "#eventParamGridPager"
    });

    $('#eventParamGrid').navGrid("#eventParamGridPager", { edit: false, add: false, del: false, refresh: false, view: false });
    //$('#eventParamGrid').inlineNav('#eventParamGridPager',
    //    // the buttons to appear on the toolbar of the grid
    //    {
    //        edit: true,
    //        add: true,
    //        del: true,
    //        cancel: true,
    //        editParams: {
    //            keys: true,
    //        },
    //        addParams: {
    //            keys: true
    //        }
    //    });

    //添加一个‘新增’按钮
    $('#eventParamGrid').navButtonAdd('#eventParamGridPager',
         {
             buttonicon: "glyphicon glyphicon-plus",
             title: "添加",
             caption: "添加",
             position: "last",
             onClickButton: addEventRow
         });
    //添加一个‘删除’按钮
    $('#eventParamGrid').navButtonAdd('#eventParamGridPager',
         {
             buttonicon: "glyphicon glyphicon-trash",
             title: "删除",
             caption: "删除",
             position: "last",
             onClickButton: delEventRow
         });

    //加载完毕后,打开所有行的编辑
    startEdit($('#eventParamGrid'));
}
//删除行
function delEventRow() {
    var $grid =$('#eventParamGrid');//得到当前筛选控件
    var rowKey = $grid.getGridParam("selrow");
    if (rowKey) {
        //解决最后一行删除的问题
        if (rowKey == 1) {
            ids = $grid.jqGrid('getDataIDs');
            $grid.delGridRow(ids[ids.length - 1]);//删除行
            startEdit($grid);//删除后启动全部编辑
        }
        else {
            $grid.delGridRow(rowKey);//删除行
            startEdit($grid);//删除后启动全部编辑
        }
    }
    else {
        alert("请选择行");
    }
}

function addEventRow()
{
    var $gridCase = $('#eventParamGrid');//得到当前参数grid

    saveRows($gridCase);//先保存当前修改

    // 选中行rowid
    var rowId = $gridCase.jqGrid('getGridParam', 'selrow');
    // 选中行实际表示的位置
    var ind = $gridCase.getInd(rowId);
    // 新插入行的位置
    var newInd = ind + 1;

    var ft = {
        "Id": 0,
        "TbReportOutEventId": 0,
        "Name": "",
        "IsField": true,
        "FieldCode": "",
        "PValue":"",//获取一个唯一值
        "Remark": ""
    };

    $gridCase.addRowData(rowId + 1, ft, newInd);

    startEdit($gridCase);
}

//编辑表格事件
function EditTbRpEvent(Identifier) {
    var events = $("#tbEventJson").val();
    if (events != null && events != "" && events != "[]") {
        var eventArr = $.parseJSON(events);
        for (var i = 0; i < eventArr.length; i++) {
            var ev = eventArr[i];
            if (Identifier == ev.Identifier) {
                //基础配置信息赋值
                $("#eventType").val(ev.EventType);
                $("#eventField").val(ev.FieldCode);

                $("#eventDisplayWay").val(ev.DisplayWay);
                $("#eventDisplayName").val(ev.DisplayName);
                $("#eventIcon").val(ev.Icon);
                $("#eventIcon").keyup();//选中一下图标控件
                $("#eventStyle").val(ev.Style);
                //加载选择按钮样式
                $("#dropdownStyleBut").removeClass();
                $("#dropdownStyleBut").addClass("btn dropdown-toggle " + (ev.Style == "" ? "btn-default" : ev.Style));
                //End加载选择按钮样式
                $("#eventTitle").val(ev.Title);
                $("#eventOrderNum").val(ev.OrderNum);

                $("#eventOpenWay").val(ev.OpenWay);
                $("#eventUrl").val(ev.Url);
                $("#eventHeight").val(ev.Height);
                $("#eventWidth").val(ev.Width);

                $("#EventIdentifier").val(ev.Identifier);//隐藏标识符赋值

                //参数列表赋值
                var params = ev.ParamListJson;

                if (params != null && params != "" && params != "[]") {
                    var paramArr = $.parseJSON(params);
                    InitTbRpParamGrid(paramArr);//加载参数列表
                }
                else {
                    InitTbRpParamGrid([]);//初始化-加载参数列表
                }
            }
        }
    }
}
//删除一条事件
function DeleteTbRpEvent(Identifier) {
    var events = $("#tbEventJson").val();
    if (events != null && events != "" && events != "[]") {
        var eventArr = $.parseJSON(events);
        for (var i = 0; i < eventArr.length; i++) {
            var ev = eventArr[i];
            if (Identifier == ev.Identifier) {
                eventArr.splice(i, 1);//删除
            }
        }
        $("#tbEventJson").val(JSON.stringify(eventArr));
        //重新加载grid
        InitTbRpEventGrid(eventArr);
    }
}

//#endregion

//选择图标事件
function ContentIconType(value) {
    $("#divTypeIcon").hide();
    $("#icon_type_img").attr("class", value);
    $("#eventIcon").val(value);
}

$(function () {
    ///选择按钮样式
    $(".dropdown-menu li button").click(function () {
        var strClassVal = $(this).attr("class");
        if (strClassVal == "noneStyleBut")
            strClassVal = "";
        $("#eventStyle").val(strClassVal);
        $("#dropdownStyleBut").removeClass();
        $("#dropdownStyleBut").addClass("btn dropdown-toggle " + (strClassVal == "" ? "btn-default" : strClassVal));
    });
});