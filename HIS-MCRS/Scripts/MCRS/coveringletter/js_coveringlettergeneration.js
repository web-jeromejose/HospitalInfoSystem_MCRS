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

    _common.getcommonlistnoclear('#category', 0, 2, 'Select Category...', null, null);

    $('#category').on('select2-selecting', function (e) {

        _common.getcommonlistnoclearALL('#company', e.val, 1, 'Select Company...', null, null);

        //DFrom = $("#fdate").datepicker('getDate');
        //DTill = $("#tdate").datepicker('getDate');
        //valfrom = moment(DFrom).format('DD-MMM-YYYY');
        //valtill = moment(DTill).format('DD-MMM-YYYY');
        //$btype = $('#btype').select2('val');
        //$fd = valfrom;
        //$td = valtill;

        //_commonExt.getclcompanynoclearALL('#company', e.val, $btype, $fd, $td, 'Select Company...', null, null)
    });

    $('#company').on('select2-selecting', function (e) {

        $('#btn_generate').prop('disabled', false);
        //$('#btn_printpreview').prop('disabled', false);
    });

    $('#myReport2').on("load", function () {
        //

        _indicator.Stop("#report-container");

        //setTimeout(function () {
        //    if (_PrintTrigger == 0) {
        //        $('#myReport2').contents().find('#Print').trigger("click");
        //        _PrintTrigger = 1;
                
        //    }
        //}, 1 * 1000);

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

    $(document).on('click', '#btn_generate', function () {
        generate_covering_letter();
        $('#btn_generate').prop('disabled', true);
    });

    $(document).on('click', '#btn_printpreview', function () {
        //print_preview();
        ardialog.Pop(4, "CAM", "Please Use Covering Letter Printing for this Option", "OK", function () { ; });
    });

    $('#progressbar_modal').on('shown.bs.modal', function () {
        $('#progress_ok').prop('disabled', true);
        _PROGRESS_START = 1;
        $('#covprog').addClass('active');
        $('#covprog').attr('aria-valuenow', 0).css('width', 0 + '%');
        $('#covprog').html('0.00 %');
        setTimeout(generation_progress(), 1 * 1000);
    });

    $('#progressbar_modal').on('hide.bs.modal', function () {
        $('#covprog').attr('aria-valuenow', 0).css('width', 0 + '%');
        $('#covprog').html('0.00 %');
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
* PROGRESS BAR Script
*/

function generation_progress() {
    if (_PROGRESS_START == 1) {
        $catid = $('#category').select2('val');
        $param = {
            catid: $catid
        }
        ajaxwrapper.stdnasync($('#url').data('clprogress'), 'POST', $param,
            function () { },
            function (data) {
                prog = prog + data.Res.GenStat;

                if (prog >= 100) {
                    prog = 100;
                }
                //console.log(prog);
                $('#covprog').attr('aria-valuenow', prog).css('width', prog + '%');
                $('#covprog').html((prog * 1).toFixed(2) + ' %')

                if (prog >= 100) {
                    stopprogress();
                    ardialog.Pop(1, "Success", "Batch Covering Letter Generation has been Completed!", "OK", function () {});

                } else {
                    setTimeout(generation_progress, 1 * 250);
                }

            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () {

                });
            });
    }
}

function stopprogress() {
    $('#progress_ok').prop('disabled', false);
    _PROGRESS_START = 0;
    $('#covprog').removeClass('active');
    //clearInterval(start_prog);
}

/* -----------------------------------------------------------------------------------------------------------------
* COVERING LETTER GENERATION Script 
* version: 1.0
* date: Jan. 18, 2016
* modified date: - Jan. 19, 2016
* - added progress bar for batch printing.
*   TODO #1 : should integrate for single generation.
*
*/

function generate_covering_letter() {
    DFrom = $("#fdate").datepicker('getDate');
    DTill = $("#tdate").datepicker('getDate');

    catid = $('#category').select2('val');
    comid = $('#company').select2('val');
    btype = $('#btype').select2('val');

    fdate = moment(DFrom).format('DD-MMM-YYYY');
    tdate = moment(DTill).format('DD-MMM-YYYY');

    if (comid == 0) {
        //batch.3
        batch_generation(fdate, tdate, catid, comid, btype);

    } else {
        single_generation(fdate, tdate, catid, comid, btype);
    }

}

function single_generation($fdate, $tdate, $catid, $comid, $btype) {
    $param = {
        fdate: $fdate,
        tdate: $tdate,
        catid: $catid,
        comid: $comid,
        btype: $btype
    }
    ardialog.Confirm(2, "Confirm", "Generate Covering Letter?", "Yes", "No",
        function () {
            ajaxwrapper.stdnasync($('#url').data('generatecovletter'), 'POST', $param,
                function () {
                    _indicator.Body();
                },
                function (data) {
                    if (data.rcode == 0) {
                        ardialog.Pop(1, "Success", data.rmsg, "OK", function () { _indicator.Stop(); });
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

function batch_generation($fdate, $tdate, $catid, $comid, $btype) {
    $param = {
        fdate: $fdate,
        tdate: $tdate,
        catid: $catid,
        comid: $comid,
        btype: $btype
    }
    ardialog.Confirm(2, "Confirm", "Generate Covering Batch Letter?", "Yes", "No",
        function () {
            $('#progressbar_modal').modal('show');
            ajaxwrapper.stdnasync($('#url').data('generatecovletter_batch'), 'POST', $param,
                function () { },
                function (data) {
                    if (data.rcode == 0) {
                        //ardialog.Pop(1, "Success", data.rmsg, "OK", function () { });
                    }
                    if (data.rcode > 0) {
                        ardialog.Pop(4, "CAM", data.rmsg, "OK", function () {
                            $('#progressbar_modal').modal('hide');
                            setTimeout(stopprogress(), 1 * 300);
                        });
                    }
                },
                function (err) {
                    ardialog.Pop(4, "Error", err, "OK", function () { });
                });

        }, function () { }
    );
}

/* -----------------------------------------------------------------------------------------------------------------
* COVERTING LETTER REPORT GENERATION Script (Printing)
* - able to generate single or batch report
*/
/*Report*/
//function print_preview() {
//    _indicator.Show("#report-container");
//    $btype = $('#btype').select2('val');
//    $ca = $('#category').select2('val');
//    $co = $('#company').select2('val');

//    DFrom = $("#fdate").datepicker('getDate');
//    DTill = $("#tdate").datepicker('getDate');
//    valfrom = moment(DFrom).format('DD-MMM-YYYY');
//    valtill = moment(DTill).format('DD-MMM-YYYY');

//    $fd = valfrom;
//    $td = valtill;

//    var repurl = $('#url').data('reporturl');
//    //alert(full);
//    var repsrc = repurl + "?rtype=10" + "&catid=" + $ca + "&comid=" + $co + "&btype=" + $btype + "&fdate=" + $fd + "&tdate=" + $td + "&rdisp=P";

//    document.getElementById("myReport2").src = repsrc;

//    $('#btn_printpreview').prop('disabled', true);
//    $('#btn_generate').prop('disabled', true);
//}
