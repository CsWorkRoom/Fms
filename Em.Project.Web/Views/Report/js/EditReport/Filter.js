/*改造筛选功能为表格和rdlc报表共有.
通过传入的参数变量来区分对tb或rdlc的筛选处理
*/

//保存前处理筛选数据为系统存储需要
function DealFilter(filterArr) {
    var regularArr = [];
    var regulars = $("#RegularVals").val();//获取隐藏正则式
    if (regulars != null && regulars.length > 0) {
        regularArr = $.parseJSON(regulars);
    }

    if (filterArr != null && filterArr.length > 0) {
        for (var i = 0; i < filterArr.length; i++) {
            //IsSearch的处理
            switch (filterArr[i].IsSearch) {
                case "否":
                    filterArr[i].IsSearch = false;
                    break;
                case "是":
                    filterArr[i].IsSearch = true;
                    break;
                default:
                    filterArr[i].IsSearch = false;
                    break;
            }
            //DataType字段类型处理
            switch (filterArr[i].DataType) {
                //"Decimal:数值型;String:字符串;Int32:整型;Int64:长整型;Int16:短整型;DateTime:时间"
                case "数值型":
                    filterArr[i].DataType = "Decimal";
                    break;
                case "字符串":
                    filterArr[i].DataType = "String";
                    break;
                case "整型":
                    filterArr[i].DataType = "Int32";
                    break;
                case "长整型":
                    filterArr[i].DataType = "Int64";
                    break;
                case "短整型":
                    filterArr[i].DataType = "Int16";
                    break;
                case "时间":
                    filterArr[i].DataType = "DateTime";
                    break;
                default:
                    filterArr[i].DataType = "String";
                    break;
            }
            //FilterType筛选类型的处理
            //"1:文本框;2:复选下拉框;3:单选下拉框;4:日期/年月日;5:日期/年月"
            switch (filterArr[i].FilterType) {
                case "文本框":
                    filterArr[i].FilterType = "1";
                    break;
                case "复选下拉框":
                    filterArr[i].FilterType = "2";
                    break;
                case "单选下拉框":
                    filterArr[i].FilterType = "3";
                    break;
                case "日期/年月日":
                    filterArr[i].FilterType = "4";
                    break;
                case "日期/年月":
                    filterArr[i].FilterType = "5";
                    break;
                default://默认为文本框
                    filterArr[i].FilterType = "1";
                    break;
            }
            //IsQuick是否快捷筛选的处理
            switch (filterArr[i].IsQuick) {
                case "否":
                    filterArr[i].IsQuick = false;
                    break;
                case "是":
                    filterArr[i].IsQuick = true;
                    break;
                default:
                    filterArr[i].IsQuick = false;
                    break;
            }
            //RegularId正则的处理
            if (regularArr != null && regularArr.length > 0) {
                for (var j = 0; j < regularArr.length; j++) {
                    if (regularArr[j] != "" && filterArr[i].RegularId == regularArr[j].text) {
                        filterArr[i].RegularId = regularArr[j].value;
                        break;
                    }
                    if (j == regularArr.length - 1) {
                        filterArr[i].RegularId = null;
                    }
                }
            }
            if (filterArr[i].RegularId == " " || $.trim(filterArr[i].RegularId) == "") {
                filterArr[i].RegularId = null;
            }
        }
    }
    return filterArr;
}

