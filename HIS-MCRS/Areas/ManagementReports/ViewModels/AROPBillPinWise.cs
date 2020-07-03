using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class AROPBillPinWise
    {
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        [Display(Name = "Pin")]
        public int Pin { get; set; }

        [Display(Name = "Pin")]
        public string PinId { get; set; }

        [Display(Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public string CategoryName { get; set; }
        public string CompanyName { get; set; }

        public List<CompanyModel> CompanyList { get; set; }
        public List<CategoryModel> CategoryList { get; set; }
    }
}