using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARPackageDealDaily
    {
        [Display(Name="From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }
        public string Packages { get; set; }
        
        public List<ARPackageBill> PackageBillList { get; set; }
    }
}