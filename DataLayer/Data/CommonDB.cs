using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Model.Common;
using System.Data.SqlClient;

namespace DataLayer.Data.Common
{
    public class CommonDB
    {
        DBHelper DB = new DBHelper("AROPBILLING");
        /***** Get Common List *****/
        public List<CommonDropdownModel> get_common_list(int id, int ctype)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@id", id),
                    new SqlParameter("@ctype", ctype) 
                };
                return DB.ExecuteSPAndReturnDataTable("aradmin.getcommondropdownlist").DataTableToList<CommonDropdownModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public List<CommonDropdownServerModel> get_common_list_server(int id, int ctype, string terms)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@id", id),
                    new SqlParameter("@ctype", ctype),
                    new SqlParameter("@terms", terms ?? string.Empty) 
                };
                return DB.ExecuteSPAndReturnDataTable("aradmin.getdropdownlistserver").DataTableToList<CommonDropdownServerModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public List<CommonDropdownModel> get_common_list_selected(int id, int ctype)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@id", id),
                    new SqlParameter("@ctype", ctype) 
                };
                return DB.ExecuteSPAndReturnDataTable("aradmin.getdropdownlistselected").DataTableToList<CommonDropdownModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public CommonDropdownModel get_cat_list(int id)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@id", id),
                };
                return DB.ExecuteSPAndReturnDataTable("aradmin.getcategoryoploadmin").DataTableToList<CommonDropdownModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public CommonDropdownModel get_gra_list(int comid, int catid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@comid", comid),
                    new SqlParameter("@catid", catid)
                };
                return DB.ExecuteSPAndReturnDataTable("aradmin.getgradeoploadmin").DataTableToList<CommonDropdownModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public PTCommonInfoModel get_com_PTName(long pin)
        {
            try
            {
                return DB
                    .ExecuteSQLAndReturnDataTable("SELECT UPPER(A.LastName) + ', ' + UPPER(A.Firstname) + ' ' + UPPER(A.MiddleName) + ' ' + UPPER(A.FamilyName) AS PTName, " +
                    "CONVERT(VARCHAR(10), A.Age) + ' ' + B.Name + ' / ' +  C.Name  AS AgeT " +
                    "FROM dbo.Patient A " + 
                    "LEFT JOIN dbo.AgeType B ON A.AgeType = B.Id " +
                    "LEFT JOIN dbo.Sex C ON A.Sex = C.ID "+
                    "WHERE A.registrationno = " + pin)
                    .DataTableToList<PTCommonInfoModel>()
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }


        public List<CommonDropdownModel> get_inv_accountpin(int reqtype, int invtype, string fdate, string tdate, long opeid, int catid, long comid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@reqtype", reqtype),
                    new SqlParameter("@invtype", invtype),
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate), 
                    new SqlParameter("@opeid", opeid), 
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@comid", comid) 
                };
                return DB.ExecuteSPAndReturnDataTable("aropbilling.get_invoice_acctpin").DataTableToList<CommonDropdownModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<CommonDropdownModel> get_inv_accountpin_ubf(int reqtype, int invtype, string fdate, string tdate, long opeid, int catid, long comid)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@reqtype", reqtype),
                    new SqlParameter("@invtype", invtype),
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate), 
                    new SqlParameter("@opeid", opeid), 
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@comid", comid) 
                };
                return DB.ExecuteSPAndReturnDataTable("aropbilling.get_invoice_acctpin_ubf").DataTableToList<CommonDropdownModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<CommonDropdownModel> get_cl_company(int catid, int btype, string fdate, string tdate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@btype", btype),
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate)
                };
                return DB.ExecuteSPAndReturnDataTable("aropbilling.get_covering_letter_company").DataTableToList<CommonDropdownModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<CommonDropdownModel> get_cl_refno(int catid, long comid, string fdate, string tdate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@catid", catid),
                    new SqlParameter("@comid", comid),
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate)
                };
                return DB.ExecuteSPAndReturnDataTable("aropbilling.get_cl_refno").DataTableToList<CommonDropdownModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

    }
}
