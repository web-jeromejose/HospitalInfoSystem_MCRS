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
    public class PolyClinicDB
    {
        DBHelper DB = new DBHelper("MCRS");
        ExceptionLogging eLOG = new ExceptionLogging();

        #region PIN Registration by Date Range
        public DataTable PINRegistrationbyDateRange(int id, string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_GetPINRegistered '" + fromdate + "', '" + todate + "', '" + id + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetEmployee()
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("select id,employeeid + ' - ' + name [name],employeeid + ' - ' + name [text] from employee where designationid in (1540,1878,1535,776) and deleted=0 order by name").ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Summary Patient Reservation
        public DataTable SummaryPatientReservation(int id, DateTime startDate, DateTime endDate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@StartDate ", startDate),
                    new SqlParameter("@EndDate ", endDate),
                    new SqlParameter("@operatorId", id)
                };
                return DB.ExecuteSPAndReturnDataTable("[MCRS].[PolyClinicReports_GetPatientReservationSummary]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
        }

        public List<GenericListModel> GetEmployeeRegister()
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("select distinct a.operatorid [id],b.employeeid + ' - ' + b.name [name], b.employeeid + ' - ' + b.name [text]  from doctorschedule a left join employee b on a.operatorid=b.id").ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Reception Transaction Summary
        public DataTable ReceptionistTransactionSummary(string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Get_ReceptionistCharges '" + fromdate + "', '" + todate + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region OPD Patient Count
        public DataTable OPPatientCount(string fromdate, string todate, int typeid)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                if (typeid == 0)
                {
                    strb.Append("[MCRS].[PolyClinic_GetOPPatientVisit] '" + fromdate + "', '" + todate + "'");
                }
                else if (typeid == 1)
                {
                    strb.Append("[MCRS].[PolyClinic_GetOPPatientReVisit] '" + fromdate + "', '" + todate + "'");
                }
                else
                {
                    strb.Append("[MCRS].[PolyClinic_GetNewOPPatientVisit] '" + fromdate + "', '" + todate + "'");
                }

                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion


        #region OP Patient Count
        public DataTable OPDPatientCount(string fromdate, string todate, int typeid)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                if (typeid == 0)
                {
                    strb.Append("SP_OP_Patient_Visits '" + fromdate + "', '" + todate + "'");
                }
                else if (typeid == 1)
                {
                    strb.Append("SP_OP_Patient_Revisits '" + fromdate + "', '" + todate + "'");
                }
                else
                {
                    strb.Append("SP_OP_NewPatient_Visits '" + fromdate + "', '" + todate + "'");
                }

                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Patient Revisits
        public DataTable PatientRevisits(string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append(" SELECT a.issueauthoritycode + '.' + REPLICATE('0', (10-LEN(a.registrationno))) " +
                            " + CONVERT(VARCHAR,a.registrationno) pin, isnull(a.firstname,'')+' '+ isnull(a.middlename,'')+' '+isnull(a.lastname,'') patientnname, " +
                            " c.Name doctorname,b.datetime visitdate,b.reason reason,c.employeeid+'-'+c.name operator  " +
                            " from patient a left join OPRevistApproval  b on a.Registrationno =b.regno " +
                            " left join employee c on c.Id =b.operatorid " +
                            " where b.DateTime>='" + fromdate + "'" +
                            " and b.datetime<='" + todate + "'" +
                            " order by b.datetime, b.regno");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region[OP Procedure Statistics]
        public DataTable getOPProcedureStatistics(DateTime startDate, DateTime endDate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@StartDate ", startDate),
                    new SqlParameter("@EndDate ", endDate)
                };
                return DB.ExecuteSPAndReturnDataTable("[MCRS].[PolyClinic_GetOPProcedureStatistics]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }
        #endregion

        #region[PatientCancelled Reservation Detail]
        public DataTable getCancelledPatientReservation(DateTime startDate, DateTime endDate, int operatorId,int doctorId,  int patientType, int departmentId)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate),
                    new SqlParameter("@operatorId", operatorId),
                    new SqlParameter("@patientType", patientType),
                    new SqlParameter("@doctorId", doctorId),
                    new SqlParameter("@departmentId", departmentId)

                };
                return DB.ExecuteSPAndReturnDataTable("[MCRS].[PolyClinicReports_GetCancelledPatientReservation]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }
        #endregion

        #region[PatientCancelled Reservation Summary by Employee]
        public DataTable getCancelledPatientReservationSummary(DateTime startDate, DateTime endDate, int operatorId,  int patientType)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate),
                    new SqlParameter("@operatorId", operatorId),
                    new SqlParameter("@patientType", patientType),
            

                };
                return DB.ExecuteSPAndReturnDataTable("[MCRS].[PolyClinicReports_GetCancelledPatientReservationSummary]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }
        #endregion


        #region[PatientCancelled Reservation Summary by Department]
        public DataTable getCancelledPatientReservationSummaryByDepartment(DateTime startDate, DateTime endDate, int departmentId, int patientType)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate),
                    new SqlParameter("@departmentId", departmentId),
                    new SqlParameter("@patientType", patientType),
            

                };
                return DB.ExecuteSPAndReturnDataTable("[MCRS].[PolyClinicReports_GetCancelledPatientReservationSummaryByDepartment]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }
        #endregion

        #region[PatientCancelled Reservation Summary by Doctor]
        public DataTable getCancelledPatientReservationSummaryByDoctor(DateTime startDate, DateTime endDate, int doctorId, int patientType)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate),
                    new SqlParameter("@doctorId", doctorId),
                    new SqlParameter("@patientType", patientType),
            

                };
                return DB.ExecuteSPAndReturnDataTable("[MCRS].[PolyClinicReports_GetCancelledPatientReservationSummaryByDoctor]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }
        #endregion

        #region[OP Bill Item Count]
        public DataTable getOPBillServiceItemCount(DateTime startDate, DateTime endDate, string itemCodes,string serviceId)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@startDate", startDate),
                    new SqlParameter("@endDate", endDate),
                    new SqlParameter("@itemCodes", itemCodes)
                    ,new SqlParameter("@serviceId", serviceId)
                };

                return DB.ExecuteSPAndReturnDataTable("[MCRS].[PolyClinic_GetOPItemBillCount]");
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }
        #endregion
 

          public List<Sex> getOPService()
          {
              var sex = new List<Sex>();
              try
              {
                  sex = DB.ExecuteSQLAndReturnDataTable("SELECT Id as id, MasterTable AS name FROM OPBService WHERE (Deleted = 0) AND (DisplayServiceId NOT IN (10, 11, 4)) ORDER BY Id ").DataTableToList<Sex>();
              }
              catch (Exception ex)
              {

                  throw new ApplicationException(Errors.ExemptionMessage(ex));
              }

              return sex;

          }


          #region[Booked Appointment by Doctor]
          public DataTable getBookedAppointmentByDoctor(DateTime startDate, DateTime endDate, int doctorId, int patientType)
          {
              try
              {
                  DB.param = new SqlParameter[]{
                    new SqlParameter("@stDate", startDate),
                    new SqlParameter("@enDate", endDate),
                    new SqlParameter("@doctorId", doctorId),
                    new SqlParameter("@patientType", patientType),
   
                };
                  return DB.ExecuteSPAndReturnDataTable("[MCRS].[PolyClinic_BookedAppointmentPerDoctor]");
              }
              catch (Exception ex)
              {
                  eLOG.LogError(ex);
                  throw new ApplicationException(Errors.ExemptionMessage(ex));
              }

          }
          #endregion

          #region[Booked Appointment by Doctor]
          public DataTable getBookedAppointmentByDepartment(DateTime startDate, DateTime endDate, string deptId, int patientType)
          {
              try
              {
                  DB.param = new SqlParameter[]{
                    new SqlParameter("@stDate", startDate),
                    new SqlParameter("@enDate", endDate),
                    new SqlParameter("@departmentId", deptId.ToString()),
                    new SqlParameter("@patientType", patientType),
   
                };
                  return DB.ExecuteSPAndReturnDataTable("[MCRS].[PolyClinic_BookedAppointmentPerDepartment]");
              }
              catch (Exception ex)
              {
                  eLOG.LogError(ex);
                  throw new ApplicationException(Errors.ExemptionMessage(ex));
              }

          }
        #endregion


        #region  BookvsWalkin

        public DataTable BookvsWalkin(string fromdate, string todate, int deptid)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                   strb.Append("[MCRS].[PolyClinicReports_BookvsWalkin] '" + fromdate + "', '" + todate + "', "+ deptid + "");
                

                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }


        #endregion



    }
}
