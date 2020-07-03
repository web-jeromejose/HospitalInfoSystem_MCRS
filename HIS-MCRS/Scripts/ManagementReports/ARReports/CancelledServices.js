function ViewModel(model, urlActions) {
   
    self = this;
    self.URIs = urlActions;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.CategoryList = ko.observableArray(model.CategoryList);
    self.CompanyList = ko.observableArray();
    self.PatientTypeList = ko.observableArray(model.PatientTypeList);

    self.SelectedCategoryId = ko.observable(self.CategoryList()[0].Id);
    self.SelectedPatientTypeValue = ko.observable(self.PatientTypeList()[0].Key);
    self.SelectedCompanyId = ko.observable();

    self.SelectedCategoryText = ko.observable(self.CategoryList()[0].Name)
    self.SelectedPatientTypeText = ko.observable(self.PatientTypeList()[0].Value)

    self.SelectedCategoryId.subscribe(function (value) {

        if (value) {

            $.each(self.CategoryList(), function (index, item) {

                if (item.Id == value.Id) {
                    self.SelectedCategoryText(item.Name);
                }
            });

            self.GetCompanies();
        }

    });

    self.SelectedPatientTypeValue.subscribe(function (value) {

        if (value) {

            $.each(self.PatientTypeList(), function (index, item) {

                if (item.Key== value) {
                    self.SelectedPatientTypeText(item.Value);
                }
            });
        }

    });

    self.GetCompanies = function () {
        self.CompanyList([]);
       
        url = self.URIs.data("getcompanybycategory");

        param = {
            categoryId: self.SelectedCategoryId().Id
        }

        ajaxWrapper.PostWithLoading(url, param, $("#CompanyId"), function (data, e) {
            
            self.CompanyList.push({ Id: 0, Name: "ALL" });
            ko.utils.arrayPushAll(self.CompanyList, data);
            
        });

    };

    self.init = function () {
        self.CategoryList().unshift({ Id: 0, Code: '', Name: "ALL" });
    }
    self.init();
}