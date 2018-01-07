(function ($) {
    $.learuntab = {
        requestFullScreen: function () {
            var de = document.documentElement;
            if (de.requestFullscreen) {
                de.requestFullscreen();
            } else if (de.mozRequestFullScreen) {
                de.mozRequestFullScreen();
            } else if (de.webkitRequestFullScreen) {
                de.webkitRequestFullScreen();
            }
        },
        showDownlist: function () {
            $(".treeview").mouseover(function () {
                $(this).children(".popover-menu").css("display", "block");
            });
            $(".treeview").mouseout(function () {
                $(this).children(".popover-menu").css("display", "none");
            });
            $(".treeview-down>li").mouseover(function () {
                $(this).find(".popover-menu-sub").css("display", "block").children("a.downitem").css("color", "#fff");
                $(this).parentsUntil(".popover-menu").children("a.downitem").css("color", "#fff")
            });
            $(".treeview-down>li").mouseout(function () {
                $(this).find(".popover-menu-sub").css("display", "none").children("a.downitem").css("color", "#475059");
                $(this).parentsUntil(".popover-menu").children("a.downitem").css("color", "");
            });
        },
        bottomShow: function () {
            $(".bottom-tabs>div.fa").click(function () {
                $(".bottom-tabs>ul").toggle("2000");
            })
        },
        exitFullscreen: function () {
            var de = document;
            if (de.exitFullscreen) {
                de.exitFullscreen();
            } else if (de.mozCancelFullScreen) {
                de.mozCancelFullScreen();
            } else if (de.webkitCancelFullScreen) {
                de.webkitCancelFullScreen();
            }
        },
        refreshTab: function () {
            var currentId = $('.page-tabs-content').find('.active').attr('data-id');
            var target = $('.LRADMS_iframe[data-id="' + currentId + '"]');
            var url = target.attr('src');
            target.attr('src', url).load(function () {});
        },
        activeTab: function () {
            var currentId = $(this).data('id');
            if (!$(this).hasClass('active')) {
                $('.mainContent .LRADMS_iframe').each(function () {
                    if ($(this).data('id') == currentId) {
                        $(this).show().siblings('.LRADMS_iframe').hide();
                        return false;
                    }
                });
                $(this).addClass('active').siblings('.menuTab').removeClass('active');
                $.learuntab.scrollToTab(this);
            }
        },
        closeOtherTabs: function () {
            $('.page-tabs-content').children("[data-id]").find('.fa-remove').parents('a').not(".active").each(function () {
                $('.LRADMS_iframe[data-id="' + $(this).data('id') + '"]').remove();
                $(this).remove();
            });
            $('.page-tabs-content').css("margin-left", "0");
        },
        closeTab: function () {
            var closeTabId = $(this).parents('.menuTab').data('id');
            var currentWidth = $(this).parents('.menuTab').width();
            if ($(this).parents('.menuTab').hasClass('active')) {
                if ($(this).parents('.menuTab').next('.menuTab').size()) {
                    var activeId = $(this).parents('.menuTab').next('.menuTab:eq(0)').data('id');
                    $(this).parents('.menuTab').next('.menuTab:eq(0)').addClass('active');

                    $('.mainContent .LRADMS_iframe').each(function () {
                        if ($(this).data('id') == activeId) {
                            $(this).show().siblings('.LRADMS_iframe').hide();
                            return false;
                        }
                    });
                    var marginLeftVal = parseInt($('.page-tabs-content').css('margin-left'));
                    if (marginLeftVal < 0) {
                        $('.page-tabs-content').animate({
                            marginLeft: (marginLeftVal + currentWidth) + 'px'
                        }, "fast");
                    }
                    $(this).parents('.menuTab').remove();
                    $('.mainContent .LRADMS_iframe').each(function () {
                        if ($(this).data('id') == closeTabId) {
                            $(this).remove();
                            return false;
                        }
                    });
                }
                if ($(this).parents('.menuTab').prev('.menuTab').size()) {
                    var activeId = $(this).parents('.menuTab').prev('.menuTab:last').data('id');
                    $(this).parents('.menuTab').prev('.menuTab:last').addClass('active');
                    $('.mainContent .LRADMS_iframe').each(function () {
                        if ($(this).data('id') == activeId) {
                            $(this).show().siblings('.LRADMS_iframe').hide();
                            return false;
                        }
                    });
                    $(this).parents('.menuTab').remove();
                    $('.mainContent .LRADMS_iframe').each(function () {
                        if ($(this).data('id') == closeTabId) {
                            $(this).remove();
                            return false;
                        }
                    });
                }
            }
            else {
                $(this).parents('.menuTab').remove();
                $('.mainContent .LRADMS_iframe').each(function () {
                    if ($(this).data('id') == closeTabId) {
                        $(this).remove();
                        return false;
                    }
                });
                $.learuntab.scrollToTab($('.menuTab.active'));
            }
            return false;
        },
        addTab: function () {
            var dataId = $(this).attr('data-id');
            var dataUrl = $(this).attr('href');
            var menuName = $.trim($(this).text());
            var flag = true;
            if (dataUrl == undefined || $.trim(dataUrl).length == 0) {
                return false;
            }
            $('.menuTab').each(function () {
                if ($(this).data('id') == dataUrl) {
                    if (!$(this).hasClass('active')) {
                        $(this).addClass('active').siblings('.menuTab').removeClass('active');
                        $.learuntab.scrollToTab(this);
                        $('.mainContent .LRADMS_iframe').each(function () {
                            if ($(this).data('id') == dataUrl) {
                                $(this).show().siblings('.LRADMS_iframe').hide();
                                return false;
                            }
                        });
                    }
                    flag = false;
                    return false;
                }
            });
            if (flag) {
                var str = '<a href="javascript:;" class="active menuTab" data-id="' + dataUrl + '">' + menuName + ' <i class="fa fa-remove"></i></a>';
                $('.menuTab').removeClass('active');
                var str1 = '<iframe class="LRADMS_iframe" id="iframe' + dataId + '" name="iframe' + dataId + '"  width="100%" height="100%" src="' + dataUrl + '" frameborder="0" data-id="' + dataUrl + '" seamless></iframe>';
                $('.mainContent').find('iframe.LRADMS_iframe').hide();
                $('.mainContent').append(str1);
                //$.loading(true);
                $('.mainContent iframe:visible').load(function () {
                    //$.loading(false);
                });
                $('.menuTabs .page-tabs-content').append(str);
                $.learuntab.scrollToTab($('.menuTab.active'));
            }
            return false;
        },
        scrollTabRight: function () {
            var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
            var tabOuterWidth = $.learuntab.calSumWidth($(".content-tabs").children().not(".menuTabs"));
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            var scrollVal = 0;
            if ($(".page-tabs-content").width() < visibleWidth) {
                return false;
            } else {
                var tabElement = $(".menuTab:first");
                var offsetVal = 0;
                while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).next();
                }
                offsetVal = 0;
                while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).next();
                }
                scrollVal = $.learuntab.calSumWidth($(tabElement).prevAll());
                if (scrollVal > 0) {
                    $('.page-tabs-content').animate({
                        marginLeft: 0 - scrollVal + 'px'
                    }, "fast");
                }
            }
        },
        scrollTabLeft: function () {
            var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
            var tabOuterWidth = $.learuntab.calSumWidth($(".content-tabs").children().not(".menuTabs"));
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            var scrollVal = 0;
            if ($(".page-tabs-content").width() < visibleWidth) {
                return false;
            } else {
                var tabElement = $(".menuTab:first");
                var offsetVal = 0;
                while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).next();
                }
                offsetVal = 0;
                if ($.learuntab.calSumWidth($(tabElement).prevAll()) > visibleWidth) {
                    while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                        offsetVal += $(tabElement).outerWidth(true);
                        tabElement = $(tabElement).prev();
                    }
                    scrollVal = $.learuntab.calSumWidth($(tabElement).prevAll());
                }
            }
            $('.page-tabs-content').animate({
                marginLeft: 0 - scrollVal + 'px'
            }, "fast");
        },
        scrollToTab: function (element) {
            var marginLeftVal = $.learuntab.calSumWidth($(element).prevAll()), marginRightVal = $.learuntab.calSumWidth($(element).nextAll());
            var tabOuterWidth = $.learuntab.calSumWidth($(".content-tabs").children().not(".menuTabs"));
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            var scrollVal = 0;
            if ($(".page-tabs-content").outerWidth() < visibleWidth) {
                scrollVal = 0;
            } else if (marginRightVal <= (visibleWidth - $(element).outerWidth(true) - $(element).next().outerWidth(true))) {
                if ((visibleWidth - $(element).next().outerWidth(true)) > marginRightVal) {
                    scrollVal = marginLeftVal;
                    var tabElement = element;
                    while ((scrollVal - $(tabElement).outerWidth()) > ($(".page-tabs-content").outerWidth() - visibleWidth)) {
                        scrollVal -= $(tabElement).prev().outerWidth();
                        tabElement = $(tabElement).prev();
                    }
                }
            } else if (marginLeftVal > (visibleWidth - $(element).outerWidth(true) - $(element).prev().outerWidth(true))) {
                scrollVal = marginLeftVal - $(element).prev().outerWidth(true);
            }
            $('.page-tabs-content').animate({
                marginLeft: 0 - scrollVal + 'px'
            }, "fast");
        },
        calSumWidth: function (element) {
            var width = 0;
            $(element).each(function () {
                width += $(this).outerWidth(true);
            });
            return width;
        },
        LeftMenu: function () {
            // 左边菜单在缩进状态滑动显示菜单
            $("#sidebar-menu .treeview").hover(
                function () {
                    var intTop = $(".navbar-fixed-top").height();
                    for (var i = 0; i < $("#sidebar-menu .treeview").length ; i++) {
                        if ($("#sidebar-menu .treeview").eq(i).html() != $(this).html()) {
                            intTop += $("#sidebar-menu .treeview").eq(i).height();
                        } else {
                            break;
                        }
                    }
                    $(this).children(".treeview-menu").css("display", "block");
                    $(this).children(".treeview-menu").css("position", "fixed");
                    $(this).children(".treeview-menu").css("top", intTop);
                    $(this).children(".treeview-menu").css("content", "");
                    $(this).children(".treeview-menu").css("content", "");
                    var intMenuHeight = $(this).children(".treeview-menu").height();
                    var intWindowHeight = window.innerHeight - intTop-10;
                    if (intMenuHeight > intWindowHeight) {
                        $(this).children(".treeview-menu").height(intWindowHeight);
                        $(this).children(".treeview-menu").css("overflow-y", "auto");
                    } 
                },
                function () {
                    $(this).children(".treeview-menu").css("display", "none");
                    $(this).children(".treeview-menu").css("position", "static");
                }
            );
            // End左边菜单在缩进状态显示列表
        },
        init: function () {
            $('.menuItem').on('click', $.learuntab.addTab);
            $('.menuTabs').on('click', '.menuTab i', $.learuntab.closeTab);
            $('.menuTabs').on('click', '.menuTab', $.learuntab.activeTab);
            $('.tabLeft').on('click', $.learuntab.scrollTabLeft);
            $('.tabRight').on('click', $.learuntab.scrollTabRight);
            $('.tabReload').on('click', $.learuntab.refreshTab);
            $('.tabCloseCurrent').on('click', function () {
                $('.page-tabs-content').find('.active i').trigger("click");
            });
            $('.tabCloseAll').click(function () {
                $('.page-tabs-content').children("[data-id]").find('.fa-remove').each(function () {
                    $('.LRADMS_iframe[data-id="' + $(this).data('id') + '"]').remove();
                    $(this).parents('a').remove();
                });
                $('.page-tabs-content').children("[data-id]:first").each(function () {
                    $('.LRADMS_iframe[data-id="' + $(this).data('id') + '"]').show();
                    $(this).addClass("active");
                });
                $('.page-tabs-content').css('margin-left', '0');
            });
            $('.tabCloseOther').on('click', $.learuntab.closeOtherTabs);
            $('.fullscreen').on('click', function () {
                if (!$(this).attr('fullscreen')) {
                    $(this).attr('fullscreen', 'true');
                    $.learuntab.requestFullScreen();
                } else {
                    $(this).removeAttr('fullscreen')
                    $.learuntab.exitFullscreen();
                }
            });
        }
    };
    $.learunindex = {
        load: function () {
            $("body").removeClass("hold-transition")
            $("#content-wrapper").find('.mainContent').height($(window).height() - 91);
            $(window).resize(function (e) {
                $("#content-wrapper").find('.mainContent').height($(window).height() - 91);
            });
            $(".sidebar-toggle").click(function () {
                var objMenu = $("#sidebar-menu > .active a:first");//记录被打开的菜单
                if (!$("body").hasClass("sidebar-collapse")) {
                    objMenu = $("#sidebar-menu > .active a:first");
                    objMenu.click();
                    $("body").addClass("sidebar-collapse");
                    $(".fa-chevron-left").removeClass("fa-chevron-left").addClass("fa-chevron-right");
                    
                } else {
                    $("body").removeClass("sidebar-collapse");
                    $(".fa-chevron-right").removeClass("fa-chevron-right").addClass("fa-chevron-left");
                    objMenu.click();                    
                }
            })
            $(window).load(function () {
                window.setTimeout(function () {
                    $('#ajax-loader').fadeOut();
                }, 300);
            }); 
        },
        jsonWhere: function (data, action) {
            if (action == null) return;
            var reval = new Array();
            $(data).each(function (i, v) {
                if (action(v)) {
                    reval.push(v);
                }
            })
            return reval;
        },
        formatData: function (data) {
            return $.map(data, function (item) {
                if (item.items.length) {
                    item.items = this.formatData(item.items);
                }
                return {
                    F_ModuleId: item.id,
                    F_ParentId: item.parentId,
                    F_EnCode: item.code,
                    F_FullName: item.name,
                    F_Icon: item.icon,
                    F_UrlAddress: item.url,
                    F_Target: item.items.length > 0 ? "expand" : "iframe",
                    F_IsMenu: item.items.length > 0 ? 0 : 1,
                    F_AllowExpand: 1,
                    F_IsPublic: 0,
                    F_AllowEdit: null,
                    F_AllowDelete: null,
                    F_SortCode: 1,
                    F_DeleteMark: 0,
                    F_EnabledMark: 1,
                    F_Description: item.name,
                    F_CreateDate: null,
                    F_Child: item.items
                };
            }.bind(this));
        },
        loadTopMenu: function (menuData) {//顶部菜单加载
            var _html = "";
            $.each(menuData, function (i, ite) {
                var row = menuData[i];
                if (!row.F_IsMenu) {
                    _html += '<li class="treeview">';
                    _html += '  <a href="javascript:">';
                    _html += '      <i class="' + row.F_Icon + '"></i> <span>' + row.F_FullName + '</span>';
                    _html += '  </a>';
                    var childNodes = ite.F_Child;
                    if (childNodes.length > 0) {
                        _html += '<div class="popover-menu" style="display: none;">';
                        _html += '  <div class="triangle-up"></div>';
                        _html += '  <ul class="treeview-down">';
                        $.each(childNodes, function (j) {
                            var subrow = childNodes[j];
                            _html += '      <li>';
                            _html += '          <a class="downitem menuItem" data-id="' + subrow.F_ModuleId + '" href="' + bootPATH + subrow.F_UrlAddress + '">';
                            _html += '              <i class="' + subrow.F_Icon + '"></i> <span>' + subrow.F_FullName + '</span>';
                            _html += '          </a>';
                            var subchildNodes = subrow.F_Child;
                            if (subchildNodes.length > 0) {
                                _html += '          <div class="popover-menu-sub" style="display:none">';
                                _html += '              <ul class="treeview-down">';
                                $.each(subchildNodes, function (k) {
                                    var itemRow = subchildNodes[k];
                                    _html += '              <li>';
                                    _html += '                  <a class="downitem menuItem" data-id="' + itemRow.F_ModuleId + '" href="' + bootPATH + itemRow.F_UrlAddress + '"><i class="' + itemRow.F_Icon + '"></i> <span>' + itemRow.F_FullName + '</span></a>';
                                    _html += '              </li>';
                                });
                                _html += '              </ul>';
                                _html += '          </div>';
                            }
                            _html += '      </li>';
                        });
                        _html += '  </ul>';
                        _html += '</div>';
                    }
                    _html += '</li>';
                }
            });
            $("#top-menu").html(_html);
        },
        loadMenu: function () {
            $.ajax({
                url: bootPATH + '/api/services/api/Modules/GetNavigationByCurrentUser',
                async: false,
                type: "POST",
                timeout: 60000,//超时，时间为1分钟
                success: function (result) {
                    var data = $.learunindex.formatData(result.result);
                    var _html = "";
                    $.each(data, function (i, ite) {
                        var row = data[i];
                        if (!row.F_IsMenu) {
                            if (i == 0) {
                                _html += '<li class="treeview active">';
                            } else {
                                _html += '<li class="treeview">';
                            }
                            _html += '<a href="#">'
                            _html += '<i class="' + row.F_Icon + '"></i><span>' + row.F_FullName + '</span><i class="fa fa-angle-right pull-right"></i>'
                            _html += '</a>'
                            var childNodes = ite.F_Child
                            if (childNodes.length > 0) {
                                _html += '<ul class="treeview-menu">';
                                $.each(childNodes, function (i) {
                                    var subrow = childNodes[i];
                                    var subchildNodes = subrow.F_Child;
                                    _html += '<li>';
                                    if (subchildNodes.length > 0) {
                                        _html += '<a href="#"><i class="' + subrow.F_Icon + '"></i>' + subrow.F_FullName + '';
                                        _html += '<i class="fa fa-angle-right pull-right"></i></a>';
                                        _html += '<ul class="treeview-menu ">';
                                        $.each(subchildNodes, function (i) {
                                            var subchildNodesrow = subchildNodes[i];
                                            _html += '<li><a class="menuItem" data-id="' + subchildNodesrow.F_ModuleId + '" href="' + bootPATH + subchildNodesrow.F_UrlAddress + '"><i class="' + subchildNodesrow.F_Icon + '"></i>' + subchildNodesrow.F_FullName + '</a></li>';
                                        });
                                        _html += '</ul>';

                                    } else {
                                        _html += '<a class="menuItem" data-id="' + subrow.F_ModuleId + '" href="' + bootPATH + subrow.F_UrlAddress + '"><i class="' + subrow.F_Icon + '"></i>' + subrow.F_FullName + '</a>';
                                    }
                                    _html += '</li>';
                                });
                                _html += '</ul>';
                            }
                            _html += '</li>'
                        }
                    });
                    $("#sidebar-menu").append(_html);
                    $.learunindex.loadTopMenu(data);//添加顶部菜单
                    //是否加载左边菜单的滑动菜单
                    var isShowLeftMenu = GetCookie("showLeftMenu");
                    if (isShowLeftMenu != null && isShowLeftMenu != undefined && (isShowLeftMenu == true || isShowLeftMenu.toLowerCase() == "true")) {
                        $("body").removeClass("sidebar-collapse");
                        $(".fa-chevron-right").removeClass("fa-chevron-right").addClass("fa-chevron-left");          
                    } else {
                        //移出左边菜单缩进状态列表
                        $("body").addClass("sidebar-collapse");
                        $(".fa-chevron-left").removeClass("fa-chevron-left").addClass("fa-chevron-right");
                        $.learuntab.LeftMenu();//加载缩进时显示菜单
                    }
                    //是否显示顶级菜单
                    if (IsNavbarShow())
                        $(".left-bar").show();
                    else
                        $(".left-bar").hide();

                    //点击缩进左边菜单事件
                    $("#tabLRBut").click(function () {
                        var strSpanClass = $(this).find("span").attr("class");
                        if (strSpanClass.indexOf("fa-chevron-right") >= 0){
                            SetCookie("showLeftMenu", false, null);//存入cookie不显示左边菜单
                            $.learuntab.LeftMenu();//加载缩进时显示菜单
                        }
                        else{
                            SetCookie("showLeftMenu", true, null);//存入cookie显示左边菜单
                            $("#sidebar-menu .treeview").unbind("mouseenter").unbind("mouseleave");//移出加载缩进时显示菜单 
                        }
                        //是否显示顶级菜单
                        if (IsNavbarShow())
                            $(".left-bar").show();
                        else
                            $(".left-bar").hide();

                    });


                    //加载自定义滚动条-start
                    $(".scrollbarContent").mCustomScrollbar({
                        scrollInertia: 0,//处理在菜单缩进时滚动条显示有误
                        scrollButtons: {
                            enable: true
                        }
                    });
                    //加载自定义滚动条-end

                    $("#sidebar-menu li a").click(function () {
                        var d = $(this), e = d.next();
                        if (e.is(".treeview-menu") && e.is(":visible")) {
                            e.slideUp(500, function () {
                                e.removeClass("menu-open");
                                //刷新滚动条
                                $(".scrollbarContent").mCustomScrollbar("update");
                            }),
                            e.parent("li").removeClass("active")
                        } else if (e.is(".treeview-menu") && !e.is(":visible")) {
                            var f = d.parents("ul").first(),
                            g = f.find("ul:visible").slideUp(500);
                            g.removeClass("menu-open");
                            var h = d.parent("li");
                            e.slideDown(500, function () {
                                e.addClass("menu-open"),
                                f.find("li.active").removeClass("active"),
                                h.addClass("active");
                                //刷新滚动条
                                $(".scrollbarContent").mCustomScrollbar("update");
                            })
                        }
                        e.is(".treeview-menu");
                    });

                    $.learuntab.init();

                }
            });

        },
        loadUser: function () {
            abp.services.api.session.getCurrentLoginInformations({ async: false }).done(function (result) {
                var user = result.user;
                var tenant = result.tenant;
                var userInfo = "";
                if (!abp.multiTenancy.isEnabled) {
                    userInfo = appSession.user.userName;
                } else {
                    if (tenant) {
                        userInfo = tenant.tenancyName + '\\' + user.userName;
                    } else {
                        userInfo = '.\\' + user.userName;
                    }
                }
                $('#userInfo').html(userInfo);
                $('#lastLoginTime').html(user.lastLoginTime);
            });
        }
    };

    $(function () {
        $.learunindex.load();
        $.learunindex.loadMenu();
        $.learunindex.loadUser();
        $.learuntab.showDownlist();
        $.learuntab.bottomShow();        
    });
      
})(jQuery);


