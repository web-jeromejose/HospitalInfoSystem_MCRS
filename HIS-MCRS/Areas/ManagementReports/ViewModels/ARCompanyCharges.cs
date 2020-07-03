using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARCompanyCharges
    {

        [Display(Name = "Services")]
        public int OPBServiceId { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Base Charge Amount")]
        public Decimal BaseCharge { get; set; }

        public bool HasBaseCharge { get; set; }

        public string CategoryText { get; set; }
        public string ServiceText { get; set; }

        public List<CategoryModel> CategoryList { get; set; }
        public List<OPBService> ServiceList { get; set; }
    }
}