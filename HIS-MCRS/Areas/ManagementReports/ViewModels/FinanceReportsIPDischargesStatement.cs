using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class FinanceReportsIPRevenue
    {
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Patient  Type")]
        public Enumerations.PatientBillType PatientBillType { get; set; }
        public string PatientTypeText { get; set; }
        [Display(Name = "Bill Type")]
        public int BillType { get; set; }
        public string BillTypeText { get; set; }
        [Display(Name = "By Cancelation  Date")]
        public bool SortByCancellationDate { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }
        [Display(Name = "PIN")]
        public int? PIN { get; set; }
        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [Display(Name = "Revenue")]
        public bool IsRevenue { get; set; }


        [Display(Name = "Start Date")]
        public DateTime StartDateExcel { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDateExcel { get; set; }
        [Display(Name = "Patient  Type")]
        public Enumerations.PatientBillType PatientBillTypeExcel { get; set; }
        [Display(Name = "Bill Type")]
        public int BillTypeExcel { get; set; }
        public List<KeyValuePair<int, string>> BillTypesExcel { get; set; }

        public List<KeyValuePair<int, string>> BillTypes { get; set; }
       
        public List<KeyValuePair<Enumerations.PatientBillType, string>> PatientBillTypes { get; set; }
        public List<KeyValuePair<Enumerations.PatientBillType, string>> PatientBillTypesExcel { get; set; }
        public List<DepartmentModel> DepartmentList { get; set; }
        public List<IPBService> Services { get; set; }
    }
}