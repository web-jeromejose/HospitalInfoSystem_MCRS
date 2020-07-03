var dtdisplaytable2;
var dtDispServiceItems;
var dtDispServiceSelItems;
var dtserlist;

/*
    Reason Common for all
*/
var $IS_REASON_COMMON = 0;
var $REASON_ID = 0;
var $RESON_DESC = '';
var $SEL_ROW;
var $CBX_STATE;
var $CUR_CELL;


/* ----------------------------------------------------------------------------------------------------------------------------
    [ Default Select2 Value ]
    - default select2 value for units
    - this is to enable select2 in datatable for all service items. only pharmacy has multiple values
    - default value will be nos with 1 id.
-------------------------------------------------------------------------------------------------------------------------------*/
var $DEF_UNIT; //default
var $PH_UNIT; // pharmacy UOM
/* ----------------------------------------------------------------------------------------------------------------------------
    [ Bill Information ]
-------------------------------------------------------------------------------------------------------------------------------*/
function get_Bill_Items($billno, $serid) {
    $param = { billno: $billno, serid: $serid };
    ajaxwrapper.std($('#url').data('getipbillitems'), 'POST', $param,
        function () {
            _indicator.Show('#billdisptable');
        },
        function (data) {
            render_bills(data.Res);
            //alert(JSON.stringify(data.Res));
            $('#btn_add_item').prop('disabled', false);
            $('#btn_delete_item').prop('disabled', false);
            _indicator.Stop('#billdisptable');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
                _indicator.Stop('#billdisptable');
            });
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
        scrollY: 335,
        scrollX: '130%',
        scrollCollapse: true,
        dom: 'Rlfrtip',
        autoWidth: false,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "SLNO", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: '3%' },
            { data: "CB", title: "", defaultContent: '<div class="sgh-tristate sgh-tristate-default" data-dstate="0" onclick="billrowstate(this)"></div>', targets: 1, className: "IT_custom_content_align_center", width: '3%' },
            { data: "SerialNo", title: "Serial No", targets: 2, className: "IT_custom_content_align_center", width: '8%' },
            { data: "ItemName", title: "Name", targets: 3, className: "IT_custom_content_align_left", width: '27%' },
            { data: "Datetime", title: "Date", targets: 4, className: "IT_custom_content_align_center", width: '7%' },
            { data: "Quantity", title: "Qty.", targets: 5, className: "IT_custom_content_align_center", width: '4%' },
            { data: "Price", title: "Amount", targets: 6, className: "IT_custom_content_align_right", width: '4%' },
            { data: "EditQuantity", title: "New Qty.", targets: 7, className: "IT_custom_content_align_center", width: '4%' },
            { data: "EditPrice", title: "New Amt.", targets: 8, className: "IT_custom_content_align_right", width: '4%' },
            { data: "DeductableAmount", title: "Deductable", targets: 9, className: "IT_custom_content_align_right", width: '4%' },
            { data: "Discount", title: "Discount", targets: 10, className: "IT_custom_content_align_right", width: '4%' },
            { data: "EReason", title: "Reason", defaultContent: '', targets: 11, className: "IT_custom_content_align_left", width: '33%' },
            { data: "CorStat", title: "", defaultContent: 0, targets: 12, visible: false },
            { data: "ReasonId", title: "", defaultContent: 0, targets: 13, visible: false },
            { data: "Code", title: "", defaultContent: 0, targets: 14, visible: false },
            { data: "BillNo", title: "", defaultContent: 0, targets: 15, visible: false },
            { data: "DepartmentId", title: "", defaultContent: 0, targets: 16, visible: false },
            { data: "ItemId", title: "", defaultContent: 0, targets: 17, visible: false },
            { data: "DeductableType", title: "", defaultContent: 0, targets: 18, visible: false },
            { data: "BedTypeId", title: "", defaultContent: 0, targets: 19, visible: false }
        ]
    });

    dtdisplaytable2.on('order.dt search.dt', function () {
        dtdisplaytable2.column(0, {
            search: 'applied',
            order: 'applied'
        })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();

}
function billrowstate(e) {

    $row = $(e).parents('tr');
    $CUR_CELL = e;

    if (dtdisplaytable2.cell($row, 12).data() == 0) {
        if ($IS_REASON_COMMON == 1) {
            state = duocheck.click(e);
            dtdisplaytable2.cell($row, 12).data(state);
            dtdisplaytable2.cell($row, 13).data($REASON_ID).draw();
            dtdisplaytable2.cell($row, 11).data($RESON_DESC).draw();
        } else {
            $('#corReasons').modal('show');
        }

    } else {
        state = duocheck.click(e);
        dtdisplaytable2.cell($row, 12).data(state);

        if (state == 0) {
            dtdisplaytable2.cell($row, 13).data('').draw();
            dtdisplaytable2.cell($row, 11).data('').draw();
        }
    }
}
function _clear() {
    ardialog.Confirm(2, "AR IP Billing", "This will refresh the page, be sure that you have saved what you are currently doing... <b>Continue</b>?", "Yes", "No",
        function () { location.reload(true); },
        function () { }
    );
}
function get_AdmissionDate($pin) {
    $param = { pin: $pin };
    ajaxwrapper.std($('#url').data('getadmissiondate'), 'POST', $param,
        function () {
            //_indicator.Show('#itemdisptable');
        },
        function (data) {
            if (data.rcode > 0) {
                ardialog.Pop(4, "Sorry", data.rmsg, "OK", function () {
                    _indicator.Stop();
                });
            } else {

                comlist = [];
                $.each(data.Res, function (i, com) {
                    comlist.push({ id: com.BillNo, text: com.AdmissionDate });
                });
                _common.bindselect2noclear('#admitdate', comlist, 'Select Admission Date...');
                setTimeout(
                    function () {
                        $('#admidate').select2('focus');
                    }, 1 * 150);
            }
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
                //_indicator.Stop('#itemdisptable');
            });
        }
    );
}
function get_PT_Information($billno) {

    $param = { billno: $billno };
    ajaxwrapper.std($('#url').data('getptbillinfo'), 'POST', $param,
        function () {
            _indicator.Body();
        },
        function (data) {

            if (data.rcode > 0) {
                ardialog.Pop(4, "Sorry", data.rmsg, "OK", function () {
                    _indicator.Stop();
                });
            } else {

                // alert(JSON.stringify(data.Res));

                $('#ptname').val(data.Res.PTName);

                $('#bedid').val(data.Res.BedTypeId);
                $('#tariffid').val(data.Res.TariffId);
                $('#dedtype').val(data.Res.DeductableType);

                $('#sex').val(data.Res.Sex);
                $('#age').val(data.Res.Age);
                $('#billdate').val(data.Res.BillDate);

                $('#category').val(data.Res.Category);
                $('#categoryid').val(data.Res.CategoryId);

                $('#company').val(data.Res.Company);
                $('#companyid').val(data.Res.CompanyId);

                $('#grade').val(data.Res.GradeName);
                $('#gradeid').val(data.Res.GradeId);

                $('#billno').val(data.Res.BillType + ' ' + data.Res.SlNo);

                $('#actualbillamt').val((data.Res.BillAmount).formatToMoney());

                if (data.Res.IsInvoiced == 1) {
                    $('#invoiced').prop('checked', true);
                } else {
                    $('#invoiced').prop('checked', false);
                }
                get_IPBill_PosNeg_Adj($billno);
                get_IPBill_Services($billno);
                _indicator.Stop();
            }
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
                _indicator.Stop();
            });
        }
    );
}
function get_IPBill_PosNeg_Adj($billno) {
    $param = { billno: $billno };
    ajaxwrapper.std($('#url').data('getposnegadj'), 'POST', $param,
        function () {
        },
        function (data) {

            $('#posadj').val(CommaFormatted((data.Res.PositiveAdj * 1).toFixed(2)));
            $('#negadj').val(CommaFormatted((data.Res.NegativeAdj * 1).toFixed(2)));
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
            });
        }
    );
}
function get_IPBill_Services($billno) {
    $param = { billno: $billno };
    ajaxwrapper.std($('#url').data('getbillservices'), 'POST', $param,
        function () {
        },
        function (data) {
            dtserlist = "";
            dtserlist = [];
            $.each(data.Res, function (i, com) {
                dtserlist.push({ id: com.Id, text: com.Name });
            });
            _common.bindselect2noclear('#services', dtserlist, 'Select Service...');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
            });
        }
    );
}
function get_IPBill_Services_All($catid, $comid, $graid) {
    $param = { catid: $catid, comid: $comid, graid: $graid };
    ajaxwrapper.std($('#url').data('getbillservicesall'), 'POST', $param,
        function () {
        },
        function (data) {
            dtserlist = "";
            dtserlist = [];
            $.each(data.Res, function (i, com) {
                dtserlist.push({ id: com.Id, text: com.Name });
            });
            _common.bindselect2noclear('#services', dtserlist, 'Select Service...');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
            });
        }
    );
}
/* ----------------------------------------------------------------------------------------------------------------------------
    [ Add Items ]
-------------------------------------------------------------------------------------------------------------------------------*/
function get_IPBillService_Items($serid, $comid, $graid) {

    $param = { serid: $serid, comid: $comid, graid: $graid };
    ajaxwrapper.std($('#url').data('getserviceitem'), 'POST', $param,
        function () {
            _indicator.Show('#itemdisptable');
        },
        function (data) {
            render_IPBillService_Items(data.Res);
            _indicator.Stop('#itemdisptable');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
                _indicator.Stop('#itemdisptable');
            });
        }
    );
}
function render_IPBillService_Items(result) {
    dtDispServiceItems = $("#itemdisptable").DataTable({
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
            { data: "OPCB1", title: "", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "ar_textalign_center", width: '5%' },
            { data: "Code", title: "Code", defaultContent: '', targets: 1, className: "IT_custom_content_align_left", width: '10%' },
            { data: "Name", title: "Name", defaultContent: '', targets: 2, className: "IT_custom_content_align_left", width: '85%' }
        ]
    });
    dtDispServiceItems.on('order.dt search.dt', function () {
        dtDispServiceItems.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();


    // Moved to document ready to facilitate pagination.    

    //$('#itemdisptable tbody > tr > td').on('dblclick', 'tbody > tr > td', function (e) {
    //    e.stopPropagation();
    //    add_Service_Item(this, e);
    //});

}
function render_IPBillService_SelItems(result) {
    dtDispServiceSelItems = $("#selitemdisptable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: true,
        searching: true,
        info: true,
        processing: false,
        scrollY: 350,
        scrollX: '100%',
        scrollCollapse: false,
        dom: 'zRlfrtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
            { data: "OPCB1", title: "SLNo", defaultContent: '', searchable: false, orderable: false, targets: 0, className: "IT_custom_content_align_center", width: '3%' },
            { data: "ItemName", title: "Item Name", defaultContent: '', targets: 1, className: "IT_custom_content_align_left", width: '34%' },
            { data: "DateTime", title: "Date", defaultContent: '', targets: 2, className: "IT_custom_content_align_center", width: '10%' },
            { data: "Qty", title: "Qty", defaultContent: '', targets: 3, className: "IT_custom_content_align_center", width: '3%' },
            { data: "Unit", title: "Unit", defaultContent: '', targets: 4, className: "IT_custom_content_align_center", width: '10%' },
            { data: "Amount", title: "Amount", defaultContent: 0, targets: 5, className: "IT_custom_content_align_right", width: '10%' },
            { data: "Deductable", title: "Ded", defaultContent: '', targets: 6, className: "IT_custom_content_align_right", width: '10%' },
            { data: "Discount", title: "Disc", defaultContent: '', targets: 7, className: "IT_custom_content_align_right", width: '10%' },
            { data: "NetAmount", title: "Net", defaultContent: '', targets: 8, className: "IT_custom_content_align_right", width: '10%' },
            { data: "ItemId", title: "Item Id", visible: false, defaultContent: '', targets: 9, className: "IT_custom_content_align_left" },
            { data: "DepId", title: "DepId", visible: false, defaultContent: '', targets: 10, className: "IT_custom_content_align_left" },

            // Details

            { data: "Code", title: "Code", visible: false, defaultContent: '', targets: 11, className: "IT_custom_content_align_left" },
            { data: "Name", title: "Name", visible: false, defaultContent: '', targets: 12, className: "IT_custom_content_align_left" },
            { data: "MUPer", title: "MUPer", visible: false, defaultContent: '', targets: 13, className: "IT_custom_content_align_left" },
            { data: "MUAmt", title: "MUAmt", visible: false, defaultContent: '', targets: 14, className: "IT_custom_content_align_left" },
            { data: "DIPer", title: "DIPer", visible: false, defaultContent: '', targets: 15, className: "IT_custom_content_align_left" },
            { data: "DIAmt", title: "DIAmt", visible: false, defaultContent: '', targets: 16, className: "IT_custom_content_align_left" },
            { data: "DEPer", title: "DEPer", visible: false, defaultContent: '', targets: 17, className: "IT_custom_content_align_left" },
            { data: "DEAmt", title: "DEAmt", visible: false, defaultContent: '', targets: 18, className: "IT_custom_content_align_left" },
            { data: "DEType", title: "DEType", visible: false, defaultContent: '', targets: 19, className: "IT_custom_content_align_left" },
            { data: "ConQty", title: "ConQty", visible: false, defaultContent: '', targets: 20, className: "IT_custom_content_align_left" },
            { data: "PackId", title: "PackId", visible: false, defaultContent: 0, targets: 21, className: "IT_custom_content_align_left" },
            { data: "DILevel", title: "DILevel", visible: false, defaultContent: 0, targets: 22, className: "IT_custom_content_align_left" },
            { data: "DELevel", title: "DELevel", visible: false, defaultContent: 0, targets: 23, className: "IT_custom_content_align_left" },
            { data: "MarkUp", title: "MarkUp", visible: false, defaultContent: 0, targets: 24, className: "IT_custom_content_align_left" },
        ]
    });
    dtDispServiceSelItems.on('order.dt search.dt', function () {
        dtDispServiceSelItems.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();
}
function add_Service_Item(cell, e) {

    _unit = 'Nos';

    _SD = moment().get('year') + "-" + (moment().get('month') + 1) + "-" + moment().get('date');
    _DTODAY = moment(_SD).format('DD-MMM-YYYY');

    if (typeof $(cell).closest("tr").find('td:eq(0)').html() != 'undefined') {
        var d = dtDispServiceItems.row($(cell).parents('tr')).data();

        if ($('#services').select2('val') == 5 || $('#services').select2('val') == 37) {

            $param = {
                itemid: d["Id"]
            };

            ajaxwrapper.std($('#url').data('getphitemuom'), 'POST', $param,
                function () {
                    //_indicator.Show('#selitemdisptable');
                },
                function (data) {
                    comlist = [];
                    $.each(data.Res, function (i, com) {
                        comlist.push({ id: com.Id, text: com.Name });
                    })

                    var _dtnewrow = dtDispServiceSelItems.row.add({
                        ItemName: d['ItemName'],
                        DateTime: _DTODAY,
                        Qty: 1,
                        Unit: '',
                        Amount: 0,
                        Deductable: 0,
                        Discount: 0,
                        NetAmount: 0,
                        ItemId: d['Id'],
                        DepId: 0,

                        Code: d['Code'],
                        Name: d['Name'],
                        MUPer: 0,
                        MUAmt: 0,
                        DIPer: 0,
                        DIAmt: 0,
                        DEPer: 0,
                        DEAmt: 0,
                        DEType: 0,
                        ConQty: 0,
                        PackId: 0
                    }).draw().nodes().to$();

                    _dtnewrow.find('td:nth(2)').addClass('cusdatepick').bind('click'); //for date
                    _dtnewrow.find('td:nth(3)').addClass('inteditor'); /// for quantity
                    _dtnewrow.find('td:nth(5)').addClass('deceditor'); /// for amount
                    _dtnewrow.find('td:nth(4)').addClass('s2editor2').bind('click'); // for unit

                    //_dtnewrow.find('td:nth(2)').addClass('s2editor2').bind('click')


                    //_dtnewrow.find('td:nth(11)').addClass('s2editor').bind('click');

                    _common.addineditor(dtDispServiceSelItems, null);

                    _common.addineditor2(dtDispServiceSelItems, comlist);
                    //_common.addineditor2(dtDispServiceSelItems, $PH_UNIT);
                    _common.initineditor(dtDispServiceSelItems);


                },
                function (err) {
                    ardialog.Pop(4, "Error", err, "OK", function () {
                        // _indicator.Stop('#selitemdisptable');
                    });
                }
            );




        } else {

            $param = {
                serid: $('#services').select2('val'),
                itemid: d["Id"],
                bedid: $('#bedid').val(),
                dedtype: $('#dedtype').val(),
                packid: 0,
                tariffid: $('#tariffid').val(),
                catid: $('#categoryid').val(),
                comid: $('#companyid').val(),
                graid: $('#gradeid').val()
            };

            //alert(JSON.stringify($param));

            ajaxwrapper.std($('#url').data('getipitemprice'), 'POST', $param,
                function () { _indicator.Show('#selitemdisptable'); },
                function (data) {
                    var _dtnewrow = dtDispServiceSelItems.row.add({
                        ItemName: d['ItemName'],
                        DateTime: _DTODAY,
                        Qty: data.Res.Qty,
                        Unit: _unit,
                        Amount: data.Res.Price,
                        Deductable: data.Res.Deductable,
                        Discount: data.Res.Discount,
                        NetAmount: data.Res.NetAmount,
                        ItemId: d['Id'],
                        DepId: data.Res.DepId,

                        Code: d['Code'],
                        Name: d['Name'],
                        MUPer: data.Res.MUPer,
                        MUAmt: data.Res.MUAmt,
                        DIPer: data.Res.DIPer,
                        DIAmt: data.Res.DIAmt,
                        DEPer: data.Res.DEPer,
                        DEAmt: data.Res.DEAmt,
                        DEType: data.Res.DEType,
                        ConQty: data.Res.ConQty,
                        PackId: 1
                    }).draw().nodes().to$();

                    _dtnewrow.find('td:nth(2)').addClass('cusdatepick').bind('click'); //for date
                    _dtnewrow.find('td:nth(3)').addClass('inteditor'); /// for quantity
                    _dtnewrow.find('td:nth(5)').addClass('deceditor'); /// for amount
                    _dtnewrow.find('td:nth(4)').addClass('s2editor2').bind('click'); // for unit

                    //_dtnewrow.find('td:nth(2)').addClass('s2editor2').bind('click')


                    //_dtnewrow.find('td:nth(11)').addClass('s2editor').bind('click');

                    _common.addineditor(dtDispServiceSelItems, null);

                    _common.addineditor2(dtDispServiceSelItems, $DEF_UNIT);
                    //_common.addineditor2(dtDispServiceSelItems, $PH_UNIT);
                    _common.initineditor(dtDispServiceSelItems);

                    //editrec.precalculate();
                    _indicator.Stop('#selitemdisptable');
                },
                function (err) {
                    ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop('#selitemdisptable'); });
                }
            );
        }
    }
    //e.stopPropagation();
    //e.preventDefault();
}
function add_Service_Item_PH($pid, $index) {
    $param = {
        serid: $('#services').select2('val'),
        itemid: dtDispServiceSelItems.cell($index, 9).data(),
        bedid: $('#bedid').val(),
        dedtype: $('#dedtype').val(),
        packid: $pid,
        tariffid: $('#tariffid').val(),
        catid: $('#categoryid').val(),
        comid: $('#companyid').val(),
        graid: $('#gradeid').val()
    };
    ajaxwrapper.std($('#url').data('getipitemprice'), 'POST', $param,
        function () { _indicator.Show('#selitemdisptable'); },
        function (data) {
            dtDispServiceSelItems.cell($index, 5).data(data.Res.Price).draw();
            dtDispServiceSelItems.cell($index, 6).data(data.Res.Deductable).draw();
            dtDispServiceSelItems.cell($index, 7).data(data.Res.Discount).draw();
            dtDispServiceSelItems.cell($index, 8).data(data.Res.NetAmount).draw();
            dtDispServiceSelItems.cell($index, 13).data(data.Res.MUPer).draw();
            dtDispServiceSelItems.cell($index, 14).data(data.Res.MUAmt).draw();
            dtDispServiceSelItems.cell($index, 15).data(data.Res.DIPer).draw();
            dtDispServiceSelItems.cell($index, 16).data(data.Res.DIAmt).draw();
            dtDispServiceSelItems.cell($index, 17).data(data.Res.DEPer).draw();
            dtDispServiceSelItems.cell($index, 18).data(data.Res.DEAmt).draw();
            dtDispServiceSelItems.cell($index, 19).data(data.Res.DEType).draw();
            dtDispServiceSelItems.cell($index, 20).data(data.Res.ConQty).draw();
            _indicator.Stop('#selitemdisptable');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop('#selitemdisptable'); });
        }
    );
}
function remove_Service_Item(cell) {
    $row = $(cell).parents('tr');
    rowIndex = dtDispServiceSelItems.cell(cell).index().row;
    dtDispServiceSelItems.row($row).remove().draw();
}
/* ----------------------------------------------------------------------------------------------------------------------------
    [ Edit Existing Items ]
-------------------------------------------------------------------------------------------------------------------------------*/
function edit_Existing_Items(e) {
    $row = $(e).parents('tr');
    rowIndex = dtdisplaytable2.cell(e).index().row;
    if (dtdisplaytable2.cell(rowIndex, 12).data() === 0) {

        if (typeof $(e).closest("tr").find('td:eq(0)').html() != 'undefined') {
            var d = dtdisplaytable2.row($(e).parents('tr')).data();
            $('#gridIndex').val(rowIndex);

            $('#itemcode').val(dtdisplaytable2.cell($row, 14).data());
            $('#itemid').val(dtdisplaytable2.cell($row, 17).data());

            $('#desc').val(d["ItemName"]);
            $('#itemprice').val(parseFloat(d["EditPrice"]).toFixed(2));

            $("#itemSLNo").val(dtdisplaytable2.cell($row, 2).data());
            
           // $("#ebilldate").val(dtdisplaytable2.cell($row, 4).data());

            $("#ebilldate").datepicker('update', dtdisplaytable2.cell($row, 4).data());

            $('#itemqty').val(d["EditQuantity"]);

            $('#itemdisc').val(((parseFloat(d["Discount"]).toFixed(2) / parseFloat(d["EditPrice"] * d["EditQuantity"]).toFixed(2)) * 100).toFixed(2));
            $('#itemdiscamt').val(parseFloat(d["Discount"]).toFixed(2));

            if (d["Deductable"] == 1) {
                $('#itemded').val(((parseFloat(d["DeductableAmount"]).toFixed(2) / ((parseFloat(d["EditPrice"] * d["EditQuantity"]).toFixed(2)) - parseFloat(d["Discount"]).toFixed(2))) * 100).toFixed(2));
                $('#itemdedamt').val(parseFloat(d["DeductableAmount"] - parseFloat(d["Discount"]).toFixed(2)).toFixed(2));
            } else {
                $('#itemded').val(((parseFloat(d["DeductableAmount"]).toFixed(2) / parseFloat(d["EditPrice"] * d["EditQuantity"]).toFixed(2)) * 100).toFixed(2));
                $('#itemdedamt').val(parseFloat(d["DeductableAmount"]).toFixed(2));
            }
            if (dtdisplaytable2.cell($row, 12).data() === 1) {
                $('#deductype').select2('data', { id: 1, text: 'Deductable After Discount' });
            } else {
                $('#deductype').select2('data', { id: 0, text: 'Deductable Before Discount' });
            }

            $('#EditItemModal').modal('show');
        }
    }
}
/* ----------------------------------------------------------------------------------------------------------------------------
    [ Pre Calculation of the Bill ]
    as of Feb 02 2016
    - [ Feb. 18, 2016 ]
    - pharmacy will have its separate calculation during saving.
-------------------------------------------------------------------------------------------------------------------------------*/
function calc_Price($index) {
    var _PRICE = dtDispServiceSelItems.cell($index, 5).data() * 1;
    var _QTY = 1; // always one due to SQL is already considering the quantity
    var _MARKPER = dtDispServiceSelItems.cell($index, 13).data() * 1;
    var _MARKAMT = dtDispServiceSelItems.cell($index, 14).data() * 1;
    var _DISCPER = dtDispServiceSelItems.cell($index, 15).data() * 1;
    var _DISCAMT = dtDispServiceSelItems.cell($index, 16).data() * 1;
    var _DEDPER = dtDispServiceSelItems.cell($index, 17).data() * 1;
    var _DEDAMT = dtDispServiceSelItems.cell($index, 18).data() * 1;
    var _DEDTYPE = dtDispServiceSelItems.cell($index, 19).data() * 1;
    var _NET = 0;
    var _DED = 0;
    var _DIS = 0;
    var _MAR = 0;
    var _FPRICE = 0;

    //MARK UP
    if (_MARKPER == 0) {
        _MAR = (_PRICE * _QTY) + _MARKAMT;
    } else {
        _MAR = (_PRICE * _QTY) * (_MARKPER / 100);
    }
    //DISCOUNT
    if (_DISCPER == 0) {
        _DIS = (_PRICE * _QTY) + _DISCAMT;
    } else {
        _DIS = (_PRICE * _QTY) * (_DISCPER / 100);
    }
    if (_DEDTYPE == 0) // deductable before discount
    {
        if (_DEDPER == 0) {
            _DED = (_PRICE * _QTY) + _DISCAMT;
        } else {
            _DED = (_PRICE * _QTY) * (_DEDPER / 100);
        }
    }
    if (_DEDTYPE == 1) // deductable after discount
    {
        if (_DEDPER == 0) {
            _DED = ((_PRICE * _QTY) - _DIS) + _DISCAMT;
        } else {
            _DED = ((_PRICE * _QTY) - _DIS) * (_DEDPER / 100);
        }
    }
    _DED = _DED.toFixed(2);
    _DIS = _DIS.toFixed(2);
    _MAR = _MAR.toFixed(2);
    _NET = (_PRICE * _QTY) - _DED - _DIS;
    _NET = _NET.toFixed(2);
    _FPRICE = _PRICE * _QTY;
    dtDispServiceSelItems.cell($index, 6).data(_DED).draw();
    dtDispServiceSelItems.cell($index, 7).data(_DIS).draw();
    dtDispServiceSelItems.cell($index, 8).data(_NET).draw();
    dtDispServiceSelItems.cell($index, 24).data(_MAR).draw();
    dtDispServiceSelItems.cell($index, 5).data(_FPRICE).draw();
}
function save_Added_Items() {

    var _VALIDATE_PRICE = 0;

    if (dtDispServiceSelItems.data().length == 0) {
        ardialog.Pop(3, "Required", "No item(s) selected, Please select Item(s)...", "OK", function () {
            //_indicator.Stop('#itemdisptable');
        });

    } else {

        $BILL_ITEM_LIST = [];

        // prepare the parameters

        $.each(dtDispServiceSelItems.rows().data(), function (i, com) {

            if (dtDispServiceSelItems.cell(i, 5).data() <= 0) {

                ardialog.Pop(3, "Required", dtDispServiceSelItems.cell(i, 12).data() + " has <b>0</b> Price. Should be greater than 0. ", "OK", function () {
                    //_indicator.Stop('#itemdisptable');
                });
                $BILL_ITEM_LIST = [];
                _VALIDATE_PRICE = 1;
                return false;
            } else {
                $BILL_ITEM_LIST.push({
                    ItemId: dtDispServiceSelItems.cell(i, 9).data(),
                    Quantity: dtDispServiceSelItems.cell(i, 3).data(),
                    MarkUpAmount: dtDispServiceSelItems.cell(i, 24).data(),
                    Discount: dtDispServiceSelItems.cell(i, 7).data(),
                    DiscLevel: dtDispServiceSelItems.cell(i, 22).data(),
                    DeductableAmount: dtDispServiceSelItems.cell(i, 6).data(),
                    DedLevel: dtDispServiceSelItems.cell(i, 23).data(),
                    DeptId: dtDispServiceSelItems.cell(i, 10).data(),
                    BillDate: dtDispServiceSelItems.cell(i, 2).data(),
                    Price: dtDispServiceSelItems.cell(i, 5).data(),
                    ItemCode: dtDispServiceSelItems.cell(i, 11).data(),
                    ItemName: dtDispServiceSelItems.cell(i, 12).data()
                });
            }
        });

        if (_VALIDATE_PRICE == 0) {

            $par = {
                billaddparam: $BILL_ITEM_LIST
            }

            $params = {
                serid: $('#services').select2('val'),
                billno: $('#admitdate').select2('val'),
                IPB: $par
            }

            ardialog.Confirm(2, "Confirm", "You are about to Add/Modify Patient Bill, Continue?", "Yes", "No",
               function () {
                   ajaxwrapper.postsave($('#url').data('saveipbilladditem'), $params, function (data) {
                       // call service
                       get_Bill_Items($('#admitdate').select2('val'), $('#services').select2('val'));
                       // call adjsutments
                       get_IPBill_PosNeg_Adj($('#admitdate').select2('val'));

                       dtDispServiceSelItems.clear().draw();

                   });
               }, function () { }
            );
        }

       // alert(JSON.stringify($params));
    }

}
function update_Bill_Item() {
    //save_Added_Items

    $bdate = $("#ebilldate").datepicker('getDate');
    fbdate = moment($bdate).format('DD-MMM-YYYY');

    $params = {
        serid: $('#services').select2('val'),
        billno: $('#admitdate').select2('val'),
        slno: $('#itemSLNo').val(),
        disc: $('#itemdiscamt').val(),
        ded: $('#itemdedamt').val(),
        eqty: $('#itemqty').val(),
        eprice: $('#itemprice').val(),
        code: $('#itemcode').val(),
        name:$('#desc').val(),
        itemid: $('#itemid').val(),
        dtime: fbdate
    };

   // alert(JSON.stringify($params));

    ardialog.Confirm(2, "Confirm", "You are about to Add/Modify Patient Bill, Continue?", "Yes", "No",
        function () {
            ajaxwrapper.postsave($('#url').data('updateipbilladditem'), $params, function (data) {
                // call service
                get_Bill_Items($('#admitdate').select2('val'), $('#services').select2('val'));
                // call adjsutments
                get_IPBill_PosNeg_Adj($('#admitdate').select2('val'));
            });
        }, function () { }
    );

}
function delete_Bill_Item() {

    $HAS_SELECTED = 0;
    $.each(dtdisplaytable2.rows().data(), function (i, com) {
        if (dtdisplaytable2.cell(i, 12).data() == 1) {
            $HAS_SELECTED = $HAS_SELECTED + 1
        }
    });

    if ($HAS_SELECTED == 0) {
        ardialog.Pop(3, "Required", "There is no item selected, please select item(s).", "OK", function () {
            //_indicator.Stop('#itemdisptable');
        });
    } else {


        $BILL_ITEM_DEL = [];

        $.each(dtdisplaytable2.rows().data(), function (i, com) {

            if (dtdisplaytable2.cell(i, 12).data() == 1) {

                $BILL_ITEM_DEL.push({
                    ItemId: dtdisplaytable2.cell(i, 17).data(),
                    SLNo: dtdisplaytable2.cell(i, 2).data(),
                    CanRe: dtdisplaytable2.cell(i, 13).data()
                });
            }
        });

        $par = { dellistparams: $BILL_ITEM_DEL }

        $params = {
            serid: $('#services').select2('val'),
            billno: $('#admitdate').select2('val'),
            IPB: $par
        }

        ardialog.Confirm(2, "Confirm", "You are about to delete selected items, Continue?", "Yes", "No",
            function () {
                ajaxwrapper.postsave($('#url').data('deleteipbilladditem'), $params, function (data) {
                    // call service
                    get_Bill_Items($('#admitdate').select2('val'), $('#services').select2('val'));
                    // call adjsutments
                    get_IPBill_PosNeg_Adj($('#admitdate').select2('val'));
                });
            }, function () { }
        );
    }
}

