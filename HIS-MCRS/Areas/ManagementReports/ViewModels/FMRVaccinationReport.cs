using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;
using HIS_MCRS.Enumerations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class FMRVaccinationReport
    {

        public int EmployeeId { get; set; }

        public string DepartmentId { get; set; }
        
        public List<DepartmentModel> DepartmentList { get; set; }

    }
}