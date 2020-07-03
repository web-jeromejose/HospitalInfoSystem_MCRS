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
    public class AuditReportsDB
    {
        DBHelper dbhelper = new DBHelper("AuditReportsDB");

        public DataTable getIPOPXrayCharge(DateTime StartDate, DateTime EndDate, int BillType,int ChargeOrBill)
        {
            var dataTable = new DataTable();
            try
            {

                // If opt1.Checked = True Then
                //    ViewReport("AR.RPT_IPOPChargedXray '" & FormatDatePlus(dtpSDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(DateAdd(DateInterval.Day, 1, dtpEDate.Value), "DD_MMM_YYYY") & "'," & cboChargeType.SelectedIndex)
                //Else
                //    ViewReport("AR.RPT_IPOPBilledXray '" & FormatDatePlus(dtpSDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(DateAdd(DateInterval.Day, 1, dtpEDate.Value), "DD_MMM_YYYY") & "'," & cboChargeType.SelectedIndex)

                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@DateFrom",StartDate),
                        new SqlParameter("@DateTo",EndDate),
                        new SqlParameter("@BillType",BillType),

                    };

                if (ChargeOrBill == 0)
                {
                      dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReport_IPOPXrayCharged]");
                }
                else
                {
                    dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReports_IPOPBilledXray]");
                }

             

              
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }
        

             
        public DataTable getOPCancelledBillByDept(DateTime StartDate, DateTime EndDate, int BillType)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@StartDate",StartDate),
                        new SqlParameter("@EndDate",EndDate),
                        new SqlParameter("@BillType",BillType),

                    };

                    dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReports_OPCancelledBillByDept]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getSublist(DateTime StartDate, DateTime EndDate, int catID)
        {
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@StartDate",StartDate),
                        new SqlParameter("@EndDate",EndDate),
                        new SqlParameter("@categoryId",catID)

                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReports_GetSubCategoryList]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }
        public DataTable getDischargeInfo(DateTime StartDate, DateTime EndDate, string DoctorId,int DocOrNone)
        {
            var dataTable = new DataTable();
            try
            {

                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@StartDate",StartDate),
                        new SqlParameter("@EndDate",EndDate),
                        new SqlParameter("@WithDoctor",DocOrNone),
                         new SqlParameter("@DocCode",DoctorId),

                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReports_DischargeIntimation]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getIPChargeBilledReport(DateTime StartDate, DateTime EndDate, int ChargedORBilled, int ChargedType, int AccountType, string DoctorId, string ServiceID, string CategoryId, string CompanyId)
        {
            var dataTable = new DataTable();
            try
            {

              
                 
                    dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),
                        new SqlParameter("@charge",ChargedORBilled),
                        new SqlParameter("@doctorId",DoctorId),
                        new SqlParameter("@serviceId",ServiceID),
                        new SqlParameter("@categoryId",CategoryId),
                        new SqlParameter("@companyId",CompanyId),
                        new SqlParameter("@accountType",AccountType),
                        new SqlParameter("@chargeType",ChargedType),

                    };

                    dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReports_IPBillCharge]");

               

     
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getGradeConsultation(DateTime StartDate, DateTime EndDate, string cat, int type)
        {
            // FormatDatePlus(dtpSDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(DateAdd(DateInterval.Day, 1, dtpEdate.Value), "DD_MMM_YYYY") & "','" & DirectCast(cboCategory.SelectedItem, ValueDescriptionPair).Value() & "'," & cboType.SelectedIndex)
            var dataTable = new DataTable();
            try
            {
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stDate",StartDate),
                        new SqlParameter("@enDate",EndDate),
                        new SqlParameter("@cat",cat),
                        new SqlParameter("@Type",type) 
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReport_GradeConsultation]");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }



        public DataTable getCompanyProfileLogs(DateTime StartDate, DateTime EndDate, string Cat, int Subcat, String SPNAME)
        {
            var dataTable = new DataTable();
            try
            {

                   

                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stDate",StartDate),
                        new SqlParameter("@enDate",EndDate),
                        new SqlParameter("@cat",Cat),
                        new SqlParameter("@sublevel",Subcat) 
                    };
                dataTable = dbhelper.ExecuteSPAndReturnDataTable(SPNAME);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }
        public DataTable getOPBillCharge(DateTime StartDate, DateTime EndDate, string DocID, string serviceId, string CatId, string CompanyId, int Ischarge, int Billed)
        {
            var dataTable = new DataTable();
            try
            {

                     //         [MCRS].[AuditReport_Get_OP_Charge_And_Billing_New]  
                     //                  @DoctorID  INT  
                     //, @ServiceID INT  
                     //, @CategoryID INT  
                     //, @CompanyID INT  
                     //, @From   DATETIME  
                     //, @To   DATETIME  
                     //, @IsCharge  BIT  
                     //, @Billed  BIT 
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@From",StartDate),
                        new SqlParameter("@To",EndDate),
                        new SqlParameter("@DoctorID",DocID),
                        new SqlParameter("@ServiceID",serviceId),
                        new SqlParameter("@CategoryID",CatId),
                        new SqlParameter("@CompanyID",(CompanyId == null ? "0" : CompanyId )),
                        new SqlParameter("@IsCharge",Ischarge),
                        new SqlParameter("@Billed",Billed),
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReport_Get_OP_Charge_And_Billing_New]");




            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        public DataTable getArOpBillMonthWise(DateTime StartDate, DateTime EndDate, string CatId, string CompanyId)
        {
            var dataTable = new DataTable();
            try
            {

 
                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@FromDate",StartDate),
                        new SqlParameter("@ToDate",EndDate),
                        new SqlParameter("@CategoryID",CatId),
                        new SqlParameter("@CompanyID",(CompanyId == null ? "0" : CompanyId )),
                      
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReports_AROPBillReportMonthWise]");




            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }

        public DataTable getSummaryBillPercentage(DateTime StartDate, DateTime EndDate, string CatId, string CompanyId, string docId)
        {
            var dataTable = new DataTable();
            try
            {
 

                dbhelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),
                        new SqlParameter("@docId",docId),
                        new SqlParameter("@catId",CatId),
                        new SqlParameter("@compId",(CompanyId == null ? "0" : CompanyId )),
                      
                    };

                dataTable = dbhelper.ExecuteSPAndReturnDataTable("[MCRS].[AuditReports_getSummaryBillPercentage]");




            }
            catch (Exception ex)
            {
                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

            return dataTable;
        }


        
    }
}
