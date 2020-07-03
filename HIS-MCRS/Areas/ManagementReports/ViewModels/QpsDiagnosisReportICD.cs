using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataLayer.Model;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class QpsDiagnosisReportICD
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        public string InPatient { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }


        public List<DepartmentModel> Department { get; set; }

        public List<CategoryModel> Category { get; set; }

        public List<CompanyModel> Company { get; set; }



    }
}