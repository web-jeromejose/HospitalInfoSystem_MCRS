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
    public class QpsReportDB
    {

        DBHelper dbhelper = new DBHelper("QpsReportDB");

        public DataTable getPatientOrDone(DateTime StartDate,DateTime EndDate)
        {  
            var dataTable = new DataTable();
                try
                {
                    dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),

                    };
                    dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetPatientOrDone]");
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(Errors.ExemptionMessage(ex));
                }

        return dataTable;
        }
        public DataTable getPatientAdmittedICU(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();
            try {
                dbhelper.param = new SqlParameter[] { 
                    new SqlParameter("@stdate",StartDate),
                    new SqlParameter("@endate",EndDate)
                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetPatientAdmittedInICU]");

            }
            catch (Exception ex) { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

            return dataTable;
        }
        public DataTable getPatientAdmittedErOR(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();
            try {
                dbhelper.param = new SqlParameter[] { 
                    new SqlParameter("@stdate",StartDate),
                    new SqlParameter("@endate",EndDate)
                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetPatientAdmittedEr]");
            }
            catch (Exception ex) { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

            return dataTable;
        }
        public DataTable getPatientVisitER(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();
            try {
                dbhelper.param = new SqlParameter[] { 
                    new SqlParameter("@stdate",StartDate),
                    new SqlParameter("@endate",EndDate)
                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetPatientVisitedEr]");
            }
            catch (Exception ex) { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

            return dataTable;
        }

        
        public DataTable getPatientListByAge(int AgeRangeID, string checkDate, DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();

            try
            {
                dbhelper.param = new SqlParameter[] { 
                    new SqlParameter("@stAgeID",AgeRangeID),
                    new SqlParameter("@enAgeID",AgeRangeID),
                    new SqlParameter("@xOption",checkDate == null ? '0' : '1' ),
                    new SqlParameter("@regSTDate",StartDate),
                    new SqlParameter("@regENDate",EndDate)
                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetPatientListByAge]");
            }
            catch (Exception ex) { throw new ApplicationException(Errors.ExemptionMessage(ex)); }
            return dataTable;
        }
        public DataTable getErConsultationStatistics(DateTime StartDate,DateTime EndDate)
        {
            var dataTable = new DataTable();

            try
            {
                dbhelper.param = new SqlParameter[] { 
                    new SqlParameter("@stdate", StartDate),
                    new SqlParameter("@endate",EndDate)
                                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetErConsultationStatistics]");

            }
            catch (Exception ex)
            { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

            return dataTable;
        }

        public DataTable getAvgLengthCriticalArea(DateTime StartDate,DateTime EndDate, int Dept,String Area)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] 
                                { 
                    new SqlParameter("@StartDate", StartDate),
                    new SqlParameter("@EndDate",EndDate),
                    new SqlParameter("@Dept",Dept),
                    new SqlParameter("@Area",Area)
                                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_AvglenghtStayCriticalArea]");

            }
            catch (Exception ex)
            { throw new ApplicationException(Errors.ExemptionMessage(ex)); }


            return dataTable;
        }
        public DataTable getMrdAdmissionDischargeStats(DateTime StartDate, DateTime EndDate,String Reptype)
        {
            var dataTable = new DataTable();

            try
            {
                dbhelper.param = new SqlParameter[] { 
                    new SqlParameter("@startdate", StartDate),
                    new SqlParameter("@enddate",EndDate),
                    new SqlParameter("@reptype",Reptype),

                                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetMrdAdmissionDischargeStats]");

             }
            catch (Exception ex)
            { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

            return dataTable;
        }

        public DataTable getXrayFromReferral(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();

            try
            {
                dbhelper.param = new SqlParameter[] { 
                    new SqlParameter("@stdate", StartDate),
                    new SqlParameter("@endate",EndDate)
                                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetXrayFromEr]");

            }
            catch (Exception ex)
            { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

            return dataTable;
        }

        public DataTable getPatientCriticalDiagnosis(DateTime EndDate,string stroption)
        {
            var dataTable = new DataTable();

            try
            {
                dbhelper.param = new SqlParameter[] { 
                 
                    new SqlParameter("@StartDate",EndDate),
                    new SqlParameter("@BackDate",stroption)
                                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetPatientCriticalDiagnosis]");

            }
            catch (Exception ex)
            { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

            return dataTable;
        }

        public DataTable getBedOccupancyFloorWise(DateTime StartDate, DateTime EndDate)
        {
            var dataTable = new DataTable();

            try
            {
                dbhelper.param = new SqlParameter[] { 
                    new SqlParameter("@StartDate", StartDate),
                    new SqlParameter("@EndDate",EndDate)
                                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetBedOccupancyFloorWise]");

            }
            catch (Exception ex)
            { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

            return dataTable;
        }

        public DataTable getPatientDiagnosisInOrOut(string isIPorOP ,int DepartmentId, int CategoryId,int CompanyId ,  DateTime StartDate,DateTime EndDate)
        {



            var dataTable = new DataTable();
            try {

                if (isIPorOP == "0")
                {
                                dbhelper.param = new SqlParameter[] { 
                                new SqlParameter("@DepartmentID", DepartmentId),
                                new SqlParameter("@CategoryID",CategoryId),
                                new SqlParameter("@CompanyID",CompanyId),
                                new SqlParameter("@DateFrom",StartDate),
                                new SqlParameter("@DateTo",EndDate)
                            };
                                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_PatientDiagnosisIC10_InPatient]");
                }
                else
                {
                    dbhelper.param = new SqlParameter[] { 
                                new SqlParameter("@DepartmentID", DepartmentId),
                                new SqlParameter("@CategoryID",CategoryId),
                                new SqlParameter("@CompanyID",CompanyId),
                                new SqlParameter("@DateFrom",StartDate),
                                new SqlParameter("@DateTo",EndDate)
                            };
                    dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetPatientDiagnosisICD10_OP]");


                }
            
            }
            catch (Exception ex)
            { throw new ApplicationException(Errors.ExemptionMessage(ex)); }
            return dataTable;
        }

        public DataTable getMedicalTowerCases(DateTime StartDate, DateTime EndDate, int Doctor)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] { 
                    new SqlParameter("@StartDate", StartDate),
                    new SqlParameter("@EndDate",EndDate),
                    new SqlParameter("@DoctorId",Doctor)
                                };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[QpsReport_GetMedicalTowerCases]");

            }
            catch (Exception ex)
            { throw new ApplicationException(Errors.ExemptionMessage(ex)); }

            return dataTable;
        }

        

        
    }
}
