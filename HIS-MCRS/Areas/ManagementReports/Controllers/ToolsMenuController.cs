using DataLayer;
using DataLayer.Data;
using DataLayer.Model;
using HIS.Controllers;
using HIS_MCRS.Areas.ManagementReports.ViewModels;
using HIS_MCRS.Common;
using HIS_MCRS.Enumerations;
using HIS_MCRS.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;


namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class ToolsMenuController : Controller
    {
        //
        // GET: /ManagementReports/ToolsMenu/


        PatientStatisticsDB patientstatsDB = new PatientStatisticsDB();
        DepartmentDB departmentDB = new DepartmentDB();
        EmployeeDB employeeDB = new EmployeeDB();
        CategoryDB categoryDB = new CategoryDB();
        CompanyDB companyDB = new CompanyDB();
        MohDB MohDB = new MohDB();
        SalesPromotionDB SalesPromoDB = new SalesPromotionDB();
        EmployeeDB empDb = new EmployeeDB();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ActualPunchInPunchOut()
        {
            var viewModel = new ToolsMenuActualPunchInOutModel()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate   = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DepartmentList = departmentDB.getAllDepartment(),
                EmployeeList = empDb.getEmployeedetails(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ActualPunchInPunchOut(ToolsMenuActualPunchInOutModel ActualPunchInPunchOut)
        {
            var vm = ActualPunchInPunchOut;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = SalesPromoDB.getActualMAgCard(vm.StartDate, vm.EndDate.AddDays(1),vm.DepartmentId,vm.EmployeeId);
                reportDocPath = @"\Areas\ManagementReports\Reports\ToolsReport\Report_ActualPunchInPunchOut.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("StartDate", vm.StartDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("EndDate", vm.EndDate.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
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
