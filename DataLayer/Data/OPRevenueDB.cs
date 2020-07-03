using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class OPRevenueDB
    {
        DBHelper dbHelper = new DBHelper("OPRevenue");

        public DataTable getOPRevenue(DateTime startDate, DateTime endDate, int patientBillType, int companyId, int departmentId, int doctorId, int regNo, string employeeId, string ModeofPayment)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@StartDate", startDate),
                                    new SqlParameter("@EndDate", endDate),
                                    new SqlParameter("@PatType", patientBillType),
                                    new SqlParameter("@companyId", companyId),
                                    new SqlParameter("@departmentId", departmentId),
                                    new SqlParameter("@doctorId", doctorId),
                                    new SqlParameter("@regNo", regNo)
                                    ,new SqlParameter("@employeeId", employeeId)
                                    ,new SqlParameter("@ModeofPayment", ModeofPayment)

                                    
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetOPRevenue]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getCancelledOPRevenue(DateTime startDate, DateTime endDate, int patientBillType, int companyId, int departmentId, int doctorId, bool sortByCancellationDate, int regNo, string employeeId, string ModeofPayment)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@StartDate", startDate),
                                    new SqlParameter("@EndDate", endDate),
                                    new SqlParameter("@PatType", patientBillType),
                                    new SqlParameter("@companyId", companyId),
                                    new SqlParameter("@departmentId", departmentId),
                                    new SqlParameter("@doctorId", doctorId),
                                    new SqlParameter("@sortByCancelDate" , sortByCancellationDate),
                                    new SqlParameter("@regNo" , regNo)
                                    ,new SqlParameter("@employeeId", employeeId)
                                    ,new SqlParameter("@ModeofPayment", ModeofPayment)
                                    
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetOpRevenueCancelled]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getAllOPRevenue(DateTime startDate, DateTime endDate, int patientBillType, int companyId, int departmentId, int doctorId, bool sortByCancelDate, int @regNo, string employeeId, string ModeofPayment)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate),
                                    new SqlParameter("@EndDate", endDate),
                                    new SqlParameter("@PatType", patientBillType),
                                    new SqlParameter("@companyId", companyId),
                                    new SqlParameter("@departmentId", departmentId),
                                    new SqlParameter("@doctorId", doctorId),
                                    new SqlParameter("@sortByCancelDate", sortByCancelDate),
                                    new SqlParameter("@regNo" , regNo)
                                    ,new SqlParameter("@employeeId" , employeeId)
                                    ,new SqlParameter("@ModeofPayment" , ModeofPayment)

                                    
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetAllOpRevenueCancelledAndNotCancelled]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getDoctorRevenueOP(DateTime startDate, DateTime endDate, int patientBillType, int departmentId, int doctorId)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate),
                                    new SqlParameter("@EndDate", endDate),
                                    new SqlParameter("@PatType", patientBillType),
                                  
                                    new SqlParameter("@departmentId", departmentId),
                                    new SqlParameter("@doctorId", doctorId)   
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[ARReport_GetDoctorRevenueOP]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public List<OPRevenueDataTAbleTAbleResult> getOPRevenueDataTAble(DateTime startDate, DateTime endDate, int patientBillType, int companyId
            , int departmentId, int doctorId, string regNo, string employeeId, string ModeofPayment)
        {
            var list = new List<OPRevenueDataTAbleTAbleResult>();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@StartDate", startDate),
                                    new SqlParameter("@EndDate", endDate),
                                    new SqlParameter("@PatType", patientBillType),
                                    new SqlParameter("@companyId", companyId),
                                    new SqlParameter("@departmentId", departmentId),
                                    new SqlParameter("@doctorId", doctorId),
                                    new SqlParameter("@regNo", regNo)
                                    ,new SqlParameter("@employeeId", employeeId)
                                    ,new SqlParameter("@ModeofPayment", ModeofPayment)

                                    
                                };

                list = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetOPRevenue]").DataTableToList<OPRevenueDataTAbleTAbleResult>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return list;
        }

        public List<getCancelledOPRevenueDataTAbleTAbleResult> getCancelledOPRevenueDataTAble(DateTime startDate, DateTime endDate, int patientBillType, int companyId, int departmentId, int doctorId
            , bool sortByCancellationDate, string regNo, string employeeId, string ModeofPayment)
        {
            var dataTable = new List<getCancelledOPRevenueDataTAbleTAbleResult>();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@StartDate", startDate),
                                    new SqlParameter("@EndDate", endDate),
                                    new SqlParameter("@PatType", patientBillType),
                                    new SqlParameter("@companyId", companyId),
                                    new SqlParameter("@departmentId", departmentId),
                                    new SqlParameter("@doctorId", doctorId),
                                    new SqlParameter("@sortByCancelDate" , sortByCancellationDate),
                                    new SqlParameter("@regNo" , regNo)
                                    ,new SqlParameter("@employeeId", employeeId)
                                    ,new SqlParameter("@ModeofPayment", ModeofPayment)
                                    
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetOpRevenueCancelled]").DataTableToList<getCancelledOPRevenueDataTAbleTAbleResult>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public List<getAllOPRevenueDataTAbleTAbleResult> getAllOPRevenueDataTAble(DateTime startDate, DateTime endDate
           , int patientBillType, int companyId, int departmentId, int doctorId
            , bool sortByCancelDate, string regNo
            , string employeeId, string ModeofPayment
            )
        {
            var dataTable = new List<getAllOPRevenueDataTAbleTAbleResult>();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate),
                                    new SqlParameter("@EndDate", endDate),
                                  new SqlParameter("@PatType", patientBillType),
                                    new SqlParameter("@companyId", "0"),
                                    new SqlParameter("@departmentId", "0"),
                                    new SqlParameter("@doctorId", "0"),
                                    new SqlParameter("@sortByCancelDate", "0"),
                                    new SqlParameter("@regNo" , "0")
                                    ,new SqlParameter("@employeeId" , "0")
                                    ,new SqlParameter("@ModeofPayment" , "0")

                                    //     new SqlParameter("@PatType", patientBillType),
                                    //new SqlParameter("@companyId", companyId),
                                    //new SqlParameter("@departmentId", departmentId),
                                    //new SqlParameter("@doctorId", doctorId),
                                    //new SqlParameter("@sortByCancelDate", sortByCancelDate),
                                    //new SqlParameter("@regNo" , regNo)
                                    //,new SqlParameter("@employeeId" , employeeId)
                                    //,new SqlParameter("@ModeofPayment" , ModeofPayment)

                                    
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetAllOpRevenueCancelledAndNotCancelled]").DataTableToList<getAllOPRevenueDataTAbleTAbleResult>(); ;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable SalesToSaudiCitizen(DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                        new SqlParameter("@StartDate", startDate),
                                        new SqlParameter("@EndDate", endDate),                   
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_SalesToSaudiCitizen_NoPharmacy]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable FinanceReport_ZeroRatedSales_AllPharmacy(DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                        new SqlParameter("@StartDate", startDate),
                                        new SqlParameter("@EndDate", endDate),                   
                                            };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_ZeroRatedSales_AllPharmacy]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public List<getOprevenue2018VM> getOprevenue2018()
        {
            var list = new List<getOprevenue2018VM>();
            try
            {
                //dbHelper.param = new SqlParameter[]{
                //                    new SqlParameter("@StartDate", startDate),
                //                    new SqlParameter("@EndDate", endDate),
                //                    new SqlParameter("@PatType", patientBillType),
                //                    new SqlParameter("@companyId", companyId),
                //                    new SqlParameter("@departmentId", departmentId),
                //                    new SqlParameter("@doctorId", doctorId),
                //                    new SqlParameter("@regNo", regNo)
                //                    ,new SqlParameter("@employeeId", employeeId)
                //                    ,new SqlParameter("@ModeofPayment", ModeofPayment)

                                    
                //                };

                list = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetOPRevenueEXCEL]").DataTableToList<getOprevenue2018VM>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return list;
        }


        public DataTable getAllOPRevenue_BEVERLY(DateTime startDate, DateTime endDate, int patientBillType, int companyId, int departmentId, int doctorId, bool sortByCancelDate, int @regNo, string employeeId, string ModeofPayment)
        {

            var dataTable = new DataTable();
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@StartDate", startDate),
                                    new SqlParameter("@EndDate", endDate),
                                    new SqlParameter("@PatType", patientBillType),
                                    new SqlParameter("@companyId", companyId),
                                    new SqlParameter("@departmentId", departmentId),
                                    new SqlParameter("@doctorId", doctorId),
                                    new SqlParameter("@sortByCancelDate", sortByCancelDate),
                                    new SqlParameter("@regNo" , regNo)
                                    ,new SqlParameter("@employeeId" , employeeId)
                                    ,new SqlParameter("@ModeofPayment" , ModeofPayment)

                                    
                                };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetAllOpRevenueCancelledAndNotCancelled_BEVERLY]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


    }








    public class OPRevenueDataTAble
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
    public class OPRevenueDataTAbleTAbleResult
    {
        public string ModeOfPayment { get; set; }
        public string ReceiptNo { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string OPID { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string CancelDate { get; set; }
        public string TransactionMonth { get; set; }
        public string PinNumber { get; set; }
        public string DoctorCode { get; set; }
        public string CompanyCode { get; set; }
        public string BillType { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string Billamount { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountAmount { get; set; }
        public string PaidAmount { get; set; }
        public string ChargeRevenue { get; set; }
        public string Recievable { get; set; }
        public string IssueAuthorityCode { get; set; }
        public string Branch { get; set; }
        public string DeptName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyGroup { get; set; }
        public string HISCashRevenue { get; set; }


    }
    public class getCancelledOPRevenueDataTAbleTAbleResult
    {
        public string ModeOfPayment { get; set; }
        public string ReceiptNo { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string OPID { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string CancelDate { get; set; }
        public string TransactionMonth { get; set; }
        public string PinNumber { get; set; }
        public string DoctorCode { get; set; }
        public string CompanyCode { get; set; }
        public string BillType { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string Billamount { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountAmount { get; set; }
        public string DeductiblePaid { get; set; }
        public string ChargeRevenue { get; set; }
        public string Recievable { get; set; }
        public string IssueAuthorityCode { get; set; }
        public string Branch { get; set; }
        public string DeptName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyGroup { get; set; }
        public string HISCashRevenue { get; set; }
    }
    public class getAllOPRevenueDataTAbleTAbleResult
    {
        public string ModeOfPayment { get; set; }
        public string ReceiptNo { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string OPID { get; set; }
        public string Nationality { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string CancelDate { get; set; }
        public string TransactionMonth { get; set; }
        public string PinNumber { get; set; }
        public string DoctorCode { get; set; }
        public string CompanyCode { get; set; }
        public string BillType { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string Billamount { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountAmount { get; set; }
        public string DeductablePaid { get; set; }
        public string ChargeRevenue { get; set; }
        public string Recievable { get; set; }
        public string IssueAuthorityCode { get; set; }
        public string Branch { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyGroup { get; set; }
        public string HISCashRevenue { get; set; }


    }
    public class getVATDetails
    {
        public string ModeOfPayment { get; set; }
        public string ReceiptNo { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string OPID { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string CancelDate { get; set; }
        public string TransactionMonth { get; set; }
        public string PinNumber { get; set; }
        public string DoctorCode { get; set; }
        public string CompanyCode { get; set; }
        public string BillType { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string Billamount { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountAmount { get; set; }
        public string DeductablePaid { get; set; }
        public string ChargeRevenue { get; set; }
        public string Recievable { get; set; }
        public string IssueAuthorityCode { get; set; }
        public string Branch { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyGroup { get; set; }
        public string HISCashRevenue { get; set; }


    }
    public class getOprevenue2018VM
    {
        public string ModeOfPayment { get; set; }
        public string ReceiptNo { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string OPID { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string CancelDate { get; set; }
        public string TransactionMonth { get; set; }
        public string PinNumber { get; set; }
        public string DoctorCode { get; set; }
        public string CompanyCode { get; set; }
        public string BillType { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string Billamount { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountAmount { get; set; }
        public string DeductablePaid { get; set; }
        public string ChargeRevenue { get; set; }
        public string Recievable { get; set; }
        public string IssueAuthorityCode { get; set; }
        public string Branch { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyGroup { get; set; }
        public string HISCashRevenue { get; set; }


    }

}
