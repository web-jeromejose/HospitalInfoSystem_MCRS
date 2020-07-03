var dtdisplaytable;
var dtdisplaytable2;
var dtbillstoprint;
var arcancellationreason = null;
var sel2billno;

var btoprint;
var $opbprintparam;

var $billpar;
var _tmp_itemprice;
var _tmp_itembatch;
var _tmp_deptid;
var _tmp_markper;
var _tmp_markamt;
var _tmp_modifytype = 2; //Financial

/* [GLOBAL VAR]  
   Service Id of Pharmacy
 */
var _GLOBAL_PHARMACY_SERVICEID = 11;

var editrec = {
    price: function () {
        //should recalculate the discount and deductables
        editrec.discP();
        editrec.dedP();
    },
    discP: function () {
        var _itemprice1 = $('#itemprice').val() * $('#itemqty').val();
        var _discamount = (_itemprice1 * ($('#itemdisc').val() / 100)).toFixed(2);
        $('#itemdiscamt').val(_discamount);

    },
    discA: function () {
        var _itemprice2 = $('#itemprice').val() * $('#itemqty').val();
        var _discpercent = (($('#itemdiscamt').val() / _itemprice2) * 100).toFixed(2);
        $('#itemdisc').val(_discpercent);
    },
    dedP: function () {
        var _itemprice3 = $('#itemprice').val() * $('#itemqty').val();

        if ($('#dedtype').select2('val') == 1) {
            _itemprice3 = _itemprice3 - $('#itemdiscamt').val();
        }

        var _dedamount = (_itemprice3 * ($('#itemded').val() / 100)).toFixed(2);
        $('#itemdedamt').val(_dedamount);
    },
    dedA: function () {
        var _itemprice4 = $('#itemprice').val() * $('#itemqty').val();

        if ($('#dedtype').select2('val') == 1) {
            _itemprice4 = _itemprice4 - $('#itemdiscamt').val();
        }
        _dedpercent = (($('#itemdedamt').val() / _itemprice4) * 100).toFixed(2);
        $('#itemded').val(_dedpercent);

    },
    dedPO: function (e) {
        var _itemprice5 = $('#itemprice').val() * $('#itemqty').val();

        if (e == 1) {
            _itemprice5 = _itemprice5 - $('#itemdiscamt').val();
        }
        //alert(e);
        var _dedamount = (_itemprice5 * ($('#itemded').val() / 100)).toFixed(2);
        $('#itemdedamt').val(_dedamount);
    },
    clickOK: function () {
        gridIndex = $('#gridIndex').val();

        if (($('#itemprice').val() * $('#itemqty').val()) <= $('#itemdiscamt').val() ||
           ($('#itemprice').val() * $('#itemqty').val()) <= $('#itemdedamt').val()) {
            ardialog.Pop(4, "Invalid", "Discount and/or Deductables should not exceed the Price Amount", "OK", function () { });
        } else if ($.trim($('#itemcode').val()).length == 0) {
            ardialog.Pop(3, "Required", "Item Code is Required!", "OK", function () { });
        } else if ($.trim($('#itemname').val()).length == 0) {
            ardialog.Pop(3, "Required", "Item Name is Required!", "OK", function () { });
        } else {
            var iprice = ($('#itemprice').val() * $('#itemqty').val()).toFixed(2);
            var ipriceorig = ($('#itemprice').val() * 1).toFixed(2);
            var ibill = ($('#itemprice').val() * $('#itemqty').val()).toFixed(2);
            var idisc = $('#itemdiscamt').val() * 1;
            var ided = $('#itemdedamt').val() * 1;

            dtdisplaytable2.cell(gridIndex, 4).data($('#itemname').val()).draw();
            dtdisplaytable2.cell(gridIndex, 14).data($('#itemcode').val()).draw();

            dtdisplaytable2.cell(gridIndex, 5).data(ipriceorig).draw();
            dtdisplaytable2.cell(gridIndex, 6).data($('#itemqty').val()).draw();

            dtdisplaytable2.cell(gridIndex, 24).data($('#itemqty').val()).draw();
            dtdisplaytable2.cell(gridIndex, 25).data($('#itemunit').select2('val')).draw();

            dtdisplaytable2.cell(gridIndex, 7).data(ibill).draw();
            dtdisplaytable2.cell(gridIndex, 8).data(idisc.toFixed(2)).draw();
            dtdisplaytable2.cell(gridIndex, 9).data(ided.toFixed(2)).draw();


            var ifded = parseFloat($('#itemdiscamt').val()) + parseFloat($('#itemdedamt').val());

            var ibal = ((iprice - ifded) * 1).toFixed(2);

            dtdisplaytable2.cell(gridIndex, 10).data(ibal).draw();

            $('#myModal').modal('hide');

            editrec.precalculate();

        }
    },
    precalculate: function () {

        var _pregross = 0;
        var _predisc = 0;
        var _preded = 0;
        var _prenet = 0;
        $.each(dtdisplaytable2.rows().data(), function (i, d) {
            _pregross = _pregross + (dtdisplaytable2.cell(i, 7).data() * 1);
            _predisc = _predisc + (dtdisplaytable2.cell(i, 8).data() * 1);
            _preded = _preded + (dtdisplaytable2.cell(i, 9).data() * 1);
            _prenet = _prenet + (dtdisplaytable2.cell(i, 10).data() * 1);
        });

        $('#gross').val(_pregross.toFixed(2));
        $('#discount').val(_predisc.toFixed(2));
        $('#deductable').val(_preded.toFixed(2));
        $('#net').val(_prenet.toFixed(2));
    },
    recalculate: function () {
        ardialog.Confirm(2, "CAM", "Do you want to recalculate the Discount and Deductable for this Bill?",
                "Yes", "No",
                function () {
                    recalculate_bill();
                },
                function () {
                    $('#btn_modify').prop('disabled', false);
                }
            );
    },
    getarcancellationreason: function ($type) {
        $param = { id: $type, ctype: 62 };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_common_list", 'POST', $param,
            function () { },
            function (data) {
                //$('#sel2billno').select2('data', null);
                arcancellationreason = [];
                $.each(data.CL, function (i, com) {
                    arcancellationreason.push({ id: com.Id, text: com.Name });
                });

                _common.addineditor(dtdisplaytable2, arcancellationreason);

            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
    },
    getitemuom: function ($itemid) {
        ajaxwrapper.stdna($('#url').data('getitemuom'), 'POST', $param,
           function () { },
           function (data) {
               $('#itemunit').select2('data', null); //do not remove this will clear the select2 data
               $('#itemunit').select2('data', { id: data.Res.Id, text: data.Res.Name });
               $('#itemunit').prop('disabled', true);
           },
           function (err) {
               ardialog.Pop(4, "Error", err, "OK", function () { });
           });
    }
}

function get_opbservice_items($serid) {
    $param = { serid: $serid };
    ajaxwrapper.std($('#url').data('getopserviceitems'), 'POST', $param,
        function () { _indicator.Show('#itemdisptable'); },
        function (data) {
            dtdisplaytable.clear().draw();
            render_service_items(data.Res);
            _indicator.Stop('#itemdisptable');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop('#itemdisptable'); });
        }
    );
}

