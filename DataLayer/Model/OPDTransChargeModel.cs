using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class OPDTransChargeModel
    {
            public string Name { get; set; }
            public string Agesex { get; set; }
            public string Branch { get; set; }
            public string BranchAddress { get; set; }
            public int Type { get; set; }
            public string OPBillId { get; set; }
            public string BillNo { get; set; }
            public int Seq { get; set; }
            public string Company { get; set; }
            public string BillDateTime { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public float PaidAmount { get; set; }
            public float BillAmount { get; set; }
            public float Balance { get; set; }
            public string Quantity { get; set; }
            public float CanAmount { get; set; }
            public string IssueUnit { get; set; }

    }
}
