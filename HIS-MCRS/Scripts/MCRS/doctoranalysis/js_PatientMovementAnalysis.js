function onchangeDoctor(e) {
    $('#DoctorName').val($("#DoctorId option:selected").text());
    console.log($('#DoctorName').val());
}


function ViewModel(OpRevenue) {
    self = this;
    self.inputUrlActions = null;
    self.StartDate = ko.observable(new Date(moment(OpRevenue.StartDate)));
    self.EndDate = ko.observable(new Date(moment(OpRevenue.EndDate)));

    self.Departments = ko.observableArray(OpRevenue.DepartmentList);
    self.Doctors = ko.observableArray([]);
    self.Employees = ko.observableArray(OpRevenue.EmpList);

    self.SelectedDoctor = ko.observable();
    self.SelectedDepartment = ko.observable(self.Departments()[0]);
    self.SelectedEmployee = ko.observable(self.Employees()[0]);


    self.permissionChanged = function (obj, event) {

        self.Doctors([]);
        self.SelectedDoctor('');
        self.Doctors.push({ OperatorId: '', FullName: '' });
        param = { searchString: $('#DepartmentId').val() };
        url = self.inputUrlActions.data('searchdoctors');

        ajaxWrapper.Get(url, param, function (data, e) {
            ko.utils.arrayPushAll(self.Doctors, data);
        });
    }
    self.init = function () {
        self.Departments.unshift({ Name: "", Id: "" });
        self.Employees.unshift({ Name: "", Id: "" });
    }
    self.init();
}
