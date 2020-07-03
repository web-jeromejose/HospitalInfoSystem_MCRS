using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Data;
using DataLayer.Model;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.Models;
using Microsoft.Reporting.WebForms;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class CurrentlyAdmittedPatientListController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamBoolandDateModel param)
        {
            var mrdindexingModel = _clPatientStatisticsDB.CurrentlyAdmittedList();

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\CurrentlyAdmittedList.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsCurrentlyAdmittedList", mrdindexingModel));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

    }
}
