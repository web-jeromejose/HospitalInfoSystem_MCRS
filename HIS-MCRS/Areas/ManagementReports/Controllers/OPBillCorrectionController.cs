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
    public class OPBillCorrectionController : BaseController
    {

        public ActionResult Index()
        {
            ViewBag.MyOperatorId = OperatorId;
            ViewBag.MyStationId = StationId;
            return View();
        }

        [HttpPost]
        public ActionResult get_opbservice_items(int serid)
        {
            OPBillCorrectionDB _DB = new OPBillCorrectionDB();
            List<OPBillItemsModel> _RE = _DB.get_opbservice_items(serid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<OPBillItemsModel>() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_opb_correction_details(long pin, string fdate, string tdate)
        {
            OPBillCorrectionDB _DB = new OPBillCorrectionDB();
            List<OPBillCorrectionDetailsModel> _RE = _DB.get_opb_correction_details(pin, fdate, tdate);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<OPBillCorrectionDetailsModel>(), rcode = _DB.ret, rmsg = _DB.retmsg }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_item_price(int serid, int itemid, int catid, long comid, long graid, long docid)
        {
            OPBillCorrectionDB _DB = new OPBillCorrectionDB();
            OPBillItemPriceModel _RE = _DB.get_item_price(serid, itemid, catid, comid, graid, docid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new OPBillItemPriceModel() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult get_item_uom(int itemid)
        {
            OPBillCorrectionDB _DB = new OPBillCorrectionDB();
            PharItemsUOMModel _RE = _DB.get_item_uom(itemid);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new PharItemsUOMModel() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult recalculate_bill (int catid, long comid, long graid, int serid, long itemid, int depid, decimal price)
        {
            OPBillCorrectionDB _DB = new OPBillCorrectionDB();
            BillRecalculationModel _RE = _DB.recalculate_bill(catid, comid, graid, serid, itemid, depid, price);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new BillRecalculationModel() }),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public JsonResult save_opbill_correction(OPBillCorrectionSaveModel OPB)
        {
            OPBillCorrectionDB _DB = new OPBillCorrectionDB();
            if (_DB.save_opbill_correction(OperatorId, StationId, OPB))
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
