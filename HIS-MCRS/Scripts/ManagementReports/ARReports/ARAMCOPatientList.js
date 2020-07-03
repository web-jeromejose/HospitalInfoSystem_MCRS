function ViewModel(model) {
    self = this;
 
    self.PatientStatusList = ko.observable(model.PatientStatusList  );
    self.RelationshipList = ko.observableArray(model.RelationshipList);
    self.RelationshipText = ko.observable("ALL");

    self.SelectedPatientStatus = ko.observable(model.PatientStatus);
    self.SelectedRelation = ko.observable(0);

    self.init = function () {
        self.RelationshipList.unshift({ Id: 0, Name: 'ALL' });
    }
    
    self.SelectedRelation.subscribe(function (newValue) {

        if (newValue) {
            $.each(self.RelationshipList(), function (i, item) {
                if (item.Id === newValue) {
                    self.RelationshipText(item.Name);
                }
            });
        }
    });
    self.init();
}
