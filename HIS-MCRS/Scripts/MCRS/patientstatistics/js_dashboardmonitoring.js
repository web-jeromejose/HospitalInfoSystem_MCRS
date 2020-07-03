//var dtdisplaytable;
var $DASH_INTERVAL = 20;

var $OPBILL_COUNT = 0;
var $OPBILL_AMOUNT = 0;
var $CANOPBILL_COUNT = 0;
var $CANOPBILL_AMOUNT = 0;
var graph_data = [];
$(document).ready(function () {

    render_cancellation_states(null)

    _SD = moment().get('year') + "-" + (moment().get('month') + 1) + "-" + moment().get('date');

    _DTODAY = moment(_SD).format('DD-MMM-YYYY');

    _common.makedate("#fdate", "DD, MM dd, yyyy");

    _common.makedate("#cfdate, #ctdate", "dd-M-yyyy");

    $(document).on('click', '#runreport', function () {
        _DFrom = $("#fdate").datepicker('getDate');

        _FDFROM = moment(_DFrom).format('DD-MMM-YYYY');
        get_Dashboard_Monitoring(_FDFROM);
        get_Dashboard_OR_Request(_FDFROM);

        get_Dashboard_OR_Request_FTD(_DTODAY);
        get_Dashboard_OR_Request_ORIG(_DTODAY);

    });

    get_BI_Service_Type();
    get_BI_Stations();
    get_BI_Reasons();
    get_BI_Operator();


    receipts = [
        { id: 1, text: 'Actual Receipts Cancelled' },
        { id: 2, text: 'All Receipts Cancelled' }
    ];

    _common.bindselect2noclear('#receipts', receipts, 'Select Filter...');
    $('#receipts').select2('data', { id: 1, text: 'Actual Receipts Cancelled' });

    chargetype = [
        { id: 0, text: '-- All --' },
        { id: 1, text: 'Cash' },
        { id: 2, text: 'Charge' }
    ];

    _common.bindselect2noclear('#chargetype', chargetype, 'Select Charge Type...');
    $('#chargetype').select2('data', { id: 0, text: '-- All --' });

    $('a[href="#cancellationstats"]').on('shown.bs.tab', function (e) {
        //e.target // newly activated tab
        //e.relatedTarget // previous active tab
        setTimeout(function () {
            dtdisplaytable.columns.adjust().draw();
        }, 1 * 150);
    });
    $(document).on('click', '#btn_run_stat', function () {

        _DFrom = $("#cfdate").datepicker('getDate');
        _TDate = $("#ctdate").datepicker('getDate');

        _FDFROM = moment(_DFrom).format('DD-MMM-YYYY');
        _FTDate = moment(_TDate).format('DD-MMM-YYYY');

        get_BI_Cancellation(
            _FDFROM,
            _FTDate,
            $('#sertype').select2('data').text,
            $('#station').select2('data').text,
            $('#chargetype').select2('data').text,
            $('#chargeoperator').select2('data').text,
            $('#reason').select2('data').text,
            $('#receipts').select2('val')
        );

        get_OPBill_Count(_FDFROM, _FTDate);

        get_OPBill_Amount(_FDFROM, _FTDate);

        get_CANOPBill_Count(_FDFROM, _FTDate,
            $('#receipts').select2('val'),
            $('#chargetype').select2('data').text);

        get_CANOPBill_Amount(_FDFROM, _FTDate,
            $('#receipts').select2('val'),
            $('#chargetype').select2('data').text);

        

    });

});

