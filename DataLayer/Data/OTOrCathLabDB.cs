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
    public class OTOrCathLabDB
    {
        DBHelper DB = new DBHelper("OTOrCathLabDB");



        public DataTable OTCathLabIsDone(DateTime StartDate, DateTime EndDate)
        {

            
        try
            {
            var dataTable = new DataTable();
             
                DB.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", StartDate.ToString()),
                                   new SqlParameter("@endate",EndDate.ToString()),
                                
                                 };


                dataTable = DB.ExecuteSPAndReturnDataTable("[MCRS].[OperationTheatre_GetCathLabDoneNew]");


                return dataTable;

            }
             catch (Exception ex)
             {
                 throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
             }



        }



        public DataTable OTOperationSelectionIsDONE(DateTime StartDate,DateTime EndDate)
        {
             try
            {
            var dataTable = new DataTable();
                DB.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", StartDate.ToString()),
                                   new SqlParameter("@endate",EndDate.ToString()),
                                
                                 };
                DB.ExecuteSPAndReturnDataTable("[MCRS].[OperationTheatre_GetOrUpdateServiceName]");

                DB.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", StartDate.ToString()),
                                   new SqlParameter("@endate",EndDate.ToString()),
                                
                                 };


                dataTable = DB.ExecuteSPAndReturnDataTable("[MCRS].[OperationTheatre_GetOrReportsDone_ALL]");


                return dataTable;

            }
             catch (Exception ex)
             {
                 throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
             }

        }


        public DataTable OTOperationSelection(DateTime StartDate, DateTime EndDate)
        {
            try
            {

                StringBuilder strb = new StringBuilder();
                strb.Append("SELECT  a.PIN, a.REQ_DATE, a.SERVICE_CODE,a.SERVICE_NAME,a.PROC_SEQ_NO," +
                         "  a.AREA_CODE, b.wipro_pin from or_request_procedure a, patient b  " +
                         " WHERE a.pin=b.pin " +
                         " and a.req_date BETWEEN TO_DATE('" + StartDate.ToString() + "','MM/DD/YYYY') AND TO_DATE('" + EndDate.ToString() + "','MM/DD/YYYY')   " +
                         "  and (a.service_code not like 'FMCAT%' and a.service_code not like 'FMVAS%')  " +
                          " and nvl(a.service_code,'xxx') <> 'xxx'" +
                             " ORDER BY a.proc_seq_no ");
                var selectFromOracle = DB.ExecuteQueryInORA(strb.ToString());

               // selectFromOracle.Rows.Count();
                foreach (DataRow row in selectFromOracle.Rows)
                {
                    
                    var array = row.ItemArray.Select(p => p.ToString()).ToArray();
                    foreach (var item in array) // Loop over the items.
                    {

                        var pinTrim = row[0].ToString().Remove(0, 4);//JD-01493398-7
                        var pin = pinTrim.Remove(7, 2);
                      
                        var reqDate = row[1].ToString();
                        var serviceCode = row[2].ToString();
                        var serviceName = row[3].ToString();
                        var procSeqNo = row[4].ToString();
                        var areaCode = row[5].ToString();
                        var wiProPin = row[6].ToString();
                        //1292450
                    var selectFromOrcath = DB.ExecuteSQLAndReturnDataTable("SELECT registrationno from ORCATHCONSOLIDATED WHERE registrationno= '" + pin + "' AND proc_seq_no='" + procSeqNo + "'");
                    
                if (selectFromOrcath.Rows.Count > 0)
                    {
                        //foreach (DataRow row1 in selectFromOrcath.Rows) // Loop over the items.
                        // {
                            
                        //     var array1 = row1.ItemArray.Select(p => p.ToString()).ToArray();

                        //     foreach (var item1 in array1) // Loop over the items.
                        //     {
                        //         var regNo = row1[0].ToString();
                        //     }

                        // }


                        //uncomment this after test

                          var strsql212 ="insert into ORCATHCONSOLIDATED (registrationno,req_date,itemcode,itemname, proc_seq_no, area_code,operationtype) " + 
                              "VALUES('" + pin + "','" + reqDate + "','" + serviceCode + "','" + serviceName + "','" + procSeqNo + "','" + areaCode + "','O')";
                          //DB.ExecuteSQLNonQuery(strsql212);
                    
                          //1493398

                        var rstOrTrack = DB.ExecuteSQLAndReturnDataTable(" SELECT ipid,WARDROOM,timeor,timetheatre,recovery,status,operatorid,"+
                        "modifieddatetime,deleted,timeout,posted,surgeon,asstsurgeon,anest   FROM ORTRACK where registrationno= '" + pin + "' and itemcode = '" + procSeqNo + "' And Convert(Date, timeor) >= '" + reqDate + "'");
                        if (rstOrTrack.Rows.Count > 0)
                        {
                            foreach (DataRow row2 in rstOrTrack.Rows) // Loop over the items.
                            { // 14 rows
                                var array2 = row2.ItemArray.Select(p => p.ToString()).ToArray();
                                foreach (var item2 in array2) // Loop over the items.
                                {
                                    var ipid = row2[0].ToString();
                                    var WARDROOM = row2[1].ToString();
                                    var timeor = row2[2].ToString();
                                    var timetheatre = row2[3].ToString();
                                    var recovery = row2[4].ToString();
                                    var status = row2[5].ToString();
                                    var operatorid = row2[6].ToString();
                                    var modifieddatetime = row2[7].ToString();
                                    var deleted = row2[8].ToString();
                                    var timeout = row2[9].ToString();
                                    var posted = row2[10].ToString();
                                    var surgeon = row2[11].ToString();
                                    var asstsurgeon = row2[12].ToString();
                                    var anest = row2[13].ToString();

                                    var strsql2 = " update ORCATHCONSOLIDATED set ipid= '" + ipid + "'," +
                                                  " WardRoom = " + "'" + WARDROOM + "'," +
                                                  " TimeOR=" + "'" + timeor + "'," +
                                                  " TimeTheatre = " + "'" + timetheatre  + "'," +
                                                  " Recovery= " + "'" + recovery + "'," +
                                                  " Status= " + "'" + status + "'," +
                                                  " OperatorID= " + "'" + operatorid + "'," +
                                                  " ModifiedDateTime = " + "'" + modifieddatetime + "'," +
                                                  " Deleted= '" + deleted + "'," +
                                                  " TimeOut= '" + timeout + "'," +
                                                  " posted= '" + posted + "'," +
                                                  " surgeon = '" + surgeon + "'," +
                                                  " AsstSurgeon= " + " '" + asstsurgeon + "'," +
                                                  " Anest = " + "'" + anest  + "'" +
                                                  " WHERE registrationno= " + "'" + pin + "'" +
                                                  " and proc_seq_no= " + procSeqNo +
                                                  " and req_date = " + "'" + reqDate + "'";

                                    // DB.ExecuteSQLNonQuery(strsql2);
                                  

                                }

                            }

                        }
                       
                    }

                }

                }

                 //ViewReport("SP_Get_OR_Reports_All '" & FormatDatePlus(stDate.Value, "DD_MMM_YYYY") & "','" & FormatDatePlus(DateAdd(DateInterval.Day, 1, enDate.Value), "DD_MMM_YYYY") & "'")
                var dataTable = new DataTable();
                DB.param = new SqlParameter[]{
                                   new SqlParameter("@stdate", StartDate.ToString()),
                                   new SqlParameter("@endate",EndDate.ToString()),
                                
                                 };

                dataTable = DB.ExecuteSPAndReturnDataTable("[MCRS].[OperationTheatreReport_GetOrReportsAll]");


                return dataTable;
                //return selectFromOracle;//DB.ExecuteQueryInORA(strb.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
            }
        }


    }
}
