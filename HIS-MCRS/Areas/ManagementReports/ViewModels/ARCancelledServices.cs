using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARCancelledServices
    {
        [Display(Name= "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        [Display(Name = "Type")]
        public string PatientType { get; set; }
        public string CategoryText { get; set; }
        public string PatientTypeText { get; set; }

        public List<CategoryModel> CategoryList { get; set; }
        public List<CompanyModel>  CompanyList { get; set; }
        public List<KeyValuePair<string, string>> PatientTypeList { get; set; }
    }
}