//**********************************************
//该页面仅支持PC端rdlc报表的展示
//**********************************************

$(document).ready(function () {
    var rdlcReport = InitRdlcReport();//解析RDLC报表
    WinResize();
    //终止事件冒泡
    if (ExternalTools != undefined && ExternalTools != null) {
        ExternalTools.addEventListener('click', function (e) {
            e.stopPropagation();
        }, false);
    }
    if (searchTools != undefined && searchTools != null) {
        searchTools.addEventListener('click', function (e) {
            e.stopPropagation();
        }, false);
    }
    if (spTools != undefined && spTools != null) {
        spTools.addEventListener('click', function (e) {
            e.stopPropagation();
        }, false);
    }

    //工具栏目的点击事件 
    $("#divTools").click(function () {
        if ($(".fa-chevron-up").is(":hidden")) {
            $("#divTools").height(28);
            $('#filterHts').collapse('show');
            $(".fa-chevron-down").hide();
            $(".fa-chevron-up").show();
            if ($("#searchTools").html() != "") {
                $("#searchTools").show();
            }
            if ($("#ExternalTools").html() != "") {
                $("#ExternalTools").show();
            }
            if ($("#spTools").html() != "") {
                $("#spTools").show();
            }
            $(this).attr("title", "点击关闭搜索条件区");
        } else {
            $(".fa-chevron-down").show();
            $(".fa-chevron-up").hide();
            if ($("#searchTools").html() != "") {
                $("#searchTools").hide();
            }
            if ($("#ExternalTools").html() != "" || $("#spTools").html() != "") {
                $("#divTools").height(28);
            } else {
                $("#divTools").height(10);
            }
            $('#filterHts').collapse('hide');
            $(this).attr("title", "点击展开搜索条件区");
        }
    });

    //是否显示筛选
    if (rdlcReport.IsShowFilter) {
        if ($.trim($("#filterHts").html()) != "") {
            $("#searchTools").show();
        }
        $("#divTools").height(28);
        $('#filterHts').collapse('show');
        $(".fa-chevron-down").hide();
        $(".fa-chevron-up").show();
        $("#divTools").attr("title", "点击关闭搜索条件区");
    }

    //当隐藏效果完成后，执行的处理事件（重新计算高度）
    $('#filterHts').on('shown.bs.collapse', function () {
        WinResize();//重新计算窗体高度
        $("#searchTools").show();
    });
    $('#filterHts').on('hidden.bs.collapse', function () {
        WinResize();//重新计算窗体高度
        $("#searchTools").hide();
    });
    //点按钮显示标签信息
    $(".popover-hide").click(function () {
        $(this).popover({ html: true });
        $(this).popover('show');
    });
    //点空白，取消显示外置事件的标签  
    $(".tab-content").click(function () {
        $('.popover-hide').popover('destroy');
    });

});

//页面元素加载完毕后触发
window.onload = function () {
    DoSearch();//自动触发查询    
}

//随着浏览器的变化而变化
$(window).resize(function () {
    WinResize();
});

//设置列表高宽
var WinResize = function () {
    //判断条件label高度是否置顶
    $("#filterHts .control-label").each(function () {
        if ($(this).height() > 17) {
            $(this).css("margin-top", "0px");
        } else {
            $(this).css("margin-top", "10px");
        }
    });

    var intTitleBar = $(".ui-jqgrid-titlebar").height() + 10;
    var grdHeight = $(window).height() - $("#navMenu").height() - $(".ui-jqgrid-hdiv").height() - $("#jqGridPager").height() - intTitleBar;
    $("#divContent").height(grdHeight);
}

//获取当前PC端RDLC报表
//childRpType:1=RDLC 2键值 3=图形 4=rdlc
//appType:PC、APP
function GetChildReport(childRpType,appType)
{
    var rdlcReport = "";
    var rps = $("#ChildReportListJson").val();
    if (rps != null && rps.length > 0) {
        var rpArr = $.parseJSON(rps);
        if (rpArr != null && rpArr.length > 0) {
            for (var i = 0; i < rpArr.length; i++) {
                rp = rpArr[i];
                if (rp.ChildReportType == childRpType && rp.ApplicationType.toUpperCase() == appType) {
                    tb = rp.ChildReportJson;
                    rdlcReport = $.parseJSON(tb);
                }
            }
        }
    }
    return rdlcReport;
}

