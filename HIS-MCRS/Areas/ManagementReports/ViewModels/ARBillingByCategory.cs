using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARBillingByCategory
    {
        
        public string JsonStrCategoryIds { get; set; }
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public string PatientType { get; set; }

        public List<CategoryModel> CategoryList { get; set; }
        public List<KeyValuePair<string, string>> PatientTypeList { get; set; }


    }
}