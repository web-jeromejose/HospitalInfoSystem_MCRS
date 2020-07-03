/* [ Ajax Wrapper]
    Version: 1.0
    By: FHD
    Date: Mar-14-2015
    
    Guide:
    std = standard
        a standard ajaxwrapper to call and perform using callback as success statement to be executed.
*/

var ajaxwrapper = {
    std: function ($awURL, $awType, $awParam, $awBSendCallB, $awSucCallB, $awErrCallB) {
        $.ajax({
            url: $awURL,
            type: $awType,
            cache: false,
            dataType: 'json',
            data: $awParam,
            beforeSend: function(){
                $awBSendCallB();
            },
            success: function (data) {
                $awSucCallB(data);
            },
            error: function (err) {
                $awErrCallB;
            }
        });
    },
    std2: function ($awURL, $awType, $awParam, $awBSendCallB, $awSucCallB, $awErrCallB) {
        var SA = JSON.stringify($awParam);
        $.ajax({
            url: $awURL,
            type: $awType,
            cache: false,
            dataType: 'json',
            data: SA,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                if ($awBSendCallB != null){
                    $awBSendCallB();
                }
            },
            success: function (data) {
                $awSucCallB(data);
            },
            error: function (err) {
                $awErrCallB;
            }
        });
    },
    stdna: function ($awURL, $awType, $awParam, $awBSendCallB, $awSucCallB, $awErrCallB) {
        $.ajax({
            url: $awURL,
            type: $awType,
            cache: false,
            dataType: 'json',
            data: $awParam,
            beforeSend: function () {
                $awBSendCallB();
            },
            success: function (data) {
                $awSucCallB(data);
            },
            error: function (err) {
                $awErrCallB;
            }
        });
    },
    stdnasync: function ($awURL, $awType, $awParam, $awBSendCallB, $awSucCallB, $awErrCallB) {
        $.ajax({
            url: $awURL,
            type: $awType,
            cache: false,
            dataType: 'json',
            data: $awParam,
            async: true,
            beforeSend: function () {
                $awBSendCallB();
            },
            success: function (data) {
                $awSucCallB(data);
            },
            error: function (err) {
                $awErrCallB;
            }
        });
    },
    stdj: function ($awURL, $awType, $awParam, $awBSendCallB, $awSucCallB, $awErrCallB) {
        $.ajax({
            url: $awURL,
            type: $awType,
            cache: false,
            data: $awParam,
            beforeSend: function () {
                $awBSendCallB();
            },
            success: function (data) {
                $awSucCallB(data);
            },
            error: function (err) {
                $awErrCallB;
            }
        });
    },
    post: function ($awURL, $awEL, $awParam) {
        var SA = JSON.stringify($awParam);
        var token = $('input[name="__RequestVerificationToken"]').val();
        var headers = {};
        headers["__RequestVerificationToken"] = token;
        $.ajax({
            url: $awURL,
            data: SA,
            type: 'post',
            //dataType: 'json',
            headers: headers,
            contentType: 'application/json; charset=utf-8',
            cache: false,
            beforeSend: function () {
                _indicator.Body();
            },
            success: function (data) {
                if (data.eType == 1) {
                    ardialog.Pop(1, "Success", data.content, "OK", function () { _indicator.Stop(); });
                } else {
                    ardialog.Pop(4, "Error", data.content, "OK", function () { _indicator.Stop(); });
                }
            },
            error: function (err) {
               
                ardialog.Pop(4, "Error", "Description: " + err, "OK", function () { _indicator.Stop(); });
            }
        });
    },
    post2: function ($awURL, $awParam, $callB){
        var SA = JSON.stringify($awParam);
        $.ajax({
            url: $awURL,
            data: SA,
            type: 'post',
            //dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            cache: false,
            beforeSend: function () {
                _indicator.Body();
            },
            success: function (data) {
                if (data.ResT == 1) {
                    if (data.regpin != undefined && $.trim(data.regpin).length > 0) {
                        $('#regpin').val(data.regpin);
                    }
                    ardialog.Pop(1, "Success", data.content, "OK", function (data) { _indicator.Stop(); setTimeout($callB, 1 * 250) });
                } else {
                    ardialog.Pop(4, "Error", data.Err, "OK", function () { _indicator.Stop(); });
                }
            },
            error: function (err) {
                ardialog.Pop(4, "Error", "Description: " + err, "OK", function () { _indicator.Stop(); });
            }
        });
    },
    post3: function ($awURL, $awParam, $callB) {
        var SA = JSON.stringify($awParam);
        $.ajax({
            url: $awURL,
            data: SA,
            type: 'post',
            //dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            cache: false,
            beforeSend: function () {
                _indicator.Body();
            },
            success: function (data) {
                alert('here');
                if (data.Err == null) {
                    ardialog.Pop(1, "Success",
                        "Bill No : " + data.Res.Billno + '<br>Amount : ' + data.Res.Amount,
                        "OK",
                        function (data) { _indicator.Stop(); setTimeout($callB, 1 * 250) });
                } else {
                    ardialog.Pop(4, "Error", data.Err, "OK", function () { _indicator.Stop(); });
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert();
                ardialog.Pop(4, "Error", "Description: " + ajaxOptions + " " + xhr.responseText + " " + thrownError, "OK", function () { _indicator.Stop(); });
            }
        });
    },
    postn: function ($awURL, $awParam) {
        var SA = JSON.stringify($awParam);
        $.ajax({
            url: $awURL,
            type: "POST",
            cache: false,
            data: SA,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () { _indicator.Body(); },
            success: function (response) {
                _indicator.Stop();
                if (response.eType == 1) {
                    ardialog.Pop(1, "Success", response.content, "OK", function () { });
                } else {
                    ardialog.Pop(4, "Error", response.content, "OK", function () { });
                }
            },
            error: function (xhr, desc, err) {
                _indicator.Stop();
                ardialog.Pop(4, "Error", "Description: " + xhr + desc + err, "OK", function () { });
            }
        });
    },
    postnc: function ($awURL, $awParam, $callBack) {
        var SA = JSON.stringify($awParam);
        $.ajax({
            url: $awURL,
            type: "POST",
            cache: false,
            data: SA,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () { _indicator.Body(); },
            success: function (response) {
                _indicator.Stop();
                if (response.eType == 1) {
                    ardialog.Pop(1, "Success", response.content, "OK", function () { });
                    $callBack();
                } else {
                    ardialog.Pop(4, "Error", response.content, "OK", function () { });
                }
            },
            error: function (xhr, desc, err) {
                _indicator.Stop();
                ardialog.Pop(4, "Error", "Description: " + xhr + desc + err, "OK", function () { });
            }
        });
    },
    postsave: function ($awURL, $awParam, $callB) {
        var SA = JSON.stringify($awParam);
        $.ajax({
            url: $awURL,
            type: "POST",
            cache: false,
            data: SA,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () { _indicator.Body(); },
            success: function (data) {
                if (data.rcode == 0) {
                    ardialog.Pop(1, "Success", data.rmsg, "OK", function () { _indicator.Stop(); setTimeout($callB, 1 * 250) });
                } else {
                    ardialog.Pop(4, "Error", data.rmsg, "OK", function () { _indicator.Stop(); });
                }
            },
            error: function (xhr, desc, err) {
                _indicator.Stop();
                ardialog.Pop(4, "Error", "Description: " + xhr + desc + err, "OK", function () { });
            }
        });
    },
    postnoindi: function ($awURL, $awParam) {
        var SA = JSON.stringify($awParam);
        $.ajax({
            url: $awURL,
            type: "POST",
            cache: false,
            data: SA,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () { },
            success: function (response) {
                if (response.eType == 1) {
                    ardialog.Pop(1, "Success", response.content, "OK", function () { });
                } else {
                    ardialog.Pop(4, "Error", response.content, "OK", function () { });
                }
            },
            error: function (xhr, desc, err) {
                ardialog.Pop(4, "Error", "Description: " + xhr + desc + err, "OK", function () { });
            }
        });
    },
    stdnanew: function ($awURL, $awType, $awParam, $awBSendCallB, $awSucCallB, $awErrCallB) {
        $.ajax({
            url: hissystem.appsserver() + hissystem.appsname() + $awURL,
            type: $awType,
            cache: false,
            dataType: 'json',
            data: $awParam,
            beforeSend: function () {
                $awBSendCallB;
            },
            success: function (datasel) {
                $awSucCallB(datasel.CLSEL);
            },
            error: function (err) {
                $awErrCallB;
            }
        });
    },
    postsearch: function ($awURL, $awParam, $callB, $inditarget) {
        var SA = JSON.stringify($awParam);
        $.ajax({
            url: $awURL,
            type: "POST",
            cache: false,
            data: SA,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                if ($inditarget != null) {
                    _indicator.Show($inditarget);
                } else {
                    _indicator.Body();
                }
            },
            success: function (data) {
                if (data.Res.length == 0) {
                    ardialog.Pop(4, "Error", 'No Record Found!', "OK", function () {
                        if ($inditarget != null) {
                            _indicator.Stop($inditarget);
                        } else {
                            _indicator.Stop();
                        }
                    });
                } else {
                    $callB(data);
                    if ($inditarget != null) {
                        _indicator.Stop($inditarget);
                    } else {
                        _indicator.Stop();
                    }
                }
            },
            error: function (xhr, desc, err) {
                ardialog.Pop(4, "Error", "Description: " + xhr + desc + err, "OK", function () {
                    if ($inditarget != null) {
                        _indicator.Stop($inditarget);
                    } else {
                        _indicator.Stop();
                    }
                });
            }
        });
    }
}