function get_Dashboard_Monitoring($date) {
    
    $param = { bdate: $date };
    ajaxwrapper.stdnasync($('#url').data('getdashboardmonitoring'), 'POST', $param,
        function () {
            //_indicator.Show('#dailydashboard');
        },
        function (data) {
            //alert(JSON.stringify(data.Res));
            $('#ipNOCI').val(data.Res.IPCurrentlyIn);
            $('#ipNOCIftd').val(data.Res.IPCurrentlyInFTD);
            $('#ipNOA').val(data.Res.IPAdmit);
            $('#ipNOAftd').val(data.Res.IPAdmitFTD);
            $('#ipNOD').val(data.Res.IPDis);
            $('#ipNODftd').val(data.Res.IPDisFTD);

            $('#delFTM').val(data.Res.DEForTheMonth);
            $('#delTD').val(data.Res.DEForTheDay);
            $('#delFTD').val(data.Res.DEToday);


            $('#erTC').val(data.Res.ERCons);
            $('#erTCftd').val(data.Res.ERConsForToday);
            $('#erTA').val(data.Res.ERAdmit);
            $('#erTAftd').val(data.Res.ERAdmitForToday);


            var _OPTotal = 0;

            _OPTotal = data.Res.OPCashTotal + data.Res.OPChargeTotal + data.Res.OPAramcoTotal;



            var _OPNOWPaidPer = 0 + '%';
            var _OPNOWFreePer = 0 + '%';

            if (data.Res.OPCurTotal != 0) {
                _OPNOWPaidPer = ((data.Res.OPCurPaid / data.Res.OPCurTotal) * 100).toFixed(2) + '%';
                _OPNOWFreePer = ((data.Res.OPCurFree / data.Res.OPCurTotal) * 100).toFixed(2) + '%';
            }

            $('#tpVcntfdd').val(_OPNOWPaidPer);
            $('#tpVperfdd').val(data.Res.OPCurPaid);

            $('#tfVcntfdd').val(_OPNOWFreePer);
            $('#tfVperfdd').val(data.Res.OPCurFree);

            $('#ttVcntfdd').val(data.Res.OPCurTotal);


            
            var _OPCPaidPer = 0 + '%';
            var _OPCFreePer = 0 + '%';
            var _OPCPer = 0 + '%';

            if (data.Res.OPCashTotal != 0) {
                _OPCPaidPer = ((data.Res.OPCashPaidCount / data.Res.OPCashTotal) * 100).toFixed(2) + '%';
                _OPCFreePer = ((data.Res.OPCashFreeCount / data.Res.OPCashTotal) * 100).toFixed(2) + '%';
                _OPCPer = ((data.Res.OPCashTotal / _OPTotal) * 100 ).toFixed(2) + '%';
            }

            $('#cspVper').val(_OPCPaidPer);
            $('#cspVcnt').val(data.Res.OPCashPaidCount);

            $('#csfVper').val(_OPCFreePer);
            $('#csfVcnt').val(data.Res.OPCashFreeCount);

            $('#cstVper').val(_OPCPer);
            $('#cstVcnt').val(data.Res.OPCashTotal);


            var _OPCHPaidPer = 0 + '%';
            var _OPCHFreePer = 0 + '%';
            var _OPCHPer = 0 + '%';

            if (data.Res.OPChargeTotal != 0) {
                _OPCHPaidPer = ((data.Res.OPChargePaidCount / data.Res.OPChargeTotal) * 100).toFixed(2) + '%';
                _OPCHFreePer = ((data.Res.OPChargeFreeCount / data.Res.OPChargeTotal) * 100).toFixed(2) + '%';
                _OPCHPer = ((data.Res.OPChargeTotal / _OPTotal) * 100).toFixed(2) + '%';
            }

            $('#chpVper').val(_OPCHPaidPer);
            $('#chpVcnt').val(data.Res.OPChargePaidCount);

            $('#chfVper').val(_OPCHFreePer);
            $('#chfVcnt').val(data.Res.OPChargeFreeCount);

            $('#chtVper').val(_OPCHPer);
            $('#chtVcnt').val(parseInt($('#chpVcnt').val()) + parseInt($('#chfVcnt').val()) );//data.Res.OPChargeTotal);

            var _OPARAMCOPaidPer = 0 + '%';
            var _OPARAMCOFreePer = 0 + '%';
            var _OPARAMCOPer = 0 + '%';

            if (data.Res.OPAramcoTotal != 0) {
                _OPARAMCOPaidPer = ((data.Res.OPAramcoPaidCount / data.Res.OPAramcoTotal) * 100).toFixed(2) + '%';
                _OPARAMCOFreePer = ((data.Res.OPAramcoFreeCount / data.Res.OPAramcoTotal) * 100).toFixed(2) + '%';
                _OPARAMCOPer = ((data.Res.OPAramcoTotal / _OPTotal) * 100).toFixed(2) + '%';
            }

            $('#arampVper').val(_OPARAMCOPaidPer);
            $('#arampVcnt').val(data.Res.OPAramcoPaidCount);

            $('#aramfVper').val(_OPARAMCOFreePer);
            $('#aramfVcnt').val(data.Res.OPAramcoFreeCount);

            $('#aramtVper').val(_OPARAMCOPer);
            $('#aramtVcnt').val(parseInt($('#arampVcnt').val()) + parseInt($('#aramfVcnt').val()) );//data.Res.OPAramcoTotal);


            var _OPTOTALPaidCount = 0;
            var _OPTOTALPaidPer = 0 + '%';
            var _OPTOTALFreeCount = 0;
            var _OPTOTALFreePer = 0 + '%';
            var _OPTOTALCombined = 0;
            var _OPTOTALPer = 0 + '%';

            
            _OPTOTALPaidCount = data.Res.OPCashPaidCount + data.Res.OPChargePaidCount + data.Res.OPAramcoPaidCount;
            _OPTOTALFreeCount = data.Res.OPCashFreeCount + data.Res.OPChargeFreeCount + data.Res.OPAramcoFreeCount;

            if (_OPTOTALPaidCount != 0) {
                _OPTOTALPaidPer = ((_OPTOTALPaidCount / _OPTotal) * 100).toFixed(2) + '%';
                _OPTOTALFreePer = ((_OPTOTALFreeCount / _OPTotal) * 100).toFixed(2) + '%';
               
            }


            $('#tpVper').val(_OPTOTALPaidPer);
            $('#tpVcnt').val(_OPTOTALPaidCount);

            $('#tfVper').val(_OPTOTALFreePer);
            $('#tfVcnt').val(_OPTOTALFreeCount);

            $('#ttVcnt').val(parseInt($('#cstVcnt').val()) + parseInt($('#chtVcnt').val()) + parseInt($('#aramtVcnt').val())   );//_OPTotal);


            $('#orFTD').val(data.Res.ORForTheDay);

            $('#orFTDftd').val(data.Res.ORForTheDayFTD);



            //_indicator.Stop('#dailydashboard');


            /* It will refresh after 20 secs */
            setTimeout(function () {
                _DFrom = $("#fdate").datepicker('getDate');

                _FDFROM = moment(_DFrom).format('DD-MMM-YYYY');
                get_Dashboard_Monitoring(_FDFROM);
            }, $DASH_INTERVAL * 1000);


        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
            });
        }
    );


    
    ajaxwrapper.stdnasync($('#url').data('graphdashboardmonitoring'), 'POST', $param,
        function () {         
        },
        function (data) {
            console.log(data);
            graph_data = data;
           

//FOR CASH
            var CHART_OUTPATIENT_CASH = document.getElementById('CHART_OUTPATIENT_CASH').getContext('2d');
           
            
            var cashLabel = [];
            var cashData = [];
           
            var cashCount = 0;
            $.each(graph_data, function (index, value) {
                cashCount =  Object.keys(value.CASH).length;
                $.each(value.CASH, function (ind, val) {                   
                    cashLabel.push(val.DeptName_CASH);
                    cashData.push(val.NO_PATIENT_CASH);
                });
               
            });

            //for CASH BG COLOR
            var i;
            var cashBGCOLOR = [];
            var randomScalingFactor = function () { return Math.round(Math.random() * 255) };
            for (i = 0; i < cashCount ; ++i) {
                cashBGCOLOR.push('rgba(' + randomScalingFactor() + ', ' + randomScalingFactor()+', 86, 0.2)');
            }
            
           
            var myChart = new Chart(CHART_OUTPATIENT_CASH, {
                type: 'horizontalBar',
                data: {
                    labels: cashLabel,// ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
                    datasets: [{
                        label: 'CASH',
                        data: cashData,// [12, 19, 3, 5, 2, 3],
                        backgroundColor: cashBGCOLOR,
                        //    [
                        //    'rgba(255, 99, 132, 0.2)',
                        //    'rgba(54, 162, 235, 0.2)',
                        //    'rgba(255, 206, 86, 0.2)',
                        //    'rgba(75, 192, 192, 0.2)',
                        //    'rgba(153, 102, 255, 0.2)',
                        //    'rgba(255, 159, 64, 0.2)'
                        //],
                        borderColor: cashBGCOLOR,
                        borderWidth: 1
                    }]
                },
                options: {
                    tooltips: {
                        enabled: true
                    }, hover: {
                        animationDuration: 1
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;
                            ctx.textAlign = 'center';
                            ctx.fillStyle = "rgba(0, 0, 0, 1)";
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                meta.data.forEach(function (bar, index) {
                                    var data = dataset.data[index];
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);

                                });
                            });
                        }
                    }
                    ,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });

