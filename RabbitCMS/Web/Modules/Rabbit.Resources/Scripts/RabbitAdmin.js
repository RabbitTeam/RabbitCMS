(function ($) {
    var rabbit = {};

    //modal
    (function () {
        rabbit.modal = function (options) {
            var html = '<div class="modal"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title"></h4></div><div class="modal-body"></div><div class="modal-footer"><button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button><button type="button" class="btn btn-primary">Save changes</button></div></div><!-- /.modal-content --></div><!-- /.modal-dialog --></div>';
            var body = $("body");
            var modal = $(html);
            modal.appendTo(body);

            options = $.extend({
                type: "",
                title: "标题",
                body: "内容",
                showClose: true,
                autoShow: true,
                onOk: null,
                onCancel: null,
                buttons: { cancel: { show: true, text: "取消" }, ok: { show: true, text: "确定" } }
            }, options);

            //button status
            (function() {
                function initButton(op, text) {
                    if (op == null) {
                        return { show: true, text: text };
                    }
                    else if (op instanceof Boolean) {
                        return { show: op, text: text };
                    }
                    return op;
                }

                options.buttons.ok = initButton(options.buttons.ok, "确定");
                options.buttons.cancel = initButton(options.buttons.cancel, "取消");
            })();

            var events = { cancelEvents: [], okEvents: [] };

            var controller = {
                show: function () {
                    return this.toggle(true);
                },
                hide: function () {
                    return this.toggle(false);
                },
                toggle: function (b) {
                    if (b == null)
                        b = modal.is(":hidden");
                    if (b)
                        modal.fadeIn();
                    else
                        modal.fadeOut();

                    return controller;
                },
                onCancel: function (fn) {
                    if (fn != null)
                        events.cancelEvents.push(fn);
                    return controller;
                },
                onOk: function (fn) {
                    if (fn != null)
                        events.okEvents.push(fn);
                    return controller;
                }
            };

            function triggerEvents(events) {
                var b = true;
                $.each(events, function () {
                    var r = this();
                    if (r === false && b === true)
                        b = false;
                });
                return b;
            }
            function triggerOkEvents() {
                return triggerEvents(events.okEvents);
            }
            function triggerCancelEvents() {
                return triggerEvents(events.cancelEvents);
            }

            //button
            (function () {
                var btnOk = modal.find(".modal-footer button:last");
                var btnCancel = modal.find(".modal-footer button:first");

                function initButtonStatus(button,op) {
                    return button.text(op.text||"").toggle(op.show);
                }

                initButtonStatus(btnOk, options.buttons.ok).click(function () {
                    if (triggerOkEvents())
                        controller.hide();
                });
                initButtonStatus(btnCancel, options.buttons.cancel);
            })();

            //modal base
            (function () {
                modal.find("[data-dismiss=modal]").click(function () {
                    if (triggerCancelEvents())
                        controller.hide();
                });

                if (options.type != null && options.type.length !== 0) {
                    modal.addClass("modal-" + options.type);
                }
                modal.find(".modal-title").append(options.title);
                modal.find(".modal-body").append(options.body);

                if (!options.showClose) {
                    modal.find(".modal-header .close").remove();
                }
                if (options.autoShow) {
                    controller.show();
                }
                if (options.onOk != null) {
                    controller.onOk(options.onOk);
                }
                if (options.onCancel != null) {
                    controller.onCancel(options.onCancel);
                }
            })();

            return $.extend(modal, controller);
        }

        function modalStatus(options) {
            var modal = rabbit.modal(options);
            var btnOk = modal.find(".modal-footer button:last");
            var btnCancel = modal.find(".modal-footer button:first");
            btnCancel.removeClass("btn-default").addClass("btn-outline");
            btnOk.removeClass("btn-primary").addClass("btn-outline");
            return modal;
        }

        rabbit.modalPrimary = function (options) {
            return modalStatus($.extend(options, { type: "primary", buttons: { cancel: false } }));
        };
        rabbit.modalInfo = function (options) {
            return modalStatus($.extend(options, { type: "info", buttons: { cancel: false } }));
        };
        rabbit.modalWarning = function (options) {
            return modalStatus($.extend(options, { type: "warning" }));
        };
        rabbit.modalSuccess = function (options) {
            return modalStatus($.extend(options, { type: "success", buttons: { cancel: false } }));
        };
        rabbit.modalDanger = function (options) {
            return modalStatus($.extend(options, { type: "danger", buttons: { cancel: false } }));
        };

    })();

    $.rabbit = function() {
        return rabbit;
    };
})(jQuery);