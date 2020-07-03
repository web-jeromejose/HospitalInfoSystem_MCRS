using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class ParamBoolandDateModel
    {
        public bool BoolValue
        {
            get { return IntValue == 1; }
            set { IntValue = value ? 1 : 0; }
        }
        public int IntValue { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

    }
}