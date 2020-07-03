
$('#reportWrapper').show();
$('#DTtablediv').hide();

$('#DataTablePdf').click(function () {

    // $('#preloader').show();
    console.log($('#form0').serialize());
    ajaxWrapper.Get($("#urlActions").data("dashboarddatatable"), $('#form0').serialize(), function (data, e) {
        console.log(data.list);
        console.log($('#BillType').val() );
        if ($('#BillType').val() == "2")//billtype all
        { BindLoadDashboard(data.list); }
        else { BindLoadDashboardDeptName(data.list);  }
         
        
     });

    $('#DTtablediv').show();
    $('#reportWrapper').hide();
});
$('#RunReport').click(function () {
    $('#reportWrapper').show();
    $('#DTtablediv').hide();
});

// ----------------------------------------- Dasboard------------------------------------------------------------------------------------------------------------------


var TblLoadDashboard;
var TblLoadDashboardId = '#DTLoadDashboard';
var TblLoadDashboardDataRow;
var ennLoadDashboard = { Btn: 0, del: 1, ctr: 2, ID: 3, Name: 4, Edited: 5, Deleted: 6 };

var Format24 = "HH:mm";
var calcHeightLoadDashboard = function () {
    return $(window).height() * 80 / 100;
};

function ShowDashboardRowCallLoadDashboard() {
    var rc = function (nRow, aData) {
        var $nRow = $(nRow);
        //if (aData["HasAccess"] === 1) {
        //    //$('td:eq(3)', nRow).html("Yes");
        //    $nRow.addClass("row_green");
        //    $('#checkFunctionConfigRole', nRow).prop('checked', true);
        //}
 
        var $nRow = $(nRow);
        //if (status == 1) { // not screen
        //    $nRow.css({ "background-color": "#fcc9c9" })
        //    $('td:eq(6)', nRow).html('<b class="btn-margin-right btn btn-xs blue"> <i class="glyphicon glyphicon-unchecked"></i> NO </b>');
        //}
        //else if (status == 2) { // screen
        //    $nRow.css({ "background-color": "#ffffd9" })
        //    $('td:eq(6)', nRow).html('<b class="btn-margin-right btn btn-xs blue"> <i class="glyphicon glyphicon-check"></i> YES </b>');
        //}
        //if (screenresult == 1) {
        //    $('td:eq(5)', nRow).html(' <i class="  glyphicon glyphicon-minus"></i><small>Negative</small> ');
        //} else {
        //    $('td:eq(5)', nRow).html(' <i class="glyphicon glyphicon-plus"></i><small>Positive</small>  ');
        //}
        //$('td:eq(4)', nRow).html(' <i class="glyphicon glyphicon-tint"></i>  ' + aData['bloodgroupname']);



        //else if (value == 1) { // Crossmatched/Reserved
        //    $nRow.css({ "background-color": "#b7f5ff" })
        //    //$('# *').prop('disabled', true);
        //}
        //else if (value == 3) { // Incompatible unit(s) order
        //    $nRow.css({ "background-color": "#d3d3d2" })
        //}
        //else if (value == 10) { // Unreserved Units
        //    $nRow.css({ "background-color": "#ffffff" })
        //}

        //if (aData['status'] == 2) { // Stat Request Type
        //    //$('td:eq(0)', nRow).addClass("btn-data-priority");

        //}

    };
    return rc;
}


