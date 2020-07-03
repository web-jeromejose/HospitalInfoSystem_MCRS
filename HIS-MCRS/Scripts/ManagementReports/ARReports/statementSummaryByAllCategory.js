function ViewModel(model) {
    self = this;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate   = ko.observable(new Date(moment(model.EndDate)));  
}
