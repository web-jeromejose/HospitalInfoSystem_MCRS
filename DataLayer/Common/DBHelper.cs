using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Configuration;
using System.Diagnostics;
using Oracle.DataAccess.Client;
using DataLayer.Common;

namespace DataLayer
{
    public class DBHelper
    {
        ExceptionLogging eLOG = new ExceptionLogging();
        string connString = "";
        string connStringORA = "";
        string connStringBI = "";
        string connStringEOD = "";
        public SqlParameter[] param = null;

        public Boolean Successful { get; private set; }
        public string ErrorMessage { get; private set; }
        public string ErrorStackTrace { get; private set; }
        public string decString { get; private set; }
        public string SqlConnectionString { get; private set; }
        string _ModuleName;

        public DBHelper(string ModuleName = "SGH_HIS")
        {
            EncryptDecrypt enc = new EncryptDecrypt();

            connString = enc.Decrypt(ConfigurationManager.ConnectionStrings["SghDbContextConnString"].ConnectionString, true);
            connStringORA = enc.Decrypt(ConfigurationManager.ConnectionStrings["ORAConnection"].ConnectionString, true);

            //connString = "Password =WHCIT;Persist Security Info=True;User Id=WHCIT;Initial Catalog=HIS;Data Source=130.1.2.90";
            connStringBI = enc.Decrypt(ConfigurationManager.ConnectionStrings["BIConnection"].ConnectionString, true);
            connStringEOD = enc.Decrypt(ConfigurationManager.ConnectionStrings["EodConnection"].ConnectionString, true);

            //for not encrpt data
            //connString = ConfigurationManager.ConnectionStrings["SghDbContextConnString"].ConnectionString + ";Application Name=SGHHIS";            

            SqlConnectionString = connString + ";Application Name=SGHHIS";
            _ModuleName = ModuleName;
        }

        public string ExecuteSQLScalar(string sql = "")
        {
            using (SqlConnection CN = new SqlConnection(connString))
            {
                try
                {
                    CN.Open();

                    DataTable dt = new DataTable();
                    this.Successful = true;
                    using (SqlCommand cmd = new SqlCommand(sql, CN))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        return cmd.ExecuteScalar().ToString();
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);
                    EventLog log = new EventLog();
                    log.Source = _ModuleName;
                    log.WriteEntry("<b>From:</b> Query<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                    //return false;
                }
            }
        }

        public Boolean ExecuteSQLNonQuery(string sql = "")
        {
            using (SqlConnection CN = new SqlConnection(connString))
            {
                try
                {
                    CN.Open();

                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(sql, CN))
                    {
                        this.Successful = true;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        bool success = cmd.ExecuteNonQuery() != 0;
                        return success;
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);
                    EventLog log = new EventLog();
                    log.Source = _ModuleName;
                    log.WriteEntry("<b>From:</b> Query<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                    //return false;
                }
            }
        }

        public Boolean ExecuteSQL(string sql = "")
        {
            using (SqlConnection CN = new SqlConnection(connString))
            {
                try
                {
                    CN.Open();

                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(sql, CN))
                    {
                        this.Successful = true;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);
                    EventLog log = new EventLog();
                    log.Source = _ModuleName;
                    log.WriteEntry("<b>From:</b> Query<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                    //return false;
                }
            }
        }

        public Boolean ExecuteSP(string spName = "")
        {
            using (SqlConnection CN = new SqlConnection(connString))
            {
                try
                {
                    CN.Open();

                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(spName, CN))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300;
                        if (param != null)
                        {
                            foreach (SqlParameter item in param)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }

                        this.Successful = true;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);
                    EventLog log = new EventLog();
                    log.Source = _ModuleName;
                    log.WriteEntry("<b>From:</b> " + spName + "<br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("<b>From:</b> " + spName + "<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                    //return false;
                }
            }
        }

