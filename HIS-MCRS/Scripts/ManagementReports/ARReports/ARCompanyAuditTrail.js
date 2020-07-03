function ViewModel(model) {
    self = this;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));

    self.CategoryList = ko.observableArray(model.CategoryList);
    self.SelectedCategory = ko.observable();
    
    self.init = function () {
        self.CategoryList.unshift({ Id: 0, Name: 'ALL', Code: '' });
    }
    
    self.init();

    self.CategoryText = ko.observable("ALL");
    self.SelectedCategory.subscribe(function (newValue) {

        if (newValue) {
            $.each(self.CategoryList(), function (index,item) {

                if (item.Id == newValue) {
                    self.CategoryText(item.Name);
                    return false;
                }
            });

        }

    });
}
