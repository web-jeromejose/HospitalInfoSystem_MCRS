
$('#reportWrapper').show();
$('#DTtablediv').hide();

$(document).ready(function () {
    var reporttype = $('#iChkSummary').is(':checked') ? 1 : 2;
    $("#ReportType").val(reporttype);
    CST.InitSelect2();
    CST.InitCheck();
    //CST.SubmitForm();
});

var CST = {
    InitSelect2: function () {
        Sel2Server($("#PIN"), $('#urlActions').data('s2allpins'), function (d) {
            $("#PIN").val(d.id);
            console.log(d);
            var param = { search: d.id };
            var url = $('#urlActions').data('s2admissiondate') + '?search=' + d.id;
            Sel2Server($("#AdmissionDate"), url, function (d) {
                console.log(d);

            });
        }, 'Search by PIN');

        Sel2Server($("#AdmissionDate"), $('#urlActions').data('s2admissiondate'), function (d) {
            console.log('AdmissionDate', d);
            $("#AdmissionDate").val(d.text);
            $("#BillNo").val(d.id);
        }, 'Search');

    },

    InitCheck: function () {
        $('#iChkSummary').click(function () {
            var reporttype = $('#iChkSummary').is(':checked') ? 1 : 2;
            $("#ReportType").val(reporttype);
        });
        $('#iChkDetail').click(function () {
            var reporttype = $('#iChkSummary').is(':checked') ? 1 : 2;
            $("#ReportType").val(reporttype);
        });
    },
    SubmitForm: function () {
       
        $('#RunReport').click(function () {
           
            var data = $("#AdmissionDate").select2('data');
            var reporttype = $('#iChkSummary').is(':checked') ? 1 : 2;
            $("#ReportType").val(reporttype);
            var url = $('#urlActions').data('showreport');

          
          
            
           
            //var param = {
            //    PIN: $("#PIN").val(),
            //    AdmissionDate: data.text,
            //    BillNo: data.id,
            //    ReportType: reporttype

            //}
        //    ajaxWrapper.Post(url, $('#form0').serialize(), function (data, e) {
        //});
        ////    //console.log(param);
        ////    ajaxWrapper.Post(url, param, function (data, e) {
        ////        $('#reportWrapper').show();
        ////        $('#DTtablediv').hide();
        ////    });

        });
    }
}


function Sel2Server(input, url, Callback, placeholder) {
    $(input).select2({
        allowClear: false,
        placeholder: placeholder,
        minimumInputLength: 0,
        ajax: {
            cache: false,
            type: 'GET',
            dataType: "json",
            url: url,
            data: function (searchTerm) {
                return { search: searchTerm };
            },
            results: function (data) {
                return { results: data };
            }
        },
        dropdownAutoWidth: true,
        formatResult: selectFormatName
    }).on("change", function () {
        Callback($(this).select2('data'));
    });
}





/*
function ViewModel(CSTAndPharmacyCostPrice) {
    
    self = this;
    self.inputUrlActions = null;
    self.PIN = ko.observableArray(CSTAndPharmacyCostPrice.PIN)
    self.AdmissionDate = ko.observableArray(CSTAndPharmacyCostPrice.StartDate);
    self.ReportType = ko.observable(CSTAndPharmacyCostPrice.ReportType);

    self.SelectedPIN = ko.observable(0);
    self.SelectedStartDate = ko.observable(0);
    
   
    self.PIN = function (query) {
        self.AdmissionDate([]);
        param = { search: query.term };
        url = self.inputUrlActions.data('s2allpins');
        ajaxWrapper.Get(url, param, function (data, e) {
            var filteredData = [];
            //filteredData = data;
            ko.utils.arrayForEach(data, function (res) {
                filteredData.push({ id: res.id, text: res.text });
            });
            query.callback({
                results: filteredData
            });
        });
    };

    self.AdmissionDate = function (query) {
        param = { search: self.SelectedPIN };
        url = self.inputUrlActions.data('s2admissiondate');
        ajaxWrapper.Get(url, param, function (data, e) {
            var filteredData = [];
            //filteredData = data;
            ko.utils.arrayForEach(data, function (res) {
                filteredData.push({ id: res.id, text: res.text });
            });
            //query.callback({
            //    results: filteredData
            //});
        });
    };

    //self.StartDate = function (query) {
    //    param = { searchString: query.term };
    //    url = self.inputUrlActions.data('s2allpins');
    //    ajaxWrapper.Get(url, param, function (data, e) {
    //        var filteredData = [];
    //        ko.utils.arrayForEach(data, function (res) {
    //            filteredData.push({ id: res.id, text: id.text });

    //        });
    //        query.callback({
    //            results: filteredData
    //        });
    //    });
    //};

    self.SearchDoctors = function (query) {
        param = { search: query.term };
        url = self.inputUrlActions.data('searchdoctors');
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

    //self.init = function () {

    //    self.Departments.unshift({ Name: "", Id: "0" });
    //}

    //self.init();
}
*/