using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model.Common
{
    public class CommonDropdownModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class CommonDropdownServerModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
    public class PTCommonInfoModel
    {
        public string PTName { get; set; }
        public string AgeT { get; set; }
    }
}
