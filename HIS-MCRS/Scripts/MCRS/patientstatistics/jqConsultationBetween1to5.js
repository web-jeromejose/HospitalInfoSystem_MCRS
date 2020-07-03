//
$(document).ready(function () {
    //
    loadsel();
    triggerevents();
});

//
function triggerevents() {
    $("#txtYear").on('change', function () {
        $("#Year").val($("#txtYear").select2('data').id);
    });

    $("#txtDepartment").on('change', function () {
        $("#Dept").val($("#txtDepartment").select2('data').id);
    });
}

//
function loadsel() {
    ajaxWrapper.Get($("#url").data("getyear"), null, function (d, e) {
        var yearDT = d;
        Sel2Client($("#txtYear"), d, function (d, e) {
        });

        var arr = getObjects(yearDT, 'id', $("#Year").val());

        if (arr.length > 0) {
            $("#txtYear").select2('data', { id: arr[0].id, text: arr[0].name });
        }
    });

    ajaxWrapper.Get($("#url").data("getdepartment"), null, function (d, e) {
        var deptDT = d;
        Sel2Client($("#txtDepartment"), d, function (d, e) {
        });

        var arr = getObjects(deptDT, 'id', $("#Dept").val());

        if (arr.length > 0) {
            $("#txtDepartment").select2('data', { id: arr[0].id, text: arr[0].name });
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