$(function () {
    //缩进时展示
    $("#change-menu").click(function () {
        if ($(".left-bar").hasClass("none")) {
            $(".left-bar").removeClass("none");
            $(".main-sidebar").addClass("none");
            $(".content-wrapper").addClass("marl0");
            $("button.sidebar-toggle").attr("disabled", "true");            
        } else if ($(".main-sidebar").hasClass("none")) {
            $(".left-bar").addClass("none");
            $(".main-sidebar").removeClass("none");
            $(".content-wrapper").removeClass("marl0");
            $("button.sidebar-toggle").removeAttr("disabled");
        }
    });
    ChangeColor();//风格切换

    TopTypeContent();
    $("#fa_help").on("click", function () {
        var urlCode = "~/Report/TbReport?code=contentHelp&helpCode=help";
        TopModeDialogUrl("h33333", "帮助文档", urlCode);
    });
    $("#AllContents").on("click", function () {
        var urlContent = "~/Report/TbReport?code=content_list";
        TopModeDialogUrl("h33334", "查看全部", urlContent);
    });
    //消息提示的宽度
    if ($(".navbar-toggle").is(":visible"))
        $("#newConeent3").css("width", "100%");
    else
        $("#newConeent3").css("width", "460px");

    MenuMin();//小菜单状态显示的事件
});