function render_service_items(result) {

    dtdisplaytable = $("#itemdisptable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 100,
        scrollCollapse: false,
        dom: 'zRlfrtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "ar_textalign_center", width: '5%' },
            { data: "Code", title: "Code", targets: 1, className: "IT_custom_content_align_left", width: '10%' },
            { data: "ItemName", title: "Name", targets: 2, className: "IT_custom_content_align_left", width: '85%' }
        ]
    });
    dtdisplaytable.on('order.dt search.dt', function () {
        dtdisplaytable.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();

    $('#itemdisptable tbody > tr > td').on('dblclick', function (e) {
        add_item_tobill(this, e);

        return false;
    });


}

function get_opbill_details($pin, $fdate, $tdate) {
    $param = { pin: $pin, fdate: $fdate, tdate: $tdate };
    ajaxwrapper.std($('#url').data('getopbcordetails'), 'POST', $param,
        function () { _indicator.Body(); },
        function (data) {
            if (data.rcode != 0) {
                ardialog.Pop(4, "Sorry", data.rmsg, "OK", function () { _indicator.Stop(); })
            } else {

                $('#ptname').val(data.Res[0].PTName);
                $('#sex').val(data.Res[0].Sex);
                $('#age').val(data.Res[0].Agetype);
                $('#authid').val(data.Res[0].AuthorityId);
                $('#transpin').val(data.Res[0].ARegistrationNo);

                $('#doctor').select2('data', { id: data.Res[0].DoctorId, text: data.Res[0].DoctorName });

                $('#category').select2('data', { id: data.Res[0].CategoryId, text: data.Res[0].Category });

                _common.getcommonlistnoclear('#company', data.Res[0].CategoryId, 1, 'Select Company...', null, function () {
                    $('#company').select2('data', { id: data.Res[0].CompanyId, text: data.Res[0].Company });
                });

                _common.getcommonlistnoclear('#grade', data.Res[0].CompanyId, 11, 'Select Grade...', null, function () {
                    $('#grade').select2('data', { id: data.Res[0].GradeId, text: data.Res[0].Grade });
                });

                $('#medid').val(data.Res[0].ActualMedId);

                dtdisplaytable2.clear().draw();
                render_bills(data.Res);

                _indicator.Stop();

                $("#btn_recalculate").prop('disabled', false);
                $("#btn_printbill").prop('disabled', false);
                
            }
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
        }
    );
}

function render_bills(result) {

    dtdisplaytable2 = $("#billdisptable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        //scrollY: 340,
        scrollX: '130%',
        scrollCollapse: true,
        dom: 'Rlfrtip',
        autoWidth: false,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "SLNO", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: '3%' },
            { data: "CB", title: "", defaultContent: '<div class="sgh-tristate sgh-tristate-default" data-tristate="0" onclick="billrowstate(this)"></div>', targets: 1, className: "IT_custom_content_align_center", width: '3%' },
            { data: "BillNo", title: "Bill No", targets: 2, className: "IT_custom_content_align_center", width: '13%' },
            { data: "BillDate", title: "Bill Date", targets: 3, className: "IT_custom_content_align_center", width: '10%' },
            { data: "ItemName", title: "Name", targets: 4, className: "IT_custom_content_align_left", width: '22%' },
            { data: "Price", title: "Price", targets: 5, className: "IT_custom_content_align_right", width: '4%' },
            { data: "Quantity", title: "Qty", targets: 6, className: "IT_custom_content_align_center", width: '3%' },
            { data: "EBillAmount", title: "E.Bill Amt", targets: 7, className: "IT_custom_content_align_right", width: '4%' },
            { data: "EDiscount", title: "E.Discount", targets: 8, className: "IT_custom_content_align_right", width: '4%' },
            { data: "EPaidAmount", title: "E.Ded-Amt", targets: 9, className: "IT_custom_content_align_right", width: '4%' },
            { data: "EBalance", title: "E.Balance", targets: 10, className: "IT_custom_content_align_right", width: '4%' },
            { data: "EReason", title: "Reason", defaultContent: '', targets: 11, className: "IT_custom_content_align_left", width: '30%' },
            { data: "CorStat", title: "", defaultContent: 0, targets: 12, visible: false },
            { data: "ReasonId", title: "", defaultContent: 0, targets: 13, visible: false },
            { data: "ItemCode", title: "", defaultContent: 0, targets: 14, visible: false },
            { data: "OpBillId", title: "", defaultContent: 0, targets: 15, visible: false },
            { data: "DepartmentId", title: "", defaultContent: 0, targets: 16, visible: false },
            { data: "ItemId", title: "", defaultContent: 0, targets: 17, visible: false },
            { data: "DiscountType", title: "", defaultContent: 0, targets: 18, visible: false },
            { data: "Deductable", title: "", defaultContent: 0, targets: 19, visible: false },
            { data: "DedDesc", title: "", defaultContent: "", targets: 20, visible: false },
            { data: "BatchId", title: "", defaultContent: "", targets: 21, visible: false },
            { data: "PolicyNo", title: "", defaultContent: "", targets: 22, visible: false },
            { data: "ServiceId", title: "", defaultContent: "", targets: 23, visible: false },
            { data: "IssueQty", title: "", defaultContent: "", targets: 24, visible: false },
            { data: "IssueUnit", title: "", defaultContent: "", targets: 25, visible: false }
        ],
        footerCallback: function (tfoot, data, start, end, display) {
            var api = this.api();
            if (data.length > 0) {
                $('#gross').val(
                    parseFloat(api.column(7)
                        .data()
                        .reduce(function (a, b) {
                            return a + b;
                        }, 0)).toFixed(2)
                );

                $('#discount').val(
                   parseFloat(api.column(8)
                       .data()
                       .reduce(function (a, b) {
                           return a + b;
                       }, 0)).toFixed(2)
                );

                $('#deductable').val(
                   parseFloat(api.column(9)
                       .data()
                       .reduce(function (a, b) {
                           return a + b;
                       }, 0)).toFixed(2)
                );

                $('#net').val(
                  parseFloat(api.column(10)
                      .data()
                      .reduce(function (a, b) {
                          return a + b;
                      }, 0)).toFixed(2)
               );
            }
        }
    });


    dtdisplaytable2.on('order.dt search.dt', function () {
        dtdisplaytable2.column(0, {
            search: 'applied',
            order: 'applied'
        })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();

    //$('#billdisptable').on('dblclick', 'td:not(:nth-of-type(2))', function (e) {
    //    show_bill_details(this);
    //});

    $('#billdisptable').on('dblclick', 'td:nth-of-type(5)', function (e) {
        show_bill_details(this);
    });

    $('#billdisptable').on('dblclick', 'td:nth-of-type(1)', function (e) {
        remove_item(this);
    });

    sel2billno = [];

    $.each(dtdisplaytable2.rows().data(), function (i, com) {
        _curid = dtdisplaytable2.cell(i, 15).data();
        if (_curid > 0) {
            if (!opbillExists(_curid)) {
                sel2billno.push({ "id": dtdisplaytable2.cell(i, 15).data(), "text": dtdisplaytable2.cell(i, 2).data(), "billdeptid": dtdisplaytable2.cell(i, 16).data() });
            }
        }
    });

    //inline editor
    _common.addineditor(dtdisplaytable2, arcancellationreason);
    _common.addineditor2(dtdisplaytable2, sel2billno);
    _common.initineditor(dtdisplaytable2);

}

function opbillExists(opbill) {
    return sel2billno.some(function (e) {
        return e.id === opbill;
    });
}

/*ON CLICK*/
function billrowstate(e) {
    $row = $(e).parents('tr');
    state = tristate.click(e);

    dtdisplaytable2.cell($row, 12).data(state);

    if (state != 1) {
        $row.find('td:nth(2)').removeClass('s2editor2');
        $row.find('td:nth(3)').removeClass('cusdatepick');
        $row.find('td:nth(11)').removeClass('s2editor');

        $row.find('td:nth(2)').unbind('click');
        $row.find('td:nth(3)').unbind('click');
        $row.find('td:nth(11)').unbind('click');

        _common.addineditor(dtdisplaytable2, arcancellationreason);
        _common.initineditor(dtdisplaytable2);

    } else {

        $row.find('td:nth(3)').addClass('cusdatepick');
        $row.find('td:nth(11)').addClass('s2editor');

        $row.find('td:nth(3)').bind('click');
        $row.find('td:nth(11)').bind('click');

        _common.addineditor(dtdisplaytable2, arcancellationreason);
        _common.initineditor(dtdisplaytable2);
    }

    // Reason is required for delete
    if (dtdisplaytable2.cell($row, 12).data() === 2) {
        $row.find('td:nth(11)').addClass('s2editor');
        $row.find('td:nth(11)').bind('click');
        _common.addineditor(dtdisplaytable2, arcancellationreason);
        _common.initineditor(dtdisplaytable2);
    }
}

function show_bill_details(e) {
    $row = $(e).parents('tr');
    rowIndex = dtdisplaytable2.cell(e).index().row;
    if (dtdisplaytable2.cell($row, 12).data() === 1 || dtdisplaytable2.cell($row, 12).data() === -2) {
        if (typeof $(e).closest("tr").find('td:eq(0)').html() != 'undefined') {
            var d = dtdisplaytable2.row($(e).parents('tr')).data();
            $('#gridIndex').val(rowIndex);

            $('#itemcode').val(dtdisplaytable2.cell($row, 14).data());

            $('#itemname').val(d["ItemName"]);
            $('#itemprice').val(parseFloat(d["Price"]).toFixed(2));

            $('#itemqty').val(d["Quantity"]);

            $('#itemdisc').val(((parseFloat(d["Discount"]).toFixed(2) / parseFloat(d["Price"] * d["Quantity"]).toFixed(2)) * 100).toFixed(2));
            $('#itemdiscamt').val(parseFloat(d["Discount"]).toFixed(2));

            if (d["Deductable"] == 1) {
                $('#itemded').val(((parseFloat(d["PaidAmount"]).toFixed(2) / ((parseFloat(d["Price"] * d["Quantity"]).toFixed(2)) - parseFloat(d["Discount"]).toFixed(2))) * 100).toFixed(2));
                $('#itemdedamt').val(parseFloat(d["PaidAmount"] - parseFloat(d["Discount"]).toFixed(2)).toFixed(2));
            } else {
                $('#itemded').val(((parseFloat(d["PaidAmount"]).toFixed(2) / parseFloat(d["Price"] * d["Quantity"]).toFixed(2)) * 100).toFixed(2));
                $('#itemdedamt').val(parseFloat(d["PaidAmount"]).toFixed(2));
            }
            if (dtdisplaytable2.cell($row, 12).data() === 1) {
                $('#dedtype').select2('data', { id: d["Deductable"], text: d["DeductableName"] });
            } else {
                $('#dedtype').select2('data', { id: dtdisplaytable2.cell($row, 19).data(), text: dtdisplaytable2.cell($row, 20).data() });
            }

            // Pharmacy Items should look into the UOM(unit of measurement)
            if (dtdisplaytable2.cell($row, 23).data() == _GLOBAL_PHARMACY_SERVICEID) {
                editrec.getitemuom(d["ItemId"]);
            } else {
                $('#itemunit').select2('data', null);
                $('#itemunit').prop('disabled', true);
            }

            $('#myModal').modal('show');
        }
    }
}

/* ADD / REMOVE ITEM */
function add_item_tobill(cell, e) {
    _SD = moment().get('year') + "-" + (moment().get('month') + 1) + "-" + moment().get('date');

    _DTODAY = moment(_SD).format('DD-MMM-YYYY');

    if (typeof $(cell).closest("tr").find('td:eq(0)').html() != 'undefined') {
        var d = dtdisplaytable.row($(cell).parents('tr')).data();

        $param = {
            serid: $('#service').select2('val'),
            itemid: d["Id"],
            catid: $('#category').select2('val'),
            comid: $('#company').select2('val'),
            graid: $('#grade').select2('val'),
            docid: $('#doctor').select2('val')
        };

        ajaxwrapper.std($('#url').data('getopitemprice'), 'POST', $param,
            function () { _indicator.Show('#billdisptable'); },
            function (data) {
                var _dtnewrow = dtdisplaytable2.row.add({
                    CB: '<div class="sgh-tristate"><span class="glyphicon glyphicon-ok"></span></div>',
                    BillNo: '',
                    BillDate: (_DTODAY).toString(),
                    ItemName: d["Name"],
                    Price: parseFloat(data.Res.Price).toFixed(2),
                    Quantity: '1',
                    EBillAmount: parseFloat(data.Res.Price).toFixed(2),
                    EDiscount: '0.00',
                    EPaidAmount: '0.00',
                    EBalance: parseFloat(data.Res.Price).toFixed(2),
                    EReason: '',
                    CorStat: -2,
                    ReasonId: 0,
                    ItemCode: d["Code"],
                    OpBillId: 0,
                    DepartmentId: data.Res.ItemDept,
                    ItemId: d["Id"],
                    DiscountType: data.Res.DiscType,
                    Deductable: data.Res.DedType,
                    DedDesc: data.Res.DedDesc,
                    BatchId: data.Res.BatchId,
                    ServiceId: $('#service').select2('val'),
                    PolicyNo: data.Res.PolicyNo
                }).draw().nodes().to$();

                _dtnewrow.find('td:nth(2)').addClass('s2editor2').bind('click')
                _dtnewrow.find('td:nth(3)').addClass('cusdatepick').bind('click')
                _dtnewrow.find('td:nth(4)').addClass('newItemDTColor')
                _dtnewrow.find('td:nth(11)').addClass('s2editor').bind('click');

                _common.addineditor(dtdisplaytable2, arcancellationreason);
                _common.addineditor2(dtdisplaytable2, sel2billno);
                _common.initineditor(dtdisplaytable2);

                editrec.precalculate();
                _indicator.Stop('#billdisptable');
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop('#billdisptable'); });
            }
        );
    }
}
function remove_item(cell) {
    $row = $(cell).parents('tr');
    rowIndex = dtdisplaytable2.cell(cell).index().row;

    /*only added item can be removed from the list (-2)*/
    if (dtdisplaytable2.cell(rowIndex, 12).data() == -2) {
        dtdisplaytable2.row($row).remove().draw();
    }
}

