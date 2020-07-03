using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class FMReport_GetVaccinationPendingModel
    {

        public int Id { get; set; }
        public string employeeid { get; set; }
        public string name { get; set; }
        public string department { get; set; }
        public string itemname { get; set; }
        public string dose1date { get; set; }
        public string dose1 { get; set; }
        public string serology { get; set; }
        public string dose2 { get; set; }
        public string dose3 { get; set; }
        public string dose2date { get; set; }
        public string serologydate { get; set; }
 
    }
}
