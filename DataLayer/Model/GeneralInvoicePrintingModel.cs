using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class GeneralInvoicePrintingModel
    {
    }

    public class ADMITDTListModel { 
        public string AdmitDateTime {get;set;}
        public long BillNo { get; set; }
 
    }

    public class BListModel {
        public string PIN { get; set; }
        public long BillNo { get; set; }
        public string AdmitDate { get; set; }
        public string Company { get; set; }

    }

    public class BListParams
    {
        public long BillNo { get; set; }

    }

    public class BListParamsList { 
       public List<BListParams> BillNoParams { get; set; }
    }

    public class GEAccountList {
        public long Id { get; set; }
        public string Name { get; set; }
    }

}
