using HIS_MCRS.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class PolyClinicOPPatientCount
    {
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate   { get; set; }
        [Display(Name = "Options")]
        public OPDSelectionCount OPDSelection { get; set; }

        public List<KeyValuePair<OPDSelectionCount, string>> OPDSelectionList { get; set; }
    }
}