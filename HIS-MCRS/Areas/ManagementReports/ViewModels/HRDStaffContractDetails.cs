using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class HRDStaffContractDetails
    {
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Position")]
        public int PositionId { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

           [Display(Name = "Evaluation Date")]
        public int EvaluationDate { get; set; }

        public List<DepartmentModel> DepartmentList { get; set; }

        public List<EmployeeModel> PositionList { get; set; }

        public List<EmployeeModel> EmployeeList { get; set; }

        public List<HDREmployeeWiseModel> EvaluationDateList { get; set; }

        


    }
}