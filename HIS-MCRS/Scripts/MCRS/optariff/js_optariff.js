$(document).ready(function () {


    /*TARIFF*/
    _common.getcommonlist('#tariff', 0, 3, 'Select Tariff...', null);
    _common.getcommonlist('#service', 0, 59, 'Select Service...', null);

    $("#service").on("select2-selecting", function (e) {
        var parts = e.val.split('~');
        get_tariff_departments(parts[1]);
    });

    $("#dept").on("select2-selecting", function (e) {
        var parts = $("#service").select2('val').split('~');
        get_tariff_price(
            $("#tariff").select2('val'),
            e.val,
            parts[1]);
    });


    $(document).on('click', '#optariffpreview', function () {
        print_preview()
    });

    $('#myModal').on('shown.bs.modal', function () {
        _indicator.Show('#report-container');
    });

    $('#myReport2').on("load", function () {
       _indicator.Stop("#report-container");
    });

    render_tariffprice(null);

});

function get_tariff_departments($mtbl) {
    $param = { mtbl: $mtbl };
    ajaxwrapper.std($('#url').data('gettariffdept'), 'POST', $param,
        function () { },
        function (data) {
            dlist = [];
            $.each(data.Res, function (i, res) { dlist.push({ id: res.Id, text: res.Name }); });
            _common.bindselect2("#dept", dlist, "Select Department...");
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { });
        }
    );
}

function get_tariff_price(id, did, mtbl) {
    if ($("#tariff").select2('val') == 0) {
        ardialog.Pop(3, "Required", "Please Select Tariff!", "OK", function () { });
    } else if ($("#service").select2('val') == 0) {
        ardialog.Pop(3, "Required", "Please Select Service!", "OK", function () { });
    } else {
        $param = { tid: id, dept: did, mtbl: mtbl };
        ajaxwrapper.std($('#url').data('gettariffprice'), 'POST', $param,
            function () { _indicator.Body(); },
            function (data) { 
                render_tariffprice(data.Res);
                _indicator.Stop();
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
            }
        );
    }
}

function render_tariffprice(result) {
    tarifftbl = $("#tarifftable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 330,
        scrollCollapse: false,
        //dom: 'frtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "ar_textalign_center", width: '5%' },
            { data: "Code", title: "Code",  targets: 1, className: "ar_textalign_left", width: '10%' },
            { data: "Name", title: "Name", targets: 2, className: "ar_textalign_left", width: '40%' },
            { data: "Price", title: "Price", targets: 3, className: "IT_custom_content_align_right", width: '15%' },
            { data: "Extra", title: "", defaultContent: '', targets: 4, className: "IT_custom_content_align_right", width: '30%' }
        ]
    });
    tarifftbl.on('order.dt search.dt', function () {
        tarifftbl.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();
    
}

function print_preview() {

    if ($("#tariff").select2('val') == 0 || $("#tariff").select2('val') == '') {
        ardialog.Pop(3, "Required", "Please Select Tariff!", "OK", function () { });
    } else if ($("#service").select2('val') == 0 || $("#service").select2('val') == '' ) {
        ardialog.Pop(3, "Required", "Please Select Service!", "OK", function () { });
    } else if ($("#dept").select2('val') == 0 || $("#dept").select2('val') == '') {
        ardialog.Pop(3, "Required", "Please Select Department!", "OK", function () { });
    } else {
        $('#myModal').modal('show');
        var parts = $("#service").select2('val').split('~');

        $tid = $('#tariff').select2('val');
        $dep = $('#dept').select2('val');
        $mtbl = parts[1];
        var repurl = $('#url').data('reporturl');;
        var repsrc = repurl + "?rtype=2" + "&tid=" + $tid + "&dep=" + $dep + "&mtbl=" + $mtbl + "&rdisp=P";

        document.getElementById("myReport2").src = repsrc;
    }
}
