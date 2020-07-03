using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Model;
using HIS_MCRS.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class InfectionControlVM
    {
        [Display(Name = "From")]
        public DateTime FromDate { get; set; }
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }
 

    }
}