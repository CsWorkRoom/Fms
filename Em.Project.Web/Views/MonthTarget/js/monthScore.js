//#region 页面入口
$(document).ready(function () {
    InitGrid();
    $("#curMsg").val("当前打分的月份为【" + $("#Month").val() + "】");
})
//#endregion
//随着浏览器的变化而变化
$(window).resize(function () {
    WinReJqGridSize("navMenu", "jqGridList");
});

//初始化-加载表格内容
function InitGrid() {
    $.jgrid.gridUnload("jqGridList");//先卸载
    $("#jqGridList").jqGrid({
        url: 'QueryTargetDetailList',
        postData: {
            targetTagId: $("#TargetTagId").val(),
            month: $("#Month").val(),
            Kvs: JSON.stringify(GetKvArr()),
            districtId: $("#DistrictId").val(),
        },
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",//如果url中需要回调函数，则此处格式为jsonp
        //altRows: true,
        editurl: 'clientArray',
        responsive: false,
        page: 1,
        colModel: [
            { label: '编号', name: 'Id', width: 50, key: true, editable: false },
             {
                 label: '月份',
                 name: 'Month',
                 width: 100,
                 editable: false
             },
             {
                 label: '归属ID',
                 name: 'DistrictId',
                 width: 100,
                 editable: false,
                 hidden: true
             },
             {
                 label: '归属',
                 name: 'DistrictName',
                 width: 100,
                 editable: false
             },
              {
                  label: '指标标识ID',
                  name: 'TargetTagId',
                  width: 50,
                  editable: false,
                  hidden: true
              },
              {
                  label: '指标标识',
                  name: 'TargetTagName',
                  width: 130,
                  editable: false
              },
             {
                 label: '指标类型ID',
                 name: 'TargetTypeId',
                 width: 50,
                 editable: false,
                 hidden: true
             },
             {
                 label: '指标类型',
                 name: 'TargetTypeName',
                 width: 130,
                 editable: false
             },
              {
                  label: '指标ID',
                  name: 'TargetId',
                  width: 50,
                  editable: false,
                  hidden: true
              },
              {
                  label: '指标名',
                  name: 'TargetName',
                  width: 130,
                  editable: false,
              },
              {
                  label: '客户经理工号',
                  name: 'ManagerNo',
                  width: 130,
                  editable: false,
              },
              {
                  label: '客户经理名',
                  name: 'ManagerName',
                  width: 130,
                  editable: false,
              },
               {
                   label: '指标权重分',
                   name: 'Weight',
                   width: 100,
                   editable: false,
               },
                {
                    label: '指标目标值',
                    name: 'TValue',
                    width: 100,
                    editable: false,
                },
                  {
                      label: '指标完成值',
                      name: 'ResultValue',
                      width: 100,
                      editable: false,
                  },
                {
                    label: '指标得分',
                    name: 'Score',
                    width: 100,
                    editable: false,
                },
                {
                    label: '得分比重',
                    name: 'ScoreWeight',
                    width: 100,
                    editable: false,
                },
                 {
                     label: '领导打分',
                     name: 'MarkScore',
                     width: 150,
                     editable: true,
                     //editrules: { required: true, number: true, custom: true, custom_func: ValidateTvalue },
                     edittype: "text"
                 },
                 {
                     label: '最终得分',
                     name: 'EndScore',
                     width: 100,
                     editable: false,
                 },
                  {
                      label: '标识当前行状态',
                      name: 'Status',
                      width: 50,
                      editable: false,
                      hidden: true
                  },
        ],
       shrinkToFit: true,//是否列宽度自适应。true=适应 false=不适应
        loadonce: false,
        viewrecords: true,
        onSelectRow: EditSelectRow
        //rowNum: fieldJson.length,
    });
    //startEdit($("#jqGridList"));
       $(window).resize();
}

//选中行启用行编辑
function EditSelectRow(id) {
    var result = ValidateTvalue();
    if (result != "" && result.length > 0) {
        abp.message.error(result);
        return;
    }

    SaveOneScore();//保存上一行数据

    //当前选中行
    $("#selectRowId").val(id);//临时存储当前选中行
    //设置当前行编辑状态
    $("#jqGridList").setCell(id, 'Status', "1|0");
    //启用当前行为编辑状态
    //$("#jqGridList").jqGrid('editRow', id, { keys: true, focusField: 17 });
    $("#jqGridList").jqGrid('editRow', id, { keys: true });
}

//查询
function DoSearch() {
    SaveTargetScore();//先保存一下
    //清空表格数据重新加载新数据
    $("#jqGridList").jqGrid('clearGridData');  //清空表格
    $("#jqGridList").jqGrid('setGridParam', {  // 重新加载数据
        url: 'QueryTargetDetailList',
        postData: {
            targetTagId: $("#TargetTagId").val(),
            month: $("#Month").val(),
            Kvs: JSON.stringify(GetKvArr()),
            districtId: $("#DistrictId").val()
        },
        mtype: "POST",
        datatype: 'json',
        page: 1
    }).trigger("reloadGrid");
    $("#selectRowId").val("");//值设为空
}

//#region 子方法

