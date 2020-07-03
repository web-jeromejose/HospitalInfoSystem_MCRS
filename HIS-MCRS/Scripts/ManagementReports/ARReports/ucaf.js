
function UCAFViewModel(model) {
    var self = this;

    self.inputUrlActions = null;

    self.RegistrationNo = ko.observable(model.RegistrationNo == 0 ? "" : model.RegistrationNo);

    self.selectedVisitId = ko.observable(model.VisitId);

    self.VisitDates = ko.observableArray(['']);

    self.getvisitDates = function () {
        self.VisitDates(['']);
        //positive inter only

        if (self.isPositveInteger(self.RegistrationNo())) {
            url = self.inputUrlActions.data('getvisitdates');
            param = { regNumber: self.RegistrationNo() }
            ajaxWrapper.Get(url, param, function (data, e) {
                data = JSON.parse(data)
                if (data.length > 0) {
                    $.each(data, function (index, item) {
                        var visitDate = new VisitDate();
                        visitDate.Id(item.Id != "" ? item.Id : 0);
                        visitDate.DateTime(moment(item.DateTime).format('DD-MMM-YYYY'));
                        visitDate.VisitType(item.VisitType);
                        visitDate.DoctorName(item.DoctorName);

                        self.VisitDates.push(visitDate);
                    })

                    self.selectedVisitId(self.VisitDates()[1].Id());
                }

            });
        }
    }
    self.isPositveInteger = function (str) {
        var n = ~~Number(str);
        return String(n) === str && n >= 0;
    }
    self.showUpdateModal = function () {
        if (self.selectedVisitId() == 0) {
            errorDialog = new Dialog("ERROR NOTIFICATION", "No Doctor's Entry Found", "alert-danger", true);
            self.Dialog(errorDialog);
        } else if (self.selectedVisitId() > 0) {
            self.showARDialog(true);
            self.getConsultationDetail();
            self.getDiagnosis();
        } else {
            //do nothing
        }
    }

    self.showARDialog = ko.observable(false);

    self.showConfirmUpdateDialog = ko.observable(false);

    self.showErrorDialog = ko.observable(false);

    self.Dialog = ko.observable(new Dialog("", "", "", false));

    self.ClinicVisitDetail = ko.observable(new UpdateInsertClinicalVisit());

    self.saveOrUpdateClinicDetails = function () {
        url = self.inputUrlActions.data('isuserardoctor');
        param = {}
        ajaxWrapper.Post(url, param, function (result, e) {
            if (result.isARDoctor == true) {

                self.saveClinicalVisit();

            } else {
                self.showConfirmUpdateDialog(false);
                errorDialog = new Dialog("ERROR NOTIFICATION", "Access Denied", "alert-danger", true);
                self.Dialog(errorDialog);
            }

        });
    };

    self.saveClinicalVisit = function () {

        self.showConfirmUpdateDialog(false);
        url = self.inputUrlActions.data('saveclinicalvisit');
        param = self.ClinicVisitDetail();
        ajaxWrapper.Post(url, param, function (result, e) {
            if (result.success) {

                self.deleteDiagnosis();
            } else {
                errorDialog = new Dialog("ERROR", "Something went wrong please contact I.T. Operator", "alert-danger", true);
                self.Dialog(errorDialog);
            }

        });
    }

    self.getConsultationDetail = function () {
        url = self.inputUrlActions.data('getconsultationdetail');
        param = { visitId: self.selectedVisitId() }
        ajaxWrapper.Get(url, param, function (consultationVisitDetail, e) {
            var insertUpdateClinicalVisit = self.ClinicVisitDetail();
            insertUpdateClinicalVisit.VisitId = consultationVisitDetail.VisitId;
            insertUpdateClinicalVisit.ChiefComplaints = consultationVisitDetail.ChiefComplaints;
            insertUpdateClinicalVisit.TreatmentPlan = consultationVisitDetail.TreatmentPlan;

            self.ClinicVisitDetail(insertUpdateClinicalVisit);
        });
    };

    self.getDiagnosis = function () {
        self.ARDiagnosis([]);
        if (self.selectedVisitId() > 0) {
            url = self.inputUrlActions.data('getdiagnosis');
            param = { visitId: self.selectedVisitId() }
            ajaxWrapper.Get(url, param, function (diagnosislist, e) {

                $.each(diagnosislist, function (index, diagnosis) {
                    self.ARDiagnosis.push(
                        new Diagnosis(diagnosis.ICDId, diagnosis.ICDCode, diagnosis.ICDDescription, false, diagnosis.VisitId, diagnosis.OperatorId));

                });
            });
        }
    };

    self.deleteDiagnosis = function () {

        url = self.inputUrlActions.data('deleteardiagnosis');
        param = { visitId: self.selectedVisitId() }
        ajaxWrapper.Post(url, param, function (result, e) {
            self.updateDiagnosis();
        });

    }

    self.updateDiagnosis = function () {
        url = self.inputUrlActions.data('updatediagnosis');
        $.each(self.ARDiagnosis(), function (index, diagnosis) {
            var param = new ARDiagnosisModel();
            param.ICDId = diagnosis.Id;
            param.ICDDescription = diagnosis.Description;
            param.ICDCode = diagnosis.Code;
            param.VisitId = diagnosis.VisitId;

            ajaxWrapper.Post(url, param, function (result, e) {
            });
        })

        self.getConsultationDetail();

        succesDialog = new Dialog("SUCCESS NOTIFICATION", "Data Updated", "alert-info", true);
        self.Dialog(succesDialog);
        //self.getDiagnosis();

    }

    self.selectedDiagnosis = ko.observable(new Diagnosis(0,'',false,0,0));

    self.ARDiagnosis = ko.observableArray([]);

    self.removeDiagnosis = function () {
        self.ARDiagnosis.remove(function (item) {
            return item == self.selectedDiagnosis();
        });

    };

    self.selectDiselectDiagnosis = function (diagnosis) {
        var selectedItem = new Diagnosis();
        for (i = 0; i < self.ARDiagnosis().length; i++) {
            self.ARDiagnosis()[i].isSelected(false);
            if (self.ARDiagnosis()[i] == diagnosis && diagnosis !== self.selectedDiagnosis()) {
                diagnosis.isSelected(true);
                selectedItem = diagnosis;
            }
        }
        self.selectedDiagnosis(selectedItem);
    };

    self.clearFields = function () {
        self.VisitDates(['']);
        self.selectedVisitId(['']);
        self.RegistrationNo('');
    }

    self.SearchICDDialog = new SearchICDViewModel(self);

}

