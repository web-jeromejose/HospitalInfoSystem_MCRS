using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARPhysiotherapyBilling
    {
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public string CategoryText { get; set; }

        public List<CategoryModel> CategoryList { get; set; }
    }
}