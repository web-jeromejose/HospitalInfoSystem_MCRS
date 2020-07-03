using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class HDREmployeeWiseModel
    {
        public int id { get; set; }
        public string employeeId { get; set; }
        public string evaluation { get; set; }
        public DateTime frommonth { get; set; }
        public DateTime tomonth { get; set; }

    }
}
