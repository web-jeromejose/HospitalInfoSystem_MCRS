using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    class BillingOfficeCompanyMappingModel
    {
    }

    public class BillOfficerCompMap {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Assigned { get; set; }
        public int CategoryId { get; set; }
        public string CatCode { get; set; } 
    }

    public class CompanyListModel {
        public int CategoryId { get; set; }
        public long CompanyId { get; set; }
    }

    public class CompanyListParams
    {
        public List<CompanyListModel> clist { get; set; }

    }
}
