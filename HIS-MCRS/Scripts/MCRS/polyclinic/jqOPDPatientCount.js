//
$(document).ready(function () {
    //
    loadsel();
    triggerevents();
});

//
function triggerevents() {
    $("#txtSelection").on('change', function () {
        $("#Id").val($("#txtSelection").select2('data').id);
    });
}

//
function loadsel() {
    ajaxWrapper.Get($("#url").data("getselection"), null, function (d, e) {
        var employeeDT = d;
        Sel2Client($("#txtSelection"), d, function (d, e) {
        });

        var arr = getObjects(employeeDT, 'id', $("#Id").val());

        if (arr.length > 0) {
            $("#txtSelection").select2('data', { id: arr[0].id, text: arr[0].name });
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