//解析RDLC报表
//不支持外置事件
function InitRdlcReport() {
    var fieldArr = $.parseJSON("[]"); //字段信息初始化
    var rdlcReport = GetChildReport(4,"PC");//获取当前PC端RDLC报表
    
    //查询父级IFRAM的ID
    var framIds = parent.frames;
    var framId = "";//IFRAM的ID
    for (var i = 0; i < framIds.length; i++) {
        if (framIds[i].location.href == location.href) {
            framId = framIds[i].name;
            break;
        }
    }

    //#region 解析筛选(含筛选事件按钮标签)

    //获取筛选集合 filterHts
    if (rdlcReport.FilterListJson != null && rdlcReport.FilterListJson != "" && rdlcReport.FilterListJson != "[]") {
        var filterArr = $.parseJSON(rdlcReport.FilterListJson);
        if (filterArr != null && filterArr.length > 0) {
            //控制筛选控件的位置顺序
            //filterArr = filterArr.sort(compare("OrderNum"));//按照OrderNum升序排序

            var htmls = "";//定义全局标签
            var script = "";

            //循环拼凑筛选功能
            for (var f = 0; f < filterArr.length; f++) {
                var ft = filterArr[f];
                //当为筛选项时
                if (ft.IsSearch) {
                    var htm = '';//初始化标签
                    var ftCtr = "";//声明控件
                    var ftCtrName = ft.FieldParam;//控件名称
                    var ftCtrDisplayName = ft.FieldName//控件显示名称
                    //控件提示语
                    var ftCtrPlaceholder = ft.Placeholder == null || ft.Placeholder == "" ? "" : ft.Placeholder;
                    //控件默认值
                    var ftCtrValue = ft.DefaultValue == null || ft.DefaultValue == "" ? "" : ft.DefaultValue;

                    //条件控件的命名格式为：cond_ + 控件名称
                    var condiCtrOne = '';//数值及时间形式
                    //#region condiCtrOne
                    condiCtrOne += '<div class="jgrid-filter-operator " style="padding-left:0px;padding-right:0px;z-index:1;"><select id="cond_' + ftCtrName + '" name="cond_' + ftCtrName + '" class="form-control option">';
                    condiCtrOne += '   <option value="=">等于</option>'
                    condiCtrOne += '   <option value=">">大于</option>'
                    condiCtrOne += '   <option value="<">小于</option>'
                    condiCtrOne += '   <option value="in">存在</option>'
                    condiCtrOne += '</select></div>'
                    //#endregion
                    var condiCtrTwo = '';//字符串形式
                    //#region condiCtrTwo
                    condiCtrTwo += '<div class="jgrid-filter-operator" style="padding-left:0px;padding-right:0px;z-index:1;"><select id="cond_' + ftCtrName + '" name="cond_' + ftCtrName + '" class="form-control option">';
                    condiCtrTwo += '   <option value="=">等于</option>'
                    condiCtrTwo += '   <option value="like">包含</option>'
                    condiCtrTwo += '   <option value="in">存在</option>'
                    condiCtrTwo += '</select></div>'
                    //#endregion

                    //自定义不加入条件控件
                    if (!ft.IsCustom) {
                        if (ft.DataType == "String")
                            ftCtr += condiCtrTwo;
                        else
                            ftCtr += condiCtrOne;
                    }

                    //解析生成控件
                    switch (ft.FilterType) {
                        case "1"://文本框
                            ftCtr += '<input id="ft_' + ftCtrName + '" name="ft_' + ftCtrName + '" value="' + ftCtrValue + '" type="text" placeholder="' + ftCtrPlaceholder + '" class="jgrid-filter-input form-control" ' + (ft.IsCustom ? 'style="padding-left:5px"' : '') + ' /></div>';
                            script += 'SetDefaultParamValue("ft_' + ftCtrName + '","' + ftCtrValue + '",false,false,' + ft.IsCustom + ');';
                            break;
                        case "2"://复选下拉框
                            var ops = GetOptions(bootPATH+"/report/GetFilterDropDown?filterId=" + ft.Id + "&code=" + $("#Code").val());
                            ftCtr += '<div class="myOwnDdl col-md-12 col-sm-12" style="padding: 0px;">  ';
                            ftCtr += '  <select id="ft_' + ftCtrName + '" name="ft_' + ftCtrName + '" value="' + ftCtrValue + '" placeholder="' + ftCtrPlaceholder + '" multiple="multiple" class="jgrid-filter-input" style="padding: 0px;color: #76838f;border: 1px solid #e4eaec; width:100%;hight:34px">';
                            ftCtr += ops;
                            ftCtr += '  </select>  '
                            ftCtr += '</div>  ';
                            //script += 'evalMultiselect("ft_' + ftCtrName + '","' + ftCtrValue + '",false)';
                            script += 'SetDefaultParamValue("ft_' + ftCtrName + '","' + ftCtrValue + '",true,false,' + ft.IsCustom + ');';
                            break;
                        case "3"://单选下拉框
                            var ops = GetOptions(bootPATH+"/report/GetFilterDropDown?filterId=" + ft.Id + "&code=" + $("#Code").val());
                            ftCtr += '<div class="myOwnDdl col-md-12 col-sm-12" style="padding: 0px;">  ';
                            ftCtr += '  <select id="ft_' + ftCtrName + '" name="ft_' + ftCtrName + '" value="' + ftCtrValue + '" placeholder="' + ftCtrPlaceholder + '" multiple="multiple" class="jgrid-filter-input" style="padding: 0px;color: #76838f;border: 1px solid #e4eaec; width:100%;hight:34px">';
                            ftCtr += ops;
                            ftCtr += '  </select>  '
                            ftCtr += '</div>  ';
                            //script += 'evalMultiselect("ft_' + ftCtrName + '","' + ftCtrValue + '",true)';
                            script += 'SetDefaultParamValue("ft_' + ftCtrName + '","' + ftCtrValue + '",true,true,' + ft.IsCustom + ');';
                            break;
                        case "4"://年月日yyyy-mm-dd
                            ftCtr += '<input type="text" id="ft_' + ftCtrName + '" name="ft_' + ftCtrName + ' value="' + ftCtrValue + '" onfocus="WdatePicker({doubleCalendar:true,dateFmt:\'yyyy-MM-dd\'})" class="form-control jgrid-filter-input"/><i class="fa fa-calendar form-control-feedback" style="padding-top:12px;" ></i> ';
                            script += 'SetDefaultParamValue("ft_' + ftCtrName + '","' + ftCtrValue + '",false,false,' + ft.IsCustom + ');';
                            break;
                        case "5"://年月yyyy-mm
                            ftCtr += '<input type="text" id="ft_' + ftCtrName + '" name="ft_' + ftCtrName + ' value="' + ftCtrValue + '" onfocus="WdatePicker({skin:\'whyGreen\',dateFmt:\'yyyy-MM\'})" class="form-control jgrid-filter-input"/><i class="fa fa-calendar form-control-feedback" style="padding-top:12px;" ></i>';
                            script += 'SetDefaultParamValue("ft_' + ftCtrName + '","' + ftCtrValue + '",false,false,' + ft.IsCustom + ');';
                            break;
                        default://默认为文本框
                            ftCtr += '<input id="ft_' + ftCtrName + '" name="ft_' + ftCtrName + '" type="text" value="' + ftCtrValue + '" placeholder="' + ftCtrPlaceholder + '" class="form-control jgrid-filter-input" style="padding-left:1px" />';
                            script += 'SetDefaultParamValue("ft_' + ftCtrName + '","' + ftCtrValue + '",false,false,' + ft.IsCustom + ');';
                            break;
                    }

                    htm += '<div class="form-group col-md-3 col-sm-3" style="padding:0px">';
                    htm += '    <label for="ft_' + ftCtrName + '" class="col-md-3 col-sm-3 control-label text-right" style="padding-left: 0px;padding-right: 2px;margin-top: 10px;line-height:17px" >' + ftCtrDisplayName + '</label>';
                    htm += '<div class="col-md-9 col-sm-9 jgrid-filter-field" style="padding:0px">' + ftCtr + '</div>';
                    htm += ' </div>';
                    htmls += htm;
                }
            }

            //条件查询
            if (htmls != null && htmls.length > 0) {
                //查询条件 
                var strSearch = '<button type="button" class="btn btn-primary" onclick="DoSearch()"><i class="glyphicon glyphicon-search"></i> 查询</button>';//查询按钮
                strSearch += '&nbsp;<button type="button" class="btn btn-warning" onclick="ResetSearch()"><i class="fa fa-reply"></i> 重置</button>';//重置按钮
                $("#searchTools").html(strSearch);
                //查询条件
                $("#filterHts").append(htmls);//添加标签
                eval(script);
            }
        }
    }
    //#endregion

    //#region 处理内置事件
    var strExternal = "";//初始化内置事件标签
    var strExtScript = "";//初始化内置事件js代码块
    var strTools = "";
    if (rdlcReport.InEventListJson != null && rdlcReport.InEventListJson != "" && rdlcReport.InEventListJson != "[]")
    {
        var inEvList = $.parseJSON(rdlcReport.InEventListJson);
        if (inEvList != null && inEvList.length > 0) {
            for(var i=0;i<inEvList.length;i++)
            {
                var inEvent = inEvList[i];
                strExternal += inEvent.BtnHtml;
                strExtScript += inEvent.BtnJs;
            }
        }
    }
    $("#ExternalTools").html(strExternal);//添加html
    $("#dynamicScript").append(strExtScript);//添加js

    //#endregion
    
    //内置事件
    if ($.trim(strExternal) != "" && $.trim(strTools) != "") {
        $("#ExternalTools").addClass("toolPlace");
    }
    strSearch = $.trim(strSearch);
    strExternal = $.trim(strExternal);
    strTools = $.trim(strTools);

    //查询按钮
    if (strSearch != "" && strExternal != "" || strTools != "") {
        $("#searchTools").addClass("toolPlace");
    }
    //strExternal = "";
    //$("#ExternalTools").html("");//添加html
    //strTools = "";
    //$("#spTools").html("");
    //是否显示搜索菜单
    if (strSearch == "" && strExternal == "") {
        $("#navMenu").hide();
    } else if (strSearch != "" && strExternal == "") {
        $("#divTools").height(10);
    }

    //目前取得是页面宽-屏幕可用工作区宽度
    var winWidth = window.screen.availWidth;
    var queryParamArr = GetQueryParamArr(rdlcReport);//获得查询参数集合列表
    return rdlcReport;
}

