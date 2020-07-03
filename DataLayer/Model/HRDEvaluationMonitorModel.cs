using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class HRDEvaluationMonitorModel
    {
            public int Id { get; set; }
            public string EmployeeId { get; set; }
            public string Name { get; set; }
            public string Deptcode { get; set; }
            public string Department { get; set; }
            public string Nationality { get; set; }
            public string EvaluationType { get; set; }
            public DateTime frommonth { get; set; }
            public DateTime tomonth { get; set; }
          public string Send { get; set; }
            public string toscore { get; set; }
            public string designation { get; set; }
            public string Designation { get; set; } 
    }
}
