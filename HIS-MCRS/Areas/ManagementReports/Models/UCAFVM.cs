using HIS_MCRS.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HIS_MCRS.Models;
namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class UCAFVM
    {
       public UCAFVM()
        {
            var emptyListItem = new List<SelectListItem>() { new SelectListItem { Text = "", Value = "" } };
            this.VisitDates = new SelectList(emptyListItem, "Value", "Text");
        }
        [Display(Name="Registration No")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid Registration Number")]
        public int? RegistrationNo { get; set; }
        [Display(Name = "Visit Date")]
        public int VisitId { get; set; }

        public SelectList VisitDates;
    }
}