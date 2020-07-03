using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARCompanyTariffCodes
    {
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string CategoryText { get; set; }
        
        public List<CategoryModel> CategoryList { get; set; }
    }
}