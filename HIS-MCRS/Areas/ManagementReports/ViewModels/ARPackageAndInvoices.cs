using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARPackageAndInvoices
    {
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "From Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To Date")]
        public DateTime EndDate { get; set; }

        public int Deal { get; set; }
        public int ReportOption { get; set; }

        public List<CategoryModel> CategoryList { get; set; }
    }
}