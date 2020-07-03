using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class MaintenanceReportsEquipmentBreakdown
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string checkbox { get; set; }
        [Display(Name = "Type of Equipment")]
        public string TypeofEquipment { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public string checkboxInDate { get; set; }

        public List<DepartmentModel> DepartmentList { get; set; }




    }
}