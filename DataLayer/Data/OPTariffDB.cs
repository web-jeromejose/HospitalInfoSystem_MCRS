using DataLayer.Common;
using DataLayer.Model;
using DataLayer.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class OPTariffDB
    {
        DBHelper DB = new DBHelper("AROPBILLING");
        ExceptionLogging eLOG = new ExceptionLogging();
        public List<CommonDropdownModel> get_tariff_Department(string mtbl)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@mtbl", mtbl),
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aradmin.gettariffdept")
                    .DataTableToList<CommonDropdownModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public List<OPTariffModel> get_Tariff_Price(int tid, int dept, string mtbl)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@tid", tid),
                    new SqlParameter("@dep", dept),
                    new SqlParameter("@mtbl", mtbl)
                };
                return DB.ExecuteSPAndReturnDataTable("aradmin.gettariffpriceop").DataTableToList<OPTariffModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
    }
}