//风格切换
var ChangeColor = function () {
    $(".changecolor li").click(function () {
        var style = $(this).attr("id");
        switch (style) {
            case "blue":
                style = "blue";
                break;
            case "blueMin":
                style = "blueMin";
                break;
            case "green":
                style = "green";
                break;
            case "purple":
                style = "purple";
                break;
            case "black":
                style = "black";
                break;
            default:
                style = "blue";
                break;
        }
        $("link[name='ColorOp']").attr("href", bootPATH + "/Common/Theme/" + style + ".css");
    });
    SetCookie("ModeFrameIds", "", null);//清除记录打开窗口的缓存
}

//加载消息列表
function TopTypeContent() {
    //功能定义code，传入参数,以,隔开.zxxx最新消息xtgg系统公告
    //var codes = "zxxx,xtgg"; 
    //var urlContents = bootPATH + "/api/services/api/Content/GetDefineNewContents?codes=" + codes;
    //获取最新的未读的3条，或者最新3条消息
    var urlContents = bootPATH + "/api/services/api/Content/GetNewContents";
    $.post(urlContents, {}, function (data) {
        var datas = data.result;
        if (datas.length > 0) {
            //判断是否是未读数据，未读的显示未读数量，已读的不显示
            if (datas[0].is_user == 1) {
                //已读不显示
            } else {
                //未读，显示记录条数
                $("#newConeentCount").html(datas.length)
            }
            //显示数据
            var htmlli = "";
            //加入最新，未读的3条信息
            for (var i = 0 ; i < datas.length; i++) {
                var urlClick = "~/Content/ContentInfo?navId=" + datas[i].id;
                htmlli += "<li style='width:100%;float:left' onclick=TopModeDialogUrl('modalId664','查看','" + urlClick + "',1000,500)>";

                htmlli += " <span title='" + datas[i].title + "' style='float: left;height:26px;line-height:26px;width: 295px;overflow: hidden;' >【" + datas[i].type + "】" + datas[i].title.substr(0, 12) + "</span>";
                htmlli += "<span style='float: right;height:26px;line-height:26px;'>【" + datas[i].createTime.replace("T", " ").substring(0, 19) + "】</span>";

                htmlli += "</li>";
            }
            //加入，查看全部                  
            // htmlli += "<li><a style='padding: 3px 10px;float: left;' id='AllContents'>查看全部</a><span style='float: right;' ></span></li>";
            $("#newConeent3").append(htmlli);
        }
    });
}