//FOR CHARGE
            var CHART_OUTPATIENT_CHARGE = document.getElementById('CHART_OUTPATIENT_CHARGE').getContext('2d');

            var chargeLabel = [];
            var chargeData = [];

            var chargeCount = 0;
            $.each(graph_data, function (index, value) {
                chargeCount = Object.keys(value.CHARGE).length;
                $.each(value.CHARGE, function (ind, val) {
                    chargeLabel.push(val.DeptName_CASH);
                    chargeData.push(val.NO_PATIENT_CASH);
                });

            });

        
            var i;
            var chargeBGCOLOR = [];
            var randomScalingFactor = function () { return Math.round(Math.random() * 255) };
            for (i = 0; i < chargeCount; ++i) {
                chargeBGCOLOR.push('rgba(' + randomScalingFactor() + ', ' + randomScalingFactor() + ', 86, 0.2)');
            }


            var myChart = new Chart(CHART_OUTPATIENT_CHARGE, {
                type: 'horizontalBar',
                data: {
                    labels: chargeLabel,
                    datasets: [{
                        label: 'CHARGE',
                        data: chargeData,
                        backgroundColor: chargeBGCOLOR,                         
                        borderColor: chargeBGCOLOR,
                        borderWidth: 1
                    }]
                },
                options: {
                    tooltips: {
                        enabled: true
                    }, hover: {
                        animationDuration: 1
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;
                            ctx.textAlign = 'center';
                            ctx.fillStyle = "rgba(0, 0, 0, 1)";
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                meta.data.forEach(function (bar, index) {
                                    var data = dataset.data[index];
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);

                                });
                            });
                        }
                    }
                    ,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });




//FOR ARAMCO
            
            var CHART_OUTPATIENT_ARAMCO = document.getElementById('CHART_OUTPATIENT_ARAMCO').getContext('2d');

            var ARAMCOLabel = [];
            var ARAMCOData = [];

            var ARAMCOCount = 0;
            $.each(graph_data, function (index, value) {
                ARAMCOCount = Object.keys(value.ARAMCO).length;
                $.each(value.ARAMCO, function (ind, val) {
                    ARAMCOLabel.push(val.DeptName_CASH);
                    ARAMCOData.push(val.NO_PATIENT_CASH);
                });

            });


            var i;
            var ARAMCOBGCOLOR = [];
            var randomScalingFactor_ARAMCOBGCOLOR = function () { return Math.round(Math.random() * 255) };
            for (i = 0; i < ARAMCOCount; ++i) {
                ARAMCOBGCOLOR.push('rgba(' + randomScalingFactor_ARAMCOBGCOLOR() + ', ' + randomScalingFactor_ARAMCOBGCOLOR() + ', 120, 0.2)');
            }


            var myChart = new Chart(CHART_OUTPATIENT_ARAMCO, {
                type: 'horizontalBar',
                data: {
                    labels: ARAMCOLabel,
                    datasets: [{
                        label: 'ARAMCO',
                        data: ARAMCOData,
                        backgroundColor: ARAMCOBGCOLOR,
                        borderColor: ARAMCOBGCOLOR,
                        borderWidth: 1
                    }]
                },
                options: {
                    tooltips: {
                        enabled: true
                    }, hover: {
                        animationDuration: 1
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;
                            ctx.textAlign = 'center';
                            ctx.fillStyle = "rgba(0, 0, 0, 1)";
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                meta.data.forEach(function (bar, index) {
                                    var data = dataset.data[index];
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);

                                });
                            });
                        }
                    }
                    ,scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });

