//
$(document).ready(function () {
    //
    loadsel();
    triggerevents();
});

//
function triggerevents() {
    $("#txtServiceType").on('change', function () {
        $("#Id").val($("#txtServiceType").select2('data').id);
    });
}

//
function loadsel() {
    ajaxWrapper.Get($("#url").data("getservicetype"), null, function (d, e) {
        var servicetypeDT = d;
        Sel2Client($("#txtServiceType"), d, function (d, e) {
        });

        var arr = getObjects(servicetypeDT, 'id', $("#Id").val());

        if (arr.length > 0) {
            $("#txtServiceType").select2('data', { id: arr[0].id, text: arr[0].name });
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