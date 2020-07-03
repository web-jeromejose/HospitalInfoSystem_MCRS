using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Common;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.Data
{
   public class InfectionControlDB
    {
       DBHelper DB = new DBHelper();

        public DataTable MOHHBVReport(string StartDate, string EndDate)
        {
            var dataTable = new DataTable();
            try
            {
               // StringBuilder strb = new StringBuilder();
              //  strb.Append("SP_Alos_Diagnosis '" + StartDate + "','" + EndDate + "','" + diagnosis + "','" + lengthofstay + "'");
               // return DB.ExecuteSQLAndReturnDataTable(strb.ToString());

                DB.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", StartDate.ToString()),
                                   new SqlParameter("@enDate", EndDate.ToString()),
                                 };
                dataTable = DB.ExecuteSPAndReturnDataTable("[MCRS].[InfectionControl_MOHStatistics]");
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }


    }
}
