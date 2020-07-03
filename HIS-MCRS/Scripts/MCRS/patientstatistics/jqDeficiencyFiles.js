//
$(document).ready(function () {
    //
    loadsel();
    triggerevents();

    if ($('input:radio[name=ReportType]:checked').val() == 1) {
        $('#LabelReportType').replaceWith('<legend id="LabelReportType">Department Summary</legend>');
        $('input:radio[name=GraphType]').removeProp('disabled');
    }
});

//
function triggerevents() {
    $(document).on("change", "#txtFloor", function (e) {
        $("#Floors").val($("#txtFloor").select2('data').id);
    });

    $("input[name='ReportType']").click(function () {
        //$('#LabelReportType').remove();
        if ($('input:radio[name=ReportType]:checked').val() == "0") {
            $('#LabelReportType').replaceWith('<legend id="LabelReportType">Deficiency List</legend>');
            $('input:radio[name=GraphType]').prop('disabled', 'disabled');
            $("input[name='GraphType']").removeProp('checked');
        } else {
            $('#LabelReportType').replaceWith('<legend id="LabelReportType">Department Summary</legend>');
            $('input:radio[name=GraphType]').removeProp('disabled');
        }
    });

    $("input[name='GraphType']").dblclick(function () {
        $(this).prop('checked', false);
    });
}

//
function loadsel() {
    ajaxWrapper.Get($("#url").data("getfloors"), null, function (d, e) {
        var floorDT = d;
        Sel2Client($("#txtFloor"), d, function (d, e) {
        });

        var arr = getObjects(floorDT, 'id', $("#Floors").val());

        if (arr.length > 0) {
            $("#txtFloor").select2('data', { id: arr[0].id, text: arr[0].name });
        }
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