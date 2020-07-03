function ViewModel(OpRevenue, elem) {
    self = this;
    console.log('self123');
    console.log(self);
    console.log(OpRevenue);
 
    self.inputUrlActions = elem;
    self.StartDate = ko.observable(new Date(moment(OpRevenue.StartDate)));
    self.EndDate = ko.observable(new Date(moment(OpRevenue.EndDate)));
    self.PatientTypes = ko.observableArray(OpRevenue.PatientTypes);
    self.CoveringLetterTypes = ko.observableArray(OpRevenue.CoveringLetterTypes);
    // self.BillTypes = ko.observableArray(OpRevenue.BillTypes);
    // self.SortByCancellationDate = ko.observable(OpRevenue.SortByCancellationDate);
    self.Departments = ko.observableArray(OpRevenue.DepartmentList);
    self.CategoryList = ko.observableArray(OpRevenue.CategoryList);
   // self.CompanyList = ko.observableArray(OpRevenue.CompanyList);
    //self.SelectedDoctor = ko.observable();
    self.SelectedCompany = ko.observable();
    self.SelectedCategory = ko.observable(self.CategoryList()[0]);
    // self.SelectedCompany = ko.observable(self.CompanyList()[0]);
    self.SelectedPatientTypes = ko.observable(self.PatientTypes()[0]);
    self.SelectedCoveringLetterTypes = ko.observable(self.CoveringLetterTypes()[0]);
    // self.SelectedBillType = ko.observable(self.BillTypes()[0]);
    self.CompanyList = ko.observableArray([]);

    self.init = function () {

        self.Departments.unshift({ Name: "ALL", Id: "0" });
        self.CategoryList.unshift({ Name: "ALL", Id: "0" });
        self.getCompanyByCategory();
    }
    self.getCompanyByCategory = function () {
        self.CompanyList([]);

        url = self.inputUrlActions.data("getcompanybycategory");
        console.log(self.SelectedCategory());
        param = { categoryId: self.SelectedCategory() };

        ajaxWrapper.GetWithLoading(url, param, $("#s2id_CompanyId"), function (data, e) {
            var defaultValue = { Id: 0, Code: "", Name: "ALL" };
            self.CompanyList.push(defaultValue);
            for (i = 0; i < data.length; i++) {
                self.CompanyList.push({ Id: data[i].Id, Code: data[i].Code, Name: data[i].Name });
            };
        });
    }

    self.init();

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


    self.SelectedCategory.subscribe(function (newValue) {
        self.getCompanyByCategory();
        
    });


 

    //self.SearchDoctors = function (query) {
    //    param = { searchString: query.term };
    //    url = self.inputUrlActions.data('searchdoctors');
    //    ajaxWrapper.Get(url, param, function (data, e) {
    //        var filteredData = [];
    //        ko.utils.arrayForEach(data, function (doctor) {
    //            filteredData.push({ id: doctor.OperatorId, text: doctor.EmpCode + " - " + doctor.FullName });

    //        });
    //        query.callback({
    //            results: filteredData
    //        });
    //    });
    //};




}