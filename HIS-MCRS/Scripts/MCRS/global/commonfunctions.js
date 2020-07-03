function CommonEmployeeFormatResult(inp) {
    return inp.text;
}
function selectFormatResult(data) {
        return data.text;
}


function CommonEmployeeFormatResult2(inp) {
    //lform = "<table style='border:1px solid #ccc;' width='100%'>" +
    //        "<tr><td style='font-weight:bold;text-align:center !important;width:10%;padding:3px;'>" + inp.bedid + "</td>" +
    //        "<td style='text-align:center !important;width:30%;padding:3px;'>" + inp.pin + "</td>" +
    //        "<td style='text-align:left !important;width:60%;padding:3px;'>" + inp.name + "</td></tr>" +
    //        "</table>";
    lform = inp.bedname + ' - ' + inp.pin + ' - ' + inp.name;
    return lform
}
function contToInt(str) {
    return String(str).split(',').join('');
}
function CommaFormatted(amount) {
    var delimiter = ","; // replace comma if desired
    var a = amount.split('.', 2);
    var d = a[1];
    var i = parseInt(a[0]);
    if (isNaN(i)) { return ''; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    var n = new String(i);
    var a = [];
    while (n.length > 3) {
        var nn = n.substr(n.length - 3);
        a.unshift(nn);
        n = n.substr(0, n.length - 3);
    }
    if (n.length > 0) { a.unshift(n); }
    n = a.join(delimiter);
    if (d.length < 1) { amount = n; }
    else { amount = n + '.' + d; }
    amount = minus + amount;
    return amount;
}
//Other way to format monery
Number.prototype.formatToMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};


(function ($) {
    $.ajaxWrapper = function (options) {

        this.defaults = {
            ShowLoading: true,
            LoaderContainerId: "#divLoader",
            LoaderText: "Loading...",
            LoadingDivID: "",
            ErrorCallBack: function (jqXHR, textStatus, errorThrown) {
                var contentType = jqXHR.getResponseHeader("Content-Type");
                if (jqXHR.status === 200 && contentType.toLowerCase().indexOf("text/html") >= 0) {
                    window.location.reload();
                    return;
                }
                else if (jqXHR.status === 500 && contentType.toLowerCase().indexOf("text/html") >= 0) {
                    document.open();
                    document.write(jqXHR.responseText);
                    document.close();
                    return;
                }
                else if (jqXHR.status === 404) {
                    document.open();
                    document.write(jqXHR.responseText);
                    document.close();
                    return;
                }

                var response = $.parseJSON("(" + jqXHR.responseText + ")");
                var output = "Message: " + response.Message + "\n\n";
                output += "StackTrace: " + response.StackTrace;
                alert(output);
            }
        };

        var o = $.extend({}, this.defaults, options);

        this.Get = function (WebService, GetData, SuccessCallBack, LoadingDivID, LoaderText, ErrorCallback) {

            if (WebService == "")
                return 1; // Incorrect Parameters

            if (ErrorCallback == null)
                ErrorCallback = o.ErrorCallBack;

            if (GetData != null && typeof (GetData) === "object")
                GetData = $.param(GetData);

            var ShowLoading = false;
            if (typeof LoadingDivID !== 'undefined' && LoadingDivID != null && LoadingDivID != '')
                ShowLoading = true;

            if (typeof LoadingDivID !== 'undefined' && LoadingDivID != null && LoadingDivID.charAt(0) != '.')
                LoadingDivID = '#' + LoadingDivID;

            $.ajax({
                type: "GET",
                url: WebService,
                data: GetData,
                cache: false,
                beforeSend: function () {
                    if (ShowLoading != null ? ShowLoading : o.ShowLoading)
                        $(LoadingDivID).block();
                },
                success: function (data, e) {
                    if (SuccessCallBack)
                        SuccessCallBack(data, e);

                    if (ShowLoading != null ? ShowLoading : o.ShowLoading)
                        $(LoadingDivID).unblock({ fadeOut: 0 });
                },
                //success: SuccessCallBack,
                error: ErrorCallback
            });

            return 0; // Successful
        };

        this.Post = function (WebService, JsonData, SuccessCallBack, LoadingDivID, LoaderText, ErrorCallback) {
            var blnBlockWholePage = false;
            if (typeof LoadingDivID !== 'undefined' && LoadingDivID != null && LoadingDivID.toLowerCase() == 'body')
                blnBlockWholePage = true;
            else if (typeof LoadingDivID !== 'undefined' && LoadingDivID != null && LoadingDivID.charAt(0) != '.')
                LoadingDivID = '#' + LoadingDivID;

            var ShowLoading = false;
            if (typeof LoadingDivID !== 'undefined' && LoadingDivID != null && LoadingDivID != '')
                ShowLoading = true;

            if (WebService == "")
                return 1; // Incorrect Parameters

            if (ErrorCallback == null)
                ErrorCallback = o.ErrorCallBack;

            if (typeof (JsonData) === "object")
                JsonData = JSON.stringify(JsonData);

            var headers = {};
            if ($('input[name="__RequestVerificationToken"]').length > 0) {
                var token = $('input[name="__RequestVerificationToken"]').val();
                headers['__RequestVerificationToken'] = token;
            }

            $.ajax({
                type: "POST",
                url: WebService,
                data: JsonData,
                cache: false,
                //dataType: "json",
                headers: headers,
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    if (ShowLoading != null ? ShowLoading : o.ShowLoading) {
                        if (!blnBlockWholePage)
                            $(LoadingDivID).block();
                        else
                            $.blockUI({
                                message: '<img class="loadinggif" src="data:image/png;base64,R0lGODlhFAAUAIAAAP///wAAACH5BAEAAAAALAAAAAAUABQAAAIRhI+py+0Po5y02ouz3rz7rxUAOw==" width="128px" height="128px" alt="Loading">',
                                css: {
                                    padding: 0, margin: 0, width: '30%', top: '40%', left: '35%', textAlign: 'center', border: 'none', backgroundColor: 'transparent', cursor: 'wait'
                                }
                            });
                    }
                },
                //success: SuccessCallBack,
                success: function (data, e) {
                    if (SuccessCallBack)
                        SuccessCallBack(data, e);

                    if (ShowLoading != null ? ShowLoading : o.ShowLoading) {
                        if (!blnBlockWholePage)
                            $(LoadingDivID).unblock({ fadeOut: 0 });
                        else
                            $.unblockUI();
                    }
                },
                error: ErrorCallback
            });

            return 0;
        };

        return this;
    };
})(jQuery);

