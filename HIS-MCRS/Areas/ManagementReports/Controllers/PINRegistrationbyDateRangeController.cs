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
    public class PINRegistrationbyDateRangeController : BaseController
    {
        private readonly PolyClinicDB _clPolyClinicDB = new PolyClinicDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamIDandDateModel param)
        {
            var patientnationalitystatisticsModel = _clPolyClinicDB.PINRegistrationbyDateRange(param.Id, param.DateFrom, param.DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\PINRegisteredDateRange.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsGetPINRegistered", patientnationalitystatisticsModel));
            reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
            reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.DateFrom));
            reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", param.DateTo));
            reportViewer.LocalReport.SetParameters(new ReportParameter("xdep", param.Id.ToString()));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public JsonResult GetEmployee()
        {
            var getemployeeModel = _clPolyClinicDB.GetEmployee();
            getemployeeModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });
            var json = getemployeeModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
