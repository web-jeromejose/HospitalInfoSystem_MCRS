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
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class ICUAdmissionWithDiagnosisController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            //ICUAdmissionWithDiagnosisModel m = new ICUAdmissionWithDiagnosisModel();
            //m.Diagnosis = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Index(ICUAdmissionWithDiagnosisModel param)
        {
            var icuadmissionwithdiagnosisModel = _clPatientStatisticsDB.ICUAdmissionwithDiagnosis(param.StartDate, param.EndDate, param.Diagnosis, param.LengthOfStay); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\ICUAdmissionWithDiagnosis.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsICUAdmissionwithDiagnosis", icuadmissionwithdiagnosisModel));
            reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
            reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
            reportViewer.LocalReport.SetParameters(new ReportParameter("IcdId", param.Diagnosis.ToString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("DayCount", param.LengthOfStay.ToString()));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);
            ViewBag.ReportViewer = reportViewer;

            return View(param);
        }

        public JsonResult GetDiagnosis(string name)
        {
            var getdiagnosisModel = new List<GenericListModel>();
            getdiagnosisModel = string.IsNullOrEmpty(name) ? _clPatientStatisticsDB.GetDiagnosis(null) : _clPatientStatisticsDB.GetDiagnosis(name);
            getdiagnosisModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });

            var json = getdiagnosisModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
