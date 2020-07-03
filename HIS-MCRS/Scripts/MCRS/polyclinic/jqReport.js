var warning;

//Ready 
$(document).ready(function () {
    //
    //Global
    loaddatepicker();

    $(document).on('click', '#button1', function (e) {
        if(!validateForm()) e.preventDefault();
    });
});

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
    if ($("#txtSelection").length > 0 && $("#txtSelection").val() == "") warning += "<p>Selection is required.</p>";
    if ($("#txtEmployee").length > 0 && $("#txtEmployee").val() == "") warning += "<p>Please Select Employee</p>";
    if ($("#txtFromDate").length > 0 && $("#txtToDate").length > 0) validateDateTime($("#txtFromDate"), $("#txtToDate"));
    //if (($("#txtFromDate").val() == null || $("#txtFromDate").val() == "") && ($("#txtToDate").val() == null || $("#txtToDate").val() == "")) {
    //    warning += "<p>Please Enter Start and End Date</p>";
    //}
    
    if (warning != "") {
        NotifyError(warning, "Parameter Error");
        //alert(warning);
        return false;
    } else {
        return true;
    }
}

//Load Date Picker
function loaddatepicker() {
    $(".datepicker").datepicker({
        format: "mm/dd/yyyy",
        autoclose: true
    });

    $(".datepicker").inputmask("m/d/y", { "placeholder": "mm/dd/yyyy" });

    $('.datetimepicker').datetimepicker({
        language: 'en',
        pick12HourFormat: false
    });

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