var LoadingDivID = '<div class="sk-folding-cube"><div class="sk-cube1 sk-cube"></div><div class="sk-cube2 sk-cube"></div><div class="sk-cube4 sk-cube"></div><div class="sk-cube3 sk-cube"></div></div>';
var ajaxWrapper = $.ajaxWrapper();

$(document).ready(function() {
    $("#modLoading").modal({
        backdrop: 'static',
        keyboard: false
    });

});

$(document).on({
    ajaxStart: function () { $("#modLoading").modal("show"); },
    ajaxStop: function () { $("#modLoading").modal("hide"); }
});

function Clear() {
    window.location.reload();
}

function AjaxWrapperGet(url, div) {
    $.ajax({
        cache: false,
        type: 'GET',
        url: url,
        async: true,
        contentType: 'application/json',
        //data: aData,
        beforeSend: function () {
            $(".spinner").modal({ keyboard: false });
        },
        complete: function () {
            $(".spinner").modal('hide');
        },
        success: function (data) {
            div.html(data);
        }
    });
}

function AjaxWrapperGetData(url, div, aData) {
    $.ajax({
        cache: false,
        type: 'GET',
        url: url,
        contentType: 'application/json',
        data: aData,
        beforeSend: function () {
            $(".spinner").modal({ keyboard: false });
        },
        success: function (data) {
            $(".spinner").modal('hide');
            div.html(data);
        }
    });
}

function NotifySuccess(msg, module) {
    toastr.success(msg, module).options = {
        "closeButton": false,
        "debug": true,
        "positionClass": "toast-top-right",
        "onclick": null,
        "showDuration": "700",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
}

function NotifyError(msg, module) {
    toastr.warning(msg, module).options = {
        "closeButton": false,
        "debug": true,
        "positionClass": "toast-top-right",
        "onclick": null,
        "showDuration": "100",
        "hideDuration": "100",
        "timeOut": "100",
        "extendedTimeOut": "100",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
}

function GetDate(Call) {
    //$(".datepicker").datetimepicker({
    //    format: 'DD MMM YYYY HH:mm',
    //    autoclose: true,
    //    pick12HourFormat: false
    //});
    var months = ['Jan', 'Feb', 'Mar', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    now = new Date(),
    formatted = now.getDate() + ' ' + months[now.getMonth()] + ' ' +
         now.getFullYear() + ' ' + now.getHours() + ':' + now.getMinutes() + ':' +
        now.getSeconds();
    Call(formatted);
}

function AjaxWrapperPost(url, aData, SuccessCallBack, module) {
    $.ajax({
        cache: false,
        type: 'POST',
        url: url,
        data: aData,
        contentType: 'application/json',
        beforeSend: function () {
            $(".spinner").modal({ keyboard: false });
        },
        success: function (data, e) {
            $(".spinner").modal('hide');
            if (data.Flag == "1") {
                NotifySuccess(data.Message, module);
            }
            else {
                NotifyError(data.Message, module);
            }

            if (SuccessCallBack)
                SuccessCallBack(data, e);
        }
    });
}

function LoadingFunc() {
    $('div._dmain').block({
        message: $(LoadingDivID),
        css: { width: '4%', border: '0px none #FFFFFF', cursor: 'wait', backgroundColor: '#FFFFFF' },
        overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.0, cursor: 'wait' }
    });
}

function selectFormatResult(data) {
    var markup = "<table><tr>";
    if (data.name !== undefined) {
        markup += "<td>" + data.text + "</td>";
    }
    markup += "</td></tr></table>"
    return markup;
}

function OpenModal(input) {
    $(input).modal({ keyboard: true });
}

function OpenDialog(title, message, CallBack) {
    swal({
        title: title, text: message, type: "info", showCancelButton: true,
        closeOnConfirm: true, showLoaderOnConfirm: true,
    }, function () {
        CallBack();
    });
}

function InitSel2CSS(input) {
    $(input).select2({ query: function () { } });
    $(input).select2("enable", false);
}

function Sel2Server(input, d) {
    $("#txtpatientID").select2({
        allowClear: false,
        placeholder: "Search Patient",
        minimumInputLength: 4,
        ajax: {
            cache: false,
            type: 'GET',
            dataType: "json",
            url: $("#url").data("patient"),
            data: function (searchTerm) {
                return { id: searchTerm };
            },
            results: function (data) {
                return { results: data };
            }
        },
        dropdownAutoWidth: true,
        formatResult: selectFormatResultPatient
    }).on("change", function () {
        Callback($(this).select2('data'));
        $("#txtsex").val($(this).select2('data').Sex);
        $("#txtname").val($(this).select2('data').name);
        $("#txtage").val($(this).select2('data').Age);
    });
}

function Sel2Client(input, data, CallBack) {
    $(input).select2({
        allowClear: true,
        initSelection: function (element, callback) {
            var selection = _.find(data, function(metric) {
                return metric.id === element.val();
            });
            callback(selection);
        },
        query: function (options) {
            var pageSize = 100;
            var startIndex = (options.page - 1) * pageSize;
            var filteredData = data;
            var stripDiacritics = window.Select2.util.stripDiacritics;

            if (options.term && options.term.length > 0) {
                if (!options.context) {
                    var term = stripDiacritics(options.term.toString().toLowerCase());
                    options.context = data.filter(function (metric) {
                        if (!metric.stripped_text) {
                            metric.stripped_text = stripDiacritics(metric.id.toString().toLowerCase());
                        } return (metric.stripped_text.indexOf(term) !== -1 || metric.id.toString().toUpperCase().indexOf(term.toUpperCase()) !== -1 || metric.name.toString().toUpperCase().indexOf(term.toUpperCase()) !== -1);

                    });
                }
                filteredData = options.context;
            }
            options.callback({
                context: filteredData,
                results: filteredData.slice(startIndex, startIndex + pageSize),
                more: (startIndex + pageSize) < filteredData.length
            });
        },
        dropdownAutoWidth: true,
        formatResult: selectFormatName
    }).on('change', function () {
        CallBack($(this).select2('data'));
    });
    $(input).select2("enable", true);
}

function selectFormatName(data) {
    var markup = "<table><tr>";
    if (data.name !== undefined) {
        markup += "<td>" + data.text + "</td>";
    }
    markup += "</td></tr></table>"
    return markup;
}