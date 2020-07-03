using HIS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class MainController : BaseController
    {
        //
        // GET: /MCRS/Main/

        public ActionResult Index()
        {
            return View();
        }

    }
}
