using DataLayer;
using DataLayer.Data;
using HIS_MCRS.Areas.ManagementReports.ViewModels;
using HIS_MCRS.Common;
using HIS_MCRS.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class InventoryReportsController : Controller
    {
        //
        // GET: /ManagementReports/InventoryReports/

        InventoryReportDB invreportsdb = new InventoryReportDB();


        StationDB stationDB = new StationDB();
        EmployeeDB employeeDB = new EmployeeDB();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchOperators(string searchString)
        {
            return Json(employeeDB.findEmployee(searchString), JsonRequestBehavior.AllowGet);
        }


        public ActionResult OperatorWiseReport()
        {
             
          
            var ViewModel = new IROperatorWiseReport()
            {

                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Stations = stationDB.getStationsByOperatorWise(),
                Summary = false,
                Breakup = false
                  

            };

            return View(ViewModel);
        }
        [HttpPost]
        public ActionResult OperatorWiseReport(IROperatorWiseReport iroperatorwisereport)
        {
            var vm = iroperatorwisereport;
            if(Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();
                
                reportData = invreportsdb.getIROperatorWiseReport(vm.StartDate, vm.EndDate.AddDays(1),vm.OperatorId,vm.Summary,vm.StationId);

                if(vm.Summary)
                reportDocPath = @"\Areas\ManagementReports\Reports\InventoryReports\Report_IROperatorWise.rdl";
                else
                reportDocPath = @"\Areas\ManagementReports\Reports\InventoryReports\Report_IROperatorWiseBreakUp.rdl";

                //sama
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToString("dd-MMM-yyyy")));
             
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("StationID", vm.GenderName));
               

                reportViewer.SizeToReportContent = true;
                reportViewer.Height = Unit.Percentage(100);
                reportViewer.Width = Unit.Percentage(100);
                reportViewer.ShowPrintButton = true;
                reportVM.ReportViewer = reportViewer;

                System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);

            }

            return View();

        }

    }
}
