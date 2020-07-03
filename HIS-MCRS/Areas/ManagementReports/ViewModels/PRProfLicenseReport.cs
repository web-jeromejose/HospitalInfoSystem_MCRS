using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using DataLayer.Model.Common;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class PRProfLicenseReport
    {
        [Display(Name = "License")]
        public int LicenseId { get; set; }

        [Display(Name = "Group By")]
        public int GroupById { get; set; }

         [Display(Name = "HR Category")]
        public int HrCategoryId { get; set; }

         public List<DepartmentModel> HrCategoryList { get; set; }
       
        public List<KeyValuePair<int, string>> LicenseList
        {
            get
            {
                var options = new List<KeyValuePair<int, string>>(){
                  new KeyValuePair<int, string> (1, "MOH"),
                  new KeyValuePair<int, string> (2, "Saudi Council"),
                  new KeyValuePair<int, string> (3, "Temporary License")
                
                };


                return options;
            }
        }


        public List<KeyValuePair<int, string>> GroupByList
        {
            get
            {
                var options = new List<KeyValuePair<int, string>>(){
                  new KeyValuePair<int, string> (0, "Category"),
                  new KeyValuePair<int, string> (1, "Department"),
                  
                
                };


                return options;
            }
        }



    }
}