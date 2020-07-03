using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class ARAdmission
    {
        public int RegistrationNo       { get; set; }
        public int BillNo               { get; set; }
        public int IpId                 { get; set; }
        public string Code              { get; set; }
        public DateTime AdmitDatetime   { get; set; }

    }
}
