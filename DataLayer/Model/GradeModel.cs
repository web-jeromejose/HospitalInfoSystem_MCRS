using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class GradeModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public int TariffId { get; set; }
        
        public string PolicyNo { get; set; }
        public string Name { get; set; }
        public string GradeName { get; set; }

    }
}
