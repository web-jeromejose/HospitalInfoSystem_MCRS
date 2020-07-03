using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class ARInPatientIPCR
    {
        [Display(Name = "Invoice Type")]
        public int InvoiceType { get; set; }
        [Display(Name="PIN")]
        public int? Pin { get; set; }
        [Display(Name = "Invoice No.")]
        public int? InvoiceNo { get; set; }
        [Display(Name = "Admit Date")]
        public int BillNo { get; set; }


        public bool SearchByPin { get; set; }

        public int PrintOptions { get; set; }

        public List<KeyValuePair<int, string>> InvoiceTypeList { get; set; }
        

    }
}