//获得筛选项
function GetKvArr() {
    var kvArr = [];
    var TargetTypeId = $("#TargetTypeId").val();//指标类型
    var TargetId = $("#TargetId").val();//指标
    var ManagerNo = $("#ManagerNo").val();//客户经理工号
    var ManagerName = $("#ManagerName").val();//客户经理名
    if (TargetTypeId != "全部" && TargetTypeId != "" && TargetTypeId != null) {
        var kv = {
            K: "targetTypeId",
            V: TargetTypeId
        };
        kvArr.push(kv);
    }
    if (TargetId != "全部" && TargetId != "" && TargetId != null) {
        var kv = {
            K: "targetId",
            V: TargetId
        };
        kvArr.push(kv);
    }
    kvArr.push({
        K: "managerNo",
        V: ManagerNo
    });
    kvArr.push({
        K: "managerName",
        V: ManagerName
    });
    return kvArr;
}

//自定义验证 value=输入控件的值，name=列名称（来自colModel）
function ValidateTvalue() {
    var result = "";
    var oldSelectRowId = $("#selectRowId").val();
    if (oldSelectRowId != null && oldSelectRowId != "" && oldSelectRowId.length > 0) {
        $("#jqGridList").jqGrid('saveRow', oldSelectRowId);//保存上一行
        var rowDatas = $("#jqGridList").jqGrid('getRowData', oldSelectRowId);//获取上一行数据

        //#region 验证分数是否为数值
        var regu = "^[0-9]+(.[0-9]{2})?$";
        //var regu = "/^\+?(\d*\.\d{2})$/";
        var re = new RegExp(regu);
        if (re.test(rowDatas.MarkScore)) {
            //return [true, ""];
        }
        else {
            //return [false, "分数【" + rowDatas.MarkScore + "】错误，请输入数值型.如：12或12.23"];
            //abp.message.error("分数【" + rowDatas.MarkScore + "】错误，请输入数值型.如：12或12.23");
            result = "打分值【" + rowDatas.MarkScore + "】错误，请输入数值型.如：12或12.23";
            $("#jqGridList").jqGrid('editRow', oldSelectRowId, { keys: true, focusField: 17 });
        }
        //#endregion

        //#region 验证分数的范围值
        var MarkScore = rowDatas.MarkScore - 0;//打分
        var Weight = rowDatas.Weight - 0;//权重分
        if (MarkScore >= 0 && MarkScore <= Weight) {
            //return [true, ""];
        }
        else {
            //return [false, "打分值应在【" + 0 + "~" + rowDatas.Weight + "】范围内"];
            //abp.message.error("打分值应在【" + 0 + "~" + rowDatas.Weight + "】范围内");
            result = "打分值【" + rowDatas.MarkScore + "】应在【" + 0 + "~" + rowDatas.Weight + "】范围内";
            $("#jqGridList").jqGrid('editRow', oldSelectRowId, { keys: true, focusField: 17 });
        }
        //#endregion

    }
    return result;
}

//保存上一条信息
function SaveOneScore() {
    //原选中行ID
    var oldSelectRowId = $("#selectRowId").val();
    if (oldSelectRowId != null && oldSelectRowId != "" && oldSelectRowId.length > 0) {
        $("#jqGridList").jqGrid('saveRow', oldSelectRowId);//保存上一行
        var rowDatas = $("#jqGridList").jqGrid('getRowData', oldSelectRowId);//获取上一行数据
        //计算当前指标最终得分
        var endScore = CalculateScore(oldSelectRowId, rowDatas);
        //设置打分后的单元格值
        $("#jqGridList").setCell(oldSelectRowId, 'EndScore', endScore);
        $("#jqGridList").setCell(oldSelectRowId, 'Status', "1|1");//修改当前行编辑状态

        //异步保存打分和得分
        $.ajax({
            type: "post",
            url: "../api/services/api/MonthBonus/SaveEndScore",
            data: {
                DetailId: rowDatas.Id,
                MarkScore: rowDatas.MarkScore,
                EndScore: endScore
            },
            success: function (e) {
                //abp.message.success("", "打分成功！");
            },
            error: function (e) {
                if (e.responseText.indexOf("<title>") != -1) {
                    var start = e.responseText.indexOf("<title>");
                    var end = e.responseText.indexOf("</title>");
                    abp.message.error(e.responseText.substring(start + 7, end), rowDatas.TargetName + "编号【" + rowDatas.Id.toString() + "】打分失败");
                }
                else
                    abp.message.error(e.error.message, rowDatas.TargetName + "编号【" + rowDatas.Id.toString() + "】打分失败");
            }
        });
    }
}

//计算总得分
function CalculateScore(rowid, rowDatas) {
    var endScore = 0;
    if (rowDatas != null) {
        //由于jqgrid中呈现的是有效的指标，故不在此处过滤IsUse为false的指标

        //#region 以下通过js变量弱类型转换为数值型
        var Score = rowDatas.Score - 0;
        var ScoreWeight = rowDatas.ScoreWeight - 0;
        var MarkScore = rowDatas.MarkScore - 0;
        //#endregion
        endScore = Score * ScoreWeight + MarkScore * (1 - ScoreWeight);
    }
    return endScore;
}

//手动点击保存（面向于最后一条或只有一行数据的情况）
function SaveTargetScore() {
    SaveOneScore();
    $("#selectRowId").val("");//值设为空
}

//#endregion