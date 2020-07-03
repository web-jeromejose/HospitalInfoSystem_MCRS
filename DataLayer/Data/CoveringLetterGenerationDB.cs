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
    public class CoveringLetterGenerationDB
    {
        DBHelper DB = new DBHelper("AROPBILLING");
        ExceptionLogging eLOG = new ExceptionLogging();
        public int retcode;
        public string retmsg;
        public bool generate_coveringletter(string fdate, string tdate, int catid, long comid, int btype)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate),
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@comid", comid),
                    new SqlParameter("@btype", btype),
                    new SqlParameter("@retcode", SqlDbType.Int),
                    new SqlParameter("@retmsg", SqlDbType.VarChar, 250),
                };

                DB.param[5].Direction = ParameterDirection.Output;
                DB.param[6].Direction = ParameterDirection.Output;
                DB.ExecuteSPAndReturnDataTable("aropbilling.generate_covering_letter");
                this.retcode = int.Parse(DB.param[5].Value.ToString());
                this.retmsg = DB.param[6].Value.ToString();
                return true;
            }
            catch (Exception ex)
            {
                this.retcode = -1;
                this.retmsg = ex.Message;
                eLOG.LogError(ex);
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public bool generate_coveringletter_batch(string fdate, string tdate, int catid, long comid, int btype, long opeid, string clip)
        {
            try
            {

                DB.param = new SqlParameter[]{
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate),
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@comid", comid),
                    new SqlParameter("@btype", btype),
                    new SqlParameter("@opeid", opeid),
                    new SqlParameter("@clip", clip),
                    new SqlParameter("@retcode", SqlDbType.Int),
                    new SqlParameter("@retmsg", SqlDbType.VarChar, 250),
                };

                DB.param[7].Direction = ParameterDirection.Output;
                DB.param[8].Direction = ParameterDirection.Output;
                DB.ExecuteSPAndReturnDataTable("aropbilling.generate_covering_letter_batch");
                this.retcode = int.Parse(DB.param[7].Value.ToString());
                this.retmsg = DB.param[8].Value.ToString();
                return true;
            }
            catch (Exception ex)
            {
                this.retcode = -1;
                this.retmsg = ex.Message;
                eLOG.LogError(ex);
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public CoveringLetterRetModel get_cl_progress(int catid, long opeid, string clip)
        { 
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@opeid", opeid),
                    new SqlParameter("@clip", clip)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.covering_letter_progress")
                    .DataTableToList<CoveringLetterRetModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public bool ungenerate_coveringletter(long refid, long opeid)
        {
            try
            {

                DB.param = new SqlParameter[]{
                    new SqlParameter("@refid", refid),
                    new SqlParameter("@opeid", opeid),
                    new SqlParameter("@retcode", SqlDbType.Int),
                    new SqlParameter("@retmsg", SqlDbType.VarChar, 250),
                };

                DB.param[2].Direction = ParameterDirection.Output;
                DB.param[3].Direction = ParameterDirection.Output;
                DB.ExecuteSPAndReturnDataTable("aropbilling.ungenerate_cl");
                this.retcode = int.Parse(DB.param[2].Value.ToString());
                this.retmsg = DB.param[3].Value.ToString();
                return true;
            }
            catch (Exception ex)
            {
                this.retcode = -1;
                this.retmsg = ex.Message;
                eLOG.LogError(ex);
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
    
    }
}