        public DataTable ExecuteSPAndReturnDataTableEODConnection(string SPName = "")
        {
            using (SqlConnection CN = new SqlConnection(connStringEOD))
            {
                try
                {
                    CN.Open();

                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(SPName, CN))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300;
                        if (param != null)
                        {
                            foreach (SqlParameter item in param)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }

                        this.Successful = true;
                        SqlDataReader rs = cmd.ExecuteReader();

                        dt.Load(rs);
                        rs.Close();
                        rs.Dispose();
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);

                    EventLog log = new EventLog();
                    log.Source = _ModuleName;
                    log.WriteEntry("<b>From:</b> " + SPName + "<br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("<b>From:</b> " + SPName + "<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                }
            }
        }


        public DataTable ExecuteSPAndReturnDataTable(string SPName = "")
        {
            using (SqlConnection CN = new SqlConnection(connString))
            {
                try
                {
                    CN.Open();

                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(SPName, CN))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        if (param != null)
                        {
                            foreach (SqlParameter item in param)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }

                        this.Successful = true;
                        SqlDataReader rs = cmd.ExecuteReader();

                        dt.Load(rs);
                        rs.Close();
                        rs.Dispose();
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);

                    EventLog log = new EventLog();
                    log.Source = _ModuleName;
                    log.WriteEntry("<b>From:</b> " + SPName + "<br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("<b>From:</b> " + SPName + "<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                }
            }
        }

        public DataTable ExecuteSQLAndReturnDataTable(string sql)
        {
            using (SqlConnection CN = new SqlConnection(connString))
            {
                try
                {
                    CN.Open();

                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(sql, CN))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;

                        this.Successful = true;
                        SqlDataReader rs = cmd.ExecuteReader();
                        dt.Load(rs);
                        rs.Close();
                        rs.Dispose();
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);
                    EventLog log = new EventLog();
                    log.Source = _ModuleName;
                    log.WriteEntry("<b>From:</b> " + sql + "<br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("<b>From:</b> " + sql + "<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                    //return null;
                }
            }
        }

        public DataSet ExecuteSPAndReturnDataSet(string SPName = "")
        {
            using (SqlConnection CN = new SqlConnection(connString))
            {
                try
                {
                    CN.Open();

                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(SPName, CN))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 3000;
                        if (param != null)
                        {
                            foreach (SqlParameter item in param)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }

                        this.Successful = true;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds;
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);
                    EventLog log = new EventLog();
                    log.Source = _ModuleName;
                    log.WriteEntry("<b>From:</b> " + SPName + "<br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("<b>From:</b> " + SPName + "<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                }
            }
        }

        /* Oracle ORA */
        public DataTable ExecuteQueryInORA(string sql)
        {
            using (OracleConnection oraCN = new OracleConnection(connStringORA))
            {
                try
                {
                    oraCN.Open();
                    using (OracleCommand oraCMD = new OracleCommand(sql, oraCN))
                    {
                        this.Successful = true;
                        OracleDataReader oraRS = oraCMD.ExecuteReader();
                        oraCMD.CommandType = CommandType.Text;
                        DataTable dt = new DataTable();
                        dt.Load(oraRS);
                        oraRS.Close();
                        oraRS.Dispose();
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);
                    EventLog log = new EventLog();
                    if (!EventLog.SourceExists("SGH MCRS"))
                        EventLog.CreateEventSource("SGH MCRS", "Application");

                    log.Source = "SGH MCRS";
                    log.WriteEntry("<b>From:</b> " + sql + "<br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("<b>From:</b> " + sql + "<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                }
            }
        }

        /* BI */
        public DataTable ExecuteSQLAndReturnDataTableBI(string sql)
        {
            using (SqlConnection CN = new SqlConnection(connStringBI))
            {
                try
                {
                    CN.Open();

                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(sql, CN))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;

                        this.Successful = true;
                        SqlDataReader rs = cmd.ExecuteReader();
                        dt.Load(rs);
                        rs.Close();
                        rs.Dispose();
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    eLOG.LogError(ex);
                    EventLog log = new EventLog();
                    log.Source = _ModuleName;
                    log.WriteEntry("<b>From:</b> " + sql + "<br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace, EventLogEntryType.Error);
                    this.Successful = false;
                    this.ErrorMessage = ex.Message;
                    this.ErrorStackTrace = ex.StackTrace;
                    throw new ApplicationException("<b>From:</b> " + sql + "<br/><br/><b>Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
                    //return null;
                }
            }
        }
    }
}
