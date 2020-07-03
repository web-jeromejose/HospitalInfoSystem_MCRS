

Number.prototype.between = function (a, b) {
    var min = Math.min.apply(Math, [a, b]),
        max = Math.max.apply(Math, [a, b]);
    return this > min && this < max;
};

Number.prototype.inBetween = function (a, b) {
    var min = Math.min.apply(Math, [a, b]),
        max = Math.max.apply(Math, [a, b]);
    return this >= min && this <= max;
};




function ViewModel(model) {
    self = this;
    self.inputUrlActions = null;
   
    self.StartDate           = ko.observable(new Date(moment(model.StartDate)));
    self.EndDate             = ko.observable(new Date(moment(model.EndDate)));
    self.StringCompanyFilter = ko.observable('');

    self.SelectedCategoryId  = ko.observable();
    self.SelectedDoctorId    = ko.observable(0);
    self.SelectedCompanies   = ko.observableArray([]);
    self.SelectAllCompany    = ko.observable(false);
    self.SelectedUCAFRecords = ko.observableArray([]);
    self.SelectAllUCAFRecord = ko.observable(false);
    self.UCAFSelectedIds     = ko.observableArray([]);

    self.Doctors             = ko.observableArray(model.Doctors);
    self.Categories          = ko.observableArray(model.Categories);
    self.Companies           = ko.observableArray([]);
    self.ARUCAFRecords       = ko.observableArray(model.ARUCAFRecords);
    self.UCAFRecordPaging    = ko.observable(new Paging(0,1,0,0,self,'default','asc'));
    self.UCAFVisitIdList     = ko.observableArray([]);
    self.UCAFTableBodyHeight = ko.computed(function () {
        if (self.ARUCAFRecords().length > 0) {self.UCAFRecordPaging
            return "350px";
        } else {
            return "80px"
        }

    }, this);

    // other ui acions
    self.Clear               = function () {
        self.ARUCAFRecords([]);
        self.UCAFVisitIdList([]);
        self.Companies([]);
        self.SelectedCategoryId(self.Categories()[0].Id);
        self.SelectedDoctorId(0);
        self.SelectAllCompany(false);
        self.SelectedUCAFRecords([]);
        self.UCAFRecordPaging(new Paging(0, 1, 0, 0, self, 'default', 'asc'));
        self.StringCompanyFilter('');
    }
   
    // server calls
    self.getCompaniesWithOPBillDetail = function () {
        self.Companies([]);
        self.StringCompanyFilter('');
        self.SelectedCompanies([]);
        url = self.inputUrlActions.data("getcomanieswithopbilldetail");
        startdate = moment(self.StartDate()).format("MM/DD/YYYY");
        enddate   = moment(self.EndDate()).format("MM/DD/YYYY");
        id = self.SelectedCategoryId() || 0;

        param = { startDate: startdate, endDate: enddate, id: id };

        ajaxWrapper.GetWithLoading(url, param, $("#companyWrapper"), function (data, e) {

            for(i = 0; i < data.length;i++) {
                self.Companies.push(new Company(data[i].Id, data[i].Name, data[i].Code, self));
            };

        });
    }

    self.getARUCAFRecord = function () {
        self.SelectedUCAFRecords([]);
        self.ARUCAFRecords([]);
        self.UCAFVisitIdList([]);
        url = self.inputUrlActions.data("getucafrecord");

        param = {
                    categoryId:self.SelectedCategoryId() ,
                    doctorId: self.SelectedDoctorId(),
                    startDate:moment(self.StartDate()).format("MM/DD/YYYY"),
                    endDate:moment(self.EndDate()).format("MM/DD/YYYY"),
                    selectedCompaniesJson:ko.toJSON(self.SelectedCompanies()), 
                    page: self.UCAFRecordPaging().CurrentPageNumber(),
                    sortBy: self.UCAFRecordPaging().SortBy(),
                    sortMode: self.UCAFRecordPaging().SortMode()
                }

        ajaxWrapper.PostWithLoading(url, param, $("#recordWrapper"), function (data, e) {
            ko.utils.arrayPushAll(self.ARUCAFRecords, data.records);
            self.UCAFRecordPaging(new Paging(data.totalPage, data.currentPage, data.pageSize, data.recordCount, self, self.UCAFRecordPaging().SortBy(), self.UCAFRecordPaging().SortMode()));
            ko.utils.arrayPushAll(self.UCAFVisitIdList, data.visitIdList);
            document.getElementById("Print").scrollIntoView();
        });
        
    };

    self.PrintPreviewBySelectedIds = function () {
        var dummy = new iframeform(self.inputUrlActions.data('ucafbatchprinting'));
        dummy.addParameter('ListOfSelectedVisitIds', ko.toJSON(self.UCAFSelectedIds()));
        self.Dialog(new Dialog("REMINDER", "Be sure to select  Lazer jet printer before printing", "alert-danger", true, dummy));
    };

    self.PrintPreviewByPage = function () {

        var dummy = new iframeform(self.inputUrlActions.data('ucafbatchprinting'));
        dummy.addParameter('ListOfSelectedVisitIds', ko.toJSON(self.PrintByPageModal().SelectedVisitIds()));
        
        self.Dialog(new Dialog("REMINDER", "Be sure to select  Lazer jet printer before printing", "alert-danger", true, dummy));
     
    };

    //change events
    self.SelectedCategoryId.subscribe(function (value) {

        self.getCompaniesWithOPBillDetail();
        self.SelectAllCompany(false);
        self.SelectedCompanies([]);
    });

    self.StartDate.subscribe(function (value) {
        self.SelectAllCompany(false);
        self.getCompaniesWithOPBillDetail();  
    });

    self.EndDate.subscribe(function (value) {
        self.SelectAllCompany(false);
        self.getCompaniesWithOPBillDetail();
       
    })

    self.SelectAllCompany.subscribe(function (checked) {
           if (checked) {
               ko.utils.arrayPushAll(self.SelectedCompanies, self.Companies());
            } else {
               self.SelectedCompanies([]);
            }
    });

    self.SelectAllUCAFRecord.subscribe(function (checked) {
        if (checked) {
            ko.utils.arrayPushAll(self.UCAFSelectedIds, self.UCAFVisitIdList());
           
        } else {
            self.UCAFSelectedIds.removeAll();
            
        }
        
    });

    self.StringCompanyFilter.subscribe(function (value) {

        if ($.trim(value) !== '') {
            for (i = 0; i < self.Companies().length ; i++) {
                var company = self.Companies()[i];
                var codename = company.Code + ' ' + company.Name;

                if (codename.toLowerCase().indexOf(value.toLowerCase()) < 0) {
                    company.Hidden(true);
                } else {
                    company.Hidden(false);
                }

            }
        } else {
            for (i = 0; i < self.Companies().length ; i++) {
                var company = self.Companies()[i];
                company.Hidden(false);
            }
        }
    })
    
    //modals & Dialogs
    self.Dialog = ko.observable(new Dialog("", "", "", false));

    self.PrintByPageModal = ko.observable(new PrintByPageViewModel(self,false));
}

