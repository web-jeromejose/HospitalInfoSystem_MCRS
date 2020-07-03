using DataLayer.Common;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class ExpiredPoliciesDB
    {
        DBHelper DB = new DBHelper("AROPBILLING");
        ExceptionLogging eLOG = new ExceptionLogging();
        public List<ExpiredPoliciesModel> get_expired_policies(int rtype)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@rtype", rtype),
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.get_expired_policies")
                    .DataTableToList<ExpiredPoliciesModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
    }
}
