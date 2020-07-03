function ViewModel(model) {

    self = this;
    self.Uri;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    console.log(model.EndDate);
    //self.SelectedCompany = ko.observable();
    //self.DoctorId = ko.observable();
    //self.StationId = ko.observable();
    //self.Sort = ko.observable();

    self.Departments = ko.observableArray(model.DepartmentList);
    self.SelectedDepartment = ko.observable(self.Departments()[0]);
    //self.Stations = ko.observableArray(model.Departments);
    //self.SortOptions = ko.observableArray(model.SortOptions);

    //self.SearchCompanies = function (query) {
    //    param = { searchString: query.term };
    //    url = self.Uri.data('searchcompanies');
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

    //self.SearchDoctors = function (query) {
    //    param = { searchString: query.term };
    //    url = self.Uri.data('searchdoctors');
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

    self.init = function () {
        //self.Stations.unshift({ Id: '', Name: '' });
     self.Departments.unshift({ Name: "ALL", Id: "0" });
    };
    self.init();

    

}