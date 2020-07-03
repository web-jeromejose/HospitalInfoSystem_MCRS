//
//Ready 
var model = $('#url').data('model');
console.log('model');
console.log(model);
$(document).ready(function () {

    createDailyJS.init();
});


//
function triggerevents() {
    var selecteditem = $("#DiagnosisSelectedValue").val();
    //console.log($("#DiagnosisSelectedValue").val());
    if (selecteditem != "") {
        var item = selecteditem.split("|");
        $("#txtDiagnosis").select2('data', { id: item[0], text: item[1] });
    }

}

//
function loadsel() {
    //ajaxWrapper.Get($("#url").data("getdiagnosis"), null, function (d, e) {
    //    var nationalityDT = d;
    //    Sel2Client($("#txtDiagnosis"), d, function (d, e) {
    //    });

    //    var arr = getObjects(nationalityDT, 'id', $("#Diagnosis").val());

    //    if (arr.length > 0) {
    //        $("#txtDiagnosis").select2('data', { id: arr[0].id, text: arr[0].name });
    //    }
    //});

    $("#txtDiagnosis").select2({
        allowClear: false,
        placeholder: "Search Diagnosis",
        minimumInputLength: 0,
        ajax: {
            cache: false,
            type: 'GET',
            dataType: "json",
            url: $("#url").data("getdiagnosis"),
            data: function (searchTerm) {
                return { name: searchTerm };
            },
            results: function (data) {
                return { results: data };
            }
        },
        dropdownAutoWidth: true
    }).on("change", function () {
        $("#Diagnosis").val($("#txtDiagnosis").select2('data').id);
        $("#DiagnosisSelectedValue").val($("#txtDiagnosis").select2('data').id + '|' + $("#txtDiagnosis").select2('data').text);
        //Callback($(this).select2('data'));
        console.log($(this).select2('data'));
        //var arr = getObjects(nationalityDT, 'id', $("#Diagnosis").val());

        //if (arr.length > 0) {
        //    $("#txtDiagnosis").select2('data', { id: arr[0].id, text: arr[0].name });
        //}
    });
}

//Function find in JSON
function getObjects(obj, key, val) {
    var objects = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] == 'object') {
            objects = objects.concat(getObjects(obj[i], key, val));
        } else if (i == key && obj[key] == val) {
            objects.push(obj);
        }
    }
    return objects;
}

function numbersOnly(element) {
    $(element).keypress(function (e) {
        //if the letter is not digit then display error and don"t type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            //NotifyError("Please enter numbers only", "Parameter Error");
            return false;
        }
    });
}

var createDailyJS = function () {

    var warning;

    /************************************************************** jqReport*******************************************************************      */
    function NotifySuccess(msg, module) {
        toastr.success(msg, module).options = {
            "closeButton": false,
            "debug": true,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "700",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
    }

    function NotifyError(msg, module) {

        toastr.warning(msg, module).options = {
            "closeButton": false,
            "debug": true,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "100",
            "hideDuration": "100",
            "timeOut": "100",
            "extendedTimeOut": "100",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        //toastr.success(msg);
        //// for errors - red box
        //toastr.error(msg);
        //// for warning - orange box
        //toastr.warning(msg);
        //// for info - blue box
        //toastr.info(msg);
    }

    //Validate Date and Time 
    function validateDateTime(dtfrom, dtto) {
        if (dtfrom.length < 0 || dtto.length < 0) return "";
        if (dtfrom.val() == null || dtfrom.val() === "") warning += "<p>Enter Start Date</p>";
        if (dtto.val() == null || dtto.val() === "") warning += "<p>Enter End Date</p>";

        var tfrom = new Date(dtfrom.val());
        tfrom = tfrom.getTime();
        var tto = new Date(dtto.val());
        tto = tto.getTime();
        if (tfrom > tto) warning += "<p>Start date should be less than the end date</p>";
    }

    //Validate Form
    function validateForm() {
        warning = "";

        //console.log($("LabelReportType"));
        //$('#Group:checked').val()
        //if ($("#txtFromDate").length > 0 && $("#txtToDate").length <= 0) warning += "<p>Please Enter Date</p>";
        //if ($("#id").length > 0 && typeof $('#id:checked').val() == "undefined") warning += "<p>Please Select from All, Charge Account or Doctor</p>";
        //if ($("#LengthOfStay").length > 0 && $("#LengthOfStay").val() == "") warning += "<p>Length of Stay is Required.</p>";
        //if ($("#txtDiagnosis").length > 0 && $("#txtDiagnosis").val() == "") warning += "<p>Diagnosis is Required.</p>";
        //if ($("#txtCity").length > 0 && $("#txtCity").val() == "") warning += "<p>City is Required.</p>";
        //if ($("#ReportType").length > 0 && typeof $('#ReportType:checked').val() == "undefined") warning += "<p>Please Select from Registration, Admission or Summary</p>";
        //if ($("#Group").length > 0 && typeof $('#Group:checked').val() == "undefined") warning += "<p>Please Select from Doctor, Nurse or Anesthesia</p>";
        //if ($("#txtFloor").length > 0 && $("#txtFloor").val() == "") warning += "<p>Floor is Required.</p>";
        //if ($("#txtSpeciality").length > 0 && $("#txtSpeciality").val() == "") warning += "<p>Speciality is Required.</p>";
        //if ($("#txtDoctor").length > 0 && $("#txtDoctor").val() == "") warning += "<p>Doctor is Required.</p>";
        //if ($("#txtPIN").length > 0 && $("#txtPIN").val() == "") warning += "<p>PIN is Required.</p>";
        //if ($("#txtServiceType").length > 0 && $("#txtServiceType").val() == "") warning += "<p>Service Type is Required.</p>";
        //if ($("#txtAgeRange").length > 0 && $("#txtAgeRange").val() == "") warning += "<p>Age Range is Required.</p>";
        //if ($("#txtYear").length > 0 && $("#txtYear").val() == "") warning += "<p>Year is Required.</p>";
        //if ($("#txtDepartment").length > 0 && $("#txtDepartment").val() == "") warning += "<p>Department is Required.</p>";
        //if ($("#txtNationality").length > 0 && $("#txtNationality").val() == "") warning += "<p>Nationality is Required.</p>";
        if ($("#txtFromDate").length > 0 && $("#txtToDate").length > 0) validateDateTime($("#txtFromDate"), $("#txtToDate"));
        if (($("#txtFromDate").val() == null || $("#txtFromDate").val() == "") && ($("#txtToDate").val() == null || $("#txtToDate").val() == "")) {
            warning += "<p>Please Enter Start and End Date</p>";
        }
        console.log(warning);
        if (warning != "") {

            NotifyError(warning, "Parameter Error");

            return false;
        } else {
            return true;
        }
    }




    /************************************************************** JS Function*******************************************************************      */

    var HandleJS = function () {

        function loaddatepicker() {
            $(".datepicker").datepicker({
                format: "mm/dd/yyyy",
                autoclose: true
            });

            //  $(".datepicker").inputmask("99-99-9999", { "placeholder": "MM-DD-YYYY" });

        }

        function initSelect2()
        {
            ajaxWrapper.Get($("#url").data("getdoclist"),null, function (xx, e) {
                Sel2Client($("#sel2doctor"), xx, function (x) {
                });
 
            });

          

        }

        $(document).ready(function () {
            loaddatepicker();
            initSelect2();
        });
    }

    return {
        //main function to initiate the module
        init: function () {
            HandleJS();
        }

    };

}();