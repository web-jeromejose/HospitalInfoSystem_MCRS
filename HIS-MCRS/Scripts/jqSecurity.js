var ajaxWrapper = $.ajaxWrapper();
var PatientViewFrame = $('<div class="modal fade" id="_message-iframe" tabindex="-1" role="dialog" aria-hidden="true"><div class="modal-dialog" style="width: 100%;height:100%;"><iframe id="iframe-link" style="width: 100%;height:100%;"></iframe></div></div>');

$(document).ready(function () {
    GetMenu();
    $(document).on("click", "._iframeload", function () {
        var url = $(this).data('url');
        $("._mainiframe").append($(PatientViewFrame));
        $("#_message-iframe").modal();
        $("#iframe-link").attr('src', url);
    });
    $(document).on('hidden.bs.modal', "#_message-iframe", function () {
        $(this).remove();
    });    
});
function GetMenu() {
    ajaxWrapper.Get($("#txtstationame").data("appmenu"), null, function (d, e) {
        $("#url-menu").html(d);
    });
}

function SecureFeatures() {
    $(".featlink").addClass("disabled");
    $(".featlink").css("pointer-events", "none");
    ajaxWrapper.Get($("#txtstationame").data("urlsecure"), null, function (d, e) {
        $.each(d, function (index, name) {
            $('.featlink').each(function () {
                if ($(this).data("featid") == name.id) {
                    $(this).removeClass("disabled");
                    $(this).css("pointer-events", "visible");
                }
            });
        });
    });
}

function SecureFunction() {
     $(".func-btn").addClass("disabled");
    ajaxWrapper.Get($("#txtstationame").data("urlfunction"), null, function (d, e) {
        console.log('d');
        console.log(d);
        console.log(e);
        //NotAuthorizedpage notauthor
        if (d[0].FeatureID === "0") {
            console.log("//NotAuthorizedpage");

            ajaxWrapper.Post($("#txtstationame").data("notauthor"), null, function (d, e) {
                $("._mainiframe").html(d);
            });
        }
       

        //$.each(d, function (index, name) {
        //    $('.func-btn').each(function () {
        //        if ($(this).data("funcid") == name.id) {
        //            $(this).removeClass("disabled");
        //        }
        //    });
        //});
    });
}
