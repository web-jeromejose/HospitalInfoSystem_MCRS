function ViewModel(model) {
    self = this;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.CategoryList = ko.observableArray(model.CategoryList);
    self.SelectedCategoryId = ko.observable();
    self.ReportOption = ko.observable(model.ReportOption);
   
    self.Category = ko.observable();
    self.SelectedCategoryId.subscribe(function (newValue) {

        if (newValue) {
            $.each(self.CategoryList(), function (index,item) {

                if (item.Id == newValue) {
                    self.Category(item.Name);
                    return false;
                }
            });

        }

    });
}
