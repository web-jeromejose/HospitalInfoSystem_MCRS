using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class StatementSummaryByAllCategoryVM
    {
        [Display(Name="From")]
        public DateTime StartDate {get; set;}
        [Display(Name="To")]
        public DateTime EndDate   {get; set;}
    }

    public class ReportDoctorListPatientRecordVM
    {
        //[Display(Name = "From")]
        //public DateTime StartDate { get; set; }
        //[Display(Name = "To")]
        //public DateTime EndDate { get; set; }
        //[Display(Name = "Doctor")]
        //public int DoctorId { get; set; }

        //public List<SelectListItem> Doctors { get; set; }

        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        public List<EmployeeModel> Doctors { get; set; }

    }

    public class NetRevenueVM
    {

        
        [Display(Name = "PIN")]
        public string RegistrationNo { get; set; }

        [Display(Name = "Cancelled Only")]
        public int CancelledOnly { get; set; }

        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        [Display(Name = "IPID")]
        public int IPID { get; set; }

        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        [Display(Name = "Bill Finalize")]
        public Enumerations.FinanceReport_NetRevenueBillFinalize BillFinalize { get; set; }
        public List<KeyValuePair<Enumerations.FinanceReport_NetRevenueBillFinalize,string>> BillFinalizeList { get; set; }

        [Display(Name = "Patient  Type")]
        public Enumerations.FinanceReport_NetRevenue PatientType { get; set; }
        public List<KeyValuePair<Enumerations.FinanceReport_NetRevenue, string>> PatientTypeList { get; set; }
        public string PatientTypeText { get; set; }
         
        [Display(Name = "Bill Type")]
        public Enumerations.FinanceReport_NetRevenueBillType BillType { get; set; }
        public List<KeyValuePair<Enumerations.FinanceReport_NetRevenueBillType,string>> BillTypeList {get;set;}
        public string BillTypeText { get; set; }

        public int Category { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public string CategoryText { get; set; }

        
        public Enumerations.FinanceReport_NetRevenueGroupByCategory GroupByCategory { get; set; }
        public List<KeyValuePair<Enumerations.FinanceReport_NetRevenueGroupByCategory, string>> GroupByCategoryList { get; set; }


    }



}