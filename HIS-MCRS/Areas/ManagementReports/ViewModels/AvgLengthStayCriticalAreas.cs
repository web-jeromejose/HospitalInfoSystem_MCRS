using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class AvgLengthStayCriticalAreas
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Area")]
        public String Area { get; set; }

        public string DeptName { get; set; }
        //public List<KeyValuePair<int, string>> BillTypes { get; set; }

        public List<DepartmentModel> DepartmentList { get; set; }

          

    }
}