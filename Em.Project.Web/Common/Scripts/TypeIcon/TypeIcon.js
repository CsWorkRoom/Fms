
//加载页面
function TopTypeIcons() {
    var list = function (datas, iconsDatas) {
        //循环图标类型 拼接li
        for (var i = 0 ; i < datas.length; i++) {
            var data = datas[i];
            if (i == 0) {
                //class='active'默认选择
                $("#typeIconul").append("<li role='presentation1' class='active' ><a href='#divType" + data.id + "' aria-controls='home' role='tab' data-toggle='tab'>" + data.value + "</a></li>");
            } else {
                $("#typeIconul").append("<li role='presentation1' ><a href='#divType" + data.id + "' aria-controls='home' role='tab' data-toggle='tab'>" + data.value + "</a></li>");
            }
        }
        //循环图标类型，一个类型对应一个显示层
        for (var i = 0 ; i < datas.length; i++) {
            //根据类型获取数据
            var data = datas[i];
            var liCount = 0;//当前类型记录条数
            //循环判断图标 ，类型相同就加入
            var divIconHtml = "";
            for (var m = 0; m < iconsDatas.length; m++) {
                //在每个分页里面填充数据
                if (data.id == iconsDatas[m]["typeId"]) {
                    // 这里显示图标，定义样式
                    divIconHtml += "<div style='display:none;' class='padding_icon' id='divicon" + liCount + "' title='" + iconsDatas[m]["type"] + "'><span  onclick='ContentIconType(\"" + iconsDatas[m]["value"] + "\")'><i class='" + iconsDatas[m]["value"] + "'></i></span></div>";
                    liCount++;
                }
            }
            //加入层,数据显示
            if (i == 0) {
                $("#divHtmlTop").append("<div role='tabpanel' class='tab-pane active' id='divType" + data.id + "'> " + divIconHtml + "</div>");
                //在对应的层显示页数                        
            } else {
                $("#divHtmlTop").append("<div role='tabpanel' class='tab-pane' id='divType" + data.id + "'> " + divIconHtml + "</div>");
            }
            //分页处理
            var PageSize = 32;//设置每页，你准备显示几条
            //计算总页数
            var PageCount = Math.ceil(liCount / PageSize);
            var currentPage = 1;//当前页，默认为1  
            //获取一个层下面的层的显示或者隐藏
            //$("#parentDiv").find("#childDiv").hide() or show();
            //在每个类型层里添加一个放置分页数字的层
            $("#divType" + data.id + "").append("<div class='icon-div-a' id='aicondivType" + data.id + "' ></div>");
            //造个简单的分页按钮  
            for (var a = 1; a <= PageCount; a++) {
                var pageN = '<div class="pageDivCount"><a href="#" iconimgid="' + data.id + '" selectPage="' + a + '" >' + a + '</a></div>';
                $("#divType" + data.id + "").find("#aicondivType" + data.id + "").append(pageN);
            }
            //显示默认页（第一页）  注意：是每个类型层的第一页
            for (var b = 0; b < 1 * PageSize; b++) {
                //隐藏的层显示到第几个层，根据ID
                $("#divType" + data.id + "").find("#divicon" + b + "").show();
            }
            $('a').click(function () {
                //显示a页对应的层,根据a*PageSize的ID来显示
                var selectPage = $(this).attr('selectPage');
                var Pageid = $(this).attr('iconimgid');
                var aPage = (selectPage - 1) * PageSize;
                //隐藏类型层下面所有数据

                $("#divType" + Pageid + "").find(".padding_icon").hide();

                for (var b = aPage; b < selectPage * PageSize; b++) {
                    //隐藏的层显示到第几个层，根据ID
                    $("#divType" + Pageid + "").find("#divicon" + b + "").show();
                }
            });
        }
    }
    $.post(bootPATH + "/api/services/api/Icon/GetAllTypeIcons", {}, function (data) {
        //console.log(data);
        $.post(bootPATH + "/api/services/api/Icon/GetAllIconsId", {}, function (dataicon) {
            //console.log(dataicon);
            //等待所有数据执行完成后 再调用方法 生成html
            list(data.result, dataicon.result);
        });

    });
}

//点击显示模态窗口，模态窗使用分部视图显示类型小图标
function MyModelTypeIcon(modelDiv) {
    var divicon = $(modelDiv);
    if (divicon.is(':hidden'))
        divicon.show();
    else
        divicon.hide();
}
