using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;


namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class MrdAdmissionDischargeStats
    {
        [Display(Name = "From ")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public string RepType { get; set; }



    }
}