using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using DataLayer.Model.Common;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class PersonnelReportsVM
    {
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public List<DepartmentModel> DepartmentList { get; set; }

        [Display(Name = "Contract Type")]
        public int ContractType { get; set; }
        public List<DepartmentModel> ContractTypeList { get; set; }


    }
}