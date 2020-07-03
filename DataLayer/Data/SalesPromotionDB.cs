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
    public class SalesPromotionDB
    {


        DBHelper dbHelper = new DBHelper("SalesPromotionDB"); 

        public DataTable getSPCensusGraph(DateTime startDate, DateTime endDate, int CategoryId)
        {
            try
            {
                // @stDate datetime,@enDate datetime,@mhid int)   
                dbHelper.param = new SqlParameter[]{
                    new SqlParameter("@stDate ", startDate),
                    new SqlParameter("@enDate ", endDate),
                    new SqlParameter("@mhid",CategoryId)
                };
                return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_CensusGraphs]");
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }
        public DataTable getSPDailyIncome(DateTime startDate, DateTime endDate,string ReportType)
        {
            try
            { 
                if (ReportType == "2")
                {
                                dbHelper.param = new SqlParameter[]{
                                new SqlParameter("@stDate ", startDate),
                                new SqlParameter("@enDate ", endDate),
                
                            };

                    return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_DailyIncomeDept]");
                 }
                else if (ReportType == "1")
                {
                                    dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@stDate ", startDate),
                                    new SqlParameter("@enDate ", endDate),
                
                                };
                    return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_DailyIncomeOffice]");

                }
                else
                {
                                dbHelper.param = new SqlParameter[]{
                                new SqlParameter("@stDate ", startDate),
                                new SqlParameter("@enDate ", endDate),
                
                            };
                    return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_DailyIncome]");

                }

               
             
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }

        public DataTable getDailyCensus(DateTime startDate, DateTime endDate)
        {
            try
            {
                // @stDate datetime,@enDate datetime,@mhid int)   
                dbHelper.param = new SqlParameter[]{
                    new SqlParameter("@stDate ", startDate),
                    new SqlParameter("@enDate ", endDate),
          
                };
                return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_DailyCensus]");
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }

        public DataTable getSPDailyIncomeHistoricalReport(DateTime startDate, DateTime endDate, string ReportType)
        {
            try
            {

        //BYOFFICE = 0,
        //BYDEPARTMENT = 1,
        //BYDOCTOR = 2

                if (ReportType == "BYOFFICE")
                {
                    dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@stDate ", startDate),
                                    new SqlParameter("@enDate ", endDate),
                                    new SqlParameter("@mhid ", "0"),
                                   // new SqlParameter("@toDate ", endDate.ToShortDateString())
                            };

                    return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_IncomeHistorical]");
                }
                else if (ReportType == "BYDEPARTMENT")
                {
                    dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@stDate ", startDate),
                                    new SqlParameter("@enDate ", endDate),
                                    new SqlParameter("@mhid ", "0"),
                                    //new SqlParameter("@toDate ", endDate.ToShortDateString())
                
                                };
                    return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_IncomeHistoricalDept]");

                }
                else
                {
                    dbHelper.param = new SqlParameter[]{
                                new SqlParameter("@stDate ", startDate),
                                new SqlParameter("@enDate ", endDate),
                                new SqlParameter("@mhid ", "0"),
                               new SqlParameter("@toDate ", endDate.ToShortDateString())
                
                            };
                    return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_IncomeHistoricalDoc]");

                }



            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }

        public DataTable getDailyActualvsBudget(string startDate, DateTime endDate, string Branch)
        {
            try
            {


                
        //SGH_JEDDAH = 0,
        //SGH_ASEER = 1,
        //SGH_RIYADH = 2,
        //SGH_MADINAH = 3,
        //SGH_SANAA = 4,
        //SGH_DUBAI = 5,
        //SGH_CAIRO = 6,


        //         If rpt1.Checked = True Then

        //    If CType(cboBranch.SelectedItem, ValueDescriptionPair).Value = 1 Then
        //        ViewReport1("SP_Get_DoctorsDailyTarget '" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(enDate.Value, "DD_MMM_YYYY") & "','" & xtoDate & "'")
        //    ElseIf CType(cboBranch.SelectedItem, ValueDescriptionPair).Value = 3 Then
        //        ViewReport1("SP_Get_DoctorsDailyTargetRiyadh '" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(enDate.Value, "DD_MMM_YYYY") & "','" & xtoDate & "'")
        //    ElseIf CType(cboBranch.SelectedItem, ValueDescriptionPair).Value = 4 Then
        //        ViewReport1("SP_Get_DoctorsDailyTargetMadinah '" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(enDate.Value, "DD_MMM_YYYY") & "','" & xtoDate & "'")
        //    ElseIf CType(cboBranch.SelectedItem, ValueDescriptionPair).Value = 5 Then
        //        ViewReport1("SP_Get_DoctorsDailyTargetSanaa '" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(enDate.Value, "DD_MMM_YYYY") & "','" & xtoDate & "'")
        //    End If

        //Else
        //    If CType(cboBranch.SelectedItem, ValueDescriptionPair).Value = 1 Then
        //        ViewReport1("SP_Get_DepartmentDailyTarget '" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(enDate.Value, "DD_MMM_YYYY") & "','" & xtoDate & "'")
        //    ElseIf CType(cboBranch.SelectedItem, ValueDescriptionPair).Value = 3 Then
        //        ViewReport1("SP_Get_DepartmentDailyTargetRiyadh '" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(enDate.Value, "DD_MMM_YYYY") & "','" & xtoDate & "'")
        //    ElseIf CType(cboBranch.SelectedItem, ValueDescriptionPair).Value = 4 Then
        //        ViewReport1("SP_Get_DepartmentDailyTargetMadinah '" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(enDate.Value, "DD_MMM_YYYY") & "','" & xtoDate & "'")
        //    ElseIf CType(cboBranch.SelectedItem, ValueDescriptionPair).Value = 5 Then
        //        ViewReport1("SP_Get_DepartmentDailyTargetSanaa '" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(enDate.Value, "DD_MMM_YYYY") & "','" & xtoDate & "'")
        //    End If

                if (Branch == "SGH_JEDDAH")
                {
                    dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@stDate ", startDate ),
                                    new SqlParameter("@enDate ", endDate ),
                                    new SqlParameter("@mhid ", "0") 
                
                            };

                    return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_IncomeHistorical]");
                }
                else if (Branch == "BYDEPARTMENT")
                {
                    dbHelper.param = new SqlParameter[]{
                                    new SqlParameter("@stDate ", startDate),
                                    new SqlParameter("@enDate ", endDate),
                                    new SqlParameter("@mhid ", "0") 
                                };
                    return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_IncomeHistoricalDept]");

                }
                else
                {
                    dbHelper.param = new SqlParameter[]{
                                new SqlParameter("@stDate ", startDate),
                                new SqlParameter("@enDate ", endDate),
                                new SqlParameter("@mhid ", "0") 
                
                            };
                    return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SalesPromotionReport_IncomeHistoricalDoc]");

                }



            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }


        public DataTable getActualMAgCard(DateTime startDate, DateTime endDate, int deptId,string empid)
        {
            try
            {
                 dbHelper.param = new SqlParameter[]{
                    new SqlParameter("@stDate ", startDate),
                    new SqlParameter("@enDate ", endDate),
                    new SqlParameter("@DeptID ", deptId),
                    new SqlParameter("@empid ", empid.ToString()),
          
                };
                return dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[SP_Get_EmployeeActualMagcard]");
            }
            catch (Exception ex)
            {

                throw new ApplicationException(Errors.ExemptionMessage(ex));
            }

        }
  


    }
}