//报表说明
function GridExplain() {
    var strTitle = "报表说明";
    var rdlcReport = GetChildReport(4,"PC");
    var strContent = (rdlcReport.Remark != undefined && rdlcReport.Remark != null && rdlcReport.Remark != "" && rdlcReport.Remark.toLowerCase() != "null" && breport.Remark != "NaN" ? rdlcReport.Remark : "暂无！");
    strContent = "报表说明：" + strContent + "</div>";

    new PopMsg(strTitle, strContent, "");//右下角显示信息
}

//根据筛选条件查询数据
function DoSearch() {
    var rdlcReport = GetChildReport(4,"PC");//获取当前PC端RDLC报表
    if (rdlcReport != "" && rdlcReport != null) {
        var queryParamArr = GetQueryParamArr(rdlcReport);//获得查询参数集合列表

        //#region 给rdlc.aspx页面控件赋值
        $("#ifm").contents().find("#code").val($("#Code").val());
        $("#ifm").contents().find("#queryParams").val(JSON.stringify(queryParamArr));
        $("#ifm").contents().find("#rpName").val($("#Name").val());
        $("#ifm").contents().find("#xmlStr").val(Encrypt(rdlcReport.RdlcXml));

        //endregion

        $("#ifm").contents().find("#SearchBtn").click();//触发RDLC子页面的查询按钮
    }
}

