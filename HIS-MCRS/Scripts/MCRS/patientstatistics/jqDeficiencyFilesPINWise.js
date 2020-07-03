//
$(document).ready(function () {
    //
    loadsel();
    triggerevents();

    if ($("#IncludeParameter").prop("checked")) $(".includeparameters").removeClass("hidden");
});

//
function triggerevents() {
    $(document).on("change", "#txtSpeciality", function (e) {
        $("#Speciality").val($("#txtSpeciality").select2('data').id);

        ajaxWrapper.Get($("#url").data("getdoctor"), { specialityid: $("#txtSpeciality").select2('data').id }, function (d, e) {
            var doctorDT = d;
            Sel2Client($("#txtDoctor"), d, function () {
            });

            var arr = getObjects(doctorDT, 'id', $("#Doctor").val());

            if (arr.length > 0) {
                $("#txtDoctor").select2('data', { id: arr[0].id, text: arr[0].name });
            }
        });
    });

    $(document).on("change", "#txtDoctor", function (e) {
        $("#Doctor").val($("#txtDoctor").select2('data').id);

        ajaxWrapper.Get($("#url").data("getpin"), {
            doctorid: $("#txtDoctor").select2('data').id,
            fromdate: $("#txtFromDate").val(),
            todate: $("#txtToDate").val()
        }, function (d, e) {
            var pinDT = d;
            Sel2Client($("#txtPIN"), d, function () {
            });

            var arr = getObjects(pinDT, 'id', $("#PIN").val());

            if (arr.length > 0) {
                $("#txtPIN").select2('data', { id: arr[0].id, text: arr[0].name });
            }
        });
    });

    $(document).on("change", "#txtPIN", function (e) {
        $("#PIN").val($("#txtPIN").select2('data').id);
    });

    $(document).on("change", "#txtStandard", function (e) {
        $("#Standard").val($("#txtStandard").select2('data').id);
    });

    $(document).on("change", "#IncludeStandards", function (e) {
        if (this.checked) {
            ajaxWrapper.Get($("#url").data("getstandard"), { isoldstandard: true }, function (d, e) {
                var standardDT = d;
                Sel2Client($("#txtStandard"), d, function (d, e) {
                });

                var arr = getObjects(standardDT, 'id', $("#Standard").val());

                if (arr.length > 0) {
                    $("#txtStandard").select2('data', { id: arr[0].id, text: arr[0].name });
                }
            });
        } else {
            ajaxWrapper.Get($("#url").data("getstandard"), { isoldstandard: false }, function (d, e) {
                var standardDT = d;
                Sel2Client($("#txtStandard"), d, function (d, e) {
                });

                var arr = getObjects(standardDT, 'id', $("#Standard").val());

                if (arr.length > 0) {
                    $("#txtStandard").select2('data', { id: arr[0].id, text: arr[0].name });
                }
            });
        }
    });

    $(document).on("change", "#IncludeParameter", function(e) {
        if (this.checked) {
            $(".includeparameters").removeClass("hidden");
        } else {
            $(".includeparameters").addClass("hidden");
        }
    });

}

//
function loadsel() {
    ajaxWrapper.Get($("#url").data("getspeciality"), null, function (d, e) {
        var specialityDT = d;
        Sel2Client($("#txtSpeciality"), d, function (d, e) {
        });

        var arr = getObjects(specialityDT, 'id', $("#Speciality").val());

        if (arr.length > 0) {
            $("#txtSpeciality").select2('data', { id: arr[0].id, text: arr[0].name });
        }

        if ($("#Speciality").val() != null || $("#Speciality").val() != "") {
            ajaxWrapper.Get($("#url").data("getdoctor"), { specialityid: $("#Speciality").val() }, function (d, e) {
                var doctorDT = d;
                Sel2Client($("#txtDoctor"), d, function () {
                });

                var arr = getObjects(doctorDT, 'id', $("#Doctor").val());

                if (arr.length > 0) {
                    $("#txtDoctor").select2('data', { id: arr[0].id, text: arr[0].name });
                }


            });
        }

        if ($("#Doctor").val() != null || $("#Doctor").val() != "") {
            ajaxWrapper.Get($("#url").data("getpin"), {
                doctorid: $("#Doctor").val(),
                fromdate: $("#txtFromDate").val(),
                todate: $("#txtToDate").val()
            }, function (d, e) {
                var pinDT = d;
                Sel2Client($("#txtPIN"), d, function () {
                });

                var arr = getObjects(pinDT, 'id', $("#PIN").val());

                if (arr.length > 0) {
                    $("#txtPIN").select2('data', { id: arr[0].id, text: arr[0].name });
                }
            });
        }

        
    });

    ajaxWrapper.Get($("#url").data("getdoctor"), null, function (d, e) {
        var doctorDT = d;

        Sel2Client($("#txtDoctor"), d, function () {
        });

        var arr = getObjects(doctorDT, 'id', $("#Doctor").val());

        if (arr.length > 0) {
            $("#txtDoctor").select2('data', { id: arr[0].id, text: arr[0].name });
        }
    });

    ajaxWrapper.Get($("#url").data("getstandard"), { isoldstandard: true }, function (d, e) {
        var standardDT = d;
        Sel2Client($("#txtStandard"), d, function (d, e) {
        });

        var arr = getObjects(standardDT, 'id', $("#Standard").val());

        if (arr.length > 0) {
            $("#txtStandard").select2('data', { id: arr[0].id, text: arr[0].name });
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