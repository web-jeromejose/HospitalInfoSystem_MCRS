using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer.Data
{
    public class FinanceReportDB
    {
        DBHelper dbHelper = new DBHelper("AdjustmentsDB");

        public DataTable getOPDCashCollectionByReceptionistIsGroup(DateTime from)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", from.ToString())
                     
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetOpdCashReceptionist]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }
        public DataTable getOPDCashCollectionByReceptionist(DateTime from)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", from.ToString())
                     
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetOPDCash]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable getBillEff(DateTime from,DateTime end ,int cat)
        {
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stDate", from.ToString()),
                                   new SqlParameter("@endate", end.ToString()),
                                   new SqlParameter("@xCat", cat),
                     
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_GetBillingEff]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable ARPackageNonPacakgewithSP(DateTime stdate, DateTime endate,string BillType, int coid,string SPNAME)
        {
             
            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", stdate.ToString()),
                                   new SqlParameter("@endate", endate.ToString()),
                                   new SqlParameter("@BillType", BillType.ToString()),
                                   new SqlParameter("@coid", coid),
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable(SPNAME);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }
        public DataTable getWareHouseIssueancetoStore(int categoryid,DateTime stdate, DateTime endate, string storename)
        {
            //ViewReport("SP_GetWarehouseIssuencetoStore " & CType(cbocategory.SelectedItem, ValueDescriptionPair).Value & ",'" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(DateAdd(DateInterval.Day, 1, enDate.Value), "DD_MMM_YYYY") & "', '" & xvar1 & "'")
//            [MCRS].[FinanceReport_WarehouseIssueStore]
//(@Category int, @stdate datetime, @endate datetime, @AreaStore varchar(50)) 

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@Category",categoryid),
                                   new SqlParameter("@endate", endate.ToString()),
                                   new SqlParameter("@stdate", stdate.ToString()),
                                   new SqlParameter("@AreaStore", (storename == "ALL" ? "%%" : "%"+storename+"%")),
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReport_WarehouseIssueStore]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }
        public DataTable getOPDAdvPayment(DateTime stDate, DateTime enDate, string pinid, string recptno)
        {

//         [MCRS].[FinanceReportOPDAdvPayment]
//(@stDate datetime, @enDate datetime,@pinid int = '0', @recptno varchar(15) = '0'

            var dataTable = new DataTable();

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@pinid",pinid),
                                   new SqlParameter("@enDate", enDate.ToString()),
                                   new SqlParameter("@stDate", stDate.ToString()),
                                   new SqlParameter("@recptno", recptno)
                                 };

                dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[FinanceReportOPDAdvPayment]");

                //oracle

                StringBuilder strb = new StringBuilder();
                StringBuilder strb2 = new StringBuilder();
                strb.Append(@"
SELECT *
from adv_payment  a inner join col_payment b on a.or_no = b.or_no 
 where  1 = 1
  and (@pinid = '0' Or a.pin = @pinid)
									  and (@recptno = '0' OR a.Or_No = @recptno)
 a.pin like '%808000%';
 ");

                strb2.Append(@"   select a.or_no OR_NO,to_number(substr(b.pin,4,8)) PIN,'' BILLNO,b.c_entry_datetimefull TRANSACTION_DATE,'' SERVICE_TYPE, 
             b.or_amount AMOUNT, 
             'Transfered to IP (' || to_char(b.adm_visit_no) || ')' TRX_TYPE 
             from adv_payment a 
             inner join col_payment b on a.or_no = b.or_no 
             where  
             b.c_trxcode=42 
             AND a.or_no = 'T192770'
             UNION ALL 
             select a.or_no OR_NO,to_number(substr(b.pin,4,8)) PIN,'' BILLNO,b.c_entry_datetimefull TRANSACTION_DATE,'' SERVICE_TYPE, 
             -b.refund_amount AMOUNT, 
             'Transfered to IP (' || to_char(b.adm_visit_no) || ')' TRX_TYPE 
             from adv_payment a 
             inner join col_payment b on a.or_no = b.or_no 
             where  
             b.c_trxcode=44 and b.refund_amount > 0
               AND a.or_no = 'T192770'
 ");
              //  var selectFromOracle = dbHelper.ExecuteQueryInORA(strb.ToString());

                //oracle
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable PendingServices(DateTime from, DateTime end, string patientype, int dept, int catId, int compId,string Pin)
        {
            var dataTable = new DataTable();

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Clear();

                sql.Append("  ");
                sql.Append("  Declare @startDate varchar(20) = '" + from.ToShortDateString() + "' ");
                sql.Append("  Declare @endDate varchar(20)= '" + end.ToShortDateString() + "'  ");
                sql.Append("  Declare @departmentid Int  =" + dept + " ");
                sql.Append("  Declare @catId Int  = " + catId + "");
                sql.Append("  Declare @companyId Int  =" + compId + " ");
                sql.Append("  Declare @PIN  varchar(20)  ='" + Pin + "' ");
                sql.Append("  DECLARE @SA Varchar(255) SET @SA = (Select top 1 IssueAuthorityCode from OrganisationDetails ) ");
                sql.Append("   ");


                if (patientype != "1")//  all=0 ip = 1 op=2
                {
                 

                    sql.Append("  select  ROW_NUMBER() OVER(PARTITION By a.BillNo  order by (select 0)) as slno ");
                    sql.Append("  ,a.InvoiceBillNo as invoiceId  ");
                    sql.Append("  ,c.Code + ' - '+d.Code+' - '+d.Name as CurrentAcct ");
                    sql.Append(" ,(@SA+'.' + REPLICATE('0',10-(LEN(CONVERT(Varchar(10),a.registrationno)))) + CONVERT( Varchar(10),a.registrationno)) PIN ");
                    sql.Append("  ,a.BillNo+' - '+cast(a.balance as varchar(20)) as billno  ");
                    sql.Append("  ,a.PaidAmount as deductible ");
                    sql.Append("  ,a.ABillDateTime as Actualdate ");
                    sql.Append("  , a.BillDateTime as CurrentDate ");
                    sql.Append("  ,A.ITEMCODE +' - '+A.ITEMNAME as Item ");
                    sql.Append("  ,e.Name as Department ");
                    sql.Append("  , 'Not Done' as ReportStatus ");
                    sql.Append("   ");
                    sql.Append("  from HIS.dbo.ARCompanyBillDetail A  ");
                    sql.Append("  inner JOIN HIS.dbo.OPTestOrderDetail B on a.OpBillId =b.OpBillId and a.ItemId = b.TestId ");
                    sql.Append("  left join HIS.dbo.category c on a.CategoryId = c.ID ");
                    sql.Append("  left join HIS.dbo.Company d on a.CompanyId = d.ID ");
                    sql.Append("  left join HIS.dbo.Department e on a.DepartmentId = e.ID ");
                    sql.Append("  where A.ServiceId = 3 and (@departmentid = 0 OR A.DepartmentId = @departmentid)   ");
                    sql.Append(" and  (@catId = 0 OR C.ID = @catId)    ");
                    sql.Append(" and  (@companyId = 0 OR D.ID = @companyId)    ");
                    sql.Append(" and  ((LEN(@pin)) = 0 OR a.registrationno = @pin )     ");
                    sql.Append("  and A.BillDateTime >= @startDate  and A.BillDateTime < @endDate ");
                    sql.Append("  AND a.HasCompanyLetter = 0 AND B.testdoneby IS NULL AND B.testdonedatetime IS NULL ");
                    sql.Append("   ");
                }

                if (patientype == "0")
                {
                    sql.Append("  union all ");
                    sql.Append("   ");
                }
                if (patientype != "2")
                {
                    sql.Append("  select  ROW_NUMBER() OVER(PARTITION By  cast(b.SlNo as varchar(20))  order by (select  0)) as slno, ");
                    sql.Append("  'IPCR-'+ cast(b.SlNo as varchar(20)) as invoiceId   ");
                    sql.Append("  ,ca.Code + ' - '+co.Code+' - '+co.Name as CurrentAcct ");
                    sql.Append(",(@SA+'.' + REPLICATE('0',10-(LEN(CONVERT(Varchar(10),ip.registrationno)))) + CONVERT(   Varchar(10),ip.registrationno)) PIN ");
                    sql.Append(",CONVERT(Varchar(10),a.BillNo) +' - '+cast((a.EditPrice * a.EditQuantity ) - (a.Discount *   a.EditQuantity) - (a.DeductableAmount * a.EditQuantity)  as varchar(20)) as billno  ");
                    sql.Append("  ,(a.DeductableAmount * a.EditQuantity) as deductible ");
                    sql.Append("  ,a.Datetime as Actualdate, a.EditOrderDateTime as CurrentDate ");
                    sql.Append("  ,A.ITEMCODE +' - '+A.ITEMNAME as Item ");
                    sql.Append("  ,e.Name as Department ");
                    sql.Append("  , 'Not Done' as ReportStatus ");
                    sql.Append("   ");
                    sql.Append("  from HIS.dbo.ARIPBillItemDetail A  ");
                    sql.Append("  inner join HIS.dbo.ARIPBILL B on A.BillNo = B.BillNo ");
                    sql.Append("  inner join HIS.dbo.RequestedTest C on A.ItemID = C.ServiceID and A.OrderID = C.OrderID ");
                    sql.Append("  left join HIS.dbo.category ca on b.CategoryId = ca.ID ");
                    sql.Append("  left join HIS.dbo.Company co on b.CompanyId = co.ID ");
                    sql.Append("  left join HIS.dbo.Department e on a.DepartmentId = e.ID ");
                    sql.Append("  left join AllInpatients ip on b.IPID = ip.IPID ");
                    sql.Append("  where  A.ServiceId = 13 and   ");
                    sql.Append("  (@departmentid = 0 OR A.DepartmentId = @departmentid)  ");
                    sql.Append(" and  (@catId = 0 OR ca.ID = @catId)    ");
                    sql.Append(" and  (@companyId = 0 OR co.ID = @companyId)    ");
                    sql.Append(" and  ((LEN(@pin)) = 0 OR ip.registrationno = @pin )     ");
                    sql.Append("  and B.InvoiceDateTime >=@startDate  and B.InvoiceDateTime < @endDate  ");
                    sql.Append("  AND B.HasCompanyLetter = 0 AND c.testdoneby IS NULL AND c.testdonedatetime IS NULL ");
                    sql.Append("   ");
                }

                dataTable = dbHelper.ExecuteSQLAndReturnDataTable(sql.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }


        public DataTable getCLDetailedReport(string stdate, int billtype)
        {
         
            var dataTable = new DataTable();
            
            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@month",stdate.Trim().Split('-')[0]),
                                   new SqlParameter("@SummaryLevel","0"),
                                    new SqlParameter("@ipop",billtype.ToString()),
                                  
                                   new SqlParameter("@year",stdate.Trim().Split('-')[1]),
                           

                };

               // dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[test]");
               dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[GetPostedCLDetails]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        public DataTable CLRevenueRecon(string fromDate, string toDate, int CaseType)
        {
       
            var dataTable = new DataTable();
 
            string casename = "";
            if (CaseType == 0)
            { casename = "0";  }
            if (CaseType == 1)
            { casename = "Reconciled"; }
            if (CaseType == 2)
            { casename = "Not Reconciled"; }
            if (CaseType == 3)
            { casename = "No CL Generated"; }
            if (CaseType == 4)
            { casename = "No Revenue Generated";  }

            try
            {
                dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@fromDate",fromDate.ToString()),
                                   new SqlParameter("@toDate",toDate.ToString()),
                                   new SqlParameter("@CaseType",casename.ToString())
                                 
 
                };

              dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[GetCLvsRevenueReport]");
              //  dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[test]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }
            return dataTable;
        }

        

    }
}
