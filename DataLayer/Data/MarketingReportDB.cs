using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DataLayer.Common;
using DataLayer.Model;

namespace DataLayer.Data
{
    public class MarketingReportDB
    {
        DBHelper dbHelper = new DBHelper("MarketingReportDB");

        public DataTable getAramcoPatient(string From, string To, int sexId)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                   // new SqlParameter("@StartDate",From.ToString("dd-MMM-yyyy")),
                   new SqlParameter("@stAge",From.ToString()),
                    new SqlParameter("@enAge",To.ToString()),
                    new SqlParameter("@nSex",sexId)
                };
          

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[MarketingReport_GetAramcoPatientByAge]");



            }catch(Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getNoOfCashVisit(DateTime From , DateTime To , int NoOfVisit)
        {
            var dataTable = new DataTable();

            try 
            {
                dbHelper.param = new SqlParameter[] { 
                new SqlParameter("@stDate", From.ToString()),
                new SqlParameter("@enDate",To.ToString()),
                new SqlParameter("@xNoVisit",NoOfVisit)
                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[MarketingReport_GetCashPatientNoVisit]");
            
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;

        }
    }
}
