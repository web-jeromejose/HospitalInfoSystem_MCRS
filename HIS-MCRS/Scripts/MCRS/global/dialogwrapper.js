/* Dialog Wrapper */
var ardialog = {
    Pop: function (type, title, content, btnlabel, callB) {
        var errorclass = "";
        switch (type) {
            case 1: errorclass = "btn btn-success btn-sm"; break;
            case 2: errorclass = "btn btn-info btn-sm"; break;
            case 3: errorclass = "btn btn-warning btn-sm"; break;
            case 4: errorclass = "btn btn-danger btn-sm"; break;
        }
        bootbox.dialog({
            title: title,
            onEscape: function () { callB(); },
            message: content,
            buttons: {
                success: {
                    label: btnlabel,
                    className: errorclass,
                    callback: function () {
                        callB();
                    }
                }
            }
        });
    },
    Append: function (type, target, message) {
        var eT;
        switch (type) {
            case 1: eT = "alert-success"; break;
            case 2: eT = "alert-info"; break;
            case 3: eT = "alert-warning"; break;
            case 4: eT = "alert-danger"; break;
        }
        var alertMe = '<div class="alert ' + eT + ' alert-dismissible" role="alert">' +
                        '<button type="button" class="close" data-dismiss="alert">' +
                            '<span aria-hidden="true">&times;</span><span class="sr-only">Close</span>' +
                        '</button>' +
                        '<strong>' + message + '</strong>' +
                      '</div>';

        $(target).html(alertMe);
    },
    Confirm: function (type, title, content, btnlabel, btnlabel2, callB, callB2) {
        var errorclass = "";
        switch (type) {
            case 1: errorclass = "btn btn-success btn-sm"; break;
            case 2: errorclass = "btn btn-info btn-sm"; break;
            case 3: errorclass = "btn btn-warning btn-sm"; break;
            case 4: errorclass = "btn btn-danger btn-sm"; break;
        }
        bootbox.dialog({
            title: title,
            onEscape: function () {
                callB2();
            },
            closeButton: true,
            message: content,
            buttons: {
                cancel: {
                    label: btnlabel2,
                    className: "btn btn-default btn-sm",
                    callback: function () {
                        $(this).modal('hide');
                        callB2();
                    }
                },
                success: {
                    label: btnlabel,
                    className: errorclass,
                    callback: function () {
                        setTimeout(callB, 1 * 250);
                    }
                }
            }
        });
    }
}
var ARspinner = '<div class="IT_loading-outer"><div class="IT_loading-middle"><div class="IT_loading-inner">'
                    + '<loader>'
                        + '<d></d>'
                        + '<d></d>'
                        + '<d></d>'
                        + '<d></d>'
                        + '<d></d>'
                        + '<d></d>'
                        + '<d></d>'
                        + '<d></d>'
                        + '<d></d>'
                        + '<d></d>'
                   + '</loader>'
                + '</div></div></div>';


var ARminispinner = '<div class="IT_loading-outer"><div class="IT_loading-middle"><div class="IT_loading-inner">' +
                        '<div class="spinner2">' +
                            '<div class="ball"></div>' +
                            '<p>Loading...</p>' +
                        '</div>' +
                    '</div></div></div>';
var _indicator = {
    Show: function (target) {
        $(target).block({
            message: ARminispinner,
            css: {
                border: 'none',
                backgroundColor: 'transparent'
            },
            overlayCSS: {
                backgroundColor: '#000',
                opacity: 0.7,
                position: 'absolute',
                zindex: '99999999',
                cursor: 'wait'
            }
        });
    },
    Body: function () {
        $.blockUI({
            message: ARspinner,
            css: {
                border: 'none',
                backgroundColor: 'transparent'
            },
            overlayCSS: {
                backgroundColor: '#000',
                opacity: 0.7,
                position: 'absolute',
                zindex: '99999999',
                cursor: 'wait'
            }
        });
    },
    Stop: function (target) {
        $(target).unblock();
        $.unblockUI();
    },
    Modal: function (target) {
        $(target).block({
            message: ARspinner,
            css: {
                border: 'none',
                backgroundColor: 'transparent'
            },
            overlayCSS: {
                backgroundColor: '#000',
                opacity: 0.7,
                position: 'absolute',
                zindex: '99999999',
                cursor: 'wait'
            }
        });
    },
    Stop2: function (target, callB) {
        setTimeout(callB, 1 * 150);
        $(target).unblock();
        $.unblockUI();
    }
}
