using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
 

namespace DataLayer.Data
{
   public class OTOrderDB
    {
       DBHelper dbHelper = new DBHelper("OT Order");

       public DataTable getSurgeryRecordSummary(DateTime from, DateTime to, int doctorId, int stationId, int sort,bool IsWithQty)
       {
           var dataTable = new DataTable();

           try
           {
               //dbHelper.param = new SqlParameter[]{
               //                    new SqlParameter("@from", from.ToString()),
               //                    new SqlParameter("@to", to.ToString()),
               //                    new SqlParameter("@DepartmentId","0"),
               //                    new SqlParameter("@DoctorId","0"),
               //                    new SqlParameter("@Sort","2")
               //                  };

               dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@from", from.ToString()),
                                   new SqlParameter("@to", to.ToString()),
                                   new SqlParameter("@DepartmentId", stationId.ToString()),
                                   new SqlParameter("@DoctorId", doctorId.ToString()),
                                   new SqlParameter("@Sort", sort.ToString())
                                 };


               if (IsWithQty)
               {
                   dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[RptSurgeryRecordSummaryWithQty]");
               
               }
               else { dataTable = dbHelper.ExecuteSPAndReturnDataTable("[OT].[RptSurgeryRecordSummary]"); 
               }

         
           }
           catch (Exception ex)
           {
               throw new ApplicationException(Errors.ExemptionMessage(ex));
           }
           return dataTable;
       }

       public DataTable getEmployeeByCategory(DateTime StartDate, DateTime EndDate, string option)
       {
           var dataTable = new DataTable();
           try
           {
               dbHelper.param = new SqlParameter[] {
                        new SqlParameter("@stdate",StartDate),
                        new SqlParameter("@endate",EndDate),
                        new SqlParameter("@optional",option)

                    };
               dataTable = dbHelper.ExecuteSPAndReturnDataTable("[MCRS].[OtherReports_GetEODStats]");
           }
           catch (Exception ex)
           {
               throw new ApplicationException(Errors.ExemptionMessage(ex));
           }

           return dataTable;
       }




