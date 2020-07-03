using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;


namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class FinanceReport
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public int CategoryId { get; set; }
        public List<CategoryModel> CategoryList { get; set; }

        public int StationId { get; set; }
        public List<Station> StationList { get; set; }

        public int ItemGroupId { get; set; }
        public List<Station> ItemGroupList { get; set; }

        public int CompanyId { get; set; }
        public List<CompanyListModel> CompanyList { get; set; }

        public int EmployeeId { get; set; }
        public List<EmployeeModel> EmployeeList { get; set; }

        public string PinId { get; set; }

        public string ReceiptNo { get; set; }

    }
    public class PendingServices
    {
        [Display(Name = "AR From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "AR To")]
        public DateTime EndDate { get; set; }


        [Display(Name = "PIN")]
        public string PIN { get; set; }


        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public List<DepartmentModel> DepartmentList { get; set; }

        [Display(Name = "Patient  Type")]
        public Enumerations.FinanceReport_PendingServicesPatientType PatientType { get; set; }
        public List<KeyValuePair<Enumerations.FinanceReport_PendingServicesPatientType, string>> PatientTypes { get; set; }

        [Display(Name = "Covering Letter")]
        public Enumerations.FinanceReport_CoveringLetterType CoveringLetterType {get;set;}
        public List<KeyValuePair<Enumerations.FinanceReport_CoveringLetterType, string>> CoveringLetterTypes {get;set;}

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public List<CategoryModel> CategoryList { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        public List<CompanyModel> CompanyList { get; set; }


       

    }

    
}