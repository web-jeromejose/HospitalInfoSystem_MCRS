using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataLayer.Model;

namespace DataLayer.Data
{
    public class TestDB
    {
        DBHelper dbHelper = new DBHelper("TestDB");

        public List<Test> getTestNameAndCode(string searchString)
        {
            var names = new List<Test>();
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append("SELECT  DISTINCT TOP 30  Code, Name FROM Test ");
                query.Append("WHERE Code + ' - ' + Name LIKE '%" + searchString + "%'");
                query.Append("ORDER BY Code, Name");
                names = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<Test>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return names;

        }
    }
}
