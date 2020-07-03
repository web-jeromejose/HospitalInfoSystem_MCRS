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


using System.IO;
using System.Security.Permissions;
using DataLayer.Model;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.Models;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class InfectionControlReportController : BaseController
    {
        //
        // GET: /ManagementReports/InfectionControlReport/
        InfectionControlDB InfectDB = new InfectionControlDB();
 

        public ActionResult Index()
        {
           
            return View();
        }


        public ActionResult MOHHBVReport()
        {
           
            var vm = new InfectionControlVM();
            vm.FromDate = DateTime.Now;
            vm.ToDate = DateTime.Now;
            return View(vm);
        }

        [HttpPost]
        public ActionResult MOHHBVReport(InfectionControlVM param)
        {
            var data = InfectDB.MOHHBVReport(param.FromDate.ToString(), param.ToDate.ToString());

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\InfectionControl\HepaStatisticsMOH.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.FromDate.ToString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", param.ToDate.ToString()));
            // reportViewer.LocalReport.SetParameters(new ReportParameter("IcdId", param.Diagnosis.ToString()));
            //reportViewer.LocalReport.SetParameters(new ReportParameter("DayCount", param.LengthOfStay.ToString()));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);
            ViewBag.ReportViewer = reportViewer;

            return View(param);
        }


    }
}
