var table;
$(document).ready(function () {
   
    InitPage();
    InitEvent();
})

//#endregion

//#region 初始化页面

function InitPage() {
    table = $('#dataTableDiv').ztable({
        action: "../api/services/api/Script/GetAllExampleScriptSearch",
        pageSize: 10,
        fields: {
            Id: {
                title: "标识",
                filter: false,
                type: "number",
                width: "100px",
                textAlign: "center",

            },
            name: {
                title: "脚本流实例名称",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            scripT_ID_STR: {
                title: "脚本流名称",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            retrY_TIME: {
                title: "允许重试次数",
                filter: true,
                type: "number",
                textAlign: "center"
            },
            starT_TIME_STR: {
                title: "启动时间",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            starT_MODEL_STR: {
                title: "启动模式",
                filter: true,
                type: "number",
                textAlign: "center"
            },
            useR_ID_STR: {
                title: "启动人",
                filter: true,
                type: "string",
                textAlign: "center"
            },
            ruN_STATUS_STR: {
                title: "运行状态",
                filter: false,
                type: "string",
                order: false,
                textAlign: "center",
            },
            iS_HAVE_FAIL_STR: {
                title: "是否有失败节点",
                filter: false,
                type: "string",
                order: false,
                textAlign: "center",
            },
            returN_CODE_STR: {
                title: "结束标识",
                filter: false,
                type: "string",
                order: false,
                textAlign: "center",
            },
            enD_TIME_STR: {
                title: "结束时间",
                filter: false,
                type: "string",
                order: false,
                textAlign: "center",
            },
            op: {
                title: "操作",
                order: false,
                width: "120px",
                textIsHtml: true,
                textAlign: "center",
                template: "<a><i onclick='onsign(<%Id%>)' class='fa fa-search ' /> </a> "
            }
        }
    });
}

//#endregion 

//#region 初始化事件

function InitEvent() {

}

function onsign(id) {
    //DiyModal.window({
    //    title: "查看当前实例",
    //    url: bootPATH +"/Script/SelectScriptExample?ExampleId=" + id,
    //    width: 850,
    //    height: 550,
    //    fullscreen: false,
    //    afterClose: function () {
    //        table.reload();
    //    }
    //}).open();
    window.location.href = "/Script/SelectScriptExample?ExampleId=" + id;
}
