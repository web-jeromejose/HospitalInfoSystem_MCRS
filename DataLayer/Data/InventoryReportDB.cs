using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
   public class InventoryReportDB
    {
       DBHelper dbhelper = new DBHelper("InventoryReportDB");

       public DataTable getIROperatorWiseReport(DateTime startDate,DateTime endDate, int Operator,bool summary, int station)
       {
           var dataTable = new DataTable();

           if (summary)
           {

               try
               {
                   dbhelper.param = new SqlParameter[] { 
                                    new SqlParameter("@stdate", startDate.ToString()),
                                    new SqlParameter("@endate", endDate.ToString()),
                                    new SqlParameter("@StationID", station)

               };
                   dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[InventoryReport_GetOperatorWise]");
               }
               catch (Exception ex) { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

           }
           else
           {
               try
               {
                   dbhelper.param = new SqlParameter[] { 
                                    new SqlParameter("@stdate", startDate.ToString()),
                                    new SqlParameter("@endate", endDate.ToString()),
                                    new SqlParameter("@StationID", station),
                                    new SqlParameter("@operatorID",Operator)

               };
                   
                   dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[InventoryReport_GetOperatorWiseByBreakUpList]");
               }
               catch (Exception ex) { throw new ApplicationException(Errors.ExemptionMessage(ex)); }
           }
         
           return dataTable;
       }
    }
}
