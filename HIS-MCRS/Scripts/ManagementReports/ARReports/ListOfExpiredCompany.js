function ViewModel(ARListOfExpiredCompany) {
    model = ARListOfExpiredCompany
    self = this;
    self.ExpiryDate = ko.observable(new Date(moment(model.ExpiryDate)));
    self.CompanyStatus = ko.observable(model.CompanyStatus);
    self.CategoryList = ko.observableArray(model.CategoryList);

    self.SelectedCategory = ko.observable(model.CategoryList[0].Id);
    
}