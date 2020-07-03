function show_change_station($html) {
    $('#changestationmodal').modal('show');
    $('#changestationalert').html($html);
}

function setCookie(name, value, days) {
    var expires;

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    } else {
        expires = "";
    }
    document.cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value) + expires + "; path=/";
}
function getCookie(name) {
    var nameEQ = encodeURIComponent(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return decodeURIComponent(c.substring(nameEQ.length, c.length));
    }
    return null;
}

$(document).ready(function () {
    //onready

    //$('#modulecurrentstation').html(getCookie('ARIPBILLING_HIS_STATION_NAME'));
    //alert('test');
    
    //_common.getcommonlist('#modulechangestation', 45, 50, 'Select Station...');

    //if (getCookie('ARIPBILLING_HIS_STATION') != null) {
    //    setTimeout(function () {
    //        $('#modulechangestation').select2('data', { id: getCookie('ARIPBILLING_HIS_STATION'), text: getCookie('ARIPBILLING_HIS_STATION_NAME') });
    //    }, 1 * 1000);
    //} else {
    //    $html = '<div class="alert alert-warning alert-dismissible" role="alert"> ' +
    //            '<strong>STATION NOT SET!</strong> Please select your station below.'+
    //            '</div>';
    //    show_change_station($html);
    //}

    //$(document).on('click', '#btn_change_station', function () {
    //    if ($('#modulechangestation').select2('val') != "") {
    //        var $sta = $('#modulechangestation').select2('data');

    //        setCookie('ARIPBILLING_HIS_STATION', $('#modulechangestation').select2('val'), 90);
    //        setCookie('ARIPBILLING_HIS_STATION_NAME', $sta.text, 90);

    //        ardialog.Pop(1, "Success", "Station has been set!", "OK", function () { location.reload(); });
    //    }
    //});

    //setTimeout(function () {
    //    toggleFull();
    //}, 1 * 500);
    //$(document).on('click', '.AR_apps_about_btn', function () {
    //    if ($('.AR_apps_about_btn').data('appsopen') == 0) {
    //        $('.AR_apps_about_btn').data('appsopen', 1);
    //        $('.AR_apps_about').css('display', 'block');
    //    } else {
    //        $('.AR_apps_about_btn').data('appsopen', 0);
    //        $('.AR_apps_about').css('display', 'none');
    //    }
    //});
 
});