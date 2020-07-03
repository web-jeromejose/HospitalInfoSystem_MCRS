using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace DataLayer.Model
{
    public class ARUCAFBatchPrintRecord
    {
        public int      VisitId {get; set;}

        public string   Pin {get; set;}
        [Display(Name="Name")]
        public string   PatientName {get; set;}
        [Display(Name="Type")]
        public string   VisitType {get; set;}
        [Display(Name="Doctor")]
        public string   DoctorName {get; set;}
        [Display(Name="Code")]
        public string   CompanyCode {get; set;}
        [Display(Name="Date")]
        public DateTime DateTime {get; set;}
       
    }
}
