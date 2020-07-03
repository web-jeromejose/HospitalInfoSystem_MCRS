function ViewModel(OpRevenue) {
    self = this;
    self.inputUrlActions = null;
    self.StartDate = ko.observable(new Date(moment(OpRevenue.StartDate)));
    self.EndDate = ko.observable(new Date(moment(OpRevenue.EndDate)));
    //self.PatientBillTypes = ko.observableArray(OpRevenue.PatientBillTypes);
    //self.BillTypes = ko.observableArray(OpRevenue.BillTypes);

    //self.SortByCancellationDate = ko.observable(OpRevenue.SortByCancellationDate);
    //self.Departments = ko.observableArray(OpRevenue.DepartmentList);

    self.SelectedDoctor = ko.observable();
    //self.SelectedCompany = ko.observable();
    //self.SelectedDepartment = ko.observable(self.Departments()[0]);
    //self.SelectedPatienBillType = ko.observable(self.PatientBillTypes()[0]);
    //self.SelectedBillType = ko.observable(self.BillTypes()[0]);
    //self.BillTypeText = ko.observable(self.BillTypes()[0].Value);
    //self.PatientTypeText = ko.observable(self.PatientBillTypes()[0].Value);

    //self.SearchCompanies = function (query) {
    //    param = { searchString: query.term };
    //    url = self.inputUrlActions.data('searchcompanies');
    //    ajaxWrapper.Get(url, param, function (data, e) {
    //        var filteredData = [];
    //        ko.utils.arrayForEach(data, function (company) {
    //            filteredData.push({ id: company.Id, text: company.Code + " - " + company.Name });

    //        });
    //        query.callback({
    //            results: filteredData
    //        });
    //    });
    //};

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

      
    }

    self.init();

    
}