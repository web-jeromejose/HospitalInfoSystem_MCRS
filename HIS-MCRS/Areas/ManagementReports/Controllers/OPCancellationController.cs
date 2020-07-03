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
    public class OPCancellationController : BaseController
    {
        private readonly PatientStatisticsDB _clPatientStatisticsDB = new PatientStatisticsDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamIDandDateModel param)
        {
            var oppha_cancelledreceiptModel = _clPatientStatisticsDB.OPPHA_CancelledReceipt(param.Id, param.DateFrom, param.DateTo); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PatientStatistics\OPPHA_CancelledReceipt.rdl";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsOPPHA_CancelledReceipt", oppha_cancelledreceiptModel));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsServiceType", _clPatientStatisticsDB.GetServiceTypeWhereID(param.Id.ToString())));
            reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", param.DateFrom));
            reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", param.DateTo));
            reportViewer.LocalReport.SetParameters(new ReportParameter("serviceid", param.Id.ToString()));
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public JsonResult GetServiceType()
        {
            var getservicetypeModel = _clPatientStatisticsDB.GetServiceType();
            //nationalityModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });
            var json = getservicetypeModel.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
