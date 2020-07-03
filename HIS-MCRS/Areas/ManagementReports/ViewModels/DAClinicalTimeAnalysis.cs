using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataLayer.Model;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class DAClinicalTimeAnalysis
    {
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }


        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        public String DoctorName { get; set; }

        public List<DepartmentModel> DepartmentList { get; set; }



    }
}