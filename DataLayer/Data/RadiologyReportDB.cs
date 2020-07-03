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
    public class RadiologyReportDB
    {
        DBHelper dbhelper = new DBHelper("RadiologyReportDB");
        ExceptionLogging eLOG = new ExceptionLogging();


        public DataTable getXrayReferral(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[RadiologyReport_GetXrayReferral]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getXrayProcedure(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[RadiologyReports_GetXrayProcedure]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }
        public DataTable getAnesthesia(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[RadiologyReports_GetAnesthesiaReport]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        

        
    }
}
