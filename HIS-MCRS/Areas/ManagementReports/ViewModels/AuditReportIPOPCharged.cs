using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;
using HIS_MCRS.Enumerations;


namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class AuditReportIPOPCharged
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public AuditReport_PackageType PackageId { get; set; }

        public AuditReport_PackageType PackageList { get; set; }

        public Enumerations.AuditReport_ChargeType ChargeType { get; set; }

        public List<KeyValuePair<Enumerations.AuditReport_ChargeType, string>> ChargeTypeList { get; set; }


    }

}