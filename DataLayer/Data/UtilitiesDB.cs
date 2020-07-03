using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class UtilitiesDB
    {
        DBHelper dbHelper = new DBHelper("UtilitiesDB");

        public void fixedHeaderTotalForMCRS(DateTime startDate)
        {
           
          
            try
            {
                dbHelper.param = new SqlParameter[] { new SqlParameter("@FromDate", startDate) };
           
                dbHelper.ExecuteSP("[Utilities].[ERP_Lock_CoveringLetter_For_MCRS]");
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            
        }
    }
}
