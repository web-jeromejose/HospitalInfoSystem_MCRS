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
    public class PatientListByAgeRangeController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            ViewBag.Disabled = "disabled";
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamIDandDateModel param)
        {
            var agerangeModel = _clPatientStatisticsDB.GetAgeRange();
            int dateoption = string.IsNullOrEmpty(param.DateFrom) ? 1 : 2;
            var agerange = agerangeModel.FirstOrDefault(w => w.id == param.Id);
            var patientbyageModel = _clPatientStatisticsDB.PatientbyAge(agerange.startage, agerange.endage, dateoption, param.DateFrom, param.DateTo); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\PatientbyAge.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsPatientbyAge", patientbyageModel));
            reportViewer.LocalReport.SetParameters(new ReportParameter("stAge", agerange.startage));
            reportViewer.LocalReport.SetParameters(new ReportParameter("enAge", agerange.endage));
            reportViewer.LocalReport.SetParameters(new ReportParameter("xOption", dateoption.ToString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("regSTDate", param.DateFrom));
            reportViewer.LocalReport.SetParameters(new ReportParameter("regENDate", param.DateTo));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;
            ViewBag.Option = dateoption == 2 ? "checked" : null;
            ViewBag.Disabled = dateoption == 2 ? null : "disabled";

            return View();
        }

        public JsonResult GetAgeRange()
        {
            var agerangeModel = _clPatientStatisticsDB.GetAgeRange();
            var json = agerangeModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
