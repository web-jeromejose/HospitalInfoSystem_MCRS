using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace DataLayer
{
    public class HISSecurity
    {
        DBHelper DB = new DBHelper("HIS");
        public string UserID { get; set; }
        public string ModuleID { get; set; }
        public string StationID { get; set; }
        public string FeatureID { get; set; }
        public List<HISSecurityModel> SecuritryFeatureCS()
        {
            try
            {
                DB.param = new SqlParameter[]{
                     new SqlParameter("UserID", UserID),
                     new SqlParameter("ModuleID", ModuleID)
                };
                return DB.ExecuteSPAndReturnDataTable("WARDS.EMPLOYEE_FEATURE").DataTableToList<HISSecurityModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public List<HISSecurityModelBackup> SecuritryFunctionCS()
        {
            try
            {
                DB.param = new SqlParameter[]{
                     new SqlParameter("UserID", UserID),
                     new SqlParameter("ModuleID", ModuleID),
                     new SqlParameter("FeatureID", FeatureID)
                     ,new SqlParameter("FunctionID", "0")
                };
//[HISGLOBAL].[EMPLOYEE_SECURITY]
//@UserID varchar(50),
//@ModuleID int,
//@FeatureID int,
//@FunctionID int

                return DB.ExecuteSPAndReturnDataTable("HISGLOBAL.EMPLOYEE_SECURITY").DataTableToList<HISSecurityModelBackup>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public List<HISMenuModel> MenuFunctionCS()
        {
            try
            {
                DB.param = new SqlParameter[]{
                     new SqlParameter("UserID", UserID),
                     new SqlParameter("ModuleID", ModuleID)
                };
                return DB.ExecuteSPAndReturnDataTable("HISGLOBAL.EMPLOYEE_FEATURE").DataTableToList<HISMenuModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        /* Added by : FHD 
        *  Adding layer of security for possible bug on back button and home redirect.    
        */
        public bool IsModuleAuthorized(string OID, string MOID)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@opeid", OID),
                    new SqlParameter("@modid", MOID),
                    new SqlParameter("@hasMA", SqlDbType.VarChar, 10)
                };

                DB.param[2].Direction = ParameterDirection.Output;
                HISModSecurity rs = DB.ExecuteSPAndReturnDataTable("WARDS.EMPLOYEE_MODULE2").DataTableToList<HISModSecurity>().FirstOrDefault();
                string CC = DB.param[2].Value.ToString();
                if (CC == "1") { return true; }
                else { return false; }
            }
            catch (Exception ex)
            {
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public bool IsFeatureAuthorized(string FID, string OID, string MOID)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@UserID", OID),
                    new SqlParameter("@ModuleID", MOID),
                    new SqlParameter("@FeatureID", FID),
                    new SqlParameter("@FeatCount", SqlDbType.VarChar, 10)
                };

                DB.param[3].Direction = ParameterDirection.Output;
                HISFeatSecurity rs = DB.ExecuteSPAndReturnDataTable("WARDS.EMPLOYEE_FEATURE2").DataTableToList<HISFeatSecurity>().FirstOrDefault();

                if (DB.param[3].Value.ToString() == "1") { return true; }
                else { return false; }
            }
            catch (Exception ex)
            {
                return false;
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
    }
    public class HISSecurityModel 
    {
        public string id { get; set; }
        public string Name { get; set; }
    }
    public class HISSecurityModelBackup
    {
        public string ModuleID { get; set; }
        public string FeatureID { get; set; }
        public string FunctionID { get; set; }
        public string PasswordTrans { get; set; }
     
    }
    public class HISFeatSecurity
    {
        public int FeatCount { get; set; }
    }

    public class HISModSecurity
    {
        public int ModCount { get; set; }
    }
    public class HISMenuModel
    {
        public string ID { get; set; }
        public string MenuName { get; set; }
        public string ParentName { get; set; }
        public string MenuURL { get; set; }
        public string ParentSequence { get; set; }
        public string MenuSequence { get; set; }
    }
}
