using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Data;
using HIS_MCRS.Areas.ManagementReports.ViewModels;
using Microsoft.Reporting.WebForms;
using DataLayer;
using System.Data;
using HIS_MCRS.Models;
using HIS_MCRS.Common;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class OperationTheatreController : Controller
    {
        CompanyDB companyDB = new CompanyDB();
        EmployeeDB employeeDB = new EmployeeDB();
        OTOrderDB otOrderDB = new OTOrderDB();
        DepartmentDB departmentDB = new DepartmentDB();
        OTOrCathLabDB OTOrCathLabDB = new OTOrCathLabDB();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SurgeryRecordSummary()
        {
            var viewModel = new OperationTheatreSurgeryRecordSummary() { 
             From = DateTime.Now,
             To = DateTime.Now,
             Departments = departmentDB.getAllDepartment()
            
            };

            return View(viewModel);
        }
       
        [HttpPost]
        public ActionResult SurgeryRecordSummary(OperationTheatreSurgeryRecordSummary viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = otOrderDB.getSurgeryRecordSummary(viewModel.From, viewModel.To.AddDays(1), viewModel.DoctorId, viewModel.DepartmentId, viewModel.SortMode, viewModel.IsWithQty);

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));


                if (viewModel.IsWithQty)
                {
                    reportDocPath = @"\Areas\ManagementReports\Reports\OperationTheatre\RptSurgeryRecordSummaryWithQty.rdl";
                
                }
                else
                {
                    reportDocPath = @"\Areas\ManagementReports\Reports\OperationTheatre\RptSurgeryRecordSummary.rdl";
                
                }

               
               //reportDocPath = @"\Areas\ManagementReports\Reports\OperationTheatre\RptSurgeryRecordSummary_final.rdl";
             
               
                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("from", viewModel.From.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("to", viewModel.To.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("aaaCompanyName", Global.OrganizationDetails.Name+" - " +Global.OrganizationDetails.City.ToUpper()));
                reportViewer.SizeToReportContent = true;
                reportViewer.ShowPrintButton = true;
                reportVM.ReportViewer = reportViewer;

                System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
            }

            return View();

        }

        public ActionResult OTOrCathLabOperation()
        {
            var viewModel = new OTOrCathLabOperation()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                OperationOrCatLab = "0",
                isDone = true
            };


            return View(viewModel);
        }
        [HttpPost]
        public ActionResult OTOrCathLabOperation(OTOrCathLabOperation operationcath)
        {
            var vm = operationcath;
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                if (true) //vm.isDone not working force true muna :)
                {
                    if (vm.OperationOrCatLab == "0")//Operation
                    {
                                 DataTable reportData = OTOrCathLabDB.OTOperationSelectionIsDONE(vm.StartDate, vm.EndDate.AddDays(1));
                                    if (reportData.Rows.Count == 0)
                                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                                    reportDocPath = @"\Areas\ManagementReports\Reports\OperationTheatre\RptOperationIsDone.rdl";
                                    reportViewer.ProcessingMode = ProcessingMode.Local;
                                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                                    ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                                    reportViewer.LocalReport.DataSources.Add(datasourceItem);
                                    reportViewer.LocalReport.SetParameters(new ReportParameter("from", vm.StartDate.ToString("dd-MMM-yyyy")));
                                    reportViewer.LocalReport.SetParameters(new ReportParameter("to", vm.EndDate.ToString("dd-MMM-yyyy")));
                                    reportViewer.LocalReport.SetParameters(new ReportParameter("aaaCompanyName", Global.OrganizationDetails.Name+" - " +Global.OrganizationDetails.City.ToUpper()));
                                    reportViewer.SizeToReportContent = true;
                                    reportViewer.ShowPrintButton = true;
                                    reportVM.ReportViewer = reportViewer;

                                    System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                                    System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                                    return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);

                    }
                    else
                    { //cath lab


                        DataTable reportData = OTOrCathLabDB.OTCathLabIsDone(vm.StartDate, vm.EndDate.AddDays(1));
                                    if (reportData.Rows.Count == 0)
                                        return Content(Errors.ReportContent("NO RECORDS FOUND"));
                                    reportDocPath = @"\Areas\ManagementReports\Reports\OperationTheatre\RptCathLabIsDone.rdl";
                                    reportViewer.ProcessingMode = ProcessingMode.Local;
                                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                                    ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                                    reportViewer.LocalReport.DataSources.Add(datasourceItem);
                                    reportViewer.LocalReport.SetParameters(new ReportParameter("from", vm.StartDate.ToString("dd-MMM-yyyy")));
                                    reportViewer.LocalReport.SetParameters(new ReportParameter("to", vm.EndDate.ToString("dd-MMM-yyyy")));
                                    reportViewer.LocalReport.SetParameters(new ReportParameter("aaaCompanyName", Global.OrganizationDetails.Name+" - " +Global.OrganizationDetails.City.ToUpper()));
                                    reportViewer.SizeToReportContent = true;
                                    reportViewer.ShowPrintButton = true;
                                    reportVM.ReportViewer = reportViewer;

                                    System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                                    System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                                    return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
                    }

                }
                else // not done
                {

                    if (vm.OperationOrCatLab == "0")//Operation to be continuee....
                    {
                        DataTable reportData = OTOrCathLabDB.OTOperationSelection(vm.StartDate, vm.EndDate);

                    }
                    else
                    { //cath

                    }
                }
               
            }
            return View();
        }


        public ActionResult ListOrDone()
        {
            var viewModel = new OperationTheatreSurgeryRecordSummary()
            {
                From = DateTime.Now,
                To = DateTime.Now,
                Departments = departmentDB.getAllDepartment()

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ListOrDone(OperationTheatreSurgeryRecordSummary viewModel)
        {
            if (Request.IsAjaxRequest())
            {
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";


                DataTable reportData = otOrderDB.getListofORDone(viewModel.From, viewModel.From.AddDays(1));

              
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));


                reportDocPath = @"\Areas\ManagementReports\Reports\OperationTheatre\OTReportListofORDoneFromOracle.rdl";


                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("from", viewModel.From.ToString("dd-MMM-yyyy")));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("to", viewModel.To.ToString("dd-MMM-yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("aaaCompanyName", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.SizeToReportContent = true;
                reportViewer.ShowPrintButton = true;
                reportVM.ReportViewer = reportViewer;

                System.Web.HttpContext.Current.Session[Global.ReportViewerSessionName] = reportViewer;
                System.Web.HttpContext.Current.Session[Global.PdfUriSessionName] = Common.Helper.getApplicationUri("Preview", "Print", null);

                return PartialView("~/Views/Shared/_reportViewer.cshtml", reportVM);
            }

            return View();

        }



        public ActionResult SearchCompanies(string searchString)
        {
            return Json(companyDB.findCompanies(searchString), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchDoctors(string searchString)
        {
            return Json(employeeDB.findDoctors(searchString), JsonRequestBehavior.AllowGet);
        }

    }
}
