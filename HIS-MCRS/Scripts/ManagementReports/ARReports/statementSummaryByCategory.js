function ViewModel(model) {
    self = this;
    self.FromDate = ko.observable(model.FromDate);
    self.ToDate = ko.observable(model.ToDate);
}
