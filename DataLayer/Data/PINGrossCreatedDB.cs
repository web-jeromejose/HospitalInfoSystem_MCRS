using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class PINGrossCreatedDB
    {
        DBHelper DB = new DBHelper();
        public List<PINGrossCreatedModel> get_pingross_created(long pin, string fdate, string tdate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@pin", pin),
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.pin_gross_created")
                    .DataTableToList<PINGrossCreatedModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
    }
}
