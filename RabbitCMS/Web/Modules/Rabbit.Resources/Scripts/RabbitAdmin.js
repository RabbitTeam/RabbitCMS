$.modal=function(options) {
    var html = '<div class="modal"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button><h4 class="modal-title"></h4></div><div class="modal-body"></div><div class="modal-footer"><button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button><button type="button" class="btn btn-primary">Save changes</button></div></div><!-- /.modal-content --></div><!-- /.modal-dialog --></div>';
    var body = $("body");
    var modal = $(html);
    modal.appendTo(body);

    options = $.extend({
        type: "",
        title: "",
        cancelText: "",
        okText:"",
        showClose:true
    }, options);

    var events = {cancelEvents:[],okEvents: [] };

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
            if (fn!=null)
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
        $.each(events, function() {
            this();
        });
    }
    function triggerOkEvents() {
        triggerEvents(events.okEvents);
    }
    function triggerCancelEvents() {
        triggerEvents(events.cancelEvents);
    }

    modal.find("[data-dismiss=modal]").click(function() {
        controller.hide();
        triggerCancelEvents();
    });
    if (options.type != null && options.type.length != 0) {
        modal.addClass("modal-" + options.type);
    }
    modal.find(".modal-title").append(options.title);
    modal.find(".modal-body").append(options.body);

    if (options.showClose == false) {
        modal.find(".modal-header .close").remove();
    }

    return controller;
}
$.modal({
    title: "test",
    body: "asda"
}).show();