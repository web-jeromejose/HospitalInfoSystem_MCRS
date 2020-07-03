function ViewModel(model, urlActions) {
    self = this;

    self.Actions = urlActions;
    self.InvoiceNo = ko.observable();
    self.Pin = ko.observable();
    self.SearchByPin = ko.observable(model.SearchByPin);
    self.InvoiceTypeList = ko.observableArray(model.InvoiceTypeList);
    self.BillNoList = ko.observableArray();
    self.ReportOption = ko.observable(1);

    self.SelectedInvoiceType = ko.observable(0);
    self.SelectedBillNo = ko.observable();

    self.SelectedInvoiceType.subscribe(function (value) {

        if (value == 0) {
            self.ReportOption(1);
        } else {
            self.ReportOption(0);
        }

    });

    self.SearchByPin.subscribe(function (value) {

        self.BillNoList([]);
        self.SelectedBillNo('');
        self.BillNoList.push( {AdmitDatetime: null,BillNo:''});
    });

    self.SearchAdmissionsByBillNo = function () {
        self.BillNoList([]);
       if (self.InvoiceNo() != "") {
                url = self.Actions.data("getadmissionbybillno");
            param = { billNo: self.InvoiceNo() };

            ajaxWrapper.GetWithLoading(url, param, $("#BillNo"), function (data, e) {

                ko.utils.arrayPushAll(self.BillNoList, data);

                var test = self.BillNoList();

                if (self.BillNoList().length <= 0) {

                    self.Dialog(new Dialog("NOTIFICATION", "Record not found please try PIN or Invoice No. ", "alert-info", true));
                    self.BillNoList.push({ AdmitDatetime: null, BillNo: '' });
                }
            });
        } else {
            self.Dialog(new Dialog("ERROR", "Invoice No is required ", "alert-danger", true));
        }

    }

    self.SearchAdmissionsByPin = function () {

        self.BillNoList([]);
        if (self.Pin() != "") {
            url = self.Actions.data("getadmissionbypin");
            param = { pin: self.Pin() };

            ajaxWrapper.GetWithLoading(url, param, $("#Pin"), function (data, e) {

                ko.utils.arrayPushAll(self.BillNoList, data);
                if (self.BillNoList().length <= 0) {
                    self.Dialog(new Dialog("NOTIFICATION", "Record not found please try PIN or Invoice No. ", "alert-info", true));
                    self.BillNoList.push({ AdmitDatetime: null, BillNo: '' });
                }
            });
        } else {
            self.Dialog(new Dialog("ERROR", "PIN is requred", "alert-danger", true));
        }

    }

    self.Dialog = ko.observable(new Dialog("", "", "", false));
}

function Dialog(header, message, alertCSS, show) {
    this.Header = ko.observable(header);
    this.Message = ko.observable(message);
    this.AlertCSS = ko.observable(alertCSS);
    this.Show = ko.observable(show);
   
}