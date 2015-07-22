(function ($) {
    $.fn.table = function (options) {
        var container = $(this);
        var table = container.find(".table");
        var tableBody = table.find("tbody");
        var overlay = container.find(".overlay");
        options = $.extend({ pageIndex: 0, pageSize: 10 }, options);
        var pageIndex = options.pageIndex, pageSize = options.pageSize;
        var tableController = {
            initPageList: function (pageCount) {
                var pageList = $(".pagination");
                pageList.empty();

                if (pageCount <= 0) {
                    pageList.hide();
                    return;
                }
                pageList.show();
                var top = $('<li><a href="#"><</a></li>').data("index", pageIndex - 1).appendTo(pageList);
                for (var i = 1; i <= pageCount; i++) {
                    var li = $('<li><a href="#">' + i + '</a></li>').data("index", i - 1);
                    pageList.append(li);
                    if ((i - 1) == pageIndex)
                        li.addClass("active");
                }

                var end = $('<li><a href="#">></a></li>').data("index", pageIndex + 1).appendTo(pageList);

                if (pageIndex == 0)
                    top.remove();
                if (pageCount == (pageIndex + 1))
                    end.remove();

                pageList.find("li").click(function () {
                    var index = parseInt($(this).data("index"));
                    pageIndex = index;
                    tableController.load();
                });
            },
            load: function () {
                overlay.show();
                $.ajax({
                    type: 'post',
                    url: options.dataSource,
                    cache: false,
                    data: { pageIndex: pageIndex, pageSize: pageSize, titleKeywords: container.find("#table_search").val() },
                    success: function (data) {
                        overlay.hide();
                        tableBody.empty();
                        tableController.initPageList(data.PageCount);
                        $.each(data.list, function () {
                            options.newTr(this, tableController).appendTo(tableBody);
                        });
                    }
                });
            }
        };
        $(".box-tools .btn-reload").click(function () {
            tableController.load();
        });

        tableController.load();

        return tableController = $.extend(container, tableController);
    };

    $.fn.dialog = function (options) {
        options = $.extend({ autoClose: true, delay: 2000 }, options);
        var container = $(this);

        return $.extend(container, {
            show: function () {
                container.fadeIn();
                if (options.autoClose) {
                    (function (c) {
                        setTimeout(function () {
                            c.hide();
                        }, options.delay);
                    })(this);
                }
            },
            hide: function () {
                container.fadeOut();
            }
        });
    }
})(jQuery);