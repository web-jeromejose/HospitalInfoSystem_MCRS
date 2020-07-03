using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Model;
using DataLayer.Model.Common;
using System.ComponentModel.DataAnnotations;

namespace HIS_MCRS.Areas.ManagementReports.ViewModels
{
    public class PRListOfEmployees
    {
       [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        public DateTime EndDate { get; set; }

        [Display(Name = "HR Category")]
        public int HrId { get; set; }

        [Display(Name = "Dept Category")]
        public int DeptId { get; set; }

        [Display(Name = "Gender")]
        public int GenderId { get; set; }

        
        [Display(Name = "Nationality")]
        public string NationalityId { get; set; }

        public bool IsWithSalary { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        
        public int EmployeeId { get; set; }

        public List<EmployeeModel> EmployeeList { get; set; }

        public List<EmployeeModel> DepartmentList { get; set; }

        public List<DepartmentModel> StaffDepartmentList { get; set; }


        public List<GenericListModel> NationalityList { get; set; }

        public List<Station> HrList { get; set; }

        //public List<Sex> GenderList { get; set; }
        public List<KeyValuePair<int, string>> GenderList
        {
            get
            {
                var options = new List<KeyValuePair<int, string>>(){
                  new KeyValuePair<int, string> (0, "ALL"),
                  new KeyValuePair<int, string> (1, "FEMALE"),
                  new KeyValuePair<int, string> (2, "MALE")
                
                };


                return options;
            }
        }

        public List<KeyValuePair<int, string>> DeptList
        {
            get
            {
                var options = new List<KeyValuePair<int, string>>(){
                  new KeyValuePair<int, string> (2, "ALL"),
                  new KeyValuePair<int, string> (0, "SGH"),
                  new KeyValuePair<int, string> (1, "Non-SGH")
                
                };


                return options;
            }
        }
        public List<KeyValuePair<int, string>> CategoryList
        {
            get
            {
                var options = new List<KeyValuePair<int, string>>(){
                  new KeyValuePair<int, string> (3, "ALL"),
                  new KeyValuePair<int, string> (1, "Medical"),
                  new KeyValuePair<int, string> (2, "Non-Medical")
                
                };


                return options;
            }
        }


    }
}