function Company(id, name, code, parent)
{
    this.Id       = id;
    this.Name     = name;
    this.Code     = code;
    this.Hidden = ko.observable(false);
       
}



function UCAFBatchPrintActionParam()
{
    
    this.StartDate  = new Date();
    this.EndDate    = new Date();
    this.CategoryId = 0;
    this.DoctorId   = 0;
    this.Categories = [];
    this.Doctors    = [];
    this.ARUCAFRecord = [];
    this.SelectedCompaniesJson = [];
    this.ListOfSelectedVisitIds = [];

}



function Paging(totalpage, currentpage, pagesize,recordcount, parent, sortBy ,sortMode){
    that = this;
    ancient = parent;
    this.TotalRecordCount = ko.observable(recordcount); 
    this.TotalPageCount = ko.observable(totalpage);
    this.CurrentPageNumber = ko.observable(currentpage);
    this.CurrentPageGroup = ko.observable();
    this.SortBy = ko.observable(sortBy);
    this.SortMode = ko.observable(sortMode);
    this.PageGroupCount = ko.observable();
    this.PageSize = ko.observable(pagesize);
    this.GroupSize = ko.computed(function () {
         return 20;
    }, this);
    this.Pages = ko.computed(function () {
        var pages = [];

        var totalpages = that.TotalPageCount();
        var currentPage = that.CurrentPageNumber();
        var pageGoupingSize = that.GroupSize();

        that.PageGroupCount(Math.ceil(totalpages / pageGoupingSize));
        if (totalpages > 0) {
            for (i = 0; i <= that.PageGroupCount(); i++) {
                var min = i * pageGoupingSize + 1;
                var max = (min + pageGoupingSize) -1;

                if (parseInt(currentPage).inBetween(min, max)) {
                    for (c = min; c <= max && c <= totalpages; c++) {
                        pages.push(new Page(c, c, c == currentPage));
                    }
                    that.CurrentPageGroup(i + 1);
                    break;
                    
                }
                
            }
        }
        return pages;
    }, this);
    this.PrintSelectionMode = ko.observable();

    this.Goto = function (page) {
        that.CurrentPageNumber(page.Number);
        ancient.getARUCAFRecord();
 
    }

    this.Next = function () {
        that.CurrentPageNumber(that.CurrentPageNumber()+1);
        ancient.getARUCAFRecord();
      
    }
    this.Prev = function () {
        that.CurrentPageNumber(that.CurrentPageNumber() - 1);
        ancient.getARUCAFRecord();
    }
    this.NextPageGroup = function () {

        var totalpages = that.TotalPageCount();
        var currentPage = that.CurrentPageNumber();
        var pageGoupingSize = that.GroupSize();
        var nextPage = 0;
        that.PageGroupCount(Math.ceil(totalpages / pageGoupingSize));

        for (i = 0; i <= that.PageGroupCount() ; i++) {
            var min = i * pageGoupingSize + 1;
            var max = (min + pageGoupingSize) - 1;

            if (parseInt(currentPage).inBetween(min, max)) {
                nextPage = max + 1;
                that.CurrentPageGroup(i + 1);
                break;
            }

        }

        that.CurrentPageNumber(nextPage);
        ancient.getARUCAFRecord();
        document.getElementById("Print").scrollIntoView();

    }
    this.PrevPageGroup = function () {
        var totalpages = that.TotalPageCount();
        var currentPage = that.CurrentPageNumber();
        var pageGoupingSize = that.GroupSize();
        var prevPage = 0;
        that.PageGroupCount(Math.ceil(totalpages / pageGoupingSize));

        for (i = 0; i <= that.PageGroupCount(); i++) {
            var min = i * pageGoupingSize + 1;
            var max = (min + pageGoupingSize) - 1;

            if (parseInt(currentPage).inBetween(min, max)) {
                prevPage = min - pageGoupingSize;
                that.CurrentPageGroup(i+1);
                break;
            }

        }

        that.CurrentPageNumber(prevPage);
        ancient.getARUCAFRecord();
        document.getElementById("Print").scrollIntoView();
    }

    this.Sort = function (sortby, sortmode) {

        if (ancient.ARUCAFRecords().length > 0) {
            that.SortBy(sortby);
            that.SortMode(sortmode);
            that.CurrentPageNumber(1);
            ancient.getARUCAFRecord();
        }
    }

}

