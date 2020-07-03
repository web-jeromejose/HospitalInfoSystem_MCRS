$(document).ready(function () {
    _common.makedate("#fdate", "dd-M-yyyy");
    _common.makedate('#tdate', "dd-M-yyyy");

    //reptypedata = [
    //   { id: 1, text: 'Standard - Original' },
    //   { id: 2, text: 'New Format - Detailed' }
    //];
    //_common.bindselect2noclear('#reptype', reptypedata, 'Select Report Template...');
    //$('#reptype').select2('data', { id: 1, text: 'Standard - Original' });


    invttype = [
        { id: 1, text: 'Generate Invoice' },
        { id: 2, text: 'Print After Invoice' },
        { id: 3, text: 'Print Before Invoice' }
    ];

    _common.bindselect2noclear('#invtype', invttype, 'Select Invoice Type...');

    $('#invtype').on('select2-selecting', function (e) {

        $('#category').prop('disabled', true);
        $('#company').prop('disabled', true);
        $('#pin').prop('disabled', true);

        DFrom = $("#fdate").datepicker('getDate');
        DTill = $("#tdate").datepicker('getDate');
        valfrom = moment(DFrom).format('DD-MMM-YYYY');
        valtill = moment(DTill).format('DD-MMM-YYYY');
        if (e.val != 1) {
            $('#btn_generate').prop('disabled', true);
        }

        _common.getcomlistnoclearinvoice("#category", 1, e.val, valfrom, valtill, 0, 0, "Select Category...", null);

    });

    $('#category').on('select2-selecting', function (e) {
        $('#company').prop('disabled', true);
        $('#pin').prop('disabled', true);

        invtype = $('#invtype').select2('val');

        DFrom = $("#fdate").datepicker('getDate');
        DTill = $("#tdate").datepicker('getDate');
        valfrom = moment(DFrom).format('DD-MMM-YYYY');
        valtill = moment(DTill).format('DD-MMM-YYYY');

        _common.getcomlistnoclearinvoice("#company", 2, invtype, valfrom, valtill, e.val, 0, "Select Company...", null);
    });

    $('#company').on('select2-selecting', function (e) {

        $('#pin').prop('disabled', true);

        invtype = $('#invtype').select2('val');
        catid = $('#category').select2('val');

        DFrom = $("#fdate").datepicker('getDate');
        DTill = $("#tdate").datepicker('getDate');
        valfrom = moment(DFrom).format('DD-MMM-YYYY');
        valtill = moment(DTill).format('DD-MMM-YYYY');

        _common.getcomlistnoclearinvoice("#pin", 3, invtype, valfrom, valtill, catid, e.val, "Select PIN...", function () {
            if (e.val == 0 && invtype == 1) {
                $('#pin').select2('data', { id: 0, text: 'All' });
                $('#pin').prop('disabled', true);
                $('#btn_generate').prop('disabled', false);
            }
        });

        
        
    });


    $('#pin').on('select2-selecting', function (e) {
        if ($("#invtype").select2('val') == 1) {
            $('#btn_generate').prop('disabled', false);
            $('#btn_printpreview').prop('disabled', true);
        } else {
            $('#btn_printpreview').prop('disabled', false);
        }
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

    $(document).on('click', '#btn_generate, #btn_printpreview', function () {
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
    $co = $('#company').select2('val');
    $ca = $('#category').select2('val');
    $pin = $('#pin').val();

    DFrom = $("#fdate").datepicker('getDate');
    DTill = $("#tdate").datepicker('getDate');
    valfrom = moment(DFrom).format('DD-MMM-YYYY');
    valtill = moment(DTill).format('DD-MMM-YYYY');

    $opeid = $('#hdn_opeid').val();

    $fd = valfrom;
    $td = valtill;
    
    var repurl = $('#url').data('reporturl');
    //alert(full);
    var repsrc = repurl + "?rtype=8" + "&invtype=" + $inv + "&catid=" + $ca + "&opeid=" + $opeid + "&comid=" + $co + "&pin=" + $pin + "&fdate=" + $fd + "&tdate=" + $td + "&rdisp=P";

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
