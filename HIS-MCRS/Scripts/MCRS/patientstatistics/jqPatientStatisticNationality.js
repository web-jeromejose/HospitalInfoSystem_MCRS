//
$(document).ready(function () {
    //
    loadsel();
    triggerevents();
});

//
function triggerevents() {
    $("#txtNationality").on('change', function () {
        $("#Id").val($("#txtNationality").select2('data').id);
    });
}

//
function loadsel() {
    ajaxWrapper.Get($("#url").data("getnationality"), null, function (d, e) {
        var nationalityDT = d;
        Sel2Client($("#txtNationality"), d, function (d, e) {
        });

        var arr = getObjects(nationalityDT, 'id', $("#Id").val());

        if (arr.length > 0) {
            $("#txtNationality").select2('data', { id: arr[0].id, text: arr[0].name });
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