/* Reload */
function _clear() {
    ardialog.Confirm(2, "CAM", "This will refresh the page, be sure that you have saved what you are currently doing...<br><b>Continue</b>?", "Yes", "No",
        function () { location.reload(true); },
        function () { }
    );
}

/*modify*/
function validate_data() {
    var _hasRequired = 0; //NO
    var _canproceed = 0; //YES
    $.each(dtdisplaytable2.rows().data(), function (i, com) {
        _reaid = dtdisplaytable2.cell(i, 13).data();
        _itemprice = dtdisplaytable2.cell(i, 5).data();
        _itemname = dtdisplaytable2.cell(i, 4).data();
        _corstat = dtdisplaytable2.cell(i, 12).data();

        if (_corstat != 0) {
            if (_reaid === 0) {
                ardialog.Pop(3, "Required", "Bill Correction Reason is Required for <b>" + _itemname + "</b>.", "OK",
                    function () { });
                _hasRequired = 1;
                return false;
            } else if (_itemprice <= 0.00 || _itemprice <= 0) {
                ardialog.Pop(3, "Required", "Check the Price for <b>" + _itemname + "</b>.", "OK",
                   function () { });
                _hasRequired = 1;
                return false;
            }
            _canproceed = 1;
        }
    });

    if (_hasRequired != 1 && _canproceed == 1) {
        save_opbill_correction();
    }
}
/* Recalculate */
function recalculate_bill() {
    _indicator.Body();
    $.each(dtdisplaytable2.rows().data(), function (i, com) {
        _itemprice = dtdisplaytable2.cell(i, 7).data();
        _itemid = dtdisplaytable2.cell(i, 17).data();
        _itemdept = dtdisplaytable2.cell(i, 16).data();
        _serid = dtdisplaytable2.cell(i, 23).data();
        _corstat = dtdisplaytable2.cell(i, 12).data();
        //works only for new and modify tagged
        if (_corstat == -2 || _corstat == 1) {
            $param = {
                catid: $('#category').select2('val'),
                comid: $('#company').select2('val'),
                graid: $('#grade').select2('val'),
                serid: _serid,
                itemid: _itemid,
                depid: _itemdept,
                price: _itemprice
            }
           // console.log($param);

            ajaxwrapper.stdna($('#url').data('recalculate'), 'POST', $param,
              function () { },
              function (data) {
                  //_billamt = (dtdisplaytable2.cell(i, 7).data() * 1).toFixed(2);
                  _billdisc = (data.Res.Discount * 1).toFixed(2);
                  _billded = (data.Res.Deductable * 1).toFixed(2);


                  _balance = parseFloat(dtdisplaytable2.cell(i, 7).data() * 1);

                  _fbalance = _balance - (parseFloat(_billdisc) + parseFloat(_billded) );

                  //dtdisplaytable2.cell(i, 7).data(_billamt).draw();
                  dtdisplaytable2.cell(i, 8).data(_billdisc);
                  dtdisplaytable2.cell(i, 9).data(_billded);
                  dtdisplaytable2.cell(i, 10).data(_fbalance.toFixed(2));
                  dtdisplaytable2.cell(i, 18).data(data.Res.DedType);
                  dtdisplaytable2.cell(i, 19).data(data.Res.DedDesc);
                  
                  
                  editrec.precalculate();
              },
              function (err) {
                  ardialog.Pop(4, "Error", err, "OK", function () { });
              });
        }
    });
    _indicator.Stop();

 
    $('#btn_modify').prop('disabled', false);
    
}

