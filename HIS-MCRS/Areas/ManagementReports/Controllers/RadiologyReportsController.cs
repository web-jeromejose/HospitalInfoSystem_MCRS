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
using HIS.Controllers;

namespace HIS_MCRS.Areas.ManagementReports.Controllers
{
    public class RadiologyReportsController : BaseController
    {
        //
        // GET: /ManagementReports/RadiologyReports/
        QpsReportDB qpsreportDb = new QpsReportDB();
        RadiologyReportDB radiologyDB = new RadiologyReportDB();
        DepartmentDB departmentDB = new DepartmentDB();
        EmployeeDB employeeDB = new EmployeeDB();
        CategoryDB categoryDB = new CategoryDB();
        CompanyDB companyDB = new CompanyDB();


        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult xrayreferral()
        {

            
            var viewModel = new RadiologyReportXrayReferral()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult xrayreferral(RadiologyReportXrayReferral param)
        {
            //SP SP_Get_XrayfromER Report_Radiology.rpt
            var vm = param;
             if (Request.IsAjaxRequest())
             {

                 var xrayreferral = radiologyDB.getXrayReferral(param.StartDate, param.EndDate.AddDays(1));


                 ReportViewerVm reportVM = new ReportViewerVm();
                 ReportViewer reportViewer = new ReportViewer();
                 string reportDocPath = "";
                 DataTable reportData = new DataTable();

                

                 reportData = radiologyDB.getXrayReferral(vm.StartDate, vm.EndDate.AddDays(1));

                 reportDocPath = @"\Areas\ManagementReports\Reports\RadiologyReports\XrayReferral.rdl";

                 if (reportData.Rows.Count == 0)
                     return Content(Errors.ReportContent("NO RECORDS FOUND"));

                 reportViewer.ProcessingMode = ProcessingMode.Local;
                 reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                 ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                 reportViewer.LocalReport.DataSources.Add(datasourceItem);

                 //For Dynamic Report Header 
                 OtherReportsDB otherReprtDB = new OtherReportsDB();
                 DataTable reportHeader = new DataTable();
                 reportHeader = otherReprtDB.getReportHeader2018();
                 ReportDataSource datareportheaderitem = new ReportDataSource("ReportHeader2018", reportHeader);
                 reportViewer.LocalReport.DataSources.Add(datareportheaderitem);


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
        public ActionResult xrayprocedure()
        {


            var viewModel = new RadiologyReportXrayReportCharges()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };


            return View(viewModel);
        }


        [HttpPost]
        public ActionResult xrayprocedure(RadiologyReportXrayReportCharges param)
        {
            //SP SP_Get_XRAYProcedure  Report_XRAYProcedureList.rpt

            var vm = param;
            if (Request.IsAjaxRequest())
            {

                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                reportData = radiologyDB.getXrayProcedure(param.StartDate, param.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\RadiologyReports\XrayProcedure.rdl";

                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;
                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                

                reportViewer = this.DynamicReportHeader(reportViewer,"DataSet2");
               

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

        public ActionResult anesthesiaschedule()
        {


            var viewModel = new RadiologyReportanesthesiaschedule()
            {

                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult anesthesiaschedule(RadiologyReportanesthesiaschedule param)
        {
            //SP SP_GetAnesthesiaScheduleReport  Report_AnesthesiaSchedule.rpt
            var vm = param;
            if (Request.IsAjaxRequest())
            {

               
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";
                DataTable reportData = new DataTable();


                reportData = radiologyDB.getAnesthesia(param.StartDate, param.EndDate.AddDays(1));
                reportDocPath = @"\Areas\ManagementReports\Reports\RadiologyReports\AnesthesiaScheduleReport.rdl";

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
        public ActionResult proceduredonepatient()
        {


            var viewModel = new RadiologyReportproceduredonepatient()
            {

                //StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                //EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            };


            return View(viewModel);
        }
        


    }
}
