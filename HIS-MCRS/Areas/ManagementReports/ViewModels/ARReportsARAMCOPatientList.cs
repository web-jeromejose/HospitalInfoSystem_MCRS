using DataLayer.Model;
using HIS_MCRS.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARReportsARAMCOPatientList
    {
        [Display(Name="Relation")]
        public int RelationshipId { get; set; }
        [Display(Name = "Patient Status")]
        public PatientStatus PatientStatus { get; set; }
        public string Relationship { get; set; }

        public List<KeyValuePair<int,string>> PatientStatusList { get; set; }
        public List<Relationship> RelationshipList { get; set; }
    }
}