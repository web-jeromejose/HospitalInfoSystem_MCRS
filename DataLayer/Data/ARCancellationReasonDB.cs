using DataLayer.Common;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class ARCancellationReasonDB
    {
        DBHelper DB = new DBHelper("AROPBILLING");
        ExceptionLogging eLOG = new ExceptionLogging();
        public string ErrorMessage { get; private set; }
        public List<ARCancellationReasonModel> get_ar_cancellation_reasons()
        {
            try
            {
                return DB.ExecuteSPAndReturnDataTable("aradmin.get_ar_cancellation_reason_list").DataTableToList<ARCancellationReasonModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public bool save_ar_cancellation_reasons(string desc, string code, int tid, int opeid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@act", 1),
                    new SqlParameter("@desc", desc),
                    new SqlParameter("@code", code),
                    new SqlParameter("@tid", tid),
                    new SqlParameter("@opeid",opeid),
                    new SqlParameter("@canid",1) //just for requirements
                };
                DB.ExecuteSPAndReturnDataTable("aradmin.proc_ar_cancellation_reasons");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                eLOG.LogError(ex);
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public bool update_ar_cancellation_reasons(string desc, string code, int tid, int opeid, int canid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                     new SqlParameter("@act", 2),
                    new SqlParameter("@desc", desc),
                    new SqlParameter("@code", code),
                    new SqlParameter("@tid", tid),
                    new SqlParameter("@opeid",opeid),
                    new SqlParameter("@canid",canid)
                };
                DB.ExecuteSPAndReturnDataTable("aradmin.proc_ar_cancellation_reasons");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                eLOG.LogError(ex);
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public bool delete_ar_cancellation_reasons(int canid, int opeid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                     new SqlParameter("@act", 3),
                    new SqlParameter("@desc", String.Empty),
                    new SqlParameter("@code", String.Empty),
                    new SqlParameter("@tid", 1),
                    new SqlParameter("@opeid",opeid),
                    new SqlParameter("@canid",canid)
                };
                DB.ExecuteSPAndReturnDataTable("aradmin.proc_ar_cancellation_reasons");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                eLOG.LogError(ex);
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
    }
}
