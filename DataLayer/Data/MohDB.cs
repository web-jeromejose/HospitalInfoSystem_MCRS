using DataLayer.Common;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
   
    public class MohDB
    {

        DBHelper dbHelper = new DBHelper("MohDB");

        public List<MohModel> getMOHCategories()
        {

            var category = new List<MohModel>();
            try
            {
                category = dbHelper.ExecuteSQLAndReturnDataTable("Select '0' AS 'Id','ALL' AS Name , '0' AS DELETED Union  Select * FROM MOH where deleted = 0 order by id").DataTableToList<MohModel>();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return category;
        }


    }
}