function BindLoadDashboardDeptName(data) {
    console.log(data);
    // http://legacy.datatables.net/release-datatables/examples/basic_init/scroll_x.html
    TblLoadDashboard = $(TblLoadDashboardId).DataTable({
        cache: false,
        destroy: true,
        data: data,
        paging: true,
        ordering: false,
        searching: true,
        info: true,
        processing: false,
        autoWidth: false,

        scrollCollapse: false,
        pageLength: 100,
        lengthChange: false,
        scrollY: calcHeightLoadDashboard(),
        scrollX: "100%",
        sScrollXInner: "100%",
        dom: '<"tbLoadDashboard">Bfrtip',
        //  dom: '<"tbLoadDashboard">Rlfrtip',
        columns: [

            { data: "OPID", title: 'OPID', className: '  ', visible: true, searchable: true, width: "2%" },
            { data: "BillNumber", title: 'Bill Number', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "BillDate", title: 'Bill Date', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "CancelDate", title: 'Cancel Date', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "TransactionMonth", title: 'TransactionMonth', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "PinNumber", title: 'PinNumber', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "BillType", title: 'Bill Type', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "DoctorCode", title: 'DoctorCode', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "CompanyCode", title: 'CompanyCode', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "CompanyName", title: 'CompanyName', className: '  ', visible: true, searchable: true, width: "2%" },

             { data: "DeptName", title: 'Medical Dept', className: '  ', visible: true, searchable: true, width: "2%" },//--not cancelled--canceellled

            // { data: "DepartmentName", title: 'Medical Dept', className: '  ', visible: true, searchable: true, width: "2%" },//--all
             { data: "ModeOfPayment", title: 'ModeOfPayment', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "ReceiptNo", title: 'ReceiptNo', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "EmployeeID", title: 'EmployeeID', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "Name", title: 'Name', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "ItemCode", title: 'Service Code', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "ItemName", title: 'Service Name', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "Quantity", title: 'Quantity', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "Rate", title: 'Rate', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "HISCashRevenue", title: 'HIS Revenue', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "DiscountPercentage", title: 'Discount Percentage', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "DiscountAmount", title: 'HIS Discount', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "PaidAmount", title: 'HIS Deductible', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "ChargeRevenue", title: 'HIS ChargeRevenue', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "Recievable", title: 'HIS Receivable', className: '  ', visible: true, searchable: true, width: "2%" },


        ],
        fnRowCallback: ShowDashboardRowCallLoadDashboard()
        // , buttons: [{ extend: 'pdfHtml5', orientation: 'landscape', pageSize: 'a4', filename: 'MCRS-IPRevenue', footer: true, message: 'messagef', messageBotton: 'test', messageTop: ' ', title: 'MCRS -IP Revenue', text: '<button type="button" style="color: #fff;background-color: #5cb85c;border-color: #4cae4c;" class="btn-margin-left btn btn-xs pull-left"> <i class="glyphicon glyphicon-print"></i> PRINT </button><br><br>' }]
         , buttons: [{ extend: 'excelHtml5', orientation: 'landscape', pageSize: 'a4', filename: 'MCRS-OPRevenue', footer: true, message: 'messagef', messageBotton: 'test', messageTop: ' ', title: 'MCRS -OP Revenue', text: '<button type="button" style="color: #fff;background-color: #5cb85c;border-color: #4cae4c;" class="btn-margin-left btn btn-xs pull-left"> <i class="glyphicon glyphicon-print"></i> Excel </button><br><br>' }]

    });


    var btns = '';
    var toolbar = btns.concat(
         '<div style="float:left;">'
         , '<button type="button" class="btn-margin-right btn btn-xs blue" Id="btnCheckAllFunctions"> <i class="glyphicon glyphicon-check"></i> PRINT </button>'
      //  , '<button type="button" class="btn-margin-right btn btn-xs blue" Id="btnUNCheckAllFunctions"> <i class="glyphicon glyphicon-unchecked"></i> All </button>'
        , '</div><br><br>');
    // $("div.tbLoadDashboard").html(toolbar);

    $('#btnCheckAllFunctions').click(function () {
        $('#preloader').show();
        $.each(TblLoadDashboard.rows().data(), function (i, row) {
            TblLoadDashboard.cell(i, 0).data('<input id="checkFunctionConfigRole" type="checkbox" checked="checked" >');
        });
        $('#preloader').hide();
    });
    $('#btnUNCheckAllFunctions').click(function () {
        $('#preloader').show();
        $.each(TblLoadDashboard.rows().data(), function (i, row) {
            TblLoadDashboard.cell(i, 0).data('<input id="checkFunctionConfigRole" type="checkbox">');
        });
        $('#preloader').hide();

    });

    $(TblLoadDashboardId + ' tbody').on('click', '#checkFunctionConfigRole', function (e) {
        var $cell = $(this).closest('td');
        var $row = $(this).closest('tr');
        var Tbl = TblLoadDashboard;
        var data = Tbl.row($row).data();
        var rowId = data[0];
        var col = Tbl.cell($(this).closest('td')).index();
        if (this.checked) {
            $row.addClass("row_green");
        } else {
            $row.removeClass("row_green");
        }
        e.stopPropagation();
    });
}

