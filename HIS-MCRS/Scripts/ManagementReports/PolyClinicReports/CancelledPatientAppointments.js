function ViewModel(CancelledAppointments) {
    model = CancelledAppointments;
    self = this;
    self.inputUrlActions;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    self.PatientTypeList = ko.observableArray(model.PatientTypeList);
    self.DoctorsList = ko.observableArray([]);
    self.Departments = ko.observableArray(model.DepartmentList);

    self.SelectedDepartment = ko.observable(self.Departments()[0]);
    self.SelectedPatientType = ko.observable(model.PatientType);
    self.SelectedEmployee = ko.observable("");
    self.SelectedDoctor = ko.observable();
    self.SelectedCompany = ko.observable();

    self.SearchEmployee = function (query) {
        param = { searchString: query.term };
        url = self.inputUrlActions.data('searchemployee');
        ajaxWrapper.Get(url, param, function (data, e) {
            var filteredData = [];
            ko.utils.arrayForEach(data, function (emp) {
                filteredData.push({ id: emp.OperatorId, text: emp.EmployeeId + " - " + emp.FullName });

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

        self.Departments.unshift({ Name: "ALL", Id: 0 });
    }

    self.init();
}