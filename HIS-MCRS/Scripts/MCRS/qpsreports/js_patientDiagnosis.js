function ViewModel(ARListOfCompanyWithTransactions) {
    model = ARListOfCompanyWithTransactions
    self = this;
    self.inputUrlActions = null;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    self.InPatient = ko.observable('0');
    //self.Department = ko.observable(model.Department);
    self.SelectedCompany = ko.observable();


    self.Category = ko.observableArray(model.Category);
    self.Departments = ko.observableArray(model.Department);
    self.SelectedDepartment = ko.observable(self.Departments()[0]);
    self.Categories = ko.observableArray(model.Category);
    self.SelectedCategory = ko.observable(self.Categories()[0]);

    self.SearchCompanies = function (query) {

        param = { searchString: query.term };
        url = self.inputUrlActions.data('searchcompanies');
        console.log(url);
        ajaxWrapper.Get(url, param, function (data, e) {
            var filteredData = [];
            ko.utils.arrayForEach(data, function (company) {
                filteredData.push({ id: company.Id, text: company.Code + " - " + company.Name });

            });
            query.callback({
                results: filteredData
            });
        });
    };

    self.init = function () {

        self.Categories.unshift({ Name: "ALL", Id: "0" });
        self.Departments.unshift({ Name: "ALL", Id: "0" });
    }

    self.init();




    //self.SelectedCategory = ko.observable(model.CategoryList[0].Id);

}