$(document).ready(function () {
    _common.makedate("#fdate", "dd-M-yyyy");
    _common.makedate('#tdate', "dd-M-yyyy");

    reptypedata = [
       { id: 1, text: 'Standard - Original' },
       { id: 2, text: 'New Format - Detailed' }
    ];
    _common.bindselect2noclear('#reptype', reptypedata, 'Select Report Template...');
    $('#reptype').select2('data', { id: 1, text: 'Standard - Original' });


    ptypedata = [
        { id: 1, text: 'IP - Inpatient' },
        { id: 2, text: 'OP - Outpatient' }
    ];

    _common.bindselect2noclear('#ptype', ptypedata, 'Trans. Type...');
    $('#ptype').select2('data', { id: 1, text: 'IP - Inpatient' });

    _common.getcommonlist('#catid', 0, 2, 'Select Category...', null);

    $('#catid').on('select2-selecting', function (e) {
        _common.getcommonlist('#comid', e.val, 1, 'Select Company...', null);
    });

    $(document).on('click', '#btn_print', function () {
        if ($('#catid').select2('val') == '' || $('#catid').select2('val') == 0) {
            ardialog.Pop(3, "Required", "Please select a Category ", "OK", function () { });
        } else if ($('#comid').select2('val') == '' || $('#comid').select2('val') == 0) {
            ardialog.Pop(3, "Required", "Please select a Company ", "OK", function () { });
        } else {
            print_preview();
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

});


/*Report*/
function print_preview() {
    _indicator.Show("#report-container");
    $pt = $('#ptype').select2('val');
    $co = $('#comid').select2('val');
    $fd = $('#fdate').val();
    $td = $('#tdate').val();
    $rf = $('#reptype').select2('val');
    var $cl = 0;
    if ($('#aftercl').is(':checked')) {
        $cl = 1;
    }
    var repurl = $('#url').data('reporturl');
    //alert(full);
    var repsrc = repurl + "?rtype=1" + "&ptype=" + $pt + "&ftype=" + $cl + "&reptype=" + $rf + "&comid=" + $co + "&fdate=" + $fd + "&tdate=" + $td + "&rdisp=L";

    document.getElementById("myReport2").src = repsrc;
}