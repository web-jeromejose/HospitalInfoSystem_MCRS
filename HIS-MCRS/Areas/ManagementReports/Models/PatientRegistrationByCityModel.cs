using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class PatientRegistrationByCityModel
    {
        public int ReportType { get; set; }

        [Required(ErrorMessage = "From Date is required")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "From Date is required")]
        public string EndDate { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
    }
}