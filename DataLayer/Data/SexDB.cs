using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
   public class SexDB
    {
        DBHelper dbHelper = new DBHelper("SexDB");

        public List<Sex> getSex()
        {
            var sex = new List<Sex>();
            try
            {
                sex = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Name FROM SEX").DataTableToList<Sex>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return sex;

        }
    }
}
