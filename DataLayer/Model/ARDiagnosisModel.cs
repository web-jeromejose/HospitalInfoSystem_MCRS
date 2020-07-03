using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class ARDiagnosisModel
    {
        public int VisitId    { get; set; }
        public int OperatorId { get; set; }
        public string TransactionDate { get; set; }
        public int ICDId { get; set; }
        public string ICDCode { get; set; }
        public string ICDDescription { get; set; }

    }
}
