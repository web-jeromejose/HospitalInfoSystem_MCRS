using HIS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HIS_MCRS.Areas.ManagementReports.Models;
using DataLayer.Data;
using Newtonsoft.Json;
using Microsoft.Reporting.WebForms;
using HIS_MCRS.Models;
using DataLayer.Model;
using System.Text;
using System.Data;
using HIS_MCRS.Common;
using HIS_MCRS;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using DataLayer;
using HIS_MCRS.Extension;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using HIS_MCRS.Areas.ManagementReports.ViewModels;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class OtherReportsController : BaseController
    {
        ARReportsDB arReportDb = new ARReportsDB();
        MCRSUserDB userDb = new MCRSUserDB();
        EmployeeDB empDb = new EmployeeDB();
        ICD10CodesDB icd10codeDb = new ICD10CodesDB();
        CategoryDB categoryDb = new CategoryDB();
        CompanyDB companyDb = new CompanyDB();
        GradeDB gradeDb = new GradeDB();
        UtilitiesDB utilitiesDB = new UtilitiesDB();
        GeneralInformationDB genInfoDB = new GeneralInformationDB();
        RelationshipDB relationshipDB = new RelationshipDB();
        DepartmentDB departmentDb = new DepartmentDB();
        ServicesDB serviceDb = new ServicesDB();
        TestDB testDb = new TestDB();
        ItemDB itemDb = new ItemDB();
        InvoiceDB invoiceDb = new InvoiceDB();
        OtherReportsDB OtherReportDB =  new OtherReportsDB();
        StationDB stationdb = new StationDB();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GeneralExpenseSummary()
        {
            
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult GeneralExpenseSummary(OtherReportsDateTimeOnly param)
        {
            //SP_OP_GeneralExpensesReport GeneralExpenseReport.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReports_OPGeneralExpensesReport]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\GeneralExpenseReport.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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
        public ActionResult RespiratoryProcedureCharges()
        {
           
             var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
           
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RespiratoryProcedureCharges(OtherReportsDateTimeOnly param)
        {
            //Sp_Extract_IP_RTProcedure Report_RTProcedure
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReports_ExtractIPRTProcedure]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_ExtractIPRTProcedure.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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


        public ActionResult IPChargesMoreThanTenK()
        {
           
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult IPChargesMoreThanTenK(OtherReportsDateTimeOnly param)
        {
            //SP_GetIPChargesSR Report_ListofIPCharges.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReports_GetIpChargesSR]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_GetIpChargesSR.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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


        public ActionResult OpdReceptionCancelled()
        { 
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult OpdReceptionCancelled(OtherReportsDateTimeOnly param)
        {
            //SP_Get_ReceptionistCancellation Report_ReceptionistCancelledCharges.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReports_GetReceptionistCancellation]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_GetReceptionistCancellation.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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



        public ActionResult XrayIpOpCharges()
        {
          
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult XrayIpOpCharges(OtherReportsDateTimeOnly param)
        {
            //SP_GET_XRAY_IPOP_CHARGES Report_XrayIPOPCharges.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReports_GetXrayIPOPCharges]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_GetXrayIPOPCharges.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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


        public ActionResult BedClearance()
        {
          
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult BedClearance(OtherReportsDateTimeOnly param)
        {
            //SP_Get_CleanBedReleaseApp  \Report_CleanBedRelease.rpt"
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReport_GetCleanBedRelease]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReport_GetCleanBedRelease.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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


        public ActionResult IpRevokeDischarge()
        {
            //SP_Get_IPRevokeDischarges Report_RevokeDischarges.rpt
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult IpRevokeDischarge(OtherReportsDateTimeOnly param)
        {
            //SP_Get_IPRevokeDischarges Report_RevokeDischarges.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReports_GetIPRevokeDischarges]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_GetIPRevokeDischarges.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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
        public ActionResult IpPackageDealDischarges()
        {
          
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult IpPackageDealDischarges(OtherReportsDateTimeOnly param)
        {
            //SP_GET_IP_PACKAGES Report_PackageDealDischarges.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReports_GetIPPackages]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_GetIPPackages.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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



        public ActionResult CancellationReportMonth()
        {
           
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CancellationReportMonth(OtherReportsDateTimeOnly param)
        {
            //SP_OP_Cancelled_Bills_MgtSummary_MonthlyNew Report_CancelledBillsMgtSummaryMonthlyNew.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReports_GetCancellationMonth]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_GetCancellationMonth.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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

        public ActionResult CurrentlyAdmittedCashPatients()
        {
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CurrentlyAdmittedCashPatients(OtherReportsDateTimeOnly param)
        { 
            var vm = param;
        if (Request.IsAjaxRequest())
        {
            var IsAdmitted = Request.Form["IsAdmitted"];

            ReportViewerVm reportVM = new ReportViewerVm();
            ReportViewer reportViewer = new ReportViewer();
            string reportDocPath = "";
            DataTable reportData = new DataTable();

            reportData = OtherReportDB.getCurrentlyAdmittedCashPatients(vm.StartDate, vm.EndDate.AddDays(1), IsAdmitted);
            reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_CurrentlyAdmittedCashPatients.rdl";

            if (reportData.Rows.Count == 0)
                return Content(Errors.ReportContent("NO RECORDS FOUND"));

            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
            ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
            reportViewer.LocalReport.DataSources.Add(datasourceItem);

            reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
            reportViewer.LocalReport.SetParameters(new ReportParameter("header", (IsAdmitted == "1" ? "CURRENTLY ADMITTED CASH PATIENTS WITHIN 24 HRS." : "CURRENTLY ADMITTED CASH PATIENTS MORE THAN 1 DAY")));
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



        public ActionResult EodStatSummary()
        {
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult EodStatSummary(OtherReportsDateTimeOnly param)
        {
            var vm = param;
            if (Request.IsAjaxRequest())
            {
                var IsAdmitted = Request.Form["IsAdmitted"];

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getEodStatSummary(vm.StartDate, vm.EndDate.AddDays(1), IsAdmitted);
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\getEodStatSummary.rdl";
                if (IsAdmitted == "2")
                {
                    reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\getEodStatSummaryByPatient.rdl";
                }
                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("header", (IsAdmitted == "1" ? "TOTAL NUMBER OF OUT-PATIENT/IN-PATIENT VISIT" : "TOTAL NUMBER OF DISCHARGED IN-PATIENT")));
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

        public ActionResult CancelledBillsReport()
        {
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult CancelledBillsReport(OtherReportsDateTimeOnly param)
        {
            var vm = param;
            if (Request.IsAjaxRequest())
            {
                var category = Request.Form["category"];

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getOPCancelledBills(vm.StartDate, vm.EndDate.AddDays(1), category);
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_OPCancelledBills.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("header", "Cancelled Bills Report"));//(category == "1" ? "CURRENTLY ADMITTED CASH PATIENTS WITHIN 24 HRS." : "CURRENTLY ADMITTED CASH PATIENTS MORE THAN 1 DAY")));
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



        public ActionResult DailyCancellationReport()
        {
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult DailyCancellationReport(OtherReportsDateTimeOnly param)
        {
            var vm = param;
            if (Request.IsAjaxRequest())
            {
                var category = Request.Form["category"];

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getDailyCancellationReport(vm.StartDate, vm.EndDate.AddDays(1), category);
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\getDailyCancellationReport.rdl";
                if(category == "1")
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\getDailyCancellationReport_Summary.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("header", (category == "1" ? "Cancelled Bills Report - MGT" : "Cancelled Bills Report - MGT")));
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



        public ActionResult CashPDDailyIncome()
        {
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CashPDDailyIncome(OtherReportsDateTimeOnly param)
        {
            //SP_OP_GeneralExpensesReport GeneralExpenseReport.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getStartEndDateandSP(vm.StartDate, vm.EndDate.AddDays(1), "[MCRS].[OtherReports_CashPdDailyIncome]");
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_CashPdDailyIncome.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
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

        public ActionResult RamadanIncome()
        {
            var viewModel = new OtherReportsDateTimeOnly()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RamadanIncome(OtherReportsDateTimeOnly param)
        {
            var vm = param;
            if (Request.IsAjaxRequest())
            {
                var IsAdmitted = Request.Form["DailySummary"];

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getRamadanIncome(vm.StartDate, vm.EndDate.AddDays(1), IsAdmitted);
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_ramadanIncome.rdl";

                if (IsAdmitted == "1")
                {
                    reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_ramadanIncome_all.rdl";
                }

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", param.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", param.EndDate.ToShortDateString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("header", (IsAdmitted == "1" ? "TOTAL NUMBER OF OUT-PATIENT/IN-PATIENT VISIT" : "TOTAL NUMBER OF DISCHARGED IN-PATIENT")));
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

        public ActionResult AmcDetailedDailyCollection()
        {
            var viewModel = new OtherReports_AmcDailyCollectionReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StationList = stationdb.getStations(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AmcDetailedDailyCollection(OtherReports_AmcDailyCollectionReport param)
        {
            var vm = param;
            if (Request.IsAjaxRequest())
            {
                
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getAmcDailyCollection(vm.StartDate, vm.EndDate.AddDays(1), vm.StationId);
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_amcDailyCollectionReport.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToShortDateString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("header", (IsAdmitted == "1" ? "TOTAL NUMBER OF OUT-PATIENT/IN-PATIENT VISIT" : "TOTAL NUMBER OF DISCHARGED IN-PATIENT")));
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

        public ActionResult AmcDailyRevenue()
        {
            var viewModel = new OtherReports_AmcDailyCollectionReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StationList = stationdb.getStations(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AmcDailyRevenue(OtherReports_AmcDailyCollectionReport param)
        {
            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();

                reportData = OtherReportDB.getAmcDailyCollection(vm.StartDate, vm.EndDate.AddDays(1), vm.StationId);
                reportDocPath = @"\Areas\ManagementReports\Reports\OtherReports\OtherReports_amcDailyCollectionReport.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);

                reportViewer.LocalReport.SetParameters(new ReportParameter("stdate", vm.StartDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endate", vm.EndDate.ToShortDateString()));
                //reportViewer.LocalReport.SetParameters(new ReportParameter("header", (IsAdmitted == "1" ? "TOTAL NUMBER OF OUT-PATIENT/IN-PATIENT VISIT" : "TOTAL NUMBER OF DISCHARGED IN-PATIENT")));
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

        public ActionResult PharmacyOpCancelledBillsApprover()
        {
            var viewModel = new OtherReports_AmcDailyCollectionReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StationList = stationdb.getCanopBillApprover(),
            };


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult PharmacyOpCancelledBillsApprover(OtherReports_AmcDailyCollectionReport param)
        {
            var vm = param;
                string BillNo = Request.Form["BillNo"];
                string EmployeeId = Request.Form["StationId"];

                bool success = OtherReportDB.updatePharmacyBillsApprover(EmployeeId,BillNo);
                if (!success)
                {
                    ViewBag.Error = "Please Enter the correct Bill No to update.";
                }
                else
                {
                    ViewBag.Success = "Updating Successful";
                }

            var viewModel = new OtherReports_AmcDailyCollectionReport()
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                StationList = stationdb.getCanopBillApprover(),
            };


            return View(viewModel);
        }

        public Boolean GetBillNo(string billno)
        {


            var getdiagnosisModel =   stationdb.getBillNo(billno);
            if (getdiagnosisModel.Count == 0)
            {
                return false;
            }
            else { return true; }

            
           
            //var getdiagnosisModel = new List<GenericListModel>();
            //getdiagnosisModel = string.IsNullOrEmpty(name) ? _clPatientStatisticsDB.GetDiagnosis(null) : _clPatientStatisticsDB.GetDiagnosis(name);
            //getdiagnosisModel.Add(new GenericListModel { id = 0, name = "ALL", text = "ALL" });

            //var json = getdiagnosisModel.DefaultIfEmpty();
            //return Json(json, JsonRequestBehavior.AllowGet);
        }



    }
}
