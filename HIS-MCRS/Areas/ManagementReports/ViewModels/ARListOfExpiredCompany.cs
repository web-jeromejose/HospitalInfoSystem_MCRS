using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARListOfExpiredCompany
    {
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Expiry To Date")]
        public DateTime ExpiryDate { get; set; }
        public Enumerations.CompanyStatus CompanyStatus { get; set; }

        public List<CategoryModel> CategoryList { get; set; }
    }
}