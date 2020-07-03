using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class MRAramcoPatient
    {
        public string From { get; set; }

        public string GenderName { get; set; }

        public string To { get; set; }
        public int SexID { get; set; }

        public List<Sex> Gender {get;set;}
    }
}