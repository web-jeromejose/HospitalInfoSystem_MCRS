using DataLayer.Data;
using DataLayer.Model;
using HIS.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class DashboardMonitoringController : BaseController
    {
        //
        // GET: /MCRS/DashboardMonitoring/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult get_DashboardMonitoring(string bdate)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            DailyDashboardMonModel _RE = _DB.get_DashboardMonitoring(bdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new DailyDashboardMonModel() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult graph_DashboardMonitoring(string bdate)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            DataSet reportData = _DB.graph_DashboardMonitoring(bdate);
           var CashList = reportData.Tables[0].AsEnumerable().Select(
               r => new CashList {
               NO_PATIENT_CASH = r.Field<int>("NO_PATIENT"),
               DeptName_CASH = r.Field<string>("DeptName")
                            });
            var ChargeList = reportData.Tables[1].AsEnumerable().Select(
               r => new CashList
               {
                   NO_PATIENT_CASH = r.Field<int>("NO_PATIENT"),
                   DeptName_CASH = r.Field<string>("DeptName")
               });
            var ARAMCOList = reportData.Tables[2].AsEnumerable().Select(
               r => new CashList
               {
                   NO_PATIENT_CASH = r.Field<int>("NO_PATIENT"),
                   DeptName_CASH = r.Field<string>("DeptName")
               });
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };

            var GraphList = new GraphList {
                CASH = new List<CashList>(CashList.ToList())
                ,CHARGE = new List<CashList>(ChargeList.ToList())
                ,ARAMCO = new List<CashList>(ARAMCOList.ToList())
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(
                                new { GraphList = GraphList }
                                 ),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_ORRequest(string bdate)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            ORDailyDashORA _RE = _DB.get_ORRequest(bdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new ORDailyDashORA() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_ORRequestFTD(string bdate)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            ORDailyDashORAFTD _RE = _DB.get_ORRequestFTD(bdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new ORDailyDashORAFTD() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_ORRequestFTDProg(string bdate)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            ORDailyDashORAOrig _RE = _DB.get_ORRequestFTDProg(bdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new ORDailyDashORAOrig() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_BI_Service_Type()
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            List<CANServiceType> _RE = _DB.get_BI_Service_Type();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<CANServiceType>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_BI_Station()
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            List<CANServiceType> _RE = _DB.get_BI_Station();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<CANServiceType>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_BI_Reason()
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            List<CANServiceType> _RE = _DB.get_BI_Reason();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<CANServiceType>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_BI_Operator()
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            List<CANServiceType> _RE = _DB.get_BI_Operator();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<CANServiceType>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_Cancellation_Stats(string fdate, string tdate,
            string sertype, string station, string billtype, string chargeby, string reason, int recfilter)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            List<CANResultModel> _RE = _DB.get_Cancellation_Stats(fdate, tdate, sertype, station, billtype, chargeby, reason, recfilter);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<CANResultModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_OPBill_Count(string fdate, string tdate)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            OPBILLActualCount _RE = _DB.get_OPBill_Count(fdate, tdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new OPBILLActualCount() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_OPBill_Amount(string fdate, string tdate)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            OPBILLActualAmount _RE = _DB.get_OPBill_Amount(fdate, tdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new OPBILLActualAmount() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_CANOPBill_Count(string fdate, string tdate, int receipts, string billtype)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            OPBILLCanCount _RE = _DB.get_CANOPBill_Count(fdate, tdate, receipts, billtype);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new OPBILLCanCount() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_CANOPBill_Amount(string fdate, string tdate, int receipts, string billtype)
        {
            PatientStatisticsDB _DB = new PatientStatisticsDB();
            OPBILLCanAmount _RE = _DB.get_CANOPBill_Amount(fdate, tdate, receipts, billtype);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new OPBILLCanAmount() }),
                ContentType = "application/json"
            };
            return result;
        }

    }
}