/* ----------------------------------------------------------------------------------------------------------------------------
    [ Other Functions for Editting ]
-------------------------------------------------------------------------------------------------------------------------------*/
var editrec = {
    price: function () {
        //should recalculate the discount and deductables
        editrec.discP();
        editrec.dedP();
    },
    discP: function () {
        var _itemprice1 = $('#itemprice').val() * 1;
        var _discamount = (_itemprice1 * ($('#itemdisc').val() / 100)).toFixed(2);
        $('#itemdiscamt').val(_discamount);

    },
    discA: function () {
        var _itemprice2 = $('#itemprice').val() * 1;
        var _discpercent = (($('#itemdiscamt').val() / _itemprice2) * 100).toFixed(2);
        $('#itemdisc').val(_discpercent);
    },
    dedP: function () {
        var _itemprice3 = $('#itemprice').val() * 1;

        if ($('#deductype').select2('val') == 1) {
            _itemprice3 = _itemprice3 - $('#itemdiscamt').val();
        }

        var _dedamount = (_itemprice3 * ($('#itemded').val() / 100)).toFixed(2);
        $('#itemdedamt').val(_dedamount);
    },
    dedA: function () {
        var _itemprice4 = $('#itemprice').val() * 1;

        if ($('#deductype').select2('val') == 1) {
            _itemprice4 = _itemprice4 - $('#itemdiscamt').val();
        }
        _dedpercent = (($('#itemdedamt').val() / _itemprice4) * 100).toFixed(2);
        $('#itemded').val(_dedpercent);

    },
    dedPO: function (e) {
        var _itemprice5 = $('#itemprice').val() * 1;

        if (e == 1) {
            _itemprice5 = _itemprice5 - $('#itemdiscamt').val();
        }
        //alert(e);
        var _dedamount = (_itemprice5 * ($('#itemded').val() / 100)).toFixed(2);
        $('#itemdedamt').val(_dedamount);
    },
    clickOK: function () {
        gridIndex = $('#gridIndex').val();

        if (($('#itemprice').val() * 1) <= $('#itemdiscamt').val() ||
           ($('#itemprice').val() * 1) <= $('#itemdedamt').val()) {
            ardialog.Pop(4, "Invalid", "Discount and/or Deductables should not exceed the Price Amount", "OK", function () { });
        } else if ($.trim($('#itemcode').val()).length == 0) {
            ardialog.Pop(3, "Required", "Item Code is Required!", "OK", function () { });
        } else if ($.trim($('#itemname').val()).length == 0) {
            ardialog.Pop(3, "Required", "Item Name is Required!", "OK", function () { });
        } else {
            var iprice = ($('#itemprice').val() * 1).toFixed(2);
            var ipriceorig = ($('#itemprice').val() * 1).toFixed(2);
            var ibill = ($('#itemprice').val() * 1).toFixed(2);
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

                _common.bindselect2noclear('#correason', arcancellationreason, 'Select Reason...');

            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
    }
}
/* ----------------------------------------------------------------------------------------------------------------------------
    [ ON DOM Ready Script ]
-------------------------------------------------------------------------------------------------------------------------------*/
$(document).ready(function () {

    _common.makedate("#ebilldate", "dd-M-yyyy");

    _common.makedecimalplus('#itemprice, #itemdisc, #itemdiscamt, #itemded, #itemdedamt');
    _common.makeintegerplus('#itemqty');

    dedtype = [
     { id: 0, text: 'Deductable Before Discount' },
     { id: 1, text: 'Deductable After Discount' }
    ];
    _common.bindselect2noclear('#deductype', dedtype, 'Select Deductable Type...');
    
    editrec.getarcancellationreason(1);

    render_bills(null);

    _common.makepinonly("#pin");
    $("#pin").on("keydown", function (event) {
        if (event.which == 13) {
            _common.pinenter('#pin', function () {
                get_AdmissionDate($('#pin').val());
            });
            $(this).prop('disabled', true);
        }
    });

    $('#admitdate').on('select2-selecting', function (e) {
        get_PT_Information(e.val);
    });

    $('#services').on('select2-selecting', function (e) {
        get_Bill_Items($('#admitdate').select2('val'), e.val);

        get_IPBillService_Items(e.val,
                $('#companyid').val(),
                $('#gradeid').val())
        ;
    });

    $(document).on('click', '#allservice', function () {
        if ($('#allservice').is(':checked')) {
            get_IPBill_Services_All(
                $('#categoryid').val(),
                $('#companyid').val(),
                $('#gradeid').val()
            );
        } else {
            get_IPBill_Services($('#admitdate').select2('val'));
        }
    });

    /*  ADD ITEM POPUP  */

    $(document).on('click', '#btn_add_item', function () {
        $('#additemmodal').modal('show');
    });

    $("#additemmodal").on('shown.bs.modal', function (event) {

        setTimeout(function () {

            dtDispServiceItems.columns.adjust().draw();
            dtDispServiceSelItems.columns.adjust().draw();
            dtDispServiceSelItems.clear().draw();

        }, 1 * 100);
    });

    //$('#EditItemModal').on('show.bs.modal', function () {

        
    //});

    render_IPBillService_Items(null);
    render_IPBillService_SelItems(null);

    /* ADD ITEM */
    $(document).on('dblclick', '#itemdisptable tbody > tr > td', function (e) {
        add_Service_Item(this, e);
        return false;
    });

    /* REMOVE */
    $(document).on('dblclick', '#selitemdisptable tbody > tr > td:nth-of-type(2)', function (e) {
        remove_Service_Item(this);
    });

    $(document).on('dblclick', '#billdisptable tbody > tr > td:nth-of-type(4)', function (e) {
        edit_Existing_Items(this);
    });



    // Default value of Unit select2

    $DEF_UNIT = [
         { id: 1, text: 'Nos' }
    ];

    $(document).on('click', '#btn_additem_save', function () {
        save_Added_Items();
    });
    $(document).on('click', '#btn_delete_item', function () {
        delete_Bill_Item();
    });
   

    $(document).on('click', '#btn_clear, #btn_clear2', function () {
        _clear();
    });

    $(document).on('click', '#btn_reason_ok', function () {

        if ($('#correason').select2('val') == 0 || $('#correason').select2('val') == '') {
            ardialog.Pop(3, "Required", 'Please Select Reason...', "OK", function () { });

        } else {

            if ($('#commonforall').is(':checked')) {

                $IS_REASON_COMMON = 1;
                $REASON_ID = $('#correason').select2('val');
                $RESON_DESC = $('#correason').select2('data').text;
            } else {

                ardialog.Confirm(2, "Confirm", "Do you want this as the reason for all selected items?", "Yes", "No",
                      function () {
                          $IS_REASON_COMMON = 1;
                          $REASON_ID = $('#correason').select2('val');
                          $RESON_DESC = $('#correason').select2('data').text;
                      }, function () {

                          $IS_REASON_COMMON = 0;
                          $REASON_ID = 0;
                          $RESON_DESC = '';
                      }
                );
            }

            $row = $($CUR_CELL).parents('tr');
            state = duocheck.click($CUR_CELL);
            dtdisplaytable2.cell($row, 12).data(state);
            $('#corReasons').modal('hide');

            dtdisplaytable2.cell($row, 13).data($('#correason').select2('val'));
            dtdisplaytable2.cell($row, 11).data($('#correason').select2('data').text);
        }
    });

    $(document).on('click', '#financial', function () {
        if ($('#financial').is(':checked')) {
            editrec.getarcancellationreason(1);
        }
    });

    $(document).on('click', '#technical', function () {
        if ($('#technical').is(':checked')) {
            editrec.getarcancellationreason(9);
        }
    });


    $('#deductype').on('select2-selecting', function (e) {
        editrec.dedPO(e.val);
    });

    $(document).on('click', '#btn_edit_update', function () {
        update_Bill_Item();
    });

});