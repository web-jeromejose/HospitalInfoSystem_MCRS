using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class AROPFrontLineBillingTracker
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }
        [Display(Name = "BillDate")]
        public bool FilterBillDate { get; set; }
        [Display(Name = "Receipt No.")]
        public bool FilterReceiptNo { get; set; }
        [Display(Name = "Receipt")]
        public string BillNo { get; set; }

    }
}