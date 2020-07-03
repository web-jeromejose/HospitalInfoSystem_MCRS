using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class HRvaccinationFile
    {
        [Display(Name = "Item Code")]
        public int ItemCode { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Serology")]
        public int Serology { get; set; }

        [Display(Name = "1st Dose")]
        public int? Dose1 { get; set; }

        [Display(Name = "2nd Dose")]
        public int Dose2 { get; set; }

        [Display(Name = "3rd Dose")]
        public int Dose3 { get; set; }

        [Display(Name = "Booster Dose")]
        public int Dose4 { get; set; }
         
        public int Deleted { get; set; }

        public List<HrVaccinationMasterFileModel> VaccinationList { get; set; }

    }
}