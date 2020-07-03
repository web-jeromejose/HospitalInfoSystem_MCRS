$(document).ready(function () {
    _common.makedate("#fdate", "dd-M-yyyy");
    _common.makedate('#tdate', "dd-M-yyyy");

    //reptypedata = [
    //   { id: 1, text: 'Standard - Original' },
    //   { id: 2, text: 'New Format - Detailed' }
    //];
    //_common.bindselect2noclear('#reptype', reptypedata, 'Select Report Template...');
    //$('#reptype').select2('data', { id: 1, text: 'Standard - Original' });

    filtertype = [
        { id: 0, text: 'ALL' },
        { id: 1, text: 'DIRECT' },
        { id: 2, text: 'INSURANCE' }
    ];

    _common.bindselect2noclear('#filtertype', filtertype, 'Select Filter...');
    $('#filtertype').select2('data', { id: 0, text: 'ALL' });


    invttype = [
        { id: 1, text: 'Out - Patient' },
        { id: 2, text: 'In - Patient' }
    ];

    _common.bindselect2noclear('#invtype', invttype, 'Select Invoice Type...');


    $('#invtype').select2('data', { id: 1, text: 'Out - Patient' });

    _common.getcommonlistnoclear('#category', 0, -200, 'Select Category...', null);

    $('#category').on('select2-selecting', function (e) {
        _common.getcommonlistnoclearALL('#company', e.val, 1, 'Select Company...', null);
    });

    $('#company').on('select2-selecting', function (e) {
        $('#btn_runreport').prop('disabled', false);
    });

    $('#myReport2').on("load", function () {
        _indicator.Stop("#report-container");
    });

    /* Upon change of date, it will automatically change the 'TO' date*/
    $('#fdate').datepicker()
        .on('changeDate', function (e) {
            _DFrom = $("#fdate").datepicker('getDate');
            _DTo = moment(_DFrom).add(1, 'M').subtract(1, 'days').format('DD-MMM-YYYY');
            $("#tdate").datepicker('update', _DTo);
        });

    $(document).on('click', '#btn_clear', function () {
        _clear();
    });

    $(document).on('click','#btn_runreport', function () {
        if ($('#catid').select2('val') == '' || $('#catid').select2('val') == 0) {
            ardialog.Pop(3, "Required", "Please select a Category ", "OK", function () { });
        } else if ($('#comid').select2('val') == '' || $('#comid').select2('val') == 0) {
            ardialog.Pop(3, "Required", "Please select a Company ", "OK", function () { });
        } else {
            print_preview();
        }
    });

});

/*Report*/
function print_preview() {
    _indicator.Show("#report-container");
    $inv = $('#invtype').select2('val');
    $filter = $('#filtertype').select2('val');
    $ca = $('#category').select2('val');
    $co = $('#company').select2('val');

    DFrom = $("#fdate").datepicker('getDate');
    DTill = $("#tdate").datepicker('getDate');
    valfrom = moment(DFrom).format('DD-MMM-YYYY');
    valtill = moment(DTill).format('DD-MMM-YYYY');
    $fd = valfrom;
    $td = valtill;

    var repurl = $('#url').data('reporturl');
    //alert(full);
    var repsrc = repurl + "?rtype=1000" + "&invtype=" + $inv + "&filtertype=" + $filter + "&catid=" + $ca + "&comid=" + $co + "&fdate=" + $fd + "&tdate=" + $td + "&rdisp=L";

    document.getElementById("myReport2").src = repsrc;

    if ($("#invtype").select('val') == 1) {
        $('#company').prop('disabled', true);
        $('#pin').prop('disabled', true);
        $('#btn_generate').prop('disabled', true);
    } else {
        $('#btn_printpreview').prop('disabled', true);
    }
}

/* Reload */
function _clear() {
    ardialog.Confirm(2, "CAM", "This will refresh the page, be sure that you have saved what you are currently doing...<br><b>Continue</b>?", "Yes", "No",
        function () { location.reload(true); },
        function () { }
    );
}
