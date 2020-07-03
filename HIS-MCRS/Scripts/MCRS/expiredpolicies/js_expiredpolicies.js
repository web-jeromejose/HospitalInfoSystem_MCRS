var dtdisplaytable;
$(document).ready(function () {

    render_exppolicy(null);

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

});

function get_exppolicy($exptype) {
    $param = { rtype: $exptype };
    ajaxwrapper.std($('#url').data('getexppolicies'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            dtdisplaytable.clear().draw();
            render_exppolicy(data.Res);
            _indicator.Stop();
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
    );
}

function render_exppolicy(result) {
    

    dtdisplaytable = $("#dtdisplaytable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 420,
        scrollCollapse: false,
        //dom: 'frtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "ar_textalign_center", width: '5%' },
            { data: "Company", title: "Company", targets: 1, className: "", width: '50%' },
            { data: "PolicyNo", title: "Policy No", targets: 2, className: "IT_custom_content_align_center", width: '15%' },
            { data: "ValFrom", title: "Valid From", targets: 3, className: "IT_custom_content_align_center", width: '15%' },
            { data: "ValTill", title: "Valid Till", targets: 4, className: "IT_custom_content_align_center", width: '15%' }
        ]
    });
    dtdisplaytable.on('order.dt search.dt', function () {
        dtdisplaytable.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();

}



function print_preview() {

    $('#myModal').modal('show');
    $exp = 0;
    if ($('#exp_new').is(':checked')) {
        $exp = 1;
    }
    if ($('#exp_terminated').is(':checked')) {
        $exp = 2;
    }
    if ($('#exp_expired').is(':checked')) {
        $exp = 3;
    }


    var repurl = $('#url').data('reporturl');;
    var repsrc = repurl + "?rtype=3" + "&exptype=" + $exp + "&rdisp=P";

    document.getElementById("myReport2").src = repsrc;

}
