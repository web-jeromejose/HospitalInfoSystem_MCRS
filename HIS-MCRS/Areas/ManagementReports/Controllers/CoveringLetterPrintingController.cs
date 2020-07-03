using DataLayer.Data;
using HIS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class CoveringLetterPrintingController : BaseController
    {
        //
        // GET: /MCRS/CoveringLetterPrinting/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult ungenerate_coveringletter(int refid)
        {
            CoveringLetterGenerationDB _DB = new CoveringLetterGenerationDB();
            if (_DB.ungenerate_coveringletter(refid, OperatorId))
            {
                return Json(new { rcode = _DB.retcode, rmsg = _DB.retmsg });
            }
            else
            {
                return Json(new { rcode = _DB.retcode, rmsg = _DB.retmsg });
            }
        }

    }
}
