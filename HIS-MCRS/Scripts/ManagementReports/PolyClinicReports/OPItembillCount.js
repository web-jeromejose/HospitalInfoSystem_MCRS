

//ko.validation.configure({
//    registerExtenders: true,
//    messagesOnModified: true,
//    insertMessages: true,
//    parseInputAttributes: true,
//    messageTemplate: null,
//    decorateElement: true,
//    errorElementClass: 'errorcontainer',
//});
ko.validation.init({
    decorateElement: true,
    errorElementClass: 'errorcontainer',
    insertMessages: true
});


function ViewModel(SummaryOfCancelledAppointments) {
    model = SummaryOfCancelledAppointments;
    self = this;
    self.inputUrlActions;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
 
    self.SelectedServiceCodes = ko.observableArray().extend({
        required: {
            params: true,
            message: "*Service item is required"
        }
    });

    self.SearchServiceItem = function (query) {
        param = { searchString: query.term, serviceId: $('#ServiceId').val() };
        console.log(param);
        url = self.inputUrlActions.data('searchserviceitem');
        ajaxWrapper.Get(url, param, function (data, e) {
            var filteredData = [];
            ko.utils.arrayForEach(data, function (item) {
                filteredData.push({ id: item.ItemId, text: "(" + item.ItemCode + ")  " + item.ItemName });

            });
            query.callback({
                results: filteredData
            });
        });
    };

    self.submit= function () {
        if (self.errors().length > 0) {
            self.errors.showAllMessages();
        } else {
            return true;
        }
    }

    self.errors = ko.validation.group(self);


}

