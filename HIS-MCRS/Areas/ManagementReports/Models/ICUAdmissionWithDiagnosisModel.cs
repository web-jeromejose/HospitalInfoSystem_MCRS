using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class ICUAdmissionWithDiagnosisModel
    {
        public Int32 Diagnosis { get; set; }
        public string DiagnosisSelectedValue { get; set; }
        public int LengthOfStay { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}