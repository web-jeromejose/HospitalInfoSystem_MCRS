using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;
using HIS_MCRS.Enumerations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class AuditReportsListSubAccounts
    {


        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public List<CategoryModel> Categories { get; set; }

    }
}