function VisitDate(id, datetime, visitType, doctorName) {
    var self = this;
    self.Id = ko.observable(id);
    self.DateTime = ko.observable(datetime);
    self.VisitType = ko.observable(visitType);
    self.DoctorName = ko.observable(doctorName);
    self.Text = ko.computed(function () {

        return self.DateTime() + " - " + self.VisitType() + " - " + self.DoctorName();
    }, self);
}

function UpdateInsertClinicalVisit(visitId, chiefComplaints, treatmentplan, operatorId, transactionDate, visitId) {
    self = this;
    self.VisitId = visitId;
    self.ChiefComplaints = chiefComplaints;
    self.TreatmentPlan = treatmentplan;
    self.OperatorId = operatorId;
    self.TransactionDateTTime = transactionDate;
}

function Diagnosis(id, code, description, selected, visitId, operatorId) {
    self = this;
    self.Id = id;
    self.Code = code;
    self.Description = description;
    self.isSelected = ko.observable(selected);
    self.VisitId = visitId;
    self.OperatorId = operatorId;
};

function ARDiagnosisModel(id, visitId, icdCode, icdDescription, transactionDate, operatorId) {
    self = this;
    self.ICDId = id;
    self.VisitId = visitId;
    self.ICDCode = icdCode;
    self.ICDDescription = icdDescription;
    self.TransactionDate = transactionDate;
    self.OperatorId = operatorId;


}

function Dialog(header, message, alertCSS, show) {
    self = this;
    self.Header = ko.observable(header);
    self.Message = ko.observable(message);
    self.AlertCSS = ko.observable(alertCSS);
    self.Show = ko.observable(show);
}


