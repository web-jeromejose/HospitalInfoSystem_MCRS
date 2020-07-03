using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer;

namespace HIS.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ApplicationVersionModel apps = new ApplicationVersionModel();
            ApplicationGlobal glob = new ApplicationGlobal();
            ConstantModel cons = new ConstantModel();
            glob.UserID = this.OperatorId.ToString();
            List<ApplicationMenuModel> menu = glob.GetApplicationMenu();
            return View(menu);
            // return View();
        }


        public JsonResult ChangeStation(int stationId)
        {
            this.StationId = stationId;
            /***WARDS ONLY***/
            var result = new { };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetMenu()
        {
            ApplicationVersionModel apps = new ApplicationVersionModel();
            ApplicationGlobal glob = new ApplicationGlobal();
            ConstantModel cons = new ConstantModel();

            glob.UserID = this.OperatorId.ToString();
            List<ApplicationMenuModel> menu = glob.GetApplicationMenu();

            return PartialView("_Menu", menu);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }


    }
}
