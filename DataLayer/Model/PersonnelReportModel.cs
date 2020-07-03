using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class PersonnelReportModel
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime DateHired { get; set; }
        public string ContractType { get; set; }
        public string PIN { get; set; }

        public DateTime doa { get; set; }
        public string employeeid { get; set; }
        public string fullname { get; set; }
        public string deptcode { get; set; }
        public string name { get; set; }
        public string cv { get; set; }
        public string orient_dept { get; set; }
        public string orient_gen { get; set; }
        public string jd { get; set; }
        public string license { get; set; }
        public string educ_cert { get; set; }
        public string fs { get; set; }
        public string ifc { get; set; }
        public string tqm { get; set; }
        public string bcls { get; set; }
        public string acls { get; set; }
        public string eval_1 { get; set; }
        public string eval_2 { get; set; }
        public string eval_3 { get; set; }
        public string eval_4 { get; set; }
        public string confidentiality { get; set; }
        public string credentialing { get; set; }
        public string previledging { get; set; }
    }
}
