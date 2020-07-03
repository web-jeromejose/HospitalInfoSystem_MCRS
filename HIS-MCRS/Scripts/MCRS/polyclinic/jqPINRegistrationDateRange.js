//
$(document).ready(function () {
    //
    loadsel();
    triggerevents();
});

//
function triggerevents() {
    $("#txtEmployee").on('change', function () {
        $("#Id").val($("#txtEmployee").select2('data').id);
    });
}

//
function loadsel() {
    ajaxWrapper.Get($("#url").data("getemployee"), null, function (d, e) {
        var employeeDT = d;
        Sel2Client($("#txtEmployee"), d, function (d, e) {
        });

        var arr = getObjects(employeeDT, 'id', $("#Id").val());

        if (arr.length > 0) {
            $("#txtEmployee").select2('data', { id: arr[0].id, text: arr[0].name });
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