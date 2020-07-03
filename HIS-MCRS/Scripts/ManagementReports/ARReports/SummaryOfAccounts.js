function ViewModel(SummaryOfAccounts, elem) {
    model = SummaryOfAccounts
    self = this;
    console.log(model);
    self.inputUrlActions = elem;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
 
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.CategoryList = ko.observableArray(model.CategoryList);
    
    self.SubCategoryList = ko.observableArray(model.SubCategoryList);
 
    self.CompanyList = ko.observableArray([]);
  
    self.GradeList = ko.observableArray([]);
 
    self.Type = ko.observable(model.Type)
 
    self.BankDetails = ko.observable(model.BankDetails)
  
    self.AfterCoveringLetter = ko.observable(model.AfterCoveringLetter)
 
    self.SelectedCategory = ko.observable(self.CategoryList()[0].Id);
   
    self.SelectedCategoryText = ko.observable(self.CategoryList()[0].Name);
    self.SelectedSubCategory = ko.observable(0);
  
    self.SelectedSubCategoryText = ko.observable('ALL');
    self.SelectedCompany = ko.observable();
   
    self.SelectedGrade = ko.observable();
    self.btnExportExcel = ko.observable();

    self.BtnExcel = function () {
 
        if (self.Type() == 1) {


            param = {


                StartDate: moment(new Date(self.StartDate())).format("DD-MMM-YYYY")
                , EndDate: moment(new Date(self.EndDate())).format("DD-MMM-YYYY")
                , CategoryId: self.SelectedCategory()
                , AfterCoveringLetter: self.AfterCoveringLetter()
                , Type: self.Type()
            };
            console.log(param);
            ajaxWrapper.GetWithLoading('SummaryOfAccountswithVATEXCEL', param, $("#revenue-chart"), function (data, e) {
                console.log(data);

                $('#gridListSummaryofAccount').DataTable({
                    cache: false,
                    destroy: true,
                    data: data.list,
                    paging: true,
                    ordering: false,
                    searching: false,
                    info: true,
                    pagingType: "full_numbers",
                    lengthMenu: [[5, 50, 80, -1], [5, 50, 80, "All"]],
                    autoWidth: false,
                    scrollX: "100%",
                    dom: ' Bfrtip',
                    // "dom": ' B <"divtoolbar"i>rt<"bottom"flip><"clear">',
                    scrollCollapse: false,
                    columns: [

                        //VIEWING

                        { data: 'CCode', title: 'Code', className: ' ', visible: true, searchable: true, width: "2%", sortable: true }
                        , { data: 'CompanyName', title: 'Comp', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }
                        , { data: 'IPCount', title: 'IP Count', className: ' ', visible: true, searchable: true, width: "1%", sortable: true }
                        , { data: 'OPCount', title: 'OP Count', className: ' ', visible: true, searchable: true, width: "1%", sortable: true }
                        , { data: 'TCount', title: 'TOT Count', className: ' ', visible: true, searchable: true, width: "2%", sortable: true }

                        , { data: 'IPGross', title: 'IP Gross', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }
                        , { data: 'IPDiscount', title: 'IP Discount', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }
                        , { data: 'IPDeductable', title: 'IP Deduc', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }
                        , { data: 'IPNet', title: 'IP NET', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }
                        , { data: 'IPVat', title: 'IP VAT', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }

                        , { data: 'OPGross', title: 'OP Gross', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }
                        , { data: 'OPDiscount', title: 'OP Discount', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }
                        , { data: 'OPDeductable', title: 'OP Deduc', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }
                        , { data: 'OPNet', title: 'OP Deduc', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }
                        , { data: 'OPVat', title: 'OP VAT', className: ' ', visible: true, searchable: true, width: "5%", sortable: true }


                        , { data: 'TGross', title: 'TOT Gross', className: ' ', visible: true, searchable: true, width: "7%", sortable: true }
                        , { data: 'TDisc', title: 'TOT Disc', className: ' ', visible: true, searchable: true, width: "7%", sortable: true }
                        , { data: 'TDeduc', title: 'TOT Deduc', className: ' ', visible: true, searchable: true, width: "7%", sortable: true }
                        , { data: 'TNet', title: 'TOT Net', className: ' ', visible: true, searchable: true, width: "7%", sortable: true }
                        , { data: 'NetVat', title: 'TOT VAT', className: ' ', visible: true, searchable: true, width: "7%", sortable: true }
                        , { data: 'TNetVat', title: 'TOT Due', className: ' ', visible: true, searchable: true, width: "7%", sortable: true }





                    ]
                    , buttons: [
                        //{
                        //    extend: 'pdfHtml5', orientation: 'landscape', pageSize: 'A4', filename: 'SUMMARY_OF_ACCOUNTS_WITH_VAT', footer: true, message: 'messagef'
                        //    , messageBottom: ' ', messageTop: ' '
                        //    , title: 'SUMMARY OF ACCOUNTS WITH VAT REPORT '
                        //    , text: '<button type="button" style="color: #fff;background-color: #5cb85c;border-color: #4cae4c;" class="btn-margin-left btn btn-xs pull-left"> <i class="glyphicon glyphicon-print"></i> PDF </button><br><br>'
                        //    , customize: function (doc) {
                        //        //get date
                        //        var date = new Date();
                        //        Date.prototype.yyyymmdd = function () {
                        //            var yyyy = this.getFullYear().toString();
                        //            var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
                        //            var dd = this.getDate().toString();
                        //            return yyyy + "/" + (mm[1] ? mm : "0" + mm[0]) + "/" + (dd[1] ? dd : "0" + dd[0]); // padding
                        //        };

                        //        var cols = [];
                        //        cols[0] = { text: date.yyyymmdd(), alignment: 'left', margin: [20] };
                        //        cols[1] = { text: ' ', alignment: 'right', margin: [0, 0, 20] };
                        //        var objFooter = {};
                        //        objFooter['columns'] = cols;
                        //        doc['footer'] = objFooter;
                        //        doc.defaultStyle.fontSize = 8; //2,3,4,etc
                        //        doc.styles.tableHeader.fontSize = 9; //2, 3, 4, etc

                        //    }
                        //},
                        //other button
                        {
                            extend: 'excelHtml5', orientation: 'landscape', pageSize: 'A4', filename: 'SUMMARY_OF_ACCOUNTS_WITH_VAT_excel', footer: true, message: 'messagef'
                            , messageBottom: ' ', messageTop: ' '
                            , title: 'SUMMARY OF ACCOUNTS WITH VAT REPORT '
                            , text: '<button type="button"  class="btn-margin-left btn btn-info btn-xs pull-left"> <i class="glyphicon glyphicon-print"></i> Excel SUMMARY OF ACCOUNTS</button><br><br>'
                            , exportOptions: { columns: ':visible' }

                        },
                    ]


                });

            });
        } else {
            alert("This is just for details...");
        }
    }


    self.init = function () {
        self.SubCategoryList().unshift({ Id: 0, Name: 'ALL' });
       
        self.getCompanyByCategory();
      
        self.GradeList.push({ Id: 0, Name: "ALL" });
   
    }

    self.getCompanyByCategory = function () {
        self.CompanyList([]);
      
        url = self.inputUrlActions.data("getcompanybycategory");
     
        param = { categoryId: self.SelectedCategory()};

        ajaxWrapper.GetWithLoading(url, param, $("#s2id_CompanyId"), function (data, e) {
            var defaultValue = { Id: 0, Code: "", Name: "ALL" };
            self.CompanyList.push(defaultValue);
            for (i = 0; i < data.length; i++) {
                self.CompanyList.push({ Id: data[i].Id, Code: data[i].Code, Name: data[i].Name });
            };
        });
    }
 


    

    self.getGradeByCompanyId = function () {

        var testt = self.SelectedCompany();
        if (self.SelectedCompany() != undefined) {
            self.GradeList.removeAll();
            url = self.inputUrlActions.data("getgradebycompanyid");
            var id = self.SelectedCompany().Id;
            param = { companyId: id };

            ajaxWrapper.GetWithLoading(url, param, $("#s2id_GradeId"), function (data, e) {
              
                var defaultValue = { Id: 0, Name: "ALL" };

                ko.utils.arrayPushAll(self.GradeList, data);
                self.GradeList.unshift(defaultValue);
                self.SelectedGrade(0);
            });
        }
    }

   

    self.init();

    self.SelectedCategory.subscribe(function (newValue) {
        self.getCompanyByCategory();
        if (newValue) {
            $.each(self.CategoryList(), function (i, item) {
                if (item.Id == newValue) {
                    self.SelectedCategoryText(item.Name);
                }
            });
        }
    });

   
    
    
    
    self.SelectedCompany.subscribe(function () {
        self.getGradeByCompanyId();
    });
  
    self.SelectedSubCategory.subscribe(function (newValue) {
       
        if (newValue) {
            $.each(self.SubCategoryList(), function (i, item) {
                if (item.Id === newValue) {
                    self.SelectedSubCategoryText(item.Name);
                }
            });
        } else {
            self.SelectedSubCategoryText("ALL");
        }

    });

    self.EnableType = function () {

        return true;
        //if (self.SelectedCategory() == 23 || self.SelectedCategory() == 24 || self.SelectedCategory() == 70) {
        //    return true;
        //} else {
        //    return false;
        //}
    };
    
}