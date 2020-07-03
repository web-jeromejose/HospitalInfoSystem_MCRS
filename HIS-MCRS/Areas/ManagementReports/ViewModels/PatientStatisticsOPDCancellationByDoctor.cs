using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class PatientStatisticsOPDCancellationByDoctor
    {
        [Display(Name="Service")]
        public int ServiceId { get; set; }
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }
        public DateTime From { get; set; }
        public DateTime To {get;set;}
        [Display(Name = "Sort By")]
        public int SortBy { get; set; }
        [Display(Name = "Group By Doctor")]
        public bool GroupByDoctor { get; set; }

        [Display(Name = "Reason")]
        public int ReasonId { get; set; }


        public List<OPBService> Services { get; set; }
        public List<EmployeeModel> Doctors { get; set; }
        public List<Cancelbillreason> CancelBillReasons { get; set; }

        public List<KeyValuePair<int, string>> SortOptions
        {
            get
            {
                var options = new List<KeyValuePair<int, string>> (){
                  new KeyValuePair<int, string> (0, "DEFAULT"),
                  new KeyValuePair<int, string> (1, "DOCTOR"),
                  new KeyValuePair<int, string> (2, "CATEGORY")
                
                };
                return options;
            }
        }

    

    }
}