//FOR INPATIENT

            
            var CHART_INPATIENT = document.getElementById('CHART_INPATIENT').getContext('2d');
  
            var IP_date = {
                label: ' ' + moment($("#fdate").datepicker('getDate')).format('DD-MMM-YYYY')   ,
                data: [$('#ipNOCI').val(), $('#ipNOA').val(),$('#ipNOD').val()],
                backgroundColor: 'rgba(0, 99, 132, 0.6)',
                borderWidth: 0,
                yAxisID: "ipDate"
            };
            var IP_ForToday = {
                label: '(For Today)',
                data: [$('#ipNOCIftd').val(), $('#ipNOAftd').val(), $('#ipNODftd').val() ],
                backgroundColor: 'rgba(99, 132, 0, 0.6)',
                borderWidth: 0,
                yAxisID: "ipToday"
            };

            var planetData = {
                labels: ["Number of Currently - In", "Number of Admission", "Number of Discharge"],
                datasets: [IP_date, IP_ForToday]
            };

            var chartOptions = {
                scales: {
                    xAxes: [{
                        barPercentage: 1,
                        categoryPercentage: 0.6
                    }],
                    yAxes: [{
                        id: "ipDate"
                    }, {
                        id: "ipToday"
                    }]
                }
            };


            var barChart = new Chart(CHART_INPATIENT, {
                type: 'bar',
                data: planetData,
                options: chartOptions
            });
 
