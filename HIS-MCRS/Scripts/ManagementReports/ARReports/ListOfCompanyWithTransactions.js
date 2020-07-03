function ViewModel(ARListOfCompanyWithTransactions) {
    model = ARListOfCompanyWithTransactions
    self = this;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.CategoryList = ko.observableArray(model.CategoryList);

    self.SelectedCategory = ko.observable(model.CategoryList[0].Id);
    
}