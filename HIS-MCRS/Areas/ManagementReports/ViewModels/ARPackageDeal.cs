using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARPackageDeal
    {
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }
        public int ReportOption { get; set; }
        public string Department { get; set; }

        public List<DepartmentModel> DepartmentList { get; set; }
    }
}