
ko.validation.init({
    decorateElement: true,
    errorElementClass: 'errorcontainer',
    insertMessages: true
});

function ViewModel(model) {
   
    self = this;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    self.FilterByBillNo = ko.observable(false);
    self.FilterByDate = ko.observable(true);
    self.Dialog = ko.observable(new Dialog("", "", "", false));
    self.BillNo = ko.observable().extend({
        required: {
            onlyIf: function() {
                return self.FilterByBillNo() === true;
            },
            message: "*Bill Number is required"
        }
    });

    self.Submit = function () {
        if (!self.FilterByBillNo() && !self.FilterByDate()) {
            self.Dialog(new Dialog("VALIDATION ERROR", "No report criteria has been selected ", "alert-danger", true));
            return false;
        } else if (self.errors().length > 0) {
            self.errors.showAllMessages();
        } else {

            return true;
        }
    };

    self.errors = ko.validation.group(self);
}

function Dialog(header, message, alertCSS, show, FnOk) {
    this.Header = ko.observable(header);
    this.Message = ko.observable(message);
    this.AlertCSS = ko.observable(alertCSS);
    this.Show = ko.observable(show);
    this.FnOK = function () {
        
    }
}
