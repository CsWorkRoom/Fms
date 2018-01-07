function LoadFieldTab() {
    $("#fieldGrid").setGridWidth($(".modal-dialog").width() - 60);
}

//加载表格报表的字段信息
function LoadTbGrid(fieldJson) {
    $.jgrid.gridUnload("fieldGrid");//先卸载
    fieldJson = fieldJson.sort(compare("OrderNum"));//按照OrderNum升序排序
    winWidth = $("#tbField").width();

    $("#fieldGrid").jqGrid({
        //altRows: true,
        data: fieldJson,
        editurl: 'clientArray',
        styleUI: 'Bootstrap',
        responsive: true,
        datatype: "local",
        page: 1,
        colModel: [
            //{ label: '字段编码', name: 'FieldCode', key: true, width: 75,hidden:true },
            { label: '字段编码', name: 'FieldCode', key: true, width: 180, editable: false },
             {
                 label: '字段名称',
                 name: 'FieldName',
                 width: 150,
                 editable: true,
                 edittype: "text",
                 editrules: { required: true }
             },
              {
                  label: '字段类型',
                  name: 'DataType',
                  width: 10,
                  hidden: true,
                  editable: true,
                  //edittype: "select",
                  //editoptions: {
                  //    value: "Decimal:数值型;String:字符串;Int32:整型;Int64:长整型;Int16:短整型;DateTime:时间"
                  //}
              },
              {
                  label: '是否排序',
                  name: 'IsOrder',
                  width: 80,
                  editable: true,
                  edittype: "select",
                  editoptions: {
                      //value: "true:是;false:否"
                      value: "true:true;false:false"
                  }
              },
               {
                   label: '是否显示',
                   name: 'IsShow',
                   width: 80,
                   editable: true,
                   edittype: "select",
                   editoptions: {
                       //value: "true:是;false:否"
                       value: "true:true;false:false"
                   }
               },
               {
                   label: '列宽',
                   name: 'Width',
                   width: 55,
                   editable: true,
                   editrules: { required: true, integer: true },
                   edittype: "text"
               },
                {
                    label: '是否查询',
                    name: 'IsSearch',
                    width: 80,
                    editable: true,
                    edittype: "select",
                    hidden: true,//隐藏此处是否查询属性
                    editoptions: {
                        //value: "true:是;false:否"
                        value: "true:true;false:false"
                    }
                },
                 {
                     label: '是否冻结',
                     name: 'IsFrozen',
                     width: 80,
                     editable: true,
                     edittype: "select",
                     editoptions: {
                         //value: "true:是;false:否"
                         value: "true:true;false:false"
                     }
                 },
                  {
                      label: '对齐方式',
                      name: 'Align',
                      width: 80,
                      editable: true,
                      edittype: "select",
                      editoptions: {
                          //value: "left:左对齐;center:居中;right:右对齐"
                          value: "left:left;center:center;right:right"
                      }
                  },
            { label: '字段序号', name: 'OrderNum', width: 70, editable: false },
                   {
                       label: '说明文字',
                       name: 'Remark',
                       width: 200,
                       editable: true,
                       //hidden:true,
                       edittype: "text",
                       editrules: { edithidden: true }
                   }


        ],
        //autoScroll: false,//当autoScroll和shrinkToFit均为false时，会出现行滚动条
        shrinkToFit: true,//是否列宽度自适应。true=适应 false=不适应
        loadonce: false,
        viewrecords: true,
        onSelectRow: EditSelectRow,
        height: window.innerHeight * 0.6,
        width: ($(".modal-body").width()-20),
        rowNum: fieldJson.length,
        pager: "#fieldGridPager"
    });

    //$('#fieldGrid').navGrid("#fieldGridPager", { edit: false, add: false, del: false, refresh: false, view: false });
    //$('#fieldGrid').inlineNav('#fieldGridPager',
    //            // the buttons to appear on the toolbar of the grid
    //            {
    //                edit: true,
    //                add: false,
    //                del: true,
    //                cancel: true,
    //                editParams: {
    //                    keys: true,
    //                },
    //                addParams: {
    //                    keys: true
    //                }
    //});

    //$("#fieldGrid").trigger("reloadGrid");//重新加载

    //$(".ui-row-ltr:odd").css("background-color", "#f9f9f9"); //为双数行表格设置背颜色素  

}

//选中行启用行编辑
function EditSelectRow(id)
{
    //原选中行ID
    var oldSelectRowId = $("#selectRowId").val();
    if (oldSelectRowId != null && oldSelectRowId != "" && oldSelectRowId.length > 0) {
        $("#fieldGrid").jqGrid('saveRow', oldSelectRowId);//保存上一行
    }

    //当前选中行
    $("#selectRowId").val(id);//临时存储当前选中行
    //$("#fieldGrid").jqGrid('editRow', id);
    $("#fieldGrid").jqGrid('editRow', id, { keys: true, focusField: 1 });
}
