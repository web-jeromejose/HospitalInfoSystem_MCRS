using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;
using HIS_MCRS.Enumerations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class AuditReportsIPCancelledByDept
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public AuditReport_IpCancelledByDept BillType { get; set; }

        public List<KeyValuePair<Enumerations.AuditReport_IpCancelledByDept, string>> BillTypeList { get; set; }

    }
}