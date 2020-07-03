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
    public class HelpDeskDB
    {
        DBHelper dbhelper = new DBHelper("HelpDeskDB");
        public int ret = 0;
        public string retmsg = "";

        public DataTable getMagcard(DateTime StartDate, DateTime EndDate, string DeptIds, String SchemaAndSPNAme)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@Datefrom",StartDate),
                        new SqlParameter("@DateTo",EndDate),
                        new SqlParameter("@DeptIds",DeptIds),

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable(SchemaAndSPNAme);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

    }
}