function Page(text, number, active) {
    this.Display = ko.observable(text);
    this.Number = ko.observable(number);
    this.isActive = ko.observable(active);
}



function iframeform(url) {
    var object = this;
    object.time = new Date().getTime();
    object.form = $('<form action="' + url + '" target="iframe' + object.time + '" method="post" style="display:none;" id="form' + object.time + '" name="form' + object.time + '"></form>');

    object.form
    object.addParameter = function (parameter, value) {
        $("<input type='hidden' />")
         .attr("name", parameter)
         .attr("value", value)
         .appendTo(object.form);
    }

    object.send = function () {
        var iframe = $('<iframe data-time="' + object.time + '" style="display:none;" id="iframe' + object.time + '"></iframe>');
        $("body").append(iframe);
        $("body").append(object.form);
        object.form.submit();
        iframe.load(function () { $('#form' + $(this).data('time')).remove(); $(this).remove(); });
    }
}


function Dialog(header, message, alertCSS, show, FnOk) {
    this.Header = ko.observable(header);
    this.Message = ko.observable(message);
    this.AlertCSS = ko.observable(alertCSS);
    this.Show = ko.observable(show);
    this.FnOK = function(){
        FnOk.send();
    }
}

function PrintByPageViewModel(parent, show) {
    ancient                 = parent;
    that = this;
    that.ShowModal = ko.observable(show);
    that.FromPageList = ko.computed(function () {
        pages = [];

        for(i = 0; i <=  ancient.UCAFRecordPaging().TotalPageCount() ;i++){
          
            if (i == 0) {
                pages.push('');
            } else {
                pages.push(i);
            }
          
        }
        return pages;
    }, this);
    that.SelectedFromPage = ko.observable();
    that.ToPageList = ko.computed(function () {
        pages = [];

        for (i = 0; i <= ancient.UCAFRecordPaging().TotalPageCount() ; i++) {
            if (i == 0) {
                pages.push('');
            } else {
                pages.push(i);
            }
        }
        return pages;
    }, this);
    that.SelectedToPage = ko.observable();

    that.SelectedVisitIds = ko.observableArray();

    that.SelectedToPage.subscribe(function (val) {

        ancient.PrintByPageModal().SelectedVisitIds([]);
        frompage = ancient.PrintByPageModal().SelectedFromPage() > 0 && ancient.PrintByPageModal().SelectedFromPage() != undefined ? ancient.PrintByPageModal().SelectedFromPage() - 1 : 0;
        topage = ancient.PrintByPageModal().SelectedToPage() > 0 && ancient.PrintByPageModal().SelectedToPage() != undefined ? ancient.PrintByPageModal().SelectedToPage(): 0;

        pagesize = ancient.UCAFRecordPaging().PageSize();

        ko.utils.arrayPushAll(ancient.PrintByPageModal().SelectedVisitIds, ancient.UCAFVisitIdList().slice(frompage * pagesize, topage * pagesize));

    }, that);

  
}