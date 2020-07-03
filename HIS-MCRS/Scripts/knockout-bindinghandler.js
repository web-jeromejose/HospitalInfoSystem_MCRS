
//Error Logger
ko.bindingHandlers.logger = {
    update: function (element, valueAccessor, allBindings) {
        //store a counter with this element
        var count = ko.utils.domData.get(element, "_ko_logger") || 0,
            data = ko.toJS(valueAccessor() || allBindings());

        ko.utils.domData.set(element, "_ko_logger", ++count);

        if (window.console && console.log) {
            console.log(count, element, data);
        }
    }
};
//keypress enter
ko.bindingHandlers.executeOnEnter = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var callback = valueAccessor();
        $(element).keypress(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13) {
                callback.call(viewModel);
                return false;
            }
            return true;
        });
    }
};
//Date picker
ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datepickerOptions || {};
        $(element).datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            try {
                observable($(element).datepicker("getDate"));//****
            }
            catch (ex) { }
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            current = $(element).datepicker("getDate");

        if (value - current !== 0) {
            $(element).datepicker("setDate", value);
        }
    }
};

//Datetime picker
ko.bindingHandlers.datetimepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datetimepickerOptions || {};
        $(element).datetimepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            try {
                observable($(element).datetimepicker("getDate"));//****
            }
            catch (ex) { }
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datetimepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            current = $(element).datetimepicker("getDate");

        if (value - current !== 0) {
            $(element).datetimepicker("setDate", value);
        }
    }
};

//Datetime picker
ko.bindingHandlers.glyphfor = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var targetId = valueAccessor();
        if (targetId) {
            $(element).click(function () {
                 $("#" + targetId).datepicker('show');
            });         
        }
    }
};

ko.bindingHandlers.checkedInArray = {
    init: function (element, valueAccessor) {
        ko.utils.registerEventHandler(element, "click", function() {
            var options = ko.utils.unwrapObservable(valueAccessor()),
                array = options.array, // don't unwrap array because we want to update the observable array itself
                value = ko.utils.unwrapObservable(options.value),
                checked = element.checked;
            ko.utils.addOrRemoveItem(array, value, checked);
        });
    },
    update: function (element, valueAccessor) {
        var options = ko.utils.unwrapObservable(valueAccessor()),
            array = ko.utils.unwrapObservable(options.array),
            value = ko.utils.unwrapObservable(options.value);

        element.checked = ko.utils.arrayIndexOf(array, value) >= 0;
    }
};



// Modal handler
ko.bindingHandlers.modal = {
    init: function (element, valueAccessor) {
        $(element).modal({
            show: false
        });

        var value = valueAccessor();
        if (typeof value === 'function') {
            $(element).on('hide.bs.modal', function () {
                value(false);
            });
        }
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).modal("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        if (ko.utils.unwrapObservable(value)) {
            $(element).modal('show');
        } else {
            $(element).modal('hide');
        }
    }
};



ko.bindingHandlers.dataTablesForEach = {
    page: 0,
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var options = ko.unwrap(valueAccessor());
        ko.unwrap(options.data);
        if (options.dataTableOptions.paging) {
            valueAccessor().data.subscribe(function (changes) {
                var table = $(element).closest('table').DataTable();
                ko.bindingHandlers.dataTablesForEach.page = table.page();
                table.destroy();
            }, null, 'arrayChange');
        }
        var nodes = Array.prototype.slice.call(element.childNodes, 0);
        ko.utils.arrayForEach(nodes, function (node) {
            if (node && node.nodeType !== 1) {
                node.parentNode.removeChild(node);
            }
        });
        return ko.bindingHandlers.foreach.init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var options = ko.unwrap(valueAccessor()),
            key = 'DataTablesForEach_Initialized';
        ko.unwrap(options.data);
        var table;
        if (!options.dataTableOptions.paging) {
            table = $(element).closest('table').DataTable();
            table.destroy();
        }
        ko.bindingHandlers.foreach.update(element, valueAccessor, allBindings, viewModel, bindingContext);
        table = $(element).closest('table').DataTable(options.dataTableOptions);
        if (options.dataTableOptions.paging) {
            if (table.page.info().pages - ko.bindingHandlers.dataTablesForEach.page == 0)
                table.page(--ko.bindingHandlers.dataTablesForEach.page).draw(false);
            else
                table.page(ko.bindingHandlers.dataTablesForEach.page).draw(false);
        }
        if (!ko.utils.domData.get(element, key) && (options.data || options.length))
            ko.utils.domData.set(element, key, true);
        return { controlsDescendantBindings: true };
    }
};

//use for 1000+ checkboxes
ko.bindingHandlers.checkedInArray = {
    init: function (element, valueAccessor) {
        ko.utils.registerEventHandler(element, "click", function () {
            var options = ko.utils.unwrapObservable(valueAccessor()),
                array = options.array, // don't unwrap array because we want to update the observable array itself
                value = ko.utils.unwrapObservable(options.value),
                checked = element.checked;
            ko.utils.addOrRemoveItem(array, value, checked);
        });
    },
    update: function (element, valueAccessor) {
        var options = ko.utils.unwrapObservable(valueAccessor()),
            array = ko.utils.unwrapObservable(options.array),
            value = ko.utils.unwrapObservable(options.value);

        element.checked = ko.utils.arrayIndexOf(array, value) >= 0;
    }
};

//entery key event use with valueUpdate: 'afterkeydown'
ko.bindingHandlers.enterkey = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var callback = valueAccessor();
        $(element).keypress(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13) {
                callback.call(viewModel);
                return false;
            }
            return true;
        });
    }
};



//ko.bindingHandlers.dataTablesForEach = {
//    page: 0,
//    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
//        var options = ko.unwrap(valueAccessor());
//        ko.unwrap(options.data);
//        if (options.dataTableOptions.paging) {
//            valueAccessor().data.subscribe(function (changes) {
//                var table = $(element).closest('table').DataTable();
//                ko.bindingHandlers.dataTablesForEach.page = table.page();
//                table.destroy();
//            }, null, 'arrayChange');
//        }
//        var nodes = Array.prototype.slice.call(element.childNodes, 0);
//        ko.utils.arrayForEach(nodes, function (node) {
//            if (node && node.nodeType !== 1) {
//                node.parentNode.removeChild(node);
//            }
//        });
//        return ko.bindingHandlers.foreach.init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
//    },
//    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
//        var options = ko.unwrap(valueAccessor()),
//            key = 'DataTablesForEach_Initialized';
//        ko.unwrap(options.data);
//        var table;
//        if (!options.dataTableOptions.paging) {
//            table = $(element).closest('table').DataTable();
//            table.destroy();
//        }
//        ko.bindingHandlers.foreach.update(element, valueAccessor, allBindings, viewModel, bindingContext);
//        table = $(element).closest('table').DataTable(options.dataTableOptions);
//        if (options.dataTableOptions.paging) {
//            if (table.page.info().pages - ko.bindingHandlers.dataTablesForEach.page == 0)
//                table.page(--ko.bindingHandlers.dataTablesForEach.page).draw(false);
//            else
//                table.page(ko.bindingHandlers.dataTablesForEach.page).draw(false);
//        }
//        if (!ko.utils.domData.get(element, key) && (options.data || options.length))
//            ko.utils.domData.set(element, key, true);
//        return { controlsDescendantBindings: true };
//    }
//};