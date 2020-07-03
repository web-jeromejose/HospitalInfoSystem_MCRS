using DataLayer.Data;
using DataLayer.Data.Common;
using DataLayer.Model;
using DataLayer.Model.Common;
using HIS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class OPTariffController : BaseController
    {
        //
        // GET: /MCRS/OPTariff/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult get_tariff_Department(string mtbl)
        {
            OPTariffDB _DB = new OPTariffDB();
            List<CommonDropdownModel> _RE = _DB.get_tariff_Department(mtbl);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<CommonDropdownModel>() }),
                ContentType = "application/json"
            };
            return result;
        }
        public ActionResult get_Tariff_Price(int tid, int dept, string mtbl)
        {
            OPTariffDB _DB = new OPTariffDB();
            List<OPTariffModel> _RE = _DB.get_Tariff_Price(tid, dept, mtbl);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { Res = _RE ?? new List<OPTariffModel>() }),
                ContentType = "application/json"
            };
            return result;
        }
    }
}
