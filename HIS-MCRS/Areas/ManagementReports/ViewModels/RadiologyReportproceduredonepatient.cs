using HIS_MCRS.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class RadiologyReportproceduredonepatient
    {

       

        public int InPatient { get; set; }

        public int RegistrationNo { get; set; }
        public string Name { get; set; }

        public string OrderNo { get; set; } //list

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        public string Procedure { get; set; }//list

        public string Technician { get; set; }
        public string Equipment { get; set; }


        [Display(Name = "Visit Date")]
        public DateTime VisitDate { get; set; }

        public string RoomTimeIn { get; set; }
        public string RoomTimeOut { get; set; }

        public string ProcedureStation { get; set; }



    }
}