using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class FinanceReportsOPRevenue
    {
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name="End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Patient  Type")]
        public Enumerations.PatientBillType PatientBillType { get; set; }
        [Display(Name = "Bill  Type")]
        public Enumerations.BillType BillType { get; set; }
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


        //for excel
        [Display(Name = "Start Date")]
        public DateTime StartDate2 { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate2 { get; set; }
        [Display(Name = "Patient  Type")]
        public Enumerations.PatientBillType PatientBillType2 { get; set; }




        public List<KeyValuePair<Enumerations.PatientBillType, string>> PatientBillTypes { get; set; }
        public List<KeyValuePair<Enumerations.PatientBillType, string>> PatientBillTypes2 { get; set; }
        public List<KeyValuePair<Enumerations.BillType, string>> BillTypes { get; set; }
        public List<DepartmentModel> DepartmentList { get; set; }

    }
 

    public class FinanceReportsOPRevenueDataTable
    {

      
        public DateTime StartDate { get; set; }
         
        public DateTime EndDate { get; set; }
        
        public Enumerations.PatientBillType PatientBillType { get; set; }
       
        public Enumerations.BillType BillType { get; set; }
        
        public bool SortByCancellationDate { get; set; }
        
        public int DepartmentId { get; set; }
        
        public int CompanyId { get; set; }
     
        public int DoctorId { get; set; }
       
        public string PIN { get; set; }
        public string EmpId { get; set; }
        public string ModeofPayment { get; set; }
     


    }
    public class FinanceReportsVatDetails
    {
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
 

    }
}