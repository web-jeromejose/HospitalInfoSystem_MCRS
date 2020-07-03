using DataLayer;
using DataLayer.Data;
using HIS_MCRS.Areas.ManagementReports.ViewModels;
using HIS_MCRS.Common;
using HIS_MCRS.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;


using System.IO;
using System.Security.Permissions;
using DataLayer.Model;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.Models;



namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class CorporateController : BaseController
    {

        //
        // GET: /ManagementReports/Corporate/
        CorporateDB corporate = new CorporateDB();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadTable()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LoadTable(DateTime startdate )
        {
            bool status = corporate.getCorporateLoadTable(startdate);
             return Json("done");
        }

        public ActionResult d_medde2()
        {
            //dont ask Me gnyan nag naming ng mis report sa eod.. d ko din alam kung anong report itatawag dto .. dko naman pwede lagay na YEARLY kse monthly nila gngerate..kapag monthly naman d tyo sure kung meron silang gngawang other process pa.kaya dont me..dont ask me.
            return View();
        }


    }
}
