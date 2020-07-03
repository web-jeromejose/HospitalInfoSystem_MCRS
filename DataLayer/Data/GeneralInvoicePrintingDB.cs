using DataLayer.Common;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class GeneralInvoicePrintingDB
    {
        DBHelper DB = new DBHelper("ARIPBILLING");
        ExceptionLogging eLOG = new ExceptionLogging();
        public int retcode;
        public string retmsg;

        public List<ADMITDTListModel> get_AdmitDT_List(long pin)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@pin", pin)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_nonpack_admitdate")
                    .DataTableToList<ADMITDTListModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<BListModel> get_Bill_List(int ispack, string billno)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@ispack", ispack),
                    new SqlParameter("@billno", billno)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_bill_list_npd")
                    .DataTableToList<BListModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<BListModel> get_Bill_List_Batch(int ispack, int catid, long comid, string fdate, string tdate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@ispack", ispack),
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@comid", comid),
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_billlist_npd_batch")
                    .DataTableToList<BListModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
                
        }

         public List<GEAccountList> get_AccountList(int rtype, int catid, string fdate, string tdate) {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@rtype", rtype),
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aripbilling.get_account_list")
                    .DataTableToList<GEAccountList>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
                
        }

        

    }
}
