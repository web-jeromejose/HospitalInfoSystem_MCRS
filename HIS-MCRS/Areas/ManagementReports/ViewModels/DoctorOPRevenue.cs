using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARReportsDoctorRevenueOP :FinanceReportsOPRevenue
    {
        [Display(Name = "Bill Type")]
        public new Enumerations.PatientBillType PatientBillType { get; set; }


    }
}