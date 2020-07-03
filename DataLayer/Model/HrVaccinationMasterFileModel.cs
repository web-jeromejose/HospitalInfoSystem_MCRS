using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class HrVaccinationMasterFileModel
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Serology { get; set; }
        public int Dose1 { get; set; }
        public int Dose2 { get; set; }
        public int Dose3 { get; set; }
        public int Dose4 { get; set; }
        public int Deleted { get; set; }
         
    }
}
