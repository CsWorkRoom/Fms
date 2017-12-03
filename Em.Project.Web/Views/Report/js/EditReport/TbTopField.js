//#region 多表头管理代码块

//多表头中相关控件的初始化应在专门的方法中实现
//加载多表头信息
function InitTbTopField() {

    var topFieldArr = [];//初始化多表头
    var fieldArr = [];//初始化字段信息

    //#region 给多表头topFieldArr和字段fieldArr赋值

    //此处假定已在主初始化方法中完成了对所有相关控件含隐藏控件的赋值
    //此处直接拿来用
    saveRows($("#fieldGrid"));
    var fields = JSON.stringify($("#fieldGrid").getRowData());//获取字段信息

    var topFields = $("#tbTopFieldJson").val(); //获取多表头隐藏控件信息
    if (topFields != null && topFields != "" && topFields != "[]") {
        topFieldArr = $.parseJSON(topFields);
    }
    if (fields != null && fields != "" && fields != "[]") {
        fieldArr = $.parseJSON(fields);
    }

    //#endregion

    //给handsontabel赋值并加载
    LoadHandsonTabel(topFieldArr, fieldArr);
}

//根据字段信息和多表头信息加载hot
//topFieldArr:多表头，fieldArr:字段信息
function LoadHandsonTabel(topFieldArr, fieldArr) {
    //当没有多表头时，需要拼凑一个基础表头

    var data = [];//data值
    var colHeadArr = [];//列

    fieldArr = fieldArr.sort(compare("OrderNum"));//根据字段序号排序-升序


    //#region 生成colHeadArr
    if (fieldArr != null && fieldArr.length > 0)//如果没有多表头topFieldArr为空，则从fieldArr入手
    {
        var frozenArr = [];
        for (var i = 0; i < fieldArr.length; i++) {
            var fd = fieldArr[i];
            //已经具有多表头时,不考虑冻结影响
            if (topFieldArr != null && topFieldArr.length > 0) {
                if (fd.IsShow) {
                    colHeadArr.push(fd.FieldCode);//hot设置表头名
                }
            }
            else {
                if (fd.IsFrozen == "true")//冻结单独拼一个数组
                {
                    frozenArr.push(fd.FieldCode);//hot设置表头名-冻结列
                }
                else if (fd.IsShow == "true")//非冻结并显示的字段
                {
                    colHeadArr.push(fd.FieldCode);//hot设置表头名
                }
            }
        }
        //得到最终的colhead
        colHeadArr = frozenArr.concat(colHeadArr);

        //colHeadArr = frozenArr.push.apply(frozenArr, colHeadArr);//在frozenArr后加入colHeadArr
    }
    //#endregion

    //生成data

    var cols = [];
    //#region 生成列头cols+第一行data
    //生成字段信息的行对象，并添加到data,作为第一行数据
    if (colHeadArr != null && colHeadArr.length > 0) {
        var js = '{';
        for (var i = 0; i < colHeadArr.length; i++) {
            for (var j = 0; j < fieldArr.length; j++) {
                if (colHeadArr[i] == fieldArr[j].FieldCode) {
                    js += '"' + fieldArr[j].FieldCode + '":' + '"' + fieldArr[j].FieldName + '",';
                    break;
                }
            }
            cols.push({ data: "" + colHeadArr[i] + "" });
        }
        js = js.substring(0, js.length - 1) + '}';
        var fdObj = $.parseJSON(js);
        data.push(fdObj);//添加最低行（字段）
    }
    //#endregion

    //处理多表头信息

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
    //#endregion


    //#region 根据多表头深度，得到其他行的数据data

    //从第二层开始生成
    for (var i = 0; i < maxNum - 1; i++) {
        var curData = cloneObj(data[0]);//深克隆一个对象
        //有可能出现中文别名调整情况，故获取第二行数据依据FieldCode来查找
        if (i == 0) {
            for (var k = 0; k < colHeadArr.length; k++) {
                for (var j = 0; j < topFieldArr.length; j++) {
                    var top = topFieldArr[j];
                    if (top.FieldCode == colHeadArr[k]) {
                        curData[colHeadArr[k]] = top.ParentName;//第二行的属性赋值
                        break;
                    }

                    if (j == topFieldArr.length - 1) {
                        curData[colHeadArr[k]] = "";//如果没有父级则赋空值
                    }
                }
            }
            data.push(curData);//添加第二行
            //data.unshift(curData);//添加第二行（添加在数组头部）
        }
        else {
            //根据上一行设置当前行
            var beforeTop = data[i];
            for (var k = 0; k < colHeadArr.length; k++) {
                for (var j = 0; j < topFieldArr.length; j++) {
                    top = topFieldArr[j];
                    if (top.Name == beforeTop[colHeadArr[k]]) {
                        curData[colHeadArr[k]] = top.ParentName;//第二行的属性赋值
                        break;
                    }
                    if (j == topFieldArr.length - 1) {
                        curData[colHeadArr[k]] = "";//如果没有父级则赋空值
                    }
                }
            }
            data.push(curData);//添加第二行
            //data.unshift(curData);//添加第二行（添加在数组头部）
        }
    }
    //#endregion

    data.reverse();//将数组倒序

    //根据data设置mergeCells--放弃。放弃动态解析合并表头内容，改由直接取合并的数据再做绑定。
    var setmergeCells = true;
    var mergeCel = $("#tbTopMerge").val();
    if (mergeCel != null && mergeCel != "") {
        setmergeCells = $.parseJSON(mergeCel);
    }

    //由于直接在原承载handsontabel的div上赋值会出现列顺序不可控情况，
    //而本功能控制字段列的顺序是必需的。故采用以下方式
    //#region 先删除handsontable的承载div，再添加
    ////给hot绑定数据，并根据多表头信息合并单元格
    var $container = $("#topFieldDiv");
    //判断div是否存在，若存在则删除
    if ($container.length > 0) {
        $container.remove();
    }
    //添加handsontable的div
    $("#tbTopFieldJson").after("<div id=\"topFieldDiv\" style=\"width: 100%; height: " + window.innerHeight * 0.6 + "px; overflow-y: hidden;overflow-x: hidden;display:none \"></div>");
    //#endregion

    $("#topFieldDiv").handsontable({
        data: data,
        colHeaders: colHeadArr,//设置列头
        manualRowResize: true,//自定义行宽
        manualColumnResize: true,//自定义列高
        manualColumnMove: true,//是否能拖动列
        //manualRowMove: false,//是否能拖动行
        columnSorting: false,//false/对象 //当值为true时，表示启用排序插件
        //rowHeaders: false,//是否显示行数字
        contextMenu: true,//右键显示更多功能,
        columns: cols,
        autoColumnSize: true,
        mergeCells: setmergeCells //加载合并项
    });

    //注释以下语句，改成上面的handsontable中直接去赋值data
    //以下语句会造成：最后一个列在往前拖动过程中会消失(┬＿┬)
    $("#topFieldDiv").handsontable("loadData", data);//加载
}

