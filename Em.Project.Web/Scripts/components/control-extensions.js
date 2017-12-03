(function ($) {

    //#region dropdownTree
    $.dropdowntree = function (source, options) {
        source = typeof source == "object" ? $(source) : $("#" + source);
        options = options || {};
        options.el = source;
        return new DropDownTree(source, options);
    }
    $.fn.dropdowntree = function (options) { return this.each(function () { return $.dropdowntree(this, options) }) }

    function DropDownTree(source, options) {
        $.extend(this, options);
        this.E = source.get(0);
        this.createOnlyId();
        this.createHtml();
        this.initEvent();
    }

    DropDownTree.prototype = {
        createOnlyId: function () {
            var id = _.random(1, 100);
            this.elementId = "dropdowntree" + id;
            if ($(this.elementId).length) {
                this.createOnlyId();
            }
        },

        createHtml: function () {
            var html = [
                "<div class='input-group'>",
                    "<input type='hidden' id='" + this.elementId + "' name='" + this.elementId + "'/>",
                    "<input type='text' id='text_" + this.elementId + "' class='form-control' readonly  />",
                    "<div id='dropDownTree_" + this.elementId + "' class='dropdown-menu dropdown-tree col-xs-12'>",
                        "<ul id='menuTree_" + this.elementId + "' class='ztree'></ul>",
                    "</div>",
                    "<span class='input-group-btn'>",
                        "<button id='btn_" + this.elementId + "' class='btn btn-default' type='button'><i class='fa fa-chevron-down'></i></button>",
                    "</span>",
                "</div>"
            ];

            $(this.E).html(html.join(''));
        },

        initEvent: function () {
            var _self = this;
            $('#btn_' + this.elementId).click(function () {
                $('#dropDownTree_' + this.elementId).toggle();
            }.bind(this));

            var setting = {
                callback: {
                    onClick: function (event, treeId, treeNode) {
                        if (treeNode.chkDisabled)
                            return;
                        $('#' + _self.elementId).val(treeNode.id);
                        $('#text_' + _self.elementId).val(treeNode.name);
                        $('#dropDownTree_' + _self.elementId).toggle();
                    }
                },
                view: {
                    showLine: false,
                    selectedMulti: false
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                }
            };

            $.post(this.url, {}, function (data) {
                if (data.result.contentEncoding)
                    data = data.result.data;
                else
                    data = data.result;

                var tree = $.fn.zTree.init($('#menuTree_' + this.elementId), setting, eval(data));
                //var value = '{value}' == '' ? null : '{value}';

                //var defualtNode = tree.getNodeByParam('id', value, null);
                //if(defualtNode)
                    //$('#text_{id}').val(defualtNode.name);
            }.bind(this));
        }
    }
    //#endregion


})(jQuery);