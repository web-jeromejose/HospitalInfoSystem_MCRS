function ViewModel(SummaryOfCancelledAppointments) {
    model = SummaryOfCancelledAppointments;
    self = this;
    self.inputUrlActions;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    self.PatientTypeList = ko.observableArray(model.PatientTypeList);
    self.DoctorsList = ko.observableArray([]);

    self.SelectedPatientType = ko.observable(model.PatientType);
    self.SelectedEmployee = ko.observable("");

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
}