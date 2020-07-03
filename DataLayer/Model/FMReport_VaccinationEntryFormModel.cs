using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
   public class FMReport_VaccinationEntryFormModel
    {
        public int Id { get; set; }
        public string employeeid { get; set; }
        public string name { get; set; }
        public string vaccineid { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string serology { get; set; }
        public string serologydate { get; set; }
        public string serologyAt { get; set; }
        public string dose1 { get; set; }
        public string dose1date { get; set; }
        public string Dose1TestedAt { get; set; }
        public string dose2 { get; set; }
        public string dose2date { get; set; }
        public string Dose2TestedAt { get; set; }
        public string dose3 { get; set; }
        public string dose3date { get; set; }
        public string Dose3TestedAt { get; set; }
        public string dose4 { get; set; }
        public string dose4date { get; set; }
        public string Dose4TestedAt { get; set; }
		
		 
    }
}
