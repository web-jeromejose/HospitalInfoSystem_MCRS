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
    public class FamilyMedicineDB
    {

        DBHelper dbhelper = new DBHelper("FamilyMedicineDB");
        ExceptionLogging eLOG = new ExceptionLogging();


        public DataTable getPrintByPending()
        {

            //SP_Get_VaccinationbyDepartment_AllPending
            var dataTable = new DataTable();
            try
            {
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[FamilyMedicine_VaccinationDeptAllPending]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getPrintByAllDone()
        {

         //  SP_Get_VaccinationbyDepartment_NonPending
            var dataTable = new DataTable();
            try
            {

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[FamilyMedicine_VaccinationDepartAllDone]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }
         public DataTable getPrintByDepartment(string DeptId)
        {

            // SP_Get_VaccinationbyDepartment
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@deptid",DeptId)
                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[FamilyMedicine_VaccinationByDept]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        
    }
}
