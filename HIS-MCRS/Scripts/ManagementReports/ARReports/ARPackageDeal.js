function ViewModel(model) {
    self = this;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.DepartmentList = ko.observableArray(model.DepartmentList);
    self.SelectedDepartmentId = ko.observable();
    self.ReportOption = ko.observable(model.ReportOption);
    self.init = function () {
        self.DepartmentList.unshift({ Id: 0, Name: 'ALL', Code: '' });
    }
    
    self.init();

    self.Department = ko.observable("ALL");
    self.SelectedDepartmentId.subscribe(function (newValue) {

        if (newValue) {
            $.each(self.DepartmentList(), function (index, item) {

                if (item.Id == newValue) {
                    self.Department(item.Name);
                    return false;
                }
            });

        }

    });
}
