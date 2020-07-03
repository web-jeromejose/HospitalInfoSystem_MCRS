function ViewModel(OPProcedureStatisctics) {
    self = this;
    self.StartDate = ko.observable(new Date(moment(OPProcedureStatisctics.StartDate)));
    self.EndDate = ko.observable(new Date(moment(OPProcedureStatisctics.EndDate)));
    self.SelectedReportType = ko.observable(OPProcedureStatisctics.ReportType);
    self.ReportTypeList = ko.observableArray(OPProcedureStatisctics.ReportTypeList);


    self.CategoryList = ko.observableArray(OPProcedureStatisctics.CategoryList);

    self.SelectedCategory = ko.observable(OPProcedureStatisctics.CategoryList[0].Id);






}