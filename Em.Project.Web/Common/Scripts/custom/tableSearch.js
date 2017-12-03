$(function () {
    divSearch();
    varButSearch();
});

//查询框
var divSearch = (function () {
    var divRow = $(".ztable-filter-container");
    divRow.children(".ztable-tool-div").css("width", "100%");
    var butRow = $("<div class='ztable-filter-div left' />");
    $(".btn-search").appendTo(butRow);
    butRow.appendTo(divRow);
    var selectFormDiv = $("<div class='collapse' id='searchDiv'/>");
    $(".ztable-filter-div").appendTo(selectFormDiv);
    selectFormDiv.appendTo(divRow);
});
 
//查询按钮
var varButSearch = (function () {
    varButSH = $("<button type='button' style='float:right' class='btn btn-primary' data-toggle='collapse' data-target='#searchDiv' aria-expanded='false' aria-controls='collapseFilter'>查询</button>");
    var divTool = $(".ztable-tool-div");
    varButSH.appendTo(divTool);
});