//是否显示顶部菜单
var IsNavbarShow = function () {
    var intNavbar = $(".navbar-fixed-top").width() - $(".navbar-right").width() - $(".logo").width() - 50;//菜单栏有效长度
    var intLeftBar = $(".left-bar").width();
    var strClass = $("#tabLRBut span").attr("class");
    if ($(".navbar-toggle").is(":visible") || intNavbar <= intLeftBar) 
        return false

    if ($(".navbar-toggle").is(":hidden") && intNavbar >= intLeftBar && strClass.indexOf("fa-chevron-left") < 0)
        return true;
}

//随着浏览器的变化而变化
$(window).resize(function () {
    //是否显示顶部菜单
    if (IsNavbarShow())
        $(".left-bar").show();
    else
        $(".left-bar").hide();
    MenuMin();//小菜单状态显示的事件
});

//小菜单状态显示的事件
var MenuMin = function () {    
    if ($(".navbar-toggle").is(":visible")) {
        $("#newConeent3").css("width", "100%");//消息提示的宽度
        var objMenu = $("#sidebar-menu > .active a:first");
        objMenu.click();
        $("body").addClass("sidebar-collapse");
        $(".fa-chevron-left").removeClass("fa-chevron-left").addClass("fa-chevron-right");
        $.learuntab.LeftMenu();//加载缩进时显示菜单
    }
    else {
        $("#newConeent3").css("width", "460px");//消息提示的宽度
    }
}