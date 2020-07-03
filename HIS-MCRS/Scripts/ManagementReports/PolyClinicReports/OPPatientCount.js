function ViewModel(OPDPatientCount) {
    self = this;
    self.StartDate = ko.observable(new Date(moment(OPDPatientCount.StartDate)));
    self.EndDate = ko.observable(new Date(moment(OPDPatientCount.EndDate)));
    self.OptonList = ko.observableArray(OPDPatientCount.OPDSelectionList);
    self.SelectedOption = ko.observable(OPDPatientCount.OPDSelection);

}