//获得查询参数集合列表
function GetQueryParamArr(rdlcReport) {
    var queryParamArr = [];//初始化得到的查询集合
    //先获得原筛选配置信息
    if (rdlcReport.FilterListJson != null && rdlcReport.FilterListJson != "" && rdlcReport.FilterListJson != "[]") {
        var filterArr = $.parseJSON(rdlcReport.FilterListJson);
        if (filterArr != null && filterArr.length > 0) {
            for (var i = 0; i < filterArr.length; i++) {
                var ft = filterArr[i];
                //为筛选条件时
                if (ft.IsSearch) {
                    var paramValue = $("#ft_" + ft.FieldParam).val();
                    paramValue = (paramValue == null ? "" : paramValue.toString());
                    //当筛选控件有值时
                    if (paramValue != null && paramValue != "" && paramValue.trim() != "") {
                        var param = {
                            FieldCode: "",
                            FieldParam: "",
                            OpType: "",
                            Value: "",
                            DataType: "",
                            FilterType: ""
                        };//初始化
                        param.FieldCode = ft.FieldCode;
                        param.FieldParam = ft.FieldParam;
                        param.FilterType = ft.FilterType;//筛选类型
                        //当不为自定义时赋值
                        if (!ft.IsCustom) {
                            param.OpType = $("#cond_" + ft.FieldParam).val();//获取条件下拉值
                        }
                        param.Value = $.trim(paramValue);//获取控件值
                        param.DataType = ft.DataType;//字段类型
                        queryParamArr.push(param);//添加筛选到集合
                    }
                }
            }
        }
    }

    //将在url中设置的参数添加到参数列表中.其中,若在url的参数名等于查询区域获得的参数名 则以查询区的为主
    var defPars = $("#KVJson").val();
    if (defPars != null && defPars != "") {
        parArr = $.parseJSON(defPars);
        //遍历替换在url中设置的默认值
        for (var i = 0; i < parArr.length; i++) {
            var par = parArr[i];
            if (queryParamArr != null && queryParamArr.length > 0) {
                for (var j = 0; j < queryParamArr.length; j++) {
                    if (par.K == queryParamArr[j].FieldParam) {
                        //不处理
                    }
                    else if (j == queryParamArr.length - 1) {
                        var param = {
                            FieldCode: "",
                            FieldParam: "",
                            OpType: "",
                            Value: "",
                            DataType: "",
                            FilterType: ""
                        };//初始化
                        param.FieldParam = par.K;
                        param.Value = $.trim(par.V);//获取控件值
                        queryParamArr.push(param);//添加筛选到集合
                    }
                }
            }
            else {
                var param = {
                    FieldCode: "",
                    FieldParam: "",
                    OpType: "",
                    Value: "",
                    DataType: "",
                    FilterType: ""
                };//初始化
                param.FieldParam = par.K;
                param.Value = $.trim(par.V);//获取控件值
                queryParamArr.push(param);//添加筛选到集合
            }
        }
    }
    return queryParamArr;
}