function save_opbill_correction() {
    $billpar = [];
    $.each(dtdisplaytable2.rows().data(), function (i, com) {

        var dd = dtdisplaytable2.row(i).data();

        if (dtdisplaytable2.cell(i, 12).data() != 0) {
            $billpar.push({
                RegistrationNo: dd["RegistrationNo"] ? dd["RegistrationNo"] : $('#pin').val(),
                OPBillId: dd["OPBillId"] ? dd["OPBillId"] : (dtdisplaytable2.cell(i, 15).data()),
                BillNo: dd["BillNo"] ? dd["BillNo"] : dtdisplaytable2.cell(i, 2).data(),
                CompanyId: dd["CompanyId"] ? dd["CompanyId"] : $('#company').select2('val'),
                CategoryId: dd["CategoryId"] ? dd["CategoryId"] : $('#category').select2('val'),
                GradeId: dd["GradeId"] ? dd["GradeId"] : $('#grade').select2('val'),
                DoctorId: dd["DoctorId"] ? dd["DoctorId"] : $('#doctor').select2('val'),
                ServiceId: dd["ServiceId"] ? dd["ServiceId"] : (dtdisplaytable2.cell(i, 23).data()),
                ItemId: dd["ItemId"] ? dd["ItemId"] : dtdisplaytable2.cell(i, 17).data(),
                DiscountType: dtdisplaytable2.cell(i, 18).data(),
                Deductable: dtdisplaytable2.cell(i, 19).data(),

                BillAmount: dd["BillAmount"] ? dd["BillAmount"] : ((dtdisplaytable2.cell(i, 5).data() * dtdisplaytable2.cell(i, 6).data()) * 1).toFixed(2),
                PaidAmount: dd["PaidAmount"] ? dd["PaidAmount"] : ((dtdisplaytable2.cell(i, 9).data() * 1).toFixed(2)),
                Discount: dd["Discount"] ? dd["Discount"] : ((dtdisplaytable2.cell(i, 8).data() * 1).toFixed(2)),

                Balance: (dtdisplaytable2.cell(i, 12).data() == -2) ? (dtdisplaytable2.cell(i, 10).data() * 1).toFixed(2) : dd["Balance"],

                Billdatetime: dtdisplaytable2.cell(i, 3).data(),
                DepartmentId: dd["Balance"] ? dd["Balance"] : dtdisplaytable2.cell(i, 16).data(),
                Quantity: dtdisplaytable2.cell(i, 6).data(),
                OperatorId: dd["OperatorId"] ? dd["OperatorId"] : 0,
                BillTypeId: 2,
                StationId: dd["StationId"] ? dd["StationId"] : 0,
                AuthorityId: $('#authid').val(),
                Posted: dd["Posted"] ? dd["Posted"] : 1,
                ARegistrationNo: $('#transpin').val(),
                AIssueAuthorityCode: dd["AIssueAuthorityCode"] ? dd["AIssueAuthorityCode"] : $('#opbbcode').html(),
                ACategoryId: dd["ACategoryId"] ? dd["ACategoryId"] : $('#category').select2('val'),
                ACompanyId: dd["ACompanyId"] ? dd["ACompanyId"] : $('#company').select2('val'),
                AGradeId: dd["AGradeId"] ? dd["AGradeId"] : $('#grade').select2('val'),
                ADoctorId: dd["ADoctorId"] ? dd["ADoctorId"] : $('#doctor').select2('val'),
                ABillAmount: (dtdisplaytable2.cell(i, 12).data() == -2) ? 0 : (dtdisplaytable2.cell(i, 7).data() * 1).toFixed(2),
                APaidAmount: (dtdisplaytable2.cell(i, 12).data() == -2) ? 0 : (dtdisplaytable2.cell(i, 9).data() * 1).toFixed(2),
                ADiscount: (dtdisplaytable2.cell(i, 12).data() == -2) ?  0 : (dtdisplaytable2.cell(i, 8).data() * 1).toFixed(2),
                ABalance: (dtdisplaytable2.cell(i, 12).data() == -2) ?  0 : (dtdisplaytable2.cell(i, 10).data() * 1).toFixed(2),
                AQuantity: (dtdisplaytable2.cell(i, 12).data() == -2) ? 0 : dtdisplaytable2.cell(i, 6).data(),
                AAuthorityId: (dtdisplaytable2.cell(i, 12).data() == -2) ? 0 : $('#authid').val(),
                ModifiedOperatorId: dd["ModifiedOperatorId"] ? dd["ModifiedOperatorId"] : 0,
                ModifiedDateTime: dd["ModifiedDateTime"] ? dd["ModifiedDateTime"] : '',
                ABillDateTime: dd["ABillDateTime"] ? dd["ABillDateTime"] : '',
                BatchId: dd["BatchId"] ? dd["BatchId"] : dtdisplaytable2.cell(i, 21).data(),
                ItemCode: dtdisplaytable2.cell(i, 14).data(),
                ItemName: dtdisplaytable2.cell(i, 4).data(),
                PolicyNo: dtdisplaytable2.cell(i, 22).data(),
                MedIdNumber: $('#medid').val(),
                CorType: dtdisplaytable2.cell(i, 12).data(),
                ReasonId: dtdisplaytable2.cell(i, 13).data(),
                ModifyType: _tmp_modifytype,
                IssueQty: dtdisplaytable2.cell(i, 24).data(),
                IssueUnit: dtdisplaytable2.cell(i, 25).data()
            });
        }
    });

    $param = { listofbill: $billpar }

    ardialog.Confirm(2, "Confirm", "You are about to Add/Modify Patient Bill, Continue?", "Yes", "No",
        function () {
            ajaxwrapper.postsave($('#url').data('saveopbillcorrection'), $param, function (data) {
                DFrom = $("#fdate").datepicker('getDate');
                DTill = $("#tdate").datepicker('getDate');
                valfrom = moment(DFrom).format('DD-MMM-YYYY');
                valtill = moment(DTill).format('DD-MMM-YYYY');
                get_opbill_details($('#pin').val(), valfrom, valtill);
            });
        }, function () { }
    );
}