function BindLoadDashboard(data) {
    console.log(data);
    // http://legacy.datatables.net/release-datatables/examples/basic_init/scroll_x.html
    TblLoadDashboard = $(TblLoadDashboardId).DataTable({
        cache: false,
        destroy: true,
        data: data,
        paging: true,
        ordering: false,
        searching: true,
        info: true,
        processing: false,
        autoWidth: false,

        scrollCollapse: false,
        pageLength: 100,
        lengthChange: false,
        scrollY: calcHeightLoadDashboard(),
        scrollX: "100%",
        sScrollXInner: "100%",
        dom: '<"tbLoadDashboard">Bfrtip',
        //  dom: '<"tbLoadDashboard">Rlfrtip',
        columns: [
 
            { data: "OPID", title: 'OPID', className: '  ', visible: true, searchable: true, width: "2%" },
            { data: "BillNumber", title: 'Bill Number', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "BillDate", title: 'Bill Date', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "CancelDate", title: 'Cancel Date', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "TransactionMonth", title: 'TransactionMonth', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "PinNumber", title: 'PinNumber', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "BillType", title: 'Bill Type', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "DoctorCode", title: 'DoctorCode', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "CompanyCode", title: 'CompanyCode', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "CompanyName", title: 'CompanyName', className: '  ', visible: true, searchable: true, width: "2%" },
             
             //{ data: "DeptName", title: 'Medical Dept', className: '  ', visible: true, searchable: true, width: "2%" },--not cancelled--canceellled

             { data: "DepartmentName", title: 'Medical Dept', className: '  ', visible: true, searchable: true, width: "2%" },//--all
             { data: "ModeOfPayment", title: 'ModeOfPayment', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "ReceiptNo", title: 'ReceiptNo', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "EmployeeID", title: 'EmployeeID', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "Name", title: 'Name', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "ItemCode", title: 'Service Code', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "ItemName", title: 'Service Name', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "Quantity", title: 'Quantity', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "Rate", title: 'Rate', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "HISCashRevenue", title: 'HIS Revenue', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "DiscountPercentage", title: 'Discount Percentage', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "DiscountAmount", title: 'HIS Discount', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "DeductablePaid", title: 'HIS Deductible', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "ChargeRevenue", title: 'HIS ChargeRevenue', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "Recievable", title: 'HIS Receivable', className: '  ', visible: true, searchable: true, width: "2%" },
           

        ],
        fnRowCallback: ShowDashboardRowCallLoadDashboard()
          , buttons: [{ extend: 'excelHtml5', orientation: 'landscape', pageSize: 'a4', filename: 'MCRS-OPRevenue', footer: true, message: 'messagef', messageBotton: 'test', messageTop: ' ', title: 'MCRS -OP Revenue', text: '<button type="button" style="color: #fff;background-color: #5cb85c;border-color: #4cae4c;" class="btn-margin-left btn btn-xs pull-left"> <i class="glyphicon glyphicon-print"></i> Excel </button><br><br>' }]

    });


    var btns = '';
    var toolbar = btns.concat(
         '<div style="float:left;">'
         , '<button type="button" class="btn-margin-right btn btn-xs blue" Id="btnCheckAllFunctions"> <i class="glyphicon glyphicon-check"></i> PRINT </button>'
      //  , '<button type="button" class="btn-margin-right btn btn-xs blue" Id="btnUNCheckAllFunctions"> <i class="glyphicon glyphicon-unchecked"></i> All </button>'
        , '</div><br><br>');
    // $("div.tbLoadDashboard").html(toolbar);

    $('#btnCheckAllFunctions').click(function () {
        $('#preloader').show();
        $.each(TblLoadDashboard.rows().data(), function (i, row) {
            TblLoadDashboard.cell(i, 0).data('<input id="checkFunctionConfigRole" type="checkbox" checked="checked" >');
        });
        $('#preloader').hide();
    });
    $('#btnUNCheckAllFunctions').click(function () {
        $('#preloader').show();
        $.each(TblLoadDashboard.rows().data(), function (i, row) {
            TblLoadDashboard.cell(i, 0).data('<input id="checkFunctionConfigRole" type="checkbox">');
        });
        $('#preloader').hide();

    });

    $(TblLoadDashboardId + ' tbody').on('click', '#checkFunctionConfigRole', function (e) {
        var $cell = $(this).closest('td');
        var $row = $(this).closest('tr');
        var Tbl = TblLoadDashboard;
        var data = Tbl.row($row).data();
        var rowId = data[0];
        var col = Tbl.cell($(this).closest('td')).index();
        if (this.checked) {
            $row.addClass("row_green");
        } else {
            $row.removeClass("row_green");
        }
        e.stopPropagation();
    });
}


