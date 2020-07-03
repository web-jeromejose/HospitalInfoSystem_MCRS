var Select2IsClicked = false;
var _common = {
    pinenter: function (el, callb) {
        p = _common.formattopin($(el).val());
        $(el).val(p);
        if (callb != null) {
            callb();
        }
    },
    makepinonly: function (el) {
        $(el).inputmask({ mask: "9{1,10}", greedy: false });
    },
    formattopin: function (val) {
        return ("0000000000" + val).slice(-10);
    },
    makeinteger: function (el) {
        $(el).inputmask("integer", { allowMinus: true, allowPlus: true });
    },
    makeintegerplus: function (el) {
        $(el).inputmask("integer", { allowMinus: false, allowPlus: true });
    },
    makedecimal: function (el) {
        /* for decimal input only */
        $(el).inputmask("decimal", { allowMinus: true, allowPlus: true });
    },
    makedecimalplus: function (el) {
        /* for decimal input only */
        $(el).inputmask("decimal", { allowMinus: false, allowPlus: true });
    },
    makedate: function (el, format) {
        /* Make an input element into a datepicker */
       
            $(el).datepicker({
                format: format,
                autoclose: true
            });
       
    },
    makedateMY: function (el, format) {
        $(el).datepicker({
            format: format,
            viewMode: "months",
            minViewMode: "months",
            autoclose: true
        });

    },
    dateshow: function (el) {
        $(el).datepicker('show');
    },
    makeselect2: function (el) {
        $(el).select2({
            allowClear: true,
            closeOnSelect: false,
            width: 'copy',
            dropdownAutoWidth: false
        });
    },
    makeselect2noclear: function (el) {
        $(el).select2({
            allowClear: false,
            closeOnSelect: false,
            width: 'copy',
            dropdownAutoWidth: false
        });
    },
    makeselect2standard: function (el, result) {
        $(el).select2({
            allowClear: false,
            closeOnSelect: false,
            data: result,
            width: 'copy',
            dropdownAutoWidth: false
        });
    },
    makecurrency: function (num) {
        /* format the number into a currency type separated by comma. */
        var p = num.toFixed(2).split(".");
        return p[0].split("").reverse().reduce(function (acc, num, i, orig) {
            return num + (i && !(i % 3) ? "," : "") + acc;
        }, "") + "." + p[1];
    },
    bindselect2noclear: function (el, datalist, placeholder) {
        $(el).select2({
            placeholder: placeholder,
            allowClear: false,
            closeOnSelect: false,
            initSelection: function (element, callback) {
                var selection = _.find(datalist, function (metric) {
                    return metric.id === element.val();
                })
                callback(selection);
            },
            formatResult: CommonEmployeeFormatResult,
            query: function (options) {
                var pageSize = 200;
                var startIndex = (options.page - 1) * pageSize;
                var filteredData = datalist;
                var stripDiacritics = window.Select2.util.stripDiacritics;

                if (options.term && options.term.length > 0) {
                    if (!options.context) {
                        var term = stripDiacritics(options.term.toLowerCase());
                        options.context = datalist.filter(function (metric) {
                            if (!metric.stripped_text) {
                                metric.stripped_text = stripDiacritics(metric.text.toLowerCase());
                            }
                            return (metric.stripped_text.indexOf(term) !== -1 || metric.text.toString().toUpperCase().indexOf(term.toUpperCase()) != -1);
                        });
                    }
                    filteredData = options.context;
                }
                options.callback({
                    context: filteredData,
                    results: filteredData.slice(startIndex, startIndex + pageSize),
                    more: (startIndex + pageSize) < filteredData.length
                });
            }
        });
        $(el).prop('disabled', false);
    },
    bindselect2: function (el, datalist, ph) {
        $(el).select2({
            placeholder: ph,
            allowClear: true,
            closeOnSelect: false,
            initSelection: function (element, callback) {
                var selection = _.find(datalist, function (metric) {
                    return metric.id === element.val();
                })
                callback(selection);
            },
            formatResult: CommonEmployeeFormatResult,
            query: function (options) {
                var pageSize = 200;
                var startIndex = (options.page - 1) * pageSize;
                var filteredData = datalist;
                var stripDiacritics = window.Select2.util.stripDiacritics;

                if (options.term && options.term.length > 0) {
                    if (!options.context) {
                        var term = stripDiacritics(options.term.toLowerCase());
                        options.context = datalist.filter(function (metric) {
                            if (!metric.stripped_text) {
                                metric.stripped_text = stripDiacritics(metric.text.toLowerCase());
                            }
                            return (metric.stripped_text.indexOf(term) !== -1 || metric.text.toString().toUpperCase().indexOf(term.toUpperCase()) != -1);
                        });
                    }
                    filteredData = options.context;
                }
                options.callback({
                    context: filteredData,
                    results: filteredData.slice(startIndex, startIndex + pageSize),
                    more: (startIndex + pageSize) < filteredData.length
                });
            }
        });
        $(el).prop('disabled', false);
    },
    bindselect2multiple: function (el, datalist, ph) {
        $(el).select2({
            placeholder: ph,
            multiple: true,
            allowClear: true,
            closeOnSelect: false,
            initSelection: function (element, callback) {
                var selection = _.find(datalist, function (metric) {
                    return metric.id === element.val();
                })
                callback(selection);
            },
            formatResult: CommonEmployeeFormatResult,
            query: function (options) {
                var pageSize = 200;
                var startIndex = (options.page - 1) * pageSize;
                var filteredData = datalist;
                var stripDiacritics = window.Select2.util.stripDiacritics;

                if (options.term && options.term.length > 0) {
                    if (!options.context) {
                        var term = stripDiacritics(options.term.toLowerCase());
                        options.context = datalist.filter(function (metric) {
                            if (!metric.stripped_text) {
                                metric.stripped_text = stripDiacritics(metric.text.toLowerCase());
                            }
                            return (metric.stripped_text.indexOf(term) !== -1 || metric.text.toString().toUpperCase().indexOf(term.toUpperCase()) != -1);
                        });
                    }
                    filteredData = options.context;
                }
                options.callback({
                    context: filteredData,
                    results: filteredData.slice(startIndex, startIndex + pageSize),
                    more: (startIndex + pageSize) < filteredData.length
                });
            }
        });
        $(el).prop('disabled', false);
    },
    getcommonlist: function (el, xid, type, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 500); }
        $param = { id: xid, ctype: type };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_common_list", 'POST', $param,
            function () { _indicator.Show(el); },
            function (data) {
                comlist = [];
                $.each(data.CL, function (i, com) {
                    comlist.push({ id: com.Id, text: com.Name });
                });
                _common.bindselect2(el, comlist, ph);
                _indicator.Stop(el);
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 500);
        }
    },
    getcommonlistnoclear: function (el, xid, type, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 1000); }
        $param = { id: xid, ctype: type };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_common_list", 'POST', $param,
            function () { _indicator.Show(el); },
            function (data) {
                comlist = [];
                $.each(data.CL, function (i, com) {
                    comlist.push({ id: com.Id, text: com.Name });
                });
                _common.bindselect2noclear(el, comlist, ph);
                _indicator.Stop(el);
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 1000);
        }
    },
    getcommonlistALL: function (el, xid, type, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 500); }
        $param = { id: xid, ctype: type };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_common_list", 'POST', $param,
            function () { _indicator.Show(el); },
            function (data) {
                comlist = [];
                comlist.push({ id: 0, text: "ALL" });
                $.each(data.CL, function (i, com) {
                    comlist.push({ id: com.Id, text: com.Name });
                });
                _common.bindselect2(el, comlist, ph);
                _indicator.Stop(el);
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 500);
        }
    },
    getcommonlistnoclearALL: function (el, xid, type, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 500); }
        $param = { id: xid, ctype: type };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_common_list", 'POST', $param,
            function () { _indicator.Show(el); },
            function (data) {
                comlist = [];
                comlist.push({ id: 0, text: "ALL" });
                $.each(data.CL, function (i, com) {
                    comlist.push({ id: com.Id, text: com.Name });
                });
                _common.bindselect2noclear(el, comlist, ph);
                _indicator.Stop(el);
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 500);
        }
    },
    getcommonlistSelectMultiple: function (el, xid, type, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 500); }
        $param = { id: xid, ctype: type };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_common_list", 'POST', $param,
            function () { _indicator.Show(el); },
            function (data) {
                comlist = [];
                $.each(data.CL, function (i, com) {
                    comlist.push({ id: com.Id, text: com.Name });
                });
                _common.bindselect2multiple(el, comlist, ph);
                _indicator.Stop(el);
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 500);
        }
    },
    bindselect2noclear2: function (el, datalist, placeholder) {
        $(el).select2({
            placeholder: placeholder,
            allowClear: false,
            closeOnSelect: false,
            initSelection: function (element, callback) {
                var selection = _.find(datalist, function (metric) {
                    return metric.id === element.val();
                })
                callback(selection);
            },
            formatResult: CommonEmployeeFormatResult,
            query: function (options) {
                var pageSize = 200;
                var startIndex = (options.page - 1) * pageSize;
                var filteredData = datalist;
                var stripDiacritics = window.Select2.util.stripDiacritics;

                if (options.term && options.term.length > 0) {
                    if (!options.context) {
                        var term = stripDiacritics(options.term.toLowerCase());
                        options.context = datalist.filter(function (metric) {
                            if (!metric.stripped_text) {
                                metric.stripped_text = stripDiacritics(metric.text.toLowerCase());
                            }
                            return (metric.stripped_text.indexOf(term) !== -1 || metric.text.toString().toUpperCase().indexOf(term.toUpperCase()) != -1);
                        });
                    }
                    filteredData = options.context;
                }
                options.callback({
                    context: filteredData,
                    results: filteredData.slice(startIndex, startIndex + pageSize),
                    more: (startIndex + pageSize) < filteredData.length
                });
            }
        });
        $(el).prop('disabled', false);
    },
    /*New Select2 Bind*/
    getcommonlistNew: function (el, xid, type, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 500); }
        $param = { id: xid, ctype: type };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_common_list", 'POST', $param,
            function () { _indicator.Show(el); },
            function (data) {
                comlist = [];
                $.each(data.CL, function (i, com) {
                    comlist.push({ id: com.Id, text: com.Name });
                });
                _common.bindselect2New(el, comlist, ph);
                _indicator.Stop(el);
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 500);
        }
    },
    bindselect2New: function (el, datalist, ph) {
        $(el).select2({
            placeholder: ph,
            allowClear: true,
            closeOnSelect: false,
            initSelection: function (element, callback) {
                var selection = _.find(datalist, function (metric) {
                    return metric.id === element.val();
                })
                callback(selection);
            },
            formatResult: CommonEmployeeFormatResult
            //,
            //query: function (options) {
            //    var pageSize = 200;
            //    var startIndex = (options.page - 1) * pageSize;
            //    var filteredData = datalist;
            //    var stripDiacritics = window.Select2.util.stripDiacritics;

            //    if (options.term && options.term.length > 0) {
            //        if (!options.context) {
            //            var term = stripDiacritics(options.term.toLowerCase());
            //            options.context = datalist.filter(function (metric) {
            //                if (!metric.stripped_text) {
            //                    metric.stripped_text = stripDiacritics(metric.text.toLowerCase());
            //                }
            //                return (metric.stripped_text.indexOf(term) !== -1 || metric.text.toString().toUpperCase().indexOf(term.toUpperCase()) != -1);
            //            });
            //        }
            //        filteredData = options.context;
            //    }
            //    options.callback({
            //        context: filteredData,
            //        results: filteredData.slice(startIndex, startIndex + pageSize),
            //        more: (startIndex + pageSize) < filteredData.length
            //    });
            //}
        });
        $(el).prop('disabled', false);
    },
    getcommonlistSS: function (el, xid, type, ph, callb1, callb2) {
        var $xid = xid;
        var $type = type;
        $(el).select2({
            placeholder: ph,
            allowClear: true,
            minimumInputLength: 2,
            ajax: {
                url: $('#url').data('arcommon') + "/get_common_list_server",
                dataType: 'json',
                cache: false,
                type: 'POST',
                quietMillis: 250,
                data: function (term) {
                    return {
                        id: $xid,
                        ctype: $type,
                        terms: term // search term
                    };
                },
                results: function (data) { //
                    return { results: data.CLS };
                },
            },
            //dropdownAutoWidth: true,
            //initSelection: function (element, callback) {
            //    var id = $(element).val();
            //    if (id !== "") {
            //        $.ajax(hissystem.appsserver() + hissystem.appsname() + "FrontOffice/Common/get_common_list_server" + id, {
            //            dataType: "json"
            //        }).done(function (data) { callback(data); });
            //    }
            //},
            formatResult: selectFormatResult,
            //formatSelection: repoFormatSelection,  // omitted for brevity, see the source of this page
            //dropdownCssClass: "bigdrop", // apply css that makes the dropdown taller
            //escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
        }).on("change", function () {

            callb2($(this).select2('data').id);

        });
        $(el).prop('disabled', false);
    },
    postselected: function (elem, xxid, ttype, callb1, callb2) {
        if (callb1 != null) {
            setTimeout(callb1, 1 * 500);
        }
        $paramsel = { id: xxid, ctype: ttype };
        ajaxwrapper.stdna("FrontOffice/Common/get_common_list_selected", 'POST', $paramsel,
            function () { },
            function (datasels) {
                //var $selparam = {};
                //    $selparam = [];
                //sel = 0;

                $.each(datasels.CLSEL, function (seli, ressx) {
                    // if (sel == 0) {
                    $(elem).select2('data', { id: ressx.Id, text: ressx.Name });
                    //sel = 1;
                    // }
                });
                //alert($selparam);

                //alert(data.CLSEL);
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 750);
        }
    },

    addineditor: function ($table, $select2data) {
        $.editable.addInputType('decimaleditor', {
            element: function (settings, original) {
                var input = $('<input type="text" class="form-control" autocomplete="off"/>');
                $(this).append(input);
                return (input);
            },
            plugin: function (settings, original) {
                $(this).find('input')
                    .inputmask("decimal", {
                        radixPoint: ".", autoGroup: true, //groupSeparator: ",", groupSize: 3,
                        digits: 2,
                        allowMinus: false, allowPlus: true
                    })
                    .keydown(function (e) {
                        var cellIndex = $table.cell(original).index();
                        if (e.keyCode == 40) { //down                    
                            if (cellIndex.row < $table.rows().data().length - 1) {
                                $($table.cell(cellIndex.row + 1, cellIndex.column).node()).click();
                            }
                        }
                        else if (e.keyCode == 38) { //up                                        
                            if (cellIndex.row > 0) {
                                $($table.cell(cellIndex.row - 1, cellIndex.column).node()).click();
                            }
                        }
                        else if (e.keyCode == 39) { //right
                            if ($(this).caret().start == $(this).val().length) {
                                $($table.cell(cellIndex.row, cellIndex.column + 1).node()).click();
                            }
                        }
                        else if (e.keyCode == 37) { //left
                            if ($(this).caret().end == 0) {
                                $($table.cell(cellIndex.row, cellIndex.column - 1).node()).click();
                            }
                        }
                        else if (e.keyCode == 13) { //enter
                            $('.numbereditor', $table.rows().nodes()).find('form').submit();
                            $($table.cell(cellIndex.row, cellIndex.column + 1).node()).click();
                        }
                    });
            }
            ,
            submit: function (e) {
                rowIdx = $table.row($(this).closest('tr')).index();

                setTimeout(function () {
                    calc_Price(rowIdx);
                }, 1 * 150);

              //  alert($table.cell(rowIdx, 7).data());


            }
        });
        $.editable.addInputType('integereditor', {
            element: function (settings, original) {
                var input = $('<input type="text" autocomplete="off" class="form-control" />'); //min="1" max="99" step="1"
                $(this).append(input);
                return (input);
            },
            plugin: function (settings, original) {
                $(this).find('input')
                    .inputmask("integer", {
                        allowMinus: false, allowPlus: true //autoGroup: true, groupSeparator: ",", groupSize: 3

                    })
                    .keydown(function (e) {
                        var cellIndex = $table.cell(original).index();
                        if (e.keyCode == 40) { //down                    
                            if (cellIndex.row < $table.rows().data().length - 1) {
                                $($table.cell(cellIndex.row + 1, cellIndex.column).node()).click();
                            }
                        }
                        else if (e.keyCode == 38) { //up                                        
                            if (cellIndex.row > 0) {
                                $($table.cell(cellIndex.row - 1, cellIndex.column).node()).click();
                            }
                        }
                        else if (e.keyCode == 39) { //right
                            if ($(this).caret().start == $(this).val().length) {
                                $($table.cell(cellIndex.row, cellIndex.column + 1).node()).click();
                            }
                        }
                        else if (e.keyCode == 37) { //left
                            if ($(this).caret().end == 0) {
                                $($table.cell(cellIndex.row, cellIndex.column - 1).node()).click();
                            }
                        }
                        else if (e.keyCode == 13) { //enter
                            $('.numbereditor2', $table.rows().nodes()).find('form').submit();
                            $($table.cell(cellIndex.row, cellIndex.column + 1).node()).click();
                        }
                    });
            },
                submit: function (e) {
                    //rowIdx = $table.row($(this).closest('tr')).index();

                    // should not compute the amount due to the query in the SQL already
                    // considering the quantity and price.

                    //setTimeout(function () {
                    //    calc_Price(rowIdx);
                    //}, 1 * 150);

                    ////  alert($table.cell(rowIdx, 7).data());


                }
        });

        $.editable.addInputType('select2editor', {
            element: function (settings, original) {
                var input = $('<input id="arcanreason" type="text" class="form-control">');
                $(this).append(input);
                return (input);
            },
            plugin: function (settings, original) {
                var select2 = $(this).find('#arcanreason').select2({
                    placeholder: 'Select Cancellation Reason...',
                    allowClear: false,
                    closeOnSelect: false,
                    initSelection: function (element, callback) {
                        var selection = _.find($select2data, function (metric) {
                            return metric.id === element.val();
                        })
                        callback(selection);
                    },
                    formatResult: CommonEmployeeFormatResult,
                    query: function (options) {
                        var pageSize = 200;
                        var startIndex = (options.page - 1) * pageSize;
                        var filteredData = $select2data;
                        var stripDiacritics = window.Select2.util.stripDiacritics;

                        if (options.term && options.term.length > 0) {
                            if (!options.context) {
                                var term = stripDiacritics(options.term.toLowerCase());
                                options.context = $select2data.filter(function (metric) {
                                    if (!metric.stripped_text) {
                                        metric.stripped_text = stripDiacritics(metric.text.toLowerCase());
                                    }
                                    return (metric.stripped_text.indexOf(term) !== -1 || metric.text.toString().toUpperCase().indexOf(term.toUpperCase()) != -1);
                                });
                            }
                            filteredData = options.context;
                        }
                        options.callback({
                            context: filteredData,
                            results: filteredData.slice(startIndex, startIndex + pageSize),
                            more: (startIndex + pageSize) < filteredData.length
                        });
                    }
                }).on("select2-blur", function () {
                    $("#arcanreason").closest('td').get(0).reset();
                }).on('select2-close', function () {
                    if (Select2IsClicked) { $("#arcanreason").closest('form').submit(); }
                    else {
                        $("#arcanreason").closest('td').get(0).reset();
                    }
                    Select2IsClicked = false;
                }).on("select2-focus", function (e) {
                    rowIndex = $table.row($(this).closest('tr')).index();
                    var a = $(this).closest('tr').find('#arcanreason').val();
                    var id = $();
                    $("#arcanreason").select2("data", { id: a, text: a });
                }).data('select2');
                select2.onSelect = (function (fn) {
                    return function (data, options) {
                        var target;
                        if (options != null) {
                            target = $(options.target);
                        }
                        Select2IsClicked = true;
                        return fn.apply(this, arguments);
                    }
                })(select2.onSelect);
            },
            submit: function (settings, original) {
                if ($("#arcanreason", this).select2('val') != null && $("#arcanreason", this).select2('val') != '') {
                    $("input", this).val($("#arcanreason", this).select2("data").text);
                    //var d = new Date();
                    //var $SD = d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();
                    ///*Validity Date*/
                    //var $AppD;
                    //if ($('#hdn_appr_days').val() == 0) {
                    //    $AppD = 100;
                    //} else {
                    //    $AppD = $('#hdn_appr_days').val();

                    //}
                    //var $VD = moment($SD).add('days', $AppD);
                    rowIdx = $table.row($(this).closest('tr')).index();
                    //if ($("#sel2billno", this).select2('data').id == 1) {
                    //    $table.cell(rowIdx, 5).data(moment($VD).format('DD-MMM-YYYY'));
                    //    $table.cell(rowIdx, 10).data(0);
                    //    $table.cell(rowIdx, 6).data('');
                    //} else {
                    //    $table.cell(rowIdx, 5).data('');
                    //    $table.cell(rowIdx, 10).data(0);
                    //    $table.cell(rowIdx, 6).data('');
                    //}
                    //if ($("#sel2billno", this).select2('data').id == 2) {
                    //    $table.cell(rowIdx, 6).data('SECOND OPINION');
                    //    $table.cell(rowIdx, 10).data(1);
                    //}
                    $table.cell(rowIdx, 13).data($("#arcanreason", this).select2('data').id);
                    //var id = $("#sel2billno", this).select2('data').id;
                    $table.columns.adjust().draw();
                }
            }
        });

        $.editable.addInputType('cusdatepicker', {
            element: function (settings, original) {
                var input = $('<input id="cusdatepick" type="text" class="form-control date" data-date-format="DD-MMM-YYYY" readonly="true"> ');
                $(this).append(input);
                return (input);
            },
            plugin: function (settings, original) {
                /*Date Picker*/
                //$(this).find('#cusdatepick').datepicker({
                //    format: "dd-M-yyyy"
                //    //,
                //    //autoclose: true
                //});

                //$(this).find('#cusdatepick').datepicker({
                //    format: "dd-M-yyyy"
                //    , autoclose: true
                //});

                $(this).find('#cusdatepick').datetimepicker({
                    pickTime: false
                }).bind('change', function (e) {
                    $(this).submit();
                });
            }
        });
            //,
            //submit: function (settings, original) {
                //if ($("#sel2billno", this).select2('val') != null && $("#sel2billno", this).select2('val') != '') {
                //    $("input", this).val($("#sel2billno", this).select2("data").text);
                //    var d = new Date();
                //    var $SD = d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();
                //    /*Validity Date*/
                //    var $AppD;
                //    if ($('#hdn_appr_days').val() == 0) {
                //        $AppD = 100;
                //    } else {
                //        $AppD = $('#hdn_appr_days').val();

                //    }
                //    var $VD = moment($SD).add('days', $AppD);
                //    rowIdx = $table.row($(this).closest('tr')).index();
                //    if ($("#sel2billno", this).select2('data').id == 1) {
                //        $table.cell(rowIdx, 5).data(moment($VD).format('DD-MMM-YYYY'));
                //        $table.cell(rowIdx, 10).data(0);
                //        $table.cell(rowIdx, 6).data('');
                //    } else {
                //        $table.cell(rowIdx, 5).data('');
                //        $table.cell(rowIdx, 10).data(0);
                //        $table.cell(rowIdx, 6).data('');
                //    }
                //    if ($("#sel2billno", this).select2('data').id == 2) {
                //        $table.cell(rowIdx, 6).data('SECOND OPINION');
                //        $table.cell(rowIdx, 10).data(1);
                //    }
                //    $table.cell(rowIdx, 9).data($("#sel2billno", this).select2('data').id);
                //    //var id = $("#sel2billno", this).select2('data').id;

                //}
           // }
    },

    addineditor2: function ($table, $select2data) {
        $.editable.addInputType('select2editor2', {
            element: function (settings, original) {
                var input = $('<input id="sel2unit" type="text" class="form-control">');
                $(this).append(input);
                return (input);
            },
            plugin: function (settings, original) {
                var select2 = $(this).find('#sel2unit').select2({
                    placeholder: 'Select Bill No...',
                    allowClear: false,
                    closeOnSelect: false,
                    initSelection: function (element, callback) {
                        var selection = _.find($select2data, function (metric) {
                            return metric.id === element.val();
                        })
                        callback(selection);
                    },
                    formatResult: CommonEmployeeFormatResult,
                    query: function (options) {
                        var pageSize = 200;
                        var startIndex = (options.page - 1) * pageSize;
                        var filteredData = $select2data;
                        var stripDiacritics = window.Select2.util.stripDiacritics;

                        if (options.term && options.term.length > 0) {
                            if (!options.context) {
                                var term = stripDiacritics(options.term.toLowerCase());
                                options.context = $select2data.filter(function (metric) {
                                    if (!metric.stripped_text) {
                                        metric.stripped_text = stripDiacritics(metric.text.toLowerCase());
                                    }
                                    return (metric.stripped_text.indexOf(term) !== -1 || metric.text.toString().toUpperCase().indexOf(term.toUpperCase()) != -1);
                                });
                            }
                            filteredData = options.context;
                        }
                        options.callback({
                            context: filteredData,
                            results: filteredData.slice(startIndex, startIndex + pageSize),
                            more: (startIndex + pageSize) < filteredData.length
                        });
                    }
                }).on("select2-blur", function () {
                    $("#sel2unit").closest('td').get(0).reset();
                }).on('select2-close', function () {
                    if (Select2IsClicked) { $("#sel2unit").closest('form').submit(); }
                    else {
                        $("#sel2unit").closest('td').get(0).reset();
                    }
                    Select2IsClicked = false;
                }).on("select2-focus", function (e) {
                    rowIndex = $table.row($(this).closest('tr')).index();
                    var a = $(this).closest('tr').find('#sel2unit').val();
                    var id = $();
                    $("#sel2unit").select2("data", { id: a, text: a });
                }).data('select2');
                select2.onSelect = (function (fn) {
                    return function (data, options) {
                        var target;
                        if (options != null) {
                            target = $(options.target);
                        }
                        Select2IsClicked = true;
                        return fn.apply(this, arguments);
                    }
                })(select2.onSelect);
            },
            submit: function (settings, original) {
                if ($("#sel2unit", this).select2('val') != null && $("#sel2unit", this).select2('val') != '') {

                    rowIdx = $table.row($(this).closest('tr')).index();


                    add_Service_Item_PH($("#sel2unit", this).select2('data').id, rowIdx);
                    $("input", this).val($("#sel2unit", this).select2("data").text);
                    //if ($("#sel2unit", this).select2('data').billdeptid === $table.cell(rowIdx, 16).data()) {
                        //
                        //$table.cell(rowIdx, 21).data($("#sel2unit", this).select2('data').id);
                        //$table.columns.adjust().draw();
                    //} else {

                    //    $table.cell(rowIdx, 15).data(0);
                        
                    //    ardialog.Pop(4, "CAM", "Only Items with <b>the same Department</b> can be billed under one bill.", "OK", function () { $table.cell(rowIdx, 2).data(""); });
                    //    $table.columns.adjust().draw();
                    //}
                }
            }
        });
    },

    initineditor: function ($table) {
        $('.deceditor', $table.rows().nodes()).editable(function (sVal, settings) {
            var cell = $table.cell($(this).closest('td')).index();
            $table.cell(cell.row, cell.column).data(sVal);
            return sVal;
        },
        {
            "type": 'decimaleditor', "style": 'display: inline;', "onblur": 'submit', "onreset": function () { },
            "event": 'click', "submit": '', "cancel": '', "placeholder": '', "cssclass": "coleditor"
        });
        $('.inteditor', $table.rows().nodes()).editable(function (sVal, settings) {
            var cell = $table.cell($(this).closest('td')).index();
            $table.cell(cell.row, cell.column).data(sVal);
            return sVal;
        },
        {
            "type": 'integereditor', "style": 'display: inline;', "onblur": 'submit', "onreset": function () { },
            "event": 'click', "submit": '', "cancel": '', "placeholder": '', "cssclass": "coleditor"
        });

        //$('.s2editor', $table.rows().nodes()).editable(function (sVal, settings) {

        //    return sVal;
        //    },
        //{
        //    "type": 'select2editor', "style": 'display: inline;',
        //    "onblur": 'submit',
        //    "onreset": function () { },
        //    "event": 'click', "submit": '', "cancel": '', "placeholder": '', "cssclass": "coleditor"
        //});

        $('.s2editor2', $table.rows().nodes()).editable(function (sVal, settings) {

            return sVal;
        },
        {
            "type": 'select2editor2', "style": 'display: inline;',
            "onblur": 'submit',
            "onreset": function () { },
            "event": 'click', "submit": '', "cancel": '', "placeholder": '', "cssclass": "coleditor"
        });

        $('.cusdatepick', $table.rows().nodes()).editable(function (sVal, settings) {

            return sVal;
        },
        {
            "type": 'cusdatepicker', "style": 'display: inline;',
            "onblur": 'submit',
            "onreset": function () { },
            "event": 'click', "submit": '', "cancel": '', "placeholder": '', "cssclass": "coleditor"
        });

    },

    checkifexistDT: function ($table, $compval, $column) {
        cDTres = false;
        $.each($table.rows().data(), function (i, row) {
            if ($.trim($table.cell(i, $column).data()) == $compval) {
                cDTres = true;
                return false;
            }
        });
        return cDTres;
    },
    gettotalgridprice: function ($table, $column) {
        _totalp = 0;
        $.each($table.rows().data(), function (i, row) {
            _totalp = _totalp + parseFloat($.trim($table.cell(i, $column).data()));

        });

        return _totalp.toFixed(2);
    },
    /*Added for AR OP Billing Dec 12, 2015*/
    getPTComPTName: function ($pin, $callb) {
        
        $param = { pin: $pin };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_com_PTName", 'POST', $param,
           function (){},
           function (data) {
               $callb(data);
           },
           function (err) {
               ardialog.Pop(4, "Error", err, "OK", function () { });
           });
    },
    getcomlistnoclearinvoice: function (el, $reqtype, $invtype, $fdate, $tdate, $catid, $comid, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 1000); }
        $param = {
            reqtype: $reqtype,
            invtype: $invtype,
            fdate: $fdate,
            tdate: $tdate,
            catid: $catid,
            comid: $comid
        };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_inv_accountpin", 'POST', $param,
            function () { _indicator.Body(); },
            function (data) {
                if (data.CL.length == 0) {
                    $('#btn_generate').prop('disabled', true);
                    $('#btn_printpreview').prop('disabled', true);
                    ardialog.Pop(4, "Sorry", "No Results found!", "OK", function () { _indicator.Stop(); });
                } else {

                    //if ($invtype == 1) {
                    //    $('#btn_generate').prop('disabled', false);
                    //    $('#btn_printpreview').prop('disabled', true);
                    //} else {
                    //    $('#btn_generate').prop('disabled', true);
                    //    $('#btn_printpreview').prop('disabled', false);
                    //}

                    comlist = [];
                    if ($catid != 0) {
                        comlist.push({ id: 0, text: "All" });
                    }
                    $.each(data.CL, function (i, com) {
                        comlist.push({ id: com.Id, text: com.Name });
                    });
                    _common.bindselect2noclear(el, comlist, ph);
                    _indicator.Stop();
                }
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 1000);
        }
    },
    getcomlistnoclearinvoiceUBF: function (el, $reqtype, $invtype, $fdate, $tdate, $catid, $comid, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 1000); }
        $param = {
            reqtype: $reqtype,
            invtype: $invtype,
            fdate: $fdate,
            tdate: $tdate,
            catid: $catid,
            comid: $comid
        };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_inv_accountpin_ubf", 'POST', $param,
            function () { _indicator.Body(); },
            function (data) {
                if (data.CL.length == 0) {
                    $('#btn_generate').prop('disabled', true);
                    $('#btn_printpreview').prop('disabled', true);
                    ardialog.Pop(4, "Sorry", "No Results found!", "OK", function () { _indicator.Stop(); });
                } else {

                    //if ($invtype == 1) {
                    //    $('#btn_generate').prop('disabled', false);
                    //    $('#btn_printpreview').prop('disabled', true);
                    //} else {
                    //    $('#btn_generate').prop('disabled', true);
                    //    $('#btn_printpreview').prop('disabled', false);
                    //}

                    comlist = [];
                    if ($catid != 0) {
                        comlist.push({ id: 0, text: "All" });
                    }
                    $.each(data.CL, function (i, com) {
                        comlist.push({ id: com.Id, text: com.Name });
                    });
                    _common.bindselect2noclear(el, comlist, ph);
                    _indicator.Stop();
                }
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { _indicator.Stop(); });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 1000);
        }
    }
}