//FOR DELIVERY
 
            var CHART_DELIVERY = document.getElementById('CHART_DELIVERY').getContext('2d');
            var myChart = new Chart(CHART_DELIVERY, {
                type: 'horizontalBar',
                data: {
                    labels: ["Monthly", "Daily", "Today"],
                    datasets: [{
                        label: 'DELIVERY',
                        data: [$('#delFTM').val(), $('#delFTD').val(), $('#delTD').val()],
                     //   backgroundColor: ARAMCOBGCOLOR,
                      //  borderColor: ARAMCOBGCOLOR,
                        borderWidth: 1
                    }]
                },
                options: {
                    tooltips: {
                        enabled: true
                    }, hover: {
                        animationDuration: 1
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;
                            ctx.textAlign = 'center';
                            ctx.fillStyle = "rgba(0, 0, 0, 1)";
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                meta.data.forEach(function (bar, index) {
                                    var data = dataset.data[index];
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);

                                });
                            });
                        }
                    }
                    , scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });

 //FOR CHART_ER

            var CHART_ER = document.getElementById('CHART_ER').getContext('2d');
            var myChart = new Chart(CHART_ER, {
                type: 'horizontalBar',
                data: {
                    labels: [
                        "Consultation " + moment($("#fdate").datepicker('getDate')).format('DD-MMM')                     
                        , "Consultation Today"
                        , "Admission " + moment($("#fdate").datepicker('getDate')).format('DD-MMM')
                        , "Admission Today"],
                    datasets: [{
                        label: 'ER ',
                        data: [
                              $('#erTC').val()
                            , $('#delTD').val(), $('#erTCftd').val()
                            , $('#erTA').val()
                            , $('#erTAftd').val()
                        ],
                        backgroundColor: [
                            'rgba(0, 99, 132, 0.6)',
                            'rgba(150, 99, 132, 0.6)',
                            'rgba(0, 99, 132, 0.6)',
                            'rgba(150, 99, 132, 0.6)',
                        ],
                        //  borderColor: ARAMCOBGCOLOR,
                        borderWidth: 1
                    }]
                },
                options: {
                    tooltips: {
                        enabled: true
                    }, hover: {
                        animationDuration: 1
                    },
                    animation: {
                        duration: 1,
                        onComplete: function () {
                            var chartInstance = this.chart,
                                ctx = chartInstance.ctx;
                            ctx.textAlign = 'center';
                            ctx.fillStyle = "rgba(0, 0, 0, 1)";
                            ctx.textBaseline = 'bottom';

                            this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                meta.data.forEach(function (bar, index) {
                                    var data = dataset.data[index];
                                    ctx.fillText(data, bar._model.x, bar._model.y - 5);

                                });
                            });
                        }
                    }
                    , scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });




        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
            });
        }
    );

}

function get_Dashboard_OR_Request($date) {
    $param = { bdate: $date };
    ajaxwrapper.stdnasync($('#url').data('orrequest'), 'POST', $param,
        function () {
        },
        function (data) {
            $('#orSCH').val(data.Res.ORRequest);
            setTimeout(function () {
                _DFrom = $("#fdate").datepicker('getDate');
                _FDFROM = moment(_DFrom).format('DD-MMM-YYYY');
                get_Dashboard_OR_Request(_FDFROM);
            }, $DASH_INTERVAL * 1000);
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
            });
        }
     );
}

function get_Dashboard_OR_Request_FTD($date) {
    $param = { bdate: $date };
    ajaxwrapper.stdnasync($('#url').data('orrequestftd'), 'POST', $param,
        function () {
        },
        function (data) {
            $('#orSCHftd').val(data.Res.ORRequestFTD);
            setTimeout(function () {
                _SD = moment().get('year') + "-" + (moment().get('month') + 1) + "-" + moment().get('date');

                _DTODAY = moment(_SD).format('DD-MMM-YYYY');
                get_Dashboard_OR_Request_FTD(_DTODAY);
            }, $DASH_INTERVAL * 1000);


        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
            });
        }
     );
}

function get_Dashboard_OR_Request_ORIG($date) {
    $param = { bdate: $date };
    ajaxwrapper.stdnasync($('#url').data('origrequest'), 'POST', $param,
        function () {
        },
        function (data) {
            $('#orSCHftdorig').val(data.Res.OrigORRequest);

            setTimeout(function () {
                _SD = moment().get('year') + "-" + (moment().get('month') + 1) + "-" + moment().get('date');

                _DTODAY = moment(_SD).format('DD-MMM-YYYY');
                get_Dashboard_OR_Request_ORIG(_DTODAY);
            }, $DASH_INTERVAL * 1000);
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
            });
        }
     );
}

function get_BI_Service_Type() {
    $param = { };
    ajaxwrapper.std($('#url').data('getbiservicetype'), 'POST', $param,
        function () {
        },
        function (data) {

                comlist = [];
                comlist.push({ id: 0, text: '-- All --' });
                $.each(data.Res, function (i, com) {
                    comlist.push({ id: 1, text: com.Name });
                });
                _common.bindselect2noclear('#sertype', comlist, 'Select Service Type...');
                $('#sertype').select2('data', { id: 0, text: '-- All --' });
                
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {});
        }
    );
}

