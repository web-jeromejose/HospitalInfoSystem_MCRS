using HIS_MCRS.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class PolyClinicProcedureStatistics
    {   
            [Display(Name = "Start Date")]
            public DateTime StartDate { get; set; }
            [Display(Name = "End Date")]
            public DateTime EndDate { get; set; }
            [Display(Name = "Report Type")]
            public Enumerations.ReportType ReportType { get; set; }

            public List<KeyValuePair<Enumerations.ReportType, string>> ReportTypeList { get; set; }
           
    }
}