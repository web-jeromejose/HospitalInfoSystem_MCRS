using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataLayer.Model;
namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class OperationTheatreSurgeryRecordSummary
    {   
        [Display(Name="Company")]
        public int CompanyId { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }
        [Display(Name = "Sort Option")]
        public int SortMode { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public bool IsWithQty { get; set; }

        public List<CompanyModel> Companies { get; set; }

        public List<DepartmentModel> Departments { get; set; }

        public List<KeyValuePair<int, string>> SortOptions {
           
            get{
                var options = new List<KeyValuePair<int, string>>(){
                    new KeyValuePair<int, string>(1, "By Department"),
                    new KeyValuePair<int, string>(2, "By Doctor")
                };
                return options;
            }
        }
    }
}