function get_BI_Stations() {
    $param = {};
    ajaxwrapper.std($('#url').data('getbistation'), 'POST', $param,
        function () {
        },
        function (data) {

            comlist = [];
            comlist.push({ id: 0, text: '-- All --' });
            $.each(data.Res, function (i, com) {
                comlist.push({ id: 1, text: com.Name });
            });
            _common.bindselect2noclear('#station', comlist, 'Select Station...');
            $('#station').select2('data', { id: 0, text: '-- All --' });
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { });
        }
    );
}

function get_BI_Reasons() {
    $param = {};
    ajaxwrapper.std($('#url').data('getbireason'), 'POST', $param,
        function () {
        },
        function (data) {

            comlist = [];
            comlist.push({ id: 0, text: '-- All --' });
            $.each(data.Res, function (i, com) {
                comlist.push({ id: 1, text: com.Name });
            });
            _common.bindselect2noclear('#reason', comlist, 'Select Station...');
            $('#reason').select2('data', { id: 0, text: '-- All --' });
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { });
        }
    );
}

function get_BI_Operator() {
    $param = {};
    ajaxwrapper.std($('#url').data('getbioperator'), 'POST', $param,
        function () {
        },
        function (data) {

            comlist = [];
            comlist.push({ id: 0, text: '-- All --' });
            $.each(data.Res, function (i, com) {
                comlist.push({ id: 1, text: com.Name });
            });
            _common.bindselect2noclear('#chargeoperator', comlist, 'Select Operator...');
            $('#chargeoperator').select2('data', { id: 0, text: '-- All --' });
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { });
        }
    );
}

function get_BI_Cancellation($fdate, $tdate, $sertype, $station, $billtype, $chargeby, $reason, $recfilter) {

    $param = {
        fdate: $fdate,
        tdate: $tdate,
        sertype: $sertype,
        station: $station,
        billtype: $billtype,
        chargeby: $chargeby,
        reason: $reason,
        recfilter: $recfilter
    };
    ajaxwrapper.std($('#url').data('getcancellation'), 'POST', $param,
        function () {
            _indicator.Show('#resulttable');
        },
        function (data) {
            render_cancellation_states(data.Res);
            _indicator.Stop('#resulttable');
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () {
                _indicator.Stop('#resulttable');
            });
        }
    );
}

function render_cancellation_states(result) {
    dtdisplaytable = $("#resulttable").DataTable({
        destroy: true,
        data: result,
        ordering: false,
        paging: false,
        searching: false,
        info: false,
        processing: false,
        scrollY: 375,
        scrollX: '100%',
        scrollCollapse: false,

        //dom: 'frtip',
        autoWidth: true,
        order: [[1, 'asc']],
        pageLength: 200,
        lengthChange: false,
        columns: [
          
            { data: "ServiceType", title: "Service Type", targets: 0, className: "IT_custom_content_align_center", width: '15%' },
            { data: "TCount", title: "Receipt Cancelled", targets: 1, className: "IT_custom_content_align_center", width: '10%' },
            { data: "TAmount", title: "Total Amount", targets: 2, className: "IT_custom_content_align_center", width: '10%' },
            { data: "Station", title: "Station", targets: 3, className: "IT_custom_content_align_left", width: '15%' },
            { data: "Operator", title: "Operator", targets: 4, className: "IT_custom_content_align_left", width: '25%' },
            { data: "Reason", title: "Reason", targets: 5, className: "IT_custom_content_align_left", width: '25%' },
           
        ]
        //,
        //createdRow: function (row, data) {
        //    if (data.Type == 1) {
        //        $(row).find('td:eq(0)').addClass('warning');
        //    } else {
        //        $(row).find('td:eq(0)').addClass('success');
        //    }
        //}
        //,
        //footerCallback: function (tfoot, data, start, end, display) {
        //    var api = this.api();

        //    $(api.column(7).footer()).html(
        //        api.column(8)
        //            .data()
        //            .reduce(function (a, b) {
        //                return a + b;
        //            }, 0).toFixed(2)
        //    );
        //    $('#grossamt').val(
        //        api.column(8)
        //            .data()
        //            .reduce(function (a, b) {
        //                return a + b;
        //            }, 0).toFixed(2)
        //    );
        //    $('#tolamtpd').val(
        //       (api.column(8)
        //            .data()
        //            .reduce(function (a, b) {
        //                return a + b;
        //            }, 0).toFixed(2)) - (api.column(12)
        //            .data()
        //            .reduce(function (a, b) {
        //                return a + b;
        //            }, 0).toFixed(2))
        //    );

        //    var secondRow = $(tfoot).next()[0];
        //    var nCells = secondRow.getElementsByTagName('td');
        //    nCells[7].innerHTML = (
        //             api.column(12)
        //            .data()
        //            .reduce(function (a, b) {
        //                return a + b;
        //            }, 0).toFixed(2)
        //        );
        //}

    });
    dtdisplaytable.on('order.dt search.dt', function () {
        dtdisplaytable.column(0, { search: 'applied', order: 'applied' })
            .nodes()
            .each(function (cell, i) { cell.innerHTML = i + 1; });
    }).draw();

    


}

