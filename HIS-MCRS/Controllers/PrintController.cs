using HIS_MCRS.Common;
using HIS_MCRS.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS_MCRS.Controllers
{
    public class PrintController : Controller
    {
        //
        // GET: /Print/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Preview()
        {
            var reportViewer = System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName];

            if (reportViewer != null)
            {
                try
                {
                    var ms = Common.Helper.createFileMemoryStream(reportViewer as ReportViewer, "PDF");

                    return new FileStreamResult(ms, "application/pdf");
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("out of memory"))
                    {
                        return Content("CANNOT GENERATE PDF, FILE IS TO LARGE");
                    }
                    else
                    {
                        return Content(e.Message);
                    }
                }
            }

            return Content("NO FILE");
        }
    }
}
