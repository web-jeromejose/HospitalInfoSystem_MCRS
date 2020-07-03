using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class FinanceRevenueAdjustments
    {

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Bill Type")]
        public int BillType { get; set; }
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        [Display(Name = "Company")]
        public int? CompanyId { get; set; }


        public List<CategoryModel> Categories { get; set; }
        public List<CompanyModel> Companies { get; set; }
        public List<KeyValuePair<int, string>> BillTypes { get; set; }
    
    }
    public class CLDetailedReport
    {

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Date")]
        public string Date { get; set; }

        [Display(Name = "Bill Type")]
        public int BillType { get; set; }
      
        public List<KeyValuePair<int, string>> BillTypes { get; set; }

    }

    
    public class CLRevenueRecon
    {

        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }


        [Display(Name = "Case Type")]
        public int BillType { get; set; }

        public List<KeyValuePair<int, string>> BillTypes { get; set; }

    }
}