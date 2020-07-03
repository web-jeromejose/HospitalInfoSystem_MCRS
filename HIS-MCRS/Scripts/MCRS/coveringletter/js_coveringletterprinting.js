var _PROGRESS_START = 0;
var _PrintTrigger = 0;
var start_prog;
var prog = 0;
$(document).ready(function () {
    _common.makedate("#fdate", "dd-M-yyyy");
    _common.makedate('#tdate', "dd-M-yyyy");

    ptype = [
       { id: 1, text: 'Both ' },
       { id: 2, text: 'OP - OutPatient' },
       { id: 3, text: 'IP - InPatient' }
    ];
    _common.bindselect2noclear('#btype', ptype, 'Select Patient Type...');
    $('#btype').select2('data', { id: 1, text: 'Both ' });

    $('#btype').on('select2-selecting', function (e) {
        $('#btn_printpreview').prop('disabled', true);
        $('#btn_ungenerate').prop('disabled', true);
        _common.getcommonlistnoclear('#category', 0, 2, 'Select Category...', null, null);
    });

    _common.getcommonlistnoclear('#category', 0, 2, 'Select Category...', null, null);

    $('#category').on('select2-selecting', function (e) {
        DFrom = $("#fdate").datepicker('getDate');
        DTill = $("#tdate").datepicker('getDate');
        valfrom = moment(DFrom).format('DD-MMM-YYYY');
        valtill = moment(DTill).format('DD-MMM-YYYY');
        $btype = $('#btype').select2('val');
        $fd = valfrom;
        $td = valtill;

        $('#btn_printpreview').prop('disabled', true);
        $('#btn_ungenerate').prop('disabled', true);

        $('#company').prop('disabled', true);

        _commonExt.getclcompanynoclearALL('#company', e.val, $btype, $fd, $td, 'Select Company...', null, null)
    });

    $('#company').on('select2-selecting', function (e) {
        DFrom = $("#fdate").datepicker('getDate');
        DTill = $("#tdate").datepicker('getDate');
        valfrom = moment(DFrom).format('DD-MMM-YYYY');
        valtill = moment(DTill).format('DD-MMM-YYYY');
        $fd = valfrom;
        $td = valtill;
        $('#btn_printpreview').prop('disabled', false);
        _commonExt.getclrefnonoclear("#refno", $('#category').select2('val'), e.val, $fd, $td, 'Select Ref. No. ...', null, null);
    }); 

    $('#refno').on('select2-selecting', function (e) {
        $('#btn_printpreview').prop('disabled', true);
        //$('#btn_ungenerate').prop('disabled', false);
        print_preview(e.val);

    });

    $('#myReport2').on("load", function () {
        _indicator.Stop("#report-container");

        //setTimeout(function () {
        //    if (_PrintTrigger == 0) {
        //        $('#myReport2').contents().find('#Print').trigger("click");
        //        _PrintTrigger = 1;

        //    }
        //}, 1 * 1000);
        if ($('#refno').select2('val') > 0) {
            $('#btn_ungenerate').prop('disabled', false);
        }

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
        print_preview(0);
    });

    $(document).on('click', '#btn_ungenerate', function () {
        ungenerate_cl($('#refno').select2('val'));
    });

});

/* Reload */
function _clear() {
    ardialog.Confirm(2, "CAM", "This will refresh the page, be sure that you have saved what you are currently doing...<br><b>Continue</b>?", "Yes", "No",
        function () { location.reload(true); },
        function () { }
    );
}

/* -----------------------------------------------------------------------------------------------------------------
* COVERTING LETTER REPORT GENERATION Script (Printing)
* - able to generate single or batch report
*/
/*Report*/
function print_preview($refid) {
    _indicator.Show("#report-container");
    $btype = $('#btype').select2('val');
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
    var repsrc = repurl + "?rtype=10" + "&refid=" + $refid + "&catid=" + $ca + "&comid=" + $co + "&btype=" + $btype + "&fdate=" + $fd + "&tdate=" + $td + "&rdisp=P";

    document.getElementById("myReport2").src = repsrc;

    $('#btn_printpreview').prop('disabled', true);
}

/* -----------------------------------------------------------------------------------------------------------------
* COVERTING LETTER UN-GENERATE Script (Delete Generated CL)
* - per reference no. only
*/
function ungenerate_cl($refid) {
    $param = {
        refid: $refid
    }
    ardialog.Confirm(2, "Confirm", "Do you want to delete the Covering Letter?", "Yes", "No",
        function () {
            $('#progressbar_modal').modal('show');
            ajaxwrapper.stdnasync($('#url').data('ungeneratecl'), 'POST', $param,
                function () { _indicator.Body(); },
                function (data) {
                    if (data.rcode == 0) {
                        ardialog.Pop(1, "Success", data.rmsg, "OK", function () {
                            _indicator.Stop();
                            $('#company').prop('disabled', true);
                            $('#refno').prop('disabled', true);
                            $('#btn_ungenerate').prop('disabled', true);
                        });
                    }
                    if (data.rcode > 0) {
                        ardialog.Pop(4, "CAM", data.rmsg, "OK", function () { _indicator.Stop(); });
                    }
                },
                function (err) {
                    ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
                });
        }, function () { }
    );
}