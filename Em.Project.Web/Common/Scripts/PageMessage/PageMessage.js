//兼容ie6的fixed代码 
//jQuery(function($j){
//	$j('#pop').positionFixed()
//})
(function ($j) {
    $j.positionFixed = function (el) {
        $j(el).each(function () {
            new fixed(this)
        })
        return el;
    }
    $j.fn.positionFixed = function () {
        return $j.positionFixed(this)
    }
    var fixed = $j.positionFixed.impl = function (el) {
        var o = this;
        o.sts = {
            target: $j(el).css('position', 'fixed'),
            container: $j(window)
        }
        o.sts.currentCss = {
            top: o.sts.target.css('top'),
            right: o.sts.target.css('right'),
            bottom: o.sts.target.css('bottom'),
            left: o.sts.target.css('left')
        }
        if (!o.ie6) return;
        o.bindEvent();
    }
    $j.extend(fixed.prototype, {
        // ie6 : $.browser.msie && $.browser.version < 7.0,
        bindEvent: function () {
            var o = this;
            o.sts.target.css('position', 'absolute')
            o.overRelative().initBasePos();
            o.sts.target.css(o.sts.basePos)
            o.sts.container.scroll(o.scrollEvent()).resize(o.resizeEvent());
            o.setPos();
        },
        overRelative: function () {
            var o = this;
            var relative = o.sts.target.parents().filter(function () {
                if ($j(this).css('position') == 'relative') return this;
            })
            if (relative.size() > 0) relative.after(o.sts.target)
            return o;
        },
        initBasePos: function () {
            var o = this;
            o.sts.basePos = {
                top: o.sts.target.offset().top - (o.sts.currentCss.top == 'auto' ? o.sts.container.scrollTop() : 0),
                left: o.sts.target.offset().left - (o.sts.currentCss.left == 'auto' ? o.sts.container.scrollLeft() : 0)
            }
            return o;
        },
        setPos: function () {
            var o = this;
            o.sts.target.css({
                top: o.sts.container.scrollTop() + o.sts.basePos.top,
                left: o.sts.container.scrollLeft() + o.sts.basePos.left
            })
        },
        scrollEvent: function () {
            var o = this;
            return function () {
                o.setPos();
            }
        },
        resizeEvent: function () {
            var o = this;
            return function () {
                setTimeout(function () {
                    o.sts.target.css(o.sts.currentCss)
                    o.initBasePos();
                    o.setPos()
                }, 1)
            }
        }
    })
})(jQuery);

jQuery(function ($j) {
    $j('.footer').positionFixed()
});

//右下角弹出信息
function PopMsg(strTitle, strContent, strBut) {
    var strHtml = '<div id="popMsg" style="display:none;">';
    strHtml += '<style type="text/css">';
    strHtml += '#popMsg{background:#fff;width:260px;border:1px solid #e0e0e0;font-size:12px;position: fixed;right:10px;bottom:10px;}';
    strHtml += '.popHead{line-height:32px;background:#f6f0f3;border-bottom:1px solid #e0e0e0;position:relative;font-size:12px;padding:0 0 0 10px;}';
    strHtml += '.popHead h2{margin: 0px;font-size:14px;color:#666;line-height:32px;height:32px;}';
    strHtml += '.popHead a#popClose:hover{color:#f00;cursor:pointer;}';
    strHtml += '.popContent{text-indent:24px;line-height:160%;margin:5px 0;color:#666;}';
    strHtml += '.footer{text-align:right;border-top:1px dotted #ccc;line-height:24px;margin:8px 0 0 0;}';
    strHtml += '</style>';
    strHtml += '<div class="popHead">';
    strHtml += '<a id="popClose" style="position:absolute;right:10px;top:1px;cursor: pointer;" title="关闭">关闭</a>';
    strHtml += '<h2 class="popTitle">温馨提示</h2>';
    strHtml += '</div>';
    strHtml += '<div style="padding:5px 10px;">';
    strHtml += '<dl>';
    strHtml += '<dd class="popContent">这里是内容简介</dd>';
    strHtml += '</dl>';
    strHtml += '<p class="footer"></p>';
    strHtml += '</div>';
    strHtml += '</div>';
    $(strHtml).appendTo("body");
    this.strTitle = strTitle;
    this.strContent = strContent;
    this.strBut = strBut;
    //添加信息
    $(".popTitle").html(this.strTitle);
    $(".popContent").html(this.strContent);
    if ($.trim(this.strBut) != "") {
        $(".footer").html(this.strBut);
        $(".footer").show();
    } else {
        $(".footer").hide();
    }
    //切换滑动效果
    $("#popMsg").slideToggle("slow", "swing");

    //关闭
    $("#popClose").click(function () {
        $("#popMsg").fadeOut("slow", "swing");
    });
}