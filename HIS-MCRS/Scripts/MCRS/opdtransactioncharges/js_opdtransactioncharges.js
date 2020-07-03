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
                    $('#agesex').val(data.Res.AgeT);
                });


                _DFrom = $("#fdate").datepicker('getDate');
                _DF = moment(_DFrom).format('DD-MMM-YYYY');

                _DTo = $("#tdate").datepicker('getDate');
                _DT = moment(_DTo).format('DD-MMM-YYYY');

                $exp = $('#btype').select2('val');

                get_opdcharges($exp, $("#pin").val(), _DF, _DT);
            });
        }
    });

    render_opdcharges(null);

    $(document).on('click', '#reportpreview', function () {
        print_preview();
    });

    $('#myModal').on('shown.bs.modal', function () {
        _indicator.Show('#report-container');
    });

    $('#myReport2').on("load", function () {
        _indicator.Stop("#report-container");
    });

    $('#fdate').datepicker()
       .on('changeDate', function (e) {
           _DFrom = $("#fdate").datepicker('getDate');
           _DTo = moment(_DFrom).add(1, 'M').subtract(1, 'days').format('DD-MMM-YYYY');
           $("#tdate").datepicker('update', _DTo);
           $("#pin").focus();
       });

    BT = [
        { id:1, text:'CASH'} , 
        { id:2, text:'CREDIT'} , 
        { id:3, text:'BOTH'} , 
    ]

    _common.bindselect2noclear("#btype", BT);

    $('#btype').select2('data', { id: 3, text: 'BOTH' });

    $('#btype').on('select2-selecting', function (e) {
        if ($("#pin").val() > 0) {
            _DFrom = $("#fdate").datepicker('getDate');
            _DF = moment(_DFrom).format('DD-MMM-YYYY');

            _DTo = $("#tdate").datepicker('getDate');
            _DT = moment(_DTo).format('DD-MMM-YYYY');

            $exp = e.val;
            get_opdcharges($exp, $("#pin").val(), _DF, _DT);
        } else {
            $("#pin").focus();
        }        
    });
});

function get_opdcharges($ctype, $pin, $fdate, $tdate) {
    //alert($fdate + ' ' + $tdate + ' ' + $pin);
    $param = {act: $ctype, pin: $pin, fdate: $fdate, tdate: $tdate };
    ajaxwrapper.std($('#url').data('getopdtranscharges'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            //dtdisplaytable.clear().draw();
            render_opdcharges(data.Res);
            _indicator.Stop();
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
    );
}

function render_opdcharges(result) {

    dtdisplaytable = $("#dtdisplaytable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 215,
        scrollCollapse: false,
        //dom: 'frtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "SLNO", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: '3%' },
            { data: "BillNo", title: "Bill No", targets: 1, className: "IT_custom_content_align_center", width: '8%' },
            { data: "Seq", title: "Seq", targets: 2, className: "IT_custom_content_align_center", width: '2%' },
            { data: "Company", title: "Company", targets: 3, className: "IT_custom_content_align_center", width: '5%' },
            { data: "BillDateTime", title: "Bill Date", targets: 4, className: "IT_custom_content_align_center", width: '10%' },
            { data: "ItemCode", title: "Code", targets: 5, className: "", width: '7%' },
            { data: "ItemName", title: "Name", targets: 6, className: "", width: '30%' },
            { data: "Quantity", title: "Qty", targets: 7, className: "IT_custom_content_align_center", width: '15%' },
            { data: "BillAmount", title: "Amount", targets: 8, className: "IT_custom_content_align_right", width: '10%' },
            { data: "Balance", title: "Net", targets: 9, className: "IT_custom_content_align_right", width: '10%' },
            { data: "Type", title: "Type", targets: 10, visible: false, className: "IT_custom_content_align_right" },
            { data: "PaidAmount", title: "", targets: 11, visible: false, className: "IT_custom_content_align_right" },
            { data: "CanAmount", title: "", targets: 12, visible: false, className: "IT_custom_content_align_right" }
        ],
        createdRow: function (row, data) {
            if (data.Type == 1) {
                $(row).find('td:eq(0)').addClass('warning');
            } else {
                $(row).find('td:eq(0)').addClass('success');
            }
        },
        footerCallback: function (tfoot, data, start, end, display) {
            var api = this.api();

                $(api.column(7).footer()).html(
                    api.column(8)
                        .data()
                        .reduce(function (a, b) {
                        return a + b;
                    }, 0).toFixed(2)
                );
                $('#grossamt').val(
                    api.column(8)
                        .data()
                        .reduce(function (a, b) {
                            return a + b;
                        }, 0).toFixed(2)
                );
                $('#tolamtpd').val(
                   (api.column(8)
                        .data()
                        .reduce(function (a, b) {
                            return a + b;
                        }, 0).toFixed(2)) - (api.column(12)
                        .data()
                        .reduce(function (a, b) {
                            return a + b;
                        }, 0).toFixed(2))
                );

                var secondRow = $(tfoot).next()[0];
                var nCells = secondRow.getElementsByTagName('td');
                nCells[7].innerHTML = (
                         api.column(12)
                        .data()
                        .reduce(function (a, b) {
                            return a + b;
                        }, 0).toFixed(2)
                    );
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
    $bt = $('#btype').select2('val');

    var repurl = $('#url').data('reporturl');;
    var repsrc = repurl + "?rtype=6" + "&pin=" + $pn + "&btype=" + $bt + "&fdate=" + $fd + "&tdate=" + $td + "&rdisp=L";

    document.getElementById("myReport2").src = repsrc;
}
