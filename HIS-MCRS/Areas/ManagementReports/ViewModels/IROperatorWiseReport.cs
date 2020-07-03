using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class IROperatorWiseReport
    {
        [Display( Name = "From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "To" )]
        public DateTime EndDate { get; set; }

         [Display(Name = "Operator")]
        public int OperatorId { get; set; } //cboOperator

         [Display(Name = "Breakup")]
        public bool Breakup { get; set; }
         [Display(Name = "Summary")]
        public bool Summary { get; set; }

        [Display(Name="Station")]
        public int StationId { get; set; }


        public string GenderName { get; set; }


        //public List<EmployeeModel> Operators { get; set; }

        public List<Station> Stations { get; set; }



    }
}