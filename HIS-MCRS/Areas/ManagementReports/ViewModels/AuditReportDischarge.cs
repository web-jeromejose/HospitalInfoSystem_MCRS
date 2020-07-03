using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class AuditReportDischarge
    {
        [Display(Name = "Doctor")]
        public string DoctorId { get; set; }

        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public List<EmployeeModel> DoctorsList { get; set; }

        public bool DocOrNone { get; set; }

    }
}