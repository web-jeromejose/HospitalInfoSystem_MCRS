function ViewModel(model) {
    self = this;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    self.CreationType = ko.observable(model.CreationType);
    self.CategoryList = ko.observableArray(model.CategoryList);

    self.SelectedCategory = ko.observable();

    self.init = function () {
        self.CategoryList.unshift({ Id: 0, Name: 'ALL', Code: '' });
    }
    
    self.init();
}
