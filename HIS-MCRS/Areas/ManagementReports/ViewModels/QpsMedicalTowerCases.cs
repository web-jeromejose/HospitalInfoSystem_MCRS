using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class QpsMedicalTowerCases
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        public List<EmployeeModel> Doctors { get; set; }

    }
}