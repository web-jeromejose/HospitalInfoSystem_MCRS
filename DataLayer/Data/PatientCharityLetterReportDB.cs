using DataLayer.Common;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class PatientCharityLetterReportDB
    {
        DBHelper DB = new DBHelper("AROPBILLING");
        ExceptionLogging eLOG = new ExceptionLogging();
        public List<PatientCharityLetterReportModel> get_ptcharityletterreport(string fdate, string tdate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("aropbilling.get_pt_charity_letter_report")
                    .DataTableToList<PatientCharityLetterReportModel>();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
    }
}
