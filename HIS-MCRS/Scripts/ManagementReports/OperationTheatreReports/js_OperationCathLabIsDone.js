
function ViewModel(model) {
    self = this;
    //self.StartDate = ko.observable(new Date(moment(model.StartDate
    self.StartDate = ko.observable(model.StartDate);
    self.EndDate = ko.observable(model.EndDate);
    self.OperationOrCatLab = ko.observable(model.OperationOrCatLab);
    self.isDone = ko.observable(model.isDone);
    //self.ReportOption = ko.observable(model.ReportOption);
    //self.CategoryList = ko.observableArray(model.CategoryList);

    //self.SelectedCategory = ko.observable();

    self.init = function () {
        //self.CategoryList.unshift({ Id: 0, Name: 'ALL', Code: '' });
    }

    self.init();
}

