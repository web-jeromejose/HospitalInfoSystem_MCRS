using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class InsertUpdateARClinicalVisitVM
    {
        public int VisitId { get; set; }
        public string ChiefComplaints { get; set; }
        public string TreatmentPlan { get; set; }
        public int OperatorId { get; set; }
        public DateTime TransactionDateTTime { get; set; }
    }
}