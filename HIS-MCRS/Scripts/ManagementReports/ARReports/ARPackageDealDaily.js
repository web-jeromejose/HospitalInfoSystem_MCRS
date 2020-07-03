function ViewModel(startDate, endDate, urlElem) {
    self = this;
    self.urlActions = urlElem;
    self.StartDate = ko.observable(new Date(moment(startDate)));
    self.EndDate = ko.observable(new Date(moment(endDate)));
    self.FilterPackage = ko.observableArray([]);
    self.SelectedPackages = ko.observableArray([]);
    self.SearchString = ko.observable();
    self.ShowAllPackage = ko.observable(true);
    self.ShowFilteredPackage = ko.observable(false);
  
    self.SearchPackage = function (searchString) {

        self.FilterPackage([]);
        url = self.urlActions.data("getarpackagebills");

        param = {
            searchString: searchString
        }

        ajaxWrapper.PostWithLoading(url, param, $("#tbl_FilteredBillpackages"), function (data, e) {

            ko.utils.arrayForEach(data, function (item) {
                self.FilterPackage.push(item.PackageName);
            });
         });

    };

    self.SearchString.subscribe(function(newValue) {
        if (newValue) {
            self.ShowAllPackage(false);
            self.ShowFilteredPackage(true);
            if ($.trim(newValue).length > 1) {
                self.SearchPackage($.trim(newValue));
            }
        } else {
            self.ShowFilteredPackage(false);
            self.ShowAllPackage(true);
        }
    },this);

    self.ShowSelectedPackage = function () {
        self.ShowAllPackage(false);
        self.ShowFilteredPackage(true);
        self.FilterPackage([]);
        ko.utils.arrayPushAll(self.FilterPackage, self.SelectedPackages());
    }

    self.ShowPackages = function () {
        self.ShowFilteredPackage(false);
        self.ShowAllPackage(true);
    }
    self.PackageWrapperHeight = ko.observable("350px");
    self.ChangeHeight = function () {

        var refHeight = $("#reportWrapper").css("height").split("px")[0];
        self.PackageWrapperHeight(refHeight - 50 +"px");
    };

    self.SelectedPackageToJSON = ko.computed(function () {
        return JSON.stringify(self.SelectedPackages());
    });
}