function ViewModel(OpRevenue) {
  self = this;
  self.inputUrlActions = null;
  self.StartDate = ko.observable(new Date(moment(OpRevenue.StartDate)));
  self.StartDate2 = ko.observable(new Date(moment(OpRevenue.StartDate2)));
  self.EndDate2 = ko.observable(new Date(moment(OpRevenue.EndDate2)));
  self.EndDate = ko.observable(new Date(moment(OpRevenue.EndDate)));
  self.PatientBillTypes = ko.observableArray(OpRevenue.PatientBillTypes);
  self.PatientBillTypes2 = ko.observableArray(OpRevenue.PatientBillTypes2);
  self.BillTypes =  ko.observableArray(OpRevenue.BillTypes);
  self.SortByCancellationDate = ko.observable(OpRevenue.SortByCancellationDate);
  self.Departments = ko.observableArray(OpRevenue.DepartmentList);

  self.SelectedDoctor = ko.observable(0);
  self.SelectedCompany = ko.observable(0);
  self.SelectedDepartment = ko.observable(self.Departments()[0]);
  self.SelectedPatienBillType = ko.observable(self.PatientBillTypes()[0]);
  self.SelectedPatienBillType2 = ko.observable(self.PatientBillTypes2()[0]);
  self.SelectedBillType = ko.observable(self.BillTypes()[0]);
 
  self.SearchCompanies = function (query) {
      param = { searchString: query.term };
      url = self.inputUrlActions.data('searchcompanies');
          ajaxWrapper.Get(url, param, function (data, e) {
              var filteredData = [];
              ko.utils.arrayForEach(data, function (company) {
                  filteredData.push({ id: company.Id, text: company.Code + " - " + company.Name });
                 
              });
              query.callback({
                  results: filteredData
              });
          }); 
  };

  self.SearchDoctors = function (query) {
      param = { searchString: query.term };
      url = self.inputUrlActions.data('searchdoctors');
      ajaxWrapper.Get(url, param, function (data, e) {
          var filteredData = [];
          ko.utils.arrayForEach(data, function (doctor) {
              filteredData.push({ id: doctor.OperatorId, text: doctor.EmpCode + " - " + doctor.FullName });

          });
          query.callback({
              results: filteredData
          });
      });
  };

  self.init = function () {

      self.Departments.unshift({ Name: "", Id: "0" });
  }

  self.init();
}