/* Printing Bills */
function render_billstoprint(results) {
    dtbillstoprint = $("#billstoprint").DataTable({
        destroy: true,
        data: results,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 300,
        scrollCollapse: false,
        dom: 'zRlfrtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 300,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "ar_textalign_center", width: '5%' },
            { data: "cbox", title: "", defaultContent: '<div class="sgh-tristate sgh-tristate-default" data-dstate="0" onclick="billsprint(this)"></div>', targets: 1, className: "IT_custom_content_align_left", width: '10%' },
            { data: "BillNo", title: "BillNo", targets: 2, className: "IT_custom_content_align_left", width: '85%' },
            { data: "OpbillId", title: "Name", targets: 3, visible: false, className: "IT_custom_content_align_left" },
            { data: "IsChecked", title: "Name", defaultContent: 0, targets: 4, visible: true, className: "IT_custom_content_align_left" }
        ]
    });
    dtbillstoprint.on('order.dt search.dt', function () {
        dtbillstoprint.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();
}

function generate_printblist() {
    btoprint = [];
    dtbillstoprint.clear().draw();
    $.each(dtdisplaytable2.rows().data(), function (i, com) {
        _curid = dtdisplaytable2.cell(i, 15).data();
        if (_curid > 0) {
            if (!opbillExists2(_curid)) {
                btoprint.push({
                    "id": dtdisplaytable2.cell(i, 15).data(),
                    "text": dtdisplaytable2.cell(i, 2).data(),
                    "billdeptid": dtdisplaytable2.cell(i, 16).data()
                });

                dtbillstoprint.row.add({
                    BillNo: dtdisplaytable2.cell(i, 2).data(),
                    OpbillId: dtdisplaytable2.cell(i, 15).data()
                });
            }
        }
    });

    $('#billprintModal').modal('show');
}

function opbillExists2(opbill) {
    return btoprint.some(function (e) {
        return e.id === opbill;
    });
}

function print_preview() {

    $('#myReportModal').modal('show');

    $opbprintparam = [];

    $.each(dtbillstoprint.rows().data(), function (i, com) {
        if (dtbillstoprint.cell(i, 4).data() == 1) {
            $opbprintparam.push({ opbill: dtbillstoprint.cell(i, 3).data() });
        }
    });

    var repurl = $('#url').data('reporturl');

    var repsrc = repurl + "?rtype=7" + "&xmldata=" + JSON.stringify($opbprintparam) + "&opeid=" + $('#his_opeid').val() + "&staid=" + $('#his_staid').val() + "&rdisp=P";

    document.getElementById("myReport2").src = repsrc;
}

function billsprint(e) {
    $row = $(e).parents('tr');
    state = duocheck.click(e);
    dtbillstoprint.cell($row, 4).data(state);
}

$(document).ready(function () {
    _common.makedate("#fdate, #tdate", "dd-M-yyyy");
    $('#fdate').datepicker()
     .on('changeDate', function (e) {
         _DFrom = $("#fdate").datepicker('getDate');
         _DTo = moment(_DFrom).add(1, 'M').subtract(1, 'days').format('DD-MMM-YYYY');
         $("#tdate").datepicker('update', _DTo);
     });

    _common.bindselect2noclear('#company', null, '');
    _common.bindselect2noclear('#grade', null, '');

    $('#company').prop('disabled', true);
    $('#grade').prop('disabled', true);


    dedtype = [
       { id: 0, text: 'Deductable Before Discount' },
       { id: 1, text: 'Deductable After Discount' }
    ];

    _common.bindselect2noclear('#dedtype', dedtype, 'Select Deductable Type...');

    _common.bindselect2noclear('#itemunit', null, 'Select Unit...');

    $('#dedtype').select2('data', { id: 0, text: 'Deductable Before Discount' });

    editrec.getarcancellationreason(2);
    $(document).on('click', '#reafin', function () {
        if ($('#reafin').is(':checked')) {
            _tmp_modifytype = 2;
            editrec.getarcancellationreason(2);
        }
    });

    $(document).on('click', '#reatech', function () {
        if ($('#reatech').is(':checked')) {
            _tmp_modifytype = 9;
            editrec.getarcancellationreason(9);
        }
    });

    _common.makepinonly("#pin");
    $("#pin").on("keydown", function (event) {
        if (event.which == 13) {
            _common.pinenter('#pin', function () {
                DFrom = $("#fdate").datepicker('getDate');
                DTill = $("#tdate").datepicker('getDate');
                valfrom = moment(DFrom).format('DD-MMM-YYYY');
                valtill = moment(DTill).format('DD-MMM-YYYY');
                get_opbill_details($('#pin').val(), valfrom, valtill);
            });
            $(this).prop('disabled', true);
        }
    });

    _common.getcommonlistnoclear('#service', 0, 60, 'Select Service...', null);
    $('#service').on('select2-selecting', function (e) {
        get_opbservice_items(e.val);
    });

    _common.getcommonlistnoclear('#doctor', 0, 53, 'Select Doctor...', null);
    _common.getcommonlistnoclear('#category', 0, 2, 'Select Category...', null);

    $('#category').on('select2-selecting', function (e) {
        _common.getcommonlistnoclear('#company', e.val, 1, 'Select Company...', null);
    });

    $('#company').on('select2-selecting', function (e) {
        _common.getcommonlistnoclear('#grade', e.val, 11, 'Select Grade...', null);
    });

    render_service_items(null);
    render_bills(null);

    $('#fdate').datepicker()
        .on('changeDate', function (e) {
            $('#pin').focus();
        });

    /*Edit Item*/

    _common.makedecimalplus('#itemprice, #itemdisc, #itemdiscamt, #itemded, #itemdedamt');
    _common.makeintegerplus('#itemqty');

    $('#dedtype').on('select2-selecting', function (e) {
        editrec.dedPO(e.val);
    });

    $(document).on('click', '#btn_editok', function () {
        editrec.clickOK();
    });

    $(document).on('click', '#btn_recalculate', function () {
        editrec.recalculate();
    });

    $(document).on('click', '#btn_clear, #btn_clear2', function () {
        _clear();
    });

    $(document).on('click', '#btn_modify', function () {
        validate_data();
    });

    $(document).on('click', '#btn_printbill', function () {
        generate_printblist();
    });

    /*Printing*/
    render_billstoprint(null);

    $("#billprintModal").on('shown.bs.modal', function (event) {
        setTimeout(function () {
            dtbillstoprint.columns.adjust().draw();
        }, 1 * 150);
    });

    $(document).on('click', '#btn_printpreview', function () {
        print_preview();
    });

    $('#myReportModal').on('shown.bs.modal', function () {
        _indicator.Show('#report-container');
    });

    $('#myReport2').on("load", function () {
        _indicator.Stop("#report-container");
    });

});