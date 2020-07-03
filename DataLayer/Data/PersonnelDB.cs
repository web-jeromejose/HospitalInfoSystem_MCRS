using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Model.Common;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.Data
{
   public class PersonnelDB
    {
        DBHelper dbHelper = new DBHelper();

        public List<Station> getAllHrCategory()
        {


            var departments = new List<Station>();
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append("select  Id,Name,'' as Code from hrcategory where deleted=0 ");
                departments = dbHelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<Station>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return departments;

        }
    
     
        public DataTable getEmployeebyCatALL(int CategoryId ,int DeptId,DateTime from, DateTime to, int Gender,string Nation)
        {
          //  ALTER Procedure [dbo].[SP_GetEmployeebyCategoryAll] (@xCatID int, @xDeptCat int, @stDate datetime, @enDate datetime, @xSex int, @Nation varchar(50))                          
           var dataTable = new DataTable();

            try
            {
     
                dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@xCatID",CategoryId),
                                    new SqlParameter("@xDeptCat", DeptId),
                                    new SqlParameter("@stDate", from.ToString()),
                                    new SqlParameter("@enDate", to.ToString()),
                                    new SqlParameter("@xSex",Gender),
                                    new SqlParameter("@Nation", (Nation == "ALL" ? "%%" : "'%"+Nation+"%'")),
                                 };
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[GetEmployeebyCatALL]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getEmployeebyCat(int CategoryId, int DeptId, DateTime from, DateTime to, int Gender, string Nation)
        {

            var dataTable = new DataTable();

            try
            {

                dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@xCatID",CategoryId),
                                   // new SqlParameter("@xDeptCat", DeptId),
                                    new SqlParameter("@stDate", from.ToString()),
                                    new SqlParameter("@enDate", to.ToString()),
                                    new SqlParameter("@xSex",Gender),
                                    new SqlParameter("@Nation", (Nation == "ALL" ? "%%" : "'%"+Nation+"%'")),
                                 };
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReport_GetEmployeebyCat]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }
         public DataTable getEmployeePassportDetail(int DeptId, DateTime from, DateTime to, int Gender)
        {

            var dataTable = new DataTable();

            try
            {
                //(@xCatID int, @xDeptCat int, @stDate datetime, @enDate datetime, @xSex int) 
                dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@xCatID",3),//3 - ALL 2-Nonmedical 1-medical
                                   // new SqlParameter("@xDeptCat", DeptId),
                                    new SqlParameter("@stDate", from.ToString()),
                                    new SqlParameter("@enDate", to.ToString()),
                                    new SqlParameter("@xSex",Gender),
                                      new SqlParameter("@xDeptCat",DeptId),
                                  
                                 };
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReport_EmployeeCategoryPassport]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }



         public DataTable getProfessionalLicense(int DeptId,int GroupById)
         {
             //        xDept = CType(cboDepartment.SelectedItem, ValueDescriptionPair).Value()

             //If xDept = 1 Then
             //    ViewReport("SP_Get_ProfessionalLicenseReport_MOH  ")
             //ElseIf xDept = 2 Then
             //    ViewReport("SP_Get_ProfessionalLicenseReport_SaudiCouncil  ")
             //Else
             //    ViewReport("SP_Get_ProfessionalLicenseReport_ALL  ")
             //End If
             var dataTable = new DataTable();


             try
             {


                
                 if (DeptId == 1)
                 {

                     dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReports_PRLicenseMOH]");
                 }
                 else if (DeptId == 2)
                 {

                     dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReports_ProfessionalLicense_SaudiCouncil]");
                 }
                 else
                 {
                     
                        dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReport_ProfessionalLicenseALL]");
                 }
                 

 
             }
             catch (Exception ex)
             {
                 throw new ApplicationException(Errors.ExemptionMessage(ex));
             }
             return dataTable;
         }
       
    
        public DataTable getDependentSummaryList()
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                
                                  
                                 };
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReports_DependentSummaryList]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public List<HRDEvaluationMonitorModel> get_dataHrCategory(int xvID)
        {
            try
            {

                dbHelper.param = new SqlParameter[]{
                  new SqlParameter("@xvID", xvID),

                };

                List<HRDEvaluationMonitorModel> DD = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReports_EmployeeExclusion]").DataTableToList<HRDEvaluationMonitorModel>();

                return DD;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }

        }

        public List<PersonnelReportModel> get_EmployeeInfoSheet(int deptId, int contractId)
        {
            try
            {

                dbHelper.param = new SqlParameter[]{
                  new SqlParameter("@deptId", deptId),
                  new SqlParameter("@contractId", contractId),
                    //new SqlParameter("@endate", tdate),
                   // new SqlParameter("@retcode", SqlDbType.Int),
                   // new SqlParameter("@retmsg", SqlDbType.VarChar, 100)
                };


                List<PersonnelReportModel> DD = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReports_GetEmployeeInfoSheet]").DataTableToList<PersonnelReportModel>();

                return DD;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }


        public DataTable getFamilyDependentList(int empid)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                
                                   new SqlParameter("@empid", empid),
                                 };
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReport_GetFamilyDependentList]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getEmpDetails(int empid)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                
                                   new SqlParameter("@empid", empid),
                                 };
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReport_GetEmployeeDetailsRpt]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getEmployeeDependeSheetBatch(string visitIdJSONArray)
        {
            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@visitIdJsonArray", visitIdJSONArray)
                                };
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReport_GetFamilyDependentListBatch]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }
        public DataTable getEmpDetailsperBatch(string visitIdJSONArray)
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@visitIdJsonArray", visitIdJSONArray)
                                };
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReport_GetEmployeeDetailsRptperBatch]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }


        public List<PRStaffDocMonitoring> getStaffDocMonitoring(DateTime stdate, DateTime endate, int empId, string deptId)
        {
            try
            {
                
                
                dbHelper.param = new SqlParameter[]{
                  new SqlParameter("@stdate", stdate),
                new SqlParameter("@endate",endate),
                new SqlParameter("@empId", empId),
                new SqlParameter("@deptId", deptId),

                };

                List<PRStaffDocMonitoring> DD = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReport_getStaffDocMonitoring]").DataTableToList<PRStaffDocMonitoring>();

                return DD;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }

        }

        public DataTable updateStaffDocMonitoring(string employeeid, string fullname, string deptcode, string name, string cv, string orient_dept, string orient_gen, string jd, string license, string educ_cert, string fs, string ifc, string tqm, string bcls, string acls, string eval_1, string eval_2, string eval_3, string eval_4, string confidentiality, string credentialing, string previledging)
 
        {

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@employeeid", employeeid.ToString()),
                                   new SqlParameter("@fullname", fullname),
                                   new SqlParameter("@deptcode", deptcode),
                                   new SqlParameter("@name", name),
                                   new SqlParameter("@cv", cv.ToString()),
                                   new SqlParameter("@orient_dept", orient_dept),
                                   new SqlParameter("@orient_gen", orient_gen),
                                   new SqlParameter("@jd", jd),
                                   new SqlParameter("@license", license),
                                   new SqlParameter("@educ_cert", educ_cert),
                                   new SqlParameter("@fs", fs),
                                   new SqlParameter("@ifc", ifc),
                                   new SqlParameter("@tqm", tqm),
                                   new SqlParameter("@bcls", bcls),
                                   new SqlParameter("@acls", acls),
                                   new SqlParameter("@eval_1", eval_1),
                                   new SqlParameter("@eval_2", eval_2),
                                   new SqlParameter("@eval_3", eval_3),
                                   new SqlParameter("@eval_4", eval_4),
                                    new SqlParameter("@confidentiality", confidentiality),
                                    new SqlParameter("@credentialing", credentialing),
                                    new SqlParameter("@previledging", previledging)

                                };
                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[PersonnelReport_StaffDocmonitoringSave]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

       

      

    }
}