/**/

function get_OPBill_Count($fdate, $tdate) {
    $param = {
        fdate: $fdate,
        tdate: $tdate
    };
    ajaxwrapper.std($('#url').data('getopcount'), 'POST', $param,
        function () {
        },
        function (data) {
            $OPBILL_COUNT = data.Res.opactualcount;
           
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { });
        }
    );
}

function get_OPBill_Amount($fdate, $tdate) {
    $param = {
        fdate: $fdate,
        tdate: $tdate
    };
    ajaxwrapper.std($('#url').data('getopamount'), 'POST', $param,
        function () {
        },
        function (data) {
            $OPBILL_AMOUNT = data.Res.opactualamount;
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { });
        }
    );
}

function get_CANOPBill_Count($fdate, $tdate, $receipts, $billtype) {
    $param = {
        fdate: $fdate,
        tdate: $tdate,
        receipts: $receipts,
        billtype: $billtype
    };
    ajaxwrapper.std($('#url').data('getcanopcount'), 'POST', $param,
        function () {
        },
        function (data) {
            $CANOPBILL_COUNT = data.Res.opcancount;
            $('#totalreceipt').val(($CANOPBILL_COUNT).toLocaleString("en"));
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { });
        }
    );
}

function get_CANOPBill_Amount($fdate, $tdate, $receipts, $billtype) {
    $param = {
        fdate: $fdate,
        tdate: $tdate,
        receipts: $receipts,
        billtype: $billtype
    };
    ajaxwrapper.std($('#url').data('getcaopamount'), 'POST', $param,
        function () {
        },
        function (data) {
            $CANOPBILL_AMOUNT = data.Res.opcanamount;
            $('#totalamount').val(($CANOPBILL_AMOUNT).toLocaleString("en"));

            setTimeout(function () {
                compute_statistics();
            }, 1 * 100);
        },
        function (err) {
            ardialog.Pop(4, "Error", err, "OK", function () { });
        }
    );
}

function compute_statistics() {
    _AMPER = $CANOPBILL_AMOUNT / $OPBILL_AMOUNT * 100;
    _COPER = $CANOPBILL_COUNT / $OPBILL_COUNT * 100;

    _AMPER = (_AMPER).toFixed(2) + '%';
    _COPER = (_COPER).toFixed(2) + '%';

    _TOPBILL = $OPBILL_AMOUNT;

    $('#pertotalamount').html(_AMPER + ' of Total Billed Amount ' + (_TOPBILL).toLocaleString("en") + ' for the Period');

    $('#pertotalcount').html(_COPER + ' of Total Billed Receipts ' + ($OPBILL_COUNT).toLocaleString("en") + ' for the Period');

}



