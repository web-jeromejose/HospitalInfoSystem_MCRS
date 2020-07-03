using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataLayer.Model;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class DAPcAdminLeaveHistory
    {
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }


        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        public String EmpName { get; set; }

        public List<EmployeeModel> EmpList { get; set; }
    }
}