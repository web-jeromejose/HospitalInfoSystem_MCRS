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
    public class ORReportController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamIDandDateModel param)
        {
            var patientnationalitystatisticsModel = _clPatientStatisticsDB.ORReport(param.DateFrom, param.DateTo); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\ORReport.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsORReport", patientnationalitystatisticsModel));
            reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.DateFrom));
            reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.DateTo));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public ActionResult ListOrDone()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ListOrDone(ParamIDandDateModel param)
        {
            var patientnationalitystatisticsModel = _clPatientStatisticsDB.GetListOfORDone(param.DateFrom, param.DateTo); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\ListOfORDone.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ListOfORDone", patientnationalitystatisticsModel));
            reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.DateFrom));
            reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.DateTo));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }
      

    }
}
