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
    public class PatientStatisticByNationalityController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamIDandDateModel param)
        {
            var patientnationalitystatisticsModel = _clPatientStatisticsDB.PatientNationalityStatistics(param.Id, param.DateFrom, param.DateTo); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\PatientNationalityStatistics.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsPatientNationalityStatistics", patientnationalitystatisticsModel));
            reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.DateFrom));
            reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", param.DateTo));
            reportViewer.LocalReport.SetParameters(new ReportParameter("Nation", param.Id.ToString()));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public JsonResult GetNationality()
        {
            var nationalityModel = _clPatientStatisticsDB.GetNationality();
            nationalityModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL"});
            var json = nationalityModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
