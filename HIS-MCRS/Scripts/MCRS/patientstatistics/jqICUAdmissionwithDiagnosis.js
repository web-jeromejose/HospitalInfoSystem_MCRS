//
$(document).ready(function () {
    //
    loadsel();
    triggerevents();

    numbersOnly($("#LengthOfStay"));
    //$(".numbersOnly").inputmask("999");

    //var validator = $("#txtDiagnosis").data('validator');
    //validator.settings.ignore = "";
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