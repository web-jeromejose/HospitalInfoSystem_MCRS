using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataLayer.Model;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class OTOrCathLabOperation
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public string OperationOrCatLab { get; set; }

        public Boolean isDone { get; set; }


    }
}