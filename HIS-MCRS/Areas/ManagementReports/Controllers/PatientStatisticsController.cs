using DataLayer.Data;
using DataLayer.Model;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HIS_MCRS.Models;
using Microsoft.Reporting.WebForms;
using System.Data;
using DataLayer;
using HIS_MCRS.Common;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class PatientStatisticsController : BaseController
    {
        OPBServiceDB opbServiceDB = new OPBServiceDB();
        EmployeeDB employeeDB = new EmployeeDB();
        PatientStatisticsDB ptStatisticsDB = new PatientStatisticsDB();
        CancelBillReasonDB cancelReason = new CancelBillReasonDB();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OPDCancellationByDoctor()
        {
            var viewModel = new PatientStatisticsOPDCancellationByDoctor() {
                 DoctorId = 0,
                 ServiceId  =0,
                 Services = opbServiceDB.getServices(),
                 Doctors  = employeeDB.getAllDoctors().OrderBy(i=>i.FullName).ToList(),
                 From = DateTime.Now,
                 To = DateTime.Now,
                 CancelBillReasons = cancelReason.getReasons()
            };



            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult OPDCancellationByDoctor(PatientStatisticsOPDCancellationByDoctor viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = ptStatisticsDB.getOPDCancellation(viewModel.From
                    , viewModel.To.AddDays(1)
                    , viewModel.ServiceId
                    , viewModel.DoctorId
                    , viewModel.SortBy
                    , viewModel.ReasonId);

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));


                if (viewModel.GroupByDoctor)
                {
                    reportDocPath = @"\Areas\ManagementReports\Reports\PatientStatistics\OPDCancellationGroupByDoctor.rdl";
                }
                else
                {
                    reportDocPath = @"\Areas\ManagementReports\Reports\PatientStatistics\OPDCancellation.rdl";
                }


                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("OPDCancellation", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("from", viewModel.From.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("to", viewModel.To.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("serviceName", viewModel.ServiceId == 0 ? "ALL" : opbServiceDB.getServiceById(viewModel.ServiceId).Name.ToUpper().Trim()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("doctorname", viewModel.DoctorId == 0 ? "ALL": employeeDB.getEmployeeByOperatorId(viewModel.DoctorId).FullName.ToUpper().Trim()));
                reportViewer.SizeToReportContent = true;
                reportViewer.ShowPrintButton = true;
                reportVM.ReportViewer = reportViewer;

                System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
            }

            return View();

        }

        public ActionResult SearchDoctors(string searchString)
        {
            return Json(employeeDB.findDoctors(searchString), JsonRequestBehavior.AllowGet);
        }



    }
}
