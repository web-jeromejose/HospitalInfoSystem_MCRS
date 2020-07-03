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
    public class OtherReportsDB
    {

        DBHelper dbhelper = new DBHelper("OtherReportsDB");
        public int ret = 0;
        public string retmsg = "";

        public DataTable getStartEndDateandSP(DateTime StartDate, DateTime EndDate, String SchemaAndSPNAme)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable(SchemaAndSPNAme);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getEvalMonitr(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[]{
                   // new SqlParameter("@pin", pin),
                    new SqlParameter("@stDate", StartDate),
                    new SqlParameter("@enDate", EndDate),
                   // new SqlParameter("@retcode", SqlDbType.Int),
                   // new SqlParameter("@retmsg", SqlDbType.VarChar, 100)
                };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[HumanResources_GetEvaluationMonitorList]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }
        public DataTable getIndividualEmplEvaluation(string xip)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[]{
                   // new SqlParameter("@pin", pin),
                    new SqlParameter("@xip", xip.ToString()),
                   // new SqlParameter("@retcode", SqlDbType.Int),
                   // new SqlParameter("@retmsg", SqlDbType.VarChar, 100)
                };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[HumanResources_GetIndividualEmplEval]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public List<HRDEvaluationMonitorModel> get_evaluation_monitor(DateTime fdate, DateTime tdate)
        {
            try
            {

                dbhelper.param = new SqlParameter[]{
                   // new SqlParameter("@pin", pin),
                    new SqlParameter("@stdate", fdate),
                    new SqlParameter("@endate", tdate),
                   // new SqlParameter("@retcode", SqlDbType.Int),
                   // new SqlParameter("@retmsg", SqlDbType.VarChar, 100)
                };


                List<HRDEvaluationMonitorModel> DD = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[HumanResources_EvaluationMonitor]").DataTableToList<HRDEvaluationMonitorModel>();

                this.ret = 0;
                this.retmsg = "Test Success";
                return DD;
            }
            catch (Exception ex)
            {

                ret = 0;
                retmsg = "Data Successfully Modified!";
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }



        public DataTable getCurrentlyAdmittedCashPatients(DateTime StartDate, DateTime EndDate, string option)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),
                        new SqlParameter("@option",option)

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[OtherReports_GetIPPackageNew]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getEodStatSummary(DateTime StartDate, DateTime EndDate, string option)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),
                        new SqlParameter("@optional",option)

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[OtherReports_GetEODStats]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }
        public DataTable getOPCancelledBills(DateTime StartDate, DateTime EndDate, string option)
        {
            var dataTable = new DataTable();
            try
            {

                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@StartDate",StartDate),
                        new SqlParameter("@EndDate",EndDate),
                        new SqlParameter("@Category",option)

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[OtherReports_OPCancelledBills]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getDailyCancellationReport(DateTime StartDate, DateTime EndDate, string option)
        {
            var dataTable = new DataTable();
            try
            {

                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@StartDate",StartDate),
                        new SqlParameter("@EndDate",EndDate),
                        new SqlParameter("@option",option)

                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[OtherReports_DailyCancelledReports]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getRamadanIncome(DateTime StartDate, DateTime EndDate, string option)
        {
            var dataTable = new DataTable();
            try
            {

                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),
                        new SqlParameter("@option",option)

                    };


                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[OtherReports_RamadanIncome]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }
        public DataTable getAmcDailyCollection(DateTime StartDate, DateTime EndDate, int StationId)
        {
            var dataTable = new DataTable();
            try
            {

                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@StartDate",StartDate),
                        new SqlParameter("@EndDate",EndDate),
                        new SqlParameter("@StationId",StationId),
                         new SqlParameter("@DepartmentId",163) //amc dpartment id? based on the OLd sp

                    };



                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[OtherReports_AmcDailyCollectionReport]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public bool updatePharmacyBillsApprover(string employeeId, string billno)
        {
            bool success = false;
            try
            {
                var dataTable = new DataTable();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append(" Select * from canopcompanybilldetail ");
                queryBuilder.Append("where billno = '" + billno + "' ");

                dataTable = dbhelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString());
                if (dataTable.Rows.Count > 0)
                {

                    success = dbhelper.ExecuteSQLNonQuery(" Update canopcompanybilldetail set canEMPLOYEEID =  '" + employeeId + "' WHERE billno = '" + billno + "'");

                }
                else
                {
                    return success;
                }

            }

            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return success;
        }

        public DataTable getARAdjustmentDetailsOp(DateTime StartDate, DateTime EndDate, int categoryId)
        {
            var dataTable = new DataTable();
            try
            {

                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),
                        new SqlParameter("@catid",categoryId),
       

                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_ARAdjustmentOP]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getIPDischarge(DateTime StartDate, DateTime EndDate, string Option)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[]{
                   // new SqlParameter("@pin", pin),
                    new SqlParameter("@FromDate", StartDate),
                    new SqlParameter("@ToDate", EndDate),
                    new SqlParameter("@Option", Option),
                   // new SqlParameter("@retmsg", SqlDbType.VarChar, 100)
                };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_getIPDischarge]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getReportHeader2018()
        {
            var dataTable = new DataTable();

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(" select BranchName ,Logo,WaterMark from aradmin.ReportHeader ");

            dataTable = dbhelper.ExecuteSQLAndReturnDataTable(queryBuilder.ToString());

            return dataTable;
        }

        public DataTable getIPDischargeVersion2(DateTime StartDate, DateTime EndDate, string Option)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[]{
                   // new SqlParameter("@pin", pin),
                    new SqlParameter("@FromDate", StartDate),
                    new SqlParameter("@ToDate", EndDate),
                    new SqlParameter("@Option", Option),
                   // new SqlParameter("@retmsg", SqlDbType.VarChar, 100)
                };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_getIPDischarge_VERSION2]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


    }
}
