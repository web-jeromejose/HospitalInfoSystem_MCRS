function ViewModel(model, urlActions) {
   
    self = this;
    self.parseCategoryList = function () {
        var array = [];


        array.push({ Id: 0, Name: "ALL" });
        $.each(model.CategoryList, function (index, item) {
            array.push({ Id: item.Id, Name: item.Name });
        });

        return array;
    }
    self.URIs = urlActions;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    self.Pin = ko.observable();

    self.CategoryList = ko.observableArray(self.parseCategoryList());
    self.CompanyList = ko.observableArray([{ Id: 0, Name: "ALL" }]);


    self.SelectedCategoryId = ko.observable(0);
    self.SelectedCompanyId = ko.observable(0);

    self.SelectedCategoryName = ko.observable("ALL")
    self.SelectedCompanyName = ko.observable("ALL")

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
    self.SelectedCompanyId.subscribe(function (value) {

        if (value) {

            $.each(self.CompanyList(), function (index, item) {

                if (item.Id== value) {
                    self.SelectedCompanyName(item.Name);
                }
            });
        }

    });

    self.GetCompanies = function () {
        self.CompanyList([]);
       
        url = self.URIs.data("getcompanybycategory");

        param = {
            categoryId: self.SelectedCategoryId()
        }

        ajaxWrapper.PostWithLoading(url, param, $("#CompanyId"), function (data, e) {
            
            self.CompanyList.push({ Id: 0, Name: "ALL" });
            ko.utils.arrayPushAll(self.CompanyList, data);
            
        });

    };

   
}