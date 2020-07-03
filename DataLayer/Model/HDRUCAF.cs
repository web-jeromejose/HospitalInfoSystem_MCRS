using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class HDRUCAFModel
    {
        public DateTime VisitDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public string VisitType { get; set; }
        public string VisitID { get; set; }
        public string PIN { get; set; }
        public string InsuCode { get; set; }
        public string VisitDept { get; set; }
        public string PTName { get; set; }
        public string IDCard{get;set;}
        public string Sex { get; set; }
        public string Class { get; set; }
        public string Doctorname { get; set; }
        public string Policyno { get; set; }
        public string company { get; set; }
       
        public int Age { get; set; }

    }
}
