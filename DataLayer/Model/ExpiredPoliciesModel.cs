using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class ExpiredPoliciesModel
    {
        public string Company { get; set; }
        public string PolicyNo { get; set; }
        public string ValFrom { get; set; }
        public string ValTill { get; set; }
    }
}
