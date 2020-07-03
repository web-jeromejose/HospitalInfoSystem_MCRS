using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 
using DataLayer.Model;
 


namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class PolyClinicOPItemBillCount
    {
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Service Item")]
        [Required(ErrorMessage="Item Code Required")]
        public string StrItemCodes { get; set; }

        public List<Sex> Services { get; set; }
        public string ServiceId { get; set; }
    }
}