using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class DeficiencyFilesModel
    {
        public bool BoolValue
        {
            get { return IncludeStandards == "Y"; }
            set { IncludeStandards = value ? "Y" : "N"; }
        }
        public string IncludeStandards { get; set; }
        public int ReportType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Group { get; set; }
        public string Floors { get; set; }
        public int GraphType { get; set; }
    }
}