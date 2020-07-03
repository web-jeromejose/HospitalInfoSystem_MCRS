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
    public class DoctorAnalysisController : Controller
    {
        //
        // GET: /MCRS/DoctorAnalysis/

        OPRevenueDB opRevenueDB = new OPRevenueDB();
        IPDischargeDB ipDischargeDB = new IPDischargeDB();
        CompanyDB companyDB = new CompanyDB();
        EmployeeDB employeeDB = new EmployeeDB();
        DepartmentDB departmentDB = new DepartmentDB();
        IPRevenueDB ipRevenueDB = new IPRevenueDB();
        CategoryDB categoryDB = new CategoryDB();
        AdjustmentsDB adjustmentDB = new AdjustmentsDB();
        DoctorAnalysisDB DoctorAnalysisDB = new DoctorAnalysisDB();


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult DoctorLeaveHistory()
        {
            var viewModel = new DADoctorLeaveHistory()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DepartmentList = departmentDB.getAllDepartmentByDistinct(),
            };
            return View(viewModel);
        }


        public ActionResult ClinicalTimeAnalysis()
        {
            var viewModel = new DAClinicalTimeAnalysis()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DepartmentList = departmentDB.getAllDepartmentByDistinct(),
            };
            return View(viewModel);
        }

        public ActionResult PcAdminLeaveHistory()
        {
            var viewModel = new DAPcAdminLeaveHistory()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EmpList = employeeDB.getAllEmpByDeptId(),
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult PcAdminLeaveHistory(DAPcAdminLeaveHistory viewmodel)
        {

            var vm = viewmodel;
            if (Request.IsAjaxRequest())
            {
                //sama
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                reportData = DoctorAnalysisDB.getPCAdminLeaveHistory(vm.StartDate, vm.EndDate.AddDays(1), vm.EmployeeId);
                reportDocPath = @"\Areas\ManagementReports\Reports\DoctorAnalysis\PcAdminLeaveHistory.rdl";

                DateTime dateTime = DateTime.Today;

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", vm.StartDate.ToString("dd/MM/yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", vm.EndDate.ToShortDateString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("DoctorName", vm.EmpName.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printdate", dateTime.ToString("dd/MM/yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printtime", dateTime.ToString("H:i:s")));
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


        [HttpPost]
        public ActionResult DoctorLeaveHistory(DADoctorLeaveHistory viewmodel)
        {

            var vm = viewmodel;
            if (Request.IsAjaxRequest())
            {
                //sama
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                reportData = DoctorAnalysisDB.getDoctorLeaveHistory(vm.StartDate, vm.EndDate.AddDays(1), vm.DoctorId);
                reportDocPath = @"\Areas\ManagementReports\Reports\DoctorAnalysis\DoctorLeaveHistory.rdl";

                DateTime dateTime = DateTime.Today;

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", vm.StartDate.ToString("dd/MM/yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", vm.EndDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("DoctorName", vm.DoctorName.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printdate", dateTime.ToString("dd/MM/yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printtime", dateTime.ToString("H:i:s")));
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



        [HttpPost]
        public ActionResult ClinicalTimeAnalysis(DAClinicalTimeAnalysis viewmodel)
        {

            var vm = viewmodel;
            if (Request.IsAjaxRequest())
            {
                //sama
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                reportData = DoctorAnalysisDB.getDoctorClinicalAnalysis(vm.StartDate, vm.EndDate.AddDays(1), vm.DoctorId);
                reportDocPath = @"\Areas\ManagementReports\Reports\DoctorAnalysis\DoctorClinicalAnalysis.rdl";

                DateTime dateTime = DateTime.Today;

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", vm.StartDate.ToString("dd/MM/yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", vm.EndDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("DoctorName", vm.DoctorName.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printdate", dateTime.ToString("dd/MM/yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printtime", dateTime.ToString("H:i:s")));
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

        public ActionResult AppointmentSchedAnalysis()
        {
            var viewModel = new DAAppointmentSchedAnalysis()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DepartmentList = departmentDB.getAllDepartmentByDistinct(),
            };
            return View(viewModel);
        }

        

          [HttpPost]
        public ActionResult AppointmentSchedAnalysis(DAAppointmentSchedAnalysis viewmodel)
        {

            var vm = viewmodel;
            if (Request.IsAjaxRequest())
            {
                //sama
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                reportData = DoctorAnalysisDB.getDoctorAppointmentSched(vm.StartDate, vm.EndDate.AddDays(1), vm.DoctorId);
                reportDocPath = @"\Areas\ManagementReports\Reports\DoctorAnalysis\DoctorSchedAnalysis.rdl";

                DateTime dateTime = DateTime.Today;

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", vm.StartDate.ToString("dd/MM/yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", vm.EndDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("DoctorName", vm.DoctorName.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printdate", dateTime.ToString("dd/MM/yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printtime", dateTime.ToString("H:i:s")));
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


        public ActionResult PatientMovementAnalysis()
        {
            var viewModel = new DAPatientMovementAnalysis() {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                DepartmentList = departmentDB.getAllDepartmentByDistinct(),
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult PatientMovementAnalysis(DAPatientMovementAnalysis viewmodel)
        {

            var vm = viewmodel;
            if (Request.IsAjaxRequest())
            {
                //sama
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();

                reportData = DoctorAnalysisDB.getPatientMovement(vm.StartDate, vm.EndDate.AddDays(1),vm.DoctorId);
                reportDocPath = @"\Areas\ManagementReports\Reports\DoctorAnalysis\PatientMovementAnalysis.rdl";

                DateTime dateTime = DateTime.Today;

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", vm.StartDate.ToString("dd/MM/yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", vm.EndDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("DoctorName", vm.DoctorName.ToString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printdate", dateTime.ToString("dd/MM/yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("printtime", dateTime.ToString("H:i:s")));
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



        public ActionResult SearchDoctors(string searchString)
        {
            return Json(employeeDB.findDoctorByDepartmentId(searchString), JsonRequestBehavior.AllowGet);
        }

    }
}
