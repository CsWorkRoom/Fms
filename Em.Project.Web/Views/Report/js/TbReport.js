//**********************************************
//该页面仅支持PC端表格报表的展示
//**********************************************

$(document).ready(function () {
    //$("#divTools span").click(function () {
    //    $('#filterHts').click();
    //});
    var tbreport = InitTbReport();//解析表格报表

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
        }
        SetDefultDataImg("jqGrid");
        SetShrinkToFit("jqGrid");
    });

    //是否显示筛选
    if (tbreport.IsShowFilter) {
        if ($.trim($("#filterHts").html()) != "") {
            $("#searchTools").show();
        }
        $("#divTools").height(28);
        $('#filterHts').collapse('show');
        SetSeach();//计算查询条件高度
        $(".fa-chevron-down").hide();
        $(".fa-chevron-up").show();
        $("#divTools").attr("title", "点击关闭搜索条件区");
    }

    //点按钮显示标签信息
    $(".popover-hide").click(function () {
        $(this).popover({ html: true });
        $(this).popover('show');
    });
    //点空白，取消显示外置事件的标签  
    $(".tab-content").click(function () {
        $('.popover-hide').popover('destroy');
    });
    
    //当隐藏效果完成后，执行的处理事件（重新计算高度）
    $('#filterHts').on('shown.bs.collapse', function () {
        SetPeportSize("jqGrid", "navMenu", "jqGridPager");//重新计算窗体高度
        $("#searchTools").show();
        SetSeach();
    });
    $('#filterHts').on('hidden.bs.collapse', function () {
        SetPeportSize("jqGrid", "navMenu", "jqGridPager");//重新计算窗体高度
        $("#searchTools").hide();
    });
    //滚动取得更多菜单
    $("#gview_jqGrid .ui-jqgrid-bdiv").scroll(function () {
        $("#moreMenu").hide();
    });
    //移出更多列表取消更多菜单
    $("#moreMenu").mouseleave(function () {
        $("#moreMenu").hide();
    });

    $(window).resize();
});

//随着浏览器的变化而变化
$(window).resize(function () {
    WinResize("jqGrid", "navMenu", "jqGridPager");
    SetSeach();//计算查询条件高度
});

var GetEventArrLength = function (EventArr) {
    if (EventArr.length == 1 && EventArr[0].DisplayName.length <= 2) {
        return 40;
    }
    var intReturnNum = 0;
    $.each(EventArr, function () {
        var intTemp = $(this)[0].DisplayName.length;
        if (intTemp > 0)
            intReturnNum += intTemp * 10;
    });
    return intReturnNum;
}

var SetSeach = function(){
    //判断条件label高度是否置顶
    $("#filterHts .control-label").each(function () {
        if ($(this).height() > 17) {
            $(this).css("margin-top", "0px");
        } else {
            $(this).css("margin-top", "10px");
        }
    });
}

///显示更多菜单 
///menuId:模型ID
///objA:点击对像
var ShowMoreMenu = function (menuId, objA) {
    var gridScrollTop = $("#gview_jqGrid .ui-jqgrid-bdiv").scrollTop();
    var intTop = (objA.offsetParent.offsetParent.offsetTop + objA.offsetParent.offsetTop + objA.offsetTop + $(objA).height() - gridScrollTop-2);
    var intLeft = (objA.offsetParent.offsetParent.offsetLeft + objA.offsetParent.offsetLeft + objA.offsetLeft + ($(objA).width()/2));
    $("#gbox_jqGrid").append($("#moreMenu"));//将隐藏层移到表格中
    $("#moreMenu").html($("#" + menuId).html());
    $("#moreMenu").css("top", intTop);
    $("#moreMenu").css("left", intLeft);
    $("#moreMenu").show();    
}

//获取当前PC端表格报表
function GetTbReport()
{
    var tbreport = "";
    var rps = $("#ChildReportListJson").val();
    if (rps != null && rps.length > 0) {
        var rpArr = $.parseJSON(rps);
        if (rpArr != null && rpArr.length > 0) {
            for (var i = 0; i < rpArr.length; i++) {
                rp = rpArr[i];
                if (rp.ChildReportType == 1 && rp.ApplicationType.toUpperCase() == "PC") {
                    tb = rp.ChildReportJson;
                    tbreport = $.parseJSON(tb);
                }
            }
        }
    }
    return tbreport;
}

