using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class PatientCharityLetterReportModel
    {
        public string PIN { get; set; }
        public string PTName { get; set; }
        public string AdvDepNo { get; set; }
        public string MOP { get; set; }
        public decimal Amount { get; set; }
        public decimal SettledDeposit { get; set; }
        public string DepDate { get; set; }
        public string Code { get; set; }
        public string BillDate { get; set; }
        public string ExpDate { get; set; }
    }
}
