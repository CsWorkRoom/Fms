//-----------------使用实例---------------
//var modal1 = DiyModal.window({
//    title: '测试1',
//    url: '@Url.Action("Index", "User", new { area="Admin"})',
//    fullscreen: true
//});

var DiyModal = {
    window: function (options) {
        var setting = $.extend({
            title: '',
            width: 400,
            height: 300,
            url: '',
            fullscreen: false,
            topShow: true,
            beginOpen: function () { },
            afterOpen: function () { },
            beginColse: function () { return true; },
            error: function (message) { alert(message); },
            afterClose: function () { }
        }, options);

        if (setting.url == '') {
            setting.error("必须指定一个请求url");
        }
        else {
            var classContent = setting.fullscreen ? "modal fade modal-diy modal-fullscreen force-fullscreen" : "modal fade modal-diy";
            var styleContent = 'width:' + setting.width + 'px;height:' + setting.height + 'px;';
            var modalWindow = null;
            var modalHtml = '<div class="' + classContent + '" id="modalWindow" tabindex="-1" ';
            modalHtml += 'role="dialog"  data-backdrop="static" >';
            modalHtml += setting.fullscreen ? '<div class="modal-dialog">' : '<div class="modal-dialog" style="' + styleContent + '">';
            modalHtml += '<div class="modal-content" style="height:100%;">';
            modalHtml += '<div class="modal-header">';
            modalHtml += '<button type="button" class="close" title="关闭">&times;</button>';
            //modalHtml += '<button type="button" class="refresh" title="刷新"><i class="fa fa-refresh"></i></button>';
            modalHtml += '<h4 class="modal-title">' + setting.title + '</h4></div>';
            modalHtml += '<div class="modal-body" style="height:'+(setting.height)+'px"><iframe width="100%" height="100%" frameborder="0" >';
            modalHtml += '</iframe></div></div></div></div>';

            var onLoadUrl = function (url) {
                frame[0].src = setting.url;
            };

            var resultFun = {
                open: function () {
                    var tdoc = document;

                    if (setting.topShow) {
                        var twin = window.parent, cover;
                        while (twin.parent && twin.parent != twin) {
                            try { if (twin.parent.document.domain != document.domain) break; }
                            catch (e) { break; }
                            twin = twin.parent;
                        }

                        tdoc = twin.document;
                    }

                    modalWindow = $(modalHtml).appendTo(tdoc.body);
                    var frame = modalWindow.find("iframe");

                    modalWindow.find(".modal-header .close").click(function () {
                        resultFun.close();
                    });

                    modalWindow.find(".modal-header .refresh").click(function () {
                        resultFun.refresh();
                    });

                    frame.load(function () {
                        abp.ui.clearBusy(".modal-body");
                    });

                    abp.ui.setBusy(".modal-body");

                    setting.beginOpen();

                    modalWindow.modal("show");


                    $(modalWindow).on('hidden.bs.modal', function () {
                        modalWindow.remove();
                       $("body .modal-backdrop").remove();
                    });

                    var onLoadUrl = function () {
                        frame[0].src = setting.url;
                    };
                    setTimeout(onLoadUrl, 1);
                   
                },
                close: function () {
                    if (setting.beginColse) {
                        setting.beginColse();
                    }
                    modalWindow.modal("hide");
                    if (setting.afterClose) {
                        setting.afterClose();
                    }
                },
                refresh: function () {
                    var frame = modalWindow.find("iframe");
                    frame[0].src = setting.url;
                }
            };

            return resultFun;
        }
    },
    closeWindow: function () {
        $(".modal-diy").remove();
        $("body .modal-backdrop").remove();
    },
    closeModal: function () {
        //找到模态框的关闭按钮，触发单击事件，最终调用modal的close方法
        $(window.parent.document.body).find('.modal-header .close').click();
    }
};
