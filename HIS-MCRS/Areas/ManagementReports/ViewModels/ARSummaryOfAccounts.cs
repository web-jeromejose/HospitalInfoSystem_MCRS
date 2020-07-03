using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARSummaryOfAccounts
    {
        [Display(Name = "From Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To Date")]
        public DateTime EndDate { get; set; }
      
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        [Display(Name = "Sub Category")]
        public int SubCategoryId { get; set; }
        [Display(Name = "Grade")]
        public int GradeId { get; set; }

        public int Type { get; set; }
        public int isAfterCoveringLetter { get; set; }

        [Display(Name = "Bank Details")]
        public bool BankDetails { get; set; }

        [Display(Name = "After Covering Letter")]
        public bool AfterCoveringLetter { get; set; }

        public string SubCategory { get; set; }
        public string Category { get; set; }
        
        public List<CategoryModel> CategoryList { get; set; }
        public List<CategoryModel> SubCategoryList { get; set; }

 

    }
}