//重置筛选条件
function ResetSearch()
{
    var rdlcReport = GetChildReport(4,"PC");//获取当前PC端RDLC报表
    //先获得原筛选配置信息
    if (rdlcReport.FilterListJson != null && rdlcReport.FilterListJson != "" && rdlcReport.FilterListJson != "[]") {
        var filterArr = $.parseJSON(rdlcReport.FilterListJson);
        if (filterArr != null && filterArr.length > 0) {

            for (var i = 0; i < filterArr.length; i++) {
                var ft = filterArr[i];
                //为筛选条件时
                if (ft.IsSearch) {
                    if (ft.FilterType == "2" || ft.FilterType == "3") {
                        $("#ft_" + ft.FieldParam).val([]).multipleSelect("refresh");//赋空值后重新加载控件值
                    }
                    else
                        $("#ft_" + ft.FieldParam).val("");
                    if (!ft.IsCustom) {
                        if (ft.FilterType == "2") {
                            $("#cond_" + ft.FieldParam).val("in");//当为复选框时，设置操作符为存在
                        }
                        else
                            $("#cond_" + ft.FieldParam).val("=");//恢复为等于
                    }
                }
            }
        }
    }
    DoSearch();//执行查询
}

//获取multiselect的options
var GetOptions = function (url) {
    var ops = "";
    var data = $.ajax({
        url: url,
        async: false
    });

    if (data != null && data.responseText != null && data.responseText.length > 0) {
        var arr = $.parseJSON(data.responseText);
        $.each(arr, function (i, item) {
            ops += "<option value='" + item.VALUE + "'>&nbsp;" + item.TEXT + "</option>\r\n";
        });
    }
    return ops;
}
//给下拉控件赋值
var evalMultiselect = function (ctrlName, ftCtrValue, isSingle) {
    var control = $('#' + ctrlName);

    var valArr = [];//初始化默认选中项
    if (ftCtrValue != null && ftCtrValue != "" && ftCtrValue.length > 0) {
        var valArr = ftCtrValue.toString().split(",");
    }

    //设置select的处理
    if (isSingle) {
        control.val(valArr).multipleSelect({
            placeholder: "请选择",
            //width: '100%',
            single: true
        });
    }
    else {
        control.val(valArr).multipleSelect();
    }
}

//设置筛选区控件的默认值（若参数变量在筛选配置区和链接中均有，则以链接中的为主）
var SetDefaultParamValue = function (ctrlName, ftCtrValue, isMultiSelect, isSingle, isCustom)
{
    var parArr = [];
    var paramName = ctrlName.split("ft_")[1];//参数名
    var defValue = ftCtrValue;//初始化

    if (!isCustom)//自定义筛选
    {
        var condCtr = $("#cond_" + paramName);//筛选操作符控件
        if (isMultiSelect && !isSingle) {
            condCtr.val("in");//当为复选框时，设置操作符为存在
        }
    }

    var defPars = $("#KVJson").val();
    if (defPars != null && defPars != "")
    {
        parArr = $.parseJSON(defPars);
    }
    //遍历替换在url中设置的默认值
    for (var i = 0; i < parArr.length; i++)
    {
        var par = parArr[i];
        if(par.K==paramName)
        {
            defValue = par.V;
        }
    }

    if (isMultiSelect) {
        evalMultiselect(ctrlName, defValue, isSingle);
    }
    else {
        var control = $('#' + ctrlName);
        control.val(defValue);
    }
}