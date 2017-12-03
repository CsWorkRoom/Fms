
//#region Ajax扩展

var App = App || {};

(function () {

    var appLocalizationSource = abp.localization.getSource('Easyman');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

    App.post = function(url, postData, funcSuccess, funcError) {
        return ajaxBase('POST', url, postData, funcSuccess, funcError);
    }

    App.get = function (url, getData, funcSuccess, funcError) {
        return ajaxBase('GET', url, getData, funcSuccess, funcError);
    }


    function ajaxBase(httpMethod, url, paramData, funcSuccess,funcError) {
        abp.ui.setBusy();
        return $.ajax({
            type: httpMethod,
            url: url,
            data: paramData,
            dataType: "json",
            contentType: 'application/json',
            success: function (data) {
                abp.ui.clearBusy();
                if (funcSuccess)
                    funcSuccess(data);
            },
            error: function (xhr) {
                if (funcError) funcError();
                abp.ui.clearBusy();
                var data = JSON.parse(xhr.responseText);
                try {
                    if (data.success === false) {
                        if (data.error.validationErrors) {
                            abp.message.error(data.error.details, data.error.message);
                        }
                        else {
                            abp.message.error(data.error.message, '执行失败');
                        }
                    }
                    else{
                        abp.message.error("服务器内部错误", '执行失败');
                    }
                } catch (e) {
                    console.log(e);
                }
                return false;
            }
        });
    }

})(App);

//#endregion

//#region 居于$.ajaxForm进行扩展
(function ($) {
    $.submitForm = function (source, options) {
        source = typeof source == "object" ? $(source) : $("#" + source);
        options = options || {};
        options.el = source;
        return new SubmitForm(source, options);
    };

    $.fn.submitForm = function (options) { return this.each(function () { return $.submitForm(this, options) }) }

    function SubmitForm(source, options) {
        $.extend(this, options);
        this.success = this.success || function () { };
        this.error = this.error || function () { };
        this.E = source.get(0);
        this.init();
    }

    SubmitForm.prototype = {
        init: function () {
            $(this.E).ajaxForm({
                //对ajaxForm事件进行拦截，重新组装数据将事件提交给webApi地址
                beforeSubmit: function (a, b, v) {
                    if (this.beforeSubmit) {
                        this.beforeSubmit(a);
                    }
                    this.submitToWebApi(a, v.url);
                    return false;
                }.bind(this)
            })
        },
        submitToWebApi: function (data,url) {
            var entityArry = {};
            $.each(data, function (index, item) {
                if (!entityArry[item.name])
                    entityArry[item.name] = item.value;
            });
            App.post(url, JSON.stringify(entityArry),this.success,this.error);
        }
    }

})(jQuery);
//#endregion
