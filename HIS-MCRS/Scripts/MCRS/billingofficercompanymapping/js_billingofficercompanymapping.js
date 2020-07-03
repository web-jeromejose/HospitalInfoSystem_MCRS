var dtdisplaytable;
var dtdisplaytable2;
$(document).ready(function () {
    _common.getcommonlistnoclear("#billingofficer", 0, 9, "Select Billing Officer...", null, null);
    render_categorylist(null);
    render_companylist(null);

    $('#billingofficer').on('select2-selecting', function (e) {

        get_billofficercategorylist(e.val);
        get_billofficercompanylist(e.val);

        $('#btn_sel_all_cat').prop('disabled', false);
        $('#btn_sel_all_com').prop('disabled', false);
        $('#btn_desel_all_com').prop('disabled', false);
    });

    $(document).on('click', '#btn_sel_all_com', function () {
        select_all_companies();
    });

    $(document).on('click', '#btn_desel_all_com', function () {
        deselect_all_companies();
    });

    $(document).on('click', '#btn_save', function () {
        save_billofficermapping();
    });

});

function get_billofficercategorylist($ofid) {
    $param = { opeid: $ofid };
    ajaxwrapper.std($('#url').data('getbillofficercategory'), 'POST', $param,
        function () { _indicator.Show('#catdisptable'); },
        function (data) {
            dtdisplaytable.clear().draw();
            render_categorylist(data.Res);
            _indicator.Stop('#catdisptable');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop('#catdisptable'); });
        }
    );

    
}

function render_categorylist(result) {
    dtdisplaytable = $("#catdisptable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 340,
        scrollCollapse: false,
        dom: 'zRlfrtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "#", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: '5%' },
            { data: "CB", title: "", defaultContent: '<div class="sgh-tristate sgh-tristate-default" data-dstate="0" onclick="catclick(this)"></div>', targets: 1, className: "IT_custom_content_align_center", width: '10%' },
            { data: "Name", title: "Category", targets: 2, className: "IT_custom_content_align_left", width: '85%' },
            { data: "Id", title: "", targets: 3, visible:false, className: "IT_custom_content_align_center", width: '15%' },
            { data: "Assigned", title: "", defaultContent: 0, visible: false, targets: 4, className: "IT_custom_content_align_center", width: '15%' }
        ], createdRow: function (row, data) {
            if (data.Assigned == 1) {
                $(row).find('td:eq(1)').html('<div class="sgh-tristate" data-dstate="1" onclick="catclick(this)"><span class="glyphicon glyphicon-ok"></span></div>')
            } else {
                $(row).find('td:eq(1)').html('<div class="sgh-tristate sgh-tristate-default" data-dstate="0" onclick="catclick(this)"></div>')
            }
        }
    });

    dtdisplaytable.on('order.dt search.dt', function () {
        dtdisplaytable.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();

    
}

function catclick(e) {
    $row = $(e).parents('tr');
    state = duocheck.click(e);
    dtdisplaytable.cell($row, 4).data(state);
    $catid = dtdisplaytable.cell($row, 3).data();

    if (state == 1) {
        add_companytolist($catid)
    } else {
        dtdisplaytable2.rows('.cat' + $catid).remove().draw();
    }

    
}

function get_billofficercompanylist($ofid) {
    $param = { opeid: $ofid };
    ajaxwrapper.std($('#url').data('getbillofficercompany'), 'POST', $param,
        function () { _indicator.Show('#comdisptable'); },
        function (data) {
            dtdisplaytable2.clear().draw();
            render_companylist(data.Res);
            _indicator.Stop('#comdisptable');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop('#comdisptable'); });
        }
    );
}