//解析表格报表
function InitTbReport() {
    var fieldArr = $.parseJSON("[]"); //字段信息初始化
    var outEventArr = [];//外置事件集合初始化
    var tbreport = GetTbReport();//获取当前PC端表格报表
    //获得事件列表（后-列、行、内容部分会使用）
    if (tbreport.OutEventListJson != null && tbreport.OutEventListJson != "" && tbreport.OutEventListJson != "[]") {
        outEventArr = $.parseJSON(tbreport.OutEventListJson);
    }

    //#region 解析生成-行事件
    //将行事件作为一个列拼凑在列首

    //#region 当前事件列表是否有行事件isRowEv
    var isRowEv = false;
    if (outEventArr != null && outEventArr.length > 0) {
        for (var j = 0; j < outEventArr.length; j++) {
            var ev = outEventArr[j];
            if (ev.EventType == "1")//为行事件时
            {
                isRowEv = true;
            }
        }
    }
    //#endregion

    //初始化当前拼凑的字段对象
    var curRowF = {
        //label: "操作区",//标签内容
        label: '<label title="操作区" style="width:98%;text-align:center">操作区</label>',
        name: "actions",//字段编码
        align: "center",//横向位置
        sortable: false,//是否排序
        hidden: false,//是否隐藏
        frozen: true,//是否冻结
        // width: 80,//列宽度
        width: GetEventArrLength(outEventArr),//列宽度
        formatter: function (cellvalue, options, rowObject) { return cellvalue; }
    };
    var intRowNum = 0;
    var formatRowFun = function (cellvalue, options, rowObject) {

        var lbl = "";
        var AddNum = 0;
        if (outEventArr != null && outEventArr.length > 0) {
            var strLi = '';
            for (var j = 0; j < outEventArr.length; j++) {
                var ev = outEventArr[j];
                if (ev.EventType == "1")//为行事件时
                {
                    if (AddNum > 1) {
                        strLi += '<li onclick="$(\'#moreMenu\').hide()">' + OutEventForLabel(ev, null, rowObject, cellvalue, null) + '</li>';
                    } else {
                        lbl += OutEventForLabel(ev, null, rowObject, cellvalue,null) + " ";
                    }
                    AddNum++;
                }                
            }
        }
        //列表菜单
        if (strLi != "") {
           var menuId = "menuData" + intRowNum;
           lbl += '<span class="dropdown" style="margin-left: 2px;"><a style="cursor: pointer;" type="button" class="aLabel" onmousemove="ShowMoreMenu(\'' + menuId + '\',this)" ><i id="icon_type_img" class="fa fa-book"></i>更多<span class="caret"></span></a><ul id="' + menuId + '" style="display:none">' + strLi + '</ul></span>';
        }

        intRowNum++;
        return lbl;
    };
    if (isRowEv) {
        curRowF.formatter = formatRowFun;
        fieldArr.push(curRowF);//把行事件添加到列首
    }
    //#endregion

    //#region 解析字段 包含事件：（包含列事件、内容/行事件(formatter)）（多表头和全局事件分别单独解析）

    if (tbreport.FieldListJson != null && tbreport.FieldListJson != "" && tbreport.FieldListJson != "[]") {
        var fields = $.parseJSON(tbreport.FieldListJson);
        fields = fields.sort(compare("OrderNum"));//根据字段序号排序-升序

        //拼凑字段信息json
        for (var i = 0; i < fields.length; i++) {

            //初始化当前拼凑的字段对象
            var curField = {
                label: "label",//标签内容
                name: "name",//字段编码
                align: "center",//横向位置
                sortable: true,//是否排序
                hidden: false,//是否隐藏
                frozen: false,//是否冻结
                // width: 80,//列宽度
                width: outEventArr.length * 30,//列宽度
                formatter: function (cellvalue, options, rowObject) { return cellvalue; }
            };

            var fd = fields[i];
            var fieldEvLb = "";//初始化列-事件lb

            //#region 事件解析（列/内容事件）

            //验证当前字段是否含列/内容事件
            //内容事件：在formatter中拼凑function（包含内容事件）
            //列事件：在表头列中拼凑带链接的lb
            if (outEventArr != null && outEventArr.length > 0) {
                //初始内容格式化方法
                var contentFomatFun = null;
                //初始化内容事件方法
                var contentEventFun = null;//注意：需要在内容事件中去获得内容格式化的值

                for (var j = 0; j < outEventArr.length; j++) {
                    var ev = outEventArr[j];
                    //字段含列事件、且url验证合法时，将进入事件拼凑环节
                    //if (ev.EventType == 4 && ev.FieldCode == fd.FieldCode && IsURL(ev.Url))
                    if (ev.EventType == 4 && ev.FieldCode == fd.FieldCode) {
                        //获取列事件标签（a/button）
                        fieldEvLb = OutEventForLabel(ev, fd.FieldName, null, null,null);
                    }
                    //#region 内容事件解析及生成

                    //字段含内容事件、
                    //if (ev.EventType == 3 && ev.FieldCode == fd.FieldCode && IsURL(ev.Url)) {
                    if (ev.EventType == 3 && ev.FieldCode == fd.FieldCode) {

                        //#region 根据字段编码获得内容格式化集合 contEventArr
                        var contEventArr = [];
                        for (var kk = 0; kk < outEventArr.length; kk++)
                        {
                            var kEv = outEventArr[kk];
                            if(kEv.EventType == 6 && kEv.FieldCode == fd.FieldCode)
                            {
                                contEventArr.push(outEventArr[kk]);
                            }
                        }
                        //#endregion

                        var evs = JSON.stringify(ev);//将对象转换为string

                        //给各个字段的内容事件赋值.赋值规则：event_字段编码
                        //不能给动态变量赋值某个隐式对象，比如json对象。
                        //应该把对象转换为string, 可采用JSON.stringify(ev)转换为字符串,然后再把值赋给动态变量
                        eval("var event_" + fd.FieldCode + " = " + evs);

                        //拼凑内容事件的formatter函数
                        contentEventFun = function (cellvalue, options, rowObject) {
                            //获取内容事件标签
                            //options.colModel.name为当前列的name名称
                            var evlb = OutEventForLabel(eval("event_" + options.colModel.name), null, rowObject, cellvalue, contEventArr);
                            if (evlb != null && evlb != "") {
                                return evlb;
                            }
                            return cellvalue;
                        };
                        //curField.formatter = formatFun;//内容格式化
                    }
                    //#endregion

                    //#region 内容格式化
                    if (ev.EventType == 6 && ev.FieldCode == fd.FieldCode) {
                        var evs = JSON.stringify(ev);//将对象转换为string
                        eval("var contEvent_" + fd.FieldCode + " = " + evs);

                        //拼凑内容格式化的formatter函数
                        contentFomatFun = function (cellvalue, options, rowObject) {
                            var res = ContForLabel(eval("contEvent_" + options.colModel.name), rowObject);//返回格式化后的值
                            if (res == "" || res == null) {
                                return cellvalue;
                            }
                            else
                                return res;
                        };
                    }
                    //#endregion
                }
            }
            if (contentEventFun != null)
            {
                curField.formatter = contentEventFun;
            }
            else if (contentFomatFun != null) {
                curField.formatter = contentFomatFun;
            }

            //若列事件返回值为空，则显示原fd.FieldName
            //若列事件返回不为空，则替换为事件lb
            var tit = (fd.Remark == null || fd.Remark == "") ? fd.FieldName : fd.Remark;
            fieldEvLb = (fieldEvLb == "" || fieldEvLb == null) ? '<label title=\'' + tit + '\' style=\'width:100%;margin-right: -20px;text-align:' + fd.Align + '\' >' + fd.FieldName + '</label>' : '<div style=\'width:100%;margin-right: -20px;text-align:' + fd.Align + '\'>' + fieldEvLb + '</div>';
            curField.label = fieldEvLb;//标签内容
            curField.name = fd.FieldCode;//字段编码
            curField.align = fd.Align;//fd.Align;//横向位置
            curField.sortable = fd.IsOrder;//是否排序
            curField.hidden = !fd.IsShow;//是否隐藏
            curField.frozen = fd.IsFrozen;//是否冻结
            curField.width = fd.Width;//列宽度           
            fieldArr.push(curField);//添加字段对象
            //#endregion
        }
    }
    //#endregion

    //#region 预留-内置事件解析
    //#endregion

    //#region 解析筛选(含筛选事件按钮标签)

    //获取筛选集合 filterHts
    if (tbreport.FilterListJson != null && tbreport.FilterListJson != "" && tbreport.FilterListJson != "[]") {
        var filterArr = $.parseJSON(tbreport.FilterListJson);
        if (filterArr != null && filterArr.length > 0) {
            //控制筛选控件的位置顺序
            filterArr = filterArr.sort(compare("OrderNum"));//按照OrderNum升序排序

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
                            ftCtr += '<div class="myOwnDdl col-md-12 col-sm-12 col-xs-12" style="padding: 0px;">  ';
                            ftCtr += '  <select id="ft_' + ftCtrName + '" name="ft_' + ftCtrName + '" value="' + ftCtrValue + '" placeholder="' + ftCtrPlaceholder + '" multiple="multiple" class="jgrid-filter-input" style="padding: 0px;color: #76838f;border: 1px solid #e4eaec; width:100%;hight:34px">';
                            ftCtr += ops;
                            ftCtr += '  </select>  '
                            ftCtr += '</div>  ';
                            //script += 'evalMultiselect("ft_' + ftCtrName + '","' + ftCtrValue + '",false)';
                            script += 'SetDefaultParamValue("ft_' + ftCtrName + '","' + ftCtrValue + '",true,false,' + ft.IsCustom + ');';
                            break;
                        case "3"://单选下拉框
                            var ops = GetOptions(bootPATH+"/report/GetFilterDropDown?filterId=" + ft.Id + "&code=" + $("#Code").val());
                            ftCtr += '<div class="myOwnDdl col-md-12 col-sm-12 col-xs-12" style="padding: 0px;">  ';
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

                    htm += '<div class="form-group col-md-3 col-sm-3 col-xs-3" style="padding:0px">';
                    htm += '<label for="ft_' + ftCtrName + '" class="col-md-3 col-sm-3 col-xs-3 control-label text-right" style="padding-left: 0px;padding-right: 2px;margin-top: 10px;line-height:17px" >' + ftCtrDisplayName + '</label>';
                    htm += '<div class="col-md-9 col-sm-9 col-xs-9 jgrid-filter-field" style="padding:0px">' + ftCtr + '</div>';
                    htm += '</div>';
                    htmls += htm;
                }
            }

            //条件查询
            if (htmls != null && htmls.length > 0) {
                //查询条件 
                var strSearch = '<button type="button" class="btn btn-primary" onclick="DoSearch()"><i class="glyphicon glyphicon-search"></i> 查询</button>';//查询按钮
                strSearch += '&nbsp;<button type="button" class="btn btn-warning" onclick="ResetSearch()"><i class="fa fa-reply"></i> 重置</button>';//重置按钮
                $("#searchTools").html(strSearch);
                $("#spanSearch").show();
                //查询条件
                $("#filterHts").append(htmls);//添加标签
                eval(script);
            } else {
                $("#spanSearch").hide();
            }
        }
    }
    //#endregion

    //#region 处理全局事件
    //全局事件
    var strTools = "";
    if (outEventArr != null && outEventArr.length > 0) {
        for (var j = 0; j < outEventArr.length; j++) {
            var ev = outEventArr[j];
            //获得全局事件
            if (ev.EventType == 2) {
                //获取列事件标签（a/button）
                fieldEvLb = OutEventForLabel(ev, null, null, null,null);
                strTools += (strTools == "" ? "" : "&nbsp;") + fieldEvLb;
            }
        }
    }
    $("#spTools").html(strTools);
    //#endregion
    
    //#region 处理内置事件
    var strExternal = "";//初始化内置事件标签
    var strExtScript = "";//初始化内置事件js代码块

    if (tbreport.InEventListJson != null && tbreport.InEventListJson != "" && tbreport.InEventListJson != "[]") {
        var inEvList = $.parseJSON(tbreport.InEventListJson);
        if (inEvList != null && inEvList.length > 0) {
            for (var i = 0; i < inEvList.length; i++) {
                var inEvent = inEvList[i];
                strExternal += inEvent.BtnHtml;//内置事件的标签
                strExtScript += inEvent.BtnJs;//内置时间按钮js
            }
        }
    }
    //自定义js方法体-字符串
    if (tbreport.JsFun != null && tbreport.JsFun != "") {
        strExtScript += tbreport.JsFun;
    }
    $("#ExternalTools").html(strExternal);//添加html
    $("#dynamicScript").html(strExtScript);//添加js
    //#endregion
    //内置事件
    if ($.trim(strExternal) != "" && $.trim(strTools) != "") {
        $("#ExternalTools").addClass("toolPlace");
    }
    strSearch = $.trim(strSearch);
    strExternal = $.trim(strExternal);
    strTools= $.trim(strTools);

    //查询按钮
    if (strSearch != "" && strExternal != "" || strTools != "") {
        $("#searchTools").addClass("toolPlace");
    }
  
    //是否显示搜索菜单
    if (strSearch == "" && strExternal == "" && strTools == "") {
        $("#navMenu").hide();
    } else if (strSearch != "" && strExternal == "" && strTools == "") {
        $("#divTools").height(10);
    }

    //获取当前显示列的宽度之和--fieldWds
    var fieldWds = 0;
    if (fieldArr != null && fieldArr.length > 0) {
        for (var i = 0; i < fieldArr.length; i++) {
            if (!isNaN(fieldArr[i].width)) {
                fieldWds += fieldArr[i].width;
            }
        }
    }

    //获取分页下拉
    var rowArr = [];
    if (tbreport.RowList != null && tbreport.RowList.length > 0) {
        var rows = $.parseJSON(tbreport.RowList);
        if (rows != null && rows.length > 0) {
            rowArr = rows;
        }
    }
    else {
        rowArr = [tbreport.RowNum, tbreport.RowNum * 2, tbreport.RowNum * 3];
    }

    //目前取得是页面宽-屏幕可用工作区宽度
    var winWidth = window.screen.availWidth;
    $("#columnSumWidth").val(fieldWds);
    //是否自适应列宽度
    var skTofit = false;
    if (fieldWds < winWidth) {
        skTofit = true;
    }

    //设置是否显示caption
    var captionStr = "";
    if (tbreport.IsShowHeader)
    {
        captionStr="<i class='fa fa-file-excel-o' aria-hidden='true'></i> " + $("#Name").val();
    }

    var queryParamArr = GetQueryParamArr(tbreport);//获得查询参数集合列表
    $.jgrid.gridUnload("jqGrid");//先卸载
    $("#jqGrid").jqGrid({
        url: 'TbQueryList',
        postData: {
            code: $("#Code").val(),
            queryParams: JSON.stringify(queryParamArr)//查询内容
        },
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",//如果url中需要回调函数，则此处格式为jsonp
        colModel: fieldArr,
        // autowidth: true,
        viewrecords: true,
        deepempty: true,//标题栏与数据对不齐的问题
        //loadonce: false,
        width: winWidth,
        // shrinkToFit: false,//是否列宽度自适应
        autoScroll: false,//当autoScroll和shrinkToFit均为false时，会出现行滚动条
        autowidth: true,
        altRows:true,
        shrinkToFit:skTofit,//是否列宽度自适应。true=适应 false=不适应
        rowNum: tbreport.RowNum,//默认分页大小-在框架动态赋值
        rowList: rowArr,//传入分页大小的下拉-在框架动态复制
        rownumbers: tbreport.IsRowNumber,//是否显示行号
        rownumWidth: tbreport.RownumWidth == "" || tbreport.RownumWidth == null ? 30 : tbreport.RownumWidth,//行号所在列的宽度
        multiboxonly: tbreport.MultiboxOnly, //是否只有点击多选框时,才执行选择多选框checkbox.默认为false,点击一行亦选定此行的多选框
        multiSort: tbreport.IsMultiSort, //是否组合排序
        scroll: tbreport.IsScroll,//是否启动滚动分页
        emptyrecords: tbreport.EmptyRecord,//滚到到底以后的提示文
        multiselect: tbreport.IsCheck,//是否支持复选
        showPager: tbreport.IsPaination,//是否启用分页
        caption: captionStr,//设置和显示表格标题
        pager: "#jqGridPager"     
    });
    //为报表说明和分页准备的最下面的行
    $('#jqGrid').navGrid('#jqGridPager',{ edit: false, add: false, del: false, search: false, refresh: false, view: false, position: "left", cloneToTop: false });

    // 设置报表说明 ->添加一个‘问号’
    $('#jqGrid').navButtonAdd('#jqGridPager',
        {
            buttonicon: "glyphicon glyphicon-question-sign",
            title: "报表说明",
            caption: "",
            position: "last",
            onClickButton: GridExplain
        });

    // 设置报表刷新 ->添加一个‘刷新’
    $('#jqGrid').navButtonAdd('#jqGridPager',
        {
            buttonicon: "glyphicon glyphicon-refresh",
            title: "重置式刷新",
            caption: "",
            position: "last",
            onClickButton: GridReflush
        });
    
    //解析和生成多表头
    if (tbreport.FieldTopListJson != null && tbreport.FieldTopListJson != "" && tbreport.FieldTopListJson != "[]") {
        //多表头集合-topFieldArr
        var topFieldArr = $.parseJSON(tbreport.FieldTopListJson);
        if (tbreport.FieldListJson != null && tbreport.FieldListJson != "" && tbreport.FieldListJson != "[]") {
            //字段集合-fieldArr
            var fieldArr = $.parseJSON(tbreport.FieldListJson);
            GenerateTopField(fieldArr, topFieldArr, outEventArr);//调用生成多表头方法
        }
    }
    //查看是否有行内事件或冻结的列，如果有，就启用
    if (isRowEv || tbreport.FieldListJson.toLowerCase().indexOf('"isfrozen":true') >= 0) {
        $("#jqGrid").jqGrid('setFrozenColumns');//设置冻结列生效
    }
    var intTitleBar = $(".ui-jqgrid-titlebar").height()+20;
    if ($(".ui-jqgrid-titlebar").is(":hidden")) {
        intTitleBar =$(".ui-jqgrid-titlebar").height()-17;
    }
    var grdHeight = $(window).height() - $("#navMenu").height() - $(".ui-jqgrid-hdiv").height() - $("#jqGridPager").height() - intTitleBar;
    $("#jqGrid").setGridHeight(grdHeight);    
    // the bindKeys() 启用键盘操作
    $("#jqGrid").jqGrid('bindKeys');
    SetJqGridHtableBkBroundColor("rgba(244, 247, 251, 0.34);");//设置表头背景色，如果为空就为#f5f5f5

    return tbreport;
}

