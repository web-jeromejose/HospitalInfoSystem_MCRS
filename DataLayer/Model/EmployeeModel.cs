using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
     public class EmployeeModel
    {
         public int OperatorId      { get; set; }
         public string EmployeeId   { get; set; }
         public string EmpCode      { get; set; }
         public string FirstName    { get; set; }
         public string MiddleName   { get; set; }
         public string LastName     { get; set; }
         public string FullName     { get; set; }
    }
}
