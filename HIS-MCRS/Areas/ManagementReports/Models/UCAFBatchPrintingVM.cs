using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class UCAFBatchPrintingVM
    {
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        public List<CategoryModel> Categories { get; set; }
        public List<SelectListItem> Doctors { get; set; }
        public List<ARUCAFBatchPrintRecord> ARUCAFRecords { get; set; }

        public string PrintSelectionMode { get; set; }
        public int PageStart { get; set; }
        public int PageEnd   { get; set; }

        public string SelectedCompaniesJson  { get; set; }
        public string ListOfSelectedVisitIds { get; set; }

    }
}