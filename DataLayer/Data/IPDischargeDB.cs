using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
   public class IPDischargeDB
    {
        DBHelper dbHelper = new DBHelper("IPDischarge");

        public DataTable getIPDischargeStatement(DateTime startDate, DateTime endDate, int patientBillType)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@startDate", startDate),
                                    new SqlParameter("@endDate", endDate),
                                     new SqlParameter("@billType", patientBillType)
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReports_GET_IPDischargeStatement]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

       
    }
}
