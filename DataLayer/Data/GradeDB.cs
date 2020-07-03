using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class GradeDB
    {
        DBHelper dbHelper = new DBHelper("GradeDB");

        public List<GradeModel> getGradeByCompanyId(int companyId)
        {

            var grade = new List<GradeModel>();
            try
            {
                grade = dbHelper.ExecuteSQLAndReturnDataTable("SELECT Id,Name FROM grade WHERE companyid = " + companyId + " and deleted = 0 ORDER BY name").DataTableToList<GradeModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return grade;
        }
    }
}
