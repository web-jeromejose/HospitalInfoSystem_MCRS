function ViewModel(model,urlInput) {
    self = this;
    self.urlActions = urlInput;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    self.SelectAll = ko.observable(false);

    self.CategoryList = ko.observableArray(model.CategoryList);
    self.PatientTypeList = ko.observableArray(model.PatientTypeList);

    self.SelectedCategoryId = ko.observableArray();
    self.SelectedPatientType = ko.observable();

    self.SelectAllCategory = function () {
         ko.utils.arrayForEach(self.CategoryList(), function (item) {
            self.SelectedCategoryId.push(item.Id);
        });
    };

    self.SelectAll.subscribe(function (value) {

        self.SelectedCategoryId([]);

        if (value) {
            self.SelectAllCategory();
        }
    });


}




