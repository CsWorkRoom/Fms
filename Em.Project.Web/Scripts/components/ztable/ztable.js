
(function ($) {
    var _currTable = null;
    var _$filterContanier = null;
    var _$tableContanier = null;
    var _$pagerContanier = null;
    var _$pager = null;

    var _filterList = null;
    var _columnList = null;
    var unloadingPage;
    var isMuRow = false;

    $(window).on('beforeunload', function () {
        unloadingPage = true;
    });
    $(window).on('unload', function () {
        unloadingPage = false;
    });

    var Table = function (element, options) {

        this.option = $.extend({
            action: "",
            fields: {},
            buttons: {},
            pageSize: 10,
            pageSizes: [10, 25, 50, 100],
            pageState: "static",
            key: "id",
            selectedCheck: [],
            currentData: []
        }, options);

        this.data = {
            currentPage: 1,
            isPostBack: false,
            order: {
                name: "Id",
                type: "Asc"
            },
            params: null
        };

        this.defualtOptions = {
            ajaxSettings: {
                dataType: 'json',
                type: 'POST',
                contentType: 'application/json'
            },
            buttons: {
                search: {
                    text: "查询",
                    icon: "fa-search",
                    style: "",
                    className: "btn-search",
                    index: 0,
                    event: this._onDefualtSearch
                }
            }
        };


        $.extend(this.option.buttons, this.defualtOptions.buttons);

        this._initTable(this);
    };

    var cssList = {
        main_container: "ztable-main-container ",
        filter_contanier: "ztable-filter-container row",
        header_contanier: "ztable-header-container row",
        title_contanier: "ztable-title-container row",
        table_contanier: "ztable-table-container row",
        pager_contanier: "ztable-pager-container row",
        filter_div: "ztable-filter-div",
        filter_label: "ztable-filter-label",
        filter_field: "ztable-filter-field",
        filter_input_contanier: "ztable-filter-input-contaniner",
        filter_input_class: "ztable-filter-input form-control",
        filter_operator_class: "ztable-filter-operator",
        toolbar_div: "ztable-tool-div",
        toolbar_btn: "btn",
        table: "ztable-table",
        table_head: "ztable-table-head",
        table_head_srcoll: "ztable-table-head-scroll",
        table_head_sorting: "sorting",
        table_head_sorting_asc: "sorting_asc",
        table_head_sorting_desc: "sorting_desc",
        table_head_sorting_no: "sorting_n",
        table_head_sorting_yes: "sorting_y",
        table_body_srcoll: "ztable-table-body-scroll",
        table_body: "ztable-table-body",
        table_mr: "ztable-table-mr",
        table_sr: "ztable-table-sr",
        table_empty: "ztable-table-empty",
        pager: {
            page_ul: "ztable-pager-pagination pagination",
            page_button: "previous",
            page_button_previous: "paginate_button previous",
            page_button_next: "paginate_button next",
            page_button_info: "paginate_button info",
        }
    };



    var messages = {

    };




    Table.prototype = {
        _initTable: function () {
            this._createFlag();
            this._createFieldAndColumnList();
            this._initContainer();
            this._initFilterContainer();
            this._initTableContainer();
            this._initPagerContainer();
            this._buildTable();
            this._loadTable();
            this._initEvent();
        },

        _initEvent: function () {
            //处理全屏/退出全屏 切换
            window.onresize = function () {
                this._resetSize();
            }.bind(this)
        },

        _createFlag: function () {
            this.option.flagId = _.random(1, 100);
            if ($('#table' + this.option.flagId).length) {
                this._createFlag();
            }
        },

        _initContainer: function () {
            if (!_currTable.hasClass(cssList.main_container))
                _currTable.addClass(cssList.main_container);
        },

        _initFilterContainer: function () {
            var self = this;

            _$filterContanier = $('<div />')
                .addClass(cssList.filter_contanier)
                .appendTo(_currTable);

            var btnContianer = $("<div />")
                       .addClass(cssList.toolbar_div)
                       .addClass("left");

            function Sorts(a, b) {
                return a.index > b.index;
            }

            var buttons = self.parseJsonToArrar(self.option.buttons).sort(Sorts);

            $.each(buttons, function (i, btn) {
                self._createbutton(btn)
                    .bind("click", self, btn.event)
                    .appendTo(btnContianer);
            });

            btnContianer.appendTo(_$filterContanier);

            $(_filterList).each(function (index, field) {


                var filterContianer = $("<div />")
                    .addClass(cssList.filter_div)
                    .addClass("left");

                self._createFilterLabel(field).appendTo(filterContianer);
                self._createFilterField(field).appendTo(filterContianer);

                filterContianer.appendTo(_$filterContanier);
            });

        },

        _initTableContainer: function () {
            _$tableContanier = $('<div />')
                .addClass(cssList.table_contanier)
                .appendTo(_currTable);
        },

        _initPagerContainer: function () {
            _$pagerContanier = $('<div />')
                .addClass(cssList.pager_contanier)
                .appendTo(_currTable);
        },

        _getSearchPara: function () {
            var result = {
                Page: {
                    PageSize: this.option.pageSize,
                    PageIndex: this.data.currentPage
                },
                Order: this.data.order,
                SearchList: this.option.searchParas || []
            };

            _$filterContanier.find("." + cssList.filter_field).each(function () {
                var operator = $(this).find("." + cssList.filter_operator_class).find("button");
                var input = $(this).find(".ztable-filter-input");

                if (input.val() != null && input.val().trim() != "") {
                    result.SearchList.push({
                        Name: input.attr("name"),
                        Value: input.val(),
                        Operator: operator && operator.length > 0 ? operator.attr("t-value") : "0",
                        TypeString: input.attr("op-type") || "string"
                    });
                }
            });


            return result;

        },

        _buildTable: function () {
            var self = this;

            $("<table />").attr("id","table"+self.option.flagId).addClass(cssList.table).appendTo(_$tableContanier);

            self._addTitle();
            self._addBody();
        },

        _loadTable: function () {
            var self = this;
            var page = null;

            var searchData = self._getSearchPara();
            self._showBusy();
            isMuRow = false;
            self.data.params = searchData;

            self._ajax({
                url: self.option.action,
                data: JSON.stringify(searchData),
                success: function (data) {
                    var result = data.data ? data.data : data;
                    page = result.page;

                    self.option.currentData = $.map(result.datas, function (item) {
                        return item[self.option.key];
                    });
                    self._addPager(page);
                    self._loadData(result);
                    self._resetSize();
                },
                error: function (xhr) {
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
                        else {
                            abp.message.error("服务器内部错误", '执行失败');
                        }
                    } catch (e) {
                        console.log(e);
                    }
                },
                complete: function () {
                    self._hideBusy();

                }
            });
        },

        _resetSize: function () {
            if (_$pager.option.data.pagerState) {
                //var height = _$pagerContanier.offset().top - _$tableContanier.find("table").offset().top - 40 - 10;
                var height = _currTable.height() - _currTable.find('.ztable-filter-container').height() - 95;

                _$tableContanier.find("table tbody").height(height);
            } else {
                //var height = _$pagerContanier.offset().top - _$tableContanier.find("table").offset().top - 40 - 10 + _$pagerContanier.height();
                var height = _currTable.height() - _currTable.find('.ztable-filter-container').height() - 95;
                _$tableContanier.find("table tbody").height(height);
            }

            //出现滚动条时 纠正thead与tbody 对齐
            _$tableContanier.find('thead').width("100%").width(_$tableContanier.find('tbody tr:first').width() + 1);
            
        },

        _loadData: function (data) {
            var self = this;
            _$tableContanier.find("table tbody").html("");

            if (!data.error) {
                if (data.page.totalCount > 0) {
                    $.each(data.datas, function (index, row) {
                        self._addRow(row);
                    });
                } else {
                    self._addNowData();
                }
            } else {
                self._addErrorRow(data.error);
            }
        },

        _addTitle: function () {
            var self = this;
            var head = $("<thead />").addClass(cssList.table_head).appendTo(_$tableContanier.find("table"));
            head.append("<tr>");

            $(_columnList).each(function (index, column) {

               
                var th = $("<th />").text(column.title).attr("column-name", column.name);

                if (column.type == "checkbox") {
                    var check = $("<input type='checkbox' />");
                    check.addClass("table_head_check" + self.option.flagId);
                    self._bindCheckClick(check, self.option.currentData);
                    th.append(check);
                    th.css("width", "12px");
                }

                else {

                    if (column.order != false) {
                        if (column.orderType == undefined) {
                            th.addClass(cssList.table_head_sorting);
                        } else if (column.orderType.toLowerCase() == "desc") {
                            th.addClass(cssList.table_head_sorting_desc);
                            self.data.order.type = "desc";
                            self.data.order.name = column.name;
                        } else if (column.orderType.toLowerCase() == "asc") {
                            th.addClass(cssList.table_head_sorting_asc);
                            self.data.order.type = "asc";
                            self.data.order.name = column.name;
                        }
                    } else {
                        th.addClass(cssList.table_head_sorting_none);
                    }

                    if (column.width) {
                        th.css("width", column.width);
                    }
                }


                if (column.textAlign) {
                    th.css("text-align", column.textAlign);
                }


                th.appendTo(head.find("tr"));
            });

            var clickEvent = function () {
                var th = $(this);
                var currClass = th[0].className;
                var name = th.attr("column-name");

                head.find("th." + cssList.table_head_sorting).removeClass().addClass(cssList.table_head_sorting);
                head.find("th." + cssList.table_head_sorting_desc).removeClass().addClass(cssList.table_head_sorting);
                head.find("th." + cssList.table_head_sorting_asc).removeClass().addClass(cssList.table_head_sorting);
                th.removeClass();

                self.data.order.name = name;
                if (currClass == "sorting_desc") {
                    th.addClass("sorting_asc");
                    self.data.order.type = "asc";
                } else {
                    th.addClass("sorting_desc");
                    self.data.order.type = "desc";
                }


                self._loadTable();
            };

            head.find("th." + cssList.table_head_sorting).bind("click", clickEvent);
            head.find("th." + cssList.table_head_sorting_desc).bind("click", clickEvent);
            head.find("th." + cssList.table_head_sorting_asc).bind("click", clickEvent);
        },

        _addBody: function () {
            var body = $("<tbody />").addClass(cssList.table_body).appendTo(_$tableContanier.find("table"));
            var table = _$tableContanier.find("table");
            var pager = _$pagerContanier;

            body.height(pager.offset().top - table.offset().top - 40 - 10);
        },

        _addPager: function (page) {
            var self = this;

            _$pager = _$pagerContanier.pager({
                pageSize: self.option.pageSize,
                pageSizes: self.option.pageSizes,
                maxPageShow: 10,
                showInfo: true,
                pagerState: self.option.pageState,
                showFristAndLast: true,
                onPageChangeEvent: function (pageIndex, pageSize) {
                    self.data.currentPage = pageIndex;
                    self.option.pageSize = pageSize;
                    self._loadTable();
                }
            });

            _$pager.load(page);
        },

        _pageChange: function (currentIndex) {
            var pagerUl = _$pagerContanier.find("ul");
            var pageCount = pagerUl.attr("pageCount");

            pagerUl.find("li").removeClass("disabled").removeClass("active");

            if (currentIndex == 1) {
                pagerUl.find("a[idx='frist']").parent().addClass("disabled");
                pagerUl.find("a[idx='prev']").parent().addClass("disabled");
            }

            if (currentIndex == pageCount) {
                pagerUl.find("a[idx='next']").parent().addClass("disabled");
                pagerUl.find("a[idx='last']").parent().addClass("disabled");
            }

            pagerUl.find("a[idx='" + currentIndex + "']").parent().addClass("active");
        },

        _addRow: function (row) {
            var self = this;
            var trClass = isMuRow ? cssList.table_mr : cssList.table_sr;
            var tableBody = _$tableContanier.find("table tbody");
            var tr = $("<tr>").addClass(trClass).appendTo(tableBody);

            $(_columnList).each(function (index, column) {
                var td = $("<td />").appendTo(tr);

                if (column.type == "checkbox") {
                    var check = $('<input type="checkbox">');
                    check.addClass("table_check_" + self.option.flagId);
                    self._bindCheckClick(check, row[self.option.key]);
                    td.append(check);
                    td.css("width", "12px");
                }
                else {
                    if (column.template) {
                        var html = "";

                        if (typeof column.template == "function") {
                            html = column.template(row, row[self._fistCharToLower(column.name)]);
                        } else {
                            html = column.template.toLowerCase();
                        }

                        $.each(row, function (i, v) {
                            var indexName = ("<%" + i + "%>").toLowerCase();

                            html = html.replace(new RegExp(indexName, "gm"), v);
                        });

                        td.html(html);
                    } else {
                        var value = row[self._fistCharToLower(column.name)];

                        if (column.format) {
                            //value = value == null ? "" : self._getTimeByTimeStr(value.replace("T", " ")).Format(column.format);
                        }

                        if (column.textIsHtml) {
                            td.html(value);
                        } else {
                            td.text(value);
                        }
                    }

                    if (column.width) {
                        td.css("width", column.width);
                    }

                    if (column.textAlign) {
                        td.css("text-align", column.textAlign);
                    }
                }
            });

            if (isMuRow)
                isMuRow = false;
            else
                isMuRow = true;
        },

        _addNowData: function () {
            var tableBody = _$tableContanier.find("table tbody");
            var tr = $("<tr>").addClass(cssList.table_empty).appendTo(tableBody);
            $("<td  />").attr("colspan", _columnList.length).html("<div><i class='fa fa-folder-open-o fa-4x'></i></div><div>没有查询到符合条件的数据</div>").appendTo(tr);
        },

        _showBusy: function (message) {
            message = message ? message : "";
            abp.ui.setBusy(_$tableContanier);
        },

        _hideBusy: function () {
            abp.ui.clearBusy(_$tableContanier);
        },

        _showError: function (message) {
            abp.message.error(message, '查询失败');
        },

        _createFieldAndColumnList: function () {
            _filterList = [];
            _columnList = [];

            if (this.option.multiselect) {
                _columnList.push({ title: "", type: "checkbox",name:"check" });
            }

            $.each(this.option.fields, function (index, field) {
                $.extend(field, { name: index });

                if (field.filter == true)
                    _filterList.push(field);

                if (!field.hidden)
                    _columnList.push(field);
            });
        },

        _createFilterLabel: function (field) {
            return $('<span />')
                .addClass(cssList.filter_label)
                .addClass("")
                .text(field.title);
        },

        _createFilterField: function (field) {
            var result = $('<div />')
                .addClass(cssList.filter_field)
                .addClass("");

            if (field.type == undefined)
                field.type = "string";

            if (field.type == "string" || field.type == "hidden") {
                this._createList({
                    name: field.name,
                    data: {
                        Contains: { displayText: "包含", value: "4" },
                        NotContains: { displayText: "不包含", value: "5" },
                        BeginWith: { displayText: "开始于", value: "6" },
                        EndWith: { displayText: "结束于", value: "7" },
                    }
                }, cssList.filter_operator_class).appendTo(result);

                this._createStringInput(field)
                    .appendTo(result);

            } else if (field.type == "number") {
                this._createList({
                    name: field.name,
                    data: {
                        Eqal: { displayText: "等于", value: "0" },
                        NotEqal: { displayText: "不等于", value: "1" },
                        MoreThan: { displayText: "大于", value: "2" },
                        LessThan: { displayText: "小于", value: "3" },
                    }
                }, cssList.filter_operator_class).appendTo(result);

                this._createNumberInput(field)
                   .appendTo(result);

            } else if (field.type == "date") {
                this._createList({
                    name: field.name,
                    data: {
                        Eqal: { displayText: "等于", value: "0" },
                        NotEqal: { displayText: "不等于", value: "1" },
                        MoreThan: { displayText: "大于", value: "2" },
                        LessThan: { displayText: "小于", value: "3" },
                    }
                }, cssList.filter_operator_class).appendTo(result);

                this._createDateInput(field)
                    .appendTo(result);
            } else if (field.type == "list") {
                this._createList(field, "ztable-dropdown-list").appendTo(result);
            } else if (field.type == "tree") {

                this._createTree(field, "")
                    .appendTo(result);

            } else if (field.type == "bool") {
                this._createList({
                    name: field.name,
                    type: 'bool',
                    data: [
                        { displayText: "是", value: true },
                        { displayText: "否", value: false }
                    ]
                }, "ztable-dropdown-list").appendTo(result);
            } else {
                alert("没有定义该种字段类型");
            }

            return result;
        },

        _bindCheckClick: function (check,value) {
            var self = this;
            check.click(function () {
                if (check.is(":checked")) {
                    if (_.isArray(value)) {
                        //拼接并剔重
                        self.option.selectedCheck = _.uniq(self.option.selectedCheck.concat(self.option.currentData));
                        $(".table_check_" + self.option.flagId).prop("checked", true);
                    }
                    else {
                        self.option.selectedCheck.push(value);
                    }
                }
                else {
                    if (_.isArray(value)) {
                        $.each(self.option.currentData, function (index, val) {
                            _.pull(self.option.selectedCheck, val);
                        });
                        $(".table_check_" + self.option.flagId).prop("checked", false);
                    }
                    else {
                        _.pull(self.option.selectedCheck, value);
                    }
                }
                if (!self.option.selectedCheck.length) {
                    $(".table_head_check" + self.option.flagId).prop("checked", false);
                }
            })
        },

        _createbutton: function (button) {
            var btn = $("<button />")
                .addClass(cssList.toolbar_btn)
                .addClass(button.className)
                .attr("style", button.style)
                .html("<i class='fa " + button.icon + "' /> " + button.text);

            return btn;
        },

        _createStringInput: function (field) {
            return $('<input type="text" name="' + field.name + '" op-type="' + field.type + '" value="' + (field.defaultValue == undefined ? "" : field.defaultValue) + '" />')
                .addClass(cssList.filter_input_class);
        },

        _createNumberInput: function (field) {
            return $('<input type="text"  name="' + field.name + '" op-type="' + field.type + '" value="' + (field.defaultValue == undefined ? "" : field.defaultValue) + '" />')
                .addClass(cssList.filter_input_class);
        },

        _createDateInput: function (field) {
            var dateInput = $('<input type="text" name="' + field.name + '" op-type="' + field.type + '" value="' + (field.defaultValue == undefined ? "" : field.defaultValue) + '" />')
               .addClass(cssList.filter_input_class);


            dateInput.datepicker({
                format: field.format,
                autoclose: true,
                language: 'zh-CN',
                toggleActive: true
            });

            return dateInput;
        },

        _createList: function (field, className) {
            var btnId = "dropdownMenu_" + field.name;
            var dropDown = $("<div />").addClass("dropdown").addClass(className);
            var btn = $('<button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" id="' + btnId + '"> </button>').appendTo(dropDown);
            var ul = $('<ul class="dropdown-menu" aria-labelledby="' + btnId + '">');
            var defultValue = field.defultValue;
            var defultText = field.defaultText;

            this._ajax({
                url: field.action,
                data: field.data,
                success: function (data) {
                    if (field.type == 'list' && !data)
                        data = [];

                    if (data instanceof Array) {
                        $('<input class="ztable-filter-input" type="hidden" />').attr("name", field.name).attr("op-type", field.type).val(defultValue).appendTo(dropDown);
                        data.splice(0, 0, { value: '', displayText: '----请选择----' });
                    }

                    $.each(data, function (index, d) {
                        if (defultValue == undefined)
                            defultValue = d.value;
                        if (defultText == undefined)
                            defultText = d.displayText;

                        var li = $('<li t-value="' + d.value + '" t-text="' + d.displayText + '"></li>').append('<a href="#">' + d.displayText + '</a>');

                        li.click(function () {
                            var l = $(this);
                            var tText = l.attr("t-text");
                            var tValue = l.attr("t-value");
                            var valueHtml = tText + '<span class="caret"></span>';

                            l.parent().parent().find(":hidden").val(tValue);
                            l.parent().parent().find(".btn").attr("t-value", tValue).attr("t-text", tText).html(valueHtml);
                            l.parent().parent().next(".form-control").css("padding-left", $(this).parent().parent().find(".btn").width() + 25 + "px");
                        });
                        ul.append(li);

                    });

                    btn.html(defultText + '<span class="caret"></span>');
                    btn.attr("t-value", defultValue);
                    btn.attr("t-text", defultText);
                    ul.appendTo(dropDown);
                },
                error: function () {
                    self._hideBusy();
                    self._showError(self.options.messages.serverCommunicationError);
                }
            });

            return dropDown;
        },

        _createTree: function (field) { },

        _ajax: function (options) {
            var self = this;

            if (options.url != null && options.url != undefined) {
                //Handlers for HTTP status codes
                var opts = {
                    statusCode: {
                        401: function () { //Unauthorized
                            self._unAuthorizedRequestHandler();
                        }
                    }
                };

                opts = $.extend(opts, self.defualtOptions.ajaxSettings, options);

                //Override success
                opts.success = function (data) {
                    //Checking for Authorization error
                    if (data && data.unAuthorizedRequest == true) {
                        self._unAuthorizedRequestHandler();
                    }

                    if (data.success) {
                        if (options.success) {
                            options.success(data.result);
                        }
                    } else {
                        if (options.error) {
                            options.error(data.error.message);
                        }
                    }
                };

                //Override error
                opts.error = function (jqXhr, textStatus, errorThrown) {
                    if (unloadingPage) {
                        jqXhr.abort();
                        return;
                    }

                    if (options.error) {
                        options.error(jqXhr);
                    }
                };

                //Override complete
                opts.complete = function () {
                    if (options.complete) {
                        options.complete();
                    }
                };

                $.ajax(opts);
            } else {
                options.success(options.data);
            }
        },

        _fistCharToLower: function (input) {
            return input.substring(0, 1).toLowerCase() + input.substring(1);
        },

        _fistCharToUpper: function (input) {
            return input.substring(0, 1).toUpperCase() + input.substring(1);
        },

        _getTimeByTimeStr: function (dateStr) {
            var timeArr = dateStr.split(" ");
            var d = timeArr[0].split("-");
            var t = timeArr[1].split(":");
            return new Date(d[0], d[1] - 1, d[2], t[0], t[1], t[2]);
        },

        parseArrarToJson: function (array) {
            var result = [];

            array.forEach(function (e) {
                result.push({ displayText: e, value: e });
            });

            return result;
        },

        parseJsonToArrar: function (json) {
            var array = new Array();

            $.each(json, function (i, s) {
                array.push(s);
            });

            return array;
        },

        _onDefualtSearch: function (arg) {
            var self = arg.data;

            self.data.isPostBack = false;
            self.data.currentPage = 1;
            self._loadTable();
        },

        _getSelectedCheck: function () {
            return this.option.selectedCheck;
        }
    };


    $.fn.ztable = function (options) {
        _currTable = $(this);
        return new Table(this, options);
    };

    Table.prototype.load = function () {
        this._loadTable();
    };

    Table.prototype.reload = function () {
        this.data.isPostBack = false;
        this.data.currentPage = 1;
        this._loadTable();
    };
})(jQuery);


