function ViewModel(model,urlInput) {
    self = this;
    self.urlActions = urlInput;
    self.StartDate = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate = ko.observable(new Date(moment(model.EndDate)));
    self.Deal = ko.observable(model.Deal);
    self.ReportOption = ko.observable(model.ReportOption);
    self.CategoryList = ko.observableArray(model.CategoryList);
    self.ServiceList = ko.observableArray(model.ServiceList);

    self.SelectedCategoryId  = ko.observable(0);
    self.SelectedServiceId = ko.observable(0);
    self.SelectedCategoryText = ko.observable("All");
    self.SelectedServiceText = ko.observable();

    self.HasBaseCharge = ko.observable(false);
    self.BaseCharge = ko.observable('0.00');

    self.init = function () {
        self.CategoryList.unshift({ Id: 0, Name: 'ALL', Code: '' });
        self.ServiceList.unshift({ Id: 0, Name: 'ALL' });
    }
    
    self.init();

    self.Print = function () {

        var dummyiframe = new iframeform(self.urlActions.data("print"), "get");
        dummyiframe.send();

        return false;
    };

    self.EnablePrint = ko.observable(false);

    self.SelectedCategoryId.subscribe(function (value) {

        if (value !== null && value !== undefined) {

            $.each(self.CategoryList(),function (index, item) {

                if (item.Id == value) {
                    self.SelectedCategoryText(item.Name);
                }
            });
        }

    });
    self.SelectedServiceId.subscribe(function (value) {

        if (value !== null && value !== undefined) {

            $.each(self.ServiceList(), function (index, item) {

                if (item.Id == value) {
                    self.SelectedServiceText(item.Name);
                }
            });
        }

    });
}





function iframeform(url, method) {
    var object = this;
    object.time = new Date().getTime();
    object.form = $('<form action="' + url + '" target="iframe' + object.time + '" method="' + method + '" style="display:none;" id="form' + object.time + '" name="form' + object.time + '"></form>');

    object.form
    object.addParameter = function (parameter, value) {
        $("<input type='hidden' />")
         .attr("name", parameter)
         .attr("value", value)
         .appendTo(object.form);
    }

    object.send = function () {
        var iframe = $('<iframe data-time="' + object.time + '" style="display:none;" id="iframe' + object.time + '"></iframe>');
        $("body").append(iframe);
        $("body").append(object.form);
        object.form.submit();
        iframe.load(function () { $('#form' + $(this).data('time')).remove(); $(this).remove(); });
    }
}


function initDataTable() {
    $("#reportWrapper table").dataTable({ bFilter: false });
    $("#reportWrapper").css("background-color", "white");

    var count = $("#reportWrapper table td:not(.dataTables_empty)").length;

    if (count > 0) {
        viewModel.EnablePrint(true);
    } else {
        viewModel.EnablePrint(false);
    }
    _indicator.Stop();
}