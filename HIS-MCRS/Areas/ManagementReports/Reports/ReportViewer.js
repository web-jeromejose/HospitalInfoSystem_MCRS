Sys.Application.add_load(function () {
    $find("ReportViewer1").add_propertyChanged(viewerPropertyChanged);
});

function viewerPropertyChanged(sender, e) {
    if (e.get_propertyName() == "isLoading") {
        if ($find("ReportViewer1").get_isLoading()) {
            // Do something when loading starts
            
        }
        else {
            // alert($('#ReportViewer1_ctl05_ctl00_CurrentPage').val());
            setTimeout(function () {
                $('#cpage').val($('#ReportViewer1_ctl05_ctl00_CurrentPage').val());
                $('#tpage').html($('#ReportViewer1_ctl05_ctl00_TotalPages').html());

                if ($('#cpage').val() == 1) {
                    $('#Previous').prop('disabled', true);
                    //var $_PRE = true
                    //$('#First').prop('disabled', true);
                } else {
                    $('#Previous').prop('disabled', false);
                   // $('#First').prop('disabled', false);
                    //var $_PRE = false;
                }

                if ($('#cpage').val() === $('#tpage').html()) {
                    $('#Next').prop('disabled', true);
                   // $('#Last').prop('disabled', true);
                    //var $_NXT = true;
                } else {
                    $('#Next, #Last').prop('disabled', false);
                   // $('#Last').prop('disabled', false);
                    //var $_NXT = false;
                }
            }, 1 * 25);
           
        }
    }
};