(function ($) {
    var _currContanier = null;

    var Pager = function (element, options) {

        this.option = $.extend({
            page: {
                pageIndex: 1,
                totalCount: 0,
            },
            data: {
                pageCount: 0,
                pagerState: true
            },
            pageSize: 10,
            pageSizes: [10, 25, 50, 100],
            maxPageShow: 10,
            pagerState: "static",
            showInfo: true,
            showFristAndLast: true,
            autoLoad: false,
            onPageChangeEvent: function (pageIndex, pageSize) { }
        }, options);

        if (this.option.autoLoad)
            this._init(this);
    };

    var cssList = {
        pager: {
            page_ul: "ztable-pager-pagination pagination",
            page_button: "previous",
            page_button_previous: "paginate_button previous",
            page_button_next: "paginate_button next",
            page_button_info: "paginate_button info",
        }
    };


    Pager.prototype = {
        _init: function (page) {
            var self = this;
            _currContanier.empty();
            var startPage = 0, endPage = 0;
            var pageSplitCount = Math.ceil(self.option.maxPageShow / 2) - 1;
            var currentPage = page.pageIndex ? page.pageIndex : 1;
            var remainder = page.totalCount % self.option.pageSize;
            var pageCount = remainder > 0 ? ((page.totalCount - remainder) / self.option.pageSize) + 1 : page.totalCount / self.option.pageSize;
            self.option.data.pageCount = pageCount;

            if (pageCount > 1 || this.option.pagerState != "auto") {

                var pageUlBase = $("<ul />").addClass(cssList.pager.page_ul).addClass("page-left").appendTo(_currContanier);
                var pagerUl = $("<ul />").addClass(cssList.pager.page_ul).addClass("page-right").attr("pageCount", pageCount).appendTo(_currContanier);

                var getATabHtml = function (pageIndex, text) {
                    return '<a href="javascript:void(0)" aria-controls="example"  idx="' + pageIndex + '" tabindex="0">' + text + '</a>';
                };
                var getPageInfoHtml = function (pageIndex, pageSum, totalCount, zself) {
                    var html = "<span style='font-size: 14px;line-height: 1.6;color: #76838f'>每页<select name='pageSize'>";

                    zself.option.pageSizes.forEach(function (e) {
                        if (parseInt(e) == parseInt(self.option.pageSize)) {
                            html += "<option selected='selected' value='" + e + "'>" + e + "</span>";
                        } else {
                            html += "<option value='" + e + "'>" + e + "</span>";
                        }
                    });
                    html += "</select> 条/共<span sty>" + totalCount + "</span> 条，当前第<span >" + pageIndex + "</span>页/共<span>" + pageSum + "</span>页</span>";

                    return html;
                };

                if (self.option.showInfo) {
                    $("<li />").addClass(cssList.pager.page_button_info).html(getPageInfoHtml(currentPage, pageCount, page.totalCount, self)).appendTo(pageUlBase);
                }


                if (self.option.showFristAndLast) {
                    $("<li />").addClass(cssList.pager.page_button_previous).html(getATabHtml("frist", "首页")).appendTo(pagerUl);
                }

                $("<li />").addClass(cssList.pager.page_button_previous).html(getATabHtml("prev", "上一页")).appendTo(pagerUl);

                if (pageCount <= (pageSplitCount * 2) + 1) {
                    startPage = 1, endPage = pageCount;
                } else {
                    //处理开始页码
                    if (currentPage - pageSplitCount < 1) {
                        startPage = 1;
                    } else {
                        startPage = parseInt(currentPage) - parseInt(pageSplitCount);
                    }

                    if (pageCount - currentPage < pageSplitCount) {
                        endPage = pageCount;
                    } else {
                        endPage = parseInt(currentPage) + parseInt(pageSplitCount);
                    }
                }

                //添加首页前的...
                if (startPage > 1) {
                    $("<li />").addClass(cssList.pager.page_button).html(getATabHtml("N", "...")).appendTo(pagerUl);
                }

                for (var i = startPage; i <= endPage; i++) {

                    $("<li />").addClass(cssList.pager.page_button).html(getATabHtml(i, i)).appendTo(pagerUl);
                }

                //添加尾页前的...
                if (endPage < pageCount) {
                    $("<li />").addClass(cssList.pager.page_button).html(getATabHtml("N", "...")).appendTo(pagerUl);
                }


                $("<li />").addClass(cssList.pager.page_button_next).html(getATabHtml("next", "下一页")).appendTo(pagerUl);
                if (self.option.showFristAndLast) {
                    $("<li />").addClass(cssList.pager.page_button_next).html(getATabHtml("last", "尾页")).appendTo(pagerUl);
                }

                self._pageChange(currentPage);

                pagerUl.find("a").click(function () {
                    var index = $(this).attr("idx");
                    var isDisabled = $(this).parent().hasClass("disabled");

                    if (index != "N") {
                        if (!isDisabled) {
                            // ReSharper disable once InconsistentNaming
                            var _pageCount = $('.page-right').attr("pageCount");
                            var currentIndex = pagerUl.find(".active a").attr("idx");

                            if (index != currentIndex) {
                                if (index == "frist") {
                                    index = 1;
                                } else if (index == "prev") {
                                    index = parseInt(currentIndex) - 1;
                                } else if (index == "next") {
                                    index = parseInt(currentIndex) + 1;
                                } else if (index == "last") {
                                    index = _pageCount;
                                } else {
                                }

                                //   self._pageChange(index);
                                self.option.onPageChangeEvent(index, self.option.pageSize);
                            }
                        }
                    }
                });

                pagerUl.find("select").change(function () {
                    self.option.page.pageIndex = 1;
                    self.option.pageSize = $(this).val();
                    self.option.onPageChangeEvent(1, $(this).val());
                });

                self.option.data.pagerState = true;
            } else {
                self.option.data.pagerState = false;
            }
        },
        _pageChange: function (currentIndex) {
            var pagerUl = _currContanier.find("ul");
            var pageCount = $('.page-right').attr("pageCount");

            pagerUl.find("li").removeClass("disabled").removeClass("active");

            if (currentIndex == 1) {
                pagerUl.find("a[idx='frist']").parent().addClass("disabled");
                pagerUl.find("a[idx='prev']").parent().addClass("disabled");
            }

            if (currentIndex == pageCount || pageCount == "0") {
                pagerUl.find("a[idx='next']").parent().addClass("disabled");
                pagerUl.find("a[idx='last']").parent().addClass("disabled");
            }

            pagerUl.find("a[idx='" + currentIndex + "']").parent().addClass("active");
        }
    };


    $.fn.pager = function (options) {
        _currContanier = $(this);
        return new Pager(this, options);
    };

    Pager.prototype.load = function (page) {
        if (page.page)
            this._init(page.page);
        else
            this._init(page);

        return this;
    };
})(jQuery);