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

       public DataTable getSurgeryRecordSummary(DateTime from, DateTime to, int doctorId, int stationId, int sort)
       {
           var dataTable = new DataTable();

           try
           {
               dbHelper.param = new SqlParameter[]{
                                   new SqlParameter("@from", from.ToString()),
                                   new SqlParameter("@to", to.ToString()),
                                   new SqlParameter("@DepartmentId", stationId.ToString()),
                                   new SqlParameter("@DoctorId", doctorId.ToString()),
                                   new SqlParameter("@Sort", sort.ToString())
                                 };

               dataTable = dbHelper.ExecuteSPAndReturnDataTable("[OT].[RptSurgeryRecordSummary]");
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

           //check if OPER_TIME is exist

           strb.Append("select *  " +
                        " FROM OT.RequestMain where OPER_DATE >='" + from + "' and OPER_DATE < '" + from + "'");
           var checkIfExistinSQLByDate = dbHelper.ExecuteSQLAndReturnDataTable(strb.ToString());
              
           if (checkIfExistinSQLByDate.Rows.Count == 0)
           {

//START for testing ONLY
                //StringBuilder strb2 = new StringBuilder();
                //strb2.Append("SELECT count(1) FROM OR_REQUEST_MAIN WHERE   OPER_DATE =  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016
                //var test = dbHelper.ExecuteQueryInORA(strb2.ToString());

                //StringBuilder strb3 = new StringBuilder();
                //strb3.Append("SELECT b.*  FROM OR_REQUEST_MAIN_DEL a inner join OR_REQUEST_MAIN b on a.OPER_DATE = b.OPER_DATE    where b.OPER_DATE =  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')   "); //'8/24/2016
                //var test3 = dbHelper.ExecuteQueryInORA(strb3.ToString());

// END for testing ONLY

                StringBuilder strb1 = new StringBuilder();
                strb1.Append("SELECT * FROM OR_REQUEST_MAIN WHERE   OPER_DATE >= TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY') and OPER_DATE <  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016

              // strb1.Append("SELECT * FROM OR_REQUEST_MAIN WHERE   OPER_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  OPER_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')   ");//TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY') and OPER_DATE <  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016

               //strb1.Append("SELECT * FROM OR_REQUEST_PROCEDURE WHERE REQ_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  REQ_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')  "); //WHERE   OPER_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  OPER_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')   ");//TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY') and OPER_DATE <  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016

               //strb1.Append("SELECT * FROM OR_REQUEST_SURGEON WHERE REQ_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  REQ_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')  "); //WHERE   OPER_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  OPER_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')   ");//TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY') and OPER_DATE <  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016

               //strb1.Append("SELECT * FROM OR_REQUEST_LIST ");// WHERE REQ_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  REQ_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')  "); //WHERE   OPER_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  OPER_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')   ");//TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY') and OPER_DATE <  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016

              // strb1.Append("SELECT * FROM OR_REQUEST_SURGEON_DEL WHERE REQ_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  REQ_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')  "); //WHERE   OPER_DATE >= TO_DATE('08-01-2016','MM-DD-YYYY') and  OPER_DATE <= TO_DATE('08-25-2016','MM-DD-YYYY')   ");//TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY') and OPER_DATE <  TO_DATE('" + from.ToShortDateString() + "','MM-DD-YYYY')  "); //'8/24/2016
              
                
                
//insert in OR_REQUEST_MAIN OR_REQUEST_PROCEDURE OR_REQUEST_SURGEON OR_REQUEST_LIST OR_REQUEST_MAIN_DEL OR_REQUEST_PROCEDURE_DEL OR_REQUEST_SURGEON_DEL
             

               var selectedORA = dbHelper.ExecuteQueryInORA(strb1.ToString());

               if (selectedORA.Rows.Count > 0) 
                           {
                               //not yet exist in the db   
                               //OR_REQUEST_MAIN .Rows.Count = 614 
                               //OR_REQUEST_PROCEDURE.Rows.Count = 708 
                               //OR_REQUEST_SURGEON 191
                               // OR_REQUEST_LIST 30
                               // OR_REQUEST_MAIN_DEL 00
                               //OR_REQUEST_PROCEDURE_DEL 00
                               //OR_REQUEST_SURGEON_DEL 00

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


//                                       query = "INSERT INTO OT.RequestProcedure  (PIN ,REQ_DATE,SERVICE_CODE,SERVICE_NAME,PROC_SEQ_NO,AREA_CODE) " +
//"VALUES ('" + row[0].ToString() + "','" + row[1].ToString() + "','" + row[2].ToString() + "','" + row[3].ToString() + "','" + row[4].ToString() + "','" + row[5].ToString() + "')";



 //query = "INSERT INTO OT.RequestSurgeon (PIN,REQ_DATE,DOC_CODE,DOC_SEQ_NO,AREA_CODE)"+
 //    "VALUES ('" + row[0].ToString() + "','" + row[1].ToString() + "','" + row[2].ToString() + "','" + row[3].ToString() + "','" + row[4].ToString() + "')";




//  query = "INSERT INTO HIS.OT.RequestList(SEQ_NO,PT_NAME,ACCT_TYPE ,CHARGE_ACCOUNT_NAME ,ROOM_NO,PIN,AGE,AGE_UNIT,SEX,OR_NO "+
//        "   ,OPER_TIME,SERVICE_NAME1,SERVICE_NAME2,SERVICE_NAME3,SERVICE_NAME4,SERVICE_NAME5,SERVICE_NAME6,SERVICE_NAME7,SERVICE_NAME8,SERVICE_NAME9"+
//        "   ,SERVICE_NAME10,SERVICE_NAME11,SERVICE_NAME12,SURGEON,SURGEON1,SURGEON2,SURGEON3,SURGEON4,SURGEON5,SURGEON6,ANAESTHETIST_NAME,ANAEST_TYPE"+
//       "    ,REMARKS1,REMARKS2,REMARKS3,REMARKS4)"+
//   "  VALUES"+
//"        ('" + convertQuotes(row[0].ToString()) + "'" +
//"        ,'" + convertQuotes(row[1].ToString()) + "'" +
//"      ,'" + convertQuotes(row[2].ToString()) + "'" +
//"        ,'" + convertQuotes(row[3].ToString()) + "'" +
//"      ,'" + convertQuotes(row[4].ToString()) + "'" +
//"       ,'" + convertQuotes(row[5].ToString()) + "'" +
//",'" + convertQuotes(row[6].ToString()) + "'" +
//",'" + convertQuotes(row[7].ToString()) + "'" +
//",'" + convertQuotes(row[8].ToString()) + "'" +
//",'" + convertQuotes(row[9].ToString()) + "'" +
//",'" + convertQuotes(row[10].ToString()) + "'" +
//",'" + convertQuotes(row[11].ToString()) + "'" +
//",'" + convertQuotes(row[12].ToString()) + "'" +
//",'" + convertQuotes(row[13].ToString()) + "'" +
//",'" + convertQuotes(row[14].ToString()) + "'" +
//",'" + convertQuotes(row[15].ToString()) + "'" +
//",'" + convertQuotes(row[16].ToString()) + "'" +
//",'" + convertQuotes(row[17].ToString()) + "'" +
//",'" + convertQuotes(row[18].ToString()) + "'" +
//",'" + convertQuotes(row[19].ToString()) + "'" +
//",'" + convertQuotes(row[20].ToString()) + "'" +
//",'" + convertQuotes(row[21].ToString()) + "'" +
//",'" + convertQuotes(row[22].ToString()) + "'" +
//",'" + convertQuotes(row[23].ToString()) + "'" +
//",'" + convertQuotes(row[24].ToString()) + "'" +
//",'" + convertQuotes(row[25].ToString()) + "'" +
//",'" + convertQuotes(row[26].ToString()) + "'" +
//",'" + convertQuotes(row[27].ToString()) + "'" +
//",'" + convertQuotes(row[28].ToString()) + "'" +
//",'" + convertQuotes(row[29].ToString()) + "'" +
//",'" + convertQuotes(row[30].ToString()) + "'" +
//",'" + convertQuotes(row[31].ToString()) + "'" +
//",'" + convertQuotes(row[32].ToString()) + "'" +
//",'" + convertQuotes(row[33].ToString()) + "'" +
//",'" + convertQuotes(row[34].ToString()) + "'" +
//",'" + convertQuotes(row[35].ToString()) + "')";

                                      dbHelper.ExecuteSQLNonQuery(query);

                               }
                           


                           }
                           else // exist in the db
                           {
                           }



           }
           else //already exist
           {

           }



           var query1 = "select * from OT.RequestMain where OPER_DATE ='" + from + "' ";

           return dbHelper.ExecuteSQLAndReturnDataTable(query1); 


           //strb.Append("SELECT SEQ_NO,PT_NAME,ROOM_NO,PIN,AGE,AGE_UNIT,SEX,OR_NO,OPER_TIME," +
           //                                       " SERVICE_NAME1,SURGEON,ANAESTHETIST_NAME,ANAEST_TYPE," +
           //                                      " REMARKS1,ACCT_TYPE,SERVICE_NAME2,SURGEON1,REMARKS2" +
           //                                " FROM OR_REQUEST_LIST " +
           //                                " ORDER BY SEQ_NO");
           //return dbHelper.ExecuteQueryInORA(strb.ToString());
              
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
