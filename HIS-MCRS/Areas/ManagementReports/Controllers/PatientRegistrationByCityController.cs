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
    public class PatientRegistrationByCityController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PatientRegistrationByCityModel param)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            var patientregistrationbycityModel = new System.Data.DataTable();

            if (param.ReportType == 1)
            {
                patientregistrationbycityModel = _clPatientStatisticsDB.PatientRegistration(param.StartDate, param.EndDate, param.City);
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsCity", _clPatientStatisticsDB.GetCityById(param.City)));

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\PatientRegistrationByCity.rdl";
                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                reportViewer.LocalReport.SetParameters(new ReportParameter("CHeader", "List of Registered Patients"));
                reportViewer.LocalReport.SetParameters(new ReportParameter("City", param.City));
            }
            else if (param.ReportType == 2)
            {
                patientregistrationbycityModel = _clPatientStatisticsDB.PatientAdmission(param.StartDate, param.EndDate, param.City);
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsCity", _clPatientStatisticsDB.GetCityById(param.City))); //_clPatientStatisticsDB.GetCity()));

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\PatientRegistrationByCity.rdl";
                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                reportViewer.LocalReport.SetParameters(new ReportParameter("CHeader", "List of Admitted Patients"));
                reportViewer.LocalReport.SetParameters(new ReportParameter("City", param.City));
            }
            else
            {
                patientregistrationbycityModel = _clPatientStatisticsDB.PatientSummary(param.StartDate, param.EndDate);

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\PatientRegistrationByCitySummary.rdl";
                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", param.StartDate));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", param.EndDate));
                reportViewer.LocalReport.SetParameters(new ReportParameter("CHeader", "Registered Patients Summary"));
            }

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsPatientRegistrationByCity", patientregistrationbycityModel));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);
            ViewBag.ReportViewer = reportViewer;

            return View(param);
        }

        public JsonResult GetCity()
        {
            var getcityModel = _clPatientStatisticsDB.GetCity();
            //getspecialityModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });

            var json = getcityModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
