function ViewModel(model,urlInput) {
    self = this;

    self.CategoryList = ko.observableArray(model.CategoryList);

    self.SelectedCategoryId = ko.observable(self.CategoryList()[0].Id);

    self.SelectedCategoryText = ko.observable(self.CategoryList()[0].Name)

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