//报表说明
function GridExplain() {
    var strTitle = "报表说明";
    var tbreport = GetTbReport();
    var strContent = (tbreport.Remark != undefined && tbreport.Remark != null && tbreport.Remark != "" && tbreport.Remark.toLowerCase() != "null" && tbreport.Remark != "NaN" ? tbreport.Remark : "暂无！");
    strContent = strContent + "</div>";

    new PopMsg(strTitle, strContent, "");//右下角显示信息
}

//报表刷新
function GridReflush() {
    ResetSearch();//调用重置按钮
}

//生成承载事件的标签（超链接、按钮）--不含'外置事件'
//------------事件说明→----------------
//1=行事件：参数：可传入当前行其他列字段值作为参数（行局部变量）
//2=全局事件-外部：参数：可传入全局变量
//3=内容事件：参数：可传入当前值所在行的其他列的值
//4=列事件-字段
//5=列事件-多表头
//------------←事件说明----------------
//------------参数说明→----------------
//outEvent：事件对象
//fieldName：依附字段的中文名 或 多表头名
//rowObject：当前行json值
//cellvalue：当前单元格值
//contEventArr：内容格式化集合(拼凑内容事件时传入)
//------------←参数说明----------------
function OutEventForLabel(outEvent, fieldName, rowObject, cellvalue, contEventArr) {
    var returnLb = "";//初始化返回lb
    var eventType = outEvent.EventType;//事件类型
    //#region openWaylb变量赋值.打开事件方式
    //返回打开事件的字符串.格式：onclick=xxxOpen();
    var openWaylb = ClickOpenWay(outEvent, rowObject);
    //若打开方法返回值为空，则返回lb亦为空（不拼凑事件lb）
    if (openWaylb == "" || openWaylb == null)
    { return returnLb; }
    //#endregion

    //#region title变量赋值.
    var tit = outEvent.Title;
    if (outEvent.Title != null && outEvent.Title != "") {
        tit = outEvent.Title;
    }
    else if (cellvalue != null && cellvalue != "") {
        tit = cellvalue;
    }
    else if (fieldName != null && fieldName != "") {
        tit = fieldName;
    }
    else if (outEvent.DisplayName != null && outEvent.DisplayName != "") {
        tit = outEvent.DisplayName;
    }
    else {
        tit = "";
    }

    //设置title属性
    var title = tit == "" || tit == null ? " " : " title='" + tit + "' ";
    //#endregion

    //#region val变量显示值
    var val = "";
    if (eventType == 1 || eventType == 2) {
        val = outEvent.DisplayName;
    }
    else if (eventType == 4 || eventType == 5) {
        val = fieldName;
    }
    else if (eventType == 3) {
        //在这个地方加入内容格式化的处理，返回的内容就是格式化的内容
        if (contEventArr != null && contEventArr.length > 0) {
            var ress = ContForLabel(contEventArr[0], rowObject);//取一条内容格式化信息
            if (ress != null && ress != "") {
                val = ress;
            }
            else val = cellvalue;
        }
        else {
            val = cellvalue;
        }
        title = " title='" + cellvalue + "' "
    }
    else {
        val = "";
    }
    if (val == null || $.trim(val)=="") {
        return " ";
    }


    if (outEvent.OpenWay == 10) {
        title = " title='" + outEvent.DisplayName + "' "
    }

    if (title != null && title != "" && title.length > 20) {
        title = title.substr(0, 20);//截取20个字符
    }
    //#endregion

    //#region 按钮样式
    var strStyle = "btn btn-info";
    if (outEvent.Style != null && outEvent.Style != "") {
        strStyle = outEvent.Style;
    }
    //#endregion

    //#region 按钮图片
    var strIoc = "";
    if (outEvent.Icon != null && outEvent.Icon != "" && $.trim(outEvent.Icon) != "null")
        strIoc = "<i class='" + outEvent.Icon + "'></i> ";

    //#endregion
 
    //对于事件是超链接还是按钮，完全取决于用户配置。
    if (outEvent.DisplayWay == 1)//超链接
    {
        returnLb = "<a name='hrefLike' class='aLabel'" + title + openWaylb + ">" + strIoc + val + "</a>";
    }
    else if (outEvent.DisplayWay == 2)//按钮
    {
        returnLb = "<button name='hrefLike' " + title + openWaylb + " class='" + strStyle + "'>" + strIoc + val + "</button>";
    }
    else//默认超链接
    {
        returnLb = "<a name='hrefLike' class='aLabel' " + title + openWaylb + ">" + strIoc + val + "</a>";
    }
    return returnLb;
}

