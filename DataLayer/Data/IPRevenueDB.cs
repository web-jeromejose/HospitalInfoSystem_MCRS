using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataLayer.Data
{
    public class IPRevenueDB
    {
        DBHelper dbHelper = new DBHelper("IPRevenueDB");


        //,viewModel.PatientBillType,viewModel.BillType,viewModel.PIN,viewModel.DoctorId,viewModel.CompanyId,viewModel.DepartmentId,viewModel.ServiceId,viewModel.EmpId
        public List<FinanceReportsIPRevenueDataTAbleResult> getIPRevenueDataTAble(DateTime startDate, DateTime endDate, int patientType,int billtype)
        {
            var companies = new List<FinanceReportsIPRevenueDataTAbleResult>();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@from",  startDate),
                                    new SqlParameter("@to", endDate),
                                    new SqlParameter("@patientType", patientType),
                                    new SqlParameter("@billType", billtype ),
                                    new SqlParameter("@companyId", "0"),
                                    new SqlParameter("@departmentId", "0"),
                                    new SqlParameter("@doctorId", "0"),
                                    new SqlParameter("@regNo", "0"),
                                    new SqlParameter("@isRevenue", 0),
                                    new SqlParameter("@serviceId", "0")
                                    ,new SqlParameter("@employeeId", "0")
                                };

                companies = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReports_GETIPRevenue]").DataTableToList<FinanceReportsIPRevenueDataTAbleResult>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return companies;
        }

        public DataTable getIPRevenue(DateTime from, DateTime to, int patientType, int billType, int companyId, int departmentId, int doctorId, int regNo, int isRevenue, int serviceId, string EmployeeId)
        {
            try
            {
                dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@from", from.ToString("dd-MMM-yyyy")),
                                    new SqlParameter("@to", to.ToString("dd-MMM-yyyy")),
                                     new SqlParameter("@billType", billType),
                                    new SqlParameter("@patientType", patientType),                                   
                                    new SqlParameter("@companyId", companyId),
                                    new SqlParameter("@departmentId", departmentId),
                                    new SqlParameter("@doctorId", doctorId),
                                    new SqlParameter("@regNo", regNo),
                                    new SqlParameter("@isRevenue",isRevenue),// Convert.ToInt16(isRevenue)),
                                    new SqlParameter("@serviceId", serviceId)
                                    ,new SqlParameter("@employeeId", EmployeeId)
                                };

                return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReports_GETIPRevenue]");

            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
        }



    }

    public class FinanceReportsIPRevenueDataTAble
    {
        public string StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PatientBillType { get; set; }
        public string BillType { get; set; }
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public string DoctorId { get; set; }
        public string PIN { get; set; }
        public string ServiceId { get; set; }
        public string EmpId { get; set; }

    }
    public class FinanceReportsIPRevenueDataTAbleResult
    {
        public string CashRevenue { get; set; }
        public string HISGainLoss { get; set; }
        public string HISRecievable { get; set; }
        public string ChargeRevenue { get; set; }
        public string Nationality { get; set; }
        public string IPID { get; set; }
        public string PName { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string InvoiceNo { get; set; }
        public string AdmissionDate { get; set; }
        public string DischargeDate { get; set; }
        public string DischargeMonth { get; set; }
        public string PIN { get; set; }
        public string RoomNo { get; set; }
        public string DoctorCode { get; set; }
        public string MedicalDept { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceDesc { get; set; }
        public string ServiceDate { get; set; }
        public string ServiceMonth { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string HISRevenue { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountAmount { get; set; }
        public string PackageAmount { get; set; }
        public string Exclusion { get; set; }
        public string Deductibles { get; set; }
        public string BillType { get; set; }
        public string PackageDealPatient { get; set; }
        public string GainLossPatient { get; set; }
        public string NewHisRevenue { get; set; }


    }
}