/*Full Screen July 20, 2015 FHD*/
function cancelFullScreen(el) {
    var requestMethod = el.cancelFullScreen || el.webkitCancelFullScreen || el.mozCancelFullScreen || el.exitFullscreen;
    if (requestMethod) { // cancel full screen.
        requestMethod.call(el);
    } else if (typeof window.ActiveXObject !== "undefined") { // Older IE.
        var wscript = new ActiveXObject("WScript.Shell");
        if (wscript !== null) {
            wscript.SendKeys("{F11}");
        }
    }
}

function requestFullScreen(el) {
    // Supports most browsers and their versions.
    var requestMethod = el.requestFullScreen || el.webkitRequestFullScreen || el.mozRequestFullScreen || el.msRequestFullscreen;

    if (requestMethod) { // Native full screen.
        requestMethod.call(el);
    } else if (typeof window.ActiveXObject !== "undefined") { // Older IE.
        var wscript = new ActiveXObject("WScript.Shell");
        if (wscript !== null) {
            wscript.SendKeys("{F11}");
        }
    }
    return false
}

function toggleFull() {
    var elem = document.body; // Make the body go full screen.
    var isInFullScreen = (document.fullScreenElement && document.fullScreenElement !== null) || (document.mozFullScreen || document.webkitIsFullScreen);

    if (isInFullScreen) {
        cancelFullScreen(document);
    } else {
        requestFullScreen(elem);
    }
    return false;
}

function dt_getdate(elem) {
    
    dateTypeVar = moment($(elem).datepicker('getDate')).format('YYYY-MM-DD hh:mm:ss');
    //return $.datepicker.formatDate('yy-mm-dd hh:mm:ss', dateTypeVar);
    return dateTypeVar;
}

function dt_getdate_small(elem) {

    dateTypeVar = moment($(elem).datepicker('getDate')).format('YYYY-MM-DD');
    //return $.datepicker.formatDate('yy-mm-dd hh:mm:ss', dateTypeVar);
    return dateTypeVar;
}


/* TRY STATE */

var tristate = {
    click: function (e) {
        var cs = $(e).data('tristate');
        if (cs < 2) {
            tristate.change(e, (cs + 1));
            return (cs + 1);
        } else {
            tristate.change(e, 0);
            return 0;
        }
    },
    change: function (e, s) {
        if (s == 0) {
            $(e).removeClass('sgh-tristate-3');
            $(e).addClass('sgh-tristate-default');
            $(e).data('tristate', 0);
            $(e).html('');
        } else if (s == 1) {
            $(e).removeClass('sgh-tristate-default');
            $(e).removeClass('sgh-tristate-3');
            $(e).data('tristate', 1);
            $(e).html('<span class="glyphicon glyphicon-ok"></span>');
        } else if (s == 2) {
            $(e).removeClass('sgh-tristate-default');
            $(e).addClass('sgh-tristate-3');
            $(e).data('tristate', 2);
            $(e).html('<span class="glyphicon glyphicon-remove"></span>');
        }
    }
}

/*DUO STATE*/

var duocheck = {
    click: function (e) {
        var stat = $(e).data('dstate');
        if (stat == 0) {
            duocheck.change(e, 1);
            return 1;
        } else if (stat == 1) {
            duocheck.change(e, 0);
            return 0;
        }

    },
    change: function (e, s) {
        if (s == 0) { //true
            $(e).removeClass('');
            $(e).addClass('sgh-tristate-default');
            $(e).data('dstate', 0);
            $(e).html('');
        } else if (s == 1) { //false
            $(e).removeClass('sgh-tristate-default');
            $(e).addClass('');
            $(e).data('dstate', 1);
            $(e).html('<span class="glyphicon glyphicon-ok"></span>');
        }
    }
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
