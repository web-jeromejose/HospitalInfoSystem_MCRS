using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace DataLayer.Data
{
   public class HumanResourcesDepartmentDB
    {
       DBHelper dbhelper = new DBHelper("HumanResourcesDepartmentDB");

        public DataTable getStaffContractDetails(int DeptId,int PosId)
        {
            //SP_GET_EMPCONTRACT_INFO  " & xDept & "," & xPos & "")

            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                       new SqlParameter("@xDept",DeptId),
                        new SqlParameter("@xPos",PosId),

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[HumanResourcesDept_GetStaffContractDetails]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public List<HRDEvaluationMonitorModel> get_VaccinationEntryForm(int EmpId, int DeptId)
        {
            try
            {

                dbhelper.param = new SqlParameter[]{
                  new SqlParameter("@empId", EmpId),
                  new SqlParameter("@deptId", DeptId),
                    //new SqlParameter("@endate", tdate),
                   // new SqlParameter("@retcode", SqlDbType.Int),
                   // new SqlParameter("@retmsg", SqlDbType.VarChar, 100)
                };


                List<HRDEvaluationMonitorModel> DD = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[HumanResources_GetVaccinationEntryForm]").DataTableToList<HRDEvaluationMonitorModel>();

                return DD;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<FMReport_VaccinationEntryFormModel> get_vaccinationperEmployee(int EmpId)
        {
            try
            {

                dbhelper.param = new SqlParameter[]{
                  new SqlParameter("@empId", EmpId),                
                };


                List<FMReport_VaccinationEntryFormModel> DD = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[FamilyMedicineReport_GetVaccinationEntryForm]").DataTableToList<FMReport_VaccinationEntryFormModel>();

                return DD;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<FMReport_GetVaccinationPendingModel> get_VaccinationSelectList(int xvID)
        {
            try
            {

                dbhelper.param = new SqlParameter[]{
                  new SqlParameter("@xvID", xvID),

                };

                List<FMReport_GetVaccinationPendingModel> DD = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[FamilyMedicineReport_GetVaccinationPending]").DataTableToList<FMReport_GetVaccinationPendingModel>();

                return DD;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable getIndividualEmplEval(int xip)
        {
                       var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                       new SqlParameter("@xip",xip),
           

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[HumanResourceDept_GetIndivEmpEval]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public List<HrVaccinationMasterFileModel> getAllVaccinationFile()
        {


            var vaccination = new List<HrVaccinationMasterFileModel>();
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append("select *  from MCRS_VaccinationFile where deleted = 0  ");
                vaccination = dbhelper.ExecuteSQLAndReturnDataTable(query.ToString()).DataTableToList<HrVaccinationMasterFileModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return vaccination;

        }

        //string test = "{\"Id\":1,\"ItemCode\":\"1\",\"ItemName\":\"MMR\",\"Serology\":true,\"Dose1\":1,\"Dose2\":true,\"Dose3\":true,\"Dose4\":true,\"Deleted\":0}";

        public DataTable saveOrUpdateVaccinationMasterFile(string Id, string ItemCode, string ItemName, string Serology, string Dose1, string Dose2, string Dose3, string Dose4, string Deleted, string IsInsert)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                       new SqlParameter("@id",Id),
                       new SqlParameter("@ItemCode",ItemCode),
                       new SqlParameter("@ItemName",ItemName),
                       new SqlParameter("@serology",(Serology == "True"  || Serology == "1" ? 1 : 0)),
                       new SqlParameter("@Dose1",(Dose1 == "True"  || Dose1 == "1"  ? 1 : 0)),
                       new SqlParameter("@Dose2",(Dose2 == "True"  || Dose2 == "1"  ? 1 : 0)),
                       new SqlParameter("@Dose3",(Dose3 == "True"  || Dose3 == "1"  ? 1 : 0)),
                       new SqlParameter("@Dose4",(Dose4 == "True"  || Dose4 == "1"  ? 1 : 0)),
                       new SqlParameter("@deleted",Deleted),
                       new SqlParameter("@isInserted",IsInsert),
           
// [MCRS].[HumanResources_saveUpdateVaccinationMasterFile]
//(  @id int ,@ItemCode int,@ItemName varchar(max),@serology int,@Dose1 int,@Dose2 int,@Dose3 int,@Dose4 int,@deleted int = '0',@isInserted int = '0'
//)
                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[HumanResources_saveUpdateVaccinationMasterFile]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getSavePerformanceEvalMapping(string visitIdJSONArray,int Dept)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[]{
                                   new SqlParameter("@jsonIds", visitIdJSONArray),
                                   new SqlParameter("@departmentId", Dept)
                                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[HumanResources_PerformanceEvaluationMapping]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }






    }
}
