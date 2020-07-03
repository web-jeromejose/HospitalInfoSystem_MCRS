using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Data;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.Models;
using Microsoft.Reporting.WebForms;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class OPVisitCensusController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamDateRangeModel daterange)
        {
            var opvisitcensusModel = _clPatientStatisticsDB.OPVisitCensus(daterange.DateFrom, daterange.DateTo); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\OPVisitCensus.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsOPVisitCensus", opvisitcensusModel));
            reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", daterange.DateFrom));
            reportViewer.LocalReport.SetParameters(new ReportParameter("endate", daterange.DateTo));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            //ReportPrintDocument rp = new ReportPrintDocument(reportViewer.LocalReport);
            //rp.Print();

            return View();
            //return View("CathProcedureDoneList", ViewBag.ReportViewer);

        }
    }
}