//保存多表头配置信息
function SaveTopField() {

    var $container = $("#topFieldDiv");
    var handsontable = $container.data('handsontable');//获取当前handsontable

    var data = handsontable.getData();//获取所有值
    var mergeCellArr = handsontable.mergeCells.mergedCellInfoCollection;//获取合并项目
    var colHeadArr = handsontable.getColHeader();//获取表头集合

    //#region 根据colHeadArr修改字段信息的顺序，然后重新加载字段表格
    saveRows($("#fieldGrid"));
    var fieldArr = $("#fieldGrid").getRowData();//获得字段集合
    if (colHeadArr != null && colHeadArr.length > 0) {
        for (var i = 0; i < colHeadArr.length; i++) {
            if (fieldArr != null && fieldArr.length > 0) {
                for (var j = 0; j < fieldArr.length; j++) {
                    if (fieldArr[j].FieldCode == colHeadArr[i]) {
                        fieldArr[j].OrderNum = i + 1;//赋值排序
                    }
                }
            }
        }
        LoadTbGrid(fieldArr);//再次加载表格
    }
    //#endregion

    //#region 解析合并项，生成多表头信息并存入隐藏域
    var topFieldArr = [];
    //#region 生成多表头信息 topFieldArr
    if (data != null && data.length > 0) {
        for (var i = data.length - 1; i >= 0; i--) {
            for (var j = 0; j < data[0].length; j++) {
                var top = {
                    ParentName: "",
                    Name: "",
                    FieldCode: ""
                };
                if (i == data.length - 1)//最后行=字段，加入字段编码的赋值
                {
                    top.FieldCode = colHeadArr[j];//字段编码赋值
                    top.Name = data[i][j];
                    var parentName = GetParentName(i - 1, j, mergeCellArr, handsontable);//获取上级单元格合并项值

                    if (parentName != null && parentName != "")//找到父级
                    {
                        top.ParentName = parentName;
                        topFieldArr.push(top);//添加一个多表头信息（含字段本身）
                    }
                }
                else//多表头时
                {
                    if (IsMergeMainCell(i, j, mergeCellArr)) {
                        top.Name = handsontable.getDataAtCell(i, j);
                        //获取其父级
                        var parentName = GetParentName(i - 1, j, mergeCellArr, handsontable);//获取上级单元格合并项值

                        top.ParentName = parentName;
                        topFieldArr.push(top);//添加一个多表头信息
                    }
                }
            }
        }
    }
    //#endregion
    if (topFieldArr != null && topFieldArr.length > 0) {
        $("#tbTopFieldJson").val(JSON.stringify(topFieldArr));
    }
    //#endregion

    //保存合并项至隐藏域
    $("#tbTopMerge").val(JSON.stringify(mergeCellArr));
    abp.message.success("数据已提交成功！但未保存，请即时保存！", "信息提示");
}

//处理合并项（将row==-1的项）--- 被废弃（由于已经在原生的remove_row方法中做了处理）
//删除row==-1的项目
//删除的合并项排除
function DealMergeCellData(mergeCellArr) {
    if (mergeCellArr != null && mergeCellArr.length > 0) {
        for (var i = mergeCellArr.length - 1; i >= 0; i--) {
            var cell = mergeCellArr[i];
            if (cell.row == -1) {
                mergeCellArr.splice(i, 1);
            }
        }
    }
}

//判断是否为合并单元格主项
function IsMergeMainCell(i, j, mergeCellArr) {
    var bool = false;

    if (mergeCellArr != null && mergeCellArr.length > 0) {
        for (var k = 0; k < mergeCellArr.length; k++) {
            var curMerCell = mergeCellArr[k];
            if (i == curMerCell.row && j == curMerCell.col) {
                bool = true;
                break;
            }
        }
    }
    return bool;
}

//获取单元格的父级
function GetParentName(i, j, mergeCellArr, handsontable) {
    var parentName = "";
    if (mergeCellArr != null && mergeCellArr.length > 0) {
        for (var k = 0; k < mergeCellArr.length; k++) {
            var curMerCell = mergeCellArr[k];
            if (curMerCell.row <= i && i <= (curMerCell.row + curMerCell.rowspan - 1) &&
                curMerCell.col <= j && j <= (curMerCell.col + curMerCell.colspan - 1)) {
                parentName = handsontable.getDataAtCell(curMerCell.row, curMerCell.col);
                break;
            }
        }
    }
    return parentName;
}
//#endregion
