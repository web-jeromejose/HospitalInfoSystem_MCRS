using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;
using HIS_MCRS.Enumerations;


namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class SalesPromotionDailyPerformanceActBudget
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

       
        public string BranchId { get; set; }

        public Bi_Site BranchList { get; set; }

        public Enumerations.SalesPromotion_ReportType ReportType { get; set; }

        public List<KeyValuePair<Enumerations.SalesPromotion_ReportType, string>> ReportTypeList { get; set; }


    }
}