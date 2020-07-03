using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class ARVisitDateModel
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string VisitType { get; set; }
        public string DoctorName { get; set; }
    }
}
