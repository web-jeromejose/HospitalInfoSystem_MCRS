using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class PolyClinicPatientReservationSummary
    {

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Patient Type")]
        public Enumerations.DoctorSchedulePatientType PatientType { get; set; }
        [Display(Name = "Registered By")]
        public int? EmployeeId { get; set; }


        public List<KeyValuePair<Enumerations.DoctorSchedulePatientType, string>> PatientTypeList { get; set; }
    }
}