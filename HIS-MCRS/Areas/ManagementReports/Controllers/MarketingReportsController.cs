using HIS_MCRS.Areas.ManagementReports.Models;

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
    public class MarketingReportsController : Controller
    {
        SexDB sexdb = new SexDB();
        MarketingReportDB marketDB = new MarketingReportDB();

        //
        // GET: /ManagementReports/MarketingReports/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AramcoPatient()
        {
            var viewModel = new MRAramcoPatient() {
            Gender = sexdb.getSex()
             };
            
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AramcoPatient(MRAramcoPatient viewmodel)
        {
            viewmodel.Gender = sexdb.getSex();
            var vm = viewmodel;
             if (Request.IsAjaxRequest())
             {

                
                 ReportViewerVm reportVM = new ReportViewerVm();
                 ReportViewer reportViewer = new ReportViewer();
                 string reportDocPath = "";

                 DataTable reportData = new DataTable();
                 reportData = marketDB.getAramcoPatient(viewmodel.From, viewmodel.To, (int)viewmodel.SexID);
                 reportDocPath = @"\Areas\ManagementReports\Reports\MarketingReports\AramcoPatientByAge.rdl";

                 DateTime dateTime = DateTime.Today;

               
                    
                
                 if (reportData.Rows.Count == 0)
                     return Content(Errors.ReportContent("NO RECORDS FOUND"));

                 reportViewer.ProcessingMode = ProcessingMode.Local;

                 reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                 ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                 reportViewer.LocalReport.DataSources.Add(datasourceItem);
                 reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                 reportViewer.LocalReport.SetParameters(new ReportParameter("between", viewmodel.From.ToString()));
                 reportViewer.LocalReport.SetParameters(new ReportParameter("to", viewmodel.To.ToString()));
                 reportViewer.LocalReport.SetParameters(new ReportParameter("category", viewmodel.GenderName));
                  reportViewer.LocalReport.SetParameters(new ReportParameter("printdate", dateTime.ToString("dd/MM/yyyy")));
                  reportViewer.LocalReport.SetParameters(new ReportParameter("printtime", dateTime.ToString("H:i:s")));
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

        public ActionResult NoOfCashVisit()
        {
            var ViewModel = new MRCashPatientVisit()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                NoVisit = 10
            };
            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult NoOfCashVisit(MRCashPatientVisit viewmodel)
        {

            var vm = viewmodel;
            if (Request.IsAjaxRequest())
            {
                //sama
                ReportViewerVm reportVM = new ReportViewerVm();
                ReportViewer reportViewer = new ReportViewer();
                string reportDocPath = "";

                DataTable reportData = new DataTable();
               
                reportData = marketDB.getNoOfCashVisit( vm.StartDate,vm.EndDate.AddDays(1), viewmodel.NoVisit );
                reportDocPath = @"\Areas\ManagementReports\Reports\MarketingReports\CashPatientVisit.rdl";

                DateTime dateTime = DateTime.Today;




                if (reportData.Rows.Count == 0)
                    return Content(Errors.ReportContent("NO RECORDS FOUND"));

                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportDocPath;

                ReportDataSource datasourceItem = new ReportDataSource("DataSet1", reportData);
                reportViewer.LocalReport.DataSources.Add(datasourceItem);
                reportViewer.LocalReport.SetParameters(new ReportParameter("branch", Global.OrganizationDetails.Name + " - " + Global.OrganizationDetails.City.ToUpper()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("stDate", viewmodel.StartDate.ToString("dd/MM/yyyy")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("enDate", viewmodel.EndDate.ToShortDateString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("xNoVisit", viewmodel.NoVisit.ToString()));
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


       
    }
}
