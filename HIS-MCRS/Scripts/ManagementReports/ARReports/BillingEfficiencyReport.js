function ViewModel(ARListOfCompanyWithTransactions) {
    model = ARListOfCompanyWithTransactions
    self = this;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.CategoryList = ko.observableArray(model.CategoryList);

    self.SelectedCategory = ko.observable(0);
    
    self.init = function () {

        self.CategoryList().unshift({ Id: 0, Name: 'ALL' });
    }

    
    self.init();
    
}