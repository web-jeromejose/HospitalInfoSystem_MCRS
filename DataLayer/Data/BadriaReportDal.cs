 using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Common;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.Data
{
   public class BadriaReportDal
    {
       DBHelper DB = new DBHelper();

       public DataTable Beverly_CreatedDailyReport(string StartDate, string EndDate,string Docid)
        {
            var dataTable = new DataTable();
            try
            {
                DB.param = new SqlParameter[]{
                                   new SqlParameter("@DateFrom", StartDate.ToString()),
                                   new SqlParameter("@DateTo", EndDate.ToString()),
                                   new SqlParameter("@docid", Docid.ToString()),
                                 };
                dataTable = DB.ExecuteSPAndReturnDataTable("[MCRS].[Badria_getDoctorCommission_Daily]");
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }



       public DataTable Beverly_CreatedDailyReportDetails(string StartDate, string EndDate, string Docid)
       {
           var dataTable = new DataTable();
           try
           {
               DB.param = new SqlParameter[]{
                                   new SqlParameter("@DateFrom", StartDate.ToString()),
                                   new SqlParameter("@DateTo", EndDate.ToString()),
                                   new SqlParameter("@docid", Docid.ToString()),
                                 };
               dataTable = DB.ExecuteSPAndReturnDataTable("[MCRS].[Badria_getDoctorCommission_Daily_Details]");
               return dataTable;
           }
           catch (Exception ex)
           {
               throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
           }
       }

        public List<RoleModel> GetDocList()
        {
            return DB.ExecuteSQLAndReturnDataTable(" select 0 as id, 'All' as text , 'All' as name  union select id,cast(Empcode as varchar(max))+'-'+  name as text ,name from doctor where deleted = 0 order by Name ").DataTableToList<RoleModel>();
        }



    }

   public class Beverly_CreatedDailyReportVM
   {
       public DateTime txtFromDate { get; set; }
       public DateTime txtToDate { get; set; }
       public string options { get; set; }
       public string sel2doctor { get; set; }
   }

   public class RoleModel
   {
       public string id { get; set; }
       public string text { get; set; }
       public string name { get; set; }
   }

}
