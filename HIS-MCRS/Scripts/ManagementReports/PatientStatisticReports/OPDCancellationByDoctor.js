function ViewModel(model) {
   
    self = this;
    self.Uri;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    self.SelectedDoctor = ko.observable();
    self.ServiceId = ko.observable(0);
    self.SortBy = ko.observable(0);
    self.ReasonId = ko.observable(0);


    self.Services = ko.observableArray(model.Services);
    self.SortOptions = ko.observableArray(model.SortOptions);
    self.CancelBillReasons = ko.observableArray(model.CancelBillReasons);


    self.SearchDoctors = function (query) {
        param = { searchString: query.term };
        url = self.Uri.data('searchdoctors');
        ajaxWrapper.Get(url, param, function (data, e) {
            var filteredData = [];
            ko.utils.arrayForEach(data, function (doctor) {
                filteredData.push({ id: doctor.OperatorId, text: doctor.EmpCode + " - " + doctor.FullName });

            });
            query.callback({
                results: filteredData
            });
        });
    };

    self.Init = function () {
        self.Services.unshift({ Id: '', Name: '' });
        self.CancelBillReasons.unshift({ Id: '', Name: '' });


    };

    self.Init();
}