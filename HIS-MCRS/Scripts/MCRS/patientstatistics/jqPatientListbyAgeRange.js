//
$(document).ready(function () {
    //
    loadsel();
    triggerevents();
});

//
function triggerevents() {
    $("#txtAgeRange").on('change', function () {
        $("#Id").val($("#txtAgeRange").select2('data').id);
    });

    $('input[type="checkbox"]').click(function () {
        var item = $(this).data("refbox");
        console.log(item);
        if (typeof item !== "undefined") {
            var toggleitem = $('*[data-ref="DateRange"]');
            console.log(toggleitem);
            if ($(this).prop("checked")) {
                toggleitem.removeProp("disabled");
                //if (item.indexOf('Time') > -1) {
                //    console.log(item.indexOf('Time'));
                //    console.log(toggleitem);
                //    if (toggleitem.value == null || toggleitem.value === "") {
                //        toggleitem.val("00:00");
                //    }
                //}
            } else {
                toggleitem.prop("disabled", true);
                toggleitem.val("");
            }
        }
    });
}

//
function loadsel() {
    ajaxWrapper.Get($("#url").data("getagerange"), null, function (d, e) {
        var agerangeDT = d;
        console.log(agerangeDT);
        Sel2Client($("#txtAgeRange"), d, function (d, e) {
        });

        var arr = getObjects(agerangeDT, 'id', $("#Id").val());

        if (arr.length > 0) {
            $("#txtAgeRange").select2('data', { id: arr[0].id, text: arr[0].name });
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