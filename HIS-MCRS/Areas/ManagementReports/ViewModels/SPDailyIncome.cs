using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class SPDailyIncome
    {

        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

  

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public String CategoryName { get; set; }

        public List<MohModel> CategoryList { get; set; }

        [Display(Name = "Report Type")]
        public Enumerations.ReportCategoryType ReportType { get; set; }

        public List<KeyValuePair<Enumerations.ReportCategoryType, string>> ReportTypeList { get; set; }

    }
}