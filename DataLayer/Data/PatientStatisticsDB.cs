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
    public class PatientStatisticsDB
    {
        DBHelper DB = new DBHelper("MCRS");
        ExceptionLogging eLOG = new ExceptionLogging();

        public int ret = 0;
        public string retmsg = "";
        public string operatorId;
        public string oraquery = "";
        public string biquery = "";

        public DailyDashboardMonModel get_DashboardMonitoring(string bdate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@date", bdate)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("MCRS.dashboard_daily_view")
                    .DataTableToList<DailyDashboardMonModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataSet graph_DashboardMonitoring(string date)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@date", date)
                };
                return DB
                    .ExecuteSPAndReturnDataSet("MCRS.dashboard_daily_view_graph");
            }
            catch (Exception ex)
            {
               
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        public ORDailyDashORA get_ORRequest(string date)
        {

            try
            {
                oraquery = "Select count(*) ORRequest from OR_REQUEST_MAIN where OPER_DATE = '" + date + "'";

                return DB.ExecuteQueryInORA(oraquery).DataTableToList<ORDailyDashORA>().FirstOrDefault();

            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public ORDailyDashORAFTD get_ORRequestFTD(string date)
        {
            // for today
            try
            {
                oraquery = "Select count(*) ORRequestFTD from OR_REQUEST_MAIN where OPER_DATE = '" + date + "'";

                return DB.ExecuteQueryInORA(oraquery).DataTableToList<ORDailyDashORAFTD>().FirstOrDefault();

            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public ORDailyDashORAOrig get_ORRequestFTDProg(string date)
        {
            // for today
            try
            {
                oraquery = "Select count(*) OrigORRequest from OR_REQUEST_MAIN where ORIG_OPERDATE_REQ = '" + date + "'";

                return DB.ExecuteQueryInORA(oraquery).DataTableToList<ORDailyDashORAOrig>().FirstOrDefault();

            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<CANServiceType> get_BI_Service_Type()
        {
            try
            {
                biquery = "Select distinct Service_Type Name,  0 as ID from BI_OP_CANCELLATION";

                return DB.ExecuteSQLAndReturnDataTableBI(biquery).DataTableToList<CANServiceType>();

            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<CANServiceType> get_BI_Station()
        {
            try
            {
                biquery = "Select distinct Station Name,  0 as ID from BI_OP_CANCELLATION";

                return DB.ExecuteSQLAndReturnDataTableBI(biquery).DataTableToList<CANServiceType>();

            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<CANServiceType> get_BI_Reason()
        {
            try
            {
                biquery = "Select distinct Reason Name,  0 as ID from BI_OP_CANCELLATION";

                return DB.ExecuteSQLAndReturnDataTableBI(biquery).DataTableToList<CANServiceType>();

            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<CANServiceType> get_BI_Operator()
        {
            try
            {
                biquery = "Select distinct Charged_by Name,  0 as ID from BI_OP_CANCELLATION";

                return DB.ExecuteSQLAndReturnDataTableBI(biquery).DataTableToList<CANServiceType>();

            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<CANResultModel> get_Cancellation_Stats(string fdate, string tdate,
            string sertype, string station, string billtype, string chargeby, string reason, int recfilter)
        {
            StringBuilder bicanquery = new StringBuilder();
            try
            {
                bicanquery.Append("SELECT y.Service_Type as ServiceType, Count(*) TCount, Sum(y.charge_amount) TAmount, y.Station, y.charged_by Operator, y.Reason FROM ");
                bicanquery.Append("(SELECT x.service_type,  x.charge_slip_number, Sum(x.charge_amount) charge_amount, x.station,  x.charged_by,  x.reason ");
                bicanquery.Append("FROM (SELECT CASE WHEN service_type = 'PIN Generation Charges' THEN 'Consultations' ELSE service_type END Service_Type, ");
                bicanquery.Append("charge_slip_number, Sum(charge_amount) charge_amount, station, charged_by, reason ");
                bicanquery.Append("FROM dbo.BI_OP_CANCELLATION WHERE  cancelled_datetime >= '" + fdate + "' AND cancelled_datetime < Dateadd(d, 1, '" + tdate + "') ");

                if (recfilter == 1)
                {
                    bicanquery.Append(" AND Isnull(reissuebillno, '') = '' ");
                }


                if (billtype != "-- All --")
                {
                    bicanquery.Append("AND bill_type = '" + billtype + "'");
                }

                if (station != "-- All --")
                {
                    bicanquery.Append("AND station = '" + station + "'");
                }

                if (chargeby != "-- All --")
                {
                    bicanquery.Append("AND charged_by = '" + chargeby + "'");
                }

                if (reason != "-- All --")
                {
                    bicanquery.Append("AND reason = '" + reason + "'");
                }

                bicanquery.Append(" GROUP  BY service_type, charge_slip_number, station, charged_by, reason) x ");

                if (sertype != "-- All --")
                {
                    bicanquery.Append("WHERE service_type = '" + sertype + "'");
                }

                bicanquery.Append(" GROUP BY x.service_type, x.charge_slip_number, x.station, x.charged_by,  x.reason) y ");
                bicanquery.Append(" GROUP  BY y.Service_type, y.station, y.charged_by, y.reason ");

                bicanquery.Append(" ORDER  BY y.service_type, y.station, y.charged_by, y.reason");

                return DB.ExecuteSQLAndReturnDataTableBI(bicanquery.ToString()).DataTableToList<CANResultModel>();

            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public OPBILLActualCount get_OPBill_Count(string fdate, string tdate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("MCRS.get_opbill_actual_count")
                    .DataTableToList<OPBILLActualCount>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public OPBILLActualAmount get_OPBill_Amount(string fdate, string tdate)
        {
            try
            {
                DB.param = new SqlParameter[]{
                    new SqlParameter("@fdate", fdate),
                    new SqlParameter("@tdate", tdate)
                };
                return DB
                    .ExecuteSPAndReturnDataTable("MCRS.get_opbill_actual_amount")
                    .DataTableToList<OPBILLActualAmount>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public OPBILLCanCount get_CANOPBill_Count(string fdate, string tdate, int receipts, string billtype)
        {
            StringBuilder bicanquery = new StringBuilder();
            try
            {
                bicanquery.Append("SELECT COUNT(*) opcancount FROM  (SELECT service_type,charge_slip_number FROM BI_OP_CANCELLATION ");
                bicanquery.Append("WHERE cancelled_datetime >= '" + fdate + "' ");
                bicanquery.Append("AND cancelled_datetime < DATEADD(D,1,'" + tdate + "')");

                if (receipts == 1)
                {
                    bicanquery.Append(" AND Isnull(reissuebillno, '') = '' ");
                }

                if (billtype != "-- All --")
                {
                    bicanquery.Append("AND bill_type = '" + billtype + "'");
                }

                bicanquery.Append(" AND service_type <> 'PIN Generation Charges' GROUP BY service_type,charge_slip_number) x ");

                return DB.ExecuteSQLAndReturnDataTableBI(bicanquery.ToString()).DataTableToList<OPBILLCanCount>().SingleOrDefault();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public OPBILLCanAmount get_CANOPBill_Amount(string fdate, string tdate, int receipts, string billtype)
        {
            StringBuilder bicanquery = new StringBuilder();
            try
            {
                bicanquery.Append("SELECT SUM(charge_amount) as opcanamount from BI_OP_CANCELLATION ");
                bicanquery.Append("WHERE cancelled_datetime >= '" + fdate + "' ");
                bicanquery.Append("AND cancelled_datetime < DATEADD(D,1,'" + tdate + "')");

                if (receipts == 1)
                {
                    bicanquery.Append(" AND Isnull(reissuebillno, '') = '' ");
                }

                if (billtype != "-- All --")
                {
                    bicanquery.Append("AND bill_type = '" + billtype + "'");
                }

                return DB.ExecuteSQLAndReturnDataTableBI(bicanquery.ToString()).DataTableToList<OPBILLCanAmount>().SingleOrDefault();
            }
            catch (Exception ex)
            {
                eLOG.LogError(ex);
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        #region In-Patient Admission
        public DataTable IPAdmissionCensus(string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_IP_Patient_Visit_Census '" + fromdate + "', '" + todate + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Out-Patient Visit
        public DataTable OPVisitCensus(string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_OP_Patient_Visit_Census '" + fromdate + "', '" + todate + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Currently Admitted Patients with Charges
        public DataTable PatientAdmitted10Days()
        {
            try
            {
                //StringBuilder strb = new StringBuilder();
                //strb.Append("[dbo].[SP_CurrentlyAdmittedPatients_10Days]");

                return DB.ExecuteSPAndReturnDataTable("[dbo].[SP_CurrentlyAdmittedPatients_10Days_2019]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable Param1()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select count(*) Param1 " +
                        "from HIS..Inpatient AS a " +
                        "INNER JOIN HIS..BedTransfers c ON a.IPID = c.ipid " +
                        "left join his..bed d on c.bedid = d.id " +
                        "Where a.admitdatetime >= '01-jan-2007' and " +
                        "C.ID IN(select top 1 d.id from HIS..BedTransfers d " +
                        "left join his..bed e on d.bedid = e.id where d.IPID = a.IPID " +
                        "ORDER BY d.id desc)");

                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable Param2()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select count(*) Param2 " +
                        "from HIS..Inpatient AS a " +
                        "INNER JOIN HIS..BedTransfers c ON a.IPID = c.ipid " +
                        "left join his..bed d on c.bedid = d.id " +
                        "Where a.admitdatetime >= '01-jan-2007' and " +
                        "C.ID IN(select top 1 d.id from HIS..BedTransfers d " +
                        "left join his..bed e on d.bedid = e.id where d.IPID = a.IPID " +
                        "ORDER BY d.id desc) and d.name like 'ER%'");

                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable ParamTotalCharges()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("Select SUM(TOTAL_BILL) as ParamTotalCharges from tbl_CurrentlyAdmittedPatient");

                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }


        public DataTable PatientAdmitted10DaysCharge()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select a.charge_acct code,c.name MASTER_CHARGE_ACCOUNT,count(*) TOTAL_PATIENT,sum(a.total_bill) TOTAL_BILL, " +
                        "sum(a.total_payment) TOTAL_PAYMENT,sum(a.total_outstanding_balance) TOTAL_BALANCE  " +
                        "from tbl_CurrentlyAdmittedPatient a " +
                        "left join company b on (a.charge_acct = b.code and b.Active=0) " +
                        "left join category c on b.categoryid = c.id " +
                        "group by a.charge_acct ,c.name");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable PatientAdmitted10DaysDoctor()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select a.doctor + ' - ' + b.name Doctor,count(*) Total_Patient " +
                        "from tbl_CurrentlyAdmittedPatient a " +
                        "left join doctor b on a.doctor = b.empcode " +
                        "group by a.doctor + ' - ' + b.name " +
                        "order by a.doctor + ' - ' + b.name ");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Daily Patient Visit Statistics
        public DataTable PatientVisitStatistics(string fromdate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Procedure_Patient_Visit_Stat '" + fromdate + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Patient Statistics by Nationality
        public DataTable PatientNationalityStatistics(int id, string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Get_PatientNationalityStatistics '" + fromdate + "', '" + todate + "', '" + id + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetNationality()
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("select id, upper(name) [name], upper(name) [text] FROM Nationality where deleted = 0 order by name ").ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Patient Consultation Between 1pm to 5pm
        public DataTable ConsultationBetween1to5(int id, string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("sp_Get_Consultation_SlipDuty '" + fromdate + "', '" + todate + "', '" + id + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetDepartment()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select distinct b.id,b.name,b.name [text] from doctor a " +
                             "left join department b on a.departmentid = b.id " +
                             "where(a.deleted = 0) and b.id not in (3,25,41,134) " +
                             "order by b.name ");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString()).ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetYear()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select distinct year(billdate) [id], year(billdate) [name], year(billdate) [text] from ipbill order by year(billdate) desc");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString()).ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Patient by Age
        public DataTable PatientbyAge(string startage, string endage, int dateoption, string startdate, string enddate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_GetPatientByAge '" + startage + "', '" + endage + "', '" + dateoption + "', '" + startdate + "', '" + enddate + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<AgeRange> GetAgeRange()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select *, name [text]  from MCRS_PatientAgeRange order by id ");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString()).ToList<AgeRange>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Department Wise Admission Indexing
        public DataTable MRDIndexing(int id, string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_GetDeptWiseAdmissionIndexing '" + fromdate + "', '" + todate + "', '" + id + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Currently Admitted Patient List
        public DataTable CurrentlyAdmittedList()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_GetPatientAdmittedList ");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region OP Cancellation
        public DataTable OPPHA_CancelledReceipt(int id, string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Get_OPPHA_Cancellation '" + fromdate + "', '" + todate + "', '" + id + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetServiceType()
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("select id,UPPER(name) name, UPPER(name) [text] from opbservice where deleted=0 order by name ").ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetServiceTypeWhereID(string id)
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("select id,UPPER(name) name, UPPER(name) [text] from opbservice where deleted=0 and id = '"+id+"' order by name ").ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        #endregion

        #region ER Consultation Statistic Count
        public DataTable ERConsultationStatisticCount(string fromdate, string todate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select count(*) [ER], '' [NonER]  from opcompanybilldetail where " +
                        "CONVERT(DATE, billdatetime, 101) BETWEEN CONVERT(DATE,'" + fromdate + "', 101) AND CONVERT(DATE, '" + todate + "', 101)  " +
                        "and serviceid = 2 and stationid in (18,189) and itemcode like 'ER%'  " +
                        "union all  " +
                        "select '' [ER], count(*) [NonER] from opcompanybilldetail where  " +
                        "CONVERT(DATE, billdatetime, 101) BETWEEN CONVERT(DATE,'" + fromdate + "', 101) AND CONVERT(DATE, '" + todate + "', 101) " +
                        "and serviceid = 2 and stationid in (18,189) and itemcode not like 'ER%' ");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Deficiency Files
        public DataTable Deficiency(string StartDate, string EndDate, string Group, string Floors, string IncludeNew)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Current_Deficiency_NEW '" + StartDate + "','" + EndDate + "','" + Group + "','" + Floors + "','" + IncludeNew + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyDepartment(string StartDate, string EndDate, string Group, string Floors, string IncludeNew)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Current_Deficiency_Department_NEW '" + StartDate + "','" + EndDate + "','" + Group + "','" + Floors + "','" + IncludeNew + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyMonthGraph(string StartDate, string EndDate, string MonCount, string Group)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Current_Deficiency_Month_Graph '" + StartDate + "','" + EndDate + "','" + MonCount + "','" + Group + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyMonthGraphWithNew(string StartDate, string EndDate, string MonCount, string Group)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Current_Deficiency_Month_Graph_WithNew '" + StartDate + "','" + EndDate + "','" + MonCount + "','" + Group + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyDepartmentGraph(string StartDate, string EndDate, string Group)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Current_Deficiency_Department_Graph '" + StartDate + "','" + EndDate + "','" + Group + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyDepartmentGraphWithNew(string StartDate, string EndDate, string Group)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Current_Deficiency_Department_Graph_WithNew '" + StartDate + "','" + EndDate + "','" + Group + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Deficiency Files PIN Wise
        public List<GenericListModel> GetSpeciality()
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("select id [id],code+'-'+name [name],code+'-'+name [text]  from  specialisation where id in " +
                                                       "(2,3,5,13,18,26,28,30,33,34,35,39,41,44,49,50,51,54,57,62,67,73,74,76,78,80,81,83,85,88,90,92,98," +
                                                       "100,101,102,106,108,111,120,128,134,135,158) ORDER BY name").ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetDoctor(int specialityid)
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("select a.ID [id],isnull(a.EmpCode,'') +'-'+isnull(a.FirstName,'')+' '+isnull(a.LastName,'') [name], " +
                                                        "isnull(a.EmpCode,'') +'-'+isnull(a.FirstName,'')+' '+isnull(a.LastName,'') [text] " +
                                                        "from doctor a, Doctor_Specialisation b " +
                                                        "where a.id=b.doctorid  and a.Deleted<>1 " +
                                                        "and b.specialisationid = " + specialityid + " " +
                                                        "and a.empcode<>'AM01' " +
                                                        "group by a.id,isnull(a.EmpCode,'') +'-'+isnull(a.FirstName,'') +' '+isnull(a.LastName,'') " +
                                                        "order by name").ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetPIN(int doctorid, string fromdate, string todate)
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("Select Distinct RegistrationNo [id], " +
                                                    "issueauthoritycode+'.'+REPLICATE('0',(10-LEN(registrationno)))+CONVERT(varchar,registrationno) [name], " +
                                                    "issueauthoritycode+'.'+REPLICATE('0',(10-LEN(registrationno)))+CONVERT(varchar,registrationno) [text] " +
                                                    "from  AllInpatients " +
                                                    "where dischargedatetime >= '" + fromdate + "' " +
                                                    "AND dischargedatetime < '" + todate + "' " +
                                                    "AND doctorid= " + doctorid).ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetStandard(bool isOldStandard)
        {
            try
            {
                if (isOldStandard)
                {
                    return DB.ExecuteSQLAndReturnDataTable("select id,name,name [text] from MRDefficencyCodes where convert(date,startdatetime) < '01 jan 2014' and id in (Select groupid from MRDefficencyCodesLookup)").ToList<GenericListModel>();
                }
                else
                {
                    return DB.ExecuteSQLAndReturnDataTable("select id,name,name [text] from MRDefficencyCodes where id in (Select groupid from MRDefficencyCodesLookup)").ToList<GenericListModel>();
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyPINWiseAll(string StartDate, string EndDate, bool is24hrs)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                if (is24hrs) strb.Append("SP_Deficiency_PinWiseAllLess24 '" + StartDate + "','" + EndDate + "'");
                else strb.Append("SP_Deficiency_PinWiseAll '" + StartDate + "','" + EndDate + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyPINWise(string StartDate, string EndDate, string PIN, bool is24hrs)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                if (is24hrs) strb.Append("SP_Deficiency_PinWiseLess24 '" + StartDate + "','" + EndDate + "','" + PIN + "'");
                else strb.Append("SP_Deficiency_PinWise '" + StartDate + "','" + EndDate + "','" + PIN + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyPINWiseComparisonAll(bool is24hrs, string MoFrom, string YrFrom, string MoTo, string YrTo, string DefId, string Speciality, string Trantype)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                if (is24hrs) strb.Append("SP_Deficiency_ComparisonAllLess24 " + MoFrom + "," + YrFrom + "," + MoTo + "," + YrTo + "," + DefId + "," + Speciality + "," + Trantype);
                else strb.Append("SP_Deficiency_ComparisonAll " + MoFrom + "," + YrFrom + "," + MoTo + "," + YrTo + "," + DefId + "," + Speciality + "," + Trantype);
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyPINWiseComparison(bool is24hrs, string MoFrom, string YrFrom, string MoTo, string YrTo, string Doctor, string DefId)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                if (is24hrs) strb.Append("SP_Deficiency_ComparisonLess24 " + MoFrom + "," + YrFrom + "," + MoTo + "," + YrTo + "," + Doctor + "," + DefId);
                else strb.Append("SP_Deficiency_Comparison " + MoFrom + "," + YrFrom + "," + MoTo + "," + YrTo + "," + Doctor + "," + DefId);
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable DeficiencyPINWiseComparisonSpeciality(bool is24hrs, string MoFrom, string YrFrom, string MoTo, string YrTo, string DefId)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                if (is24hrs)
                    strb.Append("SP_Deficiency_ComparisonAllSpecialityLess24 " + MoFrom + "," + YrFrom + "," + MoTo + "," + YrTo + "," + DefId);
                else
                    strb.Append("SP_Deficiency_ComparisonAllSpeciality " + MoFrom + "," + YrFrom + "," + MoTo + "," + YrTo + "," + DefId);
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region Patient Registration by City
        public List<GenericListModel> GetCity()
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("select id, name, name [text] from city where countryid=1 and deleted=0").ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public List<GenericListModel> GetCityById(string id)
        {
            try
            {
                return DB.ExecuteSQLAndReturnDataTable("select id, name, name [text] from city where countryid=1 and deleted=0 and id ='"+ id+"'" ).ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }


        public DataTable PatientRegistration(string StartDate, string EndDate, string City)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append(" SELECT ('SA01.' + REPLICATE('0',10-(LEN(CONVERT(Varchar(10),p.registrationno)))) + CONVERT(Varchar(10),p.registrationno)) PIN, " +
                             " p.FamilyName+' '+ " +
                             " p.Firstname +' '+p.MiddleName+' '+p.LastName pname,p.Age as age, " +
                             " agt.name as agetype,s.name as Sex," +
                             " case when p.country=-1 then countryname else co.name end as country, " +
                             " p.PPhone,p.PCellNo,p.PeMail from patient p,country co,city c, " +
                             " agetype agt, sex s " +
                             " where p.agetype=agt.id and p.Pcity*=c.id " +
                             " and p.country*=co.id " +
                             " and p.sex=s.id " +
                             " and RegDateTime >= '" + StartDate + "'" +
                             " and RegDateTime < '" + EndDate + "'" +
                             " and p.pcity = '" + City + "'" +
                             " Order By RegistrationNo");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable PatientAdmission(string StartDate, string EndDate, string City)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("select p.ipid as IpId, " +
                             " ('SA01.' + REPLICATE('0',10-(LEN(CONVERT(Varchar(10),p.registrationno)))) + CONVERT(Varchar(10),p.registrationno)) PIN, " +
                             " p.Firstname +' '+p.MiddleName+' '+p.LastName pname,p.Age as age, " +
                             " agt.name as agetype,s.name as Sex," +
                             " case when p.pcountry=-1 then countryname else co.name end as country, " +
                             " convert(varchar(25),d.name) as doctor,p.PPhone,p.PCellNo,p.PeMail " +
                             " from agetype agt,inpatient p,country co,city c, " +
                             " employee d, sex s where agt.id=p.agetype and d.id=p.doctorid " +
                             " and p.Pcity*=c.id and p.Pcountry*=co.id " +
                             " and p.sex=s.id " +
                             " and p.pcity = '" + City + "'" +
                             " and AdmitDateTime >= '" + StartDate + "'" +
                             " and AdmitDateTime < '" + EndDate + "'" +
                             " union select p.ipid as IpId, " +
                             " ('SA01.' + REPLICATE('0',10-(LEN(CONVERT(Varchar(10),p.registrationno)))) + CONVERT(Varchar(10),p.registrationno)) PIN, " +
                             " p.Firstname +' '+p.MiddleName+' '+p.LastName pname,p.Age as age, " +
                             " agt.name as agetype,s.name as Sex," +
                             " case when p.pcountry=-1 then countryname else co.name end as country, " +
                             " convert(varchar(25),d.name) as doctor,p.PPhone,p.PCellNo,p.PeMail " +
                             " from agetype agt,oldinpatient p,country co,city c, " +
                             " employee d, sex s where p.agetype=agt.id and d.id=p.doctorid " +
                             " and p.Pcity*=c.id and p.Pcountry*=co.id " +
                             " and p.sex=s.id " +
                             " and p.pcity = '" + City + "'" +
                             " and AdmitDateTime >= '" + StartDate + "'" +
                             " and AdmitDateTime < '" + EndDate + "'" +
                             " Order By Pin");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable PatientSummary(string StartDate, string EndDate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append(" SELECT x.city, sum(x.regpin) regpin, sum(x.admpin) admpin " +
                            " FROM " +
                            " (SELECT c.name city, count(P.REGISTRATIONNO) REGPIN, 0 ADMPIN " +
                            " from City c  left join patient p " +
                            " on c.ID=p.pcity " +
                            " where P.RegDateTime >= '" + StartDate + "'" +
                            " and p.RegDateTime < '" + EndDate + "'" +
                            " GROUP BY c.name " +
                            " UNION ALL " +
                            " SELECT c.name city,0 REGPIN, count(P.ipid) ADMPIN " +
                            " from city c left join inpatient p " +
                            " on c.id=p.pcity " +
                            " where P.admitDateTime >= '" + StartDate + "'" +
                            " and p.admitDateTime < '" + EndDate + "'" +
                            " GROUP BY c.Name " +
                            " UNION " +
                            " SELECT c.name city,0 REGPIN, count(p.ipid) ADMPIN " +
                            " from city c left join oldinpatient p " +
                            " on c.id=p.pcity " +
                            " where P.admitDateTime >= '" + StartDate + "'" +
                            " and p.admitDateTime < '" + EndDate + "'" +
                            " GROUP BY c.Name " +
                            ") x " +
                            " group by x.city  order by x.city ");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region ICU Admission with Diagnosis
        public List<GenericListModel> GetDiagnosis(string name)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                if (String.IsNullOrEmpty(name)) strb.Append("select top 100 id,isnull(code,'')+'-'+isnull(description,'') [name], left(isnull(code,'')+'-'+isnull(description,''), 75)+'...' [text] from icd10codes");
                else strb.Append("select top 100 id,isnull(code,'')+'-'+isnull(description,'') [name], left(isnull(code,'')+'-'+isnull(description,''), 75)+'...' [text] from icd10codes where description like '%" + name + "%' OR code like '%" + name + "%'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString()).ToList<GenericListModel>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable ICUAdmissionwithDiagnosis(string StartDate, string EndDate, Int32 diagnosis, int lengthofstay)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SP_Alos_Diagnosis '" + StartDate + "','" + EndDate + "','" + diagnosis + "','" + lengthofstay + "'");
                return DB.ExecuteSQLAndReturnDataTable(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        #region OR Report
        public DataTable ORReport(string StartDate, string EndDate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("SELECT MRD_OR.PIN,PT_NAME,SURG_CODE,OPR_DATE,OR_CODE,OR_DESC1,MRD_OR.OR_DESC2," + 
                         " MRD_OR.OR_DESC3,MRD_OR.DIAGNOSIS_1,MRD_OR.DIAGNOSIS_2,DOC_NAME, " + 
                         " DECODE(OPERATION_TYPE,1,'Major',2,'Minor',3,'Medium') op_type, " + 
                         " round(((to_date(opr_end,'HH:MIAM') - to_date(opr_time,'HH:MIAM'))*24)*60,2) spent, " + 
                         " TO_NUMBER(est_time) avetime, " + 
                         " (TO_NUMBER(est_time)-round(((to_date(opr_end,'HH:MIAM') - to_date(opr_time,'HH:MIAM'))*24)*60,2)) baltime" + 
                         " FROM MRD_OR,DOCTOR,PATIENT " + 
                         " WHERE DOCTOR.DOC_CODE = MRD_OR.SURG_CODE AND " + 
                         " PATIENT.PIN = MRD_OR.PIN AND " + 
                         " OPR_DATE BETWEEN TO_DATE('" + StartDate + "','MM/DD/YYYY') AND TO_DATE('" + EndDate + "','MM/DD/YYYY') " + 
                         " ORDER BY MRD_OR.PIN ");
                return DB.ExecuteQueryInORA(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }
        #endregion

        public DataTable GetListOfORDone(string StartDate, string EndDate)
        {
            try
            {
                StringBuilder strb = new StringBuilder();
              

                strb.Append("select a.doc_code DOC_CODE,a.pin PIN,b.adm_name PT_NAME,a.service_code SERVICE_CODE,a.service_name SERVICE_NAME,a.charge_amount CHARGE_AMOUNT from wipro_temp_charges_ip a" +
               " left join wipro_admission b on a.ipid = b.ipid   " +
               " where a.entry_date >= '" + StartDate + "' " +
               " and a.entry_date < '" + EndDate + "'   " +
               " and a.c_trxcode = '22' and surg_proc_ind = 'Y'   " +
                "and a.service_code not in ('FMERRZ9998','FMERTZ0001','FMORDZ9999',  " +
                   " 'FMORDZ0002','FMORDZ0001','FMORDZ9998','FMNUR-0013') " +
                     " order by a.pin ");

                return DB.ExecuteQueryInORA(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }

        public DataTable getOPDCancellation(DateTime from, DateTime to, int serviceId, int doctorId, int sortby, int reasonId)
        {
            var dataTable = new DataTable();

            try
            {
             
                DB.param = new SqlParameter[]{
                                   new SqlParameter("@from", from.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@to", to.ToString("dd-MMM-yyyy")),
                                   new SqlParameter("@serviceid", serviceId.ToString()),
                                   new SqlParameter("@sortOpt", sortby.ToString()),
                                   new SqlParameter("@doctorId", doctorId.ToString()),
                                   new SqlParameter("@reasonId", reasonId)
                                 };

                dataTable = DB.ExecuteSPAndReturnDataTable("[MCRS].[PatientStatisticsReport_OPDCancelation]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }
    }
}
