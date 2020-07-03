$(document).ready(function () {
    $("#GenderName").val("ALL");
    $("#SexID").change(function () {
        if ($(this).find("option:selected").text() != "All")
            $("#GenderName").val($(this).find("option:selected").text());
        else
            $("#GenderName").val("ALL");

    })

});