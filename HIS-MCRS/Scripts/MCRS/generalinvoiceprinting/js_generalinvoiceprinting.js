var $DTGENLIST;
var $ipbprintparam;
/*--------------------------------------------------------------------------------------------------
* INFO: Generate Bill List based on the admission date selected
*/
function get_GenBill_List($ispack, $billno) {
    $param = { ispack: $ispack, billno: $billno };
    ajaxwrapper.std($('#url').data('getbilllist'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            render_GenBill_List(data.Res);
            _indicator.Stop();
            $('#btn_printpreview').prop('disabled', false);
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
    );
}
function get_GenBill_List_Batch($ispack, $catid, $comid, $fdate, $tdate) {
    $param = { ispack: $ispack, catid: $catid, comid: $comid, fdate: $fdate, tdate: $tdate };
    ajaxwrapper.std($('#url').data('getbilllistbatch'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            render_GenBill_List(data.Res);
            _indicator.Stop();
            $('#btn_printpreview').prop('disabled', false);
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
    );
}
function get_GetAccountList($rtype, $catid, $fdate, $tdate, $elem, $ph) {
    $param = { rtype: $rtype, catid: $catid, fdate: $fdate, tdate: $tdate };
    ajaxwrapper.std($('#url').data('getaccountlist'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            catlist = [];
            $.each(data.Res, function (i, com) {
                catlist.push({ id: com.Id, text: com.Name });
            });
            _common.bindselect2noclear($elem, catlist, $ph);

            _indicator.Stop();
            //$('#btn_printpreview').prop('disabled', false);
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
    );
}
function get_GetAccountList2($rtype, $catid, $fdate, $tdate, $elem, $ph) {
    $param = { rtype: $rtype, catid: $catid, fdate: $fdate, tdate: $tdate };
    ajaxwrapper.std($('#url').data('getaccountlist'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            catlist2 = [];
            $.each(data.Res, function (i, com) {
                catlist2.push({ id: com.Id, text: com.Name });
            });
            _common.bindselect2noclear($elem, catlist2, $ph);

            _indicator.Stop();
            //$('#btn_printpreview').prop('disabled', false);
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
    );
}
function render_GenBill_List(result) {
    $DTGENLIST = $("#generatedbilllist").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 320,
        scrollCollapse: false,
        //dom: 'frtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: '5%' },
            { data: "CB", title: "", defaultContent: '<div class="sgh-tristate" data-dstate="1" onclick="billsprint(this)"><span class="glyphicon glyphicon-ok"></span></div>', targets: 1, className: "", width: '5%' },
            { data: "PIN", title: "PIN", defaultContent: '', targets: 2, className: "", width: '20%' },
            { data: "AdmitDate", title: "Admission Date", targets: 3, className: "IT_custom_content_align_center", width: '20%' },
            { data: "Company", title: "Company", targets: 4, className: "IT_custom_content_align_left", width: '50%' },
            { data: "BillNo", title: "", targets: 5, className: "IT_custom_content_align_left", visible: false },
            { data: "ISSEL", title: "", targets: 6, defaultContent: 1, visible: false }
        ]
    });
    $DTGENLIST.on('order.dt search.dt', function () {
        $DTGENLIST.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();
}
function billsprint(e) {
    $row = $(e).parents('tr');
    state = duocheck.click(e);
    $DTGENLIST.cell($row, 6).data(state);
}
/*--------------------------------------------------------------------------------------------------
* INFO: Report Generation | Must be executed on modal shown
*/
function setCookie2(name, value, days) {
    var expires;

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    } else {
        expires = "";
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}

function print_preview() {
    var $ismain = 0;
    $('#myReportModal').modal('show');

    $inv = $('#invtype').select2('val');

    if ($('#MainBill').is(":checked")) {
        $ismain = 0;
    }

    if ($('#BreakBill').is(":checked")) {
        $ismain = 1;
    }

    $ipbprintparam = [];
    //$ipbprintparam = {};
    $.each($DTGENLIST.rows().data(), function (i, com) {
        if ($DTGENLIST.cell(i, 6).data() == 1) {
            $ipbprintparam.push({ "ipbillno": parseInt($DTGENLIST.cell(i, 5).data()) });
        }
    });

   // alert(JSON.stringify($ipbprintparam));

    //setCookie("IPINV_BILLNO_LIST", JSON.stringify($ipbprintparam), 10);
    //document.cookie = "IPINV_BILLNO_LIST" + "=" + JSON.stringify($ipbprintparam);
    setCookie2('IPINV', JSON.stringify($ipbprintparam), 365);


    var repurl = $('#url').data('reporturl');
    //alert(full);
    var repsrc = repurl + "?rtype=1" + "&invtype=" + $inv + "&ismain=" + $ismain + "&rdisp=P";

    document.getElementById("myReport2").src = repsrc;

  
}
/*--------------------------------------------------------------------------------------------------
* INFO: Reload Page from CACHE
*/
function _clear() {
    ardialog.Confirm(2, "AR IP Billing", "This will refresh the page, be sure that you have saved what you are currently doing...<b>Continue</b>?", "Yes", "No",
        function () { location.reload(true); },
        function () { }
    );
}
/*--------------------------------------------------------------------------------------------------
* INFO: Generate Invoice
*/
/*--------------------------------------------------------------------------------------------------
* INFO: ON Ready
*/
$(document).ready(function () {
    _common.makedate("#fdate", "dd-M-yyyy");
    _common.makedate('#tdate', "dd-M-yyyy");
    _common.makepinonly("#pin");

    render_GenBill_List(null);

    $("#pin").on("keydown", function (event) {
        if (event.which == 13) {
            _common.pinenter('#pin', function () {
                DFrom = $("#fdate").datepicker('getDate');
                DTill = $("#tdate").datepicker('getDate');
                valfrom = moment(DFrom).format('DD-MMM-YYYY');
                valtill = moment(DTill).format('DD-MMM-YYYY');
                _commonExt.getadmitdtgeninv("#admitdate", $('#pin').val(), "Select Admission Date...", null);
            });
            $(this).prop('disabled', true);
        }
    });

    invttype = [
        { id: 1, text: 'Non - Package Deal' },
        { id: 2, text: 'Package Deal' }
    ];

    _common.bindselect2noclear('#invtype', invttype, 'Select Invoice Type...');

    $('#invtype').select2('data', { id: 1, text: 'Non - Package Deal' });

    $('#invtype').on('select2-selecting', function (e) {

        DFrom = $("#fdate").datepicker('getDate');
        DTill = $("#tdate").datepicker('getDate');
        valfrom = moment(DFrom).format('DD-MMM-YYYY');
        valtill = moment(DTill).format('DD-MMM-YYYY');

        setTimeout(
            function () {
                $('#pin').focus();
            }, 1 * 200);

        //if (e.val != 1) {
        //    $('#btn_generate').prop('disabled', true);
        //}
    });

    $(document).on('click', '#btn_clear', function () {  _clear(); });

    $('#admitdate').on('select2-selecting', function (e) {
        get_GenBill_List($('#invtype').select2('val'), e.val);
    });

    $('#myReport2').on("load", function () {
        _indicator.Stop("#report-container");
    });

    $('#myReportModal').on('shown.bs.modal', function () {
        _indicator.Show('#report-container');
    });

    $(document).on('click', '#btn_printpreview', function () {
        print_preview();
    });

    $('#fdate').datepicker()
        .on('changeDate', function (e) {
            _DFrom = $("#fdate").datepicker('getDate');
            _DTo = moment(_DFrom).add(1, 'M').subtract(1, 'days').format('DD-MMM-YYYY');
            $("#tdate").datepicker('update', _DTo);
        });

    $('#tdate').datepicker()
        .on('changeDate', function (e) {
            
            DFrom = $("#fdate").datepicker('getDate');
            DTill = $("#tdate").datepicker('getDate');
            valfrom = moment(DFrom).format('DD-MMM-YYYY');
            valtill = moment(DTill).format('DD-MMM-YYYY');

            get_GetAccountList(1, 0, valfrom, valtill, '#category', 'Select Category...');

           

        });

    $('#category').on('select2-selecting', function (e) {
        DFrom = $("#fdate").datepicker('getDate');
        DTill = $("#tdate").datepicker('getDate');
        valfrom = moment(DFrom).format('DD-MMM-YYYY');
        valtill = moment(DTill).format('DD-MMM-YYYY');
        get_GenBill_List_Batch($('#invtype').select2('val'), e.val, 0, valfrom, valtill);

        get_GetAccountList2(2, e.val, valfrom, valtill, '#company', 'Select Company...');
    });
    

    $('#company').on('select2-selecting', function (e) {
        DFrom = $("#fdate").datepicker('getDate');
        DTill = $("#tdate").datepicker('getDate');
        valfrom = moment(DFrom).format('DD-MMM-YYYY');
        valtill = moment(DTill).format('DD-MMM-YYYY');
        get_GenBill_List_Batch($('#invtype').select2('val'), $('#category').select2('val'), e.val, valfrom, valtill);
    });

});