//初始化筛选 -该方法会被刷新按钮调用。rpType：1=表格，2=键值，3=图形，4=rdlc
function InitFilter() {
    var controls = GetFilterControlNames();//获取当前相关控件
    //初始化相关控件变量
    var rpType = controls.rpType;
    var fieldGrid = controls.fieldGrid;
    var filterGrid = controls.filterGrid;
    var filterGridPager = controls.filterGridPager;
    var $currChildReport = $("#" + controls.currChildReport);

    //与最新的字段信息合并后再加载
    //用户可能重新解析了sql，新增或删除有字段，在jqgrid表格字段基础获取再组装。
    //最新字段
    var $filterGrid = $("#" + filterGrid);

    var fieldArr=null;//初始化字段信息
    switch (rpType){
        case "1":
        case "2":
            var $fieldGrid = $("#" + fieldGrid);
            saveRows($fieldGrid);
            fieldArr = $fieldGrid.getRowData();
            break;
        case "3":
            break;
        case "4":
            var fields = $("#FieldJson").val();
            if(fields!=null&&fields!=""&&fields!="[]")
            {
                fieldArr = $.parseJSON(fields);
            }
            break;
        default:
            break;
    }

    var filterArr = null;//初始化筛选字段信息

    //#region 点击刷新时处理
    //if (tag == 1) {
    //    saveRows($filterGrid);//在页面端保存编辑内容
    //    filterArr = $filterGrid.getRowData();
    //}
    //else {
        var currChildReport = $currChildReport.val();
        if (currChildReport != null && currChildReport != "") {
            var rp = $.parseJSON(currChildReport);
            if (rp.ChildReportJson != null && rp.ChildReportJson != "") {
                var tbRp = $.parseJSON(rp.ChildReportJson);
                if (tbRp.FilterListJson != null && tbRp.FilterListJson != "") {

                    filterArr = $.parseJSON(tbRp.FilterListJson);//赋值筛选
                }
            }
        //}
    }
    //#endregion

    //声明新筛选集合newFilterArr
    var newFilterArr = [];

    //#region 组装最新的筛选信息 newFilterArr
    if (fieldArr != null && fieldArr.length > 0) {
        //以新字段为基准进行比较
        //fieldArr = fieldArr.sort(compare("OrderNum"));//根据字段序号排序-升序

        for (var i = 0; i < fieldArr.length; i++) {
            var fd = fieldArr[i];

            if (filterArr != null && filterArr.length > 0) {
                for (var j = 0; j < filterArr.length; j++) {
                    var ft = filterArr[j];
                    //找到筛选时
                    if (fd.FieldCode == ft.FieldCode) {
                        var curFt = cloneObj(ft);
                        //替换可能已被修改的信息
                        if (rpType == 1 || rpType == 2)
                            curFt.FieldName = fd.FieldName;
                        curFt.DataType = fd.DataType;
                        curFt.IsCustom = false;
                        //curFt.OrderNum = fd.OrderNum;//存在争议,确定以哪种模式的顺序为准
                        curFt.OrderNum = curFt.OrderNum == null ? 0 : curFt.OrderNum;
                        curFt.DefaultValue = ft.DefaultValue == null ? "" : ft.DefaultValue;
                        newFilterArr.push(curFt);
                        break;
                    }
                    if (j == filterArr.length - 1) {
                        AddFilter(newFilterArr, fd);
                    }
                }
            }
            else//新增时的初始化
            {
                AddFilter(newFilterArr, fd);
            }
        }
    }
    //加入自定义筛选
    if (filterArr != null && filterArr.length > 0) {
        for (var j = 0; j < filterArr.length; j++) {
            if (filterArr[j].IsCustom.toString() == "true") {
                var ft = filterArr[j];
                ft.OrderNum = ft.OrderNum == null ? 0 : ft.OrderNum;
                ft.DefaultValue = ft.DefaultValue == null ? "" : ft.DefaultValue;
                newFilterArr.push(ft);
            }
        }
    }
    //#endregion

    if (newFilterArr != null && newFilterArr.length > 0) {
        //newFilterArr = newFilterArr.sort(compare("OrderNum"));//根据字段序号排序-升序 --暂时取消排序
        LoadFilterGrid(newFilterArr, filterGrid, filterGridPager);//加载筛选的grid
    }

}

function AddFilter(newFilterArr, fd) {
    var ft = {
        "Id": 0,
        "TbReportId": 0,
        "RdlcReportId": 0,
        "FieldCode": fd.FieldCode,
        "FieldName": fd.FieldName,
        "FieldParam": fd.FieldCode,
        "DataType": fd.DataType,
        "DefaultValue": null,
        "OrderNum": fd.OrderNum == null ? 0 : fd.OrderNum,
        "IsQuick": false,
        "FilterSql": "",
        "FilterType": "1",//文本框
        "RegularId": null,
        "IsCustom": false
    };
    newFilterArr.push(ft);
}

