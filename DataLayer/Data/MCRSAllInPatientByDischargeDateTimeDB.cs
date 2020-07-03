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
    public class MCRSAllInPatientByDischargeDateTimeDB
    {

        DBHelper dbhelper = new DBHelper("MCRSAllInPatientByDischargeDateTimeDB");
        public DataTable getAllinPatientByDischargeTime(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@StartDate",StartDate),
                        new SqlParameter("@EndDate",EndDate),
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[Generate_getAllPatientByDischargeTime]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }
    }
}
