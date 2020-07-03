using DataLayer.Data;
using DataLayer.Model;
using HIS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class IPBillCorrectionController : BaseController
    {
        //
        // GET: /MCRS/IPBillCorrection/

        public ActionResult Index()
        {
            ViewBag.MyOperatorId = OperatorId;
            ViewBag.MyStationId = StationId;
            return View();
        }

        [HttpPost]
        public ActionResult get_IPBill_Admission_List(long pin)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            List<IPBillADModel> _RE = _DB.get_IPBill_Admission_List(pin);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<IPBillADModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_IPBill_PT_Information(long billno)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            IPBillInfo _RE = _DB.get_IPBill_PT_Information(billno);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new IPBillInfo(), rcode = _DB.ret, rmsg = _DB.retmsg }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_IPBill_PosNeg_Adj(long billno)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            PosNegAdj _RE = _DB.get_IPBill_PosNeg_Adj(billno);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new PosNegAdj() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_IPBill_Services(long billno)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            List<BillServices> _RE = _DB.get_IPBill_Services(billno);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<BillServices>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_IPBill_Services_All(int catid, long comid, long graid)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            List<BillServices> _RE = _DB.get_IPBill_Services_All(catid, comid, graid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<BillServices>() }),
                ContentType = "application/json"
            };
            return result;
        }
        
        [HttpPost]
        public ActionResult get_IPBill_Items(long billno, int serid)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            List<BillItemListModel> _RE = _DB.get_IPBill_Items(billno, serid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<BillItemListModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_IPBill_ServiceItems(int serid, long comid, long graid)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            List<IPServicesItemsModel> _RE = _DB.get_IPBill_ServiceItems(serid, comid, graid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<IPServicesItemsModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

         [HttpPost]
        public ActionResult get_IPBill_ServiceItems_Price(int serid, long itemid, int bedid, int dedtype, int packid, int tariffid, int catid, long comid, long graid)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            IPItemPriceModel _RE = _DB.get_IPBill_ServiceItems_Price(serid, itemid, bedid, dedtype, packid, tariffid, catid, comid, graid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new IPItemPriceModel() }),
                ContentType = "application/json"
            };
            return result;
        }

         [HttpPost]
         public ActionResult get_PHItem_UOM(long itemid)
         {
             IPBillCorrectionDB _DB = new IPBillCorrectionDB();
             List<PHItemUOM> _RE = _DB.get_PHItem_UOM(itemid);
             var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
             var result = new ContentResult
             {
                 Content = serializer.Serialize(new { Res = _RE ?? new List<PHItemUOM>() }),
                 ContentType = "application/json"
             };
             return result;
         }

        [HttpPost]
         public JsonResult save_IPBill_Correction(int serid, long billno, BillAddSaveParams IPB)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            if (_DB.save_ipbill_corrections(OperatorId, serid, billno, IPB))
            {
                return Json(new { rcode = 0, rmsg = _DB.retmsg });
            }
            else
            {
                return Json(new { rcode = _DB.ret, rmsg = _DB.retmsg });
            }
        }

        [HttpPost]
        public JsonResult update_IPBill_Corrections(int serid, long billno, 
            long slno, float disc, float ded, int eqty, float eprice, string code, string name,
            long itemid, string dtime)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            if (_DB.update_ipbill_corrections(OperatorId, serid, billno, slno, disc, ded, eqty, eprice, code, name, itemid, dtime))
            {
                return Json(new { rcode = 0, rmsg = _DB.retmsg });
            }
            else
            {
                return Json(new { rcode = _DB.ret, rmsg = _DB.retmsg });
            }
        }

        [HttpPost]
        public JsonResult delete_IPBill_Correction(int serid, long billno, ARIPBillDelParams IPB)
        {
            IPBillCorrectionDB _DB = new IPBillCorrectionDB();
            if (_DB.delete_ipbill_corrections(OperatorId, serid, billno, IPB))
            {
                return Json(new { rcode = 0, rmsg = _DB.retmsg });
            }
            else
            {
                return Json(new { rcode = _DB.ret, rmsg = _DB.retmsg });
            }
        }
    }

}
