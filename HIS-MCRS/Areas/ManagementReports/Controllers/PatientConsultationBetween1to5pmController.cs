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
    public class PatientConsultationBetween1to5pmController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamYearandDeptModel param)
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(param.Year, 1, 1);
            DateTime lastDay = new DateTime(param.Year, 12, 31);
            var patientnationalitystatisticsModel = _clPatientStatisticsDB.ConsultationBetween1to5(param.Dept, firstDay.ToShortDateString(), lastDay.ToShortDateString()); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\ConsultationBetween1to5.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsConsultationBetween1to5", patientnationalitystatisticsModel));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsDeparment", _clPatientStatisticsDB.GetDepartment()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", firstDay.ToShortDateString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", lastDay.ToShortDateString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("xDepID", param.Dept.ToString()));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public JsonResult GetDepartment()
        {
            var departmentModel = _clPatientStatisticsDB.GetDepartment();
            departmentModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });
            var json = departmentModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetYear()
        {
            var yearModel = _clPatientStatisticsDB.GetYear();
            var json = yearModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
