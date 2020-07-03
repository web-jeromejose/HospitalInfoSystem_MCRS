function ViewModel(model,urlInput) {
    self = this;
    self.urlActions = urlInput;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.ReportOption = ko.observable(model.ReportOption);
    self.CategoryList = ko.observableArray(model.CategoryList);

    self.SelectedCategoryId  = ko.observable(0);
    self.SelectedCategoryText = ko.observable("All");
    self.SelectedTestname = ko.observable();

    self.init = function () {
        self.CategoryList.unshift({ Id: 0, Name: 'ALL', Code: '' });
       
    }

    self.SearchTestName =  function (query) {
      param = { searchString: query.term };
      url = self.urlActions.data('gettestnameandcode');
      ajaxWrapper.Get(url, param, function (data, e) {
          var filteredData = [];
          ko.utils.arrayForEach(data, function (test) {
              filteredData.push({ id: test.Code + " - " + test.Name , text: test.Code + " - " + test.Name });

          });
          query.callback({
              results: filteredData
          });
      });
  };


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