//根据传入的筛选信息加载grid
function LoadFilterGrid(newFilterArr,  filterGrid, filterGridPager) {
    //$.jgrid.gridUnload(filterGrid);//先卸载
    $.jgrid.gridUnload("tbFilterGrid");//先卸载
    $.jgrid.gridUnload("rdlcFilterGrid");//先卸载

    //var $container = $("#" + filterGrid.split("Grid")[0]);

    ////判断div是否存在，若存在则删除
    //if ($container.length > 0) {
    //    //$container.empty();
    //    $container.children().remove()
    //}
    ////添加fiter标签
    //$container.append(" <table id=\"" + filterGrid + "\"></table><div id=\"" + filterGridPager + "\"></div>");
    

    var $filterGrid = $("#" + filterGrid);
    var gwidth = $filterGrid.parents(".modal-body").width();

    $filterGrid.jqGrid({
        altRows: true,//隔行换色
        data: newFilterArr,
        editurl: 'clientArray',
        styleUI: 'Bootstrap',
        //responsive: true,
        datatype: "local",
        page: 1,
        colModel: [
            { label: 'ID', name: 'Id', width: 20, hidden: true },//id值隐藏
            { label: 'TbReportId', name: 'TbReportId', width: 20, hidden: true },//TbReportId值隐藏
            { label: 'RdlcReportId', name: 'RdlcReportId', width: 20, hidden: true },//RdlcReportId值隐藏
            { label: '字段编码', name: 'FieldCode', width: 150, editable: false },
             {//参数名作为主键
                 label: '参数名',
                 name: 'FieldParam',
                 width: 150,
                 key: true,
                 editable: true,
                 edittype: "text"
                 //editrules: { required: true }
             },
              {
                  label: '显示名称',
                  name: 'FieldName',
                  width: 150,
                  editable: true,
                  edittype: "text"
                  //editrules: { required: true }
              },
              {
                  label: '是否筛选',
                  name: 'IsSearch',
                  width: 80,
                  editable: true,
                  edittype: "select",
                  editoptions: {
                      value: "false:否;true:是"
                  }//默认为否
              },
              {
                  label: '字段类型',
                  name: 'DataType',
                  width: 90,
                  editable: true,
                  edittype: "select",
                  editoptions: {
                      value: "Decimal:数值型;String:字符串;Int32:整型;Int64:长整型;Int16:短整型;DateTime:时间"
                  }
              },

               {
                   label: '正则表达式',
                   name: 'RegularId',
                   width: 120,
                   editable: true,
                   edittype: "select",
                   editoptions: {
                       value: GetRegulars
                   }
               },
                {
                    label: '默认值',
                    name: 'DefaultValue',
                    width: 80,
                    editable: true,
                    edittype: "text"
                },
                 {
                     label: '筛选类型',
                     name: 'FilterType',
                     width: 140,
                     editable: true,
                     edittype: "select",
                     editoptions: {
                         value: "1:文本框;2:复选下拉框;3:单选下拉框;4:日期/年月日;5:日期/年月"
                     }
                 },
                  {
                      label: '下拉筛选sql',
                      name: 'FilterSql',
                      width: 130,
                      editable: true,
                      edittype: "textarea"
                  },
                  {
                      label: '筛选排序',
                      name: 'OrderNum',
                      width: 80,
                      editable: true,
                      edittype: "text"
                  },
                  {
                      label: '快捷筛选',
                      name: 'IsQuick',
                      width: 80,
                      editable: true,
                      edittype: "select",
                      editoptions: {
                          value: "false:否;true:是"
                      }//默认非快捷筛选
                  },
                  {
                      label: '内置字段？',
                      name: 'IsCustom',
                      width: 100,
                      editable: false
                  },
                  {
                      label: '筛选说明',
                      name: 'Remark',
                      width: 120,
                      editable: true,
                      edittype: "textarea",
                      //edittype: "text",
                      hidden: true,//隐藏字段
                      editrules: { edithidden: true }//让隐藏字段可编辑,编辑时显示
                  }
        ],
        loadonce: false,
        viewrecords: true,
        shrinkToFit: false,
        autoScroll: false,
        height: window.innerHeight * 0.6,
        width: gwidth,
        // autowidth: true,
        rowNum: newFilterArr.length + 3,//默认比原行数+3
        pager: "#" + filterGridPager
    });

    $filterGrid.navGrid('#' + filterGridPager,
               { edit: false, add: false, del: false, search: false, refresh: false, view: false, position: "left", cloneToTop: false });

    // 添加一个‘添加’按钮
    $filterGrid.navButtonAdd('#'+filterGridPager,
        {
            buttonicon: "glyphicon glyphicon-plus",
            title: "添加",
            caption: "添加",
            position: "last",
            onClickButton: addRow
        });

    //添加一个‘删除’按钮
    $filterGrid.navButtonAdd('#' + filterGridPager,
         {
             buttonicon: "glyphicon glyphicon-trash",
             title: "删除",
             caption: "删除",
             position: "last",
             onClickButton: delRow
         });

    //添加一个‘刷新’按钮
    $filterGrid.navButtonAdd('#' + filterGridPager,
         {
             buttonicon: "glyphicon glyphicon-refresh",
             title: "刷新",
             caption: "刷新",
             position: "last",
             onClickButton: refreshFiterGrid
         });

    //加载完毕后,打开所有行的编辑
    startEdit($filterGrid);
}

