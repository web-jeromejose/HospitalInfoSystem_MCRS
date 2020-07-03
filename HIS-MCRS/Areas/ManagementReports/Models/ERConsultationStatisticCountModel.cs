using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class ERConsultationStatisticCountModel
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int ERDoctors { get; set; }
        public int NonERDoctors { get; set; }
        public int TotalChargeinER { get; set; }
    }
}