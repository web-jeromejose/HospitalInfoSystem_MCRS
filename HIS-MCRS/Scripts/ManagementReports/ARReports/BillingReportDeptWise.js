function ViewModel(model,urlInput) {
    self = this;
    self.urlActions = urlInput;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.ReportOption = ko.observable(model.ReportOption);
    self.CategoryList = ko.observableArray(model.CategoryList);
    self.PatientTypeList = ko.observableArray(model.PatientTypeList);

    self.SelectedCategoryId = ko.observable(0);
    self.SelectedPatientTypeId = ko.observable(0);
    self.SelectedCategoryText = ko.observable(self.CategoryList()[0].Name);
    self.SelectedPatientTypeText = ko.observable("All");

    self.SelectedCategoryId.subscribe(function (value) {

        if (value !== null && value !== undefined) {

            $.each(self.CategoryList(),function (index, item) {

                if (item.Id == value) {
                    self.SelectedCategoryText(item.Name);
                }
            });
        }

    });

    self.SelectedPatientTypeId.subscribe(function (value) {

        if (value !== null && value !== undefined) {

            $.each(self.PatientTypeList(), function (index, item) {

                if (item.Key == value) {
                    self.SelectedPatientTypeText(item.Value);
                }
            });
        }

    });

}




