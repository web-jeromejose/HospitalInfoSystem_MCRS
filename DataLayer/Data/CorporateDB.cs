using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class CorporateDB
    {
        DBHelper dbHelper = new DBHelper("OPRevenue");

        public bool getCorporateLoadTable(DateTime startDate)
        {

            var dataTable = new DataTable();

            dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@StartDate", startDate)
                                };

            dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[CorporateReport_LOADTABLE]");

            return true;
        }

    }
}
