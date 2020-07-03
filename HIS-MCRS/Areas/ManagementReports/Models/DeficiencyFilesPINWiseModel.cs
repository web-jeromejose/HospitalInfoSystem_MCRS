using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HIS_MCRS.Areas.ManagementReports.Models
{
    public class DeficiencyFilesPINWiseModel
    {
        public bool IncludeStandards { get; set; }
        public string Standard { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Speciality { get; set; }
        public string Doctor { get; set; }
        public string PIN { get; set; }
        public bool ExcludeAdmission { get; set; }
        public bool IncludeParameter { get; set; }
        public string MonthFromDate { get; set; }
        public string YearFromDate { get; set; }
        public string MonthToDate { get; set; }
        public string YearToDate { get; set; }

        public class MonthDate
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        public class YearDate
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<YearDate> YearOptions;

        public IEnumerable<MonthDate> MonthOptions =
            new List<MonthDate>
        {
            new MonthDate {Id = 1, Value = "January"},
            new MonthDate {Id = 2, Value = "February"},
            new MonthDate {Id = 3, Value = "March"},
            new MonthDate {Id = 4, Value = "April"},
            new MonthDate {Id = 5, Value = "May"},
            new MonthDate {Id = 6, Value = "June"},
            new MonthDate {Id = 7, Value = "July"},
            new MonthDate {Id = 8, Value = "August"},
            new MonthDate {Id = 9, Value = "September"},
            new MonthDate {Id = 10, Value = "October"},
            new MonthDate {Id = 11, Value = "November"},
            new MonthDate {Id = 12, Value = "December"}
        };

    }
}