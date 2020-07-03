using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class AramcoScreeningModel
    {


        public string Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int StartingAge { get; set; }
        public int EndingAge { get; set; }
        public int Deleted { get; set; }

    }
}
