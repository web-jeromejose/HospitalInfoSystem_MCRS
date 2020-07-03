
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
            console.log('#preloaderget');
            $('#preloader').show();
            $.ajax({
                type: "GET",
                url: WebService,
                data: GetData,
                cache: false,
                //beforeSend: function () {
                //    LoadingFunc();
                //},
                success: function (data, e) {
                    if (SuccessCallBack)
                        SuccessCallBack(data, e);
                    $('#preloader').hide();
                //$('div._dmain').unblock();
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
            $('#preloader').show();
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
                    $('#preloader').hide();
                },
                error: ErrorCallback
            });
         
            return 0;
        };

        this.GetWithLoading = function (url, param, target, SuccessCallBack, ErrorCallback) {

            if (url == "")
                return 1; // Incorrect Parameters

            if (ErrorCallback == null)
                ErrorCallback = o.ErrorCallBack;

            if (param != null && typeof (param) === "object")
                param = $.param(param);

           

            $.ajax({
                type: "GET",
                url: url,
                data: param,
                cache: false,
                beforeSend: function () {
                    blockUILoading(true, target);
                },
                success: function (data, e) {
                    if (SuccessCallBack) {
                        SuccessCallBack(data, e);
                    }
                    
                    blockUILoading(false, target);
                },
               
                error: function () {
                    blockUILoading(false, target);
                    return ErrorCallback;
                }
            });

            return 0; 
        };
        this.PostWithLoading = function (url, param, target, SuccessCallBack, ErrorCallback) {

            if (url == "")
                return 1; // Incorrect Parameters

            if (ErrorCallback == null)
                ErrorCallback = o.ErrorCallBack;

            if (param != null && typeof (param) === "object")
                param = $.param(param);



            $.ajax({
                type: "POST",
                url: url,
                data: param,
                cache: false,
                beforeSend: function () {
                    blockUILoading(true, target);
                },
                success: function (data, e) {
                    if (SuccessCallBack) {
                        SuccessCallBack(data, e);
                    }

                    blockUILoading(false, target);
                },

                error: function () {
                    blockUILoading(false, target);
                    return ErrorCallback;
                }
            });

            return 0;
        };

        //need refference to blockuijs
        blockUILoading = function (show, target) {

            if (show) {
                if (target) {
                    _indicator.Show(target);
                } else {
                    _indicator.Body();
                }
            } else {
                if (target) {
                    _indicator.Stop(target);
                    _indicator.Stop();
                } else {
                    _indicator.Stop()
                }
            }
        }

        return this;
    };
})(jQuery);