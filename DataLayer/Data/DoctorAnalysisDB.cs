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
    public class DoctorAnalysisDB
    {
        DBHelper dbhelper = new DBHelper("DoctorAnalysisDB");
        public DataTable getPatientMovement(DateTime StartDate, DateTime EndDate, int DoctorId)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stDate",StartDate),
                        new SqlParameter("@enDate",EndDate),
                        new SqlParameter("@docid",DoctorId),
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[DoctorAnalysis_OPDoctorTimeInOut]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getDoctorAppointmentSched(DateTime StartDate, DateTime EndDate, int DoctorId)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stDate",StartDate),
                        new SqlParameter("@enDate",EndDate),
                        new SqlParameter("@docid",DoctorId),
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[DoctorAnalysisReport_AppointmentSchecInOut]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getDoctorClinicalAnalysis(DateTime StartDate, DateTime EndDate, int DoctorId)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stDate",StartDate),
                        new SqlParameter("@enDate",EndDate),
                        new SqlParameter("@docid",DoctorId),
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[DoctorAnalysisReport_DoctorClinicAnalysis]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getDoctorLeaveHistory(DateTime StartDate, DateTime EndDate, int DoctorId)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@startdate",StartDate),
                        new SqlParameter("@enddate",EndDate),
                        new SqlParameter("@docid",DoctorId),
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[DoctorAnalysisReport_GetDoctorsLeaveHistory]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getPCAdminLeaveHistory(DateTime StartDate, DateTime EndDate, int DoctorId)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@startdate",StartDate),
                        new SqlParameter("@enddate",EndDate),
                        new SqlParameter("@empid",DoctorId),
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[DoctorAnalysisReport_GetPcAdminLeaveHistory]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }







    }
}