       public DataTable getListofORDone(DateTime from, DateTime to)
       {
           StringBuilder strb = new StringBuilder();
           try
           {

           strb.Append("select *  " +
                        " FROM OT.RequestMain where OPER_DATE >='" + from + "' and OPER_DATE < '" + to + "'");
           var checkIfExistinSQLByDate = dbHelper.ExecuteSQLAndReturnDataTable(strb.ToString());
              
           if (checkIfExistinSQLByDate.Rows.Count == 0)
           {

                StringBuilder strb1 = new StringBuilder();
                strb1.Append("SELECT * FROM OR_REQUEST_MAIN WHERE   OPER_DATE >= TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY') and OPER_DATE <  TO_DATE('" + to.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016


     
               var selectedORA = dbHelper.ExecuteQueryInORA(strb1.ToString());

               if (selectedORA.Rows.Count > 0) 
                           {
                             
                               foreach (DataRow row in selectedORA.Rows)
                               {
                                   var pin = row[0].ToString();
                                   var Age = row[1].ToString();
                                   var AgeUnit = row[2].ToString();
                                   var reqDate = row[3].ToString();
                                   var reqTime = row[4].ToString();
                                   var userIdReq = row[5].ToString();
                                   var operDate = row[6].ToString();
                                   var operTime = row[7].ToString();
                                   var surgeon = row[8].ToString();
                                   var Anaesthetist = row[9].ToString();
                                   var AnaestType = row[10].ToString();
                                   var roomNo = row[11].ToString();
                                   var OrNo = row[12].ToString();
                                   var seqNo = row[13].ToString();
                                   var userIdSeq = row[14].ToString();
                                   var doneDate = row[15].ToString();
                                   var doneTime = row[16].ToString();
                                   var doneStatus = row[17].ToString();
                                   var userIdDone = row[18].ToString();
                                   var finalInd = row[19].ToString();
                                   var remarks = row[20].ToString();
                                   var transOutTime = row[21].ToString();
                                   var userIdTransOut = row[22].ToString();
                                   var acctTypeCode = row[23].ToString();
                                   var acctTypeDesc = row[24].ToString();
                                   var chargeAcct = row[25].ToString();
                                   var nurseRemarks = row[26].ToString();
                                   var areaCode = row[27].ToString();
                                   var telNo = row[28].ToString();
                                   var reqSeqNo = row[29].ToString();
                                   var origOperDateReq = row[30].ToString();
                                   var origReqSeqNo = row[31].ToString();     
                                       var query = "";

                                       query = "INSERT INTO OT.RequestMain " +
"  (PIN,AGE,AGE_UNIT,REQ_DATE,REQ_TIME,USER_ID_REQ,OPER_DATE,OPER_TIME,SURGEON,ANAESTHETIST,ANAEST_TYPE,ROOM_NO,OR_NO,SEQ_NO,USER_ID_SEQ" +
"  ,DONE_DATE,DONE_TIME,DONE_STATUS,USER_ID_DONE,FINAL_IND,REMARKS,TRANS_OUT_TIME,USER_ID_TRANS_OUT,ACCOUNT_TYPE_CODE,ACCOUNT_TYPE_DESC,CHARGE_ACCT" +
"  ,NURSE_REMARKS,AREA_CODE,TEL_NO,REQ_SEQNO,ORIG_OPERDATE_REQ,ORIG_REQ_SEQNO) " +
"  VALUES ('" + pin + "','" + Age + "','" + AgeUnit + "','" + reqDate + "','" + reqTime + "','" + userIdReq + "','" + operDate + "','" + operTime + "'" +
"   ,'" + surgeon + "','" + Anaesthetist + "','" + AnaestType + "','" + roomNo + "','" + OrNo + "','" + seqNo + "','" + userIdSeq + "'" +
"   ,'" + doneDate + "','" + doneTime + "','" + doneStatus + "','" + userIdDone + "','" + finalInd + "','" + remarks + "','" + transOutTime + "'" +
"  ,'" + userIdTransOut + "','" + acctTypeCode + "','" + acctTypeDesc + "','" + chargeAcct + "','" + nurseRemarks + "'" +
"  ,'" + areaCode + "','" + telNo + "','" + reqSeqNo + "','" + origOperDateReq + "','" + origReqSeqNo + "')";

                                      dbHelper.ExecuteSQLNonQuery(query);
                               }

                               StringBuilder strb2 = new StringBuilder();

                               strb2.Append("SELECT * FROM OR_REQUEST_PROCEDURE WHERE REQ_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  REQ_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')  "); //WHERE   OPER_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  OPER_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')   ");//TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY') and OPER_DATE <  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016


                               var selected2 = dbHelper.ExecuteQueryInORA(strb2.ToString());

                               if (selected2.Rows.Count > 0)
                               {

                                   foreach (DataRow row in selected2.Rows)
                                   {
                                       var query = "";
                                       query = "INSERT INTO OT.RequestProcedure  (PIN ,REQ_DATE,SERVICE_CODE,SERVICE_NAME,PROC_SEQ_NO,AREA_CODE) " +
               "VALUES ('" + row[0].ToString() + "','" + row[1].ToString() + "','" + row[2].ToString() + "','" + row[3].ToString() + "','" + row[4].ToString() + "','" + row[5].ToString() + "')";
                                       dbHelper.ExecuteSQLNonQuery(query);
                                   }

                               }
                               StringBuilder strb3 = new StringBuilder();

                               strb3.Append("SELECT * FROM OR_REQUEST_SURGEON WHERE REQ_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  REQ_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')  "); //WHERE   OPER_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  OPER_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')   ");//TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY') and OPER_DATE <  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016
                    var selected3 = dbHelper.ExecuteQueryInORA(strb3.ToString());

                    if (selected3.Rows.Count > 0)
                    {
                        foreach (DataRow row in selected3.Rows)
                        {
                        var query = "";
                        query = "INSERT INTO OT.RequestSurgeon (PIN,REQ_DATE,DOC_CODE,DOC_SEQ_NO,AREA_CODE)"+
                            "VALUES ('" + row[0].ToString() + "','" + row[1].ToString() + "','" + row[2].ToString() + "','" + row[3].ToString() + "','" + row[4].ToString() + "')";
                        dbHelper.ExecuteSQLNonQuery(query);
                         }
                    }





                           }
                           else // exist in the db
                           {
                           }
    
           }
           else //already exist
           {

           }

//               select * from Ot.requestmain a
//left join Ot.requestsurgeon b on a.PIN = b.PIN 
//left join OT.requestProcedure c on a.PIN = c.PIN
//left join Ot.requestList d on a.PIN = d.PIN
//where a.oper_date >= '2016-08-09' and a.oper_date < '2016-08-10' 
//order by a.pin desc

           var query1 = "select  PIN,AGE,AGE_UNIT,REQ_DATE,REQ_TIME,USER_ID_REQ,OPER_DATE,OPER_TIME,SURGEON,ANAESTHETIST,ANAEST_TYPE,ROOM_NO,OR_NO,SEQ_NO,USER_ID_SEQ,DONE_DATE,DONE_TIME"+
" ,DONE_STATUS,USER_ID_DONE,FINAL_IND,REMARKS,TRANS_OUT_TIME,USER_ID_TRANS_OUT,ACCOUNT_TYPE_CODE,ACCOUNT_TYPE_DESC,CHARGE_ACCT,NURSE_REMARKS,AREA_CODE,TEL_NO,REQ_SEQNO"+
",ORIG_OPERDATE_REQ,ORIG_REQ_SEQNO from OT.RequestMain where OPER_DATE >='" + from + "'  and OPER_DATE <  '" + to + "' ";

           return dbHelper.ExecuteSQLAndReturnDataTable(query1); 
              
           }
           catch (Exception ex)
           {
               throw new ApplicationException("Error Message:</b> <br /> " + ex.Message + "<br /><br /><b>Stack Trace:</b><br /> " + ex.StackTrace);
           }
       }
       public string convertQuotes(string str)
       {
           return str.Replace("'", "''");
       }

    }
}
