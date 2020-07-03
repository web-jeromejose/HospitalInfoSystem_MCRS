using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class MMSReportViewModel
    {

        [Display(Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

    }

    public class ToolsMenuActualPunchInOutModel
    {

        [Display(Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public List<DepartmentModel> DepartmentList { get; set; }

        [Display(Name = "Employee")]
        public string EmployeeId { get; set; }
        public List<EmployeeModel> EmployeeList { get; set; }
    }

}