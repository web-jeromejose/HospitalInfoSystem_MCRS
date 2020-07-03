var tbl;
function get_ar_cancellation_reasons() {
    $param = {};
    ajaxwrapper.std($('#url').data('getcancellationreasonlist'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            render_ar_cancellation_reasons(data.Res);
            _indicator.Stop();
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
        );
}
function render_ar_cancellation_reasons(result) {
    tbl = $("#displaytable").DataTable({
        destroy: true,
        data: result,
        paging: true,
        ordering: true,
        searching: true,
        info: true,
        scrollX: "100%",
        scrollY: 450,
        processing: false,
        autoWidth: false,
        scrollCollapse: false,
        order: [[1, 'asc']],
        pageLength: 150,
        columns: [
            { title: "", data: "OPCB1", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: "5%" },
            { data: "Id", title: "", visible: false, className: 'IT_custom_content_align_left', targets: 1 },
            { data: "Code", title: "Code", className: 'IT_custom_content_align_center', targets: 2, width: "10%" },
            { data: "Name", title: "Name", defaultContent: '', visible: true, targets: 3, width: "35%", className: 'IT_custom_content_align_left' },
            { data: "Type", title: "", defaultContent: '', visible: false, targets: 4, className: 'IT_custom_content_align_left' },
            { data: "TypeName", title: "Type", defaultContent: '', targets: 5, width: "30%", className: 'IT_custom_content_align_left' },
            { data: "cb", title: "", defaultContent: '<div class="sgh-dstate sgh-tristate-2" onclick="delete_ar_cancellation_reasons(this)"><span class="glyphicon glyphicon-remove"></span></div>', className: 'IT_custom_content_align_center', targets: 6, width: "6%" },
            { data: "extra", defaultContent: '', visible: true, targets: 7, width: "14%" }
        ]
    });
    tbl.on('order.dt search.dt', function () {
        tbl.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();

    $('#displaytable tbody').on('dblclick', 'td', function () {
        get_ar_cancellation_reasons_details(this);
    });

}
function save_ar_cancellation_reasons() {
    $code = $('#code').val();
    $desc = $('#name').val();
    $type = $('#type').select2('val');

    if ($.trim($code).length == 0) {
        ardialog.Pop(3, "Require", "AR Cancellation Code required!", "OK", function () { });
    } else if ($.trim($desc).length == 0) {
        ardialog.Pop(3, "Require", "AR Cancellation Name required!", "OK", function () { });
    } else {
        $param = {
            desc: $desc,
            code: $code,
            tid: $type
        };

        ajaxwrapper.std($('#url').data('savecncellationreason'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            if (data.eType == 1) {
                ardialog.Pop(1, "Success", data.content, "OK", function () { get_ar_cancellation_reasons(); });
            } else {
                ardialog.Pop(4, "Error", data.content, "OK", function () { });
            }
            _indicator.Stop();
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
        );

    }
}
function delete_ar_cancellation_reasons(cell) {
    var row = $(cell).parents('tr');
    $id = tbl.cell(row, 1).data();

    $param = {
        tid: $id
    };
    ardialog.Confirm(2, "Confirm", "Delete the Record?",
       "Delete", "Cancel",
       function () {
           ajaxwrapper.std($('#url').data('savecncellationreason'), 'POST', $param,
            function () { _indicator.Body(); },
            function (data) {
                if (data.eType == 1) {
                    ardialog.Pop(1, "Success", data.content, "OK", function () { get_ar_cancellation_reasons(); });
                } else {
                    ardialog.Pop(4, "Error", data.content, "OK", function () { });
                }
                _indicator.Stop();
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
            }
            );
       },
       function () { }
   );
}
function get_ar_cancellation_reasons_details(cell) {
    field_clear();
    var row = $(cell).parents('tr');
    $id = tbl.cell(row, 1).data();
    $code = tbl.cell(row, 2).data();
    $desc = tbl.cell(row, 3).data();
    $type = tbl.cell(row, 4).data();

    $('#hdn_id').val($id);
    $('#code').val($code);
    $('#name').val($desc);
    $('#type').select2('val', $type);

    $('#btn_save').hide();
    $('#btn_update').show();
    $('#myModal').modal('show');
}
function field_clear() {
    $('#code').val('');
    $('#name').val('');
    $('#hdn_id').val(0);
}
function update_ar_cancellation_reasons() {
    $id = $('#hdn_id').val();
    $code = $('#code').val();
    $desc = $('#name').val();
    $type = $('#type').select2('val');

    if ($.trim($code).length == 0) {
        ardialog.Pop(3, "Require", "AR Cancellation Code required!", "OK", function () { });
    } else if ($.trim($desc).length == 0) {
        ardialog.Pop(3, "Require", "AR Cancellation Name required!", "OK", function () { });
    } else {
        $param = {
            desc: $desc,
            code: $code,
            tid: $type,
            canid: $id
        };

        ajaxwrapper.std($('#url').data('updatecancellation'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            if (data.eType == 1) {
                ardialog.Pop(1, "Success", data.content, "OK", function () { get_ar_cancellation_reasons(); });
            } else {
                ardialog.Pop(4, "Error", data.content, "OK", function () { });
            }
            _indicator.Stop();
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
        );

    }
}
$(document).ready(function () {

    _common.getcommonlist('#type', 0, 30, 'Select Type...', null);

    get_ar_cancellation_reasons();

    $(document).on('click', '#btn_save', function (e) {
        save_ar_cancellation_reasons();
    });
    $(document).on('click', '#btn_update', function (e) {
        update_ar_cancellation_reasons();
    });
    $(document).on('click', '#btn_new', function (e) {
        $('#myModal').modal('show');
        field_clear();
        $('#btn_save').show();
        $('#btn_update').hide();
    });
});