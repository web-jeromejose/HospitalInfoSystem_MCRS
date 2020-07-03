using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class PolyClinicSummaryOfCancelledAppointments
    {
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Patient Type")]
        public Enumerations.DoctorSchedulePatientType PatientType { get; set; }
        [Display(Name = "Employee")]
        public int? EmployeeId { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Doctor")]
        public int? DoctorId { get; set; }
        public int ReportOption { get; set; }

        public List<DepartmentModel> DepartmentList { get; set; }
        public List<KeyValuePair<Enumerations.DoctorSchedulePatientType, string>> PatientTypeList { get; set; }

    }
}