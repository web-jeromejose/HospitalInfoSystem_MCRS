using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;


namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class AuditReportIPChargeBilledReport
    {


        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public int ChargedORBilled { get; set; }

        [Display(Name = "Account Type")]
        public int AccountType { get; set; }

        public int ChargedType { get; set; }

        public bool IsConsultation { get; set; }

        [Display(Name = "Doctor")]
        public string DoctorId { get; set; }

        [Display(Name = "Service")]
        public string ServiceId { get; set; }

        [Display(Name = "Category")]
        public string CategoryId { get; set; }

        [Display(Name = "Company")]
        public string CompanyId { get; set; }


        [Display(Name = "Type")]
        public int TypeIPId { get; set; }
        public List<KeyValuePair<int, string>> TypeIPList { get; set; }

        [Display(Name = "Type")]
        public int TypeGradeId { get; set; }
        public List<KeyValuePair<int, string>> TypeGradeList { get; set; }

        [Display(Name = "Level")]
        public int LevelId { get; set; }
        public List<KeyValuePair<int, string>> LevelList { get; set; }

        [Display(Name = "Level")]
        public int LevelOPId { get; set; }
        public List<KeyValuePair<int, string>> LevelOPList { get; set; }


        [Display(Name = "Category")]
        public int SubCategoryId { get; set; }
        public List<KeyValuePair<int, string>> SubCategoryList { get; set; }



        public List<KeyValuePair<int, string>> AccountTypeList { get; set; }

        public List<KeyValuePair<int, string>> ChargeTypeList { get; set; }

        public List<EmployeeModel> DoctorList { get; set; }

        public List<CategoryModel> ServiceList { get; set; }

        public List<CategoryModel> CategoryList { get; set; }

        public List<CompanyModel> CompanyList { get; set; }




    }
}