var _commonExt = {
    getclcompanynoclearALL: function (el, $catid, $btype, $fdate, $tdate, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 1000); }
        $param = {
            catid: $catid,
            btype: $btype,
            fdate: $fdate,
            tdate: $tdate
        };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_cl_company", 'POST', $param,
            function () { },
            function (data) {
                comlist = [];
                if (data.CL.length > 0) {
                    comlist.push({ id: 0, text: "ALL" });
                }
                $.each(data.CL, function (i, com) {
                    comlist.push({ id: com.Id, text: com.Name });
                });
                _common.bindselect2noclear(el, comlist, ph);
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 1000);
        }
    },
    getclrefnonoclear: function (el, $catid, $comid, $fdate, $tdate, ph, callb1, callb2) {
        if (callb1 != null) { setTimeout(callb1, 1 * 1000); }
        $param = {
            catid: $catid,
            comid: $comid,
            fdate: $fdate,
            tdate: $tdate
        };
        ajaxwrapper.stdna($('#url').data('arcommon') + "/get_cl_refno", 'POST', $param,
            function () { },
            function (data) {
                comlist = [];
                //if (data.CL.length > 0) {
                //    comlist.push({ id: 0, text: "ALL" });
                //}
                $.each(data.CL, function (i, com) {
                    comlist.push({ id: com.Id, text: com.Name });
                });
                _common.bindselect2noclear(el, comlist, ph);
            },
            function (err) {
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 1000);
        }
    },
    getadmitdtgeninv: function (el, $pin, ph, callb1, callb2) {
        
        if (callb1 != null) { setTimeout(callb1, 1 * 1000); }
        $param = {
            pin: $pin
        };
        ajaxwrapper.stdna($('#url').data('getadmitdate'), 'POST', $param,
            function () {
                _indicator.Body();
            },
            function (data) {
                comlist = [];
                //if (data.CL.length > 0) {
                //    comlist.push({ id: 0, text: "ALL" });
                //}
                $.each(data.CL, function (i, com) {
                    comlist.push({ id: com.BillNo, text: com.AdmitDateTime });
                });
                _common.bindselect2noclear(el, comlist, ph);
                _indicator.Stop();
            },
            function (err) {
                _indicator.Stop();
                ardialog.Pop(4, "Error", err, "OK", function () { });
            });
        if (callb2 != null) {
            setTimeout(callb2, 1 * 1000);
        }
    }
}


