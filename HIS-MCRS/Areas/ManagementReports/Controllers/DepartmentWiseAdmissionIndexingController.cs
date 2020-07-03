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
    public class DepartmentWiseAdmissionIndexingController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamBoolandDateModel param)
        {
            var mrdindexingModel = _clPatientStatisticsDB.MRDIndexing(param.IntValue, param.DateFrom, param.DateTo); 

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\MRDIndexing.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsMRDIndexing", mrdindexingModel));
            reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.DateFrom));
            reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", param.DateTo));
            reportViewer.LocalReport.SetParameters(new ReportParameter("ex", param.IntValue.ToString()));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

    }
}
