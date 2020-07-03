using HIS_MCRS.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class RadiologyReportXrayReportCharges
    {

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }


    }
}