function ICD10Code(id, code, description, selected) {
    self = this;
    self.Id = id;
    self.Code = ko.observable(code);
    self.Description = description;
    self.isSelected = ko.observable(selected);
}

function SearchICDViewModel(parent) {
    self = this;
    self.ICD10Codes = ko.observableArray(['']);
    self.selectedICD10Code = ko.observable(null);
    self.ShowDialog = ko.observable(false);
    self.SearchDiscripton = ko.observable();
    self.searchICDCode = function (page) {
        parent.SearchICDDialog.ICD10Codes([]);
        parent.SearchICDDialog.Pages([]);
        url = parent.inputUrlActions.data('geticd10codes');

        //cant see self after init call parent instead
        if (page) {
            param = { description: parent.SearchICDDialog.SearchDiscripton(), page:page.Number()}
        } else {
            param = { description: parent.SearchICDDialog.SearchDiscripton() }
        }
       
        ajaxWrapper.Get(url, param, function (result, e) {

            $.each(result.ICD10, function (index, icd) {

                var icdcode = new ICD10Code();
                icdcode.Id = icd.Id;
                icdcode.Code = icd.Code;
                icdcode.Description = icd.Description;
                icdcode.isSelected(false);

                //cant see self after init call parent instead
                parent.SearchICDDialog.ICD10Codes.push(icdcode);
            });

            parent.SearchICDDialog.PageSize(result.PageSize)
            parent.SearchICDDialog.CurrentPage(result.CurrentPage)
            parent.SearchICDDialog.TotalPage(result.TotalPage);

            //adding pages and limit to 1
            for (i = 1; i <= parent.SearchICDDialog.TotalPage() && i <=15 ; i++) {

                parent.SearchICDDialog.Pages.push(new Page(i, i, i == parent.SearchICDDialog.CurrentPage()));
            }
            
        });

    }

    self.TotalPage = ko.observable();
    self.CurrentPage = ko.observable();
    self.PageSize = ko.observable();
    self.Pages = ko.observableArray([]);

    self.Prev = function () {

        currentPageObj = parent.SearchICDDialog.Pages()[parent.SearchICDDialog.CurrentPage()-1];
        index = parent.SearchICDDialog.Pages().indexOf(currentPageObj);

        if (index != -1 && index > 0) {

            parent.SearchICDDialog.searchICDCode(parent.SearchICDDialog.Pages()[index-1]);
        }
        
    }
    self.Next = function () {

        currentPageObj = parent.SearchICDDialog.Pages()[parent.SearchICDDialog.CurrentPage()-1];
        index = parent.SearchICDDialog.Pages().indexOf(currentPageObj);

        if (index != -1 && index < 14 && index+1 < parent.SearchICDDialog.TotalPage()) {

            parent.SearchICDDialog.searchICDCode(parent.SearchICDDialog.Pages()[index+1]);
        }
    }

    self.selectCode = function (code) {

        parent.SearchICDDialog.setCodesToUnselected();
       
       if (code !== parent.SearchICDDialog.selectedICD10Code()) {
           code.isSelected(true);
           parent.SearchICDDialog.selectedICD10Code(code);
       } else {
           parent.SearchICDDialog.selectedICD10Code(null);
       }
       
    }

    self.AddSelectedCode = function () {

        var code = parent.SearchICDDialog.selectedICD10Code();

        var diagnonis = new Diagnosis(code.Id, code.Code, code.Description, false, parent.selectedVisitId(), 0);
        
        parent.ARDiagnosis.push(diagnonis);
        parent.SearchICDDialog.selectedICD10Code(null);
        parent.SearchICDDialog.setCodesToUnselected();
        parent.SearchICDDialog.ShowDialog(false);
    }

    self.setCodesToUnselected = function () {
        $.each(parent.SearchICDDialog.ICD10Codes(), function (index, item) {

            item.isSelected(false);

        });
    }

};

function Page(text, number, active) {
    self = this;
    self.Display = ko.observable(text);
    self.Number = ko.observable(number);
    self.isActive = ko.observable(active);
}