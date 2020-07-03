using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class AdjustmentsDB
    {
        DBHelper dbHelper = new DBHelper("AdjustmentsDB");

        public DataTable getRevenueAdjustments(DateTime from, DateTime to, int billType, int categoryId, int companyId)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@fdate", from.ToString()),
                                   new SqlParameter("@tdate", to.ToString()),
                                   new SqlParameter("@billtype", billType.ToString()),
                                   new SqlParameter("@comid", companyId.ToString()),
                                   new SqlParameter("@catid", categoryId.ToString())
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("MCRS.generate_finance_revenue_adj_report");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

    }
}