//获得内容格式化后的值
function ContForLabel(ev, rowObject) {
    var eventType = ev.EventType;
    var params = ev.ParamListJson;
    var pars = GetEventParms(params, rowObject, eventType);
    return CallFunName($.trim(ev.Url), pars.args);//调用函数
}

//获得当前事件的参数 { urlParems: urlParems, args: args }
function GetEventParms(params, rowObject, eventType)
{
    var urlParems = "";
    var args = "";
    //拼凑参数列表 
    if (params != null && params != "" && params != "[]") {
        var paramArr = $.parseJSON(params);
        if (paramArr != null && paramArr.length > 0) {
            for (var i = 0; i < paramArr.length; i++) {
                var pa = paramArr[i];
                if ((eventType == '3' || eventType == '1' || eventType == '6') && pa.IsField) {
                    urlParems += pa.Name + "=" + rowObject["" + pa.FieldCode + ""] + "&";
                    args += rowObject["" + pa.FieldCode + ""] + ",,,";
                }
                else {
                    urlParems += pa.Name + "=" + pa.PValue + "&";
                    args += pa.PValue + ",,,";
                }
            }
            //在此处加入能够支持替换当前url中的参数
            var defPars = $("#KVJson").val();
            if (defPars != null && defPars != "") {
                parArr = $.parseJSON(defPars);
                if (parArr != null && parArr.length > 0) {
                    for (var k = 0; k < parArr.length; k++)//循环替换url参数
                    {
                        urlParems = urlParems.replace("@{" + parArr[k].K + "}", parArr[k].V);//把占位符替换成值
                        args = args.replace("@{" + parArr[k].K + "}", parArr[k].V);//把占位符替换成值
                    }
                }
            }
            urlParems = urlParems.substring(0, urlParems.length - 1);
            args = args.substring(0, args.lastIndexOf(",,,"));
        }
    }
    return { urlParems: urlParems, args: args };//返回json对象
}