function render_companylist(result) {
    dtdisplaytable2 = $("#comdisptable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 350,
        scrollCollapse: false,
        dom: 'zRlfrtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "#", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: '5%' },
            { data: "CB", title: "", defaultContent: '<div class="sgh-tristate sgh-tristate-default" data-dstate="0" onclick="comclick(this)"></div>', targets: 1, className: "IT_custom_content_align_center", width: '10%' },
            { data: "Name", title: "Company", targets: 2, className: "IT_custom_content_align_left", width: '85%' },
            { data: "Id", title: "", targets: 3, visible: false, className: "IT_custom_content_align_center"},
            { data: "Assigned", title: "", defaultContent: 0, visible: false, targets: 4, className: "IT_custom_content_align_center"},
            { data: "CategoryId", title: "", defaultContent: 0, visible: false, targets: 5, className: "IT_custom_content_align_center" },
            { data: "CatCode", title: "", defaultContent: 0, visible: false, targets: 6, className: "IT_custom_content_align_center" }
        ], createdRow: function (row, data) {
            if (data.Assigned == 1) {
                $(row).find('td:eq(1)').html('<div class="sgh-tristate" data-dstate="1" onclick="comclick(this)"><span class="glyphicon glyphicon-ok"></span></div>')
            } else {
                $(row).find('td:eq(1)').html('<div class="sgh-tristate sgh-tristate-default" data-dstate="0" onclick="comclick(this)"></div>')
            }

            $(row).addClass('cat' + data.CategoryId);
        }
    });
    dtdisplaytable2.on('order.dt search.dt', function () {
        dtdisplaytable2.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();

}

function comclick(e) {
    $row = $(e).parents('tr');
    state = duocheck.click(e);
    dtdisplaytable2.cell($row, 4).data(state);
}

function add_companytolist($catid) {
    
    $param = { id: $catid, ctype: 1 };
    ajaxwrapper.stdna($('#url').data('arcommon') + "/get_common_list", 'POST', $param,
        function () { _indicator.Show('#comdisptable'); },
        function (data) {
            $.each(data.CL, function (i, dx) {
                dtdisplaytable2.row.add({
                    CB: '<div class="sgh-tristate sgh-tristate-default" data-dstate="0" onclick="comclick(this)"></div>',
                    Name: dx.Name,
                    Id: dx.Id,
                    Assigned: 0,
                    CategoryId: $catid,
                    CatCode: ''
                });
            });
            dtdisplaytable2.draw();
            _indicator.Stop('#comdisptable');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
                _indicator.Stop('#comdisptable');
            });
        });
}

function select_all_companies() {
    $.each(dtdisplaytable2.rows().data(), function (i, d) {
        dtdisplaytable2.cell(i, 1).data('<div class="sgh-tristate" data-dstate="1" onclick="comclick(this)"><span class="glyphicon glyphicon-ok"></span></div>');
        dtdisplaytable2.cell(i, 4).data(1);
    });
    dtdisplaytable2.draw();
}

function deselect_all_companies() {
    $.each(dtdisplaytable2.rows().data(), function (i, d) {
        dtdisplaytable2.cell(i, 1).data('<div class="sgh-tristate sgh-tristate-default" data-dstate="0" onclick="comclick(this)"></div>');
        dtdisplaytable2.cell(i, 4).data(0);
    });
    dtdisplaytable2.draw();
}

function save_billofficermapping() {

    if ($('#billingofficer').select2('val') == 0 || $('#billingofficer').select2('val') == "") {

        ardialog.Pop(3, "Required", "Billing Officer Required!", "OK", function () { });

    }else{
        $bomlist = [];

        $.each(dtdisplaytable2.rows().data(), function (i, d) {
            if (dtdisplaytable2.cell(i, 4).data() === 1) {
                $bomlist.push({
                    CategoryId: dtdisplaytable2.cell(i, 5).data(),
                    CompanyId: dtdisplaytable2.cell(i, 3).data()
                });
            }
        });

        $para1 = {
            clist : $bomlist
        }

        $param = {
            opeid: $('#billingofficer').select2('val'),
            CL: $para1
        };

        //alert(JSON.stringify($param));

        ardialog.Confirm(2, "Confirm", "Save Billing Officer Company Mapping, Continue?", "Yes", "No",
            function () {
                ajaxwrapper.postsave($('#url').data('savebillofficer'), $param, function (data) {
                    //
                });
            }, function () { }
        );
    }
}


