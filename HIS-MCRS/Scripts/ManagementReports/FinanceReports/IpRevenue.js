 
$('#reportWrapper').show();
 $('#DTtablediv').hide();

$('#DataTablePdf').click(function () {
  
   // $('#preloader').show();
    console.log($('#form0').serialize());
     ajaxWrapper.Get($("#urlActions").data("dashboarddatatable"), $('#form0').serialize(), function (data, e) {
         BindLoadDashboard(data.list);
       
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

        var status = aData['status'];
        var screenresult = aData['screenresult'];

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

  

            { data: "IPID", title: 'IPID', className: '  ', visible: true, searchable: true, width: "2%" },
            //{ data: "InvoiceNo", title: 'Invoice No', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "AdmissionDate", title: 'Admission Date', className: '  ', visible: true, searchable: true, width: "2%" },
       //      { data: "DischargeDate", title: 'Discharge Date', className: '  ', visible: true, searchable: true, width: "2%" },
           //  { data: "DischargeMonth", title: 'Discharge Month', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "PIN", title: 'PIN Number', className: '  ', visible: true, searchable: true, width: "2%" },
          //    { data: "BillType", title: 'BillType', className: '  ', visible: true, searchable: true, width: "2%" },
              //{ data: "", title: 'Gain / Loss  Patient', className: '  ', visible: true, searchable: true, width: "2%"
              //, render: function (row, type, data)
              //{
              
              //    var computation = (((+data.HISRevenue) * -1) - +data.PackageAmount);
              //    if (data.PIN == 0 || data.PIN == "") {
                     
              //        if (computation > 0) {
              //            return "Loss";
              //        } else {
              //            if (computation == 0) {
              //                return "-";
              //            } else { return "Gain"; }
              //        }

              //    } else {
                     
              //        if (computation > 0) {
              //            return "Loss";
              //        } else {
              //            if (computation == 0) {
              //                return "-";
              //            } else { return "Gain"; }
              //        }

              //    }

              //              }              
              //},
            //{ data: "RoomNo", title: 'RoomNo', className: '  ', visible: true, searchable: true, width: "2%" },
            //{ data: "DoctorCode", title: 'DoctorCode', className: '  ', visible: true, searchable: true, width: "2%" },
            //{ data: "MedicalDept", title: 'MedicalDept', className: '  ', visible: true, searchable: true, width: "2%" },
            //{ data: "CompanyCode", title: 'CompanyCode', className: '  ', visible: true, searchable: true, width: "2%" },
            { data: "CompanyName", title: 'CompanyName', className: '  ', visible: true, searchable: true, width: "2%" },
            { data: "EmpId", title: 'Employee Id', className: '  ', visible: true, searchable: true, width: "2%" },
       //     { data: "EmpName", title: 'Employee Name', className: '  ', visible: true, searchable: true, width: "2%" },
        //    { data: "ServiceCategory", title: 'ServiceCategory', className: '  ', visible: true, searchable: true, width: "2%" },
            { data: "ServiceCode", title: 'ServiceCode', className: '  ', visible: true, searchable: true, width: "2%" },
         //   { data: "ServiceMonth", title: 'ServiceMonth', className: '  ', visible: true, searchable: true, width: "2%" },
           
           
        //    { data: "Quantity", title: 'Quantity', className: '  ', visible: true, searchable: true, width: "2%" },
          //  { data: "Rate", title: 'Rate', className: '  ', visible: true, searchable: true, width: "2%" },
            { data: "HISRevenue", title: 'HISRevenue', className: '  ', visible: true, searchable: true, width: "2%" },
             { data: "DiscountPercentage", title: 'Discount Percentage', className: '  ', visible: true, searchable: true, width: "2%" },
              { data: "DiscountAmount", title: 'HIS Discount', className: '  ', visible: true, searchable: true, width: "2%" },
              { data: "PackageAmount", title: 'Package Deal Amount', className: '  ', visible: true, searchable: true, width: "2%" },
              {
                  data: "", title: 'HIS (Gain) / Loss ', className: '  ', visible: true, searchable: true, width: "2%" 
                  , render: function (row, type, data) {
                      if (data.PackageDealPatient == "Yes")
                      {
                          return "" + ((+data.HISRevenue * -1) - (+data.PackageAmount));
                      } else {
                          return "0";
                      }
                  }
              },
                {
                    data: "", title: 'HIS Cash Revenue ', className: '  ', visible: true, searchable: true, width: "2%"
                  , render: function (row, type, data) {
                      if (data.CompanyCode == "0000") {
                          return "" + ((+data.CashRevenue * -1) * -1);
                      } else {
                          return "0";
                      }
                  }
                },
                {
                    data: "", title: 'HIS Charge Revenue', className: '  ', visible: true, searchable: true, width: "2%"
                  , render: function (row, type, data) {
                      return "" + data.ChargeRevenue;
                       
                  }
                },
                 {
                     data: "", title: 'HIS Exclusions', className: '  ', visible: true, searchable: true, width: "2%"
                  , render: function (row, type, data) {
                      return "" + data.Exclusion;

                  }
                 },
               {
                   data: "", title: 'HIS Deductibles', className: '  ', visible: true, searchable: true, width: "2%"
                  , render: function (row, type, data) {
                      return "" + data.Deductibles;

                  }
               },
        {
            data: "", title: 'HIS Receivable', className: '  ', visible: true, searchable: true, width: "2%"
                  , render: function (row, type, data) {
                     

                      var compute = 0;
                      var total = 0;
                      if (data.PackageDealPatient == "Yes") {
                          compute = (((+data.HISRevenue) * -1) - +data.PackageAmount);
                          console.log('(data.DiscountAmount + compute)');
                          console.log((data.DiscountAmount + compute));
                          console.log('(data.HISRevenue)');
                          console.log((data.HISRevenue));
                          total = ((((+data.DiscountAmount + +compute) + (+data.HISRevenue))) * -1);
                          /*
                          -17.74 + 0.00 + 12.2928
 
                          */
                          return "" + total;
                      } else {
                          return "0.00";
                      }
                      
                  }
        },
    
 
          

        ],
        fnRowCallback: ShowDashboardRowCallLoadDashboard()
      // , buttons: [{ extend: 'pdfHtml5', orientation: 'landscape', pageSize: 'a4', filename: 'MCRS-IPRevenue', footer: true, message: 'messagef', messageBotton: 'test', messageTop: ' ', title: 'MCRS -IP Revenue', text: '<button type="button" style="color: #fff;background-color: #5cb85c;border-color: #4cae4c;" class="btn-margin-left btn btn-xs pull-left"> <i class="glyphicon glyphicon-print"></i> PRINT </button><br><br>' }]
         , buttons: [{ extend: 'excelHtml5', orientation: 'landscape', pageSize: 'a4', filename: 'MCRS-IPRevenue', footer: true, message: 'messagef', messageBotton: 'test', messageTop: ' ', title: 'MCRS -IP Revenue', text: '<button type="button" style="color: #fff;background-color: #5cb85c;border-color: #4cae4c;" class="btn-margin-left btn btn-xs pull-left"> <i class="glyphicon glyphicon-print"></i> Excel </button><br><br>' }]

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
    self.StartDateExcel = ko.observable(new Date(moment(OpRevenue.StartDateExcel)));
    self.EndDate = ko.observable(new Date(moment(OpRevenue.EndDate)));
    self.EndDateExcel = ko.observable(new Date(moment(OpRevenue.EndDateExcel)));
    self.PatientBillTypesExcel = ko.observableArray(OpRevenue.PatientBillTypesExcel);
    self.PatientBillTypes = ko.observableArray(OpRevenue.PatientBillTypes);
    self.Services = ko.observableArray(OpRevenue.Services);
    self.BillTypes = ko.observableArray(OpRevenue.BillTypes);
    self.BillTypesExcel = ko.observableArray(OpRevenue.BillTypesExcel);

    self.SortByCancellationDate = ko.observable(OpRevenue.SortByCancellationDate);
    self.Departments = ko.observableArray(OpRevenue.DepartmentList);

    self.SelectedDoctor = ko.observable(0);
    self.SelectedCompany = ko.observable(0);
    self.SelectedService = ko.observable();
    self.SelectedDepartment = ko.observable(self.Departments()[0]);
    self.SelectedPatienBillType = ko.observable(self.PatientBillTypes()[0]);
    self.SelectedPatienBillTypeExcel = ko.observable(self.PatientBillTypesExcel()[0]);
    self.SelectedBillType = ko.observable(self.BillTypes()[0]);
    self.SelectedBillTypeExcel = ko.observable(self.BillTypesExcel()[0]);
    self.BillTypeText = ko.observable(self.BillTypes()[0].Value);
    self.PatientTypeText = ko.observable(self.PatientBillTypes()[0].Value);
    self.IsRevenue = ko.observable(OpRevenue.IsRevenue);

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
        self.Services.unshift({ ServiceName: "", Id: "0" });
    }

    self.init();

    self.SelectedBillType.subscribe(function (val) {
        $.each(self.BillTypes(), function (i, item) {
            if (val == item.Key) {
                self.BillTypeText(item.Value);
            }
        })
    });

    self.SelectedPatienBillType.subscribe(function (val) {
        $.each(self.PatientBillTypes(), function (i, item) {
            if (val == item.Key) {
                self.PatientTypeText(item.Value);
            }
        })
    });
}