//返回带onclick的字符串。格式：onclick=xxxOpen(参数1,参数2,...)
function ClickOpenWay(ev, rowObject) {
    //初始化部分变量
    var eventType = ev.EventType;
    var openWay = ev.OpenWay;
    var url = $.trim(ev.Url);
    var title = ev.DisplayName;
    var width = ev.Width;
    var height = ev.Height;
    var params = ev.ParamListJson;

    var retStr = "";

    //#region 获得参数信息 urlParems、args
    var pars = GetEventParms(params, rowObject, eventType);//获得参数信息
    var urlParems = pars.urlParems;
    var args = pars.args;
    //#endregion

    if (url != null && url != "")
    {
        if (url.indexOf("?") > 0) {
            url = url + "&" + urlParems;
        }
        else {
            url = url + "?" + urlParems;
        }
        
        //url += "&time_stamp=" + new Date().getTime();//加入时间戳
    }


    //#region 返回打开方式的字符串,根据不同的打开方式分别去拼凑标签

    //<option value="1">弹出框</option>
    //<option value="2">顶级弹出框</option>
    //<option value="3">当页跳转</option>
    //<option value="4">新开网页</option>
    //<option value="5">新开Tab页</option>
    //<option value="6">ajax执行Post</option>
    //<option value="8">ajax执行Get</option>
    //<option value="9">ajax执行Post(含确认提示)</option>
    //<option value="7">弹子页</option>
    if (openWay == "1")//弹出框
    {
        var modalId = CreateRandomNum(1, 0, 1000);//取0到1000的随机数
        retStr = 'onclick=\"ModeDialogUrl(\'modalId' + modalId + '\',\'' + title + '\',\'' + url + '\',\'' + width + '\',\'' + height + '\')\"';
    }
    else if (openWay == "2")//顶级弹出框
    {
        var modalId = CreateRandomNum(1, 0, 1000);//取0到1000的随机数
        retStr = 'onclick=\"TopModeDialogUrl(\'modalId'+ modalId +'\',\'' + title + '\',\'' + url + '\',\'' + width + '\',\'' + height + '\')\"';
    }
    else if (openWay == "3")//当前页直接跳转
    {
        retStr = 'onclick=\"WindowLocation(\'' + url + '\')\"';
    }
    else if (openWay == "4")//新开网页
    {
        retStr = 'onclick=\"WindowOpen(\'' + title + '\',\'' + url + '\',\'' + width + '\',\'' + height + '\')\"';
    } else if (openWay == "5")//新开Tab页
    {
        retStr = 'onclick=\"AddTab(\'' + title + '\',\'' + url + '\')\"';
    }
    else if (openWay == "6")//ajax执行Post
    {
        retStr = 'onclick=\"AjaxPostFun(\'' + url + '\',this)\"';
    }
    else if (openWay == "8")//ajax执行Get
    {
        retStr = 'onclick=\"AjaxGetFun(\'' + url + '\',this)\"';
    }
    else if (openWay == "9")//ajax执行Post(含确认提示)
    {
        retStr = 'onclick=\"AjaxPostConfirm(\'' + url + '\',this)\"';
    }
    else if (openWay == "7")//弹出子页面
    {
        retStr = 'onclick=\"WindowSonOpen(\'' + title + '\',\'' + url + '\',\'' + width + '\',\'' + height + '\')\"';
    }
    else if (openWay == "10")//直接执行js方法时
    {
        retStr = 'onclick=\"CallFunName(\'' + $.trim(ev.Url) + '\',\'' + args + '\')\"';
        //retStr = 'onclick=\"CallFunName(\'' + $.trim(ev.Url) + '\',' + ['a', 'b', 'c'] + ')\"';
    }
    //#endregion

    return retStr;
}

//生成多表头(包含多表头事件)
function GenerateTopField(fieldArr, topFieldArr, outEventArr)
{
    //#region 得到当前多表头的深度-maxNum
    var maxNum = 1;//默认一层
    if (topFieldArr != null && topFieldArr.length > 0) {
        for (var i = 0; i < topFieldArr.length; i++) {
            var num = 1;//默认一层
            var parentName = topFieldArr[i].ParentName;//初始化父级名称
            while (parentName != null && parentName != "") {
                for (var j = 0; j < topFieldArr.length; j++) {
                    if (topFieldArr[j].Name == parentName) {
                        parentName = topFieldArr[j].ParentName;//赋父值
                        num++;
                        break;
                    }
                }
            }
            if (num > maxNum)//当前元素深度大于之前的值时，重新赋值
            {
                maxNum = num;
            }
        }
    }
    else { return;}
    //#endregion

    var colHeadArr = [];
    //fieldArr = fieldArr.sort(compare("OrderNum"));//根据字段序号排序-降序
    //#region 生成colHeadArr
    if (fieldArr != null && fieldArr.length > 0)//如果没有多表头topFieldArr为空，则从fieldArr入手
    {
        for (var i = 0; i < fieldArr.length; i++) {
            var fd = fieldArr[i];
            colHeadArr.push(fd.FieldCode);//hot设置表头名
        }
    }
    //#endregion

    //#region 循环生成多表头的行数组-topRowArr
    var topRowArr = [];

    //从第二层开始生成
    for (var i = 0; i < maxNum - 1; i++) {
        var topRow = new Array(colHeadArr.length);
        //有可能出现中文别名调整情况，故获取第二行数据依据FieldCode来查找
        if (i == 0) {
            for (var k = 0; k < colHeadArr.length; k++) {
                topRow[k] = "";//值初始化为空
                for (var j = 0; j < topFieldArr.length; j++) {
                    var top = topFieldArr[j];
                    if (top.FieldCode == colHeadArr[k]) {
                        topRow[k] = top.ParentName;
                        break;
                    }
                    //if (j == topFieldArr.length - 1) {
                    //    topRow[k] = "";//如果没有父级则赋空值
                    //}
                }
            }
            topRowArr.push(topRow);//添加
        }
        else {
            //根据上一行设置当前行
            var beforeTop = topRowArr[i-1];
            for (var k = 0; k < colHeadArr.length; k++) {
                topRow[k] = "";//值初始化为空
                for (var j = 0; j < topFieldArr.length; j++) {
                    var top = topFieldArr[j];
                    if (top.Name == beforeTop[k]) {
                        topRow[k] = top.ParentName;//第二行的属性赋值
                        break;
                    }
                    //if (j == topFieldArr.length - 1) {
                    //    topRow[k] = "";//如果没有父级则赋空值
                    //}
                }
            }
            topRowArr.push(topRow);
        }
    }
    //#endregion

    topRowArr.reverse();//将数组倒序

    //生成表格合并

    if(topRowArr!=null&&topRowArr.length>0)
    {
        //从第一行开始生成合并项
        for (var i = 0; i < topRowArr.length; i++) {
            var groupHeadArr = [];//初始化当前行的合并集合

            var row = topRowArr[i];//当前行

            for (var j = 0; j < colHeadArr.length; j++) {
                var startColName = "";//初始化起始列
                var colNum = 0;//初始化合并列数
                var end_j = 0;

                if (row[j] != null && row[j] != "") {
                    startColName = colHeadArr[j];
                    end_j = j;

                    for (var k = j; k < colHeadArr.length; k++) {
                        if (row[k] == row[j]) {
                            end_j++;
                        }
                    }
                    colNum = end_j - j;//合并列数

                    //获得当前表头的事件信息-fieldEvLb
                    var fieldEvLb = null;
                    if (outEventArr != null && outEventArr.length > 0) {
                        for (var g = 0; g < outEventArr.length; g++) {
                            var ev = outEventArr[g];
                            if (ev.EventType == 5 && ev.FieldCode == row[j]) {
                                //获取表头事件标签（a/button）
                                fieldEvLb = OutEventForLabel(ev, row[j], null, null,null);
                            }
                        }
                    }

                    fieldEvLb = (fieldEvLb == null ? row[j] : fieldEvLb);
                    var ghead = { "numberOfColumns": colNum, "titleText": "" + fieldEvLb + "", "startColumnName": "" + startColName + "" };
                    groupHeadArr.push(ghead);
                    j = end_j - 1;//下一个起始列
                }
            }
            if (groupHeadArr != null && groupHeadArr.length > 0) {
                //设置当前行的合并项
                $('#jqGrid').setGroupHeaders(
                {
                    useColSpanStyle: true,
                    groupHeaders: groupHeadArr
                });
            }
        }
    }
}

//根据筛选条件查询数据
function DoSearch() {
    var tbreport = GetTbReport();//获取当前PC端表格报表
    if (tbreport != "" && tbreport != null) {
        var queryParamArr = GetQueryParamArr(tbreport);//获得查询参数集合列表
       
        //清空表格数据重新加载新数据
        $("#jqGrid").jqGrid('clearGridData');  //清空表格
        $("#jqGrid").jqGrid('setGridParam', {  // 重新加载数据
            url: 'TbQueryList',
            postData: {
                code: $("#Code").val(),
                queryParams: JSON.stringify(queryParamArr)//查询内容
            },
            mtype: "POST",
            datatype: 'json',
            page: 1
        }).trigger("reloadGrid");
        $(window).resize();//刷新格式
    }
}

//获得查询参数集合列表
function GetQueryParamArr(tbreport)
{
    var queryParamArr = [];//初始化得到的查询集合

    //先获得原筛选配置信息
    if (tbreport.FilterListJson != null && tbreport.FilterListJson != "" && tbreport.FilterListJson != "[]") {
        var filterArr = $.parseJSON(tbreport.FilterListJson);
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
                            FilterType:""
                        };//初始化
                        param.FieldCode = ft.FieldCode;
                        param.FieldParam = ft.FieldParam;
                        param.FilterType = ft.FilterType;//筛选类型
                        //当不为自定义时赋值
                        if (!ft.IsCustom) {
                            param.OpType = $("#cond_" + ft.FieldParam).val();//获取条件下拉值
                        }
                        param.Value =$.trim(paramValue);//获取控件值(去掉前后空格)
                        param.DataType = ft.DataType;//字段类型
                        queryParamArr.push(param);//添加筛选到集合
                    }
                }
            }
        }
    }

    //将在url中设置的参数添加到参数列表中.其中,若在url的参数名等于查询区域获得的参数名 则以查询区的为主
    var defPars = $("#KVJson").val();
    if (defPars != null && defPars != "")
    {
        parArr = $.parseJSON(defPars);
        //遍历替换在url中设置的默认值
        for (var i = 0; i < parArr.length; i++) {
            var par = parArr[i];
            if (queryParamArr != null && queryParamArr.length > 0) {
                for (var j = 0; j < queryParamArr.length; j++) {
                    if (par.K == queryParamArr[j].FieldParam) {
                        //不处理
                    }
                    else if (j == queryParamArr.length - 1 && par.K != "time_stamp") {
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
            else if (par.K != "time_stamp")
            {
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
    var tbreport = GetTbReport();//获取当前PC端表格报表
    //先获得原筛选配置信息
    if (tbreport.FilterListJson != null && tbreport.FilterListJson != "" && tbreport.FilterListJson != "[]") {
        var filterArr = $.parseJSON(tbreport.FilterListJson);
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

//设置JqGrid表头背景
var SetJqGridHtableBkBroundColor = function (strColor) {
    if ($.trim(strColor) == "")
    {
        strColor = "#f5f6fa";
        return;
    }
    $(".ui-jqgrid-htable,.ui-jqgrid-labels").css("background-color", strColor);
}

var GetModelIoc = function () {
    return "<i class='fa fa-cog fa-spin fa-3x fa-fw'></i>";
}
