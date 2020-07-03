var tbl;
$(document).ready(function () {
    _common.makedate("#fdate", "dd-M-yyyy");
    _common.makedate('#tdate', "dd-M-yyyy");

    render_ptcharityletterreport(null);

    $(document).on('click', '#btn_preview', function () {
        print_preview();
    });

    $(document).on('click', '#btn_show', function () {
       
        get_ptcharityletterreport();
    });

    $('#myModal').on('shown.bs.modal', function () {
        _indicator.Show('#report-container');
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

function get_ptcharityletterreport() {

    var DFrom = $("#fdate").datepicker('getDate');
    var DTill = $("#tdate").datepicker('getDate');
    var valfrom = moment(DFrom).format('DD-MMM-YYYY');

    var valtill = moment(DTill).format('DD-MMM-YYYY');

    $fdate = valfrom;
    $tdate = valtill;

    $param = {
        fdate: $fdate,
        tdate: $tdate,
    };

    ajaxwrapper.std($('#url').data('get_ptcharityletterreport'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            tbl.clear().draw();
            render_ptcharityletterreport(data.Res);
            _indicator.Stop();
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
                _indicator.Stop();
            });
        }
    );
}

function render_ptcharityletterreport(result) {
    tbl = $('#dtdisplaytable').DataTable({
        destroy: true,
        data: result,
        paging: true,
        autoWidth: true,
        order: [],
        searching: true,
        info: true,
        scrollX: "100%",
        scrollY: 280,
        processing: false,
        scrollCollapse: false,
        responsive: false,
        dom: 'Rlfrtip',
        ordering: false,
        order: [[1, 'asc']],
        lengthChange: false,
        pageLength: 150,
        columns: [
            { data: "OPCB1", title: "SLNO", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: "2%" },
            { data: "PIN", title: "PIN", targets: 1, className: "IT_custom_content_align_left", width: "7%" },
            { data: "PTName", title: "Patient Name", targets: 2, className: "IT_custom_content_align_left", width: "20%" },
            { data: "AdvDepNo", title: "Adv. Deposit No.", targets: 3, className: "IT_custom_content_align_center", width: "10%" },
            { data: "Amount", title: "Amount", targets: 4, className: "IT_custom_content_align_right", width: "7%" },
            { data: "SettledDeposit", title: "Set. Deposit", targets: 5, className: "IT_custom_content_align_right", width: "7%" },
            { data: "MOP", title: "MOP", targets: 6, className: "IT_custom_content_align_center", width: "12%" },
            { data: "DepDate", title: "Date", targets: 7, className: "IT_custom_content_align_center", width: "10%" },
            { data: "Code", title: "Code", targets: 8, className: "IT_custom_content_align_center", width: "5%" },
            { data: "BillDate", title: "Bill Date", targets: 9, className: "IT_custom_content_align_center", width: "10%" },
            { data: "ExpDate", title: "Exp. Date", targets: 10, className: "IT_custom_content_align_center", width: "10%" },
        ],
        footerCallback: function (tfoot, data, start, end, display) {
            var api = this.api();
            if (data.length > 0) {
                $(api.column(3).footer()).html(
                   "TOTAL : "
                );

                $(api.column(4).footer()).html(
                     CommaFormatted(api.column(4).data().reduce(function (a, b) {
                         return a + b;
                     }, 0).toFixed(2))
                );
                $(api.column(5).footer()).html(
                     CommaFormatted(api.column(5).data().reduce(function (a, b) {
                         return a + b;
                     }, 0).toFixed(2))
                );

            }
        }
    });
    tbl.on('order.dt search.dt', function () {
        tbl.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();
}

/*Report*/
function print_preview() {
    $('#myModal').modal('show');


    var DFrom = $("#fdate").datepicker('getDate');
    var DTill = $("#tdate").datepicker('getDate');
    var valfrom = moment(DFrom).format('DD-MMM-YYYY');
    var valtill = moment(DTill).format('DD-MMM-YYYY');

    $fdate = valfrom;
    $tdate = valtill;


    var repurl = $('#url').data('reporturl');
    var repsrc = repurl + "?rtype=5" + "&fdate=" + $fdate + "&tdate=" + $tdate + "&rdisp=L";
    document.getElementById("myReport2").src = repsrc;
}