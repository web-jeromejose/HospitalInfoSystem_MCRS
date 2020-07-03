using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class OtherReports_AmcDailyCollectionReport
    {
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Station")]
        public int StationId { get; set; }

        public List<Station> StationList { get; set; }

    }
}