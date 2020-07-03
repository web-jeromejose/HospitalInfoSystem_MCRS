function ViewModel(model,urlInput) {
    self = this;
    self.urlActions = urlInput;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.ReportOption = ko.observable(model.ReportOption);
    self.CategoryList = ko.observableArray(model.CategoryList);

    self.SelectedCategoryId  = ko.observable(0);
    self.SelectedCategoryText = ko.observable("All");

    self.init = function () {
        self.CategoryList.unshift({ Id: 0, Name: 'ALL', Code: '' });
       
    }
    
    self.init();

    self.SelectedCategoryId.subscribe(function (value) {

        if (value !== null && value !== undefined) {

            $.each(self.CategoryList(),function (index, item) {

                if (item.Id == value) {
                    self.SelectedCategoryText(item.Name);
                }
            });
        }

    });

}




