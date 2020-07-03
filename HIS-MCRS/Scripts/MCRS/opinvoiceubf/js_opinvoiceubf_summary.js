$(document).ready(function () {
    _common.makedate("#fdate", "dd-M-yyyy");
    _common.makedate('#tdate', "dd-M-yyyy");

    invttype = [
        { id: 1, text: 'OP - Out Patient' },
        { id: 2, text: 'IP - In Patient' }
    ];

    _common.bindselect2noclear('#invtype', invttype, 'Select Invoice Type...');

    $('#invtype').select2('data', { id: 1, text: 'OP - Out Patient' });

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

    $(document).on('click', '#btn_printpreview', function () {
        print_preview();
    });

});

/*Report*/
function print_preview() {
    _indicator.Show("#report-container");
    $inv = $('#invtype').select2('val');
    DFrom = $("#fdate").datepicker('getDate');
    DTill = $("#tdate").datepicker('getDate');
    valfrom = moment(DFrom).format('DD-MMM-YYYY');
    valtill = moment(DTill).format('DD-MMM-YYYY');

    $opeid = $('#hdn_opeid').val();

    $fd = valfrom;
    $td = valtill;

    var repurl = $('#url').data('reporturl');
    //alert(full);
    var repsrc = repurl + "?rtype=11" + "&invtype=" + $inv + "&opeid=" + $opeid + "&fdate=" + $fd + "&tdate=" + $td + "&rdisp=L";

    document.getElementById("myReport2").src = repsrc;


}

/* Reload */
function _clear() {
    ardialog.Confirm(2, "CAM", "This will refresh the page, be sure that you have saved what you are currently doing...<br><b>Continue</b>?", "Yes", "No",
        function () { location.reload(true); },
        function () { }
    );
}
