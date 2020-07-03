var dtdisplaytable;
$(document).ready(function () {

    _common.makedate("#fdate", "dd-M-yyyy");
    _common.makedate("#tdate", "dd-M-yyyy");
    
    _common.makepinonly('#pin');
    $("#pin").on("keydown", function (event) {
        if (event.which == 13) {
            _common.pinenter('#pin', function () {
                

                _common.getPTComPTName($("#pin").val(), function (data) {
                    $('#ptname').val(data.Res.PTName);
                });

                


                _DFrom = $("#fdate").datepicker('getDate');
                _DF = moment(_DFrom).format('DD-MMM-YYYY');

                _DTo = $("#tdate").datepicker('getDate');
                _DT = moment(_DTo).format('DD-MMM-YYYY');

                get_pingrosscreated($("#pin").val(), _DF, _DT);
            });
        }
    });

    render_pingross(null);

    $(document).on('click', '#reportpreview', function () {
        print_preview()
    });

    $('#myModal').on('shown.bs.modal', function () {
        _indicator.Show('#report-container');
    });

    $('#myReport2').on("load", function () {
        _indicator.Stop("#report-container");
    });

    $('#exp_new, #exp_terminated, #exp_expired').on('click', function () {
        $exp = 0
        if ($('#exp_new').is(':checked')) {
            $exp = 1;
        }
        if ($('#exp_terminated').is(':checked')) {
            $exp = 2;
        }
        if ($('#exp_expired').is(':checked')) {
            $exp = 3;
        }
        get_exppolicy($exp);
    });

    $('#fdate').datepicker()
       .on('changeDate', function (e) {
           _DFrom = $("#fdate").datepicker('getDate');
           _DTo = moment(_DFrom).add(1, 'M').subtract(1, 'days').format('DD-MMM-YYYY');
           $("#tdate").datepicker('update', _DTo);
           $("#pin").focus();
       });


});

function get_pingrosscreated($pin, $fdate, $tdate) {
    //alert($fdate + ' ' + $tdate + ' ' + $pin);
    $param = { pin: $pin, fdate: $fdate, tdate: $tdate };
    ajaxwrapper.std($('#url').data('getpingrosscreated'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            //dtdisplaytable.clear().draw();
            render_pingross(data.Res);
            _indicator.Stop();
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
    );
}

function render_pingross(result) {

    dtdisplaytable = $("#dtdisplaytable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 290,
        scrollCollapse: false,
        //dom: 'frtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "SLNO", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: '5%' },
            { data: "BillNo", title: "Bill No", targets: 1, className: "IT_custom_content_align_center", width: '10%' },
            { data: "BillDateTime", title: "Bill Date", targets: 2, className: "IT_custom_content_align_center", width: '10%' },
            { data: "Gross", title: "Gross", targets: 3, className: "IT_custom_content_align_right", width: '10%' },
            { data: "Discount", title: "Discount", targets: 4, className: "IT_custom_content_align_right", width: '10%' },
            { data: "Deductable", title: "Deductable", targets: 5, className: "IT_custom_content_align_right", width: '10%' },
            { data: "Net", title: "Net Amount", targets: 6, className: "IT_custom_content_align_right", width: '10%' },
            { data: "Company", title: "Company", targets: 7, className: "IT_custom_content_align_left", width: '35%' },
        ],
        "footerCallback": function (tfoot, data, start, end, display) {
            var api = this.api();
            if (data.length > 0) {
                $(api.column(2).footer()).html(
                   "TOTAL : "
                );

                $(api.column(3).footer()).html(
                    api.column(3).data().reduce(function (a, b) {
                        return a + b;
                    }, 0).toFixed(2)
                );

                $(api.column(4).footer()).html(
                    api.column(4).data().reduce(function (a, b) {
                        return a + b;
                    }, 0).toFixed(2)
                );

                $(api.column(5).footer()).html(
                    api.column(5).data().reduce(function (a, b) {
                        return a + b;
                    }, 0).toFixed(2)
                );
                $(api.column(6).footer()).html(
                    api.column(6).data().reduce(function (a, b) {
                        return a + b;
                    }, 0).toFixed(2)
                );
            }
                
            
        }
    });
    dtdisplaytable.on('order.dt search.dt', function () {
        dtdisplaytable.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();

}



function print_preview() {

    $('#myModal').modal('show');
    $fd = $('#fdate').val();
    $td = $('#tdate').val();
    $pn = $('#pin').val();


    var repurl = $('#url').data('reporturl');;
    var repsrc = repurl + "?rtype=4" + "&pin=" + $pn + "&fdate=" + $fd + "&tdate=" + $td + "&rdisp=L";

    document.getElementById("myReport2").src = repsrc;

}