//添加一行
function addRow() {

    var $gridCase= GetFilterGrid();//得到当前筛选控件
    
    saveRows($gridCase);//先保存当前修改

    // 选中行rowid
    var rowId = $gridCase.jqGrid('getGridParam', 'selrow');
    // 选中行实际表示的位置
    var ind = $gridCase.getInd(rowId);
    // 新插入行的位置
    var newInd = ind + 1;

    var ft = {
        "Id": 0,
        "TbReportId": 0,
        "RdlcReportId": 0,
        "FieldCode": "",
        "FieldName": "",
        "FieldParam": new Date().getTime(),//获取一个唯一值
        "DataType": "String",
        "DefaultValue": null,
        "OrderNum": 100,
        "IsQuick": false,//默认均不是快捷查询
        "FilterSql": "",
        "FilterType": "1",//文本框
        "RegularId": null,
        "IsSearch": true,//筛选
        "IsCustom": true//手工添加为自定义
    };

    $gridCase.addRowData(rowId + 1, ft, newInd);

    startEdit($gridCase);

    //var grid = $("#jqGrid");
    //grid.editGridRow("new", { closeAfterAdd: true });
}

function delRow(filterGrid) {
    var $grid = GetFilterGrid();//得到当前筛选控件
    var rowKey = $grid.getGridParam("selrow");
    if (rowKey) {
        //解决最后一行删除的问题
        if (rowKey == 1) {
            saveRows($grid);//删除前保存信息
            ids = $grid.jqGrid('getDataIDs');
            $grid.delGridRow(ids[ids.length - 1]);//删除行
            startEdit($grid);//删除后启动全部编辑
        }
        else {
            saveRows($grid);//删除前保存信息
            $grid.delGridRow(rowKey);//删除行
            startEdit($grid);//删除后启动全部编辑
        }
    }
    else {
        alert("请选择行");
    }
}

function refreshFiterGrid() {

    InitFilter();
}

function startEdit($gridCase) {
    //var grid = $("#jqGrid");
    var ids = $gridCase.jqGrid('getDataIDs');

    for (var i = 0; i < ids.length; i++) {
        $gridCase.jqGrid('editRow', ids[i]);
    }
};

function saveRows($gridCase) {
    //var grid = $("#jqGrid");
    var ids = $gridCase.jqGrid('getDataIDs');

    for (var i = 0; i < ids.length; i++) {
        $gridCase.jqGrid('saveRow', ids[i]);
    }
}

//获取指定格式正则表达式集合
function GetRegulars() {
    var optionValues = "";

    var rgs = $("#RegularVals").val();
    if (rgs != null && rgs.length > 0) {
        var sec = $.parseJSON(rgs);
        if (sec != null && sec.length > 0) {
            optionValues += " : ;";
            for (var i = 0; i < sec.length; i++) {
                optionValues += sec[i].value + ":" + sec[i].text + ";";
            }
            if (optionValues.length > 0) {
                optionValues = optionValues.substr(0, optionValues.length - 1);
            }
        }
    }
    else
        optionValues = " : ";
    return optionValues;
}

//获取正则表达式集合
function SetRegularHt() {
    $.ajax({
        type: "post",
        url: "../api/services/api/Regular/GetDropDownList",
        async: false,
        success: function (e) {
            if (e.success) {
                var sec = e.result;
                if (sec != null && sec.length > 0) {
                    $("#RegularVals").val(JSON.stringify(sec));//将正则json串存入隐藏域
                }
            }
        }
    });
}

//获取当前筛选控件
function GetFilterGrid()
{
    var $gridCase = null;//初始化
    var curRpType = $("#curReportType").val();
    switch (curRpType) {
        case "1":
        case "2":
            $gridCase = $("#tbFilterGrid");
            break;
        case "3":
            break;
        case "4":
            $gridCase = $("#rdlcFilterGrid");
            break;
        default:
            break;
    }
    return $gridCase;
}
//根据子报表类型获取相关的控件名s
function GetFilterControlNames()
{
    var curRpType = $("#curReportType").val();

    var controls = {
        rpType: curRpType,
        fieldGrid: "",
        filterGrid: "",
        filterGridPager: "",
        currChildReport: ""
    }
    switch (curRpType) {
        case "1":
        case "2":
            controls.fieldGrid = "fieldGrid";
            controls.filterGrid = "tbFilterGrid";
            controls.filterGridPager = "tbFilterGridPager";
            controls.currChildReport = "currTbReport";
            break;
        case "3":
            break;
        case "4":
            controls.fieldGrid = "";
            controls.filterGrid = "rdlcFilterGrid";
            controls.filterGridPager = "rdlcFilterGridPager";
            controls.currChildReport = "currRdlcReport";
            break;
        default:
            break;
    }
    return controls;
}