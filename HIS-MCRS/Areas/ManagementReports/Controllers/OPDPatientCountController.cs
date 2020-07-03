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
    public class OPDPatientCountController : BaseController
    {
        private readonly PolyClinicDB _clPolyClinicDB = new PolyClinicDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ParamIDandDateModel param)
        {
            var opdpatientcountModel = _clPolyClinicDB.OPDPatientCount(param.DateFrom, param.DateTo, param.Id); //_clReportDAL.GetReportProcedureDoneListDAL(DateFrom, DateTo);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            if (param.Id == 0)
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\OpdPatientCount.rdl";
                reportViewer.LocalReport.SetParameters(new ReportParameter("FromDate", param.DateFrom));
                reportViewer.LocalReport.SetParameters(new ReportParameter("ToDate", param.DateTo));
            }
            else
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Areas\ManagementReports\Reports\PolyClinic\OpdRevisitPatientCount.rdl";
                reportViewer.LocalReport.SetParameters(new ReportParameter("FromDate", param.DateFrom));
                reportViewer.LocalReport.SetParameters(new ReportParameter("ToDate", param.DateTo));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Label", param.Id == 1 ? "Patient Revisit Against Total Number/Department Report" : "Number of New Patients Visited the Hospital Report"));
            }

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsOPDPatientCount", opdpatientcountModel));
            reportViewer = this.DynamicReportHeader(reportViewer, "DataSet2");
            reportViewer.LocalReport.DisplayName = base.SaveFilestreamtoPDF(reportViewer);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public JsonResult GetSelection()
        {
            var selectionlist = new List<GenericListModel>
            {
                new GenericListModel {id = 0, name = "Patient Visits Count", text = "Patient Visits Count"},
                new GenericListModel {id = 1, name = "Patient Revisits Count", text = "Patient Revisits Count"},
                new GenericListModel {id = 2, name = "New Patient Count", text = "New Patient Count"}
            };

            var json = selectionlist.DefaultIfEmpty();
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
