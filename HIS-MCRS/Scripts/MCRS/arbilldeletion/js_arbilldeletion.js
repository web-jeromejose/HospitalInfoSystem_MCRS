$(document).ready(function () {
    _common.makedate("#fdate", "dd-M-yyyy");
    _common.makedate('#tdate', "dd-M-yyyy");

    _common.makepinonly("#pin");
    $("#pin").on("keydown", function (event) {
        if (event.which == 13) {
            _common.pinenter('#pin', function () {
                DFrom = $("#fdate").datepicker('getDate');
                DTill = $("#tdate").datepicker('getDate');
                valfrom = moment(DFrom).format('DD-MMM-YYYY');
                valtill = moment(DTill).format('DD-MMM-YYYY');
            });
            $(this).prop('disabled', true);
        }
    });


    //_common.getcommonlistnoclear("#billingofficer", 0, 9, "Select Billing Officer...", null, null);
    //render_categorylist(null);
    //render_companylist(null);

    //$('#billingofficer').on('select2-selecting', function (e) {

    //    get_billofficercategorylist(e.val);
    //    get_billofficercompanylist(e.val);

    //    $('#btn_sel_all_cat').prop('disabled', false);
    //    $('#btn_sel_all_com').prop('disabled', false);
    //    $('#btn_desel_all_com').prop('disabled', false);
    //});

    //$(document).on('click', '#btn_sel_all_com', function () {
    //    select_all_companies();
    //});

    //$(document).on('click', '#btn_desel_all_com', function () {
    //    deselect_all_companies();
    //});

    //$(document).on('click', '#btn_save', function () {
    //    save_billofficermapping();
    //});

});