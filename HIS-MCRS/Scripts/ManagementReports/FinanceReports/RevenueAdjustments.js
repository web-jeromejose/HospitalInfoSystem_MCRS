function ViewModel(revenue) {
  self = this;
  self.inputUrlActions = null;
  self.StartDate = ko.observable(new Date(moment(revenue.StartDate)));
  self.EndDate = ko.observable(new Date(moment(revenue.EndDate)));
  self.Categories = ko.observableArray(revenue.Categories);
  self.Companies = ko.observableArray(revenue.Companies);
  self.BillTypes = ko.observableArray(revenue.BillTypes);
 


  self.SelectedCategoryId = ko.observable();
  self.SelectedCompanyId = ko.observable();
  self.SelectedBillTypeId = ko.observable(self.BillTypes()[0]);
 


  self.SearchCompanies = function (query) {
      param = { searchString: query.term };
      url = self.inputUrlActions.data('searchcompanies');
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

    self.Companies.unshift({ Name: "", Id: "" });
  }

  self.init();


  self.SelectedCategoryId.subscribe(function (val) {
      _indicator.Show($("#companyContainer div"));
      self.Companies([]);
      self.Companies.push({ Id: '', Name: '' });
      url = self.inputUrlActions.data('searchcompanies');
      param = { categoryId: val };
      ajaxWrapper.Get(url, param, function (data, e) {
          ko.utils.arrayPushAll(self.Companies, data);
          _indicator.Stop($("#companyContainer div"));
      });
  });

 
}