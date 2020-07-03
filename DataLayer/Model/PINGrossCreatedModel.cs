using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class PINGrossCreatedModel
    {
        public string BillNo { get; set; }
        public string BillDateTime { get; set; }
        public float Gross { get; set; }
        public float Discount { get; set; }
        public float Deductable { get; set; }
        public float Net { get; set; }
        public